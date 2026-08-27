---
name: contrato-endpoint-api
description: Regras de contrato de um endpoint desta API — casing por canal (corpo/query/form), template de rota, genérico do ActionResult<T>, paginação, exceções tipadas. Use ao adicionar ou alterar qualquer action de controller, ao criar um DTO de request/response ou de filtro, ao mexer em DocsExtensions ou na convenção de rota, e quando um filtro "não filtra", um campo chega default sem erro, ou a rota some do /api/v1. Use também antes de responder o que o doc OpenAPI diz sobre um endpoint.
---

# Contrato de endpoint da gstok-api

Todo desvio listado aqui já aconteceu neste repo e passou despercebido porque **nenhum deles
gera erro**: o request devolve 200, o campo fica no default, a rota responde em outro lugar.
Compilar e "testar no Scalar" não pega nada disso.

## 1. Casing depende do canal — três regras que não se misturam

O C# declara tudo em PascalCase. O que trafega na rede depende de por onde o campo entra:

| Canal | Casing na rede | Quem converte | Onde está a prova |
|---|---|---|---|
| Corpo JSON (`[FromBody]` e respostas) | `snake_case` | `JsonNamingPolicy.SnakeCaseLower` global | `ServiceExtensions.AddApiControllers` |
| **Query string** (`[FromQuery]`) | **PascalCase** | **ninguém** | ver abaixo |
| `multipart/form-data` | aceita os dois | `SnakeCaseFormValueProvider` | `Common/ModelBinding/` |

**Query string não passa por tradutor nenhum.** O `SnakeCaseFormValueProvider` se auto-exclui
de qualquer fonte que não seja form — `Filter()` só devolve o provider quando
`bindingSource == BindingSource.Form` — e a factory sai cedo com `if (!request.HasFormContentType) return;`.
O binder padrão de query é case-insensitive mas não entende underscore.

Consequência, medida em runtime:

```
GET /api/v1/store/produto?page_size=1  →  "page_size":10   ← ignorado, HTTP 200, sem erro
GET /api/v1/store/produto?PageSize=1   →  "page_size":1    ← aplicado
```

Ao criar um DTO de filtro ou qualquer parâmetro `[FromQuery]`, **documente isso no XML doc do
DTO**. `PaginationParams`, `ClienteFiltroDto`, `ProdutoFiltroDto` e `StoreProdutoFiltroDto` já
trazem o aviso — copie o texto de lá.

### O doc OpenAPI já mentiu sobre isso

`DocsExtensions` tinha um `AddOperationTransformer` que renomeava query params para snake_case,
"alinhando" o doc a um provider que não atua ali. Resultado: 14 endpoints anunciavam
`page_size`, `nm_produto`, `cd_inscricao_nacional`. Quem seguisse o doc tinha o filtro
ignorado em silêncio.

O bloco foi removido e **não deve voltar**. O transformer continua renomeando
`operation.RequestBody` — isso está certo, porque corpo e form-data realmente aceitam
snake_case. Se for mexer em `DocsExtensions`, a regra é: **corpo sim, query nunca.**

Para o front, a contraparte é a skill `sincronizar-dto` em `../gstok-web/.claude/skills/`.

## 2. Rota: nunca inicie um template com `/`

`[Route("/algo")]` numa action é template **absoluto**: descarta a rota do controller e, com
ela, o prefixo `api/v1` aplicado por `RotaPrefixoConvencao`.

Aconteceu com `ColecaoController.ObterPorFornecedor`, que ficou em `/fornecedor/{id}/colecao`
— 1 rota de 65 fora da versão. O gstok-web reagiu com um workaround permanente em
`ColecaoService.ts`, reconstruindo `new URL(VITE_API_URL).origin` para furar o `baseURL`.

Hoje a convenção reaplica o prefixo em templates absolutos, então uma barra inicial não quebra
mais a versão. **Escreva sem ela mesmo assim** — o mecanismo existe como rede de segurança,
não como permissão.

Regras de rota:

- `[Route("<dominio>")]` no controller, kebab-case (`tipo-produto`), sem `api/`, sem `[controller]`.
- Loja online: `[Route("store/<dominio>")]`.
- Rota aninhada em outro recurso: `[HttpGet("/fornecedor/{fornecedorId:guid}/colecao")]` funciona,
  mas prefira manter o endpoint sob o próprio controller quando não houver ganho real de semântica.

**Verificação:** com a API no ar, toda rota em `/openapi/v1.json` deve começar com `/api/v1/`.

```bash
curl -s http://localhost:5268/openapi/v1.json | grep -o '"/[^"]*"' | grep -v '^"/api/v1/'
```

## 3. `ActionResult<T>` — e o `T` tem que ser o que sai

Action com corpo declara `ActionResult<T>`, nunca `IActionResult` puro: o gerador infere o
schema do tipo de retorno, e `IActionResult` não tem genérico. Action sem corpo
(`NoContent()`/`NotFound()`, tipicamente `DELETE`, `Sair`, `Limpar`) pode manter `IActionResult`.

**Cumprir a regra com o genérico errado é pior que não cumprir**, porque o doc passa a afirmar
algo falso com aparência de verificado. `VendaController.ObterItens` declarava
`ActionResult<VendaResponseDto>` e retornava `venda.Itens` — o doc anunciava um objeto onde a
API devolve um array de `ItemVendaResponseDto`.

Antes de fechar a action, confira: o `T` declarado é exatamente o que cada `return Ok(...)`
entrega? Coleção devolve `ActionResult<List<TItem>>`, não o DTO do agregado.

## 4. Paginação

Não escreva `CountAsync` + `Skip/Take` à mão. Use `app/Common/Extensions/PaginacaoExtensions.cs`:

```csharp
// Repository — ordene ANTES de paginar.
return await query
    .OrderBy(c => c.Pessoa.NmPessoa)
    .ThenBy(c => c.Pessoa.NmSobrenome)
    .ParaPaginaAsync(pagination);

// Service — converte o envelope preservando os metadados.
return result.Mapear(ClienteAdminMapper.ParaResposta);
```

Sem `OrderBy` o Postgres não garante ordem estável entre páginas: o mesmo registro pode
aparecer duas vezes ou sumir. `PageSize` acima de 100 é silenciosamente reduzido a 100.

## 5. Erros: exceções tipadas, nunca `BadRequest` na mão

`MiddlewareExcecao` traduz `ExcecaoBase` para `ErrorResponseDto` (`severidade` + `mensagem`),
loga como warning abaixo de 500 e como error acima.

| Exceção | HTTP | Severidade | Quando |
|---|---|---|---|
| `ExcecaoNegocio` | 422 | Alerta | Regra de negócio violada |
| `ConflitoException` | 409 | Alerta | Unicidade (e-mail, CPF, SKU) ou estado que impede a ação |
| `NaoEncontradoException` | 404 | Erro | Recurso inexistente |

`null` vindo do service para "não encontrei" também é padrão aceito — o controller converte em
`NotFound()`. Escolha um dos dois por endpoint e seja consistente dentro do domínio.

Validação de entrada é por Data Annotations no DTO (`[Required]`, `[MaxLength]`, `[EmailAddress]`,
`[Phone]`, e os customizados `[Cnpj]` / `[InscricaoNacional]` em `Common/Validators/`). O
`InvalidModelStateResponseFactory` já devolve `ErrorResponseDto` com severidade Alerta.

## 6. Outras regras de serialização

- **Nunca use `[JsonPropertyName]`** — a política global cobre tudo.
- **Enums são string** (`JsonStringEnumConverter`), nunca inteiro. Enum persistido no banco
  precisa de `.HasConversion<string>()` no `AppDbContext`.
- `DateOnly` sai como `"YYYY-MM-DD"`; `Guid` sai como string.
- Ids novos: `Guid.CreateVersion7()`.

## 7. Checklist antes de fechar

1. Rota sem barra inicial; kebab-case; sem `api/`.
2. `ActionResult<T>` com o `T` que realmente sai — coleção é `List<...>`.
3. DTO de filtro/query com o aviso de PascalCase no XML doc.
4. Corpo em snake_case por herança da política — sem `[JsonPropertyName]`.
5. Paginação via `ParaPaginaAsync` / `Mapear`, com `OrderBy` antes.
6. Erros por exceção tipada.
7. Service e repository registrados em `ServiceExtensions.AddApplicationServices`.
8. `dotnet build` (com a API parada, ou `-p:BaseOutputPath=<temp>/bin/`).
9. Com a API reiniciada, conferir no `/openapi/v1.json`: a rota está sob `/api/v1/`, os query
   params saem em PascalCase e o schema de resposta é o tipo certo.

Ao terminar, diga qual verificação do passo 9 você realmente rodou. Se não reiniciou a API,
diga isso em vez de afirmar que o doc está correto — o doc só reflete o código depois do
restart.

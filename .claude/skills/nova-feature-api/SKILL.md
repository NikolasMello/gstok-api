---
name: nova-feature-api
description: Cria ou completa uma fatia vertical em app/Features/ (Controller + Service + Repository + Interfaces + DTOs + Mapper + registro no DI), seguindo o scaffold já usado em cliente, fornecedor, produto e compra. Use quando o pedido for "criar o CRUD de X", "adicionar o endpoint de X", "fazer a feature de devoluções", "preciso de um controller para X" ou qualquer variação de criar/estender um domínio da API. Use também ao acrescentar só parte da fatia (só a listagem, só o POST) para não divergir do padrão.
---

# Nova feature na gstok-api

O projeto tem ~22 fatias verticais com estrutura idêntica. **Siga o scaffold abaixo em vez de
improvisar.** Referências mais fiéis: `Cliente` (CRUD sobre entidade compartilhada com a loja,
com filtro e DTO de detalhe separado) e `Fornecedor` (CRUD simples).

## Passo 0 — antes de escrever arquivo

1. **Invoque a skill `contrato-endpoint-api`.** Ela tem as regras de casing, rota,
   `ActionResult<T>`, paginação e erros — todas com desvios que já passaram despercebidos aqui.
2. Confirme se o domínio já existe. `app/Features/` tem fatias que a UI ainda não consome, e
   `Store/<Dominio>` é uma fatia **separada** da administrativa de mesmo nome (`Cliente` ×
   `Store/Cliente` compartilham a tabela e têm regras diferentes).
3. Confirme a entidade real em `app/Domain/Models/`. Nome de tela nem sempre é nome de tabela:
   `Cliente` tem tabela própria com `pessoa_id`, e `Usuario` também aponta para `Pessoa`.

## Passo 1 — a fatia

```
app/Features/<Dominio>/
├── <Dominio>Controller.cs
├── <Dominio>Service.cs
├── <Dominio>Repository.cs
└── Interfaces/
    ├── I<Dominio>Service.cs
    └── I<Dominio>Repository.cs
```

**Todos os cinco arquivos usam `namespace gstok_api.Features.<Dominio>;`** — inclusive as
interfaces dentro de `Interfaces/`, que não ganham sub-namespace. Loja online:
`app/Features/Store/<Dominio>/` com `namespace gstok_api.Features.Store.<Dominio>;` e prefixo
`Store` no nome das classes.

Nome de arquivo = nome da classe, sempre.

### Controller

Fino: sem `AppDbContext`, sem regra de negócio, sem try/catch.

```csharp
using Microsoft.AspNetCore.Mvc;
using gstok_api.DTOs;
using gstok_api.DTOs.Cliente;

namespace gstok_api.Features.Cliente;

[ApiController]
[Route("cliente")]
public class ClienteController(IClienteService clienteService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ClienteResponseDto>>> ObterTodos(
        [FromQuery] PaginationParams pagination,
        [FromQuery] ClienteFiltroDto filtro) =>
        Ok(await clienteService.ObterTodosAsync(pagination, filtro));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClienteDetalheResponseDto>> ObterPorId(Guid id)
    {
        var cliente = await clienteService.ObterPorIdAsync(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteResponseDto>> Criar([FromBody] ClienteRequestDto dto)
    {
        var cliente = await clienteService.CriarAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = cliente.IdCliente }, cliente);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ClienteResponseDto>> Atualizar(Guid id, [FromBody] ClienteRequestDto dto)
    {
        var cliente = await clienteService.AtualizarAsync(id, dto);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var deleted = await clienteService.ExcluirAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
```

Nomes das actions em português: `ObterTodos`, `ObterPorId`, `Criar`, `Atualizar`, `Excluir`.

### Service

Regra de negócio, normalização e as exceções tipadas. Monta a entidade — o repository não
conhece DTO.

```csharp
public async Task<PagedResult<ClienteResponseDto>> ObterTodosAsync(
    PaginationParams pagination, ClienteFiltroDto filtro)
{
    var result = await clienteRepository.ObterTodosAsync(pagination, filtro);
    return result.Mapear(ClienteAdminMapper.ParaResposta);
}

public async Task<ClienteResponseDto> CriarAsync(ClienteRequestDto dto)
{
    if (await clienteRepository.InscricaoNacionalExisteAsync(dto.CdInscricaoNacional))
        throw new ConflitoException("CPF/CNPJ já cadastrado.");
    // ...
}
```

Normalize aqui, não no controller: `TextoUtils.CapitalizarNomeProprio(...)` para nomes,
`.ToLowerInvariant()` para e-mail, `.ToUpperInvariant()` para UF. Ids: `Guid.CreateVersion7()`.
`TsCriacao`/`TsEdicao`: `DateTime.UtcNow`.

### Repository

Só EF Core. Paginação sempre pelas extensões, com `OrderBy` antes:

```csharp
return await query
    .OrderBy(c => c.Pessoa.NmPessoa)
    .ParaPaginaAsync(pagination);
```

Busca textual case-insensitive: `EF.Functions.ILike(campo, $"%{termo}%")` — é Postgres, não use
`.ToLower().Contains()`.

**Antes de escrever `ExcluirAsync`, leia o `OnDelete` da entidade no `AppDbContext`.** É o que
decide se a exclusão é possível e o que vai junto:

- `Restrict` num relacionamento (ex.: `Venda → Cliente`) significa que a exclusão falha no banco.
  Cheque antes no service e lance `ConflitoException` com mensagem útil.
- `Cascade` leva dependentes junto — confirme que é o desejado.
- `SetNull` apaga o vínculo em silêncio. Em `Cliente`, a `Pessoa` só é removida quando nenhum
  `Usuario` a referencia, justamente por isso.

## Passo 2 — DTOs

`app/DTOs/<Dominio>/`, namespace `gstok_api.DTOs.<Dominio>`.

- `<Dominio>RequestDto` quando criar e atualizar têm a mesma forma (padrão de `Cliente`,
  `Pessoa`, `Endereco`); `<Dominio>CreateDto` + `<Dominio>UpdateDto` quando divergem
  (padrão de `Fornecedor`, `Produto`).
- `<Dominio>ResponseDto` para o item de listagem e `<Dominio>DetalheResponseDto` quando o
  detalhe é mais rico. **Não reaproveite um no lugar do outro** — o front tipa os dois
  separadamente e a divergência só aparece em runtime.
- `<Dominio>FiltroDto` para `[FromQuery]`, **com o aviso de PascalCase no XML doc**. Copie o
  texto de `ClienteFiltroDto`.

Validação por Data Annotations no DTO. Nunca `[JsonPropertyName]`.

## Passo 3 — Mapper

`app/Mappings/<Dominio>/<Dominio>Mapper.cs`, `public static class`, métodos `ParaResposta`,
`ParaDetalhe`, `ParaResumo`. Não há AutoMapper no projeto.

Se o domínio existir nos dois lados, desambigue no nome: `ClienteAdminMapper` (admin) ×
`StoreClienteMapper` (loja).

## Passo 4 — DI

`ServiceExtensions.AddApplicationServices`, os dois, scoped:

```csharp
services.AddScoped<IClienteRepository, ClienteRepository>();
services.AddScoped<IClienteService, ClienteService>();
```

Esquecer aqui compila e só falha na primeira requisição.

## Passo 5 — migration, se mexeu em entidade

Invoque a skill `migracao-ef`.

## Passo 6 — verificar

1. `dotnet build` — com a API parada, ou `-p:BaseOutputPath=<temp>/bin/` (o exe fica travado
   enquanto ela roda).
2. Reinicie a API e confira em `/openapi/v1.json`: rota sob `/api/v1/`, query params em
   PascalCase, schema de resposta com o tipo certo.
3. Se o contrato mudou, avise que o `../gstok-web/src/service/<dominio>/` precisa acompanhar —
   lá existe a skill `sincronizar-dto` para isso.

Ao final, liste os arquivos criados/alterados e diga o que foi de fato verificado. Se o passo 6
não rodou, diga isso em vez de afirmar que está funcionando.

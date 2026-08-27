# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Run the API (HTTP on :5268, HTTPS on :7276)
dotnet run

# Build
dotnet build

# Run tests (no test project exists yet — `dotnet test` finds nothing)
dotnet test

# EF Core migrations
dotnet ef migrations add <MigrationName>
dotnet ef database update
dotnet ef migrations remove
```

`dotnet build` fails with MSB3027 (file locked) while the API is running — the exe can't be
overwritten. Either stop the running instance or build elsewhere:
`dotnet build -p:BaseOutputPath=<temp>/bin/`.

The `.http` file at the root (`gstok-api.http`) is essentially empty; it is not a source of
truth for the available endpoints. With the API running, `http://localhost:5268/openapi/v1.json`
lists the routes actually mapped, and Scalar renders them at `/scalar`.

## Architecture

.NET 10 ASP.NET Core REST API organized in **vertical slices**, not in layers by type. The
data flow inside a slice is:

**Request → Controller → Service → Repository → DbContext (PostgreSQL)**

Everything lives under `app/`. There are no `Controllers/`, `Services/` or `Repositories/`
folders at the root — a feature's three classes sit together:

```
app/Features/<Dominio>/
├── <Dominio>Controller.cs      namespace gstok_api.Features.<Dominio>
├── <Dominio>Service.cs         idem
├── <Dominio>Repository.cs      idem
└── Interfaces/
    ├── I<Dominio>Service.cs    idem — a interface fica na mesma namespace da implementação
    └── I<Dominio>Repository.cs
```

| Folder | Role |
|---|---|
| `app/Features/<Dominio>/` | The slice: Controller + Service + Repository + `Interfaces/` |
| `app/Features/Store/<Dominio>/` | Loja online (cliente final), prefixo `Store` na classe e na rota |
| `app/DTOs/<Dominio>/` | Request/response shapes, namespace `gstok_api.DTOs.<Dominio>` |
| `app/DTOs/Common/` | `PagedResult`, `PaginationParams`, `ErrorResponseDto` — namespace raiz `gstok_api.DTOs` (exceção deliberada: são compartilhados por toda a API) |
| `app/Domain/Models/` | EF Core entities, namespace `gstok_api.Models` |
| `app/Domain/Enums/` | Enums de domínio, namespace `gstok_api.Enums` |
| `app/Mappings/<Dominio>/` | Static hand-written mappers (Model ↔ DTO). **Não há AutoMapper no projeto** |
| `app/Infrastructure/Database/` | `AppDbContext`, namespace `gstok_api.Database` |
| `app/Infrastructure/Migrations/` | Auto-generated EF Core migrations |
| `app/Common/Auth/`, `Utils/`, `Validators/`, `ModelBinding/`, `Services/`, `Extensions/` | Cross-cutting: atributos de autorização, helpers, validadores (`[Cnpj]`, `[InscricaoNacional]`), value providers, processamento de imagem, paginação |
| `app/Middleware/` | Pipeline: `MiddlewareExcecao`, `MiddlewareSessao`, `MiddlewareSessaoCliente` |
| `app/Exceptions/` | `ExcecaoBase` e derivadas (ver abaixo) |
| `app/Extensions/ServiceExtensions.cs` | Registro de DI, JSON, CORS, rate limiting, convenção de rota |
| `app/Docs/DocsExtensions.cs` | OpenAPI + Scalar, **somente em Development** |

Root namespace is `gstok_api`. **Namespace segue a pasta**, com as exceções tabeladas acima
(`Domain/Models` → `gstok_api.Models`, `Infrastructure/Database` → `gstok_api.Database`,
`DTOs/Common` → `gstok_api.DTOs`).

Nome de arquivo = nome da classe. Se divergirem, o arquivo está errado.

## Key dependencies

- **Npgsql.EntityFrameworkCore.PostgreSQL** — database provider (único em uso)
- **Microsoft.AspNetCore.OpenApi** + **Scalar.AspNetCore** — doc em `/openapi/v1.json` e `/scalar`, dev only
- **BCrypt.Net-Next** — hash de senha (`workFactor: 12`)
- **SixLabors.ImageSharp** — variantes de imagem de produto
- **Serilog** — logging em arquivo (`logs/`)
- `Microsoft.EntityFrameworkCore.InMemory` está no `.csproj` mas **não é usado** — não há
  wiring para ele em `ServiceExtensions.AddDatabase`, que sempre chama `UseNpgsql`.

## Database

Connection string em `appsettings.Development.json` sob `ConnectionStrings:DefaultConnection`.
O registro fica em `ServiceExtensions.AddDatabase`, **não em `Program.cs`** — `Program.cs` só
encadeia os métodos de extensão.

## Conventions

- Registre service e repository em `ServiceExtensions.AddApplicationServices` (scoped).
  Esquecer disso só falha em runtime, na primeira requisição.
- Controller só chama service — nunca `AppDbContext` direto.
- Retorne DTOs, nunca entidades EF.
- `[ApiController]` + `[Route("<dominio>")]` em kebab-case (`tipo-produto`), **sem** `api/` e
  **sem** `[controller]`: a convenção `RotaPrefixoConvencao` já prefixa `api/v1` em tudo.

### Rotas: nunca inicie um template com `/`

`[Route("/algo")]` numa action é template **absoluto** — descarta a rota do controller e o
prefixo `api/v1` junto. O endpoint sai fora da versão sem erro nenhum e só se descobre
chamando. Já aconteceu: `ColecaoController.ObterPorFornecedor` ficou em
`/fornecedor/{id}/colecao` e obrigou o gstok-web a furar o `baseURL` com um workaround.

Hoje `RotaPrefixoConvencao` reaplica o prefixo em templates absolutos, então a rota aninhada
funciona — mas **escreva sem a barra inicial mesmo assim**. Para conferir: com a API no ar,
toda rota em `/openapi/v1.json` deve começar com `/api/v1/`.

### ActionResult\<T\> com o genérico certo

Toda action que retorna corpo declara `ActionResult<T>` (ex.: `Task<ActionResult<ProdutoResponseDto>>`),
nunca `IActionResult` puro — o gerador infere o schema do tipo de retorno. Actions sem corpo
(`NoContent()`/`NotFound()`, tipicamente `DELETE`) podem manter `IActionResult`.

**E o `T` tem que ser o que realmente sai.** Cumprir a regra na letra com o genérico errado
documenta o schema errado, que é pior que não documentar: `VendaController.ObterItens`
declarava `ActionResult<VendaResponseDto>` e retornava `venda.Itens` — o doc anunciava um
objeto onde a API devolve um array.

### Casing por canal — três regras que não se misturam

O C# declara tudo em PascalCase, mas o que trafega depende de por onde o campo entra:

| Canal | Casing na rede | Quem converte |
|---|---|---|
| Corpo JSON (`[FromBody]`, respostas) | `snake_case` | `JsonNamingPolicy.SnakeCaseLower` global |
| **Query string** (`[FromQuery]`) | **PascalCase** | **ninguém — não há tradutor** |
| `multipart/form-data` | aceita os dois | `SnakeCaseFormValueProvider` |

O `SnakeCaseFormValueProvider` só atua sobre `BindingSource.Form`, e a factory sai cedo quando
não há form content-type. Query string, portanto, exige o nome da propriedade C#:
`?PageSize=20&NmProduto=camisa`.

Mandar `?page_size=1` devolve **HTTP 200 com o valor default** — filtro ignorado em silêncio,
sem erro de validação. É o desvio mais fácil de cometer nesta API. O `DocsExtensions` já
renomeou query params para snake_case no doc e fez 14 endpoints mentirem; o bloco foi removido
e **não deve voltar**.

- **Nunca use `[JsonPropertyName]`** — a política global cobre tudo.
- **Enums serializam como string** via `JsonStringEnumConverter` global, nunca como inteiro.
- Ver [README.md](README.md) para a convenção completa.

### Paginação

Não escreva o bloco de `CountAsync` + `Skip/Take` à mão — use `app/Common/Extensions/PaginacaoExtensions.cs`:

```csharp
// Repository — ordene ANTES; sem OrderBy o Postgres não garante ordem estável entre páginas.
return await query.OrderBy(c => c.Pessoa.NmPessoa).ParaPaginaAsync(pagination);

// Service — converte o envelope preservando os metadados.
return result.Mapear(ClienteAdminMapper.ParaResposta);
```

### Erros

Lance as exceções tipadas de `app/Exceptions/`; `MiddlewareExcecao` traduz para o
`ErrorResponseDto` (`severidade` + `mensagem`). Não retorne `BadRequest(...)` manualmente.

| Exceção | HTTP | Severidade | Quando |
|---|---|---|---|
| `ExcecaoNegocio` | 422 | Alerta | Regra de negócio violada |
| `ConflitoException` | 409 | Alerta | Unicidade (e-mail, CPF, SKU) ou estado que impede a ação |
| `NaoEncontradoException` | 404 | Erro | Recurso inexistente |

### Entity property naming

All entity properties use a 2-letter semantic prefix (PascalCase). See [README.md](README.md)
for the full prefix table. Key prefixes:

| Prefix | Meaning | C# type |
|--------|---------|---------|
| `Nm` | Name | `string` |
| `Vl` | Monetary value | `decimal` |
| `Qt` | Quantity | `decimal`/`int` |
| `Pc` | Percentage/rate | `decimal` |
| `Dt` | Date | `DateOnly` |
| `Ts` | Timestamp (UTC) | `DateTime` |
| `St` | Status | `string`/`enum` |
| `Tp` | Type/category | `string`/`enum` |
| `Fl`/`In` | Boolean flag | `bool` |
| `Cd` | Business code | `string` |
| `Nr` | Numeric measurement (dimensions, counts) | `int` |
| `Sq` | Sequence/ordering position | `int` |

Primary keys use `Id<EntityName>` (e.g. `IdProduto`, `IdColecao`). Foreign keys use
`<EntityName>Id`. Navigation properties have no prefix. Ids novos são gerados com
`Guid.CreateVersion7()`.

## Skills

`.claude/skills/` neste repo:

- **`nova-feature-api`** — criar ou completar uma fatia em `app/Features/`. Invoque antes de
  escrever o primeiro arquivo de um domínio novo.
- **`contrato-endpoint-api`** — regras de contrato de endpoint: casing por canal, rota,
  `ActionResult<T>`, paginação, erros. Invoque ao adicionar ou alterar qualquer action.

## Related project

O frontend é o `gstok-web` (Vite + TypeScript + Material UI) em `../gstok-web`, e tem as
skills `sincronizar-dto`, `nova-entidade-crud` e `padrao-componente`. Mudança de contrato
aqui quebra lá em silêncio: DTOs de listagem e de detalhe frequentemente diferem, e o front
depende do casing por canal descrito acima. Ao alterar um contrato, verifique
`../gstok-web/src/service/<dominio>/`.

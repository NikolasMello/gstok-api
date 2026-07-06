# gstok-api

API REST em .NET 10 / ASP.NET Core para o sistema GSTOK. Segue arquitetura em camadas com separação por feature.

---

## Arquitetura

```
Request → Controller → Service → Repository → DbContext (PostgreSQL)
```

| Camada | Responsabilidade |
|---|---|
| `Features/*/Controller` | Recebe a requisição, delega ao service, retorna a resposta |
| `Features/*/Service` | Contém a lógica de negócio |
| `Features/*/Repository` | Abstração de acesso ao banco via EF Core |
| `Domain/Models` | Entidades mapeadas pelo EF Core |
| `DTOs/` | Contratos de entrada e saída da API |
| `Exceptions/` | Hierarquia de exceções tipadas de domínio |
| `Middleware/` | Pipeline transversal (ex: tratamento de erros) |
| `Common/` | Utilitários compartilhados entre features |

---

## Contratos da API

### Serialização JSON

Todas as chaves de entrada e saída usam **`snake_case`** automaticamente. A conversão é global e transparente — o C# usa PascalCase com prefixo semântico, o JSON usa snake_case.

```
C# (interno)   →   JSON (contrato externo)
─────────────────────────────────────────
NmEmail        →   nm_email
DsSenha        →   ds_senha
VlTotal        →   vl_total
TsCriacao      →   ts_criacao
StPedido       →   st_pedido
```

A regra se aplica tanto ao body de entrada (`[FromBody]`) quanto às respostas. **Nunca adicione `[JsonPropertyName]`** nos DTOs — a política global cobre todos os casos.

### Enums

Enums sempre trafegam como **string** em ambas as direções:

```json
{ "st_pedido": "Confirmado", "tp_pagamento": "Pix" }
```

Nunca como inteiro. Isso vale para requests e responses.

### Respostas de erro

Todos os erros — de validação, regra de negócio ou falha interna — retornam o mesmo shape:

```json
{
  "severidade": "Alerta" | "Erro",
  "mensagem": "Descrição do problema."
}
```

| `severidade` | HTTP Status | Quando ocorre |
|---|---|---|
| `Alerta` | `400` | Falha de validação de campos |
| `Alerta` | `409` | Conflito de regra de negócio (ex: e-mail duplicado) |
| `Alerta` | `422` | Regra de domínio violada (ex: documento inválido) |
| `Erro` | `404` | Recurso não encontrado |
| `Erro` | `500` | Exceção não tratada |

`Alerta` indica que o usuário pode corrigir a ação. `Erro` indica falha inesperada que requer atenção técnica.

---

## Autenticação

A autenticação usa **JWT + refresh token via cookie HttpOnly**.

- `POST /api/v1/auth/login` — retorna `{ access_token, expires_in }` no body. O refresh token é setado automaticamente como cookie `HttpOnly; Secure; SameSite`.
- `POST /api/v1/auth/refresh` — lê o cookie, rotaciona o token e devolve um novo `access_token`. O cookie é atualizado automaticamente.
- `POST /api/v1/auth/logout` — invalida a sessão no banco e expira o cookie.

O `access_token` deve ser enviado pelo cliente no header de todas as rotas protegidas:

```
Authorization: Bearer <access_token>
```

Todas as rotas fora de `/auth` exigem autenticação.

---

## Convenção de Prefixos

Todas as propriedades de entidades e DTOs usam um prefixo de 2 letras que identifica o tipo semântico do dado.

**Formato:** `<Prefixo><Descritor>` — ex.: `VlPreco`, `NmCliente`, `TsCriacao`

| Prefixo | Categoria | Tipo C# | JSON resultante |
|---------|-----------|---------|-----------------|
| `Nm` | Nome | `string` | `nm_produto` |
| `Ds` | Descrição / senha hash | `string` | `ds_observacao` |
| `Cd` | Código de negócio | `string` | `cd_inscricao_nacional` |
| `Ur` | URL / endereço web | `string` | `ur_imagem` |
| `Nr` | Medida numérica (dimensões, contagens) | `int` | `nr_largura` |
| `Sq` | Sequência / posição de ordenação | `int` | `sq_ordem` |
| `Vl` | Valor monetário | `decimal` | `vl_total` |
| `Qt` | Quantidade | `decimal` / `int` | `qt_estoque` |
| `Pc` | Percentual / taxa | `decimal` | `pc_desconto` |
| `Dt` | Data (sem hora) | `DateOnly` | `dt_nascimento` |
| `Ts` | Timestamp UTC (data + hora) | `DateTime` | `ts_criacao` |
| `St` | Status | `enum` | `st_pedido` |
| `Tp` | Tipo / categoria | `enum` | `tp_pagamento` |
| `Fl` / `In` | Flag / indicador booleano | `bool` | `fl_ativo` |

### Regras de nomenclatura

- **Chave primária:** `Id<Entidade>` — ex.: `IdProduto`, `IdCliente`
- **Chave estrangeira:** `<Entidade>Id` — ex.: `ProdutoId`, `ClienteId` (sem prefixo semântico)
- **Propriedades de navegação EF Core:** sem prefixo — ex.: `Produto`, `ICollection<ItemPedido>`
- **Timestamps de auditoria:** `TsCriacao` (`DateTime`, UTC, obrigatório) e `TsEdicao` (`DateTime?`, UTC, nullable)

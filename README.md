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
| `Mappings/` | Classes estáticas de mapeamento Model → DTO (agrupadas por model) |
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
StVenda       →   st_venda
```

A regra se aplica tanto ao body de entrada (`[FromBody]`) quanto às respostas. **Nunca adicione `[JsonPropertyName]`** nos DTOs — a política global cobre todos os casos.

### Enums

Enums sempre trafegam como **string** em ambas as direções:

```json
{ "st_venda": "Confirmado", "tp_pagamento": "Pix" }
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

A autenticação é baseada em **sessão server-side com cookie HttpOnly**. Não há JWT nem token no cliente — o identificador de sessão trafega exclusivamente via cookie gerenciado pelo browser.

### Endpoints

| Método | Rota | Acesso |
|---|---|---|
| `POST` | `/auth/register` | Público |
| `POST` | `/auth/login` | Público |
| `POST` | `/auth/logout` | Autenticado |

Todas as demais rotas exigem sessão ativa.

### Fluxo de login

```
Cliente                          Servidor
  │                                  │
  │── POST /auth/login ─────────────►│
  │   { nm_email, ds_senha }         │  1. Verifica credenciais
  │                                  │  2. Gera token criptográfico (256 bits)
  │                                  │  3. Persiste sessão no banco (cd_token, ts_expiracao)
  │◄── 200 OK ───────────────────────│
  │    Set-Cookie: sid=<token>        │  4. Cookie HttpOnly setado automaticamente
  │    { nm_email, nm_pessoa,         │
  │      nm_sobrenome, ur_avatar }    │  5. Body retorna apenas dados de exibição
```

O token de sessão **nunca é exposto no body**. O browser gerencia o cookie e o reenvia automaticamente em todas as requisições subsequentes.

### Validação de sessão por requisição

Cada request para uma rota protegida passa pelo `MiddlewareSessao`:

```
Request chega
  │
  ├─ Endpoint tem [AllowAnonymous]? → passa direto
  │
  ├─ Cookie sid ausente? → 401
  │
  ├─ Token no IMemoryCache? → extrai UsuarioId → continua
  │
  └─ Cache miss → consulta banco
       ├─ Sessão não encontrada ou expirada? → 401
       └─ Sessão válida → armazena no cache → continua
```

O `IMemoryCache` evita consulta ao banco em requests consecutivos. O cache é invalidado imediatamente no logout.

### Cookie de sessão

| Atributo | Valor | Finalidade |
|---|---|---|
| Nome | `sid` | Identificador da sessão |
| `HttpOnly` | `true` | Inacessível via JavaScript — previne XSS |
| `Secure` | `true` em prod | Transmitido apenas via HTTPS |
| `SameSite` | `None` em prod | Permite requisições cross-origin (frontend separado) |
| `Path` | `/` | Enviado em todas as rotas |
| Expiração | 7 dias | Absoluta a partir do login |

Em desenvolvimento, `Secure` e `SameSite` podem ser ajustados em `appsettings.Development.json`.

### Logout

```
Cliente                          Servidor
  │                                  │
  │── POST /auth/logout ────────────►│  1. Lê cookie sid
  │                                  │  2. Remove sessão do banco
  │                                  │  3. Remove do IMemoryCache
  │◄── 204 No Content ───────────────│
  │    Set-Cookie: sid=; Expires=... │  4. Cookie expirado pelo servidor
```

### Response de login

```json
{
  "nm_email": "joao@email.com",
  "nm_pessoa": "João",
  "nm_sobrenome": "Silva",
  "ur_avatar": "https://..."
}
```

`nm_pessoa` sempre está preenchido — é definido no momento do registro. `nm_sobrenome` e `ur_avatar` são `null` até o usuário vincular seus dados pessoais (`Pessoa`). O frontend deve tratar esses dois campos como opcionais.

### Verificação de sessão no frontend

Como o cookie é HttpOnly, o JavaScript não consegue lê-lo diretamente. O padrão recomendado:

1. **Após login:** armazena o response (`nm_email`, `nm_pessoa` etc.) em estado global persistido no `localStorage`
2. **Ao carregar o app:** se há dados no `localStorage`, valida com `GET /usuario/sessao`
   - `200` → sessão ativa, libera rotas privadas
   - `401` → sessão expirada, limpa `localStorage` e redireciona para login
3. **Durante o uso:** qualquer `401` de qualquer endpoint deve limpar o estado e redirecionar para login (interceptor global)

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
| `St` | Status | `enum` | `st_venda` |
| `Tp` | Tipo / categoria | `enum` | `tp_pagamento` |
| `Fl` / `In` | Flag / indicador booleano | `bool` | `fl_ativo` |

### Regras de nomenclatura

- **Chave primária:** `Id<Entidade>` — ex.: `IdProduto`, `IdCliente`
- **Chave estrangeira:** `<Entidade>Id` — ex.: `ProdutoId`, `ClienteId` (sem prefixo semântico)
- **Propriedades de navegação EF Core:** sem prefixo — ex.: `Produto`, `ICollection<ItemVenda>`
- **Timestamps de auditoria:** `TsCriacao` (`DateTime`, UTC, obrigatório) e `TsEdicao` (`DateTime?`, UTC, nullable)

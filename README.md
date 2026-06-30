# gstok-api

API REST em ASP.NET Core 10 para o sistema GSTOK.

---

## EF Core — Migrations e Banco de Dados

### Aplicar migrations pendentes ao banco

```bash
dotnet ef database update
```

### Criar uma nova migration

```bash
dotnet ef migrations add <NomeDaMigration>
```

> Use nomes descritivos no imperativo: `AddSessaoTable`, `RenameUsuarioEmail`, `AddProdutoIndex`.

### Remover a última migration (não aplicada ao banco)

```bash
dotnet ef migrations remove
```

### Reverter o banco para uma migration específica

```bash
dotnet ef database update <NomeDaMigration>
```

### Reverter todas as migrations (banco vazio)

```bash
dotnet ef database update 0
```

### Listar migrations e seus status

```bash
dotnet ef migrations list
```

### Gerar SQL sem executar (dry-run)

```bash
dotnet ef migrations script
```

---

## Convenção de Prefixos de Propriedades em Entidades

Todas as propriedades de entidades (Models) seguem um prefixo de 2 letras que identifica semanticamente o tipo do dado. Em C#, o prefixo é parte do nome PascalCase da propriedade.

**Formato:** `<Prefixo><Descritor>` → ex.: `VlPreco`, `NmCliente`, `DtCadastro`

### Tabela de Prefixos

| Prefixo | Categoria | Tipo C# típico | Exemplo |
|---------|-----------|----------------|---------|
| `Nm` | Nome | `string` | `NmCliente`, `NmProduto` |
| `Ds` | Descrição | `string` | `DsProduto`, `DsObservacao` |
| `Tx` | Texto longo | `string` | `TxJustificativa`, `TxNota` |
| `Sg` | Sigla / abreviação | `string` | `SgEstado`, `SgUnidade` |
| `Cd` | Código de negócio | `string` | `CdTicker`, `CdCarteira` |
| `Nr` | Número não-monetário | `int` / `long` | `NrOrdem`, `NrParcela` |
| `Sq` | Sequência / ordenação | `int` | `SqItem`, `SqEtapa` |
| `Vl` | Valor monetário | `decimal` | `VlPreco`, `VlTotal`, `VlCusto` |
| `Qt` | Quantidade | `decimal` / `int` | `QtAcoes`, `QtEstoque` |
| `Pc` | Percentual / taxa | `decimal` | `PcDesconto`, `PcJuros`, `PcRentabilidade` |
| `Dt` | Data (sem hora) | `DateOnly` | `DtCompra`, `DtVencimento` |
| `Hr` | Hora | `TimeOnly` | `HrAbertura`, `HrFechamento` |
| `Ts` | Timestamp (data + hora) | `DateTime` | `TsCriacao`, `TsAtualizacao` |
| `St` | Status | `string` / `enum` | `StPedido`, `StConta` |
| `Tp` | Tipo / categoria | `string` / `enum` | `TpOperacao`, `TpAtivo` |
| `Fl` | Flag booleano | `bool` | `FlAtivo`, `FlProcessado` |
| `In` | Indicador booleano | `bool` | `InVerificado`, `InBloqueado` |
| `Ur` | Endereço web | `string` | `UrlLogo`, `UrlCallback` |

### Regras

1. **Chaves primárias** usam `Id` sem prefixo de categoria: `Id` (gerado pelo EF Core como `<Entidade>Id` na FK).
2. **Chaves estrangeiras** seguem o nome da entidade referenciada + `Id`: `ClienteId`, `ProdutoId` — sem prefixo adicional.
3. **Propriedades de navegação** (objetos/coleções EF Core) não levam prefixo: `Cliente`, `ICollection<Ordem>`.
4. **Percentuais** (`Pc`) são armazenados como fração decimal (0.05 = 5%) ou valor inteiro (5 = 5%) — documentar no campo qual convenção se aplica via atributo `[Comment]` no EF Core.
5. **Enum de Status/Tipo**: preferir `enum` tipado em C#; o valor persistido no banco é o nome em string (`ToString()`), não o inteiro.
6. **Timestamps de auditoria** (`TsCriacao`, `TsAtualizacao`) devem estar em UTC.

### Exemplo de entidade

```csharp
public class Ordem
{
    public Guid Id { get; set; }

    public Guid CarteiraId { get; set; }      // FK — sem prefixo de categoria
    public Carteira Carteira { get; set; }    // Navegação — sem prefixo

    public string CdTicker    { get; set; }   // Cd — código de negócio
    public TipoOperacao TpOperacao { get; set; } // Tp — tipo/categoria
    public StatusOrdem  StOrdem    { get; set; } // St — status

    public decimal QtCotas  { get; set; }     // Qt — quantidade
    public decimal VlUnitario { get; set; }   // Vl — valor monetário
    public decimal VlTotal    { get; set; }   // Vl — valor monetário
    public decimal PcCorretagem { get; set; } // Pc — percentual/taxa

    public DateOnly DtOrdem  { get; set; }    // Dt — data
    public DateTime TsCriacao { get; set; }   // Ts — timestamp UTC
}
```
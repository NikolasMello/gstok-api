---
name: migracao-ef
description: Gera e revisa migrations do EF Core neste projeto. Use ao criar ou alterar qualquer entidade em app/Domain/Models/, ao mexer no AppDbContext (relacionamento, índice, conversão de enum), ao renomear propriedade ou tabela, e quando aparecer erro de coluna inexistente em runtime ou o snapshot divergir do modelo. Use também antes de afirmar que uma mudança de modelo está aplicada no banco.
---

# Migrations do EF Core na gstok-api

Migrations aqui erram sempre da mesma forma: **a alteração do modelo é feita e a migration não
acompanha inteira**. O rastro está no histórico —
`20260822155953_CorrigirColunaStVendaFaltante` nasceu 20 h depois de `AdicionarLojaOnline` só
para acrescentar uma coluna que ficou de fora, e há três migrations `*Timestamps` criadas logo
após a tabela que deveria já tê-los (`AddTipoProdutoTimestamps` veio 2 minutos depois de
`AddTipoProduto`).

Nada disso quebra o build. Quebra em runtime, no primeiro request que toca a coluna.

## 1. Onde tudo mora

| O que | Caminho |
|---|---|
| Entidades | `app/Domain/Models/*.cs` (namespace `gstok_api.Models`) |
| DbContext | `app/Infrastructure/Database/AppDbContext.cs` (namespace `gstok_api.Database`) |
| Migrations e snapshot | `app/Infrastructure/Migrations/` |
| Connection string | `appsettings.Development.json` → `ConnectionStrings:DefaultConnection` |

## 2. Antes de gerar

Toda entidade nova precisa, no mínimo:

- `[Table("nome_snake_case")]` e `[Column("nome_snake_case")]` em **cada** propriedade.
- PK `Id<Entidade>` com `[Key]` + `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]`;
  FK `<Entidade>Id`.
- `TsCriacao` (`DateTime`) e `TsEdicao` (`DateTime?`) — **o esquecimento mais repetido deste
  repo**. Confira antes de gerar, não depois.
- `[Required]` e `[MaxLength]` onde couber: viram `NOT NULL` e `varchar(n)`.
- Prefixo semântico correto (`Nm`, `Vl`, `Qt`, `St`, `Tp`, `Fl`, `Cd`, `Dt`, `Ts`…) — ver README.
- `DbSet<XModel>` no `AppDbContext`.

No `AppDbContext`, ainda:

- **Enum persistido precisa de `.HasConversion<string>()` + `.HasMaxLength(n)`.** Sem isso vai
  para o banco como inteiro e diverge do JSON, que serializa como string.
- Relacionamento com `OnDelete` **explícito**. O default do EF nem sempre é o que se quer, e o
  `OnDelete` é o que decide se o `ExcluirAsync` da feature é viável:
  `Restrict` (ex.: `Venda → Cliente`) faz a exclusão falhar no banco — cheque antes no service;
  `Cascade` leva dependentes junto; `SetNull` apaga o vínculo em silêncio.
- Índice único onde a regra exige (`Pessoa.CdInscricaoNacional`, `ContaCliente.NmEmail`).
  Índice parcial usa `.HasFilter("...")` com o **nome da coluna no banco**, em snake_case.

## 3. Gerar

```bash
# A API precisa estar parada — o dotnet ef faz build e o exe fica travado.
dotnet ef migrations add <Nome>
```

Nome em **português, PascalCase, verbo no infinitivo**: `AdicionarCompra`,
`RenomearCdSkuParaCdEan`, `RemoverProdutoIdDoEstoque`, `TornarTipoProdutoIdObrigatorio`. (As
migrations mais antigas estão em inglês — `AddSessaoTable`. Não siga essas; o padrão atual é
português.)

## 4. Revisar o arquivo gerado — não pule

Abra o `.cs` da migration e confira:

- **Todas** as colunas esperadas estão no `Up`. É aqui que se pega a coluna faltante antes de
  ela virar um `CorrigirColuna...`.
- `Down` desfaz de verdade.
- **Renomear virou `Rename`, não `Drop` + `Add`.** O EF costuma interpretar renomeação como
  remover-e-criar, o que **apaga os dados da coluna**. Se aparecer `DropColumn` seguido de
  `AddColumn` para o que era um rename, troque à mão por `RenameColumn`/`RenameTable`.
- Coluna `NOT NULL` adicionada a tabela com dados precisa de `defaultValue` ou de um `Sql()`
  preenchendo antes — senão o `database update` falha em produção mesmo passando no dev vazio.

## 5. Aplicar e verificar

```bash
dotnet ef database update
```

Confirme que o modelo e o snapshot não divergem — se este comando acusar algo, falta migration:

```bash
dotnet ef migrations has-pending-model-changes
```

## 6. Renomeou algo? o rastro vaza para fora do banco

`20260712150110_RenomearPedidoParaVenda` renomeou a entidade no banco e **deixou a pasta
`app/DTOs/Pedido/` para trás por seis semanas**, com namespace `gstok_api.DTOs.Venda` e classes
`VendaResponseDto` dentro de arquivos chamados `PedidoResponseDto.cs`.

Ao renomear entidade ou propriedade, varra também:

- `app/DTOs/<Dominio>/` — nome da pasta, nome dos arquivos, nome das classes, namespace.
- `app/Features/<Dominio>/`, `app/Mappings/<Dominio>/`.
- `../gstok-web/src/service/<dominio>/` — o front tipa o contrato à mão; renomear campo aqui
  quebra lá **sem erro de compilação em nenhum dos dois lados**. A skill `sincronizar-dto` do
  gstok-web é a contraparte.

```bash
grep -rn "NomeAntigo" app ../gstok-web/src
```

## 7. Checklist

1. `TsCriacao`/`TsEdicao` presentes.
2. `[Table]`/`[Column]` em snake_case; prefixos semânticos corretos.
3. `DbSet` registrado; enums com `.HasConversion<string>()`; `OnDelete` explícito.
4. Migration gerada com a API parada, nome em português.
5. Arquivo da migration **lido**: colunas completas, `Down` válido, rename é rename.
6. `database update` aplicado e `has-pending-model-changes` limpo.
7. Se houve renomeação, `grep` por todo o repo e pelo `gstok-web`.

Ao final, diga quais comandos você realmente rodou. Se não aplicou no banco, diga isso — "a
migration foi gerada" e "o banco está atualizado" são afirmações diferentes.

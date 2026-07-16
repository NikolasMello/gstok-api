using gstok_api.DTOs;
using gstok_api.DTOs.Fornecedor;
using gstok_api.Exceptions;
using gstok_api.Features.Colecao;
using gstok_api.Mappings.Fornecedor;
using gstok_api.Models;

namespace gstok_api.Features.Fornecedor;

public class FornecedorService(IFornecedorRepository fornecedorRepository, IColecaoRepository colecaoRepository) : IFornecedorService
{
    private const string NmColecaoAvulsoDefault = "Avulso";

    public async Task<PagedResult<FornecedorResponseDto>> ObterTodosAsync(PaginationParams pagination)
    {
        var result = await fornecedorRepository.ObterTodosAsync(pagination);
        return new PagedResult<FornecedorResponseDto>
        {
            Items = result.Items.Select(FornecedorMapper.ParaResposta).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
    }

    public async Task<FornecedorDetalheResponseDto?> ObterPorIdAsync(Guid id)
    {
        var fornecedor = await fornecedorRepository.ObterPorIdAsync(id);
        return fornecedor is null ? null : FornecedorMapper.ParaDetalhe(fornecedor);
    }

    public async Task<FornecedorResponseDto> CriarAsync(FornecedorCreateDto dto)
    {
        if (await fornecedorRepository.CnpjExisteAsync(dto.CdCnpj))
            throw new ConflitoException("CNPJ já cadastrado.");

        var nomesColecoes = MontarNomesColecoes(dto.NmColecoes);

        var fornecedor = new FornecedorModel
        {
            IdFornecedor = Guid.CreateVersion7(),
            CdCnpj = dto.CdCnpj,
            NmEmpresa = dto.NmEmpresa,
            NmFantasia = dto.NmFantasia,
            NmMarca = dto.NmMarca,
            TsCriacao = DateTime.UtcNow
        };

        var criado = await fornecedorRepository.CriarAsync(fornecedor);

        await colecaoRepository.CriarVariosAsync(nomesColecoes.Select(nome => new ColecaoModel
        {
            IdColecao = Guid.CreateVersion7(),
            FornecedorId = criado.IdFornecedor,
            NmColecao = nome,
            TsCriacao = DateTime.UtcNow
        }));

        return FornecedorMapper.ParaResposta(criado);
    }

    private static List<string> MontarNomesColecoes(List<string> nomes)
    {
        var nomesTratados = nomes
            .Select(n => n.Trim())
            .Where(n => n.Length > 0)
            .ToList();

        var temDuplicados = nomesTratados
            .GroupBy(n => n.ToLower())
            .Any(g => g.Count() > 1);

        if (temDuplicados)
            throw new ConflitoException("Nomes de coleção não podem se repetir.");

        if (!nomesTratados.Any(n => string.Equals(n, NmColecaoAvulsoDefault, StringComparison.OrdinalIgnoreCase)))
            nomesTratados.Add(NmColecaoAvulsoDefault);

        return nomesTratados;
    }

    public async Task<FornecedorResponseDto?> AtualizarAsync(Guid id, FornecedorUpdateDto dto)
    {
        if (await fornecedorRepository.CnpjExisteAsync(dto.CdCnpj, id))
            throw new ConflitoException("CNPJ já cadastrado.");

        var fornecedor = new FornecedorModel
        {
            CdCnpj = dto.CdCnpj,
            NmEmpresa = dto.NmEmpresa,
            NmFantasia = dto.NmFantasia,
            NmMarca = dto.NmMarca
        };

        var updated = await fornecedorRepository.AtualizarAsync(id, fornecedor);
        return updated is null ? null : FornecedorMapper.ParaResposta(updated);
    }

    public async Task<bool> ExcluirAsync(Guid id) =>
        await fornecedorRepository.ExcluirAsync(id);
}

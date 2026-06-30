using gstok_api.DTOs;
using gstok_api.Interfaces;
using gstok_api.Models;

namespace gstok_api.Services;

public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    public async Task<IEnumerable<Pessoa>> GetAllAsync() =>
        await pessoaRepository.GetAllAsync();

    public async Task<Pessoa?> GetByIdAsync(Guid id) =>
        await pessoaRepository.GetByIdAsync(id);

    public async Task<Pessoa> CreateAsync(PessoaRequestDto dto)
    {
        var pessoa = new Pessoa
        {
            NrCpf = dto.NrCpf,
            NmPessoa = dto.NmPessoa,
            NmSobrenome = dto.NmSobrenome,
            NmTelefone = dto.NmTelefone,
            NmEmail = dto.NmEmail
        };
        return await pessoaRepository.CreateAsync(pessoa);
    }

    public async Task<Pessoa?> UpdateAsync(Guid id, PessoaRequestDto dto)
    {
        var pessoa = new Pessoa
        {
            NrCpf = dto.NrCpf,
            NmPessoa = dto.NmPessoa,
            NmSobrenome = dto.NmSobrenome,
            NmTelefone = dto.NmTelefone,
            NmEmail = dto.NmEmail
        };
        return await pessoaRepository.UpdateAsync(id, pessoa);
    }

    public async Task<bool> DeleteAsync(Guid id) =>
        await pessoaRepository.DeleteAsync(id);
}

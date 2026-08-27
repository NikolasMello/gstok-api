using gstok_api.Enums;

namespace gstok_api.DTOs.Pessoa;

public class PessoaResponseDto
{
    public Guid IdPessoa { get; set; }
    public string CdInscricaoNacional { get; set; } = string.Empty;
    public TipoPessoa TpPessoa { get; set; }
    public string NmPessoa { get; set; } = string.Empty;
    public string NmSobrenome { get; set; } = string.Empty;
    public string NmTelefone { get; set; } = string.Empty;
    public string NmEmailContato { get; set; } = string.Empty;
    public DateTime TsCriacao { get; set; }
    public DateTime? TsEdicao { get; set; }
}

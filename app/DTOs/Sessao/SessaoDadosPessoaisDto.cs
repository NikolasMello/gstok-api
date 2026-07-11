namespace gstok_api.DTOs.Sessao;

public class SessaoDadosPessoaisDto
{
    public string CdInscricaoNacional { get; set; } = string.Empty;
    public string NmPessoa { get; set; } = string.Empty;
    public string NmSobrenome { get; set; } = string.Empty;
    public string NmTelefone { get; set; } = string.Empty;
    public string NmEmailContato { get; set; } = string.Empty;
    public FotoPessoaResponseDto? Foto { get; set; }
}

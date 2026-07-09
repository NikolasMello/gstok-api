namespace gstok_api.DTOs;

public class FotoPessoaResponseDto
{
    public Guid IdFotoPessoa { get; set; }
    public string NmImagem { get; set; } = string.Empty;
    public string UrImagem { get; set; } = string.Empty;
    public int Largura { get; set; }
    public int Altura { get; set; }
}
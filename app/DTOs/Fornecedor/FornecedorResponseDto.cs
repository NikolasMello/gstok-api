namespace gstok_api.DTOs.Fornecedor;

public class FornecedorResponseDto
{
    public Guid IdFornecedor { get; set; }
    public string CdCnpj { get; set; } = string.Empty;
    public string NmEmpresa { get; set; } = string.Empty;
    public string? NmFantasia { get; set; }
    public string? NmMarca { get; set; }
    public DateTime TsCriacao { get; set; }
}

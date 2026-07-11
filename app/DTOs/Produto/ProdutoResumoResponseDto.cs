using gstok_api.Enums;

namespace gstok_api.DTOs;

public class ProdutoResumoResponseDto
{
    public Guid Id { get; set; }
    public string NmProduto { get; set; } = string.Empty;
    public string? NmMarca { get; set; }
    public decimal VlVenda { get; set; }
    public string? NmTipo { get; set; }
    public Estacao TpEstacao { get; set; }
    public DateTime TsCriacao { get; set; }
    public ImageVariante Avatar { get; set; } = null!;
}

//FORNECEDOR É UMA MARCA -> UM FORNECEDOR PODE TER COLEÇÕES > EXEMPLO PIMENTA / VIOLETA
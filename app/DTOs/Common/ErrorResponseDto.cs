using gstok_api.Exceptions;

namespace gstok_api.DTOs;

public class ErrorResponseDto
{
    public Severidade Severidade { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}

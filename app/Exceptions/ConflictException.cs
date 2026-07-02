namespace gstok_api.Exceptions;

// Conflito de unicidade (e-mail, CPF, SKU duplicado) — alerta, o usuário pode corrigir
public class ConflictException(string message)
    : AppException(message, Severidade.Alerta, StatusCodes.Status409Conflict);

namespace gstok_api.Exceptions;

// Conflito de unicidade (e-mail, CPF, SKU duplicado) — alerta, o usuário pode corrigir
public class ConflitoException(string message)
    : ExcecaoBase(message, Severidade.Alerta, StatusCodes.Status409Conflict);

namespace gstok_api.Exceptions;

// Violação de regra de negócio — alerta, o usuário pode corrigir a entrada
public class ExcecaoNegocio(string message)
    : ExcecaoBase(message, Severidade.Alerta, StatusCodes.Status422UnprocessableEntity);

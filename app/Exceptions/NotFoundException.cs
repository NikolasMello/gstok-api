namespace gstok_api.Exceptions;

// Recurso não encontrado — erro sem ação possível pelo usuário
public class NotFoundException(string message)
    : AppException(message, Severidade.Erro, StatusCodes.Status404NotFound);

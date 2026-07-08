namespace gstok_api.Exceptions;

// Recurso não encontrado — erro sem ação possível pelo usuário
public class NaoEncontradoException(string message)
    : ExcecaoBase(message, Severidade.Erro, StatusCodes.Status404NotFound);

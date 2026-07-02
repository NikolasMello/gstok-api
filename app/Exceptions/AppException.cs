namespace gstok_api.Exceptions;

public abstract class AppException(string message, Severidade severidade, int statusCode)
    : Exception(message)
{
    public Severidade Severidade { get; } = severidade;
    public int StatusCode { get; } = statusCode;
}

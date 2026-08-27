using gstok_api.Middleware;

namespace gstok_api.Common.Auth;

public static class ClienteContextExtensions
{
    // Só é seguro chamar em actions marcadas [ExigeCliente] — o MiddlewareSessaoCliente
    // garante que o item foi preenchido antes da action executar.
    public static Guid ObterClienteId(this HttpContext context) =>
        (Guid)context.Items[MiddlewareSessaoCliente.ClienteIdKey]!;
}

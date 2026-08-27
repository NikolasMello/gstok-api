namespace gstok_api.Common.Auth;

// Marca controllers/actions do pacote Store que exigem uma sessão de cliente válida
// (cookie "sid_cliente"). Interpretada pelo MiddlewareSessaoCliente — independente
// do MiddlewareSessao (sessão de Usuario/backoffice).
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ExigeClienteAttribute : Attribute;

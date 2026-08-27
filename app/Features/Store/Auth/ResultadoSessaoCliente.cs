namespace gstok_api.Features.Store.Auth;

public record ResultadoSessaoCliente(
    string Token,
    DateTime Expires,
    string NmEmail,
    string NmPessoa,
    string NmSobrenome
);

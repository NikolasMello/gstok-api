namespace gstok_api.Features.Auth;

public record ResultadoSessaoAuth(
    string Token,
    DateTime Expires,
    string NmEmail,
    string? NmPessoa,
    string? NmSobrenome,
    string? UrAvatar
);

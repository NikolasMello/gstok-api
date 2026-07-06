namespace gstok_api.Features.Auth;

public record AuthSessionResult(
    string AccessToken,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpires
);

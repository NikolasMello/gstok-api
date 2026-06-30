namespace gstok_api.Settings;

public class AuthSettings
{
    public JwtSettings Jwt { get; set; } = new();
    public SessionSettings Session { get; set; } = new();
}

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "gstok-api";
    public string Audience { get; set; } = "gstok-web";
    public int AccessTokenExpirationMinutes { get; set; } = 15;
}

public class SessionSettings
{
    public int RefreshTokenExpirationDays { get; set; } = 7;
}

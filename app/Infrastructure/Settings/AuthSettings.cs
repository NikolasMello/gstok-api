namespace gstok_api.Settings;

public class AuthSettings
{
    public SessionSettings Session { get; set; } = new();
    public CookieSettings Cookie { get; set; } = new();
}

public class SessionSettings
{
    public int ExpirationDays { get; set; } = 7;
}

public class CookieSettings
{
    public string? Domain { get; set; }
    public bool Secure { get; set; } = true;
    public string SameSite { get; set; } = "None";
}

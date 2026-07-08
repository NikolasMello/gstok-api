namespace gstok_api.Settings;

public class ConfiguracaoAuth
{
    public ConfiguracaoSessao Session { get; set; } = new();
    public ConfiguracaoCookie Cookie { get; set; } = new();
}

public class ConfiguracaoSessao
{
    public int ExpirationDays { get; set; } = 7;
}

public class ConfiguracaoCookie
{
    public string? Domain { get; set; }
    public bool Secure { get; set; } = true;
    public string SameSite { get; set; } = "None";
}

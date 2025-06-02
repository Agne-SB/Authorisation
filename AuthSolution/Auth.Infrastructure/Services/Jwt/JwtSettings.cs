namespace Auth.Infrastructure.Services.Jwt;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "AuthService";
    public string Audience { get; set; } = "YourClients";
    public int ExpirationMinutes { get; set; } = 15;
}

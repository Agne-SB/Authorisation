namespace Auth.Domain.Entities;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = new();
}

public class RefreshToken
{
    public string? Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
}

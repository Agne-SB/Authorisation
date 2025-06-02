using Auth.Application.Ports.Services;
using BCrypt.Net;

namespace Auth.Infrastructure.Services.Crypto;

public class BcryptCryptoService : ICryptographyService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

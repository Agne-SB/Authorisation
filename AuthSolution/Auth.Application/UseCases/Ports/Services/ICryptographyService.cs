namespace Auth.Application.Ports.Services;

public interface ICryptographyService
{
    string HashPassword(string password);
    bool VerifyPassword(string hash, string password);
}

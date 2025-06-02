using Auth.Domain.Entities;

namespace Auth.Application.Ports.Services;

public interface IAuthTokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken();
}

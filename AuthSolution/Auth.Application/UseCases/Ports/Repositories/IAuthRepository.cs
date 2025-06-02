using Auth.Domain.Entities;

namespace Auth.Application.Ports.Repositories;

public interface IAuthRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task SaveRefreshTokenAsync(string userId, RefreshToken token);

    Task CreateUserAsync(User user);

}

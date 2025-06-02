using Auth.Application.Ports.Repositories;
using Auth.Application.Ports.Services;
using Auth.Domain.Entities;

namespace Auth.Application.UseCases.Login;

public class LoginUseCase
{
    private readonly IAuthRepository _repository;
    private readonly ICryptographyService _crypto;
    private readonly IAuthTokenService _tokenService;

    public LoginUseCase(IAuthRepository repository, ICryptographyService crypto, IAuthTokenService tokenService)
    {
        _repository = repository;
        _crypto = crypto;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse?> HandleAsync(LoginRequest request)
    {
        var user = await _repository.GetUserByUsernameAsync(request.Username);
        if (user == null || !_crypto.VerifyPassword(user.PasswordHash, request.Password))
            return null;

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await _repository.SaveRefreshTokenAsync(user.Id, refreshToken);

        return new LoginResponse(accessToken, refreshToken.Token);
    }
}

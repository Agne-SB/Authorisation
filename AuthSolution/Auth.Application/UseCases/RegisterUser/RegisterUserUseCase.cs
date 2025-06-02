using Auth.Application.Ports.Repositories;
using Auth.Application.Ports.Services;
using Auth.Domain.Entities;

namespace Auth.Application.UseCases.RegisterUser;

public class RegisterUserUseCase
{
    private readonly IAuthRepository _repository;
    private readonly ICryptographyService _crypto;

    public RegisterUserUseCase(IAuthRepository repository, ICryptographyService crypto)
    {
        _repository = repository;
        _crypto = crypto;
    }

    public async Task<RegisterResponse> HandleAsync(RegisterRequest request)
    {
        if (!request.Username.EndsWith("@grasdal.no", StringComparison.OrdinalIgnoreCase))
        {
            return new RegisterResponse(false, "Email must end with @grasdal.no");
        }

        var existing = await _repository.GetUserByUsernameAsync(request.Username);
        if (existing != null)
        {
            return new RegisterResponse(false, "User already exists");
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = _crypto.HashPassword(request.Password)
        };

        await _repository.CreateUserAsync(user);

        return new RegisterResponse(true, "User registered successfully");
    }
}

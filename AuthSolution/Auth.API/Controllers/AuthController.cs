using Auth.Application.UseCases.Login;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;

    public AuthController(LoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _loginUseCase.HandleAsync(request);
        if (result == null)
            return Unauthorized("Invalid credentials");

        return Ok(result);
    }
}

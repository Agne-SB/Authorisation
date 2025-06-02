using Auth.Application.Ports.Repositories;
using Auth.Application.Ports.Services;
using Auth.Application.UseCases.Login;
using Auth.Application.UseCases.RegisterUser;
using Auth.Domain.Entities;
using Auth.Infrastructure.Repositories.SqlServer;
using Auth.Infrastructure.Services;
using Auth.Infrastructure.Services.Jwt;
using Auth.Infrastructure.Services.Crypto;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

// === AppSettings Configuration ===
builder.Services.Configure<SqlDbSettings>(
    builder.Configuration.GetSection("SqlDbSettings"));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// === Dependency Injection ===
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICryptographyService, BcryptCryptoService>();
builder.Services.AddScoped<IAuthTokenService, JwtService>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();

// === Swagger (OpenAPI) ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === Middleware ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === Endpoints ===
app.MapPost("/api/auth/login", async (
    LoginRequest request,
    LoginUseCase loginUseCase) =>
{
    var result = await loginUseCase.HandleAsync(request);
    return result is null ? Results.Unauthorized() : Results.Ok(result);
});

app.MapPost("/api/auth/register", async (
    RegisterRequest request,
    RegisterUserUseCase registerUseCase) =>
{
    var result = await registerUseCase.HandleAsync(request);
    return Results.Ok(result);
});

app.Run();

using Auth.Application.Ports.Repositories;
using Auth.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Auth.Infrastructure.Repositories.SqlServer;

public class AuthRepository : IAuthRepository
{
    private readonly string _connectionString;

    public AuthRepository(IOptions<SqlDbSettings> settings)
    {
        _connectionString = settings.Value.ConnectionString;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var user = await db.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Username = @Username",
            new { Username = username });

        if (user != null)
        {
            var tokens = await db.QueryAsync<RefreshToken>(
                "SELECT * FROM RefreshTokens WHERE UserId = @UserId",
                new { UserId = user.Id });
            user.RefreshTokens = tokens.ToList();
        }

        return user;
    }

    public async Task SaveRefreshTokenAsync(string userId, RefreshToken token)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        await db.ExecuteAsync(
            "INSERT INTO RefreshTokens (Token, ExpiresAt, IsRevoked, UserId) VALUES (@Token, @ExpiresAt, @IsRevoked, @UserId)",
            new
            {
                token.Token,
                token.ExpiresAt,
                token.IsRevoked,
                UserId = userId
            });
    }

    public async Task CreateUserAsync(User user)
{
    using IDbConnection db = new SqlConnection(_connectionString);
    await db.ExecuteAsync(
        "INSERT INTO Users (Id, Username, PasswordHash) VALUES (@Id, @Username, @PasswordHash)",
        new
        {
            user.Id,
            user.Username,
            user.PasswordHash
        });
}

}

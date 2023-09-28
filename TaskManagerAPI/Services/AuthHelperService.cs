using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services;

public interface IAuthHelperService
{
    string HashPassword(string password);

    bool VerifyPassword(string passwordHash, string inputPassword);

    public string GenerateJwtToken(User user);
}

public class AuthHelperService : IAuthHelperService
{
    private readonly IConfiguration _configuration;
    
    private readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256; 
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 10000;
    private const char Delimiter = ';';

    public AuthHelperService(IConfiguration configuration) 
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool VerifyPassword(string passwordHash, string inputPassword)
    {
        if (string.IsNullOrWhiteSpace(passwordHash) || string.IsNullOrWhiteSpace(inputPassword))
        {
            return false;
        }

        var elements = passwordHash.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);
        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }

    public string GenerateJwtToken(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentNullException(nameof(user.Username));
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentNullException(nameof(user.Email));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims, expires: DateTime.UtcNow.AddDays(60), notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException())),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
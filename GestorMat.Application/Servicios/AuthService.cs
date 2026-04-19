using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestorMat.Application.Servicios;

public class AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration, IPasswordHasher hasher)
{
    public async Task<string> LoginAsync(LoginDto dto)
    {
        Usuario? user = await usuarioRepository.ObtenerPorUsernameAsync(dto.Username);

        if (user == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        if (!hasher.Verify(dto.Password, user.PasswordHash))
        {
            throw new Exception("Password incorrecta");
        }

        return GenerarToken(user);
    }

    private string GenerarToken(Usuario usuario)
    {
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.Role, usuario.Rol?.RolName ?? string.Empty)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty)
        );

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

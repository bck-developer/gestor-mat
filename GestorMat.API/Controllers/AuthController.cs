using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

/// <summary>
/// Controlador para la autenticación de usuarios.
/// Permite obtener tokens JWT para acceder a endpoints protegidos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{

    /// <summary>
    /// Autentica un usuario y devuelve un token JWT.
    /// </summary>
    /// <param name="dto">Credenciales del usuario (username y password)</param>
    /// <returns>Token JWT y datos del usuario autenticado</returns>
    /// <response code="200">Autenticación exitosa, token generado</response>
    /// <response code="400">Credenciales inválidas o error en la autenticación</response>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            string token = await authService.LoginAsync(dto);

            return Ok(new
            {
                token = token,
                message = "Autenticación exitosa"
            });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new
            {
                message = excepcion.Message
            });
        }
    }
}
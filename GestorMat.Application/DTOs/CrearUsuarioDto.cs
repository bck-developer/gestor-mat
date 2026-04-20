namespace GestorMat.Application.DTOs;

public class CrearUsuarioDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public int IdRol { get; set; }
    public bool Activo { get; set; }
}

namespace GestorMat.Application.DTOs;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get;  set; }
    public int IdRol { get; set; }
}

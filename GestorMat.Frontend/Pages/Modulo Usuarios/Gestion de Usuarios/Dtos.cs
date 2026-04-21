namespace GestorMat.Frontend.Pages.Gestion_de_Usuarios;

public class UsuarioDto
{
    public int Id_Usuario { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public int Id_Rol { get; set; }
    public bool Activo { get; set; }
}

public class RolDto
{
    public int IdRol { get; set; }
    public string RolName { get; set; } = string.Empty;
}

public class CrearUsuarioDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public int Id_Rol { get; set; }
    public bool Activo { get; set; } = true;
}

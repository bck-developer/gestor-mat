using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades;

public class Usuario
{
    [Key]
    public int Id_Usuario { get; private set; }

    [StringLength(14, MinimumLength = 3)]
    public string Username { get; private set; }

    [StringLength(255)]
    public string PasswordHash { get; private set; }

    [StringLength(60)]
    public string Nombre { get; private set; }

    [StringLength(100)]
    [EmailAddress]
    public string Mail { get; private set; }

    public DateTime FechaAlta { get; private set; }

    public int IdRol { get; private set; }

    public bool Activo { get; private set; }

    public Rol Rol { get; private set; }

    private Usuario() { }

    public Usuario(string username, string passwordHash, string nombre, string mail, int idRol, bool activo)
    {
        Username = username;
        PasswordHash = passwordHash;
        Nombre = nombre;
        Mail = mail;
        IdRol = idRol;
        FechaAlta = DateTime.UtcNow;
        Activo = activo;
    }

    public void ActualizarDatos(string username, string nombre, string mail, int idRol, bool activo)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new Exception("Username inválido");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new Exception("Nombre inválido");

        if (string.IsNullOrWhiteSpace(mail))
            throw new Exception("Mail inválido");

        if (idRol == 0)
            throw new Exception("Rol inválido");

        Username = username;
        Nombre = nombre;
        Mail = mail;
        IdRol = idRol;
        Activo = activo;
    }

    public void CambiarPassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new Exception("Password inválido");

        PasswordHash = passwordHash;
    }

    public void Desactivar()
    {
        Activo = false;
    }

    public void Activar()
    {
        Activo = true;
    }
}

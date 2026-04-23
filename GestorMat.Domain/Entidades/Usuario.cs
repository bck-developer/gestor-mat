using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades;

public class Usuario
{
    [Key]
    public int Id_Usuario { get;set; }

    [StringLength(14, MinimumLength = 3)]
    public string Username { get;set; }

    [StringLength(255)]
    public string PasswordHash { get;set; }

    [StringLength(60)]
    public string Nombre { get;set; }

    [StringLength(100)]
    [EmailAddress]
    public string Mail { get;set; }

    public DateTime FechaAlta { get;set; }

    public int IdRol { get;set; }

    public bool Activo { get;set; }

    public Rol Rol { get;set; }

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

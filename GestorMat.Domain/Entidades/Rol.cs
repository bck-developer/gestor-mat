using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades;

public class Rol
{
    [Key]
    public int Id_Rol { get; private set; }

    [StringLength(30)]
    public string RolName { get; private set; }

    [StringLength(100)]
    public string Descripcion { get; private set; }

    public bool AccesoTotal { get; private set; }

    public ICollection<Usuario> Usuarios { get; private set; }

    private Rol() { }

    public Rol(string rolName, string descripcion, bool accesoTotal)
    {
        RolName = rolName;
        Descripcion = descripcion;
        AccesoTotal = accesoTotal;
        Usuarios = new List<Usuario>();
    }
}

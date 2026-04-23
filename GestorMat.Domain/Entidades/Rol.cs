using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades;

public class Rol
{
    [Key]
    public int Id_Rol { get; set; }

    [StringLength(30)]
    public string RolName { get; set; }

    [StringLength(100)]
    public string Descripcion { get; set; }

    public bool AccesoTotal { get; set; }

    public ICollection<Usuario> Usuarios { get; set; }

    public Rol() { }

    public Rol(string rolName, string descripcion, bool accesoTotal)
    {
        RolName = rolName;
        Descripcion = descripcion;
        AccesoTotal = accesoTotal;
        Usuarios = new List<Usuario>();
    }
}

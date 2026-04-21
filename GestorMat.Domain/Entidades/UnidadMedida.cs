using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades;

public class UnidadMedida
{
    [Key]
    public int Id_UnidadMedida { get; private set; }

    [StringLength(30)]
    public string Nombre { get; private set; }

    [StringLength(10)]
    public string Abreviatura { get; private set; }

    public bool Activo { get; private set; }


    public UnidadMedida(string nombre, string abreviatura, bool activo)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio");

        if (string.IsNullOrWhiteSpace(abreviatura))
            throw new ArgumentException("La abreviatura es obligatoria");

        Nombre = nombre;
        Abreviatura = abreviatura;
        Activo = activo;
    }

    public void Desactivar()
    {
        Activo = false;
    }
    public void Activar()
    {
        Activo = true;
    }

    public void ActualizarDatos(string nombre, string abreviatura, bool activo)
    {
        Nombre = nombre;
        Abreviatura = abreviatura;
        Activo = activo;
    }
}
using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades;

public class Deposito
{
    [Key]
    public int Id_Deposito { get; set; }

    [StringLength(20)]
    public string CodigoDeposito { get; init; } = string.Empty;

    [StringLength(60)]
    public string Nombre { get; init; } = string.Empty;

    [StringLength(120)]
    public string Direccion { get; init; } = string.Empty;

    public bool Habilitado { get; init; }

    public Deposito(string codigoDeposito, string nombre, string direccion, bool habilitado)
    {
        if (string.IsNullOrWhiteSpace(codigoDeposito))
        {
            throw new Exception("El código de depósito no puede estar vacío");
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new Exception("El nombre del depósito no puede estar vacío");
        }

        if (string.IsNullOrWhiteSpace(direccion))
        {
            throw new Exception("La dirección del depósito no puede estar vacía");
        }

        CodigoDeposito = codigoDeposito;
        Nombre = nombre;
        Direccion = direccion;
        Habilitado = habilitado;
    }
}

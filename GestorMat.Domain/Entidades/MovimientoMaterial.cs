using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades;

public class MovimientoMaterial
{
    [Key]
    public int Id_MovimientoMaterial { get; private set; }

    public int Id_Material { get; private set; }

    public int Id_DepositoOrigen { get; private set; }

    public int? Id_DepositoDestino { get; private set; }

    [StringLength(30)]
    public string CodigoMovimiento { get; private set; }

    [StringLength(20)]
    public string Tipo { get; private set; }

    public double Cantidad { get; private set; }

    public DateTime Fecha { get; private set; }

    [StringLength(50)]
    public string UserName { get; private set; }

    private MovimientoMaterial() { } // EF

    public MovimientoMaterial(
        int idMaterial,
        int idDepositoOrigen,
        int? idDepositoDestino,
        string codigoMovimiento,
        string tipo,
        double cantidad,
        DateTime fecha,
        string UserName)
    {
        Id_Material = idMaterial;
        Id_DepositoOrigen = idDepositoOrigen;
        Id_DepositoDestino = idDepositoDestino;
        CodigoMovimiento = codigoMovimiento;
        Tipo = tipo;
        Cantidad = cantidad;
        Fecha = fecha;
        this.UserName = UserName;
    }
}

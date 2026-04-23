namespace GestorMat.Application.DTOs.Saldo;

public class SaldoDto
{
    public int IdMaterial { get; set; }
    public string CodigoMaterial { get; set; } = string.Empty;
    public string NombreMaterial { get; set; } = string.Empty;

    public int IdDeposito { get; set; }
    public string CodigoDeposito { get; set; } = string.Empty;
    public string NombreDeposito { get; set; } = string.Empty;

    public string UnidadMedida { get; set; } = string.Empty;

    public double CantidadDisponible { get; set; }
    public double StockMinimo { get; set; }

    public DateTime FechaUltimoMovimiento { get; set; }
}

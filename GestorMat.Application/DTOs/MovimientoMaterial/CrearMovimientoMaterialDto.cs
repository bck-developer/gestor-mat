namespace GestorMat.Application.DTOs.MovimientoMaterial;

public class CrearMovimientoMaterialDto
{
    public int IdMaterial { get; set; }
    public int IdDepositoOrigen { get; set; }
    public int? IdDepositoDestino { get; set; }

    public string Tipo { get; set; } = string.Empty;
    public double Cantidad { get; set; }
    public DateTime Fecha { get; set; }
    public string CodigoMovimiento { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}

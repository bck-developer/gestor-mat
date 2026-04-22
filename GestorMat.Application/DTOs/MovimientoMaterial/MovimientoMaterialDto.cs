namespace GestorMat.Application.DTOs.MovimientoMaterial;

public class MovimientoMaterialDto
{
    public int Id_MovimientoMaterial { get; set; }
    public string CodigoMovimiento { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public double Cantidad { get; set; }
    public DateTime Fecha { get; set; }
    public int Id_Material { get; set; }
    public int Id_DepositoOrigen { get; set; }
    public int? Id_DepositoDestino { get; set; }
    public string UserName { get; set; } = string.Empty;
}

namespace GestorMat.Application.DTOs.Material;

public class ActualizarMaterialDto
{
    public string CodigoMaterial { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int Id_UnidadMedida { get; set; }
    public double? StockMinimo { get; set; }
    public bool PermiteStockNegativo { get; set; }
    public bool Activo { get; set; } = true;
}

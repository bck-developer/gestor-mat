namespace GestorMat.Application.DTOs.Material;

public class CrearMaterialDto
{
    public string CodigoMaterial { get; set; } = string.Empty ;
    public string Nombre { get; set; } = string.Empty ;
    public decimal Precio { get; set; }
    public int Id_UnidadMedida { get; set; }
    public bool Activo { get; set; }
    public string Descripcion { get; set; } = string.Empty ;
    public bool PermiteStockNegativo { get; set; }
    public double StockMinimo { get; set; }    
}

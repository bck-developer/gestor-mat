namespace GestorMat.Application.DTOs.Material;

public class MaterialExcelRowDto
{
    public int Fila { get; set; }

    public string CodigoMaterial { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string PrecioRaw { get; set; } = string.Empty;
    public string UnidadNombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ActivoRaw { get; set; } = string.Empty;
    public string PermiteStockNegativoRaw { get; set; } = string.Empty;
    public string StockMinimoRaw { get; set; } = string.Empty;
}

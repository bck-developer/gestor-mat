namespace GestorMat.Application.DTOs.Material;

public class MaterialExcelRowDto
{
    public int Fila { get; set; }

    public string CodigoMaterial { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string PrecioRaw { get; set; } = "";
    public string UnidadNombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public string ActivoRaw { get; set; } = "";
    public string PermiteStockNegativoRaw { get; set; } = "";
    public string StockMinimoRaw { get; set; } = "";
}

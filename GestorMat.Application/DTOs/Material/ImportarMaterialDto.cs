namespace GestorMat.Application.DTOs.Material;

public class ImportarMaterialDto
{
    public string CodigoMaterial { get; set; } = "";
    public string Nombre { get; set; } = "";
    public decimal Precio { get; set; }
    public string UnidadMedida { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public bool Activo { get; set; }
    public bool PermiteStockNegativo { get; set; }
    public double StockMinimo { get; set; }
}

public class ResultadoImportacionDto
{
    public List<string> Errores { get; set; } = new();
    public int RegistrosInsertados { get; set; }
}
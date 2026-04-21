using GestorMat.Domain.Entidades;

namespace GestorMat.Application.DTOs.Material;

public class MaterialDto
{
    public int Id_Material { get; internal set; }
    public string CodigoMaterial { get; set; } = "";
    public string Nombre { get; set; } = "";
    public decimal Precio { get; set; }
    public string UnidadMedida { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public bool PermiteStockNegativo { get; set; }
    public double StockMinimo { get; set; }
    public bool Activo { get; set; }
    public int Id_UnidadMedida { get; set; }
}

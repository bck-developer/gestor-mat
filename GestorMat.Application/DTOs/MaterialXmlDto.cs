namespace GestorMat.Application.DTOs;

public class MaterialXmlDto
{
    public required string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int IdUnidadMedida { get; set; }
}

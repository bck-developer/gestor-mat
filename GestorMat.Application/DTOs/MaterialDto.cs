namespace GestorMat.Application.DTOs;

public class MaterialDto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public decimal Precio { get; set; }

    public string UnidadMedida { get; set; } = string.Empty;
}

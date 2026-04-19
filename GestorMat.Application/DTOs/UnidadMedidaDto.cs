namespace GestorMat.Application.DTOs;

public class CrearUnidadMedidaDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class UnidadMedidaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

namespace GestorMat.Application.DTOs;

public class CrearUnidadMedidaDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
    public bool Activo { get;  set; }
}

public class UnidadMedidaDto
{
    public int Id_UnidadMedida { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
    public bool Activo { get; set; }
}

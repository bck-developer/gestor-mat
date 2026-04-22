namespace GestorMat.Application.DTOs.Material;

public class MaterialImportResultDto
{
    public int TotalProcesados { get; set; }
    public int Exitosos { get; set; }
    public int Fallidos { get; set; }

    public List<ErrorImportacionDto> Errores { get; set; } = new();
}

public class ErrorImportacionDto
{
    public int Fila { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}
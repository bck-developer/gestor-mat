namespace GestorMat.Application.Interfaces;

public interface IExcelService
{
    byte[] GenerarPlantillaMateriales(List<string> unidades);
    List<Dictionary<string, string>> LeerExcel(Stream stream);
}
using GestorMat.Application.DTOs.Material;

namespace GestorMat.Application.Interfaces;

public interface IExcelService
{
    byte[] GenerarPlantillaMateriales(List<string> unidades);

    List<MaterialExcelRowDto> LeerExcelMateriales(Stream stream);
}
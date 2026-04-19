using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Servicios;

public class MaterialService(IMaterialRepository materialRepository)
{
    public async Task CrearMaterialAsync(CrearMaterialDto dto)
    {
        Material material = new Material(dto.Nombre, dto.Precio, dto.IdUnidadMedida);
        await materialRepository.AgregarAsync(material);
    }

    public async Task ImportarVariosAsync(IEnumerable<CrearMaterialDto> dtos)
    {
        IEnumerable<Material> materiales = dtos.Select(dto => new Material(dto.Nombre, dto.Precio, dto.IdUnidadMedida));
        await materialRepository.AgregarRangoAsync(materiales);
    }

    public async Task<List<MaterialDto>> ObtenerMaterialesAsync()
    {
        IEnumerable<Material> materiales = await materialRepository.ObtenerTodosAsync();

        List<MaterialDto> resultado = materiales.Select(m => new MaterialDto
        {
            Nombre = m.Nombre,
            Precio = m.Precio,
            UnidadMedida = m.UnidadMedida != null ? m.UnidadMedida.Abreviatura : string.Empty
        }).ToList();

        return resultado;
    }
}

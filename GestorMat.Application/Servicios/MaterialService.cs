using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using Mapster;

namespace GestorMat.Application.Servicios;

public class MaterialService(IMaterialRepository repository)
{
    // CREAR
    public async Task CrearAsyncService(CrearMaterialDto dto)
    {
        Material material = new(
            dto.CodigoMaterial,
            dto.Nombre,
            dto.Precio,
            dto.Id_UnidadMedida,
            dto.Activo,
            dto.Descripcion,
            dto.PermiteStockNegativo,
            dto.StockMinimo
        );

        await repository.AgregarAsync(material);
    }

    // OBTENER TODOS
    public async Task<List<MaterialDto>> ObtenerTodosAsyncService()
    {
        var materiales = await repository.ObtenerTodosAsync();
        return materiales.Adapt<List<MaterialDto>>();
    }

    // OBTENER POR ID
    public async Task<MaterialDto?> ObtenerPorIdAsyncService(int id)
    {
        Material? m = await repository.ObtenerPorIdAsync(id);
        return m?.Adapt<MaterialDto>();
    }

    // ACTUALIZAR
    public async Task ActualizarAsyncService(int id, ActualizarMaterialDto dto)
    {
        Material? material = await repository.ObtenerPorIdAsync(id);

        if (material == null)
        {
            throw new Exception("Material no encontrado");
        }

        material.ActualizarDatos(
            dto.CodigoMaterial,
            dto.Nombre,
            dto.Precio,
            dto.Id_UnidadMedida,
            dto.Activo,
            dto.Descripcion,
            dto.PermiteStockNegativo,
            dto.StockMinimo ?? 0
        );

        if (dto.Activo)
            material.Activar();
        else
            material.Desactivar();

        await repository.EditarAsync(material);
    }

    // ELIMINAR
    public async Task EliminarAsyncService(int id)
    {
        Material? material = await repository.ObtenerPorIdAsync(id);

        if (material == null)
            throw new Exception("Material no encontrado");

        await repository.EliminarAsync(material);
    }
}

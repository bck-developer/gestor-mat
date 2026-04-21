using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

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

        return materiales.Select(m => new MaterialDto
        {
            Id_Material = m.Id_Material,
            CodigoMaterial = m.CodigoMaterial,
            Nombre = m.Nombre,
            Precio = m.Precio,
            UnidadMedida = m.UnidadMedida.Nombre,
            Descripcion = m.Descripcion,
            PermiteStockNegativo = m.PermiteStockNegativo,
            StockMinimo = m.StockMinimo,
            Activo = m.Activo,
        }).ToList();
    }

    // OBTENER POR ID
    public async Task<MaterialDto?> ObtenerPorIdAsyncService(int id)
    {
        Material? m = await repository.ObtenerPorIdAsync(id);

        if (m == null)
            return null;

        return new MaterialDto
        {
            Id_Material = m.Id_Material,
            CodigoMaterial = m.CodigoMaterial,
            Nombre = m.Nombre,
            Precio = m.Precio,
            UnidadMedida = m.UnidadMedida.Nombre,
            Descripcion = m.Descripcion,
            PermiteStockNegativo = m.PermiteStockNegativo,
            StockMinimo = m.StockMinimo,
            Activo = m.Activo,
            Id_UnidadMedida = m.Id_UnidadMedida
        };
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

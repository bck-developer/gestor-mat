using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Servicios;

public class UnidadMedidaService(IUnidadMedidaRepository repository)
{
    // 1 - CREAR
    public async Task CrearAsyncService(CrearUnidadMedidaDto dto)
    {
        UnidadMedida unidad = new(dto.Nombre, dto.Abreviatura, dto.Activo);
        await repository.AgregarAsync(unidad);
    }

    // 2 - OBTENER TODOS
    public async Task<List<UnidadMedidaDto>> ObtenerTodosAsyncService()
    {
        List<UnidadMedida> unidades = await repository.ObtenerTodosAsync();

        return unidades.Select(u => new UnidadMedidaDto
        {
            Id_UnidadMedida = u.Id_UnidadMedida,
            Nombre = u.Nombre,
            Abreviatura = u.Abreviatura,
            Activo = u.Activo
        }).ToList();
    }

    // 3 - OBTENER POR ID
    public async Task<UnidadMedida?> ObtenerPorIdAsyncService(int id)
    {
        return await repository.ObtenerPorIdAsync(id);
    }

    // 4- ACTUALIZAR
    public async Task ActualizarAsyncService(int id, CrearUnidadMedidaDto dto)
    {
        UnidadMedida? unidadMedida = await repository.ObtenerPorIdAsync(id);

        if (unidadMedida == null)
        {
            throw new Exception("Unidad de medida no encontrada");
        }

        unidadMedida.ActualizarDatos(dto.Nombre, dto.Abreviatura, dto.Activo);


        await repository.EditarAsync(unidadMedida);
    }

    // 5 - ELIMINAR FISICO 
    public async Task EliminarFisicoAsync(int id)
    {
        UnidadMedida? unidadMedida = await repository.ObtenerPorIdAsync(id);

        if (unidadMedida == null)
        {
            throw new Exception("Unidad de medida no encontrada");
        }

        await repository.EliminarAsync(unidadMedida);
    }

}

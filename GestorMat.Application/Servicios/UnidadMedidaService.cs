using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Servicios;

public class UnidadMedidaService(IUnidadMedidaRepository repository)
{
    public async Task<List<UnidadMedidaDto>> ObtenerTodasAsync()
    {
        List<UnidadMedida> unidades = await repository.ObtenerActivasAsync();

        return unidades.Select(u => new UnidadMedidaDto
        {
            Id = u.Id_UnidadMedida,
            Nombre = u.Nombre,
            Abreviatura = u.Abreviatura
        }).ToList();
    }

    public async Task CrearAsync(CrearUnidadMedidaDto dto)
    {
        UnidadMedida unidad = new UnidadMedida(dto.Nombre, dto.Abreviatura);
        await repository.AgregarAsync(unidad);
    }

    public async Task EditarAsync(int id, CrearUnidadMedidaDto dto)
    {
        UnidadMedida? unidad = await repository.ObtenerPorIdAsync(id);

        if (unidad == null)
        {
            throw new Exception("Unidad no encontrada");
        }

        unidad.Actualizar(dto.Nombre, dto.Abreviatura);
        await repository.ActualizarAsync(unidad);
    }
}

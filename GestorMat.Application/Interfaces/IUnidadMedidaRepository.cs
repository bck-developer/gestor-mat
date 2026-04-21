using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface IUnidadMedidaRepository
{
    Task<List<UnidadMedida>> ObtenerTodosAsync();
    Task<UnidadMedida?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(UnidadMedida unidad);
    Task EditarAsync(UnidadMedida unidad);
    Task EliminarAsync(UnidadMedida unidad);

}

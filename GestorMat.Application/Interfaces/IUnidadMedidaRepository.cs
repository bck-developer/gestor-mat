using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface IUnidadMedidaRepository
{
    Task<List<UnidadMedida>> ObtenerActivasAsync();
    Task<UnidadMedida?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(UnidadMedida unidad);
    Task ActualizarAsync(UnidadMedida unidad);
}

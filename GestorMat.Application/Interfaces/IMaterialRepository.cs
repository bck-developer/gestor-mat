using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface IMaterialRepository
{
    Task AgregarAsync(Material material);
    Task AgregarRangoAsync(IEnumerable<Material> materiales);
    Task<IEnumerable<Material>> ObtenerTodosAsync();
}

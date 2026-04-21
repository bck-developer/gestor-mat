using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface IMaterialRepository
{
    Task AgregarAsync(Material material);
    Task<List<Material>> ObtenerTodosAsync();
    Task<Material?> ObtenerPorIdAsync(int id);
    Task EditarAsync(Material material);
    Task EliminarAsync(Material material);
}

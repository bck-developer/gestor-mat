using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface IRolRepository
{
    Task<List<Rol>> ObtenerTodosAsync();
}

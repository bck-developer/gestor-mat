using GestorMat.Application.DTOs;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorUsernameAsync(string username);
    Task AgregarAsync(Usuario usuario);
    Task<List<Usuario>> ObtenerTodosAsync();
    Task<Usuario?> ObtenerPorIdAsync(int id);
    Task EditarAsync(Usuario usuario);

    Task EliminarFisicoAsync(Usuario usuario);
}

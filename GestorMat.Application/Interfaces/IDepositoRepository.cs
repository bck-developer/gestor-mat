using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface IDepositoRepository
{
    Task<IEnumerable<Deposito>> ObtenerTodosAsync();
    Task<Deposito?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Deposito deposito);
    Task ActualizarAsync(Deposito deposito);
    Task EliminarAsync(int id);
}

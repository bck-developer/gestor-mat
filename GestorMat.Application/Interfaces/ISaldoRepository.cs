using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface ISaldoRepository
{
    Task<Saldo?> ObtenerAsync(int idMaterial, int idDeposito);
    Task AgregarAsync(Saldo saldo);
    Task ActualizarAsync(Saldo saldo);
}

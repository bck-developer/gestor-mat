using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using GestorMat.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Repositorios;

public class SaldoRepository(AppDbContext context) : ISaldoRepository
{
    public async Task<Saldo?> ObtenerAsync(int idMaterial, int idDeposito) =>

         await context.Saldos
            .FirstOrDefaultAsync(x =>
                x.Id_Material == idMaterial &&
                x.Id_Deposito == idDeposito);


    public async Task AgregarAsync(Saldo saldo) => await context.Saldos.AddAsync(saldo);

    public Task ActualizarAsync(Saldo saldo)
    {
        context.Saldos.Update(saldo);
        return Task.CompletedTask;
    }
}

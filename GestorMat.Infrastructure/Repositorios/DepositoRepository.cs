using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using GestorMat.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Repositorios;

public class DepositoRepository(AppDbContext context) : IDepositoRepository
{
    public async Task<IEnumerable<Deposito>> ObtenerTodosAsync()
    {
        IEnumerable<Deposito> depositos = await context.Depositos
            .AsNoTracking()
            .ToListAsync();

        return depositos;
    }

    public async Task<Deposito?> ObtenerPorIdAsync(int id)
    {
        Deposito? deposito = await context.Depositos
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id_Deposito == id);

        return deposito;
    }

    public async Task AgregarAsync(Deposito deposito)
    {
        context.Depositos.Add(deposito);
        await context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Deposito deposito)
    {
        context.Depositos.Update(deposito);
        await context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        Deposito? deposito = await context.Depositos.FindAsync(id);

        if (deposito != null)
        {
            context.Depositos.Remove(deposito);
            await context.SaveChangesAsync();
        }
    }
}

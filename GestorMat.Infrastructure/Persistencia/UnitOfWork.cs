using GestorMat.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace GestorMat.Infrastructure.Persistencia;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync() => _transaction = await context.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
        await _transaction!.CommitAsync();
    }

    public async Task RollbackAsync() => await context.SaveChangesAsync();
}

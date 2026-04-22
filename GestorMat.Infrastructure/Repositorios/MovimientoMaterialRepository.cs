using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using GestorMat.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Repositorios;

public class MovimientoMaterialRepository(AppDbContext context) : IMovimientoMaterialRepository
{
    public async Task AgregarAsync(MovimientoMaterial movimiento) =>
        await context.MovimientosMaterial.AddAsync(movimiento);

    public async Task<List<MovimientoMaterial>> ObtenerTodosAsync() =>
        await context.MovimientosMaterial
            .AsNoTracking()
            .OrderByDescending(x => x.Fecha)
            .ToListAsync();


    public async Task<string?> ObtenerUltimoCodigoAsync() =>
         await context.MovimientosMaterial
            .OrderByDescending(x => x.Id_MovimientoMaterial)
            .Select(x => x.CodigoMovimiento)
            .FirstOrDefaultAsync();


    public async Task<MovimientoMaterial?> ObtenerPorIdAsync(int id) =>
        await context.MovimientosMaterial.FirstOrDefaultAsync(u => u.Id_MovimientoMaterial == id);


    public async Task<MovimientoMaterial?> ObtenerPorCodigoAsync(string codigoMovimiento)
    {
        return await context.MovimientosMaterial
            .FirstOrDefaultAsync(u => u.CodigoMovimiento == codigoMovimiento);
    }
}

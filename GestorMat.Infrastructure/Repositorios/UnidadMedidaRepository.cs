using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using GestorMat.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Repositorios
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {
        private readonly AppDbContext _context;

        public UnidadMedidaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UnidadMedida>> ObtenerActivasAsync()
        {
            return await _context.UnidadesMedida
                .AsNoTracking()
                .Where(u => u.Activo)
                .ToListAsync();
        }

        public async Task<UnidadMedida?> ObtenerPorIdAsync(int id)
        {
            return await _context.UnidadesMedida
                .FirstOrDefaultAsync(u => u.Id_UnidadMedida == id);
        }

        public async Task AgregarAsync(UnidadMedida unidad)
        {
            _context.UnidadesMedida.Add(unidad);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(UnidadMedida unidad)
        {
            _context.UnidadesMedida.Update(unidad);
            await _context.SaveChangesAsync();
        }
    }
}

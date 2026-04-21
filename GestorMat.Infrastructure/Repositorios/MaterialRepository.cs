using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using GestorMat.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Repositorios
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;

        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AgregarAsync(Material Material)
        {
            await _context.Materiales.AddAsync(Material);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Material>> ObtenerTodosAsync()
        {
            return await _context.Materiales
                .Include(u => u.UnidadMedida)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Material?> ObtenerPorIdAsync(int id)
        {
            return await _context.Materiales
                .Include(u => u.UnidadMedida)
                .FirstOrDefaultAsync(u => u.Id_Material == id);
        }

        public async Task EditarAsync(Material Material)
        {
            _context.Materiales.Update(Material);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Material Material)
        {
            _context.Materiales.Remove(Material);
            await _context.SaveChangesAsync();
        }
    }
}

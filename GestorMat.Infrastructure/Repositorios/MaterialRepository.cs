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

        public async Task AgregarAsync(Material material)
        {
            _context.Materiales.Add(material);
            await _context.SaveChangesAsync();
        }

        public async Task AgregarRangoAsync(IEnumerable<Material> materiales)
        {
            _context.Materiales.AddRange(materiales);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Material>> ObtenerTodosAsync()
        {
            return await _context.Materiales
                .AsNoTracking()
                .Include(m => m.UnidadMedida)
                .ToListAsync();
        }
    }
}

using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using GestorMat.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestorMat.Infrastructure.Repositorios
{
    public class UnidadMedidaRepository(AppDbContext context) : IUnidadMedidaRepository
    {
        public async Task AgregarAsync(UnidadMedida unidad)
        {
            context.UnidadesMedida.Add(unidad);
            await context.SaveChangesAsync();
        }

        public async Task<List<UnidadMedida>> ObtenerTodosAsync()
        {
            return await context.UnidadesMedida
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<UnidadMedida?> ObtenerPorIdAsync(int id)
        {
            return await context.UnidadesMedida
                .FirstOrDefaultAsync(u => u.Id_UnidadMedida == id);
        }

        public async Task EditarAsync(UnidadMedida unidad)
        {
            if (!unidad.Activo)
            {
                bool HayMaterialesAsociados = await context.Materiales.AnyAsync(m => m.Id_UnidadMedida == unidad.Id_UnidadMedida);

                if (HayMaterialesAsociados)
                {
                    throw new InvalidOperationException("No se puede desactivar la unidad de medida porque hay materiales asociados.");
                }
            }
            context.UnidadesMedida.Update(unidad);
            await context.SaveChangesAsync();
        }

        public async Task EliminarAsync(UnidadMedida unidad)
        {
            bool HayMaterialesAsociados = await context.Materiales.AnyAsync(m => m.Id_UnidadMedida == unidad.Id_UnidadMedida);

            if (HayMaterialesAsociados)
            {
                throw new InvalidOperationException("No se puede eliminar la unidad de medida porque hay materiales asociados.");
            }

            context.UnidadesMedida.Remove(unidad);
            await context.SaveChangesAsync();
        }
    }
}

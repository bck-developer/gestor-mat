using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Interfaces;

public interface IMovimientoMaterialRepository
{
    Task<List<MovimientoMaterial>> ObtenerTodosAsync();
    Task<string?> ObtenerUltimoCodigoAsync();
    Task AgregarAsync(MovimientoMaterial movimiento);
    Task<MovimientoMaterial?> ObtenerPorIdAsync(int id);
    Task<MovimientoMaterial?> ObtenerPorCodigoAsync(string codigoMovimiento);
}

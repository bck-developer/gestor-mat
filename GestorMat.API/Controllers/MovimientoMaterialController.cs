using GestorMat.Application.DTOs.MovimientoMaterial;
using GestorMat.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

/// <summary>
/// Controlador para la gestión de movimientos de materiales.
/// Permite registrar entradas y salidas de materiales en los depósitos.
/// </summary>
[ApiController]
[Route("api/movimientos")]
[Authorize]
public class MovimientoMaterialController(MovimientoMaterialService service) : ControllerBase
{
    /// <summary>
    /// Crea un nuevo movimiento de material (entrada o salida).
    /// </summary>
    /// <param name="dto">Datos del movimiento a crear</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="200">Movimiento creado exitosamente</response>
    /// <response code="400">Datos inválidos o error en la creación</response>
    /// <response code="401">No autorizado</response>
    [HttpPost]
    public async Task<IActionResult> Crear(CrearMovimientoMaterialDto dto)
    {
        try
        {
            await service.CrearAsync(dto);
            return Ok(new { message = "Movimiento creado exitosamente" });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Obtiene la lista de todos los movimientos de materiales registrados.
    /// </summary>
    /// <returns>Lista de movimientos</returns>
    /// <response code="200">Lista de movimientos obtenida exitosamente</response>
    /// <response code="401">No autorizado</response>
    [HttpGet]
    public async Task<IActionResult> Obtener()
    {
        List<MovimientoMaterialDto> movimientos = await service.ObtenerTodosAsync();
        return Ok(movimientos);
    }

    /// <summary>
    /// Obtiene un movimiento específico por su ID.
    /// </summary>
    /// <param name="id">ID del movimiento a obtener</param>
    /// <returns>Datos del movimiento solicitado</returns>
    /// <response code="200">Movimiento obtenido exitosamente</response>
    /// <response code="401">No autorizado</response>
    /// <response code="404">Movimiento no encontrado</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        MovimientoMaterial? movimientoMaterial = await service.ObtenerPorIdAsyncService(id);

        if (movimientoMaterial == null)
        {
            return NotFound(new { message = "Movimiento no encontrado" });
        }

        return Ok(movimientoMaterial);
    }

    /// <summary>
    /// Verifica si existe un movimiento con un código específico.
    /// </summary>
    /// <param name="codigo">Código del movimiento a buscar</param>
    /// <returns>Datos del movimiento si existe</returns>
    /// <response code="200">Movimiento encontrado</response>
    /// <response code="401">No autorizado</response>
    /// <response code="404">Movimiento no encontrado</response>
    [HttpGet("existe-codigo/{codigo}")]
    public async Task<IActionResult> ObtenerPorCodigoAsync(string codigo)
    {
        MovimientoMaterial? movimientoMaterial = await service.ObtenerPorCodigoAsync(codigo);

        if (movimientoMaterial == null)
        {
            return NotFound(new { message = "Movimiento no encontrado" });
        }

        return Ok(movimientoMaterial);
    }
}

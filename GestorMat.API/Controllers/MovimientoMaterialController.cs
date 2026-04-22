using GestorMat.Application.DTOs.MovimientoMaterial;
using GestorMat.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

[ApiController]
[Route("api/movimientos")]
[Authorize]
public class MovimientoMaterialController(MovimientoMaterialService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Crear(CrearMovimientoMaterialDto dto)
    {
        try
        {
            await service.CrearAsync(dto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Obtener()
    {
        List<MovimientoMaterialDto> lista = await service.ObtenerTodosAsync();
        return Ok(lista);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        MovimientoMaterial? movMaterial = await service.ObtenerPorIdAsyncService(id);

        if (movMaterial == null)
        {
            return NotFound();
        }

        return Ok(movMaterial);
    }


    [HttpGet("existe-codigo/{codigo}")]
    public async Task<IActionResult> ObtenerPorCodigoAsync(string codigo)
    {
        MovimientoMaterial? movMaterial = await service.ObtenerPorCodigoAsync(codigo);

        if (movMaterial == null)
        {
            return NotFound();
        }

        return Ok(movMaterial);
    }
}

using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepositoController(DepositoService depositoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenerDepositos()
    {
        try
        {
            List<DepositoDto> depositos = await depositoService.ObtenerDepositosAsync();
            return Ok(depositos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerDepositoPorId(int id)
    {
        try
        {
            DepositoDto? deposito = await depositoService.ObtenerDepositoPorIdAsync(id);

            if (deposito == null)
            {
                return NotFound(new { message = "Depósito no encontrado" });
            }

            return Ok(deposito);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CrearDeposito([FromBody] CrearDepositoDto dto)
    {
        try
        {
            await depositoService.CrearDepositoAsync(dto);
            return Created("", "Depósito creado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditarDeposito(int id, [FromBody] CrearDepositoDto dto)
    {
        try
        {
            await depositoService.EditarDepositoAsync(id, dto);
            return Ok("Depósito actualizado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarDeposito(int id)
    {
        try
        {
            await depositoService.EliminarDepositoAsync(id);
            return Ok("Depósito eliminado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/inhabilitar")]
    public async Task<IActionResult> InhabilitarDeposito(int id)
    {
        try
        {
            await depositoService.InhabilitarDepositoAsync(id);
            return Ok("Depósito inhabilitado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/habilitar")]
    public async Task<IActionResult> HabilitarDeposito(int id)
    {
        try
        {
            await depositoService.HabilitarDepositoAsync(id);
            return Ok("Depósito habilitado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

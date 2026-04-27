using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

/// <summary>
/// Controlador para la gestión de depósitos.
/// Permite crear, leer, actualizar y eliminar depósitos del sistema.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepositoController(DepositoService depositoService) : ControllerBase
{
    /// <summary>
    /// Obtiene la lista de todos los depósitos disponibles.
    /// </summary>
    /// <returns>Lista de depósitos</returns>
    /// <response code="200">Lista de depósitos obtenida exitosamente</response>
    /// <response code="400">Error al obtener los depósitos</response>
    /// <response code="401">No autorizado</response>
    [HttpGet]
    public async Task<IActionResult> ObtenerDepositos()
    {
        try
        {
            List<DepositoDto> depositos = await depositoService.ObtenerDepositosAsync();
            return Ok(depositos);
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Obtiene un depósito específico por su ID.
    /// </summary>
    /// <param name="id">ID del depósito a obtener</param>
    /// <returns>Datos del depósito solicitado</returns>
    /// <response code="200">Depósito obtenido exitosamente</response>
    /// <response code="401">No autorizado</response>
    /// <response code="404">Depósito no encontrado</response>
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
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo depósito en el sistema.
    /// </summary>
    /// <param name="dto">Datos del depósito a crear</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="201">Depósito creado exitosamente</response>
    /// <response code="400">Datos inválidos o error en la creación</response>
    /// <response code="401">No autorizado</response>
    [HttpPost]
    public async Task<IActionResult> CrearDeposito([FromBody] CrearDepositoDto dto)
    {
        try
        {
            await depositoService.CrearDepositoAsync(dto);
            return Created(string.Empty, new { message = "Depósito creado exitosamente" });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Actualiza los datos de un depósito existente.
    /// </summary>
    /// <param name="id">ID del depósito a actualizar</param>
    /// <param name="dto">Nuevos datos del depósito</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="200">Depósito actualizado exitosamente</response>
    /// <response code="400">Datos inválidos o error en la actualización</response>
    /// <response code="401">No autorizado</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> EditarDeposito(int id, [FromBody] CrearDepositoDto dto)
    {
        try
        {
            await depositoService.EditarDepositoAsync(id, dto);
            return Ok(new { message = "Depósito actualizado exitosamente" });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Elimina un depósito del sistema de forma física.
    /// </summary>
    /// <param name="id">ID del depósito a eliminar</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="200">Depósito eliminado exitosamente</response>
    /// <response code="400">Error en la eliminación</response>
    /// <response code="401">No autorizado</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarDeposito(int id)
    {
        try
        {
            await depositoService.EliminarDepositoAsync(id);
            return Ok(new { message = "Depósito eliminado exitosamente" });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Inhabilita un depósito (lo marca como no disponible para usar).
    /// </summary>
    /// <param name="id">ID del depósito a inhabilitar</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="200">Depósito inhabilitado exitosamente</response>
    /// <response code="400">Error en la inhabilitación</response>
    /// <response code="401">No autorizado</response>
    [HttpPatch("{id}/inhabilitar")]
    public async Task<IActionResult> InhabilitarDeposito(int id)
    {
        try
        {
            await depositoService.InhabilitarDepositoAsync(id);
            return Ok(new { message = "Depósito inhabilitado exitosamente" });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Habilita un depósito (lo marca como disponible para usar).
    /// </summary>
    /// <param name="id">ID del depósito a habilitar</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="200">Depósito habilitado exitosamente</response>
    /// <response code="400">Error en la habilitación</response>
    /// <response code="401">No autorizado</response>
    [HttpPatch("{id}/habilitar")]
    public async Task<IActionResult> HabilitarDeposito(int id)
    {
        try
        {
            await depositoService.HabilitarDepositoAsync(id);
            return Ok(new { message = "Depósito habilitado exitosamente" });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }
}

using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

/// <summary>
/// Controlador para la gestión de materiales.
/// Proporciona endpoints para crear, leer, actualizar y eliminar materiales.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaterialController(MaterialService service) : ControllerBase
{
    /// <summary>
    /// Obtiene la lista de todos los materiales disponibles.
    /// </summary>
    /// <returns>Lista de materiales</returns>
    /// <response code="200">Lista de materiales obtenida exitosamente</response>
    /// <response code="401">No autorizado</response>
    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        List<MaterialDto> materiales = await service.ObtenerTodosAsyncService();
        return Ok(materiales);
    }

    /// <summary>
    /// Obtiene un material específico por su ID.
    /// </summary>
    /// <param name="id">ID del material a obtener</param>
    /// <returns>Datos del material solicitado</returns>
    /// <response code="200">Material obtenido exitosamente</response>
    /// <response code="401">No autorizado</response>
    /// <response code="404">Material no encontrado</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        MaterialDto? material = await service.ObtenerPorIdAsyncService(id);

        if (material == null)
            return NotFound(new { message = "Material no encontrado" });

        return Ok(material);
    }

    /// <summary>
    /// Crea un nuevo material en el sistema.
    /// </summary>
    /// <param name="dto">Datos del material a crear</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="200">Material creado exitosamente</response>
    /// <response code="400">Datos inválidos o error en la creación</response>
    /// <response code="401">No autorizado</response>
    [HttpPost]
    public async Task<IActionResult> Crear(CrearMaterialDto dto)
    {
        if (dto.Id_UnidadMedida <= 0)
            return BadRequest(new { message = "Unidad de medida inválida" });

        try
        {
            await service.CrearAsyncService(dto);
            return Ok(new { message = "Material creado exitosamente" });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Actualiza los datos de un material existente.
    /// </summary>
    /// <param name="id">ID del material a actualizar</param>
    /// <param name="dto">Nuevos datos del material</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="200">Material actualizado exitosamente</response>
    /// <response code="400">Datos inválidos o error en la actualización</response>
    /// <response code="401">No autorizado</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarMaterialDto dto)
    {
        try
        {
            await service.ActualizarAsyncService(id, dto);
            return Ok(new { message = "Material actualizado exitosamente" });
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }

    /// <summary>
    /// Elimina un material del sistema.
    /// </summary>
    /// <param name="id">ID del material a eliminar</param>
    /// <returns>Respuesta del servidor</returns>
    /// <response code="204">Material eliminado exitosamente</response>
    /// <response code="400">Error en la eliminación</response>
    /// <response code="401">No autorizado</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await service.EliminarAsyncService(id);
            return NoContent();
        }
        catch (Exception excepcion)
        {
            return BadRequest(new { message = excepcion.Message });
        }
    }
}

using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using GestorMat.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de unidades de medida.
    /// Permite crear, leer, actualizar y eliminar unidades de medida del sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UnidadMedidaController(UnidadMedidaService service) : ControllerBase
    {
        /// <summary>
        /// Crea una nueva unidad de medida en el sistema.
        /// </summary>
        /// <param name="dto">Datos de la unidad de medida a crear</param>
        /// <returns>Respuesta del servidor</returns>
        /// <response code="200">Unidad de medida creada exitosamente</response>
        /// <response code="400">Datos inválidos o error en la creación</response>
        /// <response code="401">No autorizado</response>
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUnidadMedidaDto dto)
        {
            try
            {
                await service.CrearAsyncService(dto);
                return Ok(new { message = "Unidad de medida creada exitosamente" });
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }

        /// <summary>
        /// Obtiene la lista de unidades de medida disponibles.
        /// </summary>
        /// <param name="soloActivos">Filtro para obtener solo unidades activas (opcional)</param>
        /// <returns>Lista de unidades de medida</returns>
        /// <response code="200">Lista de unidades obtenida exitosamente</response>
        /// <response code="400">Error al obtener las unidades</response>
        /// <response code="401">No autorizado</response>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(bool soloActivos = false)
        {
            try
            {
                List<UnidadMedidaDto> unidades = await service.ObtenerTodosAsyncService(soloActivos);
                return Ok(unidades);
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }

        /// <summary>
        /// Obtiene una unidad de medida específica por su ID.
        /// </summary>
        /// <param name="id">ID de la unidad de medida a obtener</param>
        /// <returns>Datos de la unidad de medida solicitada</returns>
        /// <response code="200">Unidad de medida obtenida exitosamente</response>
        /// <response code="401">No autorizado</response>
        /// <response code="404">Unidad de medida no encontrada</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            UnidadMedida? unidadmedida = await service.ObtenerPorIdAsyncService(id);

            if (unidadmedida == null)
            {
                return NotFound(new { message = "Unidad de medida no encontrada" });
            }

            UnidadMedidaDto unidadDto = new UnidadMedidaDto
            {
                Id_UnidadMedida = unidadmedida.Id_UnidadMedida,
                Nombre = unidadmedida.Nombre,
                Abreviatura = unidadmedida.Abreviatura,
                Activo = unidadmedida.Activo
            };

            return Ok(unidadDto);
        }

        /// <summary>
        /// Actualiza los datos de una unidad de medida existente.
        /// </summary>
        /// <param name="id">ID de la unidad de medida a actualizar</param>
        /// <param name="dto">Nuevos datos de la unidad de medida</param>
        /// <returns>Respuesta del servidor</returns>
        /// <response code="200">Unidad de medida actualizada exitosamente</response>
        /// <response code="400">Datos inválidos o error en la actualización</response>
        /// <response code="401">No autorizado</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, CrearUnidadMedidaDto dto)
        {
            try
            {
                await service.ActualizarAsyncService(id, dto);
                return Ok(new { message = "Unidad de medida actualizada exitosamente" });
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }

        /// <summary>
        /// Elimina una unidad de medida del sistema de forma física.
        /// </summary>
        /// <param name="id">ID de la unidad de medida a eliminar</param>
        /// <returns>Respuesta del servidor</returns>
        /// <response code="204">Unidad de medida eliminada exitosamente</response>
        /// <response code="400">Error en la eliminación</response>
        /// <response code="401">No autorizado</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await service.EliminarFisicoAsync(id);
                return NoContent();
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }
    }
}

using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using GestorMat.Domain.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadMedidaController(UnidadMedidaService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUnidadMedidaDto dto)
        {
            try
            {
                await service.CrearAsyncService(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                return Ok(await service.ObtenerTodosAsyncService());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            UnidadMedida? unidadmedida = await service.ObtenerPorIdAsyncService(id);

            if (unidadmedida == null)
            {
                return NotFound();
            }

            return Ok(new UnidadMedidaDto
            {
                Id_UnidadMedida = unidadmedida.Id_UnidadMedida,
                Nombre = unidadmedida.Nombre,
                Abreviatura = unidadmedida.Abreviatura,
                Activo = unidadmedida.Activo
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, CrearUnidadMedidaDto dto)
        {

            try
            {
                await service.ActualizarAsyncService(id, dto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await service.EliminarFisicoAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}

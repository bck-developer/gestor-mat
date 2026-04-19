using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuariosController(UsuarioService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioDto dto)
        {
            try
            {
                await _service.CrearAsync(dto);
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
            try
            {
                return Ok(await _service.ObtenerAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var usuario = await _service.ObtenerEntidadPorIdAsync(id);

            if (usuario == null)
                return NotFound();

            return Ok(new UsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Mail = usuario.Mail,
                IdRol = usuario.IdRol
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int id, CrearUsuarioDto dto)
        {
            await _service.EditarAsync(id, dto);
            return Ok();
        }

        // INHABILITAR
        [HttpPatch("{id}/inhabilitar")]
        public async Task<IActionResult> Inhabilitar(int id)
        {
            await _service.InhabilitarAsync(id);
            return Ok();
        }

        // REHABILITAR
        [HttpPatch("{id}/rehabilitar")]
        public async Task<IActionResult> Rehabilitar(int id)
        {
            await _service.RehabilitarAsync(id);
            return Ok();
        }

        // ELIMINAR FISICO (opcional)
        [HttpDelete("{id}/fisico")]
        public async Task<IActionResult> EliminarFisico(int id)
        {
            await _service.EliminarFisicoAsync(id);
            return NoContent();
        }
    }
}

using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using GestorMat.Domain.Entidades;
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
                await _service.CrearAsyncService(dto);
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
                return Ok(await _service.ObtenerTodosAsyncService());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            Usuario? usuario = await _service.ObtenerPorIdAsyncService(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(new UsuarioDto
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Mail = usuario.Mail,
                IdRol = usuario.IdRol,
                Activo = usuario.Activo
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, CrearUsuarioDto dto)
        {
            await _service.ActualizarAsyncService(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarFisicoAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/inhabilitar")]
        public async Task<IActionResult> Inhabilitar(int id)
        {
            await _service.InhabilitarAsyncService(id);
            return Ok();
        }

        [HttpPatch("{id}/rehabilitar")]
        public async Task<IActionResult> Rehabilitar(int id)
        {
            await _service.RehabilitarAsyncService(id);
            return Ok();
        }
    }
}

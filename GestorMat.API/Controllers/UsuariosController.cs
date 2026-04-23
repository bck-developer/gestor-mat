using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using GestorMat.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "admin")]
    public class UsuariosController(UsuarioService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Crear(CrearUsuarioDto dto)
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
            Usuario? usuario = await service.ObtenerPorIdAsyncService(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(new UsuarioDto
            {
                Id_Usuario = usuario.Id_Usuario,
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
            await service.ActualizarAsyncService(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await service.EliminarFisicoAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/inhabilitar")]
        public async Task<IActionResult> Inhabilitar(int id)
        {
            await service.InhabilitarAsyncService(id);
            return Ok();
        }

        [HttpPatch("{id}/rehabilitar")]
        public async Task<IActionResult> Rehabilitar(int id)
        {
            await service.RehabilitarAsyncService(id);
            return Ok();
        }
    }
}

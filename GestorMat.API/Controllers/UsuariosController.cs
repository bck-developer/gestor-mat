using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using GestorMat.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de usuarios del sistema.
    /// Solo accesible por usuarios con rol "admin".
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class UsuariosController(UsuarioService service) : ControllerBase
    {
        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="dto">Datos del usuario a crear</param>
        /// <returns>Respuesta del servidor</returns>
        /// <response code="200">Usuario creado exitosamente</response>
        /// <response code="400">Datos inválidos o error en la creación</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Permiso denegado (requiere rol admin)</response>
        [HttpPost]
        public async Task<IActionResult> Crear(CrearUsuarioDto dto)
        {
            try
            {
                await service.CrearAsyncService(dto);
                return Ok(new { message = "Usuario creado exitosamente" });
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }

        /// <summary>
        /// Obtiene la lista de todos los usuarios del sistema.
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        /// <response code="200">Lista de usuarios obtenida exitosamente</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Permiso denegado (requiere rol admin)</response>
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                List<UsuarioDto> usuarios = await service.ObtenerTodosAsyncService();
                return Ok(usuarios);
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }

        /// <summary>
        /// Obtiene un usuario específico por su ID.
        /// </summary>
        /// <param name="id">ID del usuario a obtener</param>
        /// <returns>Datos del usuario solicitado</returns>
        /// <response code="200">Usuario obtenido exitosamente</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Permiso denegado (requiere rol admin)</response>
        /// <response code="404">Usuario no encontrado</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            Usuario? usuario = await service.ObtenerPorIdAsyncService(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            UsuarioDto usuarioDto = new UsuarioDto
            {
                Id_Usuario = usuario.Id_Usuario,
                Username = usuario.Username,
                Nombre = usuario.Nombre,
                Mail = usuario.Mail,
                Rol = usuario.Rol?.RolName ?? string.Empty,
                Activo = usuario.Activo
            };

            return Ok(usuarioDto);
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        /// <param name="id">ID del usuario a actualizar</param>
        /// <param name="dto">Nuevos datos del usuario</param>
        /// <returns>Respuesta del servidor</returns>
        /// <response code="200">Usuario actualizado exitosamente</response>
        /// <response code="400">Datos inválidos o error en la actualización</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Permiso denegado (requiere rol admin)</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, CrearUsuarioDto dto)
        {
            try
            {
                await service.ActualizarAsyncService(id, dto);
                return Ok(new { message = "Usuario actualizado exitosamente" });
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }

        /// <summary>
        /// Elimina un usuario del sistema de forma física.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>Respuesta del servidor</returns>
        /// <response code="204">Usuario eliminado exitosamente</response>
        /// <response code="400">Error en la eliminación</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Permiso denegado (requiere rol admin)</response>
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

        /// <summary>
        /// Inhabilita un usuario (lo marca como inactivo).
        /// </summary>
        /// <param name="id">ID del usuario a inhabilitar</param>
        /// <returns>Respuesta del servidor</returns>
        /// <response code="200">Usuario inhabilitado exitosamente</response>
        /// <response code="400">Error en la inhabilitación</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Permiso denegado (requiere rol admin)</response>
        [HttpPatch("{id}/inhabilitar")]
        public async Task<IActionResult> Inhabilitar(int id)
        {
            try
            {
                await service.InhabilitarAsyncService(id);
                return Ok(new { message = "Usuario inhabilitado exitosamente" });
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }

        /// <summary>
        /// Rehabilita un usuario (lo marca como activo).
        /// </summary>
        /// <param name="id">ID del usuario a rehabilitar</param>
        /// <returns>Respuesta del servidor</returns>
        /// <response code="200">Usuario rehabilitado exitosamente</response>
        /// <response code="400">Error en la rehabilitación</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Permiso denegado (requiere rol admin)</response>
        [HttpPatch("{id}/rehabilitar")]
        public async Task<IActionResult> Rehabilitar(int id)
        {
            try
            {
                await service.RehabilitarAsyncService(id);
                return Ok(new { message = "Usuario rehabilitado exitosamente" });
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }
    }
}

using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    /// <summary>
    /// Controlador para la gestión de roles del sistema.
    /// Solo accesible por usuarios con rol "admin".
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class RolesController : ControllerBase
    {
        private readonly RolService _service;

        /// <summary>
        /// Crea una nueva instancia del controlador de Roles.
        /// </summary>
        /// <param name="service">Servicio de roles</param>
        public RolesController(RolService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene la lista de todos los roles disponibles en el sistema.
        /// </summary>
        /// <returns>Lista de roles</returns>
        /// <response code="200">Lista de roles obtenida exitosamente</response>
        /// <response code="400">Error al obtener los roles</response>
        /// <response code="401">No autorizado</response>
        /// <response code="403">Permiso denegado (requiere rol admin)</response>
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            try
            {
                List<RolDto> roles = await _service.ObtenerAsync();
                return Ok(roles);
            }
            catch (Exception excepcion)
            {
                return BadRequest(new { message = excepcion.Message });
            }
        }
    }
}

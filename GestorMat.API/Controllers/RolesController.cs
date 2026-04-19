using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RolService _service;

        public RolesController(RolService service)
        {
            _service = service;
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
    }
}

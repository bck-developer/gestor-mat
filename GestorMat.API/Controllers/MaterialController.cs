using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Servicios;
using GestorMat.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly MaterialService _service;

        public MaterialController(MaterialService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
            => Ok(await _service.ObtenerTodosAsyncService());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            MaterialDto? material = await _service.ObtenerPorIdAsyncService(id);

            if (material == null)
                return NotFound();

            return Ok(material);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearMaterialDto dto)
        {
            if (dto.Id_UnidadMedida <= 0)
                return BadRequest("Unidad de medida inválida");

            await _service.CrearAsyncService(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, ActualizarMaterialDto dto)
        {
            await _service.ActualizarAsyncService(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsyncService(id);
            return NoContent();
        }
    }
}

using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadMedidaController : ControllerBase
    {
        private readonly UnidadMedidaService _service;

        public UnidadMedidaController(UnidadMedidaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var unidades = await _service.ObtenerTodasAsync();
                return Ok(unidades);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CrearUnidadMedidaDto dto)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CrearUnidadMedidaDto dto)
        {
            try
            {
                await _service.EditarAsync(id, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

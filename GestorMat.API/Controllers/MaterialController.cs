using GestorMat.Application.DTOs;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly MaterialService _materialService;
        private readonly XmlService _xmlService;

        public MaterialController(MaterialService materialService, XmlService xmlService)
        {
            _materialService = materialService;
            _xmlService = xmlService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CrearMaterial([FromBody] CrearMaterialDto dto)
        {
            try
            {
                await _materialService.CrearMaterialAsync(dto);
                return Created("", "Material creado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObtenerMateriales()
        {
            try
            {
                var materiales = await _materialService.ObtenerMaterialesAsync();
                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("importar-xml")]
        [Authorize]
        public async Task<IActionResult> ImportarXml(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo inválido");

            try
            {
                using var stream = archivo.OpenReadStream();
                var materialesXml = _xmlService.LeerMaterialesDesdeXml(stream);

                var dtos = materialesXml.Select(dto => new CrearMaterialDto
                {
                    Nombre = dto.Nombre,
                    Precio = dto.Precio,
                    IdUnidadMedida = dto.IdUnidadMedida
                });

                await _materialService.ImportarVariosAsync(dtos);
                return Ok("Materiales importados correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

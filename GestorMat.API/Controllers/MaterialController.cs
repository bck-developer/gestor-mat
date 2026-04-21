using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Interfaces;
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
        private readonly MaterialService _service;
        private readonly IExcelService _excelService;
        private readonly MaterialImportService _importService;
        private readonly UnidadMedidaService _unidadService;

        public MaterialController(
            MaterialService service,
            IExcelService excelService,
            MaterialImportService importService,
            UnidadMedidaService unidadService)
        {
            _service = service;
            _excelService = excelService;
            _importService = importService;
            _unidadService = unidadService;
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


        [HttpGet("plantilla")]
        public async Task<IActionResult> DescargarPlantilla()
        {
            var unidades = await _unidadService.ObtenerTodosAsyncService();
            var nombres = unidades.Select(u => u.Nombre).ToList();

            var file = _excelService.GenerarPlantillaMateriales(nombres);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PlantillaMateriales.xlsx"
            );
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Importar(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var rows = _excelService.LeerExcel(stream);

            var items = rows.Select(r => new ImportarMaterialDto
            {
                CodigoMaterial = r["CodigoMaterial"],
                Nombre = r["Nombre"],
                Precio = decimal.TryParse(r["Precio"], out var p) ? p : 0,
                UnidadMedida = r["UnidadMedida"],
                Descripcion = r["Descripcion"],
                Activo = r["Activo"] == "true",
                PermiteStockNegativo = r["PermiteStockNegativo"] == "true",
                StockMinimo = double.TryParse(r["StockMinimo"], out var s) ? s : 0
            }).ToList();

            var resultado = await _importService.ImportarAsync(items);

            return Ok(resultado);
        }
    }
}

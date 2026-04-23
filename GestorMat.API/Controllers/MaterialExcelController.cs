using GestorMat.Application.DTOs;
using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Interfaces;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

[ApiController]
[Route("api/material-excel")]
//[Authorize]
public class MaterialExcelController(
    IExcelService excelService,
    MaterialImportService importService,
    UnidadMedidaService unidadService) : ControllerBase
{
    public const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string fileDownloadName = "PlantillaMateriales.xlsx";

    [HttpGet("plantilla")]
    public async Task<IActionResult> DescargarPlantilla()
    {
        List<UnidadMedidaDto> unidades = await unidadService.ObtenerTodosAsyncService(true);
        List<string> nombres = unidades.Select(u => u.Nombre).ToList();

        byte[] file = excelService.GenerarPlantillaMateriales(nombres);

        return File(file, contentType, fileDownloadName);
    }

    [HttpPost("importar")]
    public async Task<IActionResult> ImportarExcel()
    {
        IFormFile? file = Request.Form.Files.FirstOrDefault();

        if (file == null || file.Length == 0)
        {
            return BadRequest("Archivo inválido");
        }

        using Stream stream = file.OpenReadStream();

        List<MaterialExcelRowDto> filas = excelService.LeerExcelMateriales(stream);

        MaterialImportResultDto resultado = await importService.ImportarAsync(filas);

        return Ok(resultado);
    }
}
using GestorMat.Application.DTOs;
using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Interfaces;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

/// <summary>
/// Controlador para importar y exportar materiales en formato Excel.
/// Permite descargar plantillas y cargar datos masivos de materiales.
/// </summary>
[ApiController]
[Route("api/material-excel")]
[Authorize]
[ApiExplorerSettings(IgnoreApi = true)]
public class MaterialExcelController(
    IExcelService excelService,
    MaterialImportService importService,
    UnidadMedidaService unidadService) : ControllerBase
{
    private const string TipoContenido = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private const string NombreArchivo = "PlantillaMateriales.xlsx";

    /// <summary>
    /// Descarga una plantilla Excel para importar materiales.
    /// La plantilla incluye columnas predefinidas y validaciones.
    /// </summary>
    /// <returns>Archivo Excel con la plantilla</returns>
    [HttpGet("plantilla")]
    public async Task<IActionResult> DescargarPlantilla()
    {
        List<UnidadMedidaDto> unidades = await unidadService.ObtenerTodosAsyncService(true);
        List<string> nombresUnidades = unidades.Select(unidad => unidad.Nombre).ToList();

        byte[] archivos = excelService.GenerarPlantillaMateriales(nombresUnidades);

        return File(archivos, TipoContenido, NombreArchivo);
    }

    /// <summary>
    /// Importa materiales desde un archivo Excel.
    /// Valida los datos y reporta errores en las filas problemáticas.
    /// </summary>
    /// <returns>Resultado de la importación con resumen de éxitos y errores</returns>
    [HttpPost("importar")]
    public async Task<IActionResult> ImportarExcel()
    {
        IFormFile? archivo = Request.Form.Files.FirstOrDefault();

        if (archivo == null || archivo.Length == 0)
        {
            return BadRequest(new { message = "Archivo inválido o no proporcionado" });
        }

        using Stream flujo = archivo.OpenReadStream();

        List<MaterialExcelRowDto> filas = excelService.LeerExcelMateriales(flujo);

        MaterialImportResultDto resultado = await importService.ImportarAsync(filas);

        return Ok(resultado);
    }
}

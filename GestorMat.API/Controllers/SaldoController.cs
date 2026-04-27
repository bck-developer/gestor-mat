using GestorMat.Application.DTOs.Saldo;
using GestorMat.Application.Interfaces;
using GestorMat.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.Api.Controllers;

/// <summary>
/// Controlador para consultar saldos de materiales en depósitos.
/// Proporciona endpoints para consultar y exportar reportes de saldos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SaldoController(
    ISaldoRepository saldoRepository,
    IPdfSaldoService pdfService) : ControllerBase
{
    /// <summary>
    /// Obtiene los saldos de materiales con filtros opcionales.
    /// </summary>
    /// <param name="idMaterial">Filtro por ID de material (opcional)</param>
    /// <param name="idDeposito">Filtro por ID de depósito (opcional)</param>
    /// <param name="incluirMaterialesInactivos">Incluir materiales inactivos en el resultado (default: false)</param>
    /// <param name="incluirDepositosInactivos">Incluir depósitos inactivos en el resultado (default: false)</param>
    /// <returns>Lista de saldos</returns>
    /// <response code="200">Saldos obtenidos exitosamente</response>
    /// <response code="401">No autorizado</response>
    [HttpGet]
    public async Task<ActionResult<List<SaldoDto>>> Obtener(
        [FromQuery] int? idMaterial,
        [FromQuery] int? idDeposito,
        [FromQuery] bool incluirMaterialesInactivos = false,
        [FromQuery] bool incluirDepositosInactivos = false)
    {
        SaldoQuery consulta = new()
        {
            IdMaterial = idMaterial,
            IdDeposito = idDeposito,
            IncluirMaterialesInactivos = incluirMaterialesInactivos,
            IncluirDepositosInactivos = incluirDepositosInactivos
        };

        List<SaldoDto> resultado = await saldoRepository.ObtenerSaldos(consulta);

        return Ok(resultado);
    }

    /// <summary>
    /// Exporta un reporte de saldos en formato PDF.
    /// </summary>
    /// <param name="idMaterial">Filtro por ID de material (opcional)</param>
    /// <param name="idDeposito">Filtro por ID de depósito (opcional)</param>
    /// <param name="incluirMaterialesInactivos">Incluir materiales inactivos en el reporte (default: false)</param>
    /// <param name="incluirDepositosInactivos">Incluir depósitos inactivos en el reporte (default: false)</param>
    /// <returns>Archivo PDF con el reporte de saldos</returns>
    /// <response code="200">Reporte PDF generado exitosamente</response>
    /// <response code="400">No hay datos para generar el reporte</response>
    /// <response code="401">No autorizado</response>
    /// <response code="500">Error generando el PDF</response>
    [HttpGet("pdf")]
    public async Task<IActionResult> ExportarPdf(
        [FromQuery] int? idMaterial,
        [FromQuery] int? idDeposito,
        [FromQuery] bool incluirMaterialesInactivos = false,
        [FromQuery] bool incluirDepositosInactivos = false)
    {
        try
        {
            SaldoQuery consulta = new SaldoQuery
            {
                IdMaterial = idMaterial,
                IdDeposito = idDeposito,
                IncluirMaterialesInactivos = incluirMaterialesInactivos,
                IncluirDepositosInactivos = incluirDepositosInactivos
            };

            List<SaldoDto> datos = await saldoRepository.ObtenerSaldos(consulta);

            if (datos == null || datos.Count == 0)
            {
                return BadRequest(new { message = "No hay datos para generar el reporte" });
            }

            string filtros = ConstruirTextoFiltros(
                datos,
                idMaterial,
                idDeposito,
                incluirMaterialesInactivos,
                incluirDepositosInactivos
            );

            byte[] archivoPdf = pdfService.GenerarReporte(datos, filtros);

            return File(archivoPdf, "application/pdf", "ReporteSaldos.pdf");
        }
        catch (Exception excepcion)
        {
            return StatusCode(500, new { message = $"Error generando PDF: {excepcion.Message}" });
        }
    }

    /// <summary>
    /// Construye una cadena de texto descriptiva de los filtros aplicados.
    /// </summary>
    /// <param name="datos">Datos de saldos obtenidos</param>
    /// <param name="idMaterial">ID de material filtrado</param>
    /// <param name="idDeposito">ID de depósito filtrado</param>
    /// <param name="incluirMaterilesInactivos">Indica si se incluyen materiales inactivos</param>
    /// <param name="incluirDepositosInactivos">Indica si se incluyen depósitos inactivos</param>
    /// <returns>Texto descriptivo de los filtros</returns>
    private string ConstruirTextoFiltros(
        List<SaldoDto> datos,
        int? idMaterial,
        int? idDeposito,
        bool incluirMaterilesInactivos,
        bool incluirDepositosInactivos)
    {
        string material = "Todos";
        string deposito = "Todos";

        if (idMaterial.HasValue)
        {
            SaldoDto? item = datos.FirstOrDefault();
            material = item != null ? item.CodigoMaterial : idMaterial.Value.ToString();
        }

        if (idDeposito.HasValue)
        {
            SaldoDto? item = datos.FirstOrDefault();
            deposito = item != null ? item.CodigoDeposito : idDeposito.Value.ToString();
        }

        string estadoMaterial = incluirMaterilesInactivos ? "Incluye inactivos" : "Solo activos";
        string estadoDeposito = incluirDepositosInactivos ? "Incluye inactivos" : "Solo activos";

        return $"Material: {material} | Depósito: {deposito} | {estadoMaterial} | {estadoDeposito}";
    }
}

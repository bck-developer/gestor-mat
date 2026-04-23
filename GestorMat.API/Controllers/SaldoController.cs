using GestorMat.Application.DTOs.Saldo;
using GestorMat.Application.Interfaces;
using GestorMat.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaldoController : ControllerBase
{
    private readonly ISaldoRepository saldoRepository;
    private readonly IPdfSaldoService pdfService;

    public SaldoController(
        ISaldoRepository saldoRepository,
        IPdfSaldoService pdfService)
    {
        this.saldoRepository = saldoRepository;
        this.pdfService = pdfService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SaldoDto>>> Obtener(
        [FromQuery] int? idMaterial,
        [FromQuery] int? idDeposito,
        [FromQuery] bool incluirMaterialesInactivos = false,
        [FromQuery] bool incluirDepositosInactivos = false)
    {
        SaldoQuery query = new()
        {
            IdMaterial = idMaterial,
            IdDeposito = idDeposito,
            IncluirMaterialesInactivos = incluirMaterialesInactivos,
            IncluirDepositosInactivos = incluirDepositosInactivos
        };

        List<SaldoDto> resultado = await saldoRepository.ObtenerSaldos(query);

        return Ok(resultado);
    }

    [HttpGet("pdf")]
    public async Task<IActionResult> ExportarPdf(
        [FromQuery] int? idMaterial,
        [FromQuery] int? idDeposito,
        [FromQuery] bool incluirMaterialesInactivos = false,
        [FromQuery] bool incluirDepositosInactivos = false)
    {
        try
        {
            SaldoQuery query = new()
            {
                IdMaterial = idMaterial,
                IdDeposito = idDeposito,
                IncluirMaterialesInactivos = incluirMaterialesInactivos,
                IncluirDepositosInactivos = incluirDepositosInactivos
            };

            List<SaldoDto> data = await saldoRepository.ObtenerSaldos(query);

            if (data == null || data.Count == 0)
            {
                return BadRequest("No hay datos para generar el reporte");
            }

            string filtros = ConstruirTextoFiltros(
                idMaterial,
                idDeposito,
                incluirMaterialesInactivos,
                incluirDepositosInactivos
            );

            byte[] pdf = pdfService.GenerarReporte(data, filtros);

            return File(pdf, "application/pdf", "ReporteSaldos.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error generando PDF: {ex.Message}");
        }
    }

    private string ConstruirTextoFiltros(
        int? idMaterial,
        int? idDeposito,
        bool incluirMatInactivos,
        bool incluirDepInactivos)
    {
        string material = idMaterial.HasValue ? idMaterial.Value.ToString() : "Todos";
        string deposito = idDeposito.HasValue ? idDeposito.Value.ToString() : "Todos";

        string matEstado = incluirMatInactivos ? "Incluye inactivos" : "Solo activos";
        string depEstado = incluirDepInactivos ? "Incluye inactivos" : "Solo activos";

        return $"Material: {material} | Depósito: {deposito} | {matEstado} | {depEstado}";
    }
}
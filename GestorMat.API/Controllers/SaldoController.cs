using GestorMat.Application.DTOs.Saldo;
using GestorMat.Application.Interfaces;
using GestorMat.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class SaldoController(
    ISaldoRepository saldoRepository,
    IPdfSaldoService pdfService) : ControllerBase
{
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
            SaldoQuery query = new SaldoQuery
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
                data,
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
    List<SaldoDto> data,
    int? idMaterial,
    int? idDeposito,
    bool incluirMatInactivos,
    bool incluirDepInactivos)
    {
        string material = "Todos";
        string deposito = "Todos";

        if (idMaterial.HasValue)
        {
            SaldoDto? item = data.FirstOrDefault();
            material = item != null ? item.CodigoMaterial : idMaterial.Value.ToString();
        }

        if (idDeposito.HasValue)
        {
            SaldoDto? item = data.FirstOrDefault();
            deposito = item != null ? item.CodigoDeposito : idDeposito.Value.ToString();
        }

        string matEstado = incluirMatInactivos ? "Incluye inactivos" : "Solo activos";
        string depEstado = incluirDepInactivos ? "Incluye inactivos" : "Solo activos";

        return $"Material: {material} | Depósito: {deposito} | {matEstado} | {depEstado}";
    }

}
using GestorMat.Application.DTOs.Saldo;
using GestorMat.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestorMat.Infrastructure.Servicios;

public class PdfSaldoService : IPdfSaldoService
{
    public byte[] GenerarReporte(List<SaldoDto> data, string filtros)
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Margin(30);

                page.Header().Element(container =>
                {
                    ConstruirHeader(container, filtros);
                });

                page.Content().Element(container =>
                {
                    ConstruirTabla(container, data);
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Generado el ");
                    text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                });
            });
        }).GeneratePdf();
    }

    private void ConstruirHeader(IContainer container, string filtros)
    {
        container.Column(column =>
        {
            column.Item().Text("Reporte de Saldos de Materiales")
                .FontSize(18)
                .Bold();

            column.Item().Text(filtros)
                .FontSize(10)
                .FontColor(Colors.Grey.Darken1);
        });
    }

    private void ConstruirTabla(IContainer container, List<SaldoDto> data)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(2);
                columns.RelativeColumn(2);
                columns.RelativeColumn(1);
                columns.RelativeColumn(1);
                columns.RelativeColumn(1);
                columns.RelativeColumn(2);
            });

            // HEADER
            table.Header(header =>
            {
                header.Cell().Element(CellHeader).Text("Material");
                header.Cell().Element(CellHeader).Text("Depósito");
                header.Cell().Element(CellHeader).Text("UM");
                header.Cell().Element(CellHeader).Text("Disponible");
                header.Cell().Element(CellHeader).Text("Mínimo");
                header.Cell().Element(CellHeader).Text("Últ. Movimiento");
            });

            // ROWS
            foreach (SaldoDto item in data)
            {
                decimal diferencia = Convert.ToDecimal(item.CantidadDisponible - item.StockMinimo);

                decimal porcentaje = item.StockMinimo == 0
                    ? 0
                    : diferencia / Convert.ToDecimal(item.StockMinimo);

                table.Cell().Element(CellBody).Text($"{item.CodigoMaterial} - {item.NombreMaterial}");
                table.Cell().Element(CellBody).Text($"{item.CodigoDeposito} - {item.NombreDeposito}");
                table.Cell().Element(CellBody).Text(item.UnidadMedida);
                table.Cell().Element(CellBody).Text(item.CantidadDisponible.ToString("N2"));
                table.Cell().Element(CellBody).Text(item.StockMinimo.ToString("N2"));

                string fecha = item.FechaUltimoMovimiento == DateTime.MinValue
      ? "-"
      : item.FechaUltimoMovimiento.ToString("dd/MM/yyyy");

                table.Cell().Element(CellBody).Text(fecha);
            }
        });
    }

    private static IContainer CellHeader(IContainer container)
    {
        return container
            .Padding(5)
            .Background(Colors.Grey.Lighten2)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Medium);
    }

    private static IContainer CellBody(IContainer container)
    {
        return container
            .Padding(5)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2);
    }
}
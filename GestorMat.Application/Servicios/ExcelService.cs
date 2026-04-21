using ClosedXML.Excel;
using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Interfaces;

namespace GestorMat.Infrastructure.Services;

public class ExcelService : IExcelService
{
    public byte[] GenerarPlantillaMateriales(List<string> unidades)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Materiales");

        ws.Cell(1, 1).Value = "Codigo Material";
        ws.Cell(1, 2).Value = "Nombre";
        ws.Cell(1, 3).Value = "Precio";
        ws.Cell(1, 4).Value = "Unidad de medida";
        ws.Cell(1, 5).Value = "Descripción";
        ws.Cell(1, 6).Value = "Activo";
        ws.Cell(1, 7).Value = "Permite stock negativo";
        ws.Cell(1, 8).Value = "Stock mínimo";

        // Dropdown unidades
        var wsUnidades = wb.Worksheets.Add("Unidades");
        for (int i = 0; i < unidades.Count; i++)
            wsUnidades.Cell(i + 1, 1).Value = unidades[i];

        var rango = ws.Range("D2:D1000");
        rango.SetDataValidation().List(wsUnidades.Range($"A1:A{unidades.Count}"));

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    public List<MaterialExcelRowDto> LeerExcelMateriales(Stream stream)
    {
        var lista = new List<MaterialExcelRowDto>();

        using var wb = new XLWorkbook(stream);
        var ws = wb.Worksheet(1);

        int lastRow = ws.LastRowUsed().RowNumber();

        for (int fila = 2; fila <= lastRow; fila++)
        {
            if (ws.Row(fila).IsEmpty()) continue;

            lista.Add(new MaterialExcelRowDto
            {
                Fila = fila,
                CodigoMaterial = ws.Cell(fila, 1).GetString(),
                Nombre = ws.Cell(fila, 2).GetString(),
                PrecioRaw = ws.Cell(fila, 3).GetString(),
                UnidadNombre = ws.Cell(fila, 4).GetString(),
                Descripcion = ws.Cell(fila, 5).GetString(),
                ActivoRaw = ws.Cell(fila, 6).GetString(),
                PermiteStockNegativoRaw = ws.Cell(fila, 7).GetString(),
                StockMinimoRaw = ws.Cell(fila, 8).GetString()
            });
        }

        return lista;
    }
}
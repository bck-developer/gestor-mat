using ClosedXML.Excel;
using GestorMat.Application.Interfaces;

namespace GestorMat.Infrastructure.Services;

public class ExcelService : IExcelService
{
    public byte[] GenerarPlantillaMateriales(List<string> unidades)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Materiales");

        ws.Cell(1, 1).Value = "CodigoMaterial";
        ws.Cell(1, 2).Value = "Nombre";
        ws.Cell(1, 3).Value = "Precio";
        ws.Cell(1, 4).Value = "UnidadMedida";
        ws.Cell(1, 5).Value = "Descripcion";
        ws.Cell(1, 6).Value = "Activo";
        ws.Cell(1, 7).Value = "PermiteStockNegativo";
        ws.Cell(1, 8).Value = "StockMinimo";

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

    public List<Dictionary<string, string>> LeerExcel(Stream stream)
    {
        using var wb = new XLWorkbook(stream);
        var ws = wb.Worksheet(1);

        var rows = new List<Dictionary<string, string>>();

        foreach (var row in ws.RowsUsed().Skip(1))
        {
            var dict = new Dictionary<string, string>
            {
                ["CodigoMaterial"] = row.Cell(1).GetString(),
                ["Nombre"] = row.Cell(2).GetString(),
                ["Precio"] = row.Cell(3).GetString(),
                ["UnidadMedida"] = row.Cell(4).GetString(),
                ["Descripcion"] = row.Cell(5).GetString(),
                ["Activo"] = row.Cell(6).GetString(),
                ["PermiteStockNegativo"] = row.Cell(7).GetString(),
                ["StockMinimo"] = row.Cell(8).GetString()
            };

            rows.Add(dict);
        }

        return rows;
    }
}

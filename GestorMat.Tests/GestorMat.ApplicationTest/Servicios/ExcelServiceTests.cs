using Xunit;
using GestorMat.Infrastructure.Services;
using GestorMat.Application.DTOs.Material;
using System.IO;
using ClosedXML.Excel;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class ExcelServiceTests
{
    private readonly ExcelService _service = new();

    #region GenerarPlantillaMateriales Tests

    [Fact]
    public void GenerarPlantillaMateriales_DebeGenerarArchivoValido()
    {
        // Arrange
        List<string> unidades = new() { "Kilogramo", "Litro", "Metro", "Unidad" };

        // Act
        byte[] resultado = _service.GenerarPlantillaMateriales(unidades);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
        Assert.True(resultado.Length > 0);
    }

    [Fact]
    public void GenerarPlantillaMateriales_DebeContenerEncabezados()
    {
        // Arrange
        List<string> unidades = new() { "Kilogramo", "Litro" };

        // Act
        byte[] resultado = _service.GenerarPlantillaMateriales(unidades);

        // Assert
        using var stream = new MemoryStream(resultado);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);

        Assert.Equal("Código Material", worksheet.Cell(1, 1).GetString());
        Assert.Equal("Nombre", worksheet.Cell(1, 2).GetString());
        Assert.Equal("Precio", worksheet.Cell(1, 3).GetString());
        Assert.Equal("Unidad de medida", worksheet.Cell(1, 4).GetString());
        Assert.Equal("Descripción", worksheet.Cell(1, 5).GetString());
        Assert.Equal("Activo", worksheet.Cell(1, 6).GetString());
        Assert.Equal("Permite stock negativo", worksheet.Cell(1, 7).GetString());
        Assert.Equal("Stock mínimo", worksheet.Cell(1, 8).GetString());
    }

    [Fact]
    public void GenerarPlantillaMateriales_DebeIncluirHojaDeUnidades()
    {
        // Arrange
        List<string> unidades = new() { "Kilogramo", "Litro", "Metro" };

        // Act
        byte[] resultado = _service.GenerarPlantillaMateriales(unidades);

        // Assert
        using var stream = new MemoryStream(resultado);
        using var workbook = new XLWorkbook(stream);

        Assert.Equal(2, workbook.Worksheets.Count);
        Assert.Equal("Unidades", workbook.Worksheet(2).Name);
    }

    [Fact]
    public void GenerarPlantillaMateriales_ConUnidadesVacias_DebeGenerarArchivo()
    {
        // Arrange
        List<string> unidades = new();

        // Act
        byte[] resultado = _service.GenerarPlantillaMateriales(unidades);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
    }

    #endregion

    #region LeerExcelMateriales Tests

    [Fact]
    public void LeerExcelMateriales_ConArchivoValido_DebeExtraerDatos()
    {
        // Arrange
        using var ms = new MemoryStream();
        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("Materiales");
            ws.Cell(1, 1).Value = "Código Material";
            ws.Cell(1, 2).Value = "Nombre";
            ws.Cell(1, 3).Value = "Precio";
            ws.Cell(1, 4).Value = "Unidad de medida";
            ws.Cell(1, 5).Value = "Descripción";
            ws.Cell(1, 6).Value = "Activo";
            ws.Cell(1, 7).Value = "Permite stock negativo";
            ws.Cell(1, 8).Value = "Stock mínimo";

            ws.Cell(2, 1).Value = "MAT001";
            ws.Cell(2, 2).Value = "Acero";
            ws.Cell(2, 3).Value = "50.00";
            ws.Cell(2, 4).Value = "Kilogramo";
            ws.Cell(2, 5).Value = "Acero industrial";
            ws.Cell(2, 6).Value = "SI";
            ws.Cell(2, 7).Value = "NO";
            ws.Cell(2, 8).Value = "10";

            wb.SaveAs(ms);
        }
        ms.Position = 0;

        // Act
        var resultado = _service.LeerExcelMateriales(ms);

        // Assert
        Assert.NotNull(resultado);
        Assert.Single(resultado);
        Assert.Equal("MAT001", resultado[0].CodigoMaterial);
        Assert.Equal("Acero", resultado[0].Nombre);
        Assert.Equal("50.00", resultado[0].PrecioRaw);
        Assert.Equal("Kilogramo", resultado[0].UnidadNombre);
    }

    [Fact]
    public void LeerExcelMateriales_ConMultipleFilas_DebeExtraerTodas()
    {
        // Arrange
        using var ms = new MemoryStream();
        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("Materiales");
            ws.Cell(1, 1).Value = "Código Material";
            ws.Cell(1, 2).Value = "Nombre";
            ws.Cell(1, 3).Value = "Precio";
            ws.Cell(1, 4).Value = "Unidad de medida";
            ws.Cell(1, 5).Value = "Descripción";
            ws.Cell(1, 6).Value = "Activo";
            ws.Cell(1, 7).Value = "Permite stock negativo";
            ws.Cell(1, 8).Value = "Stock mínimo";

            for (int i = 2; i <= 5; i++)
            {
                ws.Cell(i, 1).Value = $"MAT{i:000}";
                ws.Cell(i, 2).Value = $"Material {i}";
                ws.Cell(i, 3).Value = "100.00";
                ws.Cell(i, 4).Value = "Litro";
                ws.Cell(i, 5).Value = $"Descripción {i}";
                ws.Cell(i, 6).Value = "SI";
                ws.Cell(i, 7).Value = "NO";
                ws.Cell(i, 8).Value = "5";
            }

            wb.SaveAs(ms);
        }
        ms.Position = 0;

        // Act
        var resultado = _service.LeerExcelMateriales(ms);

        // Assert
        Assert.Equal(4, resultado.Count);
        Assert.All(resultado, item => Assert.NotEmpty(item.CodigoMaterial));
    }

    [Fact]
    public void LeerExcelMateriales_ConFilasVacias_NoDebeIncluirlas()
    {
        // Arrange
        using var ms = new MemoryStream();
        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("Materiales");
            ws.Cell(1, 1).Value = "Código Material";
            ws.Cell(1, 2).Value = "Nombre";
            ws.Cell(1, 3).Value = "Precio";
            ws.Cell(1, 4).Value = "Unidad de medida";
            ws.Cell(1, 5).Value = "Descripción";
            ws.Cell(1, 6).Value = "Activo";
            ws.Cell(1, 7).Value = "Permite stock negativo";
            ws.Cell(1, 8).Value = "Stock mínimo";

            ws.Cell(2, 1).Value = "MAT001";
            ws.Cell(2, 2).Value = "Acero";
            ws.Cell(2, 3).Value = "50.00";
            ws.Cell(2, 4).Value = "Kilogramo";
            ws.Cell(2, 5).Value = "Descripción";
            ws.Cell(2, 6).Value = "SI";
            ws.Cell(2, 7).Value = "NO";
            ws.Cell(2, 8).Value = "10";

            // Fila 3 está vacía

            ws.Cell(4, 1).Value = "MAT004";
            ws.Cell(4, 2).Value = "Cobre";
            ws.Cell(4, 3).Value = "75.00";
            ws.Cell(4, 4).Value = "Kilogramo";
            ws.Cell(4, 5).Value = "Descripción 2";
            ws.Cell(4, 6).Value = "SI";
            ws.Cell(4, 7).Value = "NO";
            ws.Cell(4, 8).Value = "5";

            wb.SaveAs(ms);
        }
        ms.Position = 0;

        // Act
        var resultado = _service.LeerExcelMateriales(ms);

        // Assert
        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public void LeerExcelMateriales_DebeAsignarNumeroDeFila()
    {
        // Arrange
        using var ms = new MemoryStream();
        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("Materiales");
            ws.Cell(1, 1).Value = "Código Material";
            ws.Cell(1, 2).Value = "Nombre";
            ws.Cell(1, 3).Value = "Precio";
            ws.Cell(1, 4).Value = "Unidad de medida";
            ws.Cell(1, 5).Value = "Descripción";
            ws.Cell(1, 6).Value = "Activo";
            ws.Cell(1, 7).Value = "Permite stock negativo";
            ws.Cell(1, 8).Value = "Stock mínimo";

            ws.Cell(2, 1).Value = "MAT001";
            ws.Cell(2, 2).Value = "Acero";
            ws.Cell(2, 3).Value = "50.00";
            ws.Cell(2, 4).Value = "Kilogramo";
            ws.Cell(2, 5).Value = "Descripción";
            ws.Cell(2, 6).Value = "SI";
            ws.Cell(2, 7).Value = "NO";
            ws.Cell(2, 8).Value = "10";

            wb.SaveAs(ms);
        }
        ms.Position = 0;

        // Act
        var resultado = _service.LeerExcelMateriales(ms);

        // Assert
        Assert.Equal(2, resultado[0].Fila);
    }

    #endregion
}

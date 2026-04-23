using Xunit;
using GestorMat.Application.Servicios;
using System.Xml.Linq;
using System.IO;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class XmlServiceTests
{
    private readonly XmlService _service = new();

    #region LeerMaterialesDesdeXml Tests

    [Fact]
    public void LeerMaterialesDesdeXml_ConArchivoValido_DebeExtraerMateriales()
    {
        // Arrange
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root>
                <Material>
                    <Nombre>Acero</Nombre>
                    <Precio>50</Precio>
                    <Stock>100</Stock>
                    <IdUnidadMedida>1</IdUnidadMedida>
                </Material>
                <Material>
                    <Nombre>Cobre</Nombre>
                    <Precio>75</Precio>
                    <Stock>50</Stock>
                    <IdUnidadMedida>1</IdUnidadMedida>
                </Material>
            </Root>";

        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }
        stream.Position = 0;

        // Act
        var resultado = _service.LeerMaterialesDesdeXml(stream);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("Acero", resultado[0].Nombre);
        Assert.Equal(50, resultado[0].Precio);
        Assert.Equal(100, resultado[0].Stock);
        Assert.Equal(1, resultado[0].IdUnidadMedida);
    }

    [Fact]
    public void LeerMaterialesDesdeXml_ConUnMaterial_DebeExtraerCorrectamente()
    {
        // Arrange
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root>
                <Material>
                    <Nombre>Hierro</Nombre>
                    <Precio>30</Precio>
                    <Stock>200</Stock>
                    <IdUnidadMedida>2</IdUnidadMedida>
                </Material>
            </Root>";

        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }
        stream.Position = 0;

        // Act
        var resultado = _service.LeerMaterialesDesdeXml(stream);

        // Assert
        Assert.Single(resultado);
        Assert.Equal("Hierro", resultado[0].Nombre);
        Assert.Equal(30, resultado[0].Precio);
        Assert.Equal(200, resultado[0].Stock);
        Assert.Equal(2, resultado[0].IdUnidadMedida);
    }

    [Fact]
    public void LeerMaterialesDesdeXml_ConArchivoVacio_DebeRetornarListaVacia()
    {
        // Arrange
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root>
            </Root>";

        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }
        stream.Position = 0;

        // Act
        var resultado = _service.LeerMaterialesDesdeXml(stream);

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public void LeerMaterialesDesdeXml_ConElementosFaltantes_DebeAsignarValoresDefault()
    {
        // Arrange
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root>
                <Material>
                    <Nombre>Acero</Nombre>
                </Material>
            </Root>";

        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }
        stream.Position = 0;

        // Act
        var resultado = _service.LeerMaterialesDesdeXml(stream);

        // Assert
        Assert.Single(resultado);
        Assert.Equal("Acero", resultado[0].Nombre);
        Assert.Equal(0m, resultado[0].Precio);
        Assert.Equal(0, resultado[0].Stock);
        Assert.Equal(0, resultado[0].IdUnidadMedida);
    }

    [Fact]
    public void LeerMaterialesDesdeXml_ConMultiplesMateriales_DebeMapearTodos()
    {
        // Arrange
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root>
                <Material>
                    <Nombre>Material1</Nombre>
                    <Precio>10.00</Precio>
                    <Stock>10</Stock>
                    <IdUnidadMedida>1</IdUnidadMedida>
                </Material>
                <Material>
                    <Nombre>Material2</Nombre>
                    <Precio>20.00</Precio>
                    <Stock>20</Stock>
                    <IdUnidadMedida>2</IdUnidadMedida>
                </Material>
                <Material>
                    <Nombre>Material3</Nombre>
                    <Precio>30.00</Precio>
                    <Stock>30</Stock>
                    <IdUnidadMedida>3</IdUnidadMedida>
                </Material>
            </Root>";

        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }
        stream.Position = 0;

        // Act
        var resultado = _service.LeerMaterialesDesdeXml(stream);

        // Assert
        Assert.Equal(3, resultado.Count);
        Assert.All(resultado, item => Assert.NotNull(item.Nombre));
        Assert.NotEqual(resultado[0].Precio, resultado[1].Precio);
    }

    [Fact]
    public void LeerMaterialesDesdeXml_ConPreciosDecimales_DebeMapearCorrectamente()
    {
        // Arrange
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root>
                <Material>
                    <Nombre>Acero Premium</Nombre>
                    <Precio>123</Precio>
                    <Stock>50</Stock>
                    <IdUnidadMedida>1</IdUnidadMedida>
                </Material>
            </Root>";

        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }
        stream.Position = 0;

        // Act
        var resultado = _service.LeerMaterialesDesdeXml(stream);

        // Assert
        Assert.Single(resultado);
        Assert.Equal(123m, resultado[0].Precio);
    }

    [Fact]
    public void LeerMaterialesDesdeXml_ConNombresEspeciales_DebeExtraerCorrectamente()
    {
        // Arrange
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root>
                <Material>
                    <Nombre>Acero Inoxidable 304</Nombre>
                    <Precio>50.00</Precio>
                    <Stock>100</Stock>
                    <IdUnidadMedida>1</IdUnidadMedida>
                </Material>
            </Root>";

        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }
        stream.Position = 0;

        // Act
        var resultado = _service.LeerMaterialesDesdeXml(stream);

        // Assert
        Assert.Single(resultado);
        Assert.Equal("Acero Inoxidable 304", resultado[0].Nombre);
    }

    [Fact]
    public void LeerMaterialesDesdeXml_ConStockAlto_DebeExtraerCorrectamente()
    {
        // Arrange
        string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root>
                <Material>
                    <Nombre>Material</Nombre>
                    <Precio>50.00</Precio>
                    <Stock>999999</Stock>
                    <IdUnidadMedida>1</IdUnidadMedida>
                </Material>
            </Root>";

        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, leaveOpen: true))
        {
            writer.Write(xml);
            writer.Flush();
        }
        stream.Position = 0;

        // Act
        var resultado = _service.LeerMaterialesDesdeXml(stream);

        // Assert
        Assert.Single(resultado);
        Assert.Equal(999999, resultado[0].Stock);
    }

    #endregion
}

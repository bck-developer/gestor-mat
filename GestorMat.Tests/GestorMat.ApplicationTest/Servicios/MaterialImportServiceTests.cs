using Xunit;
using Moq;
using GestorMat.Application.Servicios;
using GestorMat.Application.Interfaces;
using GestorMat.Application.DTOs.Material;
using GestorMat.Domain.Entidades;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class MaterialImportServiceTests
{
    private readonly Mock<IMaterialRepository> _materialRepository = new();
    private readonly Mock<IUnidadMedidaRepository> _unidadRepository = new();
    private readonly MaterialImportService _service;

    public MaterialImportServiceTests()
    {
        _service = new MaterialImportService(_materialRepository.Object, _unidadRepository.Object);
    }

    #region ImportarAsync Tests

    [Fact]
    public async Task ImportarAsync_ConFilasValidas_DebeImportarExitosamente()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "Acero",
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.TotalProcesados);
        Assert.Equal(1, resultado.Exitosos);
        Assert.Equal(0, resultado.Fallidos);
        Assert.Empty(resultado.Errores);
        _materialRepository.Verify(r => r.AgregarAsync(It.IsAny<Material>()), Times.Once);
    }

    [Fact]
    public async Task ImportarAsync_ConMultipleFilasValidas_DebeImportarTodas()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "Acero",
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            },
            new MaterialExcelRowDto
            {
                Fila = 3,
                CodigoMaterial = "MAT002",
                Nombre = "Cobre",
                PrecioRaw = "75.50",
                UnidadNombre = "Kilogramo",
                Descripcion = "Cobre puro",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "SI",
                StockMinimoRaw = "5"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(2, resultado.TotalProcesados);
        Assert.Equal(2, resultado.Exitosos);
        Assert.Equal(0, resultado.Fallidos);
        _materialRepository.Verify(r => r.AgregarAsync(It.IsAny<Material>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ImportarAsync_ConPrecioInvalido_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "Acero",
                PrecioRaw = "INVALIDO",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.TotalProcesados);
        Assert.Equal(0, resultado.Exitosos);
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
        Assert.Contains("Precio inválido", resultado.Errores[0].Mensaje);
    }

    [Fact]
    public async Task ImportarAsync_ConStockMinimoInvalido_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "Acero",
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "INVALIDO"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
        Assert.Contains("Stock mínimo inválido", resultado.Errores[0].Mensaje);
    }

    [Fact]
    public async Task ImportarAsync_ConUnidadNoExistente_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "Acero",
                PrecioRaw = "50.00",
                UnidadNombre = "Tonelada",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
        Assert.Contains("no existe", resultado.Errores[0].Mensaje);
    }

    [Fact]
    public async Task ImportarAsync_ConCodigoVacio_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "",
                Nombre = "Acero",
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
        Assert.Contains("Código obligatorio", resultado.Errores[0].Mensaje);
    }

    [Fact]
    public async Task ImportarAsync_ConNombreVacio_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "",
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
        Assert.Contains("Nombre obligatorio", resultado.Errores[0].Mensaje);
    }

    [Fact]
    public async Task ImportarAsync_ConPrecioNegativo_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "Acero",
                PrecioRaw = "-50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
    }

    [Fact]
    public async Task ImportarAsync_ConCodigoMasDE30Caracteres_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001123456789012345678901234567890",
                Nombre = "Acero",
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
        Assert.Contains("máximo 30 caracteres", resultado.Errores[0].Mensaje);
    }

    [Fact]
    public async Task ImportarAsync_ConNombreMasDE80Caracteres_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        string nombreLargo = new string('A', 81);
        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = nombreLargo,
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
    }

    [Fact]
    public async Task ImportarAsync_ConStockMinimoNegativo_DebeRegistrarError()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "Acero",
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "-5"
            }
        };

        // Act
        MaterialImportResultDto resultado= await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
    }

    [Fact]
    public async Task ImportarAsync_MezclaDeFrechasValideEInvalidas_DebeReportarAmbas()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        _unidadRepository.Setup(r => r.ObtenerTodosAsync(false))
            .ReturnsAsync(new List<UnidadMedida> { unidad });

        List<MaterialExcelRowDto> filas = new()
        {
            new MaterialExcelRowDto
            {
                Fila = 2,
                CodigoMaterial = "MAT001",
                Nombre = "Acero",
                PrecioRaw = "50.00",
                UnidadNombre = "Kilogramo",
                Descripcion = "Acero industrial",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "10"
            },
            new MaterialExcelRowDto
            {
                Fila = 3,
                CodigoMaterial = "MAT002",
                Nombre = "Cobre",
                PrecioRaw = "INVALIDO",
                UnidadNombre = "Kilogramo",
                Descripcion = "Cobre puro",
                ActivoRaw = "SI",
                PermiteStockNegativoRaw = "NO",
                StockMinimoRaw = "5"
            }
        };

        // Act
        MaterialImportResultDto resultado = await _service.ImportarAsync(filas);

        // Assert
        Assert.Equal(2, resultado.TotalProcesados);
        Assert.Equal(1, resultado.Exitosos);
        Assert.Equal(1, resultado.Fallidos);
        Assert.Single(resultado.Errores);
        _materialRepository.Verify(r => r.AgregarAsync(It.IsAny<Material>()), Times.Once);
    }

    #endregion
}

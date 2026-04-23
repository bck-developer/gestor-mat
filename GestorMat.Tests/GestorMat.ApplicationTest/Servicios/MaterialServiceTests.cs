using Xunit;
using Moq;
using GestorMat.Application.Servicios;
using GestorMat.Application.Interfaces;
using GestorMat.Application.DTOs.Material;
using GestorMat.Domain.Entidades;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class MaterialServiceTests
{
    private readonly Mock<IMaterialRepository> _repository = new();
    private readonly MaterialService _service;

    public MaterialServiceTests() =>
        _service = new MaterialService(_repository.Object);

    #region CrearAsyncService Tests

    [Fact]
    public async Task CrearAsyncService_ConDatosValidos_DebeCrearMaterial()
    {
        // Arrange
        CrearMaterialDto dto = new()
        {
            CodigoMaterial = "MAT001",
            Nombre = "Acero Inoxidable",
            Precio = 50.00m,
            Id_UnidadMedida = 1,
            Activo = true,
            Descripcion = "Acero inoxidable grado industrial",
            PermiteStockNegativo = false,
            StockMinimo = 10
        };

        // Act
        await _service.CrearAsyncService(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.Is<Material>(m =>
            m.CodigoMaterial == dto.CodigoMaterial &&
            m.Nombre == dto.Nombre &&
            m.Precio == dto.Precio &&
            m.Id_UnidadMedida == dto.Id_UnidadMedida &&
            m.Activo
        )), Times.Once);
    }

    [Fact]
    public async Task CrearAsyncService_ConStockNegativoPermitido_DebeCrearCorrectamente()
    {
        // Arrange
        CrearMaterialDto dto = new()
        {
            CodigoMaterial = "MAT002",
            Nombre = "Cobre",
            Precio = 75.50m,
            Id_UnidadMedida = 1,
            Activo = true,
            Descripcion = "Cobre puro",
            PermiteStockNegativo = true,
            StockMinimo = 5
        };

        // Act
        await _service.CrearAsyncService(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.IsAny<Material>()), Times.Once);
    }

    [Fact]
    public async Task CrearAsyncService_ConMaterialInactivo_DebeCrearCorrectamente()
    {
        // Arrange
        CrearMaterialDto dto = new()
        {
            CodigoMaterial = "MAT003",
            Nombre = "Aluminio",
            Precio = 30.00m,
            Id_UnidadMedida = 1,
            Activo = false,
            Descripcion = "Aluminio puro",
            PermiteStockNegativo = false,
            StockMinimo = 0
        };

        // Act
        await _service.CrearAsyncService(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.IsAny<Material>()), Times.Once);
    }

    #endregion

    #region ObtenerTodosAsyncService Tests

    [Fact]
    public async Task ObtenerTodosAsyncService_ConMaterialesExistentes_DebeRetornarListaDtos()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        List<Material> materiales =
        [
            new("MAT001", "Acero", 50.00m, 1, true, "Acero industrial", false, 10)
            { Id_Material = 1, UnidadMedida = unidad },

            new("MAT002", "Cobre", 75.50m, 1, true, "Cobre puro", true, 5)
            { Id_Material = 2, UnidadMedida = unidad }
        ];

        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(materiales);

        // Act
        List<MaterialDto> resultado = await _service.ObtenerTodosAsyncService();

        // Assert
        Assert.Equal(2, resultado.Count);
        Assert.Equal(("Acero", "Cobre"), (resultado[0].Nombre, resultado[1].Nombre));
        Assert.Equal("Kilogramo", resultado[0].UnidadMedida);
    }

    [Fact]
    public async Task ObtenerTodosAsyncService_SinMateriales_DebeRetornarListaVacia()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync([]);

        // Act
        List<MaterialDto> resultado = await _service.ObtenerTodosAsyncService();

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodosAsyncService_DebeMapearTodosLosCampos()
    {
        // Arrange
        UnidadMedida unidad = new("Litro", "L", true) { Id_UnidadMedida = 2 };

        List<Material> materiales =
        [
            new("MAT004", "Pintura", 120.00m, 2, true, "Pintura acrílica", false, 20)
            { Id_Material = 4, UnidadMedida = unidad }
        ];

        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(materiales);

        // Act
        List<MaterialDto> resultado = await _service.ObtenerTodosAsyncService();

        // Assert
        MaterialDto item = resultado[0];
        Assert.Equal((4, "MAT004", "Pintura", 120.00m, "Pintura acrílica", false, 20, true),
                     (item.Id_Material, item.CodigoMaterial, item.Nombre, item.Precio,
                      item.Descripcion, item.PermiteStockNegativo, item.StockMinimo, item.Activo));
    }

    #endregion

    #region ObtenerPorIdAsyncService Tests

    [Fact]
    public async Task ObtenerPorIdAsyncService_ConIdExistente_DebeRetornarMaterialDto()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        Material material = new("MAT001", "Acero", 50.00m, 1, true, "Acero industrial", false, 10)
        { Id_Material = 1, UnidadMedida = unidad };

        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(material);

        // Act
        MaterialDto? resultado = await _service.ObtenerPorIdAsyncService(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal((1, "Acero", "MAT001", 50.00m, "Kilogramo"),
                     (resultado.Id_Material, resultado.Nombre, resultado.CodigoMaterial, resultado.Precio, resultado.UnidadMedida));
    }

    [Fact]
    public async Task ObtenerPorIdAsyncService_ConIdNoExistente_DebeRetornarNull()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((Material?)null);

        // Act
        MaterialDto? resultado = await _service.ObtenerPorIdAsyncService(999);

        // Assert
        Assert.Null(resultado);
    }

    #endregion

    #region ActualizarAsyncService Tests

    [Fact]
    public async Task ActualizarAsyncService_ConMaterialExistente_DebeActualizarDatos()
    {
        // Arrange
        Material material = new("MAT001", "Acero", 50.00m, 1, true, string.Empty, false, 10)
        { Id_Material = 1 };

        ActualizarMaterialDto dto = new()
        {
            CodigoMaterial = "MAT001",
            Nombre = "Acero Mejorado",
            Precio = 60.00m,
            Id_UnidadMedida = 1,
            Activo = true,
            Descripcion = "Acero industrial mejorado",
            PermiteStockNegativo = false,
            StockMinimo = 15
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(material);

        // Act
        await _service.ActualizarAsyncService(1, dto);

        // Assert
        _repository.Verify(r => r.EditarAsync(It.IsAny<Material>()), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsyncService_ConMaterialNoExistente_DebeLanzarExcepcion()
    {
        // Arrange
        ActualizarMaterialDto dto = new()
        {
            CodigoMaterial = "MAT999",
            Nombre = "Material Fantasma",
            Precio = 100.00m,
            Id_UnidadMedida = 1,
            Activo = true,
            Descripcion = string.Empty,
            PermiteStockNegativo = false,
            StockMinimo = 0
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((Material?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.ActualizarAsyncService(999, dto));
    }

    #endregion

    #region EliminarAsyncService Tests

    [Fact]
    public async Task EliminarAsyncService_ConMaterialExistente_DebeEliminar()
    {
        // Arrange
        Material material = new("MAT001", "Acero", 50.00m, 1, true, string.Empty, false, 10)
        { Id_Material = 1 };

        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(material);

        // Act
        await _service.EliminarAsyncService(1);

        // Assert
        _repository.Verify(r => r.EliminarAsync(It.IsAny<Material>()), Times.Once);
    }

    [Fact]
    public async Task EliminarAsyncService_ConMaterialNoExistente_DebeLanzarExcepcion()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((Material?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.EliminarAsyncService(999));
    }

    #endregion
}
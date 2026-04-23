using GestorMat.Application.DTOs;
using GestorMat.Application.Interfaces;
using GestorMat.Application.Servicios;
using GestorMat.Domain.Entidades;
using Moq;
using Xunit;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class UnidadMedidaServiceTests
{
    private readonly Mock<IUnidadMedidaRepository> _repository = new();
    private readonly UnidadMedidaService _service;

    public UnidadMedidaServiceTests() =>
        _service = new UnidadMedidaService(_repository.Object);

    #region CrearAsyncService Tests

    [Fact]
    public async Task CrearAsyncService_ConDatosValidos_DebeGuardarUnidadMedida()
    {
        // Arrange
        CrearUnidadMedidaDto dto = new()
        {
            Nombre = "Kilogramo",
            Abreviatura = "kg",
            Activo = true
        };

        // Act
        await _service.CrearAsyncService(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.Is<UnidadMedida>(u =>
            u.Nombre == dto.Nombre &&
            u.Abreviatura == dto.Abreviatura &&
            u.Activo
        )), Times.Once);
    }

    [Fact]
    public async Task CrearAsyncService_ConUnidadInactiva_DebeCrearCorrectamente()
    {
        // Arrange
        CrearUnidadMedidaDto dto = new()
        {
            Nombre = "Litro",
            Abreviatura = "L",
            Activo = false
        };

        // Act
        await _service.CrearAsyncService(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.IsAny<UnidadMedida>()), Times.Once);
    }

    #endregion

    #region ObtenerTodosAsyncService Tests

    [Fact]
    public async Task ObtenerTodosAsyncService_ConUnidadesExistentes_DebeRetornarListaDtos()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync(true)).ReturnsAsync([
            new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 },
            new("Litro", "L", true) { Id_UnidadMedida = 2 }
        ]);

        // Act
        List<UnidadMedidaDto> resultado = await _service.ObtenerTodosAsyncService(true);

        // Assert
        Assert.Equal(2, resultado.Count);
        Assert.Equal(("Kilogramo", "Litro"), (resultado[0].Nombre, resultado[1].Nombre));
        Assert.Equal("kg", resultado[0].Abreviatura);
    }

    [Fact]
    public async Task ObtenerTodosAsyncService_SinUnidades_DebeRetornarListaVacia()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync(true)).ReturnsAsync([]);

        // Act
        List<UnidadMedidaDto> resultado = await _service.ObtenerTodosAsyncService(true);

        // Assert
        Assert.Empty(resultado);
    }

    #endregion

    #region ObtenerPorIdAsyncService Tests

    [Fact]
    public async Task ObtenerPorIdAsyncService_ConIdExistente_DebeRetornarUnidadMedida()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerPorIdAsync(1))
                   .ReturnsAsync(new UnidadMedida("Kilogramo", "kg", true) { Id_UnidadMedida = 1 });

        // Act
        UnidadMedida? resultado = await _service.ObtenerPorIdAsyncService(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Kilogramo", resultado.Nombre);
    }

    [Fact]
    public async Task ObtenerPorIdAsyncService_ConIdNoExistente_DebeRetornarNull()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((UnidadMedida?)null);

        // Act
        UnidadMedida? resultado = await _service.ObtenerPorIdAsyncService(999);

        // Assert
        Assert.Null(resultado);
    }

    #endregion

    #region ActualizarAsyncService Tests

    [Fact]
    public async Task ActualizarAsyncService_ConUnidadExistente_DebeActualizarDatos()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        CrearUnidadMedidaDto dto = new()
        {
            Nombre = "Tonelada",
            Abreviatura = "t",
            Activo = true
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(unidad);

        // Act
        await _service.ActualizarAsyncService(1, dto);

        // Assert
        _repository.Verify(r => r.EditarAsync(It.IsAny<UnidadMedida>()), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsyncService_ConUnidadNoExistente_DebeLanzarExcepcion()
    {
        // Arrange
        CrearUnidadMedidaDto dto = new()
        {
            Nombre = "Tonelada",
            Abreviatura = "t",
            Activo = true
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((UnidadMedida?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.ActualizarAsyncService(1, dto));
    }

    [Fact]
    public async Task ActualizarAsyncService_ConUnidadInactiva_DebeActualizarAInactiva()
    {
        // Arrange
        UnidadMedida unidad = new("Kilogramo", "kg", true) { Id_UnidadMedida = 1 };

        CrearUnidadMedidaDto dto = new()
        {
            Nombre = "Kilogramo",
            Abreviatura = "kg",
            Activo = false
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(unidad);

        // Act
        await _service.ActualizarAsyncService(1, dto);

        // Assert
        _repository.Verify(r => r.EditarAsync(It.IsAny<UnidadMedida>()), Times.Once);
    }

    #endregion
}
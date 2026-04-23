using Xunit;
using Moq;
using GestorMat.Application.Servicios;
using GestorMat.Application.Interfaces;
using GestorMat.Application.DTOs;
using GestorMat.Domain.Entidades;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class DepositoServiceTests
{
    private readonly Mock<IDepositoRepository> _repository = new();
    private readonly DepositoService _service;

    public DepositoServiceTests() =>
        _service = new DepositoService(_repository.Object);

    #region CrearDepositoAsync Tests

    [Fact]
    public async Task CrearDepositoAsync_ConDatosValidos_DebeGuardarDeposito()
    {
        // Arrange
        CrearDepositoDto dto = new()
        {
            CodigoDeposito = "DEP001",
            Nombre = "Depósito Central",
            Direccion = "Calle Principal 123",
            Habilitado = true
        };

        // Act
        await _service.CrearDepositoAsync(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.Is<Deposito>(d =>
            d.CodigoDeposito == dto.CodigoDeposito &&
            d.Nombre == dto.Nombre &&
            d.Direccion == dto.Direccion &&
            d.Habilitado
        )), Times.Once);
    }

    [Fact]
    public async Task CrearDepositoAsync_ConDepositoDeshabilitado_DebeCrearCorrectamente()
    {
        // Arrange
        CrearDepositoDto dto = new()
        {
            CodigoDeposito = "DEP002",
            Nombre = "Depósito Secundario",
            Direccion = "Calle Secundaria 456",
            Habilitado = false
        };

        // Act
        await _service.CrearDepositoAsync(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.IsAny<Deposito>()), Times.Once);
    }

    #endregion

    #region ObtenerDepositosAsync Tests

    [Fact]
    public async Task ObtenerDepositosAsync_ConDepositosExistentes_DebeRetornarListaDtos()
    {
        // Arrange
        List<Deposito> depositos =
        [
            new("DEP001", "Depósito Central", "Calle Principal 123", true) { Id_Deposito = 1 },
            new("DEP002", "Depósito Norte", "Calle Norte 456", true) { Id_Deposito = 2 }
        ];

        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(depositos);

        // Act
        List<DepositoDto> resultado = await _service.ObtenerDepositosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal(("Depósito Central", "DEP001"), (resultado[0].Nombre, resultado[0].CodigoDeposito));
        Assert.Equal("Depósito Norte", resultado[1].Nombre);
    }

    [Fact]
    public async Task ObtenerDepositosAsync_SinDepositos_DebeRetornarListaVacia()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync([]);

        // Act
        List<DepositoDto> resultado = await _service.ObtenerDepositosAsync();

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerDepositosAsync_ConDepositosDeshabilitados_DebeMantenerEstado()
    {
        // Arrange
        List<Deposito> depositos =
        [
            new("DEP001", "Depósito Activo", "Calle Principal 123", true) { Id_Deposito = 1 },
            new("DEP002", "Depósito Inactivo", "Calle Secundaria 456", false) { Id_Deposito = 2 }
        ];

        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(depositos);

        // Act
        List<DepositoDto> resultado = await _service.ObtenerDepositosAsync();

        // Assert
        Assert.Equal(2, resultado.Count);
        Assert.True(resultado[0].Habilitado);
        Assert.False(resultado[1].Habilitado);
    }

    #endregion

    #region ObtenerDepositoPorIdAsync Tests

    [Fact]
    public async Task ObtenerDepositoPorIdAsync_ConIdExistente_DebeRetornarDepositoDto()
    {
        // Arrange
        Deposito deposito = new("DEP001", "Depósito Central", "Calle Principal 123", true)
        {
            Id_Deposito = 1
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(deposito);

        // Act
        DepositoDto? resultado = await _service.ObtenerDepositoPorIdAsync(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal((1, "Depósito Central", "DEP001", "Calle Principal 123", true),
                     (resultado.Id_Deposito, resultado.Nombre, resultado.CodigoDeposito, resultado.Direccion, resultado.Habilitado));
    }

    [Fact]
    public async Task ObtenerDepositoPorIdAsync_ConIdNoExistente_DebeRetornarNull()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((Deposito?)null);

        // Act
        DepositoDto? resultado = await _service.ObtenerDepositoPorIdAsync(999);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task ObtenerDepositoPorIdAsync_ConDepositoDeshabilitado_DebeMantenerEstado()
    {
        // Arrange
        Deposito deposito = new("DEP002", "Depósito Inactivo", "Calle Secundaria 456", false)
        {
            Id_Deposito = 2
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(2)).ReturnsAsync(deposito);

        // Act
        DepositoDto? resultado = await _service.ObtenerDepositoPorIdAsync(2);

        // Assert
        Assert.NotNull(resultado);
        Assert.False(resultado.Habilitado);
    }

    #endregion
}
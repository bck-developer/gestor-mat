using GestorMat.Application.DTOs.MovimientoMaterial;
using GestorMat.Application.Interfaces;
using GestorMat.Domain.Entidades;
using Moq;
using Xunit;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class MovimientoMaterialServiceTests
{
    private readonly Mock<IMovimientoMaterialRepository> _movRepository = new();
    private readonly Mock<ISaldoRepository> _saldoRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly MovimientoMaterialService _service;

    public MovimientoMaterialServiceTests()
    {
        _service = new MovimientoMaterialService(_movRepository.Object, _saldoRepository.Object, _unitOfWork.Object);
    }

    #region CrearAsync Tests

    [Fact]
    public async Task CrearAsync_TipoIngreso_DebeCrearMovimientoYSumarSaldo()
    {
        // Arrange
        CrearMovimientoMaterialDto dto = new()
        {
            IdMaterial = 1,
            IdDepositoOrigen = 1,
            IdDepositoDestino = null,
            CodigoMovimiento = "MOV001",
            Tipo = "Ingreso",
            Cantidad = 100,
            Fecha = DateTime.Now,
            UserName = "usuario1"
        };

        Saldo saldo = new(1, 1, 50, DateTime.Now);
        _saldoRepository.Setup(r => r.ObtenerAsync(1, 1)).ReturnsAsync(saldo);

        // Act
        await _service.CrearAsync(dto);

        // Assert
        _unitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _saldoRepository.Verify(r => r.ActualizarAsync(It.IsAny<Saldo>()), Times.Once);
        _movRepository.Verify(r => r.AgregarAsync(It.IsAny<MovimientoMaterial>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CrearAsync_TipoEgreso_DebeCrearMovimientoYRestarSaldo()
    {
        // Arrange
        CrearMovimientoMaterialDto dto = new()
        {
            IdMaterial = 1,
            IdDepositoOrigen = 1,
            IdDepositoDestino = null,
            CodigoMovimiento = "MOV002",
            Tipo = "Egreso",
            Cantidad = 50,
            Fecha = DateTime.Now,
            UserName = "usuario1"
        };

        Saldo saldo = new(1, 1, 100, DateTime.Now);
        _saldoRepository.Setup(r => r.ObtenerAsync(1, 1)).ReturnsAsync(saldo);

        // Act
        await _service.CrearAsync(dto);

        // Assert
        _unitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _saldoRepository.Verify(r => r.ActualizarAsync(It.IsAny<Saldo>()), Times.Once);
        _movRepository.Verify(r => r.AgregarAsync(It.IsAny<MovimientoMaterial>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CrearAsync_TipoIngresoEgreso_DebeRestarDelOrigenYSumarAlDestino()
    {
        // Arrange
        CrearMovimientoMaterialDto dto = new()
        {
            IdMaterial = 1,
            IdDepositoOrigen = 1,
            IdDepositoDestino = 2,
            CodigoMovimiento = "MOV003",
            Tipo = "Ingreso/Egreso",
            Cantidad = 75,
            Fecha = DateTime.Now,
            UserName = "usuario1"
        };

        Saldo saldoOrigen = new(1, 1, 100, DateTime.Now);
        Saldo saldoDestino = new(1, 2, 50, DateTime.Now);

        _saldoRepository.Setup(r => r.ObtenerAsync(1, 1)).ReturnsAsync(saldoOrigen);
        _saldoRepository.Setup(r => r.ObtenerAsync(1, 2)).ReturnsAsync(saldoDestino);

        // Act
        await _service.CrearAsync(dto);

        // Assert
        _unitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _saldoRepository.Verify(r => r.ActualizarAsync(It.IsAny<Saldo>()), Times.Exactly(2));
        _movRepository.Verify(r => r.AgregarAsync(It.IsAny<MovimientoMaterial>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CrearAsync_ConSaldoNoExistente_DebeCrearNuevoSaldo()
    {
        // Arrange
        CrearMovimientoMaterialDto dto = new()
        {
            IdMaterial = 1,
            IdDepositoOrigen = 1,
            IdDepositoDestino = null,
            CodigoMovimiento = "MOV004",
            Tipo = "Ingreso",
            Cantidad = 100,
            Fecha = DateTime.Now,
            UserName = "usuario1"
        };

        _saldoRepository.Setup(r => r.ObtenerAsync(1, 1)).ReturnsAsync((Saldo?)null);

        // Act
        await _service.CrearAsync(dto);

        // Assert
        _saldoRepository.Verify(r => r.AgregarAsync(It.IsAny<Saldo>()), Times.Once);
        _movRepository.Verify(r => r.AgregarAsync(It.IsAny<MovimientoMaterial>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CrearAsync_ConTipoInvalido_DebeLanzarExcepcion()
    {
        // Arrange
        CrearMovimientoMaterialDto dto = new()
        {
            IdMaterial = 1,
            IdDepositoOrigen = 1,
            IdDepositoDestino = null,
            CodigoMovimiento = "MOV005",
            Tipo = "TipoInvalido",
            Cantidad = 100,
            Fecha = DateTime.Now,
            UserName = "usuario1"
        };

        // Act & Assert
        Exception? excepcion = await Assert.ThrowsAsync<Exception>(() => _service.CrearAsync(dto));
        Assert.Contains("Tipo de movimiento inválido", excepcion.Message);
        _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
    }

    [Fact]
    public async Task CrearAsync_SiLaTransaccionFalla_DebeHacerRollback()
    {
        // Arrange
        CrearMovimientoMaterialDto dto = new()
        {
            IdMaterial = 1,
            IdDepositoOrigen = 1,
            IdDepositoDestino = null,
            CodigoMovimiento = "MOV006",
            Tipo = "Ingreso",
            Cantidad = 100,
            Fecha = DateTime.Now,
            UserName = "usuario1"
        };

        _saldoRepository.Setup(r => r.ObtenerAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Error en base de datos"));

        // Act & Assert
        Exception? excepcion = await Assert.ThrowsAsync<Exception>(() => _service.CrearAsync(dto));
        _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    #endregion

    #region ObtenerTodosAsync Tests

    [Fact]
    public async Task ObtenerTodosAsync_ConMovimientosExistentes_DebeRetornarLista()
    {
        // Arrange
        Material material = new("MAT001", "Acero", 50.00m, 1, true, string.Empty, false, 10)
        { Id_Material = 1 };

        Deposito deposito = new("DEP001", "Deposito 1", "Ubicación 1", true);

        List<MovimientoMaterial> movimientos = new()
        {
            new MovimientoMaterial(1, 1, null, "MOV001", "Ingreso", 100, DateTime.Now, "usuario1")
        };

        _movRepository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(movimientos);

        // Act
        List<MovimientoMaterialDto> resultado = await _service.ObtenerTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado);
    }

    [Fact]
    public async Task ObtenerTodosAsync_SinMovimientos_DebeRetornarListaVacia()
    {
        // Arrange
        _movRepository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(new List<MovimientoMaterial>());

        // Act
        List<MovimientoMaterialDto> resultado = await _service.ObtenerTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    #endregion
}

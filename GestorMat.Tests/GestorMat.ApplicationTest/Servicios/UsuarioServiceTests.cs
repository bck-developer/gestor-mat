using Xunit;
using Moq;
using GestorMat.Application.Servicios;
using GestorMat.Application.Interfaces;
using GestorMat.Application.DTOs;
using GestorMat.Domain.Entidades;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _repository = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly UsuarioService _service;

    public UsuarioServiceTests() =>
        _service = new UsuarioService(_repository.Object, _hasher.Object);

    #region CrearAsyncService Tests

    [Fact]
    public async Task CrearAsyncService_ConDatosValidos_DebeCrearUsuario()
    {
        // Arrange
        CrearUsuarioDto dto = new()
        {
            Username = "jdoe",
            Password = "Password123",
            Nombre = "John Doe",
            Mail = "john@example.com",
            Id_Rol = 1,
            Activo = true
        };

        _hasher.Setup(h => h.Hash(dto.Password)).Returns("hash");

        // Act
        await _service.CrearAsyncService(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.Is<Usuario>(u =>
            u.Username == dto.Username &&
            u.Nombre == dto.Nombre &&
            u.Mail == dto.Mail &&
            u.IdRol == dto.Id_Rol &&
            u.Activo)), Times.Once);
    }

    [Fact]
    public async Task CrearAsyncService_ConPasswordInvalida_DebeLanzarExcepcion()
    {
        // Arrange
        CrearUsuarioDto dto = new()
        {
            Username = "jdoe",
            Password = string.Empty,
            Nombre = "John Doe",
            Mail = "john@example.com",
            Id_Rol = 1,
            Activo = true
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CrearAsyncService(dto));
        _repository.Verify(r => r.AgregarAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task CrearAsyncService_ConPasswordNula_DebeLanzarExcepcion()
    {
        // Arrange
        CrearUsuarioDto dto = new()
        {
            Username = "jdoe",
            Password = string.Empty,
            Nombre = "John Doe",
            Mail = "john@example.com",
            Id_Rol = 1,
            Activo = true
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CrearAsyncService(dto));
    }

    [Fact]
    public async Task CrearAsyncService_ConRolCero_DebeLanzarExcepcion()
    {
        // Arrange
        CrearUsuarioDto dto = new()
        {
            Username = "jdoe",
            Password = "Password123",
            Nombre = "John Doe",
            Mail = "john@example.com",
            Id_Rol = 0,
            Activo = true
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CrearAsyncService(dto));
    }

    [Fact]
    public async Task CrearAsyncService_ConUsuarioInactivo_DebeCrearCorrectamente()
    {
        // Arrange
        CrearUsuarioDto dto = new()
        {
            Username = "admin",
            Password = "Admin123",
            Nombre = "Administrador",
            Mail = "admin@example.com",
            Id_Rol = 1,
            Activo = false
        };

        _hasher.Setup(h => h.Hash(dto.Password)).Returns("hash");

        // Act
        await _service.CrearAsyncService(dto);

        // Assert
        _repository.Verify(r => r.AgregarAsync(It.IsAny<Usuario>()), Times.Once);
    }

    #endregion

    #region ObtenerTodosAsyncService Tests

    [Fact]
    public async Task ObtenerTodosAsyncService_ConUsuariosExistentes_DebeRetornarListaDtos()
    {
        // Arrange
        Rol rol = new() { Id_Rol = 1, RolName = "Admin", AccesoTotal = true };

        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync([
            new("jdoe", "hash", "John Doe", "john@example.com", 1, true) { Id_Usuario = 1, Rol = rol },
            new("admin", "hash", "Administrador", "admin@example.com", 1, true) { Id_Usuario = 2, Rol = rol }
        ]);

        // Act
        List<UsuarioDto> resultado = await _service.ObtenerTodosAsyncService();

        // Assert
        Assert.Equal(2, resultado.Count);
        Assert.Equal(("jdoe", "admin"), (resultado[0].Username, resultado[1].Username));
        Assert.Equal("Admin", resultado[0].Rol);
    }

    [Fact]
    public async Task ObtenerTodosAsyncService_SinUsuarios_DebeRetornarListaVacia()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync([]);

        // Act
        List<UsuarioDto> resultado = await _service.ObtenerTodosAsyncService();

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodosAsyncService_ConUsuarioSinRol_DebeRetornarRolVacio()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync([
            new("user1", "hash", "Usuario 1", "user1@example.com", 1, true) { Id_Usuario = 1, Rol = null }
        ]);

        // Act
        List<UsuarioDto> resultado = await _service.ObtenerTodosAsyncService();

        // Assert
        Assert.Single(resultado);
        Assert.Equal(string.Empty, resultado[0].Rol);
    }

    #endregion

    #region ObtenerPorIdAsyncService Tests

    [Fact]
    public async Task ObtenerPorIdAsyncService_ConIdExistente_DebeRetornarUsuario()
    {
        // Arrange
        Rol rol = new() { Id_Rol = 1, RolName = "Admin", AccesoTotal = true };

        _repository.Setup(r => r.ObtenerPorIdAsync(1))
                   .ReturnsAsync(new Usuario("jdoe", "hash", "John Doe", "john@example.com", 1, true)
                   { Id_Usuario = 1, Rol = rol });

        // Act
        Usuario? resultado = await _service.ObtenerPorIdAsyncService(1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(("jdoe", "John Doe"), (resultado.Username, resultado.Nombre));
    }

    [Fact]
    public async Task ObtenerPorIdAsyncService_ConIdNoExistente_DebeRetornarNull()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((Usuario?)null);

        // Act
        Usuario? resultado = await _service.ObtenerPorIdAsyncService(999);

        // Assert
        Assert.Null(resultado);
    }

    #endregion

    #region ActualizarAsyncService Tests

    [Fact]
    public async Task ActualizarAsyncService_ConPassword_DebeActualizarYHashear()
    {
        // Arrange
        Usuario usuario = new("jdoe", "hash", "John Doe", "john@example.com", 1, true) { Id_Usuario = 1 };

        CrearUsuarioDto dto = new()
        {
            Username = "nuevo",
            Password = "123",
            Nombre = "Nuevo",
            Mail = "nuevo@mail.com",
            Id_Rol = 2,
            Activo = true
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Hash(dto.Password)).Returns("hash");

        // Act
        await _service.ActualizarAsyncService(1, dto);

        // Assert
        _repository.Verify(r => r.EditarAsync(It.IsAny<Usuario>()), Times.Once);
        _hasher.Verify(h => h.Hash(dto.Password), Times.Once);
    }

    [Fact]
    public async Task ActualizarAsyncService_SinPassword_NoDebeHashear()
    {
        // Arrange
        Usuario usuario = new("jdoe", "hash", "John Doe", "john@example.com", 1, true) { Id_Usuario = 1 };

        CrearUsuarioDto dto = new()
        {
            Username = "nuevo",
            Password = string.Empty,
            Nombre = "Nuevo",
            Mail = "nuevo@mail.com",
            Id_Rol = 1,
            Activo = true
        };

        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);

        // Act
        await _service.ActualizarAsyncService(1, dto);

        // Assert
        _hasher.Verify(h => h.Hash(It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Eliminación / Estado Tests

    [Fact]
    public async Task EliminarFisicoAsync_ConUsuarioExistente_DebeEliminar()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerPorIdAsync(1))
                   .ReturnsAsync(new Usuario("u", "h", "n", "m", 1, true) { Id_Usuario = 1 });

        // Act
        await _service.EliminarFisicoAsync(1);

        // Assert
        _repository.Verify(r => r.EliminarAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task CambiosEstado_ConUsuarioExistente_DebenPersistir()
    {
        // Arrange
        Usuario usuario = new("u", "h", "n", "m", 1, true) { Id_Usuario = 1 };
        _repository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(usuario);

        // Act
        await _service.InhabilitarAsyncService(1);
        await _service.RehabilitarAsyncService(1);

        // Assert
        _repository.Verify(r => r.EditarAsync(It.IsAny<Usuario>()), Times.Exactly(2));
    }

    #endregion
}
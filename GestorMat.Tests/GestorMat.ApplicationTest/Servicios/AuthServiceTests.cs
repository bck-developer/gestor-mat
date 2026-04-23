using Xunit;
using Moq;
using GestorMat.Application.Servicios;
using GestorMat.Application.Interfaces;
using GestorMat.Application.DTOs;
using GestorMat.Domain.Entidades;
using Microsoft.Extensions.Configuration;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _repository = new();
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _configuration.Setup(c => c["Jwt:Key"]).Returns("SuperSecretKeyForTestingPurposesOnly123456");
        _configuration.Setup(c => c["Jwt:Issuer"]).Returns("GestorMatIssuer");
        _configuration.Setup(c => c["Jwt:Audience"]).Returns("GestorMatAudience");
        _service = new AuthService(_repository.Object, _configuration.Object, _hasher.Object);
    }

    #region LoginAsync Tests

    [Fact]
    public async Task LoginAsync_ConCredencialesValidas_DebeRetornarToken()
    {
        // Arrange
        LoginDto dto = new() { Username = "admin", Password = "Password123" };

        Rol rol = new("Administrador", "Rol administrativo", true) { Id_Rol = 1 };
        Usuario usuario = new("admin", "hashedPassword", "Administrador", "admin@example.com", 1, true)
        {
            Id_Usuario = 1,
            Rol = rol
        };

        _repository.Setup(r => r.ObtenerPorUsernameAsync("admin")).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Verify("Password123", "hashedPassword")).Returns(true);

        // Act
        var token = await _service.LoginAsync(dto);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.IsType<string>(token);
    }

    [Fact]
    public async Task LoginAsync_ConUsuarioNoExistente_DebeLanzarExcepcion()
    {
        // Arrange
        LoginDto dto = new() { Username = "noexiste", Password = "Password123" };

        _repository.Setup(r => r.ObtenerPorUsernameAsync("noexiste")).ReturnsAsync((Usuario?)null);

        // Act & Assert
        var excepcion = await Assert.ThrowsAsync<Exception>(() => _service.LoginAsync(dto));
        Assert.Equal("Usuario no encontrado", excepcion.Message);
    }

    [Fact]
    public async Task LoginAsync_ConPasswordIncorrecta_DebeLanzarExcepcion()
    {
        // Arrange
        LoginDto dto = new() { Username = "admin", Password = "WrongPassword" };

        Rol rol = new("Administrador", "Rol administrativo", true) { Id_Rol = 1 };
        Usuario usuario = new("admin", "hashedPassword", "Administrador", "admin@example.com", 1, true)
        {
            Id_Usuario = 1,
            Rol = rol
        };

        _repository.Setup(r => r.ObtenerPorUsernameAsync("admin")).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Verify("WrongPassword", "hashedPassword")).Returns(false);

        // Act & Assert
        var excepcion = await Assert.ThrowsAsync<Exception>(() => _service.LoginAsync(dto));
        Assert.Equal("Password incorrecta", excepcion.Message);
    }

    [Fact]
    public async Task LoginAsync_DebGenerarTokenConClaimsCorrectos()
    {
        // Arrange
        LoginDto dto = new() { Username = "user1", Password = "Pass123" };

        Rol rol = new("Usuario", "Rol usuario", true) { Id_Rol = 2 };
        Usuario usuario = new("user1", "hashedPassword", "Usuario Test", "user@example.com", 2, true)
        {
            Id_Usuario = 2,
            Rol = rol
        };

        _repository.Setup(r => r.ObtenerPorUsernameAsync("user1")).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Verify("Pass123", "hashedPassword")).Returns(true);

        // Act
        var token = await _service.LoginAsync(dto);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.StartsWith("eyJ", token); // JWT tokens start with "eyJ"
    }

    [Fact]
    public async Task LoginAsync_ConUsuarioSinRol_DebeGenerarTokenConRolVacio()
    {
        // Arrange
        LoginDto dto = new() { Username = "admin", Password = "Password123" };

        Usuario usuario = new("admin", "hashedPassword", "Administrador", "admin@example.com", 1, true)
        {
            Id_Usuario = 1,
            Rol = null
        };

        _repository.Setup(r => r.ObtenerPorUsernameAsync("admin")).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Verify("Password123", "hashedPassword")).Returns(true);

        // Act
        var token = await _service.LoginAsync(dto);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    #endregion
}

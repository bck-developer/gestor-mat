using Xunit;
using Moq;
using GestorMat.Application.Servicios;
using GestorMat.Application.Interfaces;
using GestorMat.Application.DTOs;
using GestorMat.Domain.Entidades;

namespace GestorMat.Tests.GestorMat.ApplicationTest.Servicios;

public class RolServiceTests
{
    private readonly Mock<IRolRepository> _repository = new();
    private readonly RolService _service;

    public RolServiceTests() =>
        _service = new RolService(_repository.Object);

    #region ObtenerAsync Tests

    [Fact]
    public async Task ObtenerAsync_ConRolesExistentes_DebeRetornarListaDtos()
    {
        // Arrange
        List<Rol> roles =
        [
            new() { Id_Rol = 1, RolName = "Admin", AccesoTotal = true },
            new() { Id_Rol = 2, RolName = "Usuario", AccesoTotal = true },
            new() { Id_Rol = 3, RolName = "Supervisor", AccesoTotal = false }
        ];

        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync(roles);

        // Act
        List<RolDto> resultado = await _service.ObtenerAsync();

        // Assert
        Assert.Equal(3, resultado.Count);
        Assert.Equal(("Admin", "Usuario", "Supervisor"),
                     (resultado[0].RolName, resultado[1].RolName, resultado[2].RolName));
    }

    [Fact]
    public async Task ObtenerAsync_SinRoles_DebeRetornarListaVacia()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync()).ReturnsAsync([]);

        // Act
        List<RolDto> resultado = await _service.ObtenerAsync();

        // Assert
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerAsync_ConUnRol_DebeRetornarCorrectamente()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync())
                   .ReturnsAsync([new() { Id_Rol = 1, RolName = "Admin", AccesoTotal = true }]);

        // Act
        List<RolDto> resultado = await _service.ObtenerAsync();

        // Assert
        Assert.Single(resultado);
        Assert.Equal("Admin", resultado[0].RolName);
    }

    [Fact]
    public async Task ObtenerAsync_MantieneLosIdsCorrectos()
    {
        // Arrange
        _repository.Setup(r => r.ObtenerTodosAsync())
                   .ReturnsAsync([
                       new() { Id_Rol = 10, RolName = "Admin", AccesoTotal = true },
                       new() { Id_Rol = 20, RolName = "Usuario", AccesoTotal = true },
                       new() { Id_Rol = 30, RolName = "Supervisor", AccesoTotal = false }
                   ]);

        // Act
        List<RolDto> resultado = await _service.ObtenerAsync();

        // Assert
        Assert.Equal((10, 20, 30),
                     (resultado[0].IdRol, resultado[1].IdRol, resultado[2].IdRol));
    }

    #endregion
}
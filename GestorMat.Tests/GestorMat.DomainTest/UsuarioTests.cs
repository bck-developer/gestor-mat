using GestorMat.Domain.Entidades;
using Xunit;

namespace GestorMat.Tests.GestorMat.DomainTest;

public class UsuarioTests
{
    [Fact]
    public void Usuario_AlCrearConDatosValidos_DebeTenerPropiedadesCorrectas()
    {
        Usuario usuario = new("testuser", "hashedpass", "Test User", "test@example.com", 1, true);
        Assert.Equal("testuser", usuario.Username);
        Assert.Equal("hashedpass", usuario.PasswordHash);
        Assert.Equal("Test User", usuario.Nombre);
        Assert.Equal("test@example.com", usuario.Mail);
        Assert.Equal(1, usuario.IdRol);
        Assert.True(usuario.Activo);
    }

    [Fact]
    public void Usuario_DebeTenerFechaAltaAlCrearse()
    {
        Usuario usuario = new("user2", "pass2", "User 2", "user2@test.com", 2, true);
        Assert.NotEqual(default(DateTime), usuario.FechaAlta);
        Assert.True(usuario.FechaAlta <= DateTime.UtcNow);
    }

    [Fact]
    public void Usuario_ActualizarDatos_DebeActualizarPropiedades()
    {
        Usuario usuario = new("user1", "oldpass", "Old Name", "old@test.com", 1, true);
        usuario.ActualizarDatos("newuser", "New Name", "new@test.com", 2, false);
        Assert.Equal("newuser", usuario.Username);
        Assert.Equal("New Name", usuario.Nombre);
        Assert.Equal("new@test.com", usuario.Mail);
        Assert.Equal(2, usuario.IdRol);
        Assert.False(usuario.Activo);
    }

    [Fact]
    public void Usuario_ActualizarDatos_ConUsernameVacio_DebeThrow()
    {
        Usuario usuario = new("user1", "pass", "Name", "mail@test.com", 1, true);
        Assert.Throws<Exception>(() => usuario.ActualizarDatos("", "Name", "mail@test.com", 1, true));
    }

    [Fact]
    public void Usuario_ActualizarDatos_ConNombreVacio_DebeThrow()
    {
        Usuario usuario = new("user1", "pass", "Name", "mail@test.com", 1, true);
        Assert.Throws<Exception>(() => usuario.ActualizarDatos("user", "", "mail@test.com", 1, true));
    }

    [Fact]
    public void Usuario_CambiarPassword_DebeActualizarPasswordHash()
    {
        Usuario usuario = new("user1", "oldpass", "Name", "mail@test.com", 1, true);
        usuario.CambiarPassword("newpass");
        Assert.Equal("newpass", usuario.PasswordHash);
    }

    [Fact]
    public void Usuario_Desactivar_DebeMarcarComoInactivo()
    {
        Usuario usuario = new("user1", "pass", "Name", "mail@test.com", 1, true);
        usuario.Desactivar();
        Assert.False(usuario.Activo);
    }

    [Fact]
    public void Usuario_Activar_DebeMarcarComoActivo()
    {
        Usuario usuario = new("user1", "pass", "Name", "mail@test.com", 1, false);
        usuario.Activar();
        Assert.True(usuario.Activo);
    }
}

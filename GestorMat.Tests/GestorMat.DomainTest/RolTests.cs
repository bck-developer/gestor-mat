using GestorMat.Domain.Entidades;
using Xunit;

namespace GestorMat.Tests.GestorMat.DomainTest;

public class RolTests
{
    [Fact]
    public void Rol_AlCrearConDatosValidos_DebeTenerPropiedadesCorrectas()
    {
        Rol rol = new("Admin", "Administrador del sistema", true);
        Assert.Equal("Admin", rol.RolName);
        Assert.Equal("Administrador del sistema", rol.Descripcion);
        Assert.True(rol.AccesoTotal);
    }

    [Fact]
    public void Rol_DebeTenerPropiedadIdRol()
    {
        Rol rol = new("User", "Usuario regular", false);
        Assert.NotNull(rol);
        Assert.False(rol.AccesoTotal);
    }

    [Fact]
    public void Rol_DebeTenerColeccionDeUsuarios()
    {
        Rol rol = new("Guest", "Invitado", false);
        Assert.NotNull(rol.Usuarios);
        Assert.IsType<List<Usuario>>(rol.Usuarios);
        Assert.Empty(rol.Usuarios);
    }
}

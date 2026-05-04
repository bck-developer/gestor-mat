using GestorMat.Domain.Entidades;
using Xunit;

namespace GestorMat.Tests.GestorMat.DomainTest;

public class UnidadMedidaTests
{
    [Fact]
    public void UnidadMedida_AlCrearConDatosValidos_DebeTenerPropiedadesCorrectas()
    {
        UnidadMedida unidad = new("Kilogramo", "kg", true);
        Assert.Equal("Kilogramo", unidad.Nombre);
        Assert.Equal("kg", unidad.Abreviatura);
        Assert.True(unidad.Activo);
    }

    [Fact]
    public void UnidadMedida_DebeTenerAbreviaturaNomeVacia()
    {
        UnidadMedida unidad = new("Litro", "lt", false);
        Assert.NotEmpty(unidad.Abreviatura);
        Assert.False(unidad.Activo);
    }

    [Fact]
    public void UnidadMedida_ConNombreVacio_DebeThrow()
    {
        Assert.Throws<ArgumentException>(() => new UnidadMedida("", "u", true));
    }

    [Fact]
    public void UnidadMedida_ConAbreviaturaVacia_DebeThrow()
    {
        Assert.Throws<ArgumentException>(() => new UnidadMedida("Unidad", "", true));
    }
}

using GestorMat.Domain.Entidades;
using Xunit;

namespace GestorMat.Tests.GestorMat.DomainTest;

public class DepositoTests
{
    [Fact]
    public void Deposito_AlCrearConDatosValidos_DebeTenerPropiedadesCorrectas()
    {
        Deposito deposito = new("DEP001", "Depósito 1", "Calle Principal 123", true);
        Assert.Equal("DEP001", deposito.CodigoDeposito);
        Assert.Equal("Depósito 1", deposito.Nombre);
        Assert.Equal("Calle Principal 123", deposito.Direccion);
        Assert.True(deposito.Habilitado);
    }

    [Fact]
    public void Deposito_ConCodigoVacio_DebeThrow()
    {
        Assert.Throws<Exception>(() => new Deposito("", "Name", "Address", true));
    }

    [Fact]
    public void Deposito_ConNombreVacio_DebeThrow()
    {
        Assert.Throws<Exception>(() => new Deposito("DEP001", "", "Address", true));
    }

    [Fact]
    public void Deposito_ConDireccionVacia_DebeThrow()
    {
        Assert.Throws<Exception>(() => new Deposito("DEP001", "Name", "", true));
    }

    [Fact]
    public void Deposito_DebeAceptarNombresLargos()
    {
        string nombreLargo = "Este es un nombre muy largo para un deposito que tiene muchas caracteres";
        Deposito deposito = new("DEP002", nombreLargo, "Address", false);
        Assert.Equal(nombreLargo, deposito.Nombre);
        Assert.False(deposito.Habilitado);
    }

    [Fact]
    public void Deposito_DebePermitirCodigosConMayusculas()
    {
        Deposito deposito = new("DEP-ALMACEN-001", "Almacén", "Dirección", true);
        Assert.Equal("DEP-ALMACEN-001", deposito.CodigoDeposito);
        Assert.True(deposito.Habilitado);
    }
}

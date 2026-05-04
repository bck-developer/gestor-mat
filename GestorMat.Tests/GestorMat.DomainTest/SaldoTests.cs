using GestorMat.Domain.Entidades;
using Xunit;

namespace GestorMat.Tests.GestorMat.DomainTest;

public class SaldoTests
{
    #region Helpers

    private Saldo CrearSaldo(
        int material = 1,
        int deposito = 1,
        double cantidad = 100,
        DateTime? fecha = null)
    {
        return new Saldo(
            material,
            deposito,
            cantidad,
            fecha ?? DateTime.UtcNow
        );
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Saldo_AlCrearConDatosValidos_DebeAsignarPropiedadesCorrectamente()
    {
        DateTime fecha = DateTime.UtcNow;

        Saldo saldo = CrearSaldo(1, 2, 50, fecha);

        Assert.Equal(1, saldo.Id_Material);
        Assert.Equal(2, saldo.Id_Deposito);
        Assert.Equal(50, saldo.Cantidad);
        Assert.Equal(fecha, saldo.FechaUltimaModificacion);
    }

    [Fact]
    public void Saldo_AlCrearConValoresMinimos_DebeAsignarCorrectamente()
    {
        DateTime fecha = DateTime.UtcNow;

        Saldo saldo = CrearSaldo(1, 1, 0, fecha);

        Assert.Equal(1, saldo.Id_Material);
        Assert.Equal(1, saldo.Id_Deposito);
        Assert.Equal(0, saldo.Cantidad);
    }

    [Fact]
    public void Saldo_AlCrearConCantidadGrande_DebeManejarlaBienI()
    {
        double cantidadGrande = double.MaxValue / 2;

        Saldo saldo = CrearSaldo(cantidad: cantidadGrande);

        Assert.Equal(cantidadGrande, saldo.Cantidad);
    }

    [Fact]
    public void Saldo_AlCrearConFechaNull_DebeUsarFechaActual()
    {
        DateTime tiempoAntes = DateTime.UtcNow.AddSeconds(-1);

        Saldo saldo = CrearSaldo(1, 1, 100, null);

        DateTime tiempoDespues = DateTime.UtcNow.AddSeconds(1);
        Assert.True(saldo.FechaUltimaModificacion >= tiempoAntes && saldo.FechaUltimaModificacion <= tiempoDespues);
    }

    #endregion

    #region Incrementar Tests

    [Fact]
    public void Incrementar_DebeSumarCantidadYActualizarFecha()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);
        DateTime fechaAntes = DateTime.Now.AddSeconds(-1);

        saldo.Incrementar(50);

        Assert.Equal(150, saldo.Cantidad);
        Assert.True(saldo.FechaUltimaModificacion >= fechaAntes);
    }

    [Fact]
    public void Incrementar_ConCantidadCero_NoDebeModificarSaldo()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);

        saldo.Incrementar(0);

        Assert.Equal(100, saldo.Cantidad);
    }

    [Fact]
    public void Incrementar_ConCantidadDecimal_DebeHandleFloatingPoint()
    {
        Saldo saldo = CrearSaldo(cantidad: 100.5);

        saldo.Incrementar(50.25);

        Assert.Equal(150.75, saldo.Cantidad);
    }

    [Fact]
    public void Incrementar_ConCantidadNegativa_DisminuyeSaldo_PorqueNoHayValidacion()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);

        saldo.Incrementar(-20);

        Assert.Equal(80, saldo.Cantidad);
    }

    [Fact]
    public void Incrementar_EnMultiplesOcasiones_DebeAcumular()
    {
        Saldo saldo = CrearSaldo(cantidad: 0);

        saldo.Incrementar(25);
        saldo.Incrementar(25);
        saldo.Incrementar(50);

        Assert.Equal(100, saldo.Cantidad);
    }

    #endregion

    #region Decrementar Tests

    [Fact]
    public void Decrementar_DebeRestarCantidadYActualizarFecha()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);
        DateTime fechaAntes = DateTime.Now.AddSeconds(-1);

        saldo.Decrementar(30);

        Assert.Equal(70, saldo.Cantidad);
        Assert.True(saldo.FechaUltimaModificacion >= fechaAntes);
    }

    [Fact]
    public void Decrementar_ConCantidadCero_NoDebeModificarSaldo()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);

        saldo.Decrementar(0);

        Assert.Equal(100, saldo.Cantidad);
    }

    [Fact]
    public void Decrementar_ConCantidadDecimal_DebeHandleFloatingPoint()
    {
        Saldo saldo = CrearSaldo(cantidad: 100.75);

        saldo.Decrementar(50.25);

        Assert.Equal(50.5, saldo.Cantidad);
    }

    [Fact]
    public void Decrementar_PuedeGenerarSaldoNegativo_PorqueNoHayValidacion()
    {
        Saldo saldo = CrearSaldo(cantidad: 10);

        saldo.Decrementar(50);

        Assert.Equal(-40, saldo.Cantidad);
    }

    [Fact]
    public void Decrementar_ConCantidadNegativa_IncrementaSaldo()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);

        saldo.Decrementar(-25);

        Assert.Equal(125, saldo.Cantidad);
    }

    [Fact]
    public void Decrementar_EnMultiplesOcasiones_DebeDisminuirGradualmente()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);

        saldo.Decrementar(25);
        saldo.Decrementar(25);
        saldo.Decrementar(25);

        Assert.Equal(25, saldo.Cantidad);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Saldo_ConOperacionesMixtas_DebeCalcularCorrectamente()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);

        saldo.Incrementar(50);
        saldo.Decrementar(30);
        saldo.Incrementar(20);

        Assert.Equal(140, saldo.Cantidad);
    }

    [Fact]
    public void Saldo_FechaDeModificacion_DebeActualizarseEnCadaOperacion()
    {
        Saldo saldo = CrearSaldo(cantidad: 100);
        DateTime fechaPrimera = DateTime.Now;

        System.Threading.Thread.Sleep(10);
        saldo.Incrementar(50);
        DateTime fechaSegunda = saldo.FechaUltimaModificacion;

        System.Threading.Thread.Sleep(10);
        saldo.Decrementar(25);
        DateTime fechaTercera = saldo.FechaUltimaModificacion;

        Assert.True(fechaSegunda > fechaPrimera);
        Assert.True(fechaTercera > fechaSegunda);
    }

    #endregion
}

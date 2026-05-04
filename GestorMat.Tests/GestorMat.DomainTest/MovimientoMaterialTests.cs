using GestorMat.Domain.Entidades;
using Xunit;

namespace GestorMat.Tests.GestorMat.DomainTest;

public class MovimientoMaterialTests
{
    #region Helpers

    private MovimientoMaterial CrearMovimiento(
        int idMaterial = 1,
        int idOrigen = 1,
        int? idDestino = 2,
        string codigo = "MOV001",
        string tipo = "INGRESO",
        double cantidad = 10,
        DateTime? fecha = null,
        string user = "admin")
    {
        return new MovimientoMaterial(
            idMaterial,
            idOrigen,
            idDestino,
            codigo,
            tipo,
            cantidad,
            fecha ?? DateTime.UtcNow,
            user
        );
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void MovimientoMaterial_AlCrearConDatosValidos_DebeAsignarPropiedadesCorrectamente()
    {
        DateTime fecha = DateTime.UtcNow;

        MovimientoMaterial mov = CrearMovimiento(
            idMaterial: 5,
            idOrigen: 1,
            idDestino: 2,
            codigo: "MOV123",
            tipo: "TRANSFERENCIA",
            cantidad: 50,
            fecha: fecha,
            user: "jdoe"
        );

        Assert.Equal(5, mov.Id_Material);
        Assert.Equal(1, mov.Id_DepositoOrigen);
        Assert.Equal(2, mov.Id_DepositoDestino);
        Assert.Equal("MOV123", mov.CodigoMovimiento);
        Assert.Equal("TRANSFERENCIA", mov.Tipo);
        Assert.Equal(50, mov.Cantidad);
        Assert.Equal(fecha, mov.Fecha);
        Assert.Equal("jdoe", mov.UserName);
    }

    [Fact]
    public void MovimientoMaterial_PuedeTenerDepositoDestinoNulo()
    {
        MovimientoMaterial mov = CrearMovimiento(idDestino: null);

        Assert.Null(mov.Id_DepositoDestino);
    }

    [Fact]
    public void MovimientoMaterial_DebeAceptarCantidadCero()
    {
        MovimientoMaterial mov = CrearMovimiento(cantidad: 0);

        Assert.Equal(0, mov.Cantidad);
    }

    #endregion

    #region Casos Borde (sin validación actual)

    [Fact]
    public void MovimientoMaterial_AceptaStringsVacios_PorqueNoHayValidacion()
    {
        MovimientoMaterial mov = CrearMovimiento(
            codigo: "",
            tipo: "",
            user: ""
        );

        Assert.Equal(string.Empty, mov.CodigoMovimiento);
        Assert.Equal(string.Empty, mov.Tipo);
        Assert.Equal(string.Empty, mov.UserName);
    }

    [Fact]
    public void MovimientoMaterial_AceptaCantidadNegativa_PorqueNoHayValidacion()
    {
        MovimientoMaterial mov = CrearMovimiento(cantidad: -10);

        Assert.Equal(-10, mov.Cantidad);
    }

    #endregion
}

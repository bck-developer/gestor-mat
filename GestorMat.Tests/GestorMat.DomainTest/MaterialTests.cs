using GestorMat.Domain.Entidades;
using Xunit;

namespace GestorMat.Tests.GestorMat.DomainTest;

public class MaterialTests
{
    #region Helpers

    private Material CrearMaterial(
        string codigo = "MAT001",
        string nombre = "Material 1",
        decimal precio = 100,
        int unidad = 1,
        bool activo = true,
        string descripcion = "Desc",
        bool permiteStockNegativo = false,
        double stockMinimo = 10)
    {
        return new Material(
            codigo,
            nombre,
            precio,
            unidad,
            activo,
            descripcion,
            permiteStockNegativo,
            stockMinimo
        );
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Material_AlCrearConDatosValidos_DebeTenerPropiedadesCorrectas()
    {
        Material material = CrearMaterial();

        Assert.Equal("MAT001", material.CodigoMaterial);
        Assert.Equal("Material 1", material.Nombre);
        Assert.Equal(100, material.Precio);
        Assert.Equal(1, material.Id_UnidadMedida);
        Assert.True(material.Activo);
        Assert.Equal("Desc", material.Descripcion);
        Assert.False(material.PermiteStockNegativo);
        Assert.Equal(10, material.StockMinimo);
    }

    [Fact]
    public void Material_ConNombreVacio_DebeThrow()
    {
        Assert.Throws<Exception>(() => CrearMaterial(nombre: ""));
    }

    [Fact]
    public void Material_ConPrecioCero_DebeThrow()
    {
        Assert.Throws<Exception>(() => CrearMaterial(precio: 0));
    }

    [Fact]
    public void Material_ConPrecioNegativo_DebeThrow()
    {
        Assert.Throws<Exception>(() => CrearMaterial(precio: -10));
    }

    #endregion

    #region ActualizarDatos Tests

    [Fact]
    public void ActualizarDatos_ConDatosValidos_DebeActualizarPropiedades()
    {
        Material material = CrearMaterial();

        material.ActualizarDatos(
            "MAT002",
            "Material 2",
            200,
            2,
            false,
            "Nueva desc",
            true,
            5
        );

        Assert.Equal("MAT002", material.CodigoMaterial);
        Assert.Equal("Material 2", material.Nombre);
        Assert.Equal(200, material.Precio);
        Assert.Equal(2, material.Id_UnidadMedida);
        Assert.False(material.Activo);
        Assert.Equal("Nueva desc", material.Descripcion);
        Assert.True(material.PermiteStockNegativo);
        Assert.Equal(5, material.StockMinimo);
    }

    [Fact]
    public void ActualizarDatos_ConNombreVacio_DebeThrow()
    {
        Material material = CrearMaterial();

        Assert.Throws<Exception>(() =>
            material.ActualizarDatos("MAT002", "", 100, 1, true, "Desc", false, 10));
    }

    [Fact]
    public void ActualizarDatos_ConPrecioInvalido_DebeThrow()
    {
        Material material = CrearMaterial();

        Assert.Throws<Exception>(() =>
            material.ActualizarDatos("MAT002", "Nombre", 0, 1, true, "Desc", false, 10));
    }

    #endregion

    #region Estado Tests

    [Fact]
    public void Desactivar_DebeMarcarComoInactivo()
    {
        Material material = CrearMaterial(activo: true);

        material.Desactivar();

        Assert.False(material.Activo);
    }

    [Fact]
    public void Activar_DebeMarcarComoActivo()
    {
        Material material = CrearMaterial(activo: false);

        material.Activar();

        Assert.True(material.Activo);
    }

    #endregion
}

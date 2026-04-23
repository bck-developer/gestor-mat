using System;
using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades;

public class Material
{
    public int Id_Material { get; set; }

    [StringLength(30)]
    public string CodigoMaterial { get; private set; } = string.Empty;

    [StringLength(80)]
    public string Nombre { get; private set; }

    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal Precio { get; private set; }

    public bool Activo { get; private set; }

    public int Id_UnidadMedida { get; private set; }

    [StringLength(255)]
    public string Descripcion { get; private set; }

    public bool PermiteStockNegativo { get; private set; }

    [Range(0, double.MaxValue)]
    public double StockMinimo { get; private set; }
    public UnidadMedida UnidadMedida { get; set; }

    private Material() { }

    public Material(
        string codigoMaterial,
        string nombre,
        decimal precio,
        int idUnidadMedida,
        bool activo,
        string descripcion,
        bool permiteStockNegativo,
        double stockMinimo)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }

        if (precio <= 0)
        {
            throw new Exception("El precio debe ser mayor a 0");
        }

        CodigoMaterial = codigoMaterial;
        Nombre = nombre;
        Precio = precio;
        Id_UnidadMedida = idUnidadMedida;
        Activo = activo;
        Descripcion = descripcion;
        PermiteStockNegativo = permiteStockNegativo;
        StockMinimo = stockMinimo;
    }


    public void ActualizarDatos(string codigoMaterial, string nombre, decimal precio, int idUnidadMedida, bool activo, string descripcion, bool permiteStockNegativo, double stockMinimo)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }

        if (precio <= 0)
        {
            throw new Exception("El precio debe ser mayor a 0");
        }

        CodigoMaterial = codigoMaterial;
        Nombre = nombre;
        Precio = precio;
        Id_UnidadMedida = idUnidadMedida;
        Activo = activo;
        Descripcion = descripcion;
        PermiteStockNegativo = permiteStockNegativo;
        StockMinimo = stockMinimo;
    }

    public void Desactivar()
    {
        Activo = false;
    }

    public void Activar()
    {
        Activo = true;
    }
}

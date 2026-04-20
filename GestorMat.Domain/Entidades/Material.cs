namespace GestorMat.Domain.Entidades;

public class Material
{
    public int Id_Material { get; private set; }
    public string Nombre { get; init; }
    public decimal Precio { get; init; }
    public bool Activo { get; init; }
    public int IdUnidadMedida { get; init; }
    public UnidadMedida UnidadMedida { get; init; }
    public string Descripcion { get; init; }
    
    public bool PermiteStockNegativo { get; init; }
    public double StockMinimo { get; init; }

    public Material(string nombre, decimal precio, int idUnidadMedida)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }

        if (precio <= 0)
        {
            throw new Exception("El precio debe ser mayor a 0");
        }

        Nombre = nombre;
        Precio = precio;
        IdUnidadMedida = idUnidadMedida;
        Activo = true;
    }
}

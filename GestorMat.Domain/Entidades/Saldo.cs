using System.ComponentModel.DataAnnotations;

namespace GestorMat.Domain.Entidades
{
    public class Saldo
    {
        [Key]
        public int Id_Saldo { get; private set; }
        public int Id_Material { get; private set; }
        public int Id_Deposito { get; private set; }
        public double Cantidad { get; private set; }
        public DateTime FechaUltimaModificacion { get; private set; }
        public Material Material { get; private set; }
        public Deposito Deposito { get; private set; }


        private Saldo() { }

        public Saldo(int idMaterial, int idDeposito, double cantidad, DateTime fecha)
        {
            Id_Material = idMaterial;
            Id_Deposito = idDeposito;
            Cantidad = cantidad;
            FechaUltimaModificacion = fecha;
        }

        public void Incrementar(double cantidad)
        {
            Cantidad += cantidad;
            FechaUltimaModificacion = DateTime.Now;
        }

        public void Decrementar(double cantidad)
        {
            Cantidad -= cantidad;
            FechaUltimaModificacion = DateTime.Now;
        }
    }
}

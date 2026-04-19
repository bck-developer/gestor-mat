using System;
using System.Collections.Generic;
using System.Text;

namespace GestorMat.Domain.Entidades
{
    public class Saldo
    {
        public int Id_Saldo { get; private set; }
        public int Id_Material { get; private set; }
        public int Id_Deposito { get; private set; }
        public decimal Cantidad { get; private set; }

        public Material Material { get; private set; }
        public Deposito Deposito { get; private set; }
    }
}

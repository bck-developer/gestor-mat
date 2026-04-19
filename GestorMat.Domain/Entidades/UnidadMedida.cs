using System;

namespace GestorMat.Domain.Entidades
{
    public class UnidadMedida
    {

            public int Id_UnidadMedida { get; private set; }
            public string Nombre { get; private set; }
            public string Abreviatura { get; private set; }
            public bool Activo { get; private set; }
        

        public UnidadMedida(string nombre, string abreviatura)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(abreviatura))
                throw new ArgumentException("La abreviatura es obligatoria");

            Nombre = nombre;
            Abreviatura = abreviatura;
            Activo = true;
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public void Actualizar(string nombre, string abreviatura)
        {
            Nombre = nombre;
            Abreviatura = abreviatura;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorMat.Domain.Entidades
{
    public class Rol
    {
        public int IdRol { get; private set; }
        public string RolName { get; private set; }
        public string Descripcion { get; private set; }
        public bool AccesoTotal { get; private set; }

        public ICollection<Usuario> Usuarios { get; private set; }

        // 👇 ESTE ES CLAVE PARA EF
        private Rol() { }

        public Rol(string rolName, string descripcion, bool accesoTotal)
        {
            RolName = rolName;
            Descripcion = descripcion;
            AccesoTotal = accesoTotal;
            Usuarios = new List<Usuario>();
        }
    }
}

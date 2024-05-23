using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public enum Jerarquia
    {
        Cliente,
        Administrador
    }
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string UrlImagenPerfil { get; set; }
        public Jerarquia Jerarquia { get; set; }
    }
}

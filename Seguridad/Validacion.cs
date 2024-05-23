using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Seguridad
{
    public static class Validacion
    {
        // Método para verificar si la sesión de un usuario está activa
        // Comprueba si el usuario proporcionado es válido y tiene un Id válido
        // Devuelve true si la sesión está activa, si no, devuelve false
        public static bool SesionActiva(object usuario)
        {
            // Convierte el objeto usuario en un objeto Usuario o establece usu en null si es nulo.
            Usuario user = usuario != null ? (Usuario)usuario : null;

            // Comprueba si user no es nulo y tiene un Id distinto de cero para determinar si la sesión está activa.
            if (user != null && user.Id != 0)
                return true; // La sesión está activa.
            else
                return false; // La sesión no está activa.
        }
    }
}

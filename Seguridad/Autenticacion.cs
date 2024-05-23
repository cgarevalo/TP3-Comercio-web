using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seguridad
{
    public static class Autenticacion
    {
        // Determina si un usuario tiene privilegios de administrador basándose en su jerarquía.
        public static bool EsAdministrador(object usuraio)
        {
            // Convierte el objeto en un Usuario.
            Usuario user = usuraio as Usuario;

            return user != null && user.Jerarquia == Jerarquia.Administrador; // Retorna false si no se cumplen ambas condiciones.
        }
    }
}

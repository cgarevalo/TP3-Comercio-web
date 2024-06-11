using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace comercio_web
{
    public partial class MiMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Carga la imágen por defecto de la imágen de perfíl
            imgPerfil.ImageUrl = "https://static.vecteezy.com/system/resources/previews/005/005/788/original/user-icon-in-trendy-flat-style-isolated-on-grey-background-user-symbol-for-your-web-site-design-logo-app-ui-illustration-eps10-free-vector.jpg";

            if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
            {
                // Si hay una sesión activa, obtiene los datos del usuario de la sesión y configura la interfaz de usuario
                Usuario usuario = (Usuario)Session["usuarioEnSesion"];

                if (!String.IsNullOrEmpty(usuario.Nombre))
                    lblUsuario.Text = usuario.Nombre;

                if (!String.IsNullOrEmpty(usuario.UrlImagenPerfil))
                    imgPerfil.ImageUrl = "~/Images/Perfiles/" + usuario.UrlImagenPerfil;
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}
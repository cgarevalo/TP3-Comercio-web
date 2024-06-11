using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace comercio_web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
                return;

            try
            {
                string email = txtEmail.Text;
                string pass = txtContraseña.Text;

                if (!String.IsNullOrWhiteSpace(email) || !String.IsNullOrWhiteSpace(pass))
                {
                    lblError.Text = "";
                    Usuario usuarioEntrante = new Usuario();
                    UsuarioNegocio negocioUsuario = new UsuarioNegocio();

                    usuarioEntrante.Email = email;
                    usuarioEntrante.Pass = pass;

                    if (negocioUsuario.IniciarSesion(usuarioEntrante))
                    {
                        Session.Add("usuarioEnSesion", usuarioEntrante);
                        Response.Redirect("MiPerfil.aspx", false);
                    }
                    else
                    {
                        lblError.Text = "La contraseña o el correo electrónico son incorrectos";
                        return;
                    }
                }
                else
                {
                    lblError.Text = "Debe llenar ambos campos";
                    return;
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx");
            }
        }
    }
}
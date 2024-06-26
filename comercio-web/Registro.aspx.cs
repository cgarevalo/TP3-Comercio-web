﻿using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace comercio_web
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptar_Click1(object sender, EventArgs e)
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

                    // Verifica que pass y email no superen los 20 y 100 caracteres, para que no de error
                    if (pass.Length > 20)
                    {
                        lblError.Text = "La contraseña no puede superar los 20 caracteres";
                        return;
                    }
                    if (email.Length > 100)
                    {
                        lblError.Text = "El correo no puede superar los 100 caracteres";
                        return;
                    }

                    lblError.Text = string.Empty;
                    Usuario nuevoUsuario = new Usuario();
                    UsuarioNegocio negocioUsuario = new UsuarioNegocio();

                    nuevoUsuario.Email = email;
                    nuevoUsuario.Pass = pass;
                    nuevoUsuario.Jerarquia = Jerarquia.Cliente; // Se registra como cliente por defecto

                    negocioUsuario.RegistrarUsuario(nuevoUsuario);
                    Response.Redirect("Login.aspx", false);
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
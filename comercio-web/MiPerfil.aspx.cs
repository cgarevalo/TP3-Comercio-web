using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace comercio_web
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
                {
                    Usuario usuarioConectado = (Usuario)Session["usuarioEnSesion"];

                    txtEmail.Text = usuarioConectado.Email;
                    txtNombre.Text = usuarioConectado.Nombre;
                    txtApellido.Text = usuarioConectado.Apellido;

                    if (!String.IsNullOrEmpty(usuarioConectado.UrlImagenPerfil))
                    {
                        // Ruta física completa del archivo en el servidor
                        string rutaFisica = Server.MapPath("~/Images/Perfiles/" + usuarioConectado.UrlImagenPerfil);

                        if (File.Exists(rutaFisica))
                            imgPerfil.Src = "~/Images/Perfiles/" + usuarioConectado.UrlImagenPerfil;
                        else
                            imgPerfil.Src = "https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg";
                    }
                    else
                    {
                        imgPerfil.Src = "https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg";
                    }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;

            if (String.IsNullOrWhiteSpace(nombre) && String.IsNullOrWhiteSpace(apellido) && !fudImagenPerfil.HasFile)
            {
                lblError.Text = "Debe ingresar al menos un nombre, apellido o cargar una imagen de perfil.";
                return;
            }

            // Verifica que nombre y apellido no superen los 50 caracteres, para que no de error
            if (nombre.Length > 50)
            {
                lblError.Text = "El nombre no puede superar los 50 caracteres";
                return;
            }
            if (apellido.Length > 50)
            {
                lblError.Text = "El apellido no puede superar los 50 caracteres";
                return;
            }

            try
            {
                // Elimina cualquier mensaje de error anterior
                lblError.Text = string.Empty;

                // Verificar si la sesión del usuario está activa
                if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
                {
                    UsuarioNegocio negocioUsuario = new UsuarioNegocio();
                    Usuario usuarioMod = (Usuario)Session["usuarioEnSesion"];

                    // Actualizar los campos si no están vacíos
                    if (!String.IsNullOrWhiteSpace(nombre))
                        usuarioMod.Nombre = nombre;
                    if (!String.IsNullOrWhiteSpace(apellido))
                        usuarioMod.Apellido = apellido;

                    // Maneja la carga de la imagen de perfil si se seleccionó una
                    if (fudImagenPerfil.HasFile)
                    {
                        try
                        {
                            // Obtiene la extensión del archivo subido
                            string extensionArchivo = Path.GetExtension(fudImagenPerfil.FileName).ToLower();

                            // Lista de extensiones de imagen permitidas
                            string[] extensiones = { ".jpg", ".jpeg", ".png", ".gif" };

                            // Verifica si la extensión del archivo subido está en la lista de extensiones permitidas
                            if (extensiones.Contains(extensionArchivo))
                            {
                                lblFudError.Text = string.Empty;
                                // Asigna el nombre de la imagen a UrlImagenPerfil de Usuario y la guarda
                                usuarioMod.UrlImagenPerfil = Seguridad.Utilidades.GuardarImagen(fudImagenPerfil.PostedFile, usuarioMod, Server.MapPath("~"));

                                // Carga la imagen en el imgPerfil
                                imgPerfil.Src = "~/Images/Perfiles/" + usuarioMod.UrlImagenPerfil;
                            }
                            else
                            {
                                // Si la extensión no es válida, muestra un mensaje de error y sale
                                lblFudError.Text = "Solo imágenes png, jpg, jpeg y gif";
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = "Error al cargar la imagen de perfil: " + ex.Message;
                            return;
                        }
                    }

                    // Actualiza al usuario en sesión
                    Session["usuarioEnSesion"] = usuarioMod;

                    // Actualiza los datos del usuario
                    negocioUsuario.ActualizarPerfil(usuarioMod);

                    // Redirige a la misma página para que se refleje el cambio en la foto de perfil, sin tener que recargar la página manualmente
                    Response.Redirect("MiPerfil.aspx", false);
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
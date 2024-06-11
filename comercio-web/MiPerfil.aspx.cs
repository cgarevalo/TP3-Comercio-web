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

            try
            {
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
                            // Obtiene la ruta de la carpeta
                            string ruta = Server.MapPath("~/Images/Perfiles/");

                            // Verifica si el usuario ya tiene una imagen de perfil
                            if (!String.IsNullOrEmpty(usuarioMod.UrlImagenPerfil))
                            {
                                // Ruta completa de la imagen antigua
                                string rutaImgAntigua = Path.Combine(ruta, usuarioMod.UrlImagenPerfil);

                                if (File.Exists(rutaImgAntigua))
                                {
                                    // Elimina la imagen antigua si existe
                                    File.Delete(rutaImgAntigua);
                                }
                            }

                            // Generar un nombre único para la imagen
                            string nombreImagen = $"user-{usuarioMod.Id.ToString()}-.jpg";

                            // Combina la ruta de la imagen con el nombre
                            string rutaCompleta = Path.Combine(ruta, nombreImagen);

                            // Guarda la imagen
                            fudImagenPerfil.SaveAs(rutaCompleta);

                            // Asigna el nombre de la imagen a UrlImagenPerfil de Usuario
                            usuarioMod.UrlImagenPerfil = nombreImagen;

                            // Carga la imagen en el imgPerfil
                            imgPerfil.Src = "~/Images/Perfiles/" + usuarioMod.UrlImagenPerfil;
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = "Error al cargar la imagen de perfil: " + ex.Message;
                            return;
                        }
                    }

                    // Actualiza los datos del usuario
                    negocioUsuario.ActualizarPerfil(usuarioMod);
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace comercio_web
{
    public partial class Detalles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != "")
                {
                    ComercioNegocio negocio = new ComercioNegocio();
                    string idArt = Request.QueryString["id"];

                    List<Articulo> articulo = negocio.ListarArticulos(idArt);

                    foreach (Articulo art in articulo)
                    {
                        if (!String.IsNullOrEmpty(art.Imagen))
                        {
                            if (art.Imagen.StartsWith("http://") || art.Imagen.StartsWith("https://"))
                                imgArticulo.ImageUrl = art.Imagen;
                            else
                            {
                                string rutaLocal = Server.MapPath("~/Images/Artículos/" + art.Imagen);
                                if (File.Exists(rutaLocal))
                                    imgArticulo.ImageUrl = "~/Images/Artículos/" + art.Imagen;
                                else
                                    imgArticulo.ImageUrl = "~/Images/Artículos/Default-Image.jpg";
                            }
                        }
                        else
                        {
                            imgArticulo.ImageUrl = "~/Images/Artículos/Default-Image.jpg";
                        }

                        txtCodigo.Text = art.CodigoArticulo;
                        txtNombre.Text = art.Nombre;
                        txtCategoria.Text = art.Categoria.ToString();
                        txtMarca.Text = art.Marca.ToString();
                        txtDescripcion.Text = art.Descripcion;
                        txtPrecio.Text = art.Precio.ToString("N2", new CultureInfo("es-ES"));
                    }
                }
            }
        }
    }
}
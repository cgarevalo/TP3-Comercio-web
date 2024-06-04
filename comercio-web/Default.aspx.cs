using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace comercio_web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Articulo> articulos = new List<Articulo>();
            ComercioNegocio negocio = new ComercioNegocio();
            articulos = negocio.ListarArticulos();

            foreach (Articulo art in articulos)
            {
                if (art.Imagen != null)
                {
                    if (!(art.Imagen.StartsWith("http://") || art.Imagen.StartsWith("https://")))
                    {
                        art.Imagen = "./Images/Artículos/" + art.Imagen;
                    }
                }
            }

            if (!IsPostBack)
            {
                repArticulos.DataSource = articulos;
                repArticulos.DataBind();
            }
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtBuscaProducto_TextChanged(object sender, EventArgs e)
        {
            //if (txtBuscaProducto.Text.Trim() == "")
            //{
            //    txtBuscaProducto.Text = "Buscar un producto...";
            //}
        }
    }
}
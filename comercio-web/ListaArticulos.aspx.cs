using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;

namespace comercio_web
{
    public partial class ListaArticulos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ComercioNegocio negocio = new ComercioNegocio();
                Session.Add("listaArticulos", negocio.ListarArticulos());
                dgvArticulos.DataSource = Session["listaArticulos"];
                dgvArticulos.DataBind();
            }
        }
    }
}
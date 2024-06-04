using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;
using System.Globalization;

namespace comercio_web
{
    public partial class ListaArticulos : System.Web.UI.Page
    {
        public bool FiltroAvanzado { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            FiltroAvanzado = chkFiltroAvanzado.Checked;

            if (!IsPostBack)
            {
                ComercioNegocio negocio = new ComercioNegocio();
                Session.Add("listaArticulos", negocio.ListarArticulos());
                dgvArticulos.DataSource = Session["listaArticulos"];
                dgvArticulos.DataBind();
            }
        }

        protected void dgvArticulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = dgvArticulos.SelectedDataKey.Value.ToString();
            Response.Redirect("FormAgregarArticulo.aspx?id=" + id);
        }

        protected void txtFiltroNombreCodigo_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtFiltroNombreCodigo.Text;

            List<Articulo> articulos = (List<Articulo>)Session["listaArticulos"];
            List<Articulo> listaFiltrada = articulos.FindAll(a => a.CodigoArticulo.ToLower().Contains(filtro.ToLower()) || a.Nombre.ToLower().Contains(filtro.ToLower()));

            dgvArticulos.DataSource = listaFiltrada;
            dgvArticulos.DataBind();
        }

        protected void chkFiltroAvanzado_CheckedChanged(object sender, EventArgs e)
        {
            // Sincroniza FiltroAvanzado con el chkFiltroAvanzado
            FiltroAvanzado = chkFiltroAvanzado.Checked;
            // Deshabilita el txtFiltroNombreCodigo si se dio en el chkFiltroAvanzado
            txtFiltroNombreCodigo.Enabled = !FiltroAvanzado;

            //if (FiltroAvanzado)
            //{
            //    ddlCampo.SelectedIndex = 0;
            //    ddlCampo_SelectedIndexChanged(sender, e);
            //}
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Vacía el contenido de ddlCriterio, antes de cargarlo, para que no se acumulen
            ddlCriterio.Items.Clear();

            // Switch para cargar los items de ddlCriterio
            switch (ddlCampo.SelectedItem.ToString())
            {
                // Categoría y marca tienen los mismos criterios
                case "Categoría":
                case "Marca":
                    ddlCriterio.Items.Add("Comienza con");
                    ddlCriterio.Items.Add("Contiene");
                    ddlCriterio.Items.Add("Termina con");
                    break;
                case "Precio":
                    ddlCriterio.Items.Add("Menor a");
                    ddlCriterio.Items.Add("Igual a");
                    ddlCriterio.Items.Add("Mayor a");
                    break;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string campo = ddlCampo.SelectedItem.ToString();
            string criterio = ddlCriterio.SelectedItem.ToString();
            string filtro = txtFiltroAvanzado.Text;

            try
            {
                if (campo == "Precio")
                {
                    if (Decimal.TryParse(filtro, out decimal precio))
                    {
                        filtro = precio.ToString("N2", new CultureInfo("es-ES"));
                    }
                    else
                    {
                        lblError.Text = "Ingrece un precio válido";
                        return;
                    }
                }

                ComercioNegocio negocio = new ComercioNegocio();
                dgvArticulos.DataSource = negocio.FiltrarArticulos(campo, criterio, filtro);
                dgvArticulos.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx");
            }
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            // Deselecciona los filtros
            ddlCampo.ClearSelection();
            ddlCriterio.ClearSelection();
            // Vacía el txtFiltroAvanzado
            txtFiltroAvanzado.Text = "";

            // Vacía a ddlCriterio
            ddlCriterio.Items.Clear();

            // Restaurar la lista completa de datos
            dgvArticulos.DataSource = Session["listaArticulos"];
            dgvArticulos.DataBind();
        }

        protected void dgvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Establece la fuente de datos del GridView a la lista de libros almacenada en la sesión.
            dgvArticulos.DataSource = Session["listaArticulos"];
            dgvArticulos.DataBind();

            dgvArticulos.PageIndex = e.NewPageIndex;
            dgvArticulos.DataBind();
        }
    }
}
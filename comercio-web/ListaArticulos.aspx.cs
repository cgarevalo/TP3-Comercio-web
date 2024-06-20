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
            if (!(Seguridad.Autenticacion.EsAdministrador(Session["usuarioEnSesion"])))
            {
                Session.Add("error", "Se requieren permisos de administrador para estár en la página");
                Response.Redirect("Error.aspx", false);
                return;
            }

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
            Response.Redirect($"FormAgregarArticulo.aspx?id={id}");
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
            ActualizarFiltroAvanzado();
            LimpiarFiltro();
        }

        private void ActualizarFiltroAvanzado()
        {
            // Actualiza la propiedad FiltroAvanzado basado en el estado del chkFiltro
            FiltroAvanzado = chkFiltroAvanzado.Checked;

            // Habilita o deshabilita el txtBuscarProducto de búsqueda de producto según el estado del FiltroAvanzado
            txtFiltroNombreCodigo.Enabled = !FiltroAvanzado;

            // Habilita o deshabilita el ddlCampo de campo según el estado del chkFiltro
            ddlCampo.Enabled = chkFiltroAvanzado.Checked;
        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilita o deshabilita ddlCriterio dependiendo de lo que se haya selecionado en ddlCampo
            if (ddlCampo.SelectedItem.ToString() != "Seleccione un campo")
                ddlCriterio.Enabled = true;          
            else
            {
                // Deshabilita ddlCriterio, txtFiltroAvanzado y btnBuscar si se seleccionó "Seleccione un campo"
                ddlCriterio.Enabled = false;
                txtFiltroAvanzado.Enabled = ddlCriterio.Enabled;
                btnBuscar.Enabled = ddlCriterio.Enabled;
            }

            // Vacía el contenido de ddlCriterio, antes de cargarlo, para que no se acumulen
            ddlCriterio.Items.Clear();

            // Switch para cargar los items de ddlCriterio
            switch (ddlCampo.SelectedItem.ToString())
            {
                // Categoría y marca tienen los mismos criterios
                case "Categoría":
                case "Marca":
                    ddlCriterio.Items.Add("Seleccione un criterio");
                    ddlCriterio.Items.Add("Comienza con");
                    ddlCriterio.Items.Add("Contiene");
                    ddlCriterio.Items.Add("Termina con");
                    break;
                case "Precio":
                    ddlCriterio.Items.Add("Seleccione un criterio");
                    ddlCriterio.Items.Add("Menor a");
                    ddlCriterio.Items.Add("Igual a");
                    ddlCriterio.Items.Add("Mayor a");
                    break;
            }
        }

        protected void ddlCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilita o deshabilita el txtFiltroAvanzado dependiendo del estado de ddlCriterio, o lo que se haya selecionado
            if (ddlCriterio.SelectedItem.ToString() != "Seleccione un criterio")
            {
                txtFiltroAvanzado.Enabled = true;
            }                
            else
            {
                txtFiltroAvanzado.Enabled = false;
            }

            btnBuscar.Enabled = txtFiltroAvanzado.Enabled;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string campo = ddlCampo.SelectedItem.ToString();
            string criterio = ddlCriterio.SelectedItem.ToString();
            string filtro = txtFiltroAvanzado.Text;

            if (String.IsNullOrEmpty(campo) || String.IsNullOrEmpty(criterio) || String.IsNullOrWhiteSpace(filtro))
            {
                lblError.Text = "No puede dejar nada vacío";
                return;
            }

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

                lblError.Text = string.Empty;
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
            LimpiarFiltro();
        }

        private void LimpiarFiltro()
        {
            // Deselecciona los filtros
            ddlCampo.ClearSelection();
            ddlCriterio.ClearSelection();

            // Vacía a ddlCriterio
            ddlCriterio.Items.Clear();

            // Deshabilita ddlCriterio
            ddlCriterio.Enabled = false;

            // Vacía el txtFiltroAvanzado
            txtFiltroAvanzado.Text = string.Empty;

            // Deshabilita txtFiltroAvanzado
            txtFiltroAvanzado.Enabled = false;

            // Limpia cualquier mensaje de error previo
            lblError.Text = string.Empty;

            // Deshabilita btnBuscar
            btnBuscar.Enabled = false;

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
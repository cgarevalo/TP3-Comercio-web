using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace comercio_web
{
    public partial class Default : System.Web.UI.Page
    {
        public bool FiltroAvanzado { get; set; }
        
        ComercioNegocio negocio = new ComercioNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarFavoritos();

            if (!IsPostBack)
            {
                List<Articulo> articulos = new List<Articulo>();

                // Obtiene la lista de artículos
                articulos = ObtenerArticulos();
                // Almacena la lista de artículos en sesión
                Session.Add("listaArticulos", articulos);
                repArticulos.DataSource = articulos;
                repArticulos.DataBind();
            }
        }

        protected void txtBuscarProducto_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscarProducto.Text;

            // Obtiene la lista de artículos en sesión
            List<Articulo> articulos = (List<Articulo>)Session["listaArticulos"];

            // Filtra la lista de artículos basándose en el texto del filtro (por código de artículo o nombre)
            List<Articulo> listaFiltrada = articulos.FindAll(a => a.CodigoArticulo.ToLower().Contains(filtro.ToLower()) || a.Nombre.ToLower().Contains(filtro.ToLower()));

            repArticulos.DataSource = listaFiltrada;
            repArticulos.DataBind();
        }

        protected void chkFiltro_CheckedChanged(object sender, EventArgs e)
        {
            // Actualiza la propiedad FiltroAvanzado basado en el estado del chkFiltro
            FiltroAvanzado = chkFiltro.Checked;

            // Habilita o deshabilita el txtBuscarProducto de búsqueda de producto según el estado del FiltroAvanzado
            txtBuscarProducto.Enabled = !FiltroAvanzado;

            // Habilita o deshabilita el ddlCampo de campo según el estado del chkFiltro
            ddlCampo.Enabled = chkFiltro.Checked;

        }

        protected void ddlCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Habilita ddlCriterio
            ddlCriterio.Enabled = true;

            // Habilita btnBuscar
            btnBuscar.Enabled = true;

            // Vacía el contenido de ddlCriterio, antes de cargarlo, para que no se acumulen
            ddlCriterio.Items.Clear();

            btnLimpiarFiltro.Enabled = true;

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
            txtFiltro.Enabled = true;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Verifica si el campo de texto del filtro no está vacío
            if (!String.IsNullOrWhiteSpace(txtFiltro.Text))
            {
                string campo = ddlCampo.SelectedItem.ToString();
                string criterio = ddlCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;

                try
                {
                    // Si el campo seleccionado es "Precio", convierte el valor del filtro a decimal
                    if (campo == "Precio")
                    {
                        if (Decimal.TryParse(filtro, out decimal precio))
                        {
                            // Formatea el valor del filtro como un número con dos decimales
                            filtro = precio.ToString("N2", new CultureInfo("es-ES"));
                            lblError.Text = ""; // Limpia cualquier mensaje de error previo
                        }
                        else
                        {
                            // Si la conversión falla, muestra un mensaje de error y salir del método
                            lblError.Text = "Ingrece un precio válido";
                            return;
                        }
                    }

                    // Filtra los artículos según el campo, criterio y valor del filtro
                    List<Articulo> articulos = negocio.FiltrarArticulos(campo, criterio, filtro);
                    // Actualiza las URLs de las imágenes de los artículos
                    ArmarUrlImagen(articulos);

                    // Enlaza los artículos filtrados al control Repeater
                    repArticulos.DataSource = articulos;
                    repArticulos.DataBind();
                }
                catch (Exception ex)
                {
                    Session.Add("error", ex.Message);
                    Response.Redirect("Error.aspx");
                }
            }
            else
            {
                lblError.Text = "No puede dejar el filtro vacío";
                return;
            }
        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            // Deselecciona los filtros
            ddlCampo.ClearSelection();
            ddlCriterio.ClearSelection();

            // Vacía a ddlCriterio
            ddlCriterio.Items.Clear();

            // Deshabilita ddlCriterio
            ddlCriterio.Enabled = false;

            // Vacía el txtFiltroAvanzado
            txtFiltro.Text = "";

            // Deshabilita txtFiltro
            txtFiltro.Enabled = false;

            // Obtiene la lista completa de artículos
            List<Articulo> articulos = ObtenerArticulos();

            // Desactiva los botones de limpiar filtro y buscar
            btnLimpiarFiltro.Enabled = false;
            btnBuscar.Enabled = false;

            // Limpia cualquier mensaje de error previo
            lblError.Text = "";

            // Enlaza la lista completa de artículos al control Repeater
            repArticulos.DataSource = articulos;
            repArticulos.DataBind();
        }

        private List<Articulo> ObtenerArticulos()
        {
            // Obtiene la lista de artículos
            List<Articulo> articulos = negocio.ListarArticulos();

            // Actualiza las URL de las imágenes de los artículos
            ArmarUrlImagen(articulos);

            // Devuelve la lista
            return articulos;
        }

        private void ArmarUrlImagen(List<Articulo> articulos)
        {
            // Recorre cada artículo en la lista
            foreach (Articulo art in articulos)
            {
                // Verifica si Imagen no está vacío o en blanco
                if (!String.IsNullOrWhiteSpace(art.Imagen))
                {
                    // Si la URL de la imagen no comienza con http:// o https://
                    if (!(art.Imagen.StartsWith("http://") || art.Imagen.StartsWith("https://")))
                    {
                        art.Imagen = "./Images/Artículos/" + art.Imagen;
                    }
                }
                else
                {
                    // Si Imagen está vacío
                    art.Imagen = "https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg";
                }
            }
        }

        protected void btnIconoFav_Click(object sender, ImageClickEventArgs e)
        {
            if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
            {
                ImageButton btnIcono = (ImageButton)sender;
                UsuarioNegocio negocioUser = new UsuarioNegocio();

                int idUsuario = ((Usuario)Session["usuarioEnSesion"]).Id;
                int idArticulo = int.Parse(btnIcono.CommandArgument);

                List<Favorito> listaFavoritos = (List<Favorito>)Session["usuarioFavoritos"];

                Favorito favorito = listaFavoritos.FirstOrDefault(f => f.IdUsuario == idUsuario && f.IdArticulo == idArticulo);

                if (negocioUser.VerificarFavorito(idUsuario, idArticulo))
                {
                    negocioUser.EliminarFavorito(favorito);
                    listaFavoritos.Remove(favorito);
                    btnIcono.ImageUrl = "Images/Iconos/bookmark.svg";
                }
                else
                {
                    Favorito nuevoFavorito = new Favorito();
                    nuevoFavorito.IdUsuario = idUsuario;
                    nuevoFavorito.IdArticulo = idArticulo;

                    btnIcono.ImageUrl = "Images/Iconos/bookmark-check.svg";
                    negocioUser.AgregarFavorito(nuevoFavorito);
                    listaFavoritos.Add(nuevoFavorito);
                }

                // Actualiza la sesión con la lista de favoritos actualizada
                Session["usuarioFavoritos"] = listaFavoritos;
            }
            else
            {
                // Si no está logueado, lo manda al login 
                Response.Redirect("Login.aspx");
            }
        }

        private void CargarFavoritos()
        {
            if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
            {
                UsuarioNegocio negocioUser = new UsuarioNegocio();
                int idUsuario = ((Usuario)Session["usuarioEnSesion"]).Id;

                if (Session["usuarioEnSesion"] != null)
                {
                    List<Favorito> listaFavoritos = negocioUser.ObtenerFavoritos(idUsuario);
                    Session.Add("usuarioFavoritos", listaFavoritos);
                }              
            }
        }

        protected string ImagenBotonFav(object idArticulo)
        {
            List<Favorito> listaFavoritos = (List<Favorito>)Session["usuarioFavoritos"];

            if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
            {
                int idArt = int.Parse(idArticulo.ToString());

                if (listaFavoritos.Any(f => f.IdArticulo == idArt))
                    return "Images/Iconos/bookmark-check.svg";
                else
                    return "Images/Iconos/bookmark.svg";
            }
            else
            {
                return "Images/Iconos/bookmark.svg";
            }
        }
    }
}
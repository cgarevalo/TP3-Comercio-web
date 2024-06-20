using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
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
                Inicializar();
            }
        }

        private void Inicializar()
        {
            List<Articulo> articulos = new List<Articulo>();

            // Obtiene la lista de artículos
            articulos = ObtenerArticulos();

            // Almacena la lista de artículos en sesión
            Session.Add("listaArticulos", articulos);

            repArticulos.DataSource = articulos;
            repArticulos.DataBind();
        }

        // Método para buscar un artículo por nombre o código
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

        // Habilita al filtro
        protected void chkFiltro_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarFiltroAvanzado();
            LimpiarFiltro();
        }

        private void ActualizarFiltroAvanzado()
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
            // Habilita o deshabilita ddlCriterio dependiendo de lo que se haya seleccionado en ddlCampo
            if (ddlCampo.SelectedItem.ToString() != "Seleccione un campo")
                ddlCriterio.Enabled = true;
            else
            {
                // Deshabilita ddlCriterio, txtFiltro y btnBuscar si se seleccionó "Seleccione un campo"
                ddlCriterio.Enabled = false;
                txtFiltro.Enabled = ddlCriterio.Enabled;
                btnBuscar.Enabled = ddlCriterio.Enabled;
            }

            ddlCriterio.Enabled = ddlCampo.SelectedItem.ToString() != "Seleccione un campo";

            // Vacía el contenido de ddlCriterio, antes de cargarlo, para que no se acumulen
            ddlCriterio.Items.Clear();

            // Habilita el botón para filtrar
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
            // Habilita o deshabilita el txtFiltro dependiendo de lo que se haya selecionado en ddlCriterio
            txtFiltro.Enabled = ddlCriterio.SelectedItem.ToString() != "Seleccione un criterio";

            // Habilita btnBuscar
            btnBuscar.Enabled = txtFiltro.Enabled;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Verifica si el campo de texto del filtro no está vacío
            if (String.IsNullOrWhiteSpace(txtFiltro.Text))
            {
                lblError.Text = "No puede dejar el filtro vacío";
                return;
            }

            string campo = ddlCampo.SelectedItem.ToString();
            string criterio = ddlCriterio.SelectedItem.ToString();
            string filtro = txtFiltro.Text;

            if (!ValidarEntradas(campo, criterio, filtro))
                return;

            try
            {

                // Si el campo seleccionado es "Precio", convierte el valor del filtro a decimal
                if (campo == "Precio")
                {
                    if (Decimal.TryParse(filtro, out decimal precio))
                    {
                        // Formatea el valor del filtro como un número con dos decimales
                        filtro = precio.ToString("N2", new CultureInfo("es-ES"));
                        lblError.Text = string.Empty; // Limpia cualquier mensaje de error previo
                    }
                    else
                    {
                        // Si la conversión falla, muestra un mensaje de error y sale del método
                        lblError.Text = "Ingrece un precio válido";
                        return;
                    }
                }

                // Filtra los artículos según el campo, criterio y valor del filtro
                List<Articulo> articulos = negocio.FiltrarArticulos(campo, criterio, filtro);

                // Actualiza las URLs de las imágenes de los artículos
                Seguridad.Utilidades.ArmarUrlImagen(articulos);

                lblError.Text = string.Empty; // Limpia cualquier mensaje de error previo

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

        private bool ValidarEntradas(string campo, string criterio, string filtro)
        {
            // Verifica si se seleccionó algo válido en ddlCampo
            if (campo == "Seleccione un campo")
            {
                lblError.Text = "Debe seleccionar un campo";
                return false;
            }

            // Verifica si se seleccionó algo válido en ddlCriterio
            if (criterio == "Seleccione un criterio")
            {
                lblError.Text = "Debe seleccionar un criterio";
                return false;
            }

            // Verifica si campo, criterio y filtro tengan algo
            if (String.IsNullOrEmpty(campo) || String.IsNullOrEmpty(criterio) || String.IsNullOrWhiteSpace(filtro))
            {
                lblError.Text = "No puede dejar nada vacío";
                return false;
            }

            return true;
        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            // Limpia todos los filtros y restablece los controles
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
            txtFiltro.Text = string.Empty;

            // Deshabilita txtFiltro
            txtFiltro.Enabled = false;

            // Obtiene la lista completa de artículos
            List<Articulo> articulos = ObtenerArticulos();

            // Desactiva los botones de limpiar filtro y buscar
            btnLimpiarFiltro.Enabled = false;
            btnBuscar.Enabled = false;

            // Limpia cualquier mensaje de error previo
            lblError.Text = string.Empty;

            // Enlaza la lista completa de artículos al control Repeater
            repArticulos.DataSource = articulos;
            repArticulos.DataBind();
        }

        // Método que obtiene la lista de artículos
        private List<Articulo> ObtenerArticulos()
        {
            // Obtiene la lista de artículos
            List<Articulo> articulos = negocio.ListarArticulos();

            // Actualiza las URL de las imágenes de los artículos
            Seguridad.Utilidades.ArmarUrlImagen(articulos);

            // Devuelve la lista
            return articulos;
        }

        // Método para eliminar o agregar un favorito
        protected void btnIconoFav_Click(object sender, ImageClickEventArgs e)
        {
            if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
            {
                // Captura el botón que disparó el evento
                ImageButton btnIcono = (ImageButton)sender;
                UsuarioNegocio negocioUser = new UsuarioNegocio();

                // Obtiene el ID del usuario desde la sesión y el ID del artículo desde el argumento del botón
                int idUsuario = ((Usuario)Session["usuarioEnSesion"]).Id;
                int idArticulo = int.Parse(btnIcono.CommandArgument);

                // Obtiene la lista de favoritos del usuario almacenado en sesión
                List<Favorito> listaFavoritos = (List<Favorito>)Session["usuarioFavoritos"];

                // Busca si el artículo ya está en la lista de favoritos del usuario
                Favorito favorito = listaFavoritos.FirstOrDefault(f => f.IdUsuario == idUsuario && f.IdArticulo == idArticulo);

                // Verifica si el artículo ya está marcado como favorito
                if (negocioUser.VerificarFavorito(idUsuario, idArticulo))
                {
                    // Si el artículo está marcado como favorito, lo elimina de la base de datos y de la lista de favoritos
                    negocioUser.EliminarFavorito(favorito);
                    listaFavoritos.Remove(favorito);

                    // Actualiza el ícono del botón a no favorito
                    btnIcono.ImageUrl = "Images/Iconos/bookmark.svg";
                }
                else
                {
                    // Si el artículo no está marcado como favorito, crea un nuevo favorito y lo agrega a la base de datos y a la lista de favoritos
                    Favorito nuevoFavorito = new Favorito();
                    nuevoFavorito.IdUsuario = idUsuario;
                    nuevoFavorito.IdArticulo = idArticulo;

                    // Actualiza el ícono del botón a favorito
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

        // Método para asignar el ícono correspondiente al botón de favorito
        protected string ImagenBotonFav(object idArticulo)
        {
            // Obtiene la lista de favoritos del usuario almacenada en sesión
            List<Favorito> listaFavoritos = (List<Favorito>)Session["usuarioFavoritos"];

            if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
            {
                int idArt = int.Parse(idArticulo.ToString());

                // Verifica si el artículo está en la lista de favoritos del usuario
                if (listaFavoritos.Any(f => f.IdArticulo == idArt))
                    return "Images/Iconos/bookmark-check.svg"; // Si está en la lista, devuelve el ícono de favorito
                else
                    return "Images/Iconos/bookmark.svg"; // Si no está en la lista, devuelve el ícono de no favorito
            }
            else
            {
                // Si la sesión no está activa, devuelve el ícono de no marcado como favorito
                return "Images/Iconos/bookmark.svg";
            }
        }

        // Método que obtiene la imagen del artículo y en caso de no existir retorna una por defecto
        protected string ObtenerImagenArt(object imagen)
        {
            // Verifica si la ruta de la imagen no es nula o vacía
            if (!String.IsNullOrEmpty(imagen.ToString()))
            {
                // Si la ruta comienza con "http://" o "https://"
                if (imagen.ToString().StartsWith("https://") || imagen.ToString().StartsWith("http://"))
                {
                    return imagen.ToString();
                }
                else
                {
                    // Si la imagen es local, verifica si el archivo existe en el servidor
                    string rutaLOcal = Server.MapPath(imagen.ToString());
                    if (File.Exists(rutaLOcal))
                    {
                        return imagen.ToString();
                    }
                }
            }

            // Si la imagen no existe o la ruta está vacía, devuelve la imagen por defecto
            return "./Images/Artículos/Default-Image.jpg";
        }

        protected void btnVerDetalles_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string idArt = btn.CommandArgument;

            Response.Redirect($"Detalles.aspx?id={idArt}");
        }
    }
}
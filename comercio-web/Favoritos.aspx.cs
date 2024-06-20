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
    public partial class Favoritos : System.Web.UI.Page
    {
        public List<Articulo> ArticulosFavoritos { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ArticulosFavoritos = ObtenerListaArticulos();

                // Actualiza las URL de las imágenes de los artículos
                Seguridad.Utilidades.ArmarUrlImagen(ArticulosFavoritos);

                repArticulosFav.DataSource = ArticulosFavoritos;
                repArticulosFav.DataBind();
            }
        }

        private List<Articulo> ObtenerListaArticulos()
        {
            if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
            {
                UsuarioNegocio negocioUser = new UsuarioNegocio();
                List<Favorito> listaFavoritos = CargarFavoritos();
                List<int> listaIds = new List<int>();

                foreach (Favorito fav in listaFavoritos)
                {
                    listaIds.Add(fav.IdArticulo);
                }

                return negocioUser.ObtenerListaArticulosFav(listaIds);
            }

            return new List<Articulo>();
        }

        private List<Favorito> CargarFavoritos()
        {
            if (Seguridad.Validacion.SesionActiva(Session["usuarioEnSesion"]))
            {
                UsuarioNegocio negocioUser = new UsuarioNegocio();
                int idUsuario = ((Usuario)Session["usuarioEnSesion"]).Id;

                if (Session["usuarioEnSesion"] != null)
                {
                    List<Favorito> listaFavoritos = negocioUser.ObtenerFavoritos(idUsuario);
                    Session.Add("usuarioFavoritos", listaFavoritos);

                    return listaFavoritos;
                }
            }

            return new List<Favorito>();
        }
    }
}
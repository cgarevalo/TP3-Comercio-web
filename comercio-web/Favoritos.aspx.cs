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
                ActualizarRepeater();
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

                    List<Favorito> listaFavoritos = negocioUser.ObtenerFavoritos(idUsuario);
                    Session.Add("usuarioFavoritos", listaFavoritos);

                    return listaFavoritos;
            }

            return new List<Favorito>();
        }

        protected void btnQuitarFavorito_Click(object sender, EventArgs e)
        {
            try
            {               
                Button boton = (Button)sender;

                // Obtiene el ID del usuario desde la sesión y el ID del artículo desde el argumento del botón
                int idUser = ((Usuario)Session["usuarioEnSesion"]).Id;
                int idArticulo = int.Parse(boton.CommandArgument);

                EliminarFavorito(idUser, idArticulo);
                ActualizarRepeater();
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx");
            }
        }

        private void EliminarFavorito(int idUser, int idArticulo)
        {
            UsuarioNegocio negocioUser = new UsuarioNegocio();
            List<Favorito> listaFavoritos = (List<Favorito>)Session["usuarioFavoritos"];

            // Encuantra el favorito en la lista en sesión de favoritos
            Favorito fav = listaFavoritos.FirstOrDefault(f => f.IdUsuario == idUser && f.IdArticulo == idArticulo);

            // Elimina el favorito de la base de datos
            negocioUser.EliminarFavorito(fav);
            // Remueve el favorito de la lista de favoritos
            listaFavoritos.Remove(fav);
            // Actualiza la lista de favoritos en sesión con la lista anterior
            Session["usuarioFavoritos"] = listaFavoritos;
        }

        private void ActualizarRepeater()
        {
            // Actualiza el repeater
            ArticulosFavoritos = ObtenerListaArticulos();
            // Actualiza las URL de las imágenes de los artículos
            Seguridad.Utilidades.ArmarUrlImagen(ArticulosFavoritos);
            repArticulosFav.DataSource = ArticulosFavoritos;
            repArticulosFav.DataBind();
        }
    }
}
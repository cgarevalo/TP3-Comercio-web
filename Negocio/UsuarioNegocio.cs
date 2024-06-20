using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Dominio;

namespace Negocio
{
    public class UsuarioNegocio
    {
        public void RegistrarUsuario(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();
            string consulta = "INSERT INTO USERS (email,pass, admin) VALUES(@email, @pass, @admin)";

            try
            {
                datos.SetearConsulta(consulta);

                datos.SetearParametro("@email", usuario.Email);
                datos.SetearParametro("@pass", usuario.Pass);

                switch (usuario.Jerarquia)
                {
                    case Jerarquia.Cliente:
                        datos.SetearParametro("@admin", 0);
                        break;
                    case Jerarquia.Administrador:
                        datos.SetearParametro("@admin", 1);
                        break;
                }

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool IniciarSesion(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();
            string consulta = @"SELECT Id, email, Pass, nombre, apellido, urlImagenPerfil, admin
            FROM USERS
            WHERE email = @email AND pass = @pass
            ";

            try
            {
                datos.SetearConsulta(consulta);

                datos.SetearParametro("@email", usuario.Email);
                datos.SetearParametro("@pass", usuario.Pass);

                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario.Id = (int)datos.Lector["Id"];
                    if (!(datos.Lector["nombre"] is DBNull))
                        usuario.Nombre = (string)datos.Lector["nombre"];
                    if (!(datos.Lector["apellido"] is DBNull))
                        usuario.Apellido = (string)datos.Lector["apellido"];
                    if (!(datos.Lector["urlImagenPerfil"] is DBNull))
                        usuario.UrlImagenPerfil = (string)datos.Lector["urlImagenPerfil"];

                    if ((bool)datos.Lector["admin"])
                        usuario.Jerarquia = Jerarquia.Administrador;
                    else
                        usuario.Jerarquia = Jerarquia.Cliente;

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ActualizarPerfil(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            string consulta = "UPDATE USERS SET nombre = @nombre, apellido = @apellido, urlImagenPerfil = @urlImagen WHERE Id = @id";

            try
            {
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@id", usuario.Id);
                datos.SetearParametro("@nombre", usuario.Nombre ?? (object)DBNull.Value);
                datos.SetearParametro("@apellido", usuario.Apellido ?? (object)DBNull.Value);
                datos.SetearParametro("@urlImagen", usuario.UrlImagenPerfil ?? (object)DBNull.Value);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void AgregarFavorito(Favorito favorito)
        {
            AccesoDatos datos = new AccesoDatos();
            string consulta = "INSERT INTO FAVORITOS (IdUser, IdArticulo) VALUES(@idUser, @idArticulo)";

            try
            {
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@idUser", favorito.IdUsuario);
                datos.SetearParametro("@idArticulo", favorito.IdArticulo);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Favorito> ObtenerFavoritos(int idUsuario)
        {
            string consulta = "SELECT Id, IdUser, IdArticulo FROM FAVORITOS WHERE IdUser = @idUser";
            AccesoDatos datos = new AccesoDatos();
            List<Favorito> listaFavoritos = new List<Favorito>();

            try
            {
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@idUser", idUsuario);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Favorito favorito = new Favorito();
                    favorito.Id = (int)datos.Lector["Id"];
                    favorito.IdUsuario = (int)datos.Lector["IdUser"];
                    favorito.IdArticulo = (int)datos.Lector["IdArticulo"];

                    listaFavoritos.Add(favorito);
                }

                return listaFavoritos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool VerificarFavorito(int idUsuario, int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();
            string consulta = "SELECT Id, IdUser, IdArticulo FROM FAVORITOS WHERE IdUser = @idUser AND IdArticulo = @idArticulo";

            try
            {
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@idUser", idUsuario);
                datos.SetearParametro("@idArticulo", idArticulo);
                datos.EjecutarLectura();

                // Si se encuentra el favorito, devuelve true, si no, retorna false
                return datos.Lector.Read();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Articulo> ObtenerListaArticulosFav(List<int> listaIds)
        {
            // Si la lista de IDs está vacía, retorna una lista vacía
            if (listaIds == null || listaIds.Count == 0)
                return new List<Articulo>();

            AccesoDatos datos = new AccesoDatos();
            List<Articulo> listaArticulos = new List<Articulo>();

            // Convierte la lista de ids en una cadena separada por comas
            string ids = string.Join(",", listaIds);

            // Consulta para obtener los artículos basados en los ids proporcionados
            string consulta = $@"SELECT A.Id AS idArticulo, A.Codigo, A.Nombre, A.Descripcion AS
                artDescripcion, A.ImagenUrl AS imagen, A.Precio, A.IdCategoria, A.IdMarca, C.Descripcion
                AS categoria, M.Descripcion AS marca
                FROM ARTICULOS A
                INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id
                INNER JOIN MARCAS M ON A.IdMarca = M.Id
                WHERE A.Id IN ({ids})";

            try
            {
                datos.SetearConsulta(consulta);

                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    Articulo art = new Articulo();

                    art.Id = (int)datos.Lector["idArticulo"];
                    art.CodigoArticulo = (string)datos.Lector["Codigo"];
                    art.Nombre = (string)datos.Lector["Nombre"];
                    art.Descripcion = (string)datos.Lector["artDescripcion"];
                    art.Precio = (decimal)datos.Lector["Precio"];
                    art.Imagen = (string)datos.Lector["imagen"];

                    art.Categoria = new Categoria();
                    art.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    art.Categoria.Descripcion = (string)datos.Lector["categoria"];

                    art.Marca = new Marca();
                    art.Marca.Id = (int)datos.Lector["IdMarca"];
                    art.Marca.Descripcion = (string)datos.Lector["marca"];

                    listaArticulos.Add(art);
                }

                return listaArticulos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Método para eliminar un favorito de la base de datos
        public void EliminarFavorito(Favorito favorito)
        {
            AccesoDatos datos = new AccesoDatos();
            string consulta = "DELETE FROM FAVORITOS WHERE IdUser = @idUser AND IdArticulo = @idArticulo";

            try
            {
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@idUser", favorito.IdUsuario);
                datos.SetearParametro("@idArticulo", favorito.IdArticulo);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarFavoritoPorArticulo(int idArticulo)
        {
            AccesoDatos datos = new AccesoDatos();
            string consulta = "DELETE FROM FAVORITOS WHERE IdArticulo = @idArt";

            try
            {
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@idArt", idArticulo);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}

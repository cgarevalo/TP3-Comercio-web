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
                datos.SetearParametro("@nombre", usuario.Nombre);
                datos.SetearParametro("@apellido", usuario.Apellido);
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
    }
}

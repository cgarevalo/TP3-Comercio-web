﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Dominio;

namespace Negocio
{
    public class ComercioNegocio
    {
        AccesoDatos datosAcceso = new AccesoDatos();

        // Método para obtener la lista de artículos desde la base de datos
        public List<Articulo> ListarArticulos(string idAr = "")
        {
            List<Articulo> listaArticulos = new List<Articulo>();

            try
            {
                string consulta = @"SELECT A.Id AS idArticulo, A.Codigo, A.Nombre, A.Descripcion AS
                artDescripcion, A.ImagenUrl AS imagen, A.Precio, A.IdCategoria, A.IdMarca, C.Descripcion
                AS categoria, M.Descripcion AS marca
                FROM ARTICULOS A
                INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id
                INNER JOIN MARCAS M ON A.IdMarca = M.Id ";

                if (idAr != "")
                {
                    consulta += "WHERE A.Id = " + idAr;
                }

                datosAcceso.SetearConsulta(consulta);
                datosAcceso.EjecutarConsulta();

                while (datosAcceso.Lector.Read())
                {
                    // Crea un nuevo objeto Articulo y lo agrega a la lista
                    Articulo art = new Articulo();

                    art.Id = (int)datosAcceso.Lector["idArticulo"];
                    art.CodigoArticulo = (string)datosAcceso.Lector["Codigo"];
                    art.Nombre = (string)datosAcceso.Lector["Nombre"];
                    art.Descripcion = (string)datosAcceso.Lector["artDescripcion"];
                    art.Precio = (decimal)datosAcceso.Lector["Precio"];

                    if (!(datosAcceso.Lector["imagen"] is DBNull))
                    {
                        art.Imagen = (string)datosAcceso.Lector["imagen"];
                    }

                    art.Categoria = new Categoria();
                    art.Categoria.Id = (int)datosAcceso.Lector["IdCategoria"];
                    art.Categoria.Descripcion = (string)datosAcceso.Lector["categoria"];

                    art.Marca = new Marca();
                    art.Marca.Id = (int)datosAcceso.Lector["IdMarca"];
                    art.Marca.Descripcion = (string)datosAcceso.Lector["marca"];

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
                datosAcceso.CerrarConexion();
            }
        }

        // Método para obtener la lista de marcas desde la base de datos
        public List<Marca> ListaMarcas()
        {
            List<Marca> listaMarcas = new List<Marca>();

            try
            {
                datosAcceso.SetearConsulta("Select Id, Descripcion From MARCAS");
                datosAcceso.EjecutarConsulta();

                while (datosAcceso.Lector.Read())
                {
                    Marca marca = new Marca();

                    marca.Id = (int)datosAcceso.Lector["Id"];
                    marca.Descripcion = (string)datosAcceso.Lector["Descripcion"];
                    listaMarcas.Add(marca);

                }

                return listaMarcas;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        // Método para obtener la lista de categorías desde la base de datos
        public List<Categoria> ListaCategorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();

            try
            {
                datosAcceso.SetearConsulta("Select Id, Descripcion From CATEGORIAS");
                datosAcceso.EjecutarConsulta();

                while (datosAcceso.Lector.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.Id = (int)datosAcceso.Lector["Id"];
                    categoria.Descripcion = (string)datosAcceso.Lector["Descripcion"];
                    listaCategorias.Add(categoria);

                }

                return listaCategorias;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        // Método para agregar un nuevo artículo a la base de datos
        public void AgregarArticulo(Articulo nuevoArticulo)
        {
            try
            {
                string consulta = @"INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca,
                IdCategoria, ImagenUrl, Precio)
                Values (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @imagenUrl, @precio)";

                datosAcceso.SetearConsulta(consulta);

                datosAcceso.SetearParametro("@codigo", nuevoArticulo.CodigoArticulo);
                datosAcceso.SetearParametro("@nombre", nuevoArticulo.Nombre);
                datosAcceso.SetearParametro("@descripcion", nuevoArticulo.Descripcion);
                datosAcceso.SetearParametro("@idCategoria", nuevoArticulo.Categoria.Id);
                datosAcceso.SetearParametro("@idMarca", nuevoArticulo.Marca.Id);
                datosAcceso.SetearParametro("@imagenUrl", nuevoArticulo.Imagen ?? (object)DBNull.Value);
                datosAcceso.SetearParametro("@precio", nuevoArticulo.Precio);

                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        // Método para modificar un artículo en la base de datos
        public void Modificar(Articulo modificarArt)
        {
            try
            {
                string consulta = @"Update ARTICULOS Set Codigo = @codigo, Nombre = @nombre,
                Descripcion = @desc, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @imag,
                Precio = @precio Where Id = @id";

                datosAcceso.SetearConsulta(consulta);

                datosAcceso.SetearParametro("@codigo", modificarArt.CodigoArticulo);
                datosAcceso.SetearParametro("@nombre", modificarArt.Nombre);
                datosAcceso.SetearParametro("@desc", modificarArt.Descripcion);
                datosAcceso.SetearParametro("@idMarca", modificarArt.Marca.Id);
                datosAcceso.SetearParametro("@idCategoria", modificarArt.Categoria.Id);
                datosAcceso.SetearParametro("@imag", modificarArt.Imagen ?? (object)DBNull.Value);
                datosAcceso.SetearParametro("@precio", modificarArt.Precio);
                datosAcceso.SetearParametro("id", modificarArt.Id);

                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        // Método para agregar una Categoría a la base de datos
        public void AgregarCategoria(Categoria nuevaCategoria)
        {
            try
            {
                datosAcceso.SetearConsulta("INSERT INTO CATEGORIAS (Descripcion) Values (@cateDesc)");
                datosAcceso.SetearParametro("@cateDesc", nuevaCategoria.Descripcion);
                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        // Método para agregar una Marca a la base de datos
        public void AgregarMarca(Marca nuevaMarca)
        {
            try
            {
                datosAcceso.SetearConsulta("INSERT INTO MARCAS (Descripcion) Values (@marcaDesc)");
                datosAcceso.SetearParametro("@MarcaDesc", nuevaMarca.Descripcion);
                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        // Método para eliminar un artículo de la base de datos
        public void Eliminar(int idArticulo)
        {
            try
            {
               UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                usuarioNegocio.EliminarFavoritoPorArticulo(idArticulo);

                datosAcceso.SetearConsulta("Delete From ARTICULOS Where Id = @idArt");
                datosAcceso.SetearParametro("@idArt", idArticulo);
                datosAcceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosAcceso.CerrarConexion();
            }
        }

        public List<Articulo> FiltrarArticulos(string campo, string criterio, string filtro)
        {
            string consulta = @"SELECT A.Id AS idArticulo, A.Codigo, A.Nombre, A.Descripcion AS
                artDescripcion, A.ImagenUrl AS imagen, A.Precio, A.IdCategoria, A.IdMarca, C.Descripcion
                AS categoria, M.Descripcion AS marca
                FROM ARTICULOS A
                INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id
                INNER JOIN MARCAS M ON A.IdMarca = M.Id
                WHERE ";

            List<Articulo> listafiltrada = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Arma la consulta según el campo seleccionado
                switch (campo)
                {
                    case "Categoría":
                        consulta += "C.Descripcion ";
                        break;

                    case "Marca":
                        consulta += "M.Descripcion ";
                        break;

                    case "Precio":
                        consulta += "A.Precio ";
                        break;
                }

                // Agrega la condición según el criterio seleccionado
                switch (criterio)
                {
                    case "Comienza con":
                        consulta += "LIKE @filtro + '%'";
                        break;

                    case "Contiene":
                        consulta += "LIKE '%' + @filtro + '%'";
                        break;

                    case "Termina con":
                        consulta += "LIKE '%' + @filtro";
                        break;

                    case "Menor a":
                        consulta += "< @filtro";
                        break;

                    case "Igual a":
                        consulta += "= @filtro";
                        break;

                    case "Mayor a":
                        consulta += "> @filtro";
                        break;
                }

                datos.SetearConsulta(consulta);

                // Convierte el filtro a decimal si el campo es Precio
                if (campo == "Precio" && Decimal.TryParse(filtro, out decimal filtroDecimal))
                    datos.SetearParametro("@filtro", filtroDecimal);
                else
                    datos.SetearParametro("@filtro", filtro);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo art = new Articulo();
                    art.Id = (int)datos.Lector["idArticulo"];
                    art.CodigoArticulo = (string)datos.Lector["Codigo"];
                    art.Nombre = (string)datos.Lector["Nombre"];
                    art.Descripcion = (string)datos.Lector["artDescripcion"];
                    if (!(datos.Lector["imagen"] is DBNull))
                        art.Imagen = (string)datos.Lector["imagen"];
                    art.Precio = (decimal)datos.Lector["Precio"];

                    art.Categoria = new Categoria();
                    art.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    art.Categoria.Descripcion = (string)datos.Lector["categoria"];

                    art.Marca = new Marca();
                    art.Marca.Id = (int)datos.Lector["IdMarca"];
                    art.Marca.Descripcion = (string)datos.Lector["marca"];

                    listafiltrada.Add(art);
                }

                return listafiltrada;
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            // Inicializa una nueva instancia de la clase AccesoDatos y configura la conexión a la base de datos
            conexion = new SqlConnection(ConfigurationManager.AppSettings["cadenaConexion"]);
            comando = new SqlCommand();
        }

        public void SetearConsulta(string consulta)
        {
            // Establece la consulta SQL a ejecutar en la base de datos
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void EjecutarConsulta()
        {
            // Ejecuta la consulta en la base de datos
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LimpiarParametros();
            }
        }

        public void EjecutarAccion()
        {
            // Ejecuta una acción (como una inserción, actualización o eliminación) en la base de datos
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LimpiarParametros();
            }
        }

        public void SetearParametro(string nombre, object valor)
        {
            // Agrega un parámetro a la consulta SQL, para hacer una consulta más fácil 
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void CerrarConexion()
        {
            // Cierra la conexión a la base de datos y el lector de datos
            if (lector != null)
            {
                lector.Close();
            }

            conexion.Close();
        }

        public void EjecutarLectura()
        {
            // Ejecuta una lectura de datos en la base de datos
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                LimpiarParametros();
            }
        }

        // Limpia los parámetros después de ejecutar una consulta
        private void LimpiarParametros()
        {
            comando.Parameters.Clear();
        }
    }
}

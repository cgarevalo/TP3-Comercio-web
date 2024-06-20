using Dominio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Seguridad
{
    public static class Utilidades
    {
        public static void ArmarUrlImagen(List<Articulo> articulos)
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

        public static string GuardarImagen(HttpPostedFile imagen, Articulo articulo, string rutaServidor)
        {
            // Obtiene la ruta de la carpeta
            string ruta = Path.Combine(rutaServidor + "/Images/Artículos/");

            // Verifica si el usuario ya tiene una imagen de perfil
            if (!String.IsNullOrEmpty(articulo.Imagen))
            {
                // Ruta completa de la imagen antigua
                string rutaImgAntigua = Path.Combine(ruta, articulo.Imagen);

                if (File.Exists(rutaImgAntigua))
                {
                    // Elimina la imagen antigua si existe
                    File.Delete(rutaImgAntigua);
                }
            }

            // Generar un nombre único para la imagen
            string nombreImagen = Guid.NewGuid().ToString() + ".jpg";

            // Combina la ruta de la imagen con el nombre
            string rutaCompleta = Path.Combine(ruta, nombreImagen);

            // Guarda la imagen
            imagen.SaveAs(rutaCompleta);

            return nombreImagen;
        }

        public static string GuardarImagen(HttpPostedFile imagen, Usuario usuario, string rutaServidor)
        {
            // Obtiene la ruta de la carpeta
            string ruta = Path.Combine(rutaServidor + "/Images/Perfiles/");

            // Verifica si el usuario ya tiene una imagen de perfil
            if (!String.IsNullOrEmpty(usuario.UrlImagenPerfil))
            {
                // Ruta completa de la imagen antigua
                string rutaImgAntigua = Path.Combine(ruta, usuario.UrlImagenPerfil);

                if (File.Exists(rutaImgAntigua))
                {
                    // Elimina la imagen antigua si existe
                    File.Delete(rutaImgAntigua);
                }
            }

            // Generar un nombre único para la imagen
            string nombreImagen = $"usuario-{usuario.Id.ToString()}-.jpg";

            // Combina la ruta de la imagen con el nombre
            string rutaCompleta = Path.Combine(ruta, nombreImagen);

            // Guarda la imagen
            imagen.SaveAs(rutaCompleta);

            return nombreImagen;
        }

        public static void EliminarImagenLocal(Articulo articulo, string rutaServidor)
        {
            // Obtiene la ruta de la carpeta
            string ruta = Path.Combine(rutaServidor + "/Images/Artículos/");
            string rutaImagen = Path.Combine(ruta + articulo.Imagen);

            if (File.Exists(rutaImagen))
            {
                // Elimina la imagen si existe
                File.Delete(rutaImagen);
            }
        }
    }
}

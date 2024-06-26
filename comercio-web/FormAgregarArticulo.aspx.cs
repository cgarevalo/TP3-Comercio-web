using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using System.Globalization;
using Seguridad;
using System.IO;
using System.Text.RegularExpressions;

namespace comercio_web
{
    public partial class FormAgregarArticulo : System.Web.UI.Page
    {
        public bool OrigenImagen { get; set; }
        public bool ConfirmarEliminacion { get; set; }

        ComercioNegocio negocio = new ComercioNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Seguridad.Autenticacion.EsAdministrador(Session["usuarioEnSesion"])))
            {
                Session.Add("error", "Se requieren permisos de administrador para estár en la página");
                Response.Redirect("Error.aspx", false);
                return;
            }

            // Actualizar OrigenImagen en cada carga de página, incluidos los postbacks
            OrigenImagen = chkOrigen.Checked;
            ConfirmarEliminacion = false;

            try
            {
                if (!IsPostBack)
                {
                    List<Categoria> categoriasLista = negocio.ListaCategorias();
                    categoriasLista.Add(new Categoria { Descripcion = "Nueva categoría" });
                    List<Marca> marcasLista = negocio.ListaMarcas();
                    marcasLista.Add(new Marca { Descripcion = "Nueva marca" });

                    Session.Add("listaCategorias", categoriasLista);
                    ddlCategoria.DataSource = Session["listaCategorias"];
                    ddlCategoria.DataValueField = "Id";
                    ddlCategoria.DataTextField = "Descripcion";
                    ddlCategoria.DataBind();

                    Session.Add("listaMarcas", marcasLista);
                    ddlMarca.DataSource = Session["listaMarcas"];
                    ddlMarca.DataValueField = "Id";
                    ddlMarca.DataTextField = "Descripcion";
                    ddlMarca.DataBind();

                    // Asigna una imagen de marcador de posición al control imgCargarPortada
                    imgImagenArt.Src = "https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg";
                }

                // Configuración específica si se está modificando un libro existente
                string id = Request.QueryString["id"] != null ? Request.QueryString["id"] : "";
                // El operador ternario se utiliza para verificar si el parámetro "id" está presente en la URL. Si Request.QueryString["id"] no es nulo, se asigna su valor a la variable "id", si no, "id" se establece como una cadena vacía ("").


                // Si id no está vacío y no es un postback (Se está modificando)
                if (id != "" && !IsPostBack)
                {
                    // Cambia el título de la página a "Modificar artículo", porque se está modificando
                    Title = "Modificar artículo";

                    // Obtiene el artículo seleccionado para su modificación
                    List<Articulo> articulo = negocio.ListarArticulos(id);
                    Articulo seleccionado = articulo[0];

                    // Guarda el artículo seleccionado en la sesión para uso posterior
                    Session.Add("artSeleccionado", seleccionado);

                    // Carga los campos del formulario con los datos del artículo seleccionado
                    txtCodigo.Text = seleccionado.CodigoArticulo;
                    txtNombre.Text = seleccionado.Nombre;
                    txtDescripcion.Text = seleccionado.Descripcion;

                    string precioStr = seleccionado.Precio.ToString("N2", new CultureInfo("es-ES"));
                    txtPrecio.Text = precioStr;
                    ddlCategoria.SelectedValue = seleccionado.Categoria.Id.ToString();
                    ddlMarca.SelectedValue = seleccionado.Marca.Id.ToString();

                    if (seleccionado.Imagen != null)
                    {
                        if (seleccionado.Imagen.StartsWith("http://") || seleccionado.Imagen.StartsWith("https://"))
                        {
                            txtImagenArt.Text = seleccionado.Imagen;
                            imgImagenArt.Src = seleccionado.Imagen;
                        }
                        else
                        {
                            // Marca el chkOrigen como seleccionado
                            chkOrigen.Checked = true;
                            // Sincroniza OrigenImagen con el estado de chkOrigen
                            OrigenImagen = chkOrigen.Checked;
                            imgImagenArt.Src = "~/Images/Artículos/" + seleccionado.Imagen;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx");
            }
        }

        protected void chkOrigen_CheckedChanged(object sender, EventArgs e)
        {
            // Sincroniza OrigenImagen con el estado de chkOrigen
            OrigenImagen = chkOrigen.Checked;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            // Valida la página antes de realizar la operación de agregar o modificar
            Page.Validate();

            // Si la página no es válida, se interrumpe el flujo y se sale
            if (!Page.IsValid)
                return;

            Articulo nuevoArticulo = new Articulo();

            // Si la página es válida, continúa con la lógica de agregar o modificar el libro
            try
            {
                string codigo = txtCodigo.Text;
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                string imagen = txtImagenArt.Text;
                string stringPrecio = txtPrecio.Text;
                decimal precio;
                bool textosCargados = Seguridad.Validacion.ValidarTexto(codigo) && Seguridad.Validacion.ValidarTexto(nombre) && Seguridad.Validacion.ValidarTexto(descripcion);

                if (textosCargados && Seguridad.Validacion.ValidarNumero(stringPrecio, out precio))
                {
                    if (!(nombre.Length <= 50 && codigo.Length <= 50 && descripcion.Length <= 150))
                    {
                        lblError.Text = "Código y nombre no pueden superar los 50 caracteres y descripción no puede superar los 150";
                        return;

                    }

                    nuevoArticulo.CodigoArticulo = codigo;
                    nuevoArticulo.Nombre = nombre;
                    nuevoArticulo.Descripcion = descripcion;
                    nuevoArticulo.Precio = precio;

                }
                else
                {
                    lblError.Text = "Por favor, complete todos los campos correctamente.";
                    return;
                }

                // Asigna las categorías y marcas seleccionadas
                nuevoArticulo.Categoria = new Categoria();
                nuevoArticulo.Categoria.Id = int.Parse(ddlCategoria.SelectedValue);

                nuevoArticulo.Marca = new Marca();
                nuevoArticulo.Marca.Id = int.Parse(ddlMarca.SelectedValue);


                // Switch para manejar la imagen según su origen
                switch (OrigenImagen)
                {
                    case true:
                        // Maneja la imagen si es de origen local
                        if (fuImagenArt.HasFile)
                        {
                            // Obtiene la extensión del archivo subido
                            string extensionArchivo = Path.GetExtension(fuImagenArt.FileName).ToLower();
                            // Lista de extensiones de imagen permitidas
                            string[] extensiones = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

                            // Verifica si la extensión del archivo subido está en la lista de extensiones permitidas
                            if (extensiones.Contains(extensionArchivo))
                            {
                                // Si se está modificando (hay un id en la query string)
                                if (Request.QueryString["id"] != null)
                                {
                                    // Envía el artículo en sesión y no el nuevo porque se está modificando y se tiene que eliminar la imagen anterior
                                    nuevoArticulo.Imagen = Seguridad.Utilidades.GuardarImagen(fuImagenArt.PostedFile, (Articulo)Session["artSeleccionado"], Server.MapPath("~"));
                                }
                                else
                                {
                                    // Envía el articulo nuevo porque no se está modificando
                                    nuevoArticulo.Imagen = Seguridad.Utilidades.GuardarImagen(fuImagenArt.PostedFile, nuevoArticulo, Server.MapPath("~"));
                                }

                                // Limpia cualquier mensaje de error previo
                                lblErrorLocal.Text = string.Empty;
                            }
                            else
                            {
                                // Si la extensión no es válida, muestra un mensaje de error y sale
                                lblErrorLocal.Text = "Solo imágenes png, jpg, jpeg, gif y webp";
                                return;
                            }
                        }
                        else
                        {
                            if (Request.QueryString["id"] != null)
                            {
                                // Conserva la imagen local si no se seleccionó otra al modificar
                                nuevoArticulo.Imagen = ((Articulo)Session["artSeleccionado"]).Imagen;
                            }
                            else
                            {
                                // Si no se seleccionó una imagen nueva y no se está modificando, muestra un mensaje de error y retorna
                                lblErrorLocal.Text = "Seleccione una imagen";
                                return;
                            }
                        }
                        
                        break;

                    case false:
                        // Maneja la imagen si es por url
                        if (!String.IsNullOrWhiteSpace(imagen))
                        {
                            // Verifica si la URL es válida, si no, sale
                            if (!ComprobarUrl())
                                return;

                            nuevoArticulo.Imagen = imagen;
                        }
                        else if (Request.QueryString["id"] != null)
                        {
                            lblErrorUrl.Text = string.Empty;
                            // Conserva la url existente si no se proporcionó una nueva al modificar
                            nuevoArticulo.Imagen = ((Articulo)Session["artSeleccionado"]).Imagen;
                        }
                        else
                        {
                            // Si no se proporcionó una url nueva y no se está modificando, muestra un mensaje de error y retorna
                            lblErrorUrl.Text = "Imagen por url requerida";
                            return;
                        }
                        break;
                }

                if (Request.QueryString["id"] != null)
                {
                    // Limpia posibles mensajes de errores previos
                    lblError.Text = string.Empty;

                    // Le asigna como id el id que se mando por url para que se pueda modificar
                    string id = Request.QueryString["id"];

                    nuevoArticulo.Id = int.Parse(id);

                    negocio.Modificar(nuevoArticulo);
                }
                else
                {

                    // Limpia posibles mensajes de errores previos
                    lblError.Text = string.Empty;

                    // Si no se está modificando, es una inserción nueva, llama al método para agregar un artículo
                    negocio.AgregarArticulo(nuevoArticulo);
                }

                Response.Redirect("ListaArticulos.aspx", false);
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx");
            }
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si se seleccionó "Nueva categoría"
            if (ddlCategoria.SelectedItem.Text == "Nueva categoría")
            {
                txtNuevaCategoria.Visible = true;
                btnAgregarCategoria.Visible = true;
            }
            // Si se deseleccionó "Nueva categoría"
            else
            {
                txtNuevaCategoria.Visible = false;
                btnAgregarCategoria.Visible = false;
            }
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            string descCategoria = txtNuevaCategoria.Text;

            try
            {
                if (!String.IsNullOrEmpty(descCategoria))
                {
                    List<Categoria> categorias = (List<Categoria>)Session["listaCategorias"];

                    if (!categorias.Exists(c => c.Descripcion.ToLower() == descCategoria.ToLower()))
                    {
                        // Agrega la nueva categoría a la base de datos
                        Categoria nuevaCategoria = new Categoria { Descripcion = descCategoria };
                        negocio.AgregarCategoria(nuevaCategoria);

                        // Obtiene la lista de categorías actualizada desde la base de datos
                        categorias = negocio.ListaCategorias();

                        // Agrega "Nueva categoría" al final de la lista de categorías
                        categorias.Add(new Categoria { Descripcion = "Nueva categoría" });

                        // Actualiza la lista de categorías en sesión
                        Session["listaCategoria"] = categorias;

                        // Actualiza el ddlCategoria con la lista que contiene la nueva categoría
                        ddlCategoria.DataSource = categorias;
                        ddlCategoria.DataValueField = "Id";
                        ddlCategoria.DataTextField = "Descripcion";
                        ddlCategoria.DataBind();

                        // Encuentra la nueva categoría por su descripción, y le asigna sus datos a "nuavaCategoria"
                        nuevaCategoria = categorias.Find(c => c.Descripcion.ToLower() == descCategoria.ToLower());

                        // Selecciona la nueva categoría en el ddlCategoria
                        ddlCategoria.SelectedValue = nuevaCategoria.Id.ToString();

                        // Esconde el botón y textBox
                        txtNuevaCategoria.Visible = false;
                        btnAgregarCategoria.Visible = false;
                    }
                    else
                    {
                        lblMensajeCate.Visible = true;
                        lblMensajeCate.Text = "La categoría ya existe";
                        return;
                    }

                    if (lblMensajeCate.Visible)
                        lblMensajeCate.Visible = false;

                    // Actualiza el UpdatePanel de udpCategoriaMarca para reflejar los cambios
                    udpCategoriaMarca.Update();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx");
            }
        }

        protected void ddlMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Si se seleccionó "Nueva marca"
            if (ddlMarca.SelectedItem.Text == "Nueva marca")
            {
                txtNuevaMarca.Visible = true;
                btnAgregarMarca.Visible = true;
            }
            // Si se deseleccionó "Nueva marca"
            else
            {
                txtNuevaMarca.Visible = false;
                btnAgregarMarca.Visible = false;
            }
        }

        protected void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            string descMarca = txtNuevaMarca.Text;

            try
            {
                if (!String.IsNullOrEmpty(descMarca))
                {
                    List<Marca> marcas = (List<Marca>)Session["listaMarcas"];

                    if (!marcas.Exists(m => m.Descripcion.ToLower() == descMarca.ToLower()))
                    {
                        // Agrega la nueva marca a la base de datos
                        Marca nuevaMarca = new Marca { Descripcion = descMarca };
                        negocio.AgregarMarca(nuevaMarca);

                        // Obtiene la lista de marcas actualizada desde la base de datos
                        marcas = negocio.ListaMarcas();

                        // Agrega "Nueva marca" al final de la lista de marcas
                        marcas.Add(new Marca { Descripcion = "Nueva marca" });

                        // Actualiza la lista de marcas en sesión
                        Session["listaMarcas"] = marcas;

                        // Actualiza el ddlMarca con la lista que contiene la nueva marca
                        ddlMarca.DataSource = marcas;
                        ddlMarca.DataValueField = "Id";
                        ddlMarca.DataTextField = "Descripcion";
                        ddlMarca.DataBind();

                        // Encuentra la nueva marca por su descripción, y le asigna sus datos a "nuavaMarca"
                        nuevaMarca = marcas.Find(m => m.Descripcion.ToLower() == descMarca.ToLower());

                        // Selecciona la nueva categoría en el ddlMarca
                        ddlMarca.SelectedValue = nuevaMarca.Id.ToString();

                        // Esconde el botón y textBox
                        txtNuevaMarca.Visible = false;
                        btnAgregarMarca.Visible = false;
                    }
                    else
                    {
                        lblMensajeMarc.Visible = true;
                        lblMensajeMarc.Text = "La marca ya existe";
                        return;
                    }

                    if (lblMensajeMarc.Visible)
                        lblMensajeMarc.Visible = false;

                    // Actualiza el UpdatePanel de udpCategoriaMarca para reflejar los cambios
                    udpCategoriaMarca.Update();
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.Message);
                Response.Redirect("Error.aspx");
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                ConfirmarEliminacion = true;
            }
        }

        protected void btnConfirmarEliminacion_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["artSeleccionado"] != null)
                {
                    if (chkEliminar.Checked)
                    {
                        UsuarioNegocio negocioUser = new UsuarioNegocio();
                        Articulo articulo = (Articulo)Session["artSeleccionado"];

                        // Primero elimina los favoritos asociados a este artículo
                        negocioUser.EliminarFavoritoPorArticulo(articulo.Id);

                        // Segundo elimina la imagen local, si es de origen local,
                        if (OrigenImagen)
                        {
                            Seguridad.Utilidades.EliminarImagenLocal(articulo, Server.MapPath("~"));
                        }

                        // Despues elimina el artículo
                        negocio.Eliminar(articulo.Id);

                        Response.Redirect("ListaArticulos.aspx", false);
                    }
                }
                else
                {
                    Session.Add("error", "Algo salió mal");
                    Response.Redirect("Error.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
                Response.Redirect("Error.aspx");
            }
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtImagenArt.Text))
                ComprobarUrl();
            else
                lblErrorUrl.Text = "Primero ingrese una url";
        }

        private bool ComprobarUrl()
        {
            string urlReg = @"^(https?:\/\/)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/[^\s]*)?$";
            string urlImagen = txtImagenArt.Text;

            // Verifica si la url es válida
            if (Regex.IsMatch(urlImagen, urlReg))
            {
                // Asigna la url al src de imgImagenArt
                imgImagenArt.Src = txtImagenArt.Text;

                // Limpiar el mensaje de error si la url es válida
                lblErrorUrl.Text = string.Empty;
                return true;
            }
            else
            {
                // Muestra un mensaje de error si la url no es válida
                lblErrorUrl.Text = "Ingrese una url válida";
                return false;
            }
        }

        protected void cvUrl_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value.Length >= 1000)
            {
                args.IsValid = true;
                lblAdvertencia.Text = "La URL tiene o supera los 1000 caracteres, verifique si funciona";
            }
            else
            {
                args.IsValid = true; // Siempre se valida en true para permitir que el formulario se envíe
                lblAdvertencia.Text = string.Empty;
            }
        }
    }
}
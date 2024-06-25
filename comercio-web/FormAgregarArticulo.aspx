<%@ Page Title="Agregar artículo" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="FormAgregarArticulo.aspx.cs" Inherits="comercio_web.FormAgregarArticulo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/clases.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="row">
        <div class="col-5">
            <div class="mb-3">
                <asp:Label ID="lblError" CssClass="text-danger" runat="server" Text=""></asp:Label>
            </div>
            <div class="mb-3">
                <label for="txtCodigo" class="form-label">Código</label>
                <asp:TextBox ID="txtCodigo" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ErrorMessage="Código requerido" CssClass="text-danger" ControlToValidate="txtCodigo" runat="server" />
            </div>
            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" CssClass="form-control" MaxLength="50" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ErrorMessage="Nombre requerido" CssClass="text-danger" ControlToValidate="txtNombre" runat="server" />
            </div>
            <div class="mb-3">
                <label>Descripción</label>
                <asp:TextBox ID="txtDescripcion" CssClass="form-control" TextMode="MultiLine" MaxLength="150" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ErrorMessage="Descripción requerida" CssClass="text-danger" ControlToValidate="txtDescripcion" runat="server" />
            </div>

            <asp:UpdatePanel ID="udpCategoriaMarca" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="mb-3">
                        <label for="ddlCategoria" class="form-label">Categoría</label>
                        <asp:DropDownList ID="ddlCategoria" AutoPostBack="true" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>

                        <div class="mb-3 d-flex mt-1">
                            <asp:TextBox ID="txtNuevaCategoria" CssClass="form-control me-3" MaxLength="50" Visible="false" runat="server"></asp:TextBox>
                            <asp:Button ID="btnAgregarCategoria" OnClick="btnAgregarCategoria_Click" CssClass="btn btn-success" runat="server" Text="Agregar" Visible="false" />
                        </div>
                        <asp:Label ID="lblMensajeCate" CssClass="form-label text-danger" Visible="false" runat="server" Text=""></asp:Label>

                    </div>
                    <div class="mb-3">
                        <label for="ddlMarca" class="form-label">Marca</label>
                        <asp:DropDownList ID="ddlMarca" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMarca_SelectedIndexChanged" runat="server"></asp:DropDownList>

                        <div class="mb-3 d-flex mt-1">
                            <asp:TextBox ID="txtNuevaMarca" CssClass="form-control me-3" MaxLength="50" Visible="false" runat="server"></asp:TextBox>
                            <asp:Button ID="btnAgregarMarca" CssClass="btn btn-success" Visible="false" runat="server" OnClick="btnAgregarMarca_Click" Text="Agregar" />
                        </div>
                        <asp:Label ID="lblMensajeMarc" CssClass="form-label text-danger" Visible="false" runat="server" Text=""></asp:Label>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="mb-3">
                <label for="txtPrecio" class="form-label">Precio</label>
                <asp:TextBox ID="txtPrecio" CssClass="form-control" runat="server"></asp:TextBox>
                <div class="modal-content">
                    <asp:RequiredFieldValidator ErrorMessage="Precio requerido" CssClass="text-danger" ControlToValidate="txtPrecio" runat="server" />
                    <asp:RegularExpressionValidator ErrorMessage="Ingrese un precio válido" CssClass="text-danger" ControlToValidate="txtPrecio" ValidationExpression="^\d{1,3}(\.\d{3})*(,\d{2})?$" runat="server" />
                </div>
            </div>

            <div class="d-flex">
                <asp:Button ID="btnAceptar" CssClass="btn btn-primary me-2" OnClick="btnAceptar_Click" runat="server" Text="Aceptar" />
                <a href="ListaArticulos.aspx" class="btn btn-secondary me-2">Cancelar</a>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="d-flex">
                            <asp:Button ID="btnEliminar" CssClass="btn btn-warning d-flex" OnClick="btnEliminar_Click" runat="server" Text="Eliminar" />
                            <%if (ConfirmarEliminacion)
                                { %>
                            <div class="d-flex align-items-center">
                                <asp:CheckBox ID="chkEliminar" CssClass="form-check d-flex align-items-center me-2" runat="server" Text="Confirmar" />
                                <asp:Button ID="btnConfirmarEliminacion" runat="server" CssClass="btn btn-danger" Text="Eliminar" OnClick="btnConfirmarEliminacion_Click" />
                            </div>
                            <%  } %>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="col-5">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="mb-3">
                        <label for="chkOrigen" class="form-label">Imagen local</label>
                        <asp:CheckBox ID="chkOrigen" OnCheckedChanged="chkOrigen_CheckedChanged" CssClass="form-check" AutoPostBack="true" runat="server" />
                    </div>

                    <%if (OrigenImagen)
                        { %>
                    <div class="mb-3">
                        <label for="fuImagenArt" class="form-label">Seleccione una imagen</label>
                        <asp:FileUpload ID="fuImagenArt" CssClass="form-control" accept=".jpg,.jpeg,.png,.gif" OnChange="preVisualizarImagen()" runat="server" />
                        <asp:Label ID="lblErrorLocal" CssClass="text-danger" runat="server" Text=""></asp:Label>
                    </div>
                    <%  }

                        else
                        { %>
                    <div class="mb-3">
                        <label for="txtImagenArt" class="form-label">Pegue la url de la imagen</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtImagenArt" CssClass="form-control" MaxLength="1000" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvUrl" ControlToValidate="txtImagenArt" OnServerValidate="cvUrl_ServerValidate" runat="server" />
                            <asp:Button ID="btnVer" CssClass="btn btn-success" runat="server" Text="Ver" OnClick="btnVer_Click" />
                        </div>
                        <asp:Label ID="lblErrorUrl" CssClass="text-danger" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblAdvertencia" runat="server"></asp:Label>
                    </div>
                    <%  } %>

                    <div class="mb-3">
                        <img id="imgImagenArt" src="#" class="img-fluid mb-3 " height="450" width="350" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <%--Javascript--%>
    <script>
        // Función para previsualizar la imagen seleccionada en el control FileUpload antes de cargarla en el servidor
        function preVisualizarImagen() {
            // Obtiene el control fileUpload y el control Image por sus Id
            const fileUpload = document.getElementById("<%= fuImagenArt.ClientID %>");
            const imgPerfil = document.getElementById("<%= imgImagenArt.ClientID %>");

            // Verifica si se ha seleccionado un archivo
            if (fileUpload.files && fileUpload.files[0]) {

                // Crea un nuevo fileReader para leer el contenido del archivo
                let reader = new FileReader();

                // Define la función que se ejecuta cuando el archivo se ha leído completamente
                reader.onload = function (e) {

                    // Asigna el contenido leído (URL de la imagen) al control Image para previsualización
                    imgPerfil.src = e.target.result;
                };

                // Lee el contenido del archivo como una URL de datos
                reader.readAsDataURL(fileUpload.files[0]);
            }
        }
    </script>
    <%--Jabascript--%>
</asp:Content>

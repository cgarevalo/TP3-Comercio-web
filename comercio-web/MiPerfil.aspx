<%@ Page Title="Mi perfil" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="comercio_web.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-4">
            <div class="mb-3">
                <label for="txtEmail" class="form-label">Correo electrónico</label>
                <asp:TextBox ID="txtEmail" CssClass="form-control mb-3" Enabled="false" runat="server"></asp:TextBox>
                <asp:Button ID="btnCambiarCorreo" CssClass="btn btn-primary" runat="server" Text="Cambiar correo electrónico" Enabled="false" />
            </div>

            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label for="txtApellido" class="form-label">Apellido</label>
                <asp:TextBox ID="txtApellido" CssClass="form-control" runat="server"></asp:TextBox>
            </div>

            <div class="mb-3">
                <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="btnGuardar_Click" />
                <a href="Default.aspx" class="btn btn-secondary">Cancelar</a>
                <div>
                    <asp:Label ID="lblError" CssClass="form-label text-danger" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>

        <div class="col-4">
            <div class="mb-3">
                <label for="fudImagenPerfil" class="form-label">Imagen de perfíl</label>
                <asp:FileUpload ID="fudImagenPerfil" CssClass="form-control" runat="server" accept=".jpg,.jpeg,.png,.gif" OnChange="previewImage()" />
                <asp:Label ID="lblFudError" runat="server" CssClass="text-danger" Text=""></asp:Label>
            </div>

            <img id="imgPerfil" class="img-fluid m-3" runat="server" src="https://static.vecteezy.com/system/resources/previews/016/916/479/original/placeholder-icon-design-free-vector.jpg" alt="" />
        </div>

    </div>

    <script>
        // Función para previsualizar la imagen seleccionada en el control FileUpload antes de cargarla en el servidor
        function previewImage() {

            // Obtiene el control FileUpload y el control Image por sus IDs
            var fileUpload = document.getElementById("<%= fudImagenPerfil.ClientID %>");
            var imgPerfil = document.getElementById("<%= imgPerfil.ClientID %>");

            // Verifica si se ha seleccionado un archivo
            if (fileUpload.files && fileUpload.files[0]) {

                // Crea un nuevo FileReader para leer el contenido del archivo
                var reader = new FileReader();

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
</asp:Content>

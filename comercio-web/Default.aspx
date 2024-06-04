<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="comercio_web.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/clases.css" rel="stylesheet" />
    <script src="JavaScript/funciones.js"></script>
    <%--<style>
        .favorito-container {
            text-align: right;
        }

        .favorito {
            cursor: pointer;
            width: 24px;
            height: 24px;
        }

        .favorito.active {
            content: url("Images/Iconos/bookmark-check.svg");
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-4 m-auto mb-3">
            <asp:TextBox ID="txtBuscaProducto" CssClass="form-control" OnTextChanged="txtBuscaProducto_TextChanged" placeholder="Buscar un producto..." runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row">
        <div class="col-3">
            <div class="mb-3">
                <div class="d-flex moverDerecha">
                    <label for="ddlCampo" class="form-label">Campo</label>
                </div>
                <div class="input-group">
                    <asp:Button ID="btnLimpiarFiltro" CssClass="btn btn-primary" runat="server" Text="Limpiar" />
                    <asp:DropDownList ID="ddlCampo" CssClass="form-control" runat="server"
                        OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Seleccione un campo" />
                        <asp:ListItem Text="Marca" />
                        <asp:ListItem Text="Categoría" />
                        <asp:ListItem Text="Precio" />
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>

    <div class="row row-cols-1 row-cols-md-3 g-4">
        <asp:Repeater ID="repArticulos" runat="server">
            <ItemTemplate>
                <div class="col">
                    <div class="card">
                        <img src="<%#Eval("Imagen") %>" class="card-img-top" height="500px" alt="Artículo" />
                        <div class="card-body">
                            <h4><%#Eval("Nombre") %></h4>
                            <p><%#Eval("Descripcion") %></p>
                            <p><strong>Precio:</strong> <%#Eval("Precio", "{0:N2}") %></p>
                            <div class="favorito-container">
                                <%--<asp:Image ID="imgFvorito" ImageUrl="" runat="server" />--%>
                                <img src="Images/Iconos/bookmark.svg" class="favorito" onclick="marcarFavorito(this)" alt="Alternate Text" />
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>

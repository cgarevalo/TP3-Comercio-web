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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- TextBox de búsqueda centrado arriba -->
            <div class="row">
                <div class="col-4 m-auto mb-5">
                    <asp:TextBox ID="txtBuscarProducto" AutoPostBack="true" CssClass="form-control" OnTextChanged="txtBuscarProducto_TextChanged" placeholder="Buscar un producto..." runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <!-- Filtro a la izquierda -->
                <div class="col-3 bg-body-secondary">
                    <label for="chkFiltro" class="form-label">Filtro</label>
                    <asp:CheckBox ID="chkFiltro" CssClass="form-control mb-3" AutoPostBack="true" OnCheckedChanged="chkFiltro_CheckedChanged" runat="server" />

                    <div class="mb-3">
                        <label for="ddlCampo" class="form-label">Campo</label>
                        <asp:DropDownList ID="ddlCampo" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged" Enabled="false" AutoPostBack="true">
                            <asp:ListItem Text="Seleccione un campo" />
                            <asp:ListItem Text="Marca" />
                            <asp:ListItem Text="Categoría" />
                            <asp:ListItem Text="Precio" />
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label for="ddlCriterio" class="form-label">Criterio</label>
                        <asp:DropDownList ID="ddlCriterio" CssClass="form-control" Enabled="false" OnSelectedIndexChanged="ddlCriterio_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label for="txtFiltro" class="form-label">Filtro</label>
                        <asp:TextBox ID="txtFiltro" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                        <div>
                            <asp:Label ID="lblError" CssClass="form-label text-danger" runat="server" Text=""></asp:Label>
                        </div>

                    </div>
                    <asp:Button ID="btnBuscar" Text="Buscar" Enabled="false" OnClick="btnBuscar_Click" CssClass="btn btn-primary me-3" runat="server" />
                    <asp:Button ID="btnLimpiarFiltro" CssClass="btn btn-primary" OnClick="btnLimpiarFiltro_Click" runat="server" Enabled="false" Text="Limpiar filtros" />
                </div>

                <!-- Repeater -->
                <div class="col-9">
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
                                                <asp:ImageButton ID="btnIconoFav" runat="server" CommandArgument='<%#Eval("Id") %>' CommandName="articuloId" ImageUrl='<%# ImagenBotonFav(Eval("Id")) %>' AlternateText="Agregar a Favoritos"  OnClick="btnIconoFav_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

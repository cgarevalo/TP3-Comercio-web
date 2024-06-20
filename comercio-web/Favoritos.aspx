<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="comercio_web.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/clases.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row row-cols-1 row-cols-md-3 g-4">
        <%if (ArticulosFavoritos.Count > 0)
            { %>
        <asp:Repeater ID="repArticulosFav" runat="server">
            <ItemTemplate>
                <div class="col">
                    <div class="card">
                        <img src="<%#Eval("Imagen") %>" class="imgTamanio" alt="Artículo" />
                        <div class="card-body">
                            <h4 class="card-title text-truncate"><%#Eval("Nombre") %></h4>
                            <p class="card-text text-truncate"><%#Eval("Descripcion") %></p>
                            <p class="card-text"><strong>Precio:</strong> <%#Eval("Precio", "{0:N2}") %></p>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <%  }
            else
            {%>
        <h1>No hay favoritos</h1>
          <%}%>
    </div>
</asp:Content>

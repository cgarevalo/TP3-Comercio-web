<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="ListaArticulos.aspx.cs" Inherits="comercio_web.ListaArticulos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>Artículos</h3>
    <asp:GridView ID="dgvArticulos" CssClass="table table-dark table-bordered" runat="server"
     AutoGenerateColumns="false" DataKeyNames="ID" >
        <Columns>
            <asp:BoundField HeaderText="Código" DataField="CodigoArticulo" />
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
            <asp:BoundField HeaderText="Categoría" DataField="Categoria" />
            <asp:BoundField HeaderText="Marca" DataField="Marca" />
            <asp:BoundField HeaderText="Precio" DataField="Precio" />
        </Columns>
    </asp:GridView>
</asp:Content>

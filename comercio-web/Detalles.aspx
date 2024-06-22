<%@ Page Title="Detalles" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Detalles.aspx.cs" Inherits="comercio_web.Detalles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/clases.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-3 d-flex justify-content-start align-items-center">
            <asp:Image ID="imgArticulo" CssClass="img-fluid" runat="server" />
        </div>
        <div class="col-8 offset-1">
            <div class="row">
                <div class="col-6">
                    <div class="mb-3">
                        <label for="txtCodigo" class="form-label"></label>
                        <asp:TextBox ID="txtCodigo" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtNombre" class="form-label"></label>
                        <asp:TextBox ID="txtNombre" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>

                    <div>
                        <label for="txtCategoria" class="form-label"></label>
                        <asp:TextBox ID="txtCategoria" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtMarca" class="form-label"></label>
                        <asp:TextBox ID="txtMarca" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtPrecio" class="form-label"></label>
                        <asp:TextBox ID="txtPrecio" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-6">
                    <div class="mb-3">
                        <div class="mb-3">
                            <label for="txtDescripcion" class="form-label"></label>
                            <asp:TextBox ID="txtDescripcion" CssClass="form-control limiteTextBox" TextMode="MultiLine" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

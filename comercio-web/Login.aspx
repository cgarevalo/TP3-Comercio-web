<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="comercio_web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-4">
                    <div class="mb-3">
                        <h3>Inicio de sesión</h3>
                        <label for="txtEmail" class="form-label">Correo electrónico</label>
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Ingrese un correo" CssClass="text-danger" ControlToValidate="txtEmail" runat="server" />
                        <asp:RegularExpressionValidator ErrorMessage="Ingrese un correo válido" CssClass="text-danger" ValidationExpression="^[^@]+@[^@]+\.[a-zA-Z]{2,}$" ControlToValidate="txtEmail" runat="server" />
                    </div>

                    <div class="mb-3">
                        <label for="txtContraseña" class="form-label">Contraseña</label>
                        <asp:TextBox ID="txtContraseña" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Ingrese una contraseña" CssClass="text-danger" ControlToValidate="txtContraseña" runat="server" />
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="lblError" CssClass="form-label text-danger" runat="server" Text=""></asp:Label>
                    </div>

                    <asp:Button ID="btnIngresar" CssClass="btn btn-primary" runat="server" Text="Ingresar" OnClick="btnIngresar_Click" />
                    <a href="Default.aspx" class="btn btn-secondary">Cancelar</a>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

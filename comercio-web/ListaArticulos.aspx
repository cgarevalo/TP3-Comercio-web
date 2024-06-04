<%@ Page Title="" Language="C#" MasterPageFile="~/MiMaster.Master" AutoEventWireup="true" CodeBehind="ListaArticulos.aspx.cs" Inherits="comercio_web.ListaArticulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/clases.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-5">
                    <div class="mb-3">
                        <label for="txtFiltroNombreCodigo" class="form-label">Buscar por nombre o código</label>
                        <asp:TextBox ID="txtFiltroNombreCodigo" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtFiltroNombreCodigo_TextChanged" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="col-5">
                <div class="mb-3">
                    <label for="chkFiltroAvanzado" class="form-label">Filtro avanzado</label>
                    <asp:CheckBox ID="chkFiltroAvanzado" AutoPostBack="true" OnCheckedChanged="chkFiltroAvanzado_CheckedChanged" runat="server" />
                </div>
            </div>

            <%if (FiltroAvanzado)
                { %>
            <div class="row">
                <div class="col-3">
                    <div class="d-flex moverDerecha">
                        <label for="ddlCampo" class="form-label">Campo</label>
                    </div>
                    <div class="input-group">
                        <asp:Button ID="btnLimpiarFiltros" runat="server" OnClick="btnLimpiarFiltros_Click" CssClass="btn btn-primary" Text="Limpiar" />
                        <asp:DropDownList ID="ddlCampo" CssClass="form-control" runat="server"
                            OnSelectedIndexChanged="ddlCampo_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Seleccione un campo" />
                            <asp:ListItem Text="Marca" />
                            <asp:ListItem Text="Categoría" />
                            <asp:ListItem Text="Precio" />
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-3">
                    <div class="mb-3">
                        <label for="ddlCriterio" class="form-label">Criterio</label>
                        <asp:DropDownList ID="ddlCriterio" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>

                <div class="col-3">
                    <div class="mb-3">
                        <label for="txtFiltroAvanzado" class="form-label">Filtro avanzado</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtFiltroAvanzado" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:Button ID="btnBuscar" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" runat="server" />
                            <asp:Label ID="lblError" runat="server" CssClass="form-label text-danger" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <%  } %>

            <h3>Artículos</h3>
            <asp:GridView ID="dgvArticulos" runat="server" CssClass="table table-dark table-bordered" AutoGenerateColumns="false" DataKeyNames="ID" AllowPaging="true" PageSize="10" OnPageIndexChanging="dgvArticulos_PageIndexChanging" OnSelectedIndexChanged="dgvArticulos_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField HeaderText="Código" DataField="CodigoArticulo" />
                    <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                    <asp:BoundField HeaderText="Categoría" DataField="Categoria" />
                    <asp:BoundField HeaderText="Marca" DataField="Marca" />
                    <asp:BoundField HeaderText="Precio" DataFormatString="{0:N2}" DataField="Precio" />
                    <asp:CommandField HeaderText="Seleccionar" ShowSelectButton="true" SelectText="✍" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>

    <a href="FormAgregarArticulo.aspx" class="btn btn-primary">Agregar</a>
</asp:Content>

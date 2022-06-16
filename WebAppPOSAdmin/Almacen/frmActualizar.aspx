<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmActualizar.aspx.cs" Inherits="WebAppPOSAdmin.Almacen.frmActualizar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Actualizaci&oacute;n y cierre de inventarios</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-7 col-lg-offset-1">
                <asp:Label runat="server" Text="Proveedor"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlProveedor" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-lg-3">
                <div>&nbsp;</div>
                <asp:Button Text="Actualizar y cerrar inventario" CssClass="form-control btn btn-primary" runat="server" ID="btnActualizar" OnClick="btnActualizar_Click" OnClientClick="return confirm('Está seguro de actualizar el inventario seleccionado?')" />
            </div>
        </div>
        <div class="panel-footer">&nbsp;</div>
    </div>
    <br />
</asp:Content>

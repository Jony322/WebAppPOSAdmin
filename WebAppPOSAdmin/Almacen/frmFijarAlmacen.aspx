<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmFijarAlmacen.aspx.cs" Inherits="WebAppPOSAdmin.Almacen.frmFijarAlmacen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Fijar Inventario</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-md-8 col-md-offset-2">
                <div class="form-group">
                    <asp:Label Text="Proveedor:" runat="server" />
                    <asp:DropDownList runat="server" ID="ddlProveedor" CssClass="form-control" />
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <asp:Button Text="Fijar inventario" runat="server" CssClass="btn btn-success" id="btnFijar" OnClick="btnFijar_Click" OnClientClick="return confirm('Desea fijar el inventario para el Proveedor seleccionado?');"/>
        </div>
    </div>
    <br />
</asp:Content>

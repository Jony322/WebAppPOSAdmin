<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CatImpresoras.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.CatImpresoras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-print"></span>&nbsp&nbsp Configuración de Impresora para etiquetas</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-8 col-lg-offset-2">
                <asp:Label Text="Impresoras del sistema:" runat="server" />
                <asp:DropDownList ID="availablePrinters" runat="server">
                    <asp:ListItem> -- Elija una impresora -- </asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rAvailablePrinters" runat="server" ErrorMessage="*" ControlToValidate="availablePrinters" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
            
        </div>
        <div class="panel-footer">
            <asp:Button Text="Guardar ruta" CssClass="active btn btn-default" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" />
        </div>
    </div>
    <br />
    </asp:Content>

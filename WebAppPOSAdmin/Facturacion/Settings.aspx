<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="WebAppPOSAdmin.Facturacion.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-wrench"></span>&nbsp;Configuración para Facturación</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <contenttemplate>
                    <div class="form-group col-lg-4 col-lg-offset-2">
                        <asp:Label Text="Usuario CFDI:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                            <asp:TextBox runat="server" ID="txtCfdiUser" CssClass="form-control text-center" />
                        </div>
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Label Text="Contraseña CFDI:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-asterisk"></span></span>
                            <asp:TextBox runat="server" ID="txtCfdiPassword" CssClass="form-control text-center" />
                        </div>
                    </div>
                    <div class="form-group col-lg-4 col-lg-offset-2">
                        <asp:Label Text="Directorio para archivos TXT:" runat="server" />
                        <asp:TextBox runat="server" ID="txtPathTXT" placeholder="Ej. C:\Facturacion\TXT\" CssClass="form-control text-center" />
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Label Text="Directorio para archivos PDF:" runat="server" />
                        <asp:TextBox runat="server" ID="txtPathPDF" placeholder="Ej. C:\Facturacion\PDF\" CssClass="form-control text-center" />
                    </div>
                </contenttemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel-footer">
            <asp:UpdatePanel runat="server">
                <contenttemplate>
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar ajustes" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                    <div>&nbsp;</div>
                </contenttemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

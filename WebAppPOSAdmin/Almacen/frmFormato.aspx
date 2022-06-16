<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmFormato.aspx.cs" Inherits="WebAppPOSAdmin.Almacen.frmFormato" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Formato de inventarios</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="form-group col-lg-8 col-lg-offset-1">
                        <asp:Label Text="Proveedor" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlProveedor" />
                    </div>
                    <div class="form-group col-lg-2">
                        <div>&nbsp;</div>
                        <asp:Button Text="Ver Formato" CssClass="form-control btn btn-primary" ID="btnVer" runat="server" OnClick="btnVer_Click" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row"></div>
            <div class="col-lg-10 col-lg-offset-1">&nbsp;</div>
        </div>
        <div class="panel-footer">&nbsp;</div>
    </div>
    <br />
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Basculas.aspx.cs" Inherits="WebAppPOSAdmin.Utilities.Basculas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-export"></span>&nbsp&nbsp Generador de archivos para Básculas</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-4">&nbsp;</div>
            <div class="col-lg-2">
                <asp:Button CssClass="active btn btn-default" ID="btnExportToDAT" runat="server" Text="Generar DAT" OnClick="btnExportToDAT_Click" />
            </div>
            <div class="col-lg-2">
                <asp:Button CssClass="active btn btn-default" ID="btnExportToXLS" runat="server" Text="Generar XLS" OnClick="btnExportToXLS_Click"  />
            </div>
            <div class="col-lg-4">&nbsp;</div>
        </div>
        <div class="panel-footer">&nbsp;</div>
    </div>
</asp:Content>

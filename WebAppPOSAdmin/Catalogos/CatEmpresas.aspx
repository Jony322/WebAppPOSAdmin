<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CatEmpresas.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.CatEmpresas" %>

<%@ Register Src="~/Controles/notificaciones.ascx" TagPrefix="uc1" TagName="notificaciones" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:notificaciones runat="server" ID="notificaciones" />
    <div class="panel panel-success quitar-margin ">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list"></span>&nbsp&nbsp Administrador de Empresas</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="form-group col-lg-6">
                <asp:Label Text="RFC:" runat="server" />
                <asp:TextBox runat="server" ID="txtrfc" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-6">
                <asp:Label Text="Razón social:" runat="server" />
                <asp:TextBox runat="server" ID="txtRazonSocial" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-6">
                <asp:Label Text="Representante:" runat="server" />
                <asp:TextBox runat="server" ID="txtRepresentante" CssClass="form-control" />
            </div>
            <div class="row"></div>
            <div class="form-group col-lg-4">
                <asp:Label Text="Calle:" runat="server" />
                <asp:TextBox runat="server" ID="txtCalle" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="Numero Exterior:" runat="server" />
                <asp:TextBox runat="server" ID="txtNuExterior" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="Numero Interior:" runat="server" />
                <asp:TextBox runat="server" ID="txtNuInteriro" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="Colonia:" runat="server" />
                <asp:TextBox runat="server" ID="txtColonia" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="Municipio:" runat="server" />
                <asp:TextBox runat="server" ID="txtMunicipio" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="Estado:" runat="server" />
                <asp:TextBox runat="server" ID="txtEstado" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="País:" runat="server" />
                <asp:TextBox runat="server" ID="txtPais" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="Código postal:" runat="server" />
                <asp:TextBox runat="server" ID="txtCodPostal" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="Télefono:" runat="server" />
                <asp:TextBox runat="server" ID="txtTelefono" CssClass="form-control" />
            </div>
            <div class="form-group col-lg-4">
                <asp:Label Text="E-mail:" runat="server" />
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" />
            </div>
            <asp:FileUpload ID="FileUpload" runat="server" />
            
        </div>
        <div class="panel-footer">
            <asp:Button Text="Guardar" CssClass="btn btn-success" runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" />
        </div>
    </div>
    <br />
</asp:Content>

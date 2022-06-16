<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-wrench"></span>&nbsp;Configuraciones</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="form-group col-lg-2 col-lg-offset-2">
                        <asp:Label Text="Código normal:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-barcode"></span></span>
                            <asp:TextBox runat="server" ID="txtCodNormal" CssClass="form-control text-center" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Código pesable:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-barcode"></span></span>
                            <asp:TextBox runat="server" ID="txtCodPesable" CssClass="form-control text-center" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Código no pesable:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-barcode"></span></span>
                            <asp:TextBox runat="server" ID="txtCodNoPesable" CssClass="form-control text-center" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="I.V.A.:" runat="server" />
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtIVA" />
                            <span class="input-group-addon">%</span>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel-footer">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar ajustes" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                    <div>&nbsp;</div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

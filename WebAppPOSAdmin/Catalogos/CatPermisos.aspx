<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CatPermisos.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.CatPermisos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-success quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-pencil"></span>&nbsp&nbsp Administrador de Permisos</h4>
            </div>
        </div>
        <div class="panel-body">
            
                <h3>Permisos</h3>
                <div>
                    <div class="form-group">
                        <strong>
                            <asp:Label Text="Descripción del permiso:" runat="server" />
                        </strong>
                        <asp:TextBox runat="server" ID="txtIdpermiso" CssClass="form-control"  Placeholder="Id permiso" />
                    </div>
                    <div class="form-group">
                        <strong>
                            <asp:Label Text="Descripción del permiso:" runat="server" />
                        </strong>
                        <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control" TextMode="MultiLine" Placeholder="Aqui la descripción" />
                    </div>
                </div>
                <div class="form-group">
                    <strong>
                        <asp:Label Text="Selecciona el sistema:" runat="server" />
                    </strong>
                    <asp:RadioButtonList runat="server" ID="rbtPermisos">
                        <asp:ListItem Text="Administrador" Value="pos_admin" />
                        <asp:ListItem Text="Cajas" Value="pos_caja" />
                        <asp:ListItem Text="Colectora" Value="pos_colector" />
                    </asp:RadioButtonList>
                </div>
            <div class="form-group">
                <asp:Button Text="Guardar" runat="server" CssClass="btn  btn-success" id="btnGuardar" OnClick="btnGuardar_Click" />
            </div>
            </div>
    </div>
</asp:Content>

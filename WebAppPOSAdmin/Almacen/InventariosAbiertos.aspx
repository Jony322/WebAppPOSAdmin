<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InventariosAbiertos.aspx.cs" Inherits="WebAppPOSAdmin.Almacen.InventariosAbiertos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Inventarios Abiertos</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvInventarios" runat="server" AllowPaging="True" AutoGenerateColumns="False" CssClass="table table-striped">
                        <Columns>
                            <asp:BoundField DataField="fecha_ini" HeaderText="Fecha inicio" ReadOnly="True" SortExpression="fecha_ini" ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="razon_social" HeaderText="Proveedor" ReadOnly="True" SortExpression="razon_social" />
                            <asp:BoundField DataField="user_name" HeaderText="Responsable" ReadOnly="True" SortExpression="user_name" ItemStyle-HorizontalAlign="center" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel-footer">&nbsp;</div>
    </div>
</asp:Content>


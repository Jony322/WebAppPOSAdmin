<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCapturaInventario.aspx.cs" Inherits="WebAppPOSAdmin.Almacen.frmCapturaInventario" %>

<%@ Register Src="~/Controles/notificaciones.ascx" TagPrefix="uc1" TagName="notificaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:notificaciones runat="server" ID="notificaciones" />
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Captura de inventarios</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="form-group col-lg-6 col-lg-offset-2">
                <asp:Label Text="Proveedor" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlProveedor" OnSelectedIndexChanged="ddlProveedor_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="--SELECCIONAR--" />
                </asp:DropDownList>
            </div>
            <div class="form-group col-lg-2">
                <div>&nbsp;</div>
                <asp:Button Text="Crear PDF" CssClass="form-control active btn btn-danger" ID="btnCreatePDF" ValidationGroup="boton" runat="server" OnClick="btnCreatePDF_Click" Enabled="False" />
            </div>
            <div class="row"></div>
            <div class="col-lg-12">
                <asp:GridView ID="gvCaptura" runat="server" DataKeyNames="cod_barras" AutoGenerateColumns="False" OnRowDataBound="gvCaptura_RowDataBound" CssClass="table table-striped">
                    <Columns>
                        <asp:BoundField DataField="cod_barras" HeaderText="Cod. Barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="descripcion_larga" HeaderStyle-Width="280px" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion_larga" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="descripcion" HeaderText="Unidad" ReadOnly="True" SortExpression="descripcion" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="stock_estimado" HeaderText="Ex. Estimada" ReadOnly="True" SortExpression="stock_estimado" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                        <asp:BoundField DataField="stock_fisico" HeaderText="Ex. Física" ReadOnly="True" SortExpression="stock_fisico" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="panel-footer">&nbsp;</div>
    </div>
    <br />
</asp:Content>

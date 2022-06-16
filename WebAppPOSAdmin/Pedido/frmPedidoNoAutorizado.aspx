<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPedidoNoAutorizado.aspx.cs" Inherits="WebAppPOSAdmin.Pedido.frmPedidoNoAutorizado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Pedidos NO Autorizados</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-6 col-lg-offset-2">
                <asp:Label Text="Proveedor" runat="server" />
                <asp:DropDownList ID="ddlProveedor" CssClass="form-control" runat="server" />
            </div>
            <div class="form-group col-lg-2">
                <div>&nbsp;</div>
                <asp:Button Text="Ver" CssClass="form-control btn btn-primary " ID="btnVer" runat="server" OnClick="btnVer_Click" />
            </div>
            <!-- RESULTADOS DE BÚSQUEDA -->
            <div class="col-lg-9 col-lg-offset-1">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvOrders_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="num_pedido" HeaderText="# Pedido" ReadOnly="True" SortExpression="num_pedido" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField DataField="razon_social" HeaderText="Proveedor" ReadOnly="True" SortExpression="razon_social" />
                                <asp:BoundField DataField="fecha_pedido" HeaderText="Fecha pedido" ReadOnly="True" SortExpression="fecha_pedido" ItemStyle-HorizontalAlign="center" />
                                <asp:BoundField DataField="fecha_autorizado" HeaderText="Fecha autorización" ReadOnly="True" SortExpression="fecha_autorizado" ItemStyle-HorizontalAlign="center" />
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ImageUrl="~/Images/delete.png" runat="server" ID="ibtDelete" CommandName="delete" CommandArgument='<%#Bind("id_pedido") %>' ToolTip="Eliminar pedido" data-toggle="modal" data-target="#myModal" OnClientClick="return confirm('Esta seguro de eliminar éste pedido?')" />&nbsp;
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvOrders" EventName="RowUpdating" />
                        <asp:AsyncPostBackTrigger ControlID="gvOrders" EventName="RowDeleting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel-footer">&nbsp;</div>
        <br />
    </div>
</asp:Content>

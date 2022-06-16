<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPedidoAutorizado.aspx.cs" Inherits="WebAppPOSAdmin.Pedido.frmPedidoAutorizado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Pedidos Autorizados</h4>
            </div>
        </div>
        <div class="panel-body" style="min-height: 300px">
            <div class="col-lg-2">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Label Text="Fecha inicial:" runat="server" />
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFechaIni" placeholder="dd/mm/aaaa" />
                            <span class="input-group-addon">
                                <asp:Image ID="imgCalendarIni" ImageUrl="~/Images/calendar.png" runat="server" />
                            </span>
                        </div>
                        <cc1:CalendarExtender ID="calFecha_ini" runat="server" PopupButtonID="imgCalendarIni" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" />
                        <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" ControlToValidate="txtFechaIni" ErrorMessage="Fecha Requerida" ForeColor="Red" ValidationGroup="enventoVer"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-lg-2">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Label Text="Hasta...:" runat="server" />
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFechaFin" placeholder="dd/mm/aaaa" />
                            <span class="input-group-addon">
                                <asp:Image ID="imgCalendarFin" ImageUrl="~/Images/calendar.png" runat="server" />
                            </span>
                        </div>
                        <cc1:CalendarExtender ID="calFecha_fin" runat="server" PopupButtonID="imgCalendarFin" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                        <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" ErrorMessage="Fecha Requerida" ForeColor="Red" ValidationGroup="enventoVer"></asp:RequiredFieldValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-lg-6">
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
                                        <asp:ImageButton ImageUrl="~/Images/view.png" runat="server" ID="ibtView" CommandName="view" CommandArgument='<%#Bind("id_pedido") %>' ToolTip="Ver detalle" data-toggle="modal" data-target="#myModal" />&nbsp;
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="row"></div>
            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel">Detalle de Pedido</h4>
                        </div>
                        <div class="modal-body">
                            <div class="col-lg-2">
                                <asp:Button Text="Exportar a Excel" CssClass="form-control btn btn-success " ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click" UseSubmitBehavior="False" />
                            </div>
                            <div class="col-lg-2">
                                <asp:Button Text="Exportar a PDF" CssClass="btn btn-danger " ID="btnExportarPdf" runat="server" OnClick="btnExportarPdf_Click" UseSubmitBehavior="False" />
                            </div>
                            <div class="row">&nbsp;</div>
                            <div class="col-lg-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvOrderDetail" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                                            <Columns>
                                                <asp:BoundField DataField="cod_barras" HeaderText="Cod. Barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="unidad" HeaderText="Unidad" ReadOnly="True" SortExpression="unidad" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="panel-footer">&nbsp;</div>
        <br />
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmVentasCanceladas.aspx.cs" Inherits="WebAppPOSAdmin.Cajas.frmVentasCanceladas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Ventas Canceladas</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-3 col-lg-offset-2">
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
            <div class="col-lg-3">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Label Text="Fecha final:" runat="server" />
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
            <div class="col-lg-2">
                <asp:Label Text="Caja" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCaja" />
            </div>
            <div class="row"></div>
            <div class="col-lg-3 col-lg-offset-2">
                <asp:Label Text="Supervisor:" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSupervisor"></asp:DropDownList>
            </div>
            <div class="col-lg-3">
                <asp:Label Text="Cajero:" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCajero"></asp:DropDownList>
            </div>
            <div class="col-lg-2">
                <div>&nbsp;</div>
                <asp:Button Text="Ver" ID="btnVer" runat="server" CssClass="btn form-control btn-primary" OnClick="btnVer_Click" />
            </div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row col-lg-8 col-lg-offset-2">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvCancelaciones" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvCancelaciones_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="id_pos" HeaderText="Caja" ReadOnly="True" SortExpression="id_pos" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="status" HeaderText="Status" ReadOnly="True" SortExpression="status" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="fecha_cancel" HeaderText="Fecha" ReadOnly="True" SortExpression="fecha_cancel" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cajero" HeaderText="Cajero" ReadOnly="True" SortExpression="cajero" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="supervisor" HeaderText="Supervisor" ReadOnly="True" SortExpression="supervisor" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ImageUrl="~/Images/view.png" runat="server" ID="ibtView" CommandName="view" CommandArgument='<%#Bind("id_venta") %>' ToolTip="Ver detalle" data-toggle="modal" data-target="#myModal" />
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
                            <h4 class="modal-title" id="myModalLabel">Detalle de Venta Cancelada</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">&nbsp;</div>
                            <div class="col-lg-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvCancelacionArticulo" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                                            <Columns>
                                                <asp:BoundField DataField="cod_barras" HeaderText="Cod. Barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="total" HeaderText="Total" ReadOnly="True" SortExpression="total" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
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
        <div class="panel-footer">
            <asp:Button Text="Crear a Excel" ID="btnExportarExcel" runat="server" CssClass="btn btn-success" OnClick="btnExportarExcel_Click" Enabled="False" />
            <asp:Button Text="Crear a PDF" ID="btnExportarPdf" runat="server" CssClass="btn btn-danger" OnClick="btnExportarPdf_Click" Enabled="False" />

        </div>
    </div>
    <br />
</asp:Content>

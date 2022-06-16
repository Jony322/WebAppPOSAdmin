<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmRelacion.aspx.cs" Inherits="WebAppPOSAdmin.Cajas.frmRelacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp;&nbsp; Relación Ventas</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-2 col-lg-offset-2">
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
                <asp:Label Text="Hora Final:" runat="server" />
                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtHoraIni" Text="07:00:00"></asp:TextBox>
                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AcceptAMPM="False" TargetControlID="txtHoraIni" MaskType="Time" Mask="99:99:99" />
            </div>
            <div class="col-lg-4">
                <asp:Label Text="Cod. Barras:" runat="server" />
                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtBarras"></asp:TextBox>
            </div>

            <div class="row"></div>
            <div class="col-lg-2 col-lg-offset-2">
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
                <asp:Label Text="Hora Final:" runat="server" />
                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtHoraFin" Text="22:00:00"></asp:TextBox>
                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptAMPM="False" TargetControlID="txtHoraFin" MaskType="Time" Mask="99:99:99" />
            </div>
            <div class="col-lg-2">
                <asp:Label Text="Caja" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCaja" />
            </div>
            <div class="col-lg-2">
                <div>&nbsp;</div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button Text="Ver" CssClass="form-control btn btn-primary" runat="server" ID="btnVer" OnClick="btnVer_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="row">&nbsp;</div>
            <div class="col-lg-8 col-lg-offset-2" style="margin-top: 110px;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvVentas" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvVentas_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="id_pos" HeaderText="Caja" ReadOnly="True" SortExpression="id_pos" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cajero" HeaderText="Cajero" ReadOnly="True" SortExpression="cajero" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="folio" HeaderText="Folio" ReadOnly="True" SortExpression="folio" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="fecha_venta" HeaderText="Fecha" ReadOnly="True" SortExpression="fecha_venta" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="total_venta" HeaderText="Total" ReadOnly="True" SortExpression="total_venta" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ImageUrl="~/Images/view.png" runat="server" ID="ibtView" CommandName="view" CommandArgument='<%#Bind("id_venta") %>' ToolTip="Ver detalle" data-toggle="modal" data-target="#myModal" />&nbsp;
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
                            <div class="row">&nbsp;</div>
                            <div class="col-lg-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvVentaDetail" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
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
                            <div class="row">&nbsp;</div>
                        </div>
                        <div class="modal-footer">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button Text="Crear Excel" ID="btnExcel" runat="server" CssClass="btn btn-success" OnClick="btnExcel_Click" />
                    <asp:Button Text="Crear PDF" ID="btnPdf" runat="server" CssClass="btn btn-danger" OnClick="btnPdf_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <br />
</asp:Content>


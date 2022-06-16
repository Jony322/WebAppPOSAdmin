<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmMovCaja.aspx.cs" Inherits="WebAppPOSAdmin.Cajas.frmMovCaja" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controles/notificaciones.ascx" TagPrefix="uc1" TagName="notificaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Movimiento Caja</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-3 col-lg-offset-2" >
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
            <div class="col-lg-3" >
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

            <div class="row col-lg-3 col-lg-offset-2">
                <asp:Label Text="Supervisor:" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSupervisor"></asp:DropDownList>
            </div>
            <div class="col-lg-3">
                <asp:Label Text="Cajero:" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCajero"></asp:DropDownList>
            </div>
            <div class="col-lg-2">
                <div>&nbsp;</div>
                <asp:Button Text="Ver" ID="btnVer" runat="server" CssClass="form-control btn btn-primary" OnClick="btnVer_Click" />
            </div>

            <div class="row col-lg-12" style="margin-top: 80px;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMovimiento" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="id_pos" HeaderText="Caja" ReadOnly="True" SortExpression="id_pos" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="True" SortExpression="fecha" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G}" />
                                <asp:BoundField DataField="cajero" HeaderText="Cajero" ReadOnly="True" SortExpression="cajero" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Supervisor">
                                    <ItemTemplate>
                                        <asp:Literal Text='<%# Eval("supervisor") %>' runat="server" ID="ltSupervisor" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="transaccion" HeaderText="Tipo de evento" ReadOnly="True" SortExpression="transaccion" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cod_barras" HeaderText="Cod. Barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ReadOnly="True" SortExpression="descripcion" />
                                <asp:BoundField DataField="precio_regular" HeaderText="Precio Real" ReadOnly="True" SortExpression="precio_regular" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="precio_venta" HeaderText="Precio Venta" ReadOnly="True" SortExpression="precio_venta" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="descuento" HeaderText="Descuento" ReadOnly="True" SortExpression="descuento" DataFormatString="{0:P0}" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel-footer">
            
            <asp:Button Text="Crear Excel" CssClass="btn btn-success" ID="btnExcel" runat="server" OnClick="btnExcel_Click" />
            <asp:Button Text="Crear PDF" CssClass="btn btn-danger" ID="btnPdf" runat="server" OnClick="btnPdf_Click" />
        </div>
    </div>
    <br />
</asp:Content>

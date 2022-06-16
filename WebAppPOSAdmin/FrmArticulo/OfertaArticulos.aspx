<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OfertaArticulos.aspx.cs" Inherits="WebAppPOSAdmin.FrmArticulo.OfertaArticulos" %>

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
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Ofertas de artículos</h4>
            </div>
        </div>
        <div class="panel-body">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">Administrador de Ofertas</a></li>
                <li role="presentation"><a href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab" onclick="changeTab(2); return false">Ofertar suspendidas</a></li>
            </ul>
            <div class="col-lg-12">
                <div class="tab-content">
                    <!-- TAB - Administrador de Ofertas -->
                    <div role="tabpanel" class="tab-pane fade in active" id="tab1">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="form-group col-lg-6">
                                    <asp:HiddenField runat="server" ID="txtIdOferta" />
                                    <asp:Label Text="Descripción de la oferta:" runat="server" />
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDescOffer" value="" placeholder="Describe la oferta. Ej. Oferta Navideña" />
                                </div>
                                <div class="form-group col-lg-3">
                                    <asp:Label Text="Fecha inicial:" runat="server" />
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFecha_ini" value="" placeholder="dd/mm/aaaa" />
                                        <span class="input-group-addon">
                                            <asp:Image ID="imgCalendarIni" ImageUrl="~/Images/calendar.png" runat="server" />
                                        </span>
                                    </div>
                                    <cc1:CalendarExtender ID="calFecha_ini" runat="server" PopupButtonID="imgCalendarIni" TargetControlID="txtFecha_ini" Format="dd/MM/yyyy" />
                                </div>
                                <div class="form-group col-lg-3">
                                    <asp:Label Text="Fecha final:" runat="server" />
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFecha_fin" value="" placeholder="dd/mm/aaaa" />
                                        <span class="input-group-addon">
                                            <asp:Image ID="imgCalendarEnd" ImageUrl="~/Images/calendar.png" runat="server" />
                                        </span>
                                        <cc1:CalendarExtender ID="calFecha_fin" runat="server" PopupButtonID="imgCalendarEnd" TargetControlID="txtFecha_fin" Format="dd/MM/yyyy" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="col-lg-12 well">
                            <div class="col-lg-6">
                                <asp:Label Text="Proveedor:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlProveedores">
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4">
                                <asp:Label Text="Descripción del artículo:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control" Placeholder="Ingrese la descripción del artículo" ID="txtDescripcion" />
                            </div>
                            <div class="col-lg-2">
                                <div>&nbsp;</div>
                                <asp:Button Text="Agregar artículos" runat="server" CssClass="form-control btn btn-block btn-primary" ID="btnAddItems" OnClick="addItems" UseSubmitBehavior="False" />
                            </div>
                        </div>
                    </div>
                    <!-- TAB - Ver suspendidas -->
                    <div role="tabpanel" class="tab-pane fade" id="tab2">
                        <div class="form-group col-lg-10">
                            <asp:Label Text="Ofertas suspendidas" runat="server" />
                            <asp:DropDownList runat="server" ID="ddlOfertas" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group col-lg-2">
                            <div class="row">&nbsp;</div>
                            <asp:Button Text="Recuperar" ValidationGroup="boton" CssClass="form-control btn btn-primary" ID="btnRecoveryOffer" runat="server" OnClick="btnRecoveryOffer_Click" />
                        </div>
                    </div>
                </div>

                <!-- BUTTONS CONTROLS -->
                <div class="form-group col-lg-3">
                    <div class="row">&nbsp;</div>
                    <asp:Button Text="Lanzar oferta" ValidationGroup="boton" CssClass="form-control btn btn-success" ID="btnOffer1" runat="server" OnClick="createOffer" />
                </div>
                <div class="form-group col-lg-3">
                    <div class="row">&nbsp;</div>
                    <asp:Button Text="Suspender oferta" ValidationGroup="boton" CssClass="form-control btn btn-warning" ID="btnSuspender" runat="server" data-toggle="modal" data-target="#SuspendedModal" OnClientClick="return false" OnClick="suspendedOffer"/>
                </div>
                <div class="form-group col-lg-3">
                    <div class="row">&nbsp;</div>
                    <asp:Button Text="Limpiar oferta" ValidationGroup="boton" CssClass="form-control btn btn-info" ID="btnCleanOffer" runat="server" OnClick="cleanOffer" OnClientClick="return confirm('La oferta será borrada. Desea limpiar la Oferta actual?');" />
                </div>
                <div class="form-group col-lg-3">
                    <div class="row">&nbsp;</div>
                    <asp:Button Text="Crear nueva oferta" ValidationGroup="boton" CssClass="form-control btn btn-primary" ID="btnNewOffer" runat="server" data-toggle="modal" data-target="#modalOffer" OnClick="btnNewOffer_Click" />
                </div>
            </div>


            <div class="col-lg-12">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvOfferDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="cod_barras" GridLines="Vertical" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="cod_barras" HeaderText="Código" SortExpression="cod_barras" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="descripcion_larga" HeaderText="Descripción" SortExpression="descripcion_larga" ReadOnly="True" />
                                <asp:BoundField DataField="unidad" HeaderText="U.M." SortExpression="unidad" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="precio_venta" HeaderText="Precio de venta" SortExpression="precio_venta" ReadOnly="True" ItemStyle-HorizontalAlign="Right" />
                                <asp:TemplateField HeaderText="Precio de oferta" SortExpression="precio_oferta" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPrecioOferta" ValidationGroup="boton" runat="server" CssClass="form-control update-price" onKeyDown="return validateNumberAndGotoNextCtrl(event,this)" Text='<%# Bind("precio_oferta") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:CommandField HeaderText="Editar" ShowEditButton="True" ShowHeader="True" />--%>
                            </Columns>
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvOfferDetail" EventName="RowUpdating" />
                        <asp:AsyncPostBackTrigger ControlID="gvOfferDetail" EventName="RowDeleting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <!-- PANEL DE SUSPENDER -->
            <div class="modal fade" id="SuspendedModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="NewModalLabel">Suspender Oferta</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group col-lg-6 col-lg-offset-2">
                                <asp:Label Text="¿Cómo deseas suspender las ofertas?" runat="server" />
                                <asp:DropDownList ID="ddlSuspender" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="1" Text="Actualizar la última lista de ofertas suspendidas"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Crear una nueva lista de ofertas suspendidas"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-1">
                                <div>&nbsp;</div>
                                <asp:Button runat="server" ID="btnSuspenderNow" Text="Suspender" CssClass="btn btn-warning" UseSubmitBehavior="False" OnClick="suspendedOffer" />
                            </div>
                            <div class="row">&nbsp;</div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" id="closeNewModal" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- PANEL IMPRIMIR -->
            <div class="modal fade" id="PrinterModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="PrinterModalLabel">Imprisión de Etiquetas</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group col-lg-6 col-lg-offset-2">
                                <asp:Label Text="¿Qué tipo de etiquetas desea imprimir?" runat="server" />
                                <asp:DropDownList ID="ddlPrinterType" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="1" Text="Etiquetas Adheribles"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Etiquetas para Anaquel"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-1">
                                <div>&nbsp;</div>
                                <asp:Button runat="server" ID="btnPrinterLabels" Text="Imprimir" CssClass="btn btn-primary" UseSubmitBehavior="False" OnClick="btnPrinterLabels_Click" />
                            </div>
                            <div class="row">&nbsp;</div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" id="closePrinterModal" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div class="panel-footer">
            <div class="col-lg-3">
                <asp:Button Text="Lanzar ofertar" ValidationGroup="boton" CssClass="form-control btn btn-success" ID="btnOffer2" runat="server" OnClick="createOffer" />
            </div>
            <div class="col-lg-3">
                <asp:Button Text="Suspender oferta" ValidationGroup="boton" CssClass="form-control btn btn-warning" ID="btnSuspender2" runat="server" data-toggle="modal" data-target="#SuspendedModal" OnClientClick="return false"/>
            </div>
            <div class="col-lg-3">
                <asp:Button Text="Limpiar oferta" ValidationGroup="boton" CssClass="form-control btn btn-info" ID="btnCleanOffer2" runat="server" OnClick="cleanOffer" />
            </div>
            <div class="col-lg-3">
                <asp:Button Text="Imprimir etiquetas" ValidationGroup="boton" CssClass="form-control btn btn-primary" ID="btnPrintLabels" runat="server" data-toggle="modal" data-target="#PrinterModal" OnClientClick="return false" />
            </div>
            <div class="row">&nbsp;</div>
        </div>
    </div>

    <br />
</asp:Content>


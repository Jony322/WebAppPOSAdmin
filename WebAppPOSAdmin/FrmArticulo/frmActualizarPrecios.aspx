<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmActualizarPrecios.aspx.cs" Inherits="WebAppPOSAdmin.FrmArticulo.frmActualizarPrecios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Cambio de precios</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:Label runat="server" ID="lblMensaje" CssClass="label-danger"></asp:Label>
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">Actualizar precios</a></li>
            </ul>
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane fade in active" id="tab1">
                    <div class="col-lg-12 well">
                        <div class="form-group col-lg-5">
                            <asp:Label Text="Proveedores:" runat="server" />
                            <asp:DropDownList runat="server" ID="ddlProveedor" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group col-lg-3">
                            <asp:Label Text="Descripción del artículo:" runat="server" />
                            <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control" Placeholder="Escribe la descripción del artículo." />
                        </div>
                        <div class="form-group col-lg-2">
                            <asp:Label Text="Ordenar por:" runat="server" />
                            <asp:DropDownList runat="server" ID="ddlOrderBy" CssClass="form-control">
                                <asp:ListItem Value="descripcion">Descripción</asp:ListItem>
                                <asp:ListItem Value="precio_compra">Costo</asp:ListItem>
                                <asp:ListItem Value="utilidad">Utilidad</asp:ListItem>
                                <asp:ListItem Value="precio_venta">Precio Venta</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group col-lg-2">
                            <div>&nbsp;</div>
                            <asp:Button Text="Buscar" runat="server" CssClass="btn btn-block btn-primary" ID="btnBuscar" OnClick="btnBuscar_Click" UseSubmitBehavior="False" CausesValidation="False" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-12 withScroll">
                <asp:UpdatePanel runat="server" ID="pULista">
                    <ContentTemplate>
                        <asp:GridView ID="gvArticulos" runat="server" AutoGenerateColumns="False" DataKeyNames="cod_barras,precio_compra,utilidad,precio_venta,iva" OnRowCancelingEdit="gvArticulos_RowCancelingEdit" OnRowEditing="gvArticulos_RowEditing" OnRowUpdating="gvArticulos_RowUpdating" GridLines="Vertical" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="cod_barras" HeaderText="Código de barras" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="descripcion_larga" HeaderText="Descripción" SortExpression="descripcion_larga" ReadOnly="True" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="unidad" HeaderText="Unidad" SortExpression="unidad" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Precio de compra" SortExpression="precio_compra" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPrecioCompra" CssClass="form-control update-price" onblur="actualizarPrecioVenta(this);" onKeyDown="return validateNumberAndGotoNextCtrl(event,this)" runat="server" Text='<%# Bind("precio_compra") %>' ValidationGroup="boton"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Utilidad" SortExpression="utilidad" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUtilidad" CssClass="form-control update-price" onblur="actualizarPrecioVenta(this);" onKeyDown="return validateNumberAndGotoNextCtrl(event,this)" runat="server" Text='<%# Bind("utilidad") %>' ValidationGroup="boton"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Precio de venta" SortExpression="precio_venta" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPrecioVenta" ValidationGroup="boton" CssClass="form-control update-price" onblur="return actualizarUtilidadArticulo(this)" onKeyDown="return validateNumberAndGotoNextCtrl(event,this)" runat="server" Text='<%# Bind("precio_venta") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="I.V.A." SortExpression="iva" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtIva" Enabled="false" ValidationGroup="boton" CssClass="form-control update-price" onblur="actualizarUtilidadArticulo(this);" runat="server" Text='<%# Bind("iva") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:GridView ID="gvKITs" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="cod_barras_kit" HeaderText="Código barras KIT" SortExpression="cod_barras_kit" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripción KIT afectado" SortExpression="descripcion" ReadOnly="True" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cod_barras_pro" HeaderText="Código barras Prod" SortExpression="cod_barras_pro" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnActualizar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel-footer">
            <asp:Button Text="Actualizar" CssClass="active btn btn-info" ID="btnActualizar" runat="server" OnClick="btnActualizar_Click" ValidationGroup="boton" OnClientClick="return confirm('Está seguro de aplicar los cambios de precio?');" />
            <asp:Button Text="Suspender" CssClass="active btn btn-default" ID="btnSuspender" runat="server" OnClick="btnSuspender_Click" ValidationGroup="boton" />
            <asp:Button Text="Llamar Suspenciones" CssClass="active btn btn-default" ID="btnLlamarSuspenciones" runat="server" OnClick="btnLlamarSuspenciones_Click" />
            <asp:Button Text="Imprimir Etiqueta" CssClass="active btn btn-default" ID="btnImprimirEtiqueta" runat="server" OnClick="btnImprimirEtiqueta_Click" OnClientClick="return confirm('Desea imprimir las etiquetas de los artículos con cambios de precio')" />
        </div>
    </div>
    <br />

    <!-- MODAL WINDOW -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Suspender actualización de precios</h4>
                </div>
                <div class="modal-body">
                    <p>
                        Existen artículos pendientes por aplicar el cambio de precio.<br />
                        ¿Qué acción desea realizar?
                    </p>
                    <asp:Button Text="Guardar e inicar nueva suspensión" CssClass="active btn btn-default" ID="btnNewItems" runat="server" data-dismiss="modal" UseSubmitBehavior="False" OnClick="btnNewItems_Click" />&nbsp;
                    <asp:Button Text="Agregar cambios a suspensión existente" CssClass="active btn btn-default" ID="btnAddItems" runat="server" data-dismiss="modal" UseSubmitBehavior="False" OnClick="btnAddItems_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>


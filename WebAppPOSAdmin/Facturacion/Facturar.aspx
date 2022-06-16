<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Facturar.aspx.cs" Inherits="WebAppPOSAdmin.Facturacion.Facturar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Facturaci&oacute;n</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:Panel runat="server" DefaultButton="btnRecovery">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="form-group col-lg-3">
                            <asp:Label Text="RFC del Cliente:" runat="server" />
                            <asp:TextBox runat="server" PlaceHolder="Ingresa RFC a facturar" CssClass="form-control text-center" ID="txtRFC" />
                            <asp:HiddenField runat="server" ID="txtIdCliente"></asp:HiddenField>
                            <asp:Button runat="server" ID="btnRecovery" Style="visibility: hidden" OnClick="btnRecovery_Click" UseSubmitBehavior="False" />
                            </span>
                        </div>
                        <div class=" form-group col-lg-1">
                            <div>&nbsp;</div>
                            <asp:ImageButton runat="server" ImageUrl="~/Images/find.png" ID="ibtFindItem" CssClass="btn btn-default btn-lg" data-toggle="modal" data-target="#myModal" ToolTip="Buscar Cliente" OnClick="ibtFindItem_Click" />
                        </div>
                        <div class=" form-group col-lg-1">
                            <div>&nbsp;</div>
                            <asp:ImageButton runat="server" ImageUrl="~/Images/add_user.png" ID="ibtAddClient" CssClass="btn btn-default btn-lg" ToolTip="Agregar Cliente" />
                        </div>
                        <div class="form-group col-lg-7">
                            <asp:Label Text="Razón Social:" runat="server" />
                            <div class="input-group">
                                <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                                <asp:TextBox runat="server" PlaceHolder="" CssClass="form-control" ID="txtRazonSocial" ReadOnly="true" />
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRecovery" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>

            <div class="row"></div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Correo electrónico:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-envelope"></span></span>
                            <asp:TextBox runat="server" ID="txtCorreo" CssClass="form-control" TextMode="Email" />
                        </div>
                    </div>
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Correo electrónico 2:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-envelope"></span></span>
                            <asp:TextBox runat="server" ID="txtCorreo2" CssClass="form-control" TextMode="Email" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnRecovery" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row"></div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Caja" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCaja" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Ticket:" runat="server" />
                        <asp:TextBox runat="server" PlaceHolder="#" CssClass="form-control text-center" ID="txtFolio" onkeypress="return validateIntegerNumber(event)" />
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Método de Pago:" runat="server" />
                        <asp:ListBox ID="lstMetodoPago" runat="server" SelectionMode="Multiple" CssClass="form-control" Rows="3" />
                    </div>

                    <div class="form-group col-lg-5">
                        <asp:Label Text="Uso de CFDI:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlUsoCFDI" />
                    </div>

                    <div class="row">&nbsp;</div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Condiciones de pago:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCondicionPago" />
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Tipo de comprobante:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlTipoComprobante" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="CFDI relacionado:" runat="server" />
                        <asp:TextBox runat="server" PlaceHolder="# CFDI" CssClass="form-control text-center" ID="txtCFDIrelacionado" />
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Label Text="UUID relacionado:" runat="server" />
                        <asp:TextBox runat="server" PlaceHolder="# UUID" CssClass="form-control text-center" ID="txtUUIDrelacionado" />
                    </div>
                    <div class="form-group col-lg-2">
                        <div>&nbsp;</div>
                        <asp:Button runat="server" ID="btnAddTicket" Text="Agregar Ticket" CssClass="form-control btn btn-success" OnClick="btnAddTicket_Click" UseSubmitBehavior="False" />
                    </div>
                    <div class="form-group col-lg-2">
                        <div>&nbsp;</div>
                        <asp:Button runat="server" ID="btnAddItem" Text="Agregar Artículo" CssClass="form-control btn btn-primary" data-toggle="modal" data-target="#NewModal" />
                    </div>
                    <div class="form-group col-lg-2">
                        <div>&nbsp;</div>
                        <asp:Button runat="server" ID="btnResetDetail" Text="Limpiar detalle" CssClass="form-control btn btn-info" OnClick="btnResetDetail_Click" />
                    </div>

                    <div class="row">&nbsp;</div>
                    <div class="form-group col-lg-2 col-lg-offset-1">
                        <asp:Label Text="Sub Total:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" ID="txtSubTotal" Text="0.00" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Descuento:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" ID="txtDescuento" Text="0.00" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="I.V.A.:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" ID="txtIVA" Text="0.00" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Total:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" ID="txtTotal" Text="0.00" ReadOnly="true" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <div>&nbsp;</div>
                        <asp:Button runat="server" ID="btnFacturar" Text="Facturar" CssClass="btn btn-primary" Enabled="False" OnClick="btnFacturar_Click" UseSubmitBehavior="true" OnClientClick="return confirm('Está seguro de genera la Factura?')" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="row col-lg-12" style="margin-top: 20px;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="cod_barras" HeaderText="Código barras" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" SortExpression="descripcion" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="unidad" HeaderText="Unidad" SortExpression="unidad" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="total" HeaderText="Total" SortExpression="total" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- PANEL DE BUSQUEDA -->
            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel">Buscar Cliente</h4>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="form-group col-lg-12 input-group">
                                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFindClient" PlaceHolder="Escribe lo que estás buscando" />
                                        <span class="input-group-btn">
                                            <asp:Button Text="Buscar" CssClass="btn btn-success" Style="font-size: 14px" ID="btnFindClient" runat="server" UseSubmitBehavior="False" OnClick="btnFindClient_Click" />
                                        </span>
                                    </div>
                                    <div class="form-group col-lg-12">
                                        <asp:GridView ID="gvResultsFind" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" OnRowCommand="gvResultsFind_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="rfc" HeaderText="RFC" SortExpression="rfc" ItemStyle-HorizontalAlign="center" />
                                                <asp:BoundField DataField="razon_social" HeaderText="Razón Social" SortExpression="descripcion" />
                                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" Text="Seleccionar" CommandName="selectedItem" CommandArgument='<%# Eval("id_cliente") + "," + Eval("rfc") %>' ToolTip="Seleccionar" OnClientClick="$('#closeModal').click()" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" id="closeModal" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- PANEL DE BUSQUEDA -->
            <div class="modal fade" id="NewModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="NewModalLabel">Agregar art&iacute;culo</h4>
                        </div>
                        <div class="modal-body">
                            <div class="col-lg-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="form-group col-lg-3">
                                            <asp:Label Text="Clave de Producto SAT:" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtBarCode" PlaceHolder="00000000000" />
                                        </div>
                                        <div class="form-group col-lg-6">
                                            <asp:Label Text="Descripción:" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control text-left" ID="txtDescripcion" PlaceHolder="Descripción del artículo" />
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <asp:Label Text="Unidad:" runat="server" />
                                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlUnidad"></asp:DropDownList>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <asp:Label Text="Cantidad:" runat="server" />
                                            <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCantidad" Text="0" OnBlur="CalcularTotalArticulo()" />
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <asp:Label Text="Precio Unitario:" runat="server" />
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox runat="server" CssClass="form-control text-right" ID="txtPrecioVta" Text="0.00" OnBlur="CalcularTotalArticulo()" />
                                            </div>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <asp:Label Text="I.V.A." runat="server" />
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <asp:CheckBox ID="chkIVA" runat="server" Checked="false" OnCheckedChanged="chkIVA_CheckedChanged" AutoPostBack="True" />
                                                </span>
                                                <asp:TextBox runat="server" CssClass="form-control text-right" Text="0.00" ID="txtItemIVA" />
                                                <span class="input-group-addon">%</span>
                                            </div>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <asp:Label Text="Total:" runat="server" />
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox runat="server" CssClass="form-control text-right" ID="txtTotalItem" Text="0.00" ReadOnly="true" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkIVA" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnAddItemInvoice" Text="Agregar artículo" CssClass="btn btn-success" UseSubmitBehavior="False" OnClick="btnAddItemInvoice_Click" />
                            <button type="button" class="btn btn-default" id="closeNewModal" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel-footer">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Label runat="server" ID="lblWarning" CssClass="form-control" Text="" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <br />
</asp:Content>

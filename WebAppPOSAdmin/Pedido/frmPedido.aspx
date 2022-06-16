<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPedido.aspx.cs" Inherits="WebAppPOSAdmin.Pedido.frmPedido" %>

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
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Pedidos</h4>
            </div>
        </div>
        <div class="panel-body">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">Registrar</a></li>
                <li role="presentation"><a href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab" onclick="changeTab(2); return false">Suspendidos</a></li>
            </ul>
            <div class="tab-content">
                <!-- REGISTRO DE PEDIDOS -->
                <div role="tabpanel" class="tab-pane fade in active" id="tab1">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>


                            <asp:HiddenField runat="server" ID="txtID"></asp:HiddenField>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Plan" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlTipoPlan" />
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Histórico" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlHistorico" />
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Días:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDiasPedir" />
                            </div>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Proveedor:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlProveedor" />
                            </div>
                            <div class="form-group col-lg-1">
                                <div>&nbsp;</div>
                                <asp:Button ID="btnVer" runat="server" Text="Ver" CssClass="form-control active btn btn-primary" OnClick="btnVer_Click" UseSubmitBehavior="False" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!-- PEDIDOS -->
                <div role="tabpanel" class="tab-pane fade" id="tab2">
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Pedidos suspendidos:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSuspendidos" />
                    </div>
                    <div class="form-group col-lg-3">
                        <br />
                        <asp:Button ID="btnRecuperar" runat="server" Text="Recuperar" CssClass="btn btn-primary" OnClick="btnRecuperar_Click" UseSubmitBehavior="False" />
                    </div>
                </div>
            </div>


            <div class="form-group col-lg-12">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div style="height: 350px; width: auto; overflow: auto;">
                            <asp:GridView ID="gvPedidos" DataKeyNames="cod_barras" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvPedidos_RowDataBound" CssClass="table table-striped" OnRowCommand="gvPedidos_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="cod_barras" HeaderText="Código barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="articulo" HeaderText="Descripción del artículo" ReadOnly="True" SortExpression="articulo" />
                                    <asp:BoundField DataField="unidad" HeaderText="Unidad " ReadOnly="True" SortExpression="unidad" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="cantidad_um" HeaderText="UMC" ReadOnly="True" SortExpression="cantidad_um" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                    <asp:BoundField DataField="costo" HeaderText="Costo" ReadOnly="True" SortExpression="costo" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F}" />
                                    <asp:BoundField DataField="stock_cja" HeaderText="Exis. cja" ReadOnly="True" SortExpression="stock_cja" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                    <asp:BoundField DataField="stock_pza" HeaderText="Exis. pza" ReadOnly="True" SortExpression="stock_pza" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                    <asp:BoundField DataField="sugerido" HeaderText="Sugerido" SortExpression="sugerido" ReadOnly="true" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                    <asp:TemplateField HeaderText="A pedir" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNuevoValor" ValidationGroup="boton" CssClass="form-control textBox-center" runat="server" Text='<%# Bind("pedir","{0:G9}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" ReadOnly="true" Visible="false" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="~/Images/cancel.png" runat="server" ID="ibtDelete" CommandName="obsolete" CommandArgument='<%#Bind("cod_barras") %>' ToolTip="Mandar a Obsoletos" OnClientClick="return confirm('Desea mandar a obsoletos éste artículo?')" />
                                            <asp:HiddenField runat="server" ID="txtCodigoAnexo" Value='<%# Bind("cod_anexo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle BackColor="Blue" Font-Bold="True" ForeColor="Green" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvPedidos" EventName="RowUpdating" />
                        <asp:AsyncPostBackTrigger ControlID="btnRecuperar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnVer" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="col-lg-12 " style="margin-top: 5px;">
                        <div class="form-group col-lg-offset-5 col-lg-2">
                            <asp:Label Text="Total Cajas:" runat="server" />
                            <asp:TextBox ID="txtTotalCjs" CssClass="form-control text-center" runat="server" Text="0"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-3">
                            <asp:Label Text="Costo Total:" runat="server" />
                            <asp:TextBox ID="txtTotalPedido" CssClass="form-control text-right" runat="server" Text="0.00"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-2">
                            <div>&nbsp;</div>
                            <asp:Button ID="btnCalculate" runat="server" Text="Calcular Pedido" CssClass="form-control active btn btn-primary" UseSubmitBehavior="False" OnClick="btnCalculate_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="col-lg-12" style="margin-top: 6px;">
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Enero" runat="server" />
                            <input type="text" id="actual_ene" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Febrero" runat="server" />
                            <input type="text" id="actual_feb" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Marzo" runat="server" />
                            <input type="text" id="actual_mar" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Abril" runat="server" />
                            <input type="text" id="actual_abr" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Mayo" runat="server" />
                            <input type="text" id="actual_may" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Junio" runat="server" />
                            <input type="text" id="actual_jun" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Julio" runat="server" />
                            <input type="text" id="actual_jul" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Agosto" runat="server" />
                            <input type="text" id="actual_ago" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Septiembre" runat="server" />
                            <input type="text" id="actual_sep" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Octubre" runat="server" />
                            <input type="text" id="actual_oct" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Noviembre" runat="server" />
                            <input type="text" id="actual_nov" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Diciembre" runat="server" />
                            <input type="text" id="actual_dic" class="form-control" disabled value="0.00" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="col-lg-12" style="margin-top: 5px;">
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Enero" runat="server" />
                            <input type="text" id="pasado_ene" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Febrero" runat="server" />
                            <input type="text" id="pasado_feb" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Marzo" runat="server" />
                            <input type="text" id="pasado_mar" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Abril" runat="server" />
                            <input type="text" id="pasado_abr" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Mayo" runat="server" />
                            <input type="text" id="pasado_may" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Junio" runat="server" />
                            <input type="text" id="pasado_jun" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Julio" runat="server" />
                            <input type="text" id="pasado_jul" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Agosto" runat="server" />
                            <input type="text" id="pasado_ago" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Septiembre" runat="server" />
                            <input type="text" id="pasado_sep" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Octubre" runat="server" />
                            <input type="text" id="pasado_oct" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Noviembre" runat="server" />
                            <input type="text" id="pasado_nov" class="form-control" disabled value="0.00" />
                        </div>
                        <div class="form-group col-lg-1">
                            <asp:Label Text="Diciembre" runat="server" />
                            <input type="text" id="pasado_dic" class="form-control" disabled value="0.00" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="panel-footer">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button Text="Guardar Pedido" ID="btnGuardar" CssClass="active btn btn-success" runat="server" OnClick="btnGuardar_Click" ValidationGroup="boton" UseSubmitBehavior="False" />
                    <asp:Button Text="Suspender Pedido" ID="btnSuspender" CssClass="active btn btn-warning" runat="server" OnClick="btnSuspender_Click" UseSubmitBehavior="False" />
                    <asp:Button Text="Exportar a PDF" ID="btnExportarPDF" runat="server" CssClass="active btn btn-danger" OnClick="btnExportarPDF_Click" Enabled="False" UseSubmitBehavior="False" />
                    <asp:Button Text="Exportar a XLS" ID="btnExportarXLS" runat="server" CssClass="active btn btn-primary" UseSubmitBehavior="False" OnClick="btnExportarXLS_Click" Enabled="False" />
                    <asp:Label ID="etEstado" Visible="false" runat="server" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <br />
</asp:Content>

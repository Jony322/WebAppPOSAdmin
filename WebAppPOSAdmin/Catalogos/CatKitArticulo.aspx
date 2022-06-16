<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CatKitArticulo.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.CatKitArticulo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controles/notificaciones.ascx" TagPrefix="uc1" TagName="notificaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:notificaciones runat="server" ID="notificaciones" />

    <script>

        var iva = parseFloat(<%= decimal.Parse(Session["iva"].ToString()) %>);
        var statusCRUD = "<%= Session["status"] %>";
        var BarCode = "<%= Session["barCode"] %>";
        var InternalCode = "<%= Session["internalCode"] %>";
    </script>

    <div class="panel panel-success quitar-margin">

        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-pencil"></span>&nbsp&nbsp KIT de artículos</h4>
            </div>
        </div>
        <div class="panel-body">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">KIT de Artículos</a></li>
                <li role="presentation"><a href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab" onclick="changeTab(2); return false">Buscar</a></li>
            </ul>
            <!--BEGIN TABS -->
            <div class="tab-content col-lg-12">
                <!-- TAB PARA REGISTRAR NUEVO ARTÍCULO -->
                <div role="tabpanel" class="tab-pane fade in active" id="tab1">
                    <menu type="context" id="codeGenerate">
                        <menuitem label="Generar código normal" id="codNormal" onclick="getCodigo('normal','<%= txtCodBarras.ClientID %>'); return false" icon="/Images/barcode.png"></menuitem>
                        <menuitem label="Generar código pesable" id="codPesable" onclick="getCodigo('pesable','<%= txtCodBarras.ClientID %>'); return false" icon="/Images/barcode.png"></menuitem>
                        <menuitem label="Generar código no pesable" id="codNoPesable" onclick="getCodigo('nopesable','<%= txtCodBarras.ClientID %>'); return false" icon="/Images/barcode.png"></menuitem>
                    </menu>
                    <div class="form-group col-lg-4">
                        <asp:Label Text="Código de barras del KIT:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCodBarras" onBlur="validateBarCodeKit(this)" contextmenu="codeGenerate" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Código Interno:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCodInterno" onblur="validateInternalCode(this)" />
                    </div>
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Descripción normal:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDescripcion" onblur="copyText(this,'txtDescripcioCorta')" />
                    </div>
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Descripcion corta:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDescripcioCorta" MaxLength="30" />
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Fecha inicial del Kit:" runat="server" />
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtFecha_ini" placeholder="dd/mm/aaaa" />
                            <span class="input-group-addon">
                                <asp:Image ID="imgCalendarIni" ImageUrl="~/Images/calendar.png" runat="server" />
                            </span>
                        </div>
                        <cc1:CalendarExtender ID="calFecha_ini" runat="server" PopupButtonID="imgCalendarIni" TargetControlID="txtFecha_ini" Format="dd/MM/yyyy" />
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Fecha final del Kit:" runat="server" />
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtFecha_fin" placeholder="dd/mm/aaaa" />
                            <span class="input-group-addon">
                                <asp:Image ID="imgCalendarEnd" ImageUrl="~/Images/calendar.png" runat="server" />
                            </span>
                            <cc1:CalendarExtender ID="calFecha_fin" runat="server" PopupButtonID="imgCalendarEnd" TargetControlID="txtFecha_fin" Format="dd/MM/yyyy" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Unidad:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlUnidad"></asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Piezas:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtPiezas" Text="1" TextMode="Number" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Costo:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" Text="0.00" ID="txtCosto" onblur="calcularPrecioVentaArticulo(this)" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Utilidad:" runat="server" />
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control text-right" Text="0.00" ID="txtUtilidad" onblur="calcularPrecioVentaArticulo()" />
                            <span class="input-group-addon">%</span>
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Precio venta:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" Text="0.00" ID="txtPrecioVenta" onblur="calcularUtilidadArticulo()" />
                        </div>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="I.V.A." runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">
                                <asp:CheckBox ID="chkIVA" runat="server" onclick="setIVA(this)" Checked="false" />
                                <!--input type="checkbox" name="chkIVA" id="chkIVA" aria-label="IVA" onclick="setIVA(this)"-->
                            </span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" Text="0.00" ID="txtIva" onblur="calcularPrecioVentaArticulo()" />
                            <span class="input-group-addon">%</span>
                        </div>
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Disponible:" runat="server" />
                        <div class="input-group">
                            <asp:CheckBox runat="server" CssClass="form-control" ID="chkDisponible" Checked="true" />
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div>&nbsp;</div>
                        <div class="input-group">
                            <asp:Button Text="Guardar KIT" CssClass="btn btn-success" runat="server" ID="btnSave" OnClick="btnSave_Click" />
                        </div>
                    </div>

                    <div class="col-lg-12 well">
                        <asp:UpdatePanel ID="upKitDetail" runat="server">
                            <contenttemplate>

                                <div class="form-group col-lg-5">
                                    <asp:Label Text="Código de barras:" runat="server" />
                                    <div class="input-group">
                                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtBarCodeToKit" placeholder="Ingresar código de barras" onBlur="validateBarCodeKit(this)" />
                                        <span class="input-group-btn">
                                            <button type="button" class="btn btn-default" style="font-size: 14px" data-toggle="modal" data-target="#myModal" onclick="return resetFinderKit()">Buscar artículo</button>
                                        </span>
                                    </div>
                                </div>
                                <div class="form-group col-lg-3">
                                    <asp:Label Text="Piezas:" runat="server" />
                                    <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtQuantityToKit" Text="1" TextMode="Number" />
                                </div>
                                <div class="col-lg-4">
                                    <div>&nbsp;</div>
                                    <div class="input-group">
                                        <asp:Button Text="Agregar/Actualizar artículo al KIT" CssClass="btn btn-success" runat="server" ID="btnAddItemToKit" UseSubmitBehavior="False" OnClick="btnAddItemToKit_Click" />
                                    </div>
                                </div>

                                <div class="col-lg-12">
                                    <!-- Table -->
                                    <asp:GridView ID="gvKitDetail" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvKitDetail_RowCommand">
                                        <columns>
                                            <asp:BoundField DataField="cod_barras" HeaderText="Código de barras" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Descripción del artículo" />
                                            <asp:BoundField DataField="precio_venta" HeaderText="Precio vta del artículo" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cantidad" HeaderText="Cantidad en KIT" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Editar artículo" ItemStyle-HorizontalAlign="Center">
                                                <itemtemplate>
                                                    <asp:ImageButton ImageUrl="~/Images/editar.png" CssClass="btnEditar" runat="server" ToolTip="Actualizar artículo del KIT" ID="updateToKit" CommandName="update" CommandArgument='<%# Bind("cod_barras") %>' />
                                                </itemtemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eliminar artículo" ItemStyle-HorizontalAlign="Center">
                                                <itemtemplate>
                                                    <asp:ImageButton ImageUrl="~/Images/delete.png" CssClass="btnEditar" runat="server" ToolTip="Quitar del KIT" ID="deleteToKit" CommandName="delete" CommandArgument='<%# Bind("cod_barras") %>' OnClientClick="return confirm('Está seguro de querer eliminar éste artículo del Kit?');" />
                                                </itemtemplate>
                                            </asp:TemplateField>
                                        </columns>
                                    </asp:GridView>
                                </div>

                            </contenttemplate>

                            <triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvKitDetail" EventName="RowUpdating" />
                                <asp:AsyncPostBackTrigger ControlID="gvKitDetail" EventName="RowDeleting" />
                            </triggers>

                        </asp:UpdatePanel>
                    </div>

                    <!-- Modal -->
                    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                        <div class="modal-dialog modal-lg" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title" id="myModalLabel">Buscar artículo</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-group col-lg-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">
                                                <input type="radio" name="findBy2" id="rbtBarCode2" value="barCode" />&nbsp;Código barras&nbsp;&nbsp;
                                                <input type="radio" name="findBy2" id="rbtInternalCode2" value="internalCode" />&nbsp;Código interno&nbsp;&nbsp;
                                                <input type="radio" name="findBy2" id="rbtDescription2" value="description" checked />&nbsp;Descripción&nbsp;
                                            </span>
                                            <input type="text" name="findText2" id="findText2" class="form-control" placeholder="Escribe lo que estás buscando" />
                                            <span class="input-group-btn">
                                                <button class="btn btn-lg btn-default" type="button" onclick="findArticuloToKit(findBy2.value,findText2.value); return false;" style="font-size: 14px">Buscar</button>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-12">
                                        <table id="tableRecords2" class="table table-striped" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>Cód. Barras</th>
                                                    <th>Cód. Interno</th>
                                                    <th>Descripción</th>
                                                    <th>Seleccionar</th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                                </div>
                            </div>
                        </div>
                    </div>



                </div>
                <!-- TAB BUSCAR -->
                <div role="tabpanel" class="tab-pane fade" id="tab2">
                    <div class="row">&nbsp;</div>
                    <div class="form-group col-lg-12">
                        <div class="input-group">
                            <span class="input-group-addon">
                                <input type="radio" name="findBy" id="rbtBarCode" value="barCode" />&nbsp;Código barras&nbsp;&nbsp;
                                <input type="radio" name="findBy" id="rbtInternalCode" value="internalCode" />&nbsp;Código interno&nbsp;&nbsp;
                                <input type="radio" name="findBy" id="rbtDescription" value="description" checked />&nbsp;Descripción&nbsp;
                            </span>
                            <input type="text" name="findText" id="findText" class="form-control" placeholder="Escribe lo que estás buscando" />
                            <span class="input-group-btn">
                                <button class="btn btn-lg btn-default" type="button" onclick="findArticulo(findBy.value,findText.value, true); return false;" style="font-size: 14px">Buscar</button>
                            </span>
                        </div>
                    </div>
                    <div class="form-group col-lg-12">
                        <table id="tableRecords" class="table table-striped" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Cód. Barras</th>
                                    <th>Cód. Interno</th>
                                    <th>Descripción</th>
                                    <th>Existencia</th>
                                    <th>Editar</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

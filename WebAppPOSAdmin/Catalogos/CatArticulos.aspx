<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CatArticulos.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.CatArticulos" %>

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
        var BarCode = "";
        var InternalCode = "<%= Session["internalCode"] %>"; 
    </script>

    <div class="panel panel-success quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-pencil"></span>&nbsp&nbsp Administrador de Artículos</h4>
            </div>
        </div>
        <div class="panel-body">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">Artículo</a></li>
                <li role="presentation"><a href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab" onclick="changeTab(2); return false">Anexos</a></li>
                <li role="presentation"><a href="#tab3" aria-controls="tab3" role="tab" data-toggle="tab" onclick="changeTab(3); return false">Asociados</a></li>
                <li role="presentation"><a href="#tab4" aria-controls="tab4" role="tab" data-toggle="tab" onclick="changeTab(4); return false">Buscar</a></li>
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
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Código de barras:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCodBarras" onBlur="validateBarCode(this)" contextmenu="codeGenerate" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Clave de producto" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCveProducto" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Código Interno:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCodInterno" onblur="validateInternalCode(this)" />
                    </div>
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Descripción normal:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDescripcion" onblur="copyText(this,'txtDescripcionCorta')" />
                    </div>
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Descripción corta:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDescripcionCorta" MaxLength="30" />
                    </div>
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Proveedor:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlProveedor"></asp:DropDownList>
                    </div>
                    <div class="row"></div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Departamento:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDepartamento" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:UpdatePanel runat="server" ID="UPrimeraLinia">
                            <ContentTemplate>
                                <asp:Label Text="Categoría:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlPrimeraLinia" AutoPostBack="True" OnSelectedIndexChanged="ddlPrimeraLinia_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlDepartamento" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:UpdatePanel runat="server" ID="UpSegundaLinia">
                            <ContentTemplate>
                                <asp:Label Text="Sub-Categoría:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSegundaLinia" AutoPostBack="True" OnSelectedIndexChanged="ddlSegundaLinia_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlPrimeraLinia" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:UpdatePanel runat="server" ID="UpTereceraLinia">
                            <ContentTemplate>
                                <asp:Label Text="Línea:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlTerceraLinia"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlSegundaLinia" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Unidad:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlUnidad"></asp:DropDownList>
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Piezas:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtPiezas" Text="1" TextMode="Number" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Stock:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" Text="0" ID="txtStock" TextMode="Number" ReadOnly="True" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Stock Mínimo:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" Text="0" ID="txtStockMinimo" TextMode="Number" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Stock Máximo:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" Text="0" ID="txtStockMaximo" TextMode="Number" />
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Costo:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" Text="0.00" ID="txtCosto" onblur="calcularPrecioVentaArticulo()" />
                        </div>
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Utilidad:" runat="server" />
                        <div class="input-group">
                            <asp:TextBox runat="server" CssClass="form-control text-right" Text="0.00" ID="txtUtilidad" onblur="calcularPrecioVentaArticulo()" />
                            <span class="input-group-addon">%</span>
                        </div>
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Precio venta:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox runat="server" CssClass="form-control text-right" Text="0.00" ID="txtPrecioVenta" onblur="calcularUtilidadArticulo()" />
                        </div>
                    </div>
                    <div class="form-group col-lg-3">
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
                    <div class="col-lg-3">
                        <asp:Button Text="Guardar artículo" CssClass="btn btn-primary btn-block" runat="server" ID="btnSaveItem" OnClick="btnGuardar_Click" />
                    </div>
                </div>
                <!-- TAB PARA AGREGAR ANEXOS -->
                <div role="tabpanel" class="tab-pane fade" id="tab2">
                    <asp:UpdatePanel ID="upDetailAnexos" runat="server">
                        <ContentTemplate>
                            <menu type="context" id="codeGenerateAnexo">
                                <menuitem label="Genarar código normal" id="codNormal" onclick="getCodigo('normal','<%= txtCodBarrasAnexo.ClientID %>'); return false" icon="/Images/barcode.png"></menuitem>
                                <menuitem label="Generar código pesable" id="codPesable" onclick="getCodigo('pesable','<%= txtCodBarrasAnexo.ClientID %>'); return false" icon="/Images/barcode.png"></menuitem>
                                <menuitem label="Generar código no pesable" id="codNoPesable" onclick="getCodigo('nopesable','<%= txtCodBarrasAnexo.ClientID %>'); return false" icon="/Images/barcode.png"></menuitem>
                            </menu>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Código de Barras:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCodBarrasAnexo" onBlur="return validateBarCodeAnexo(this)" contextmenu="codeGenerateAnexo" />
                            </div>
                            <div class="form-group col-lg-2 ">
                                <asp:Label Text="Código Interno:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCodInternoAnexo" onblur="validateInternalCode(this)" />
                            </div>
                            <div class="form-group col-lg-4 ">
                                <asp:Label Text="Descripción normal:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtDescripcionAnexo" onblur="copyText(this,'txtDescripcionCortaAnexo')" />
                            </div>
                            <div class="form-group col-lg-4 ">
                                <asp:Label Text="Descripción corta:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtDescripcionCortaAnexo" MaxLength="30" />
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Unidad:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlUnidadAnexo"></asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Piezas:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control text-center" Text="1" ID="txtPiezasAnexo" TextMode="Number" onblur="calcularCostoAnexoAndPrecioVenta()" />
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Costo:" runat="server" />
                                <div class="input-group">
                                    <span class="input-group-addon">$</span>
                                    <asp:TextBox runat="server" CssClass="form-control text-right" Text="0" ID="txtCostoAnexo" onblur="calcularPrecioVentaAnexo()" />
                                </div>
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Utilidad:" runat="server" />
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control text-right" Text="0" ID="txtUtilidadAnexo" onblur="calcularPrecioVentaAnexo()" />
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="Precio venta:" runat="server" />
                                <div class="input-group">
                                    <span class="input-group-addon">$</span>
                                    <asp:TextBox runat="server" CssClass="form-control text-right" Text="0" ID="txtPrecioVentaAnexo" onblur="calcularUtilidadAnexo()" />
                                </div>
                            </div>
                            <div></div>
                            <div class="form-group col-lg-2">
                                <div>&nbsp;</div>
                                <asp:Button Text="Guardar/Actualizar" CssClass="btn btn-success btn-block" runat="server" ID="btnAnexar" OnClick="btnAnexar_Click" UseSubmitBehavior="False" />
                            </div>
                            <div class="row"></div>
                            <div class="col-lg-12 well">
                                <asp:GridView ID="gvDetailAnexos" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvDetailAnexos_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="cod_barras" HeaderText="Código de barras" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="descripcion" HeaderText="Descripción del artículo" />
                                        <asp:BoundField DataField="um" HeaderText="Unidad" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="cantidad_um" HeaderText="Piezas" DataFormatString="{0:G5}" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="precio_compra" HeaderText="Costo" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="utilidad" HeaderText="Utilidad" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="precio_venta" HeaderText="Precio Vta" DataFormatString="{0:F2}" ItemStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/Images/editar.png" CssClass="btnEditar" runat="server" ToolTip="Actualizar anexo" ID="updateAnexo" CommandName="update" CommandArgument='<%# Bind("cod_barras") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/Images/delete.png" CssClass="btnEditar" runat="server" ToolTip="Eliminar anexo" ID="deleteAnexo" CommandName="delete" CommandArgument='<%# Bind("cod_barras") %>' OnClientClick="return confirm('Está seguro de eliminar éste anexo?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvDetailAnexos" EventName="RowDeleting" />
                            <asp:AsyncPostBackTrigger ControlID="gvDetailAnexos" EventName="RowUpdating" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <!-- TAB PARA AGREGAR ASOCIADOS -->
                <div role="tabpanel" class="tab-pane fade" id="tab3">
                    <asp:UpdatePanel ID="upDetailAsociados" runat="server">
                        <ContentTemplate>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Codigo Asociado" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control text-center" placeholder="Código asociado" ID="txtCodAsociado" />
                            </div>
                            <div class="form-group col-lg-2">
                                <div>&nbsp;</div>
                                <asp:Button Text="Asociar código" runat="server" CssClass="btn btn-success btn-block" ID="btnAsociar" OnClick="btnAsociar_Click" UseSubmitBehavior="False" />
                            </div>
                            <div class="col-lg-6 well">
                                <asp:GridView ID="gvAsociados" runat="server" AutoGenerateColumns="False" OnRowCommand="gvAsociados_RowCommand" CssClass="table table-striped">
                                    <Columns>
                                        <asp:BoundField DataField="cod_barras" HeaderText="Codigo Asociado" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="Convertir en principal" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/Images/convertir.png" CssClass="btnEditar" runat="server" ToolTip="Convertir en principal" ID="imgConvertir" CommandName="convert" CommandArgument='<%# Bind("cod_barras") %>' OnClientClick="return confirm('Está seguro de converir éste artículo en principal?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ImageUrl="~/Images/delete.png" CssClass="btnEditar" runat="server" ToolTip="Convertir en principal" ID="imgEliminar" CommandName="delete" CommandArgument='<%# Bind("cod_barras") %>' OnClientClick="return confirm('Está seguro de eliminar éste artículo asociado?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvAsociados" EventName="RowDeleting" />
                            <asp:AsyncPostBackTrigger ControlID="gvAsociados" EventName="RowUpdating" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <!-- TAB PARA BUSCAR ARTÍCULOS -->
                <div role="tabpanel" class="tab-pane fade" id="tab4">
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
                                <button class="btn btn-lg btn-default" type="button" onclick="findArticulo(findBy.value,findText.value, false); return false;" style="font-size: 14px">Buscar</button>
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
            <!--END TABS -->
        </div>
    </div>
    <br />
</asp:Content>

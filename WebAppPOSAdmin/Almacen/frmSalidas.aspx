<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSalidas.aspx.cs" Inherits="WebAppPOSAdmin.Almacen.frmSalidas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Almacen Salidas</h4>
            </div>
        </div>
        <div class="panel-body">
            <!-- CONTROLES PRINCIPALES -->
            <div class="form-group col-lg-4">
                <asp:Label Text="Movimiento Almacen:" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlMvtoAlmacen" />
            </div>
            <div class="form-group col-lg-8">
                <asp:Label Text="Observaciones" runat="server" />
                <asp:TextBox runat="server" PlaceHolder="Observaciones...." CssClass="form-control" ID="txtObservaciones" />
                <div>&nbsp;</div>
            </div>

            <!--2DA LÍNEA DE CONTROLES-->
            <asp:Panel runat="server" DefaultButton="btnBuscar">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="form-group col-lg-2">
                            <asp:Label Text="Cod. Barras:" runat="server" />
                            <span class="input-group">
                                <asp:TextBox runat="server" PlaceHolder="Cod. barras del articulo" CssClass="form-control" ID="txtCodBarras" />
                                <span class="input-group-btn">
                                    <asp:ImageButton runat="server" ImageUrl="~/Images/find.png" ID="ibtFindItem" CssClass="btn btn-default btn-lg" OnClick="ibtFindItem_Click" data-toggle="modal" data-target="#myModal" />
                                </span>
                            </span>
                            <asp:Button runat="server" CssClass="btn btn-primary btn-lg" ID="btnBuscar" OnClick="btnBuscar_Click" ValidationGroup="eventoBuscar" Style="visibility: hidden" />
                            <asp:RequiredFieldValidator ID="rfvCodBarras" ControlToValidate="txtCodBarras" runat="server" ErrorMessage="El código de barras es requerido" ForeColor="Red" ValidationGroup="eventoBuscar"></asp:RequiredFieldValidator>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAnexar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Descripción:" runat="server" />
                        <asp:TextBox runat="server" Enabled="false" PlaceHolder="" CssClass="form-control" ID="txtDescripcion" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtDescripcion" runat="server" ErrorMessage="Observación Requerida" ForeColor="Red" ValidationGroup="eventoAnexar"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-lg-1">
                        <asp:Label Text="UMC:" runat="server" />
                        <asp:TextBox runat="server" Enabled="false" PlaceHolder="" CssClass="form-control" ID="txtUMC" />
                    </div>
                    <div class="form-group col-lg-1">
                        <asp:Label Text="Unidad:" runat="server" />
                        <asp:TextBox runat="server" Enabled="false" PlaceHolder="" CssClass="form-control" ID="txtUnidad" />
                    </div>
                    <div class="form-group col-lg-1">
                        <asp:Label Text="Cantidad:" runat="server" />
                        <asp:TextBox runat="server" PlaceHolder="" CssClass="form-control" ID="txtCantidad" onFocus="this.select();" />
                        <asp:RequiredFieldValidator ID="rfvCantidad" ControlToValidate="txtCantidad" runat="server" ErrorMessage="Cantidad Requerida" ForeColor="Red" ValidationGroup="eventoAnexar"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revCantidad" ControlToValidate="txtCantidad" runat="server" ErrorMessage="Sólo valores enteros o decimales" ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)" ValidationGroup="eventoAnexar" ForeColor="Red"></asp:RegularExpressionValidator>
                    </div>
                    <div class="form-group col-lg-1">
                        <asp:Label Text="Regalo:" runat="server" />
                        <asp:TextBox runat="server" PlaceHolder="" CssClass="form-control" ID="txtRegalo" />
                        <asp:RegularExpressionValidator ID="revRegalo" ControlToValidate="txtRegalo" runat="server" ErrorMessage="Sólo valores enteros o decimales" ForeColor="Red" ValidationGroup="eventoAnexar" ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)"></asp:RegularExpressionValidator>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Piezas:" runat="server" />
                        <asp:DropDownList runat="server" PlaceHolder="" CssClass="form-control" ID="ddlPiezas" />
                    </div>
                    <div class="form-group col-lg-1">
                        <div>&nbsp;</div>
                        <asp:Button Text="Anexar" CssClass="btn btn-success" ID="btnAnexar" runat="server" OnClick="btnAnexar_Click" ValidationGroup="eventoAnexar" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnAnexar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <!-- PANEL DE BUSCAR -->
            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel">Buscar artículo</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group col-lg-12">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="input-group">
                                            <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFindItem" PlaceHolder="Escribe lo que estás buscando" />
                                            <span class="input-group-btn">
                                                <asp:Button Text="Buscar" CssClass="btn btn-success" Style="font-size: 14px" ID="btnFindItemDesc" runat="server" UseSubmitBehavior="False" OnClick="btnFindItemDesc_Click" />
                                            </span>
                                        </div>
                                        <asp:GridView ID="gvResultsFind" runat="server" AllowPaging="False" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvResultsFind_RowCommand">
                                            <Columns>
                                                <asp:BoundField DataField="cod_barras" HeaderText="Código barras" ReadOnly="True" SortExpression="id_oferta" ItemStyle-HorizontalAlign="center" />
                                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" Text="Seleccionar" CommandName="selectedItem" CommandArgument='<%# Eval("cod_barras") %>' ToolTip="Agregar artículo" OnClientClick="$('#closeModal').click()" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="MyPagerStyle" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnFindItemDesc" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" id="closeModal" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="form-group col-lg-12">
                        <asp:GridView ID="gvAnexos" runat="server" AutoGenerateColumns="False" GridLines="Vertical" CssClass="table table-striped">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <Columns>
                                <asp:BoundField DataField="cod_barras" HeaderText="Cod. Barras" SortExpression="cod_barras" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripcion" SortExpression="descripcion" />
                                <asp:BoundField DataField="unidad" HeaderText="Unidad" SortExpression="unidad" />
                                <asp:BoundField DataField="umc" HeaderText="UMC" SortExpression="umc" />
                                <asp:BoundField DataField="can_cja" HeaderText="Can. Cja" SortExpression="can_cja" />
                                <asp:BoundField DataField="can_pza" HeaderText="Can. Pzas" SortExpression="can_pza" />
                                <asp:BoundField DataField="regalo" HeaderText="Regalo" SortExpression="regalo" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAnexar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="panel-footer">
            <asp:Button Text="Guardar" CssClass="btn btn-default" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" ValidationGroup="eventoGuardar" />
        </div>
    </div>
    <br />
</asp:Content>
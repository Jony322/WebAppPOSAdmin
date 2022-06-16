<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="confirmacionpedidos.aspx.cs" Inherits="WebAppPOSAdmin.Pedido.confirmacionpedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Confirmación de Pedidos</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-6">
                <asp:Label Text="Pedido/Proveedor:" runat="server" />
                <asp:DropDownList runat="server" ID="ddlPedido" CssClass="form-control" OnSelectedIndexChanged="ddlPedido_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Text="--SELECCIONAR--" />
                </asp:DropDownList>
            </div>
            <div class="col-lg-2">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Label Text="Días" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDias" Enabled="false" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPedido" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="col-lg-2">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Label Text="Total:" runat="server" />
                        <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" placeholder="0" Enabled="true"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCalcular" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="col-lg-2">
                <div>&nbsp;</div>
                <asp:Button Text="Calcular" runat="server" CssClass="form-control active btn btn-primary" ID="btnCalcular" OnClick="btnCalcular_Click" />
            </div>
            <div class="col-lg-12" style="margin-top: 25px">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div style="height: 350px; width: auto; overflow: auto;">
                            <asp:GridView ID="gvpedidoconfirmacion" DataKeyNames="cod_barras" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="gvpedidoconfirmacion_RowCancelingEdit" OnRowEditing="gvpedidoconfirmacion_RowEditing" OnRowUpdating="gvpedidoconfirmacion_RowUpdating" CssClass="table table-striped" OnRowDataBound="gvpedidoconfirmacion_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="cod_barras" HeaderText="Cod. Barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                    <asp:BoundField DataField="unidad" HeaderText="Unidad" ReadOnly="True" SortExpression="unidad" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="umc" HeaderText="UMC" ReadOnly="True" SortExpression="umc" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                    <%--<asp:BoundField DataField="precio_costo" HeaderText="Costo" ReadOnly="True" SortExpression="precio_costo" />--%>
                                    <asp:TemplateField HeaderText="Costo" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Literal Text='<%# Eval("precio_costo","{0:F}") %>' runat="server" ID="ltPrecioCosto" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="existencia_cja" HeaderText="Existencia Cjas." ReadOnly="True" SortExpression="existencia_cja" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                    <asp:BoundField DataField="existencia_pza" HeaderText="Existencias Pzas." ReadOnly="True" SortExpression="existencia_pza" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                    <asp:BoundField DataField="cant_original" HeaderText="Pedido Original" ReadOnly="True" SortExpression="cant_original" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                    <%--<asp:BoundField DataField="total" HeaderText="Total" ReadOnly="True" SortExpression="total" />--%>
                                    <asp:TemplateField HeaderText="Pedido Real" SortExpression="pedido_real" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtReal" CssClass="form-control textBox-center" Height="30px" Width="110px" runat="server" Text='<%# Bind("cantidad","{0:G9}") %>' onKeyDown="return validateNumberAndGotoNextCtrl(event,this)"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPedido" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
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
            <asp:Button Text="Guardar" ValidationGroup="boton" CssClass="btn btn-success" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" />
            <asp:Button Text="Autorizar" ValidationGroup="boton" CssClass="btn btn-primary" ID="btnAutorizar" runat="server" OnClick="btnAutorizar_Click" />
        </div>
    </div>
    <br />
</asp:Content>

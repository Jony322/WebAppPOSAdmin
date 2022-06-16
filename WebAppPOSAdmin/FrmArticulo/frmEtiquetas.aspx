<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmEtiquetas.aspx.cs" Inherits="WebAppPOSAdmin.FrmArticulo.frmEtiquetas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .panelModal {
            background-color: #ffffff;
            width: 50%;
            height: auto;
        }

        .modalBack {
            background-color: #000000;
            filter: alpha(opacity=90);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Etiquetas</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:Panel runat="server" DefaultButton="btnBuscar">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="form-group col-lg-4 col-lg-offset-2">
                            <asp:Label Text="Articulo:" runat="server" />
                            <asp:TextBox runat="server" PlaceHolder="Código de barras, código interno o Descripción" CssClass="form-control" ID="txtDescripcion" ValidationGroup="buscar" AutoCompleteType="Disabled" />
                            <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ErrorMessage="*" ControlToValidate="txtDescripcion" ForeColor="Red" ValidationGroup="buscar"></asp:RequiredFieldValidator>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:Label Text="" ID="etMensaje" runat="server" ForeColor="Red" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnVisible" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

                <div class="col-lg-2">
                    <br />
                    <asp:Button Text="Buscar" CssClass="active btn btn-primary " ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" ValidationGroup="buscar" UseSubmitBehavior="False" />
                    <asp:HiddenField ID="modal_inicial" runat="server" />
                    <asp:HiddenField ID="modal_second" runat="server" />
                </div>
                <div class="form-group col-lg-2">
                    <br />
                    <asp:CheckBox Text="PRECIO NORMAL" runat="server" ID="chkPrecioNormal" />
                </div>
            </asp:Panel>

            <ajaxToolkit:ModalPopupExtender ID="btnBuscar_Modal" Enabled="true" TargetControlID="modal_inicial" BackgroundCssClass="modalBack" PopupControlID="panel_modal" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll"></ajaxToolkit:ModalPopupExtender>
            <asp:Panel runat="server" ID="panel_modal" CssClass="modal-dialog" DefaultButton="btnVisible" role="dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4>Número de etiquetas</h4>
                    </div>
                    <div class="modal-body">
                        <div class="col-lg-6 col-lg-offset-3">
                            <asp:Label runat="server" Text="Cantidad:"></asp:Label>
                            <asp:TextBox runat="server" PlaceHolder="Cantidad de etiquetas" CssClass="form-control text-danger" ID="txtCantidad" ValidationGroup="mover" onfocus="this.value=1; this.select();"  />
                            <asp:RequiredFieldValidator ID="rqvCantidad" runat="server" ErrorMessage="*" ControlToValidate="txtCantidad" ForeColor="#FF3300" ValidationGroup="mover"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revCantidad" runat="server" ErrorMessage="Sólo numeros " ControlToValidate="txtCantidad" ForeColor="Red" ValidationGroup="mover" ValidationExpression="^[1-9]+\d*$"></asp:RegularExpressionValidator>
                        </div>
                        <br />
                        <asp:Button Text="Aceptar" ID="btnVisible" runat="server" OnClick="btnVisible_Click" CssClass="active btn btn-primary" UseSubmitBehavior="False" />
                    </div>
                </div>
            </asp:Panel>

            <ajaxToolkit:ModalPopupExtender ID="modalFindItem" Enabled="true" TargetControlID="modal_second" BackgroundCssClass="modalBack" PopupControlID="FindItem" runat="server" CancelControlID="btnCancel" RepositionMode="RepositionOnWindowResizeAndScroll" CacheDynamicResults="True"></ajaxToolkit:ModalPopupExtender>
            <asp:Panel runat="server" ID="FindItem" CssClass="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4>Buscar artículo por descripción</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="col-lg-8">
                                    <asp:Label runat="server" Text="Descripción del artículo:"></asp:Label>
                                    <asp:TextBox runat="server" PlaceHolder="Describe el artículo que buscas" CssClass="form-control" ID="txtFindItem" />
                                </div>
                                <div class="col-lg-2">
                                    <div>&nbsp;</div>
                                    <asp:Button Text="Buscar" ID="btnFindItem" runat="server" CssClass="active btn btn-primary" OnClick="btnFindItem_Click" />
                                </div>
                                <div class="col-lg-2">
                                    <div>&nbsp;</div>
                                    <asp:Button Text="Cancelar" ID="btnCancel" runat="server" CssClass="active btn btn-danger" OnClick="btnCancel_Click" />
                                </div>
                                <div class="row col-lg-12" style="height: 350px; overflow: auto;">
                                    <asp:GridView runat="server" ID="gvFindItem" AutoGenerateColumns="False" GridLines="Vertical" CssClass="table table-striped" OnRowCommand="gvFindItem_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="cod_barras" HeaderText="Codigo barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="select" Text="Seleccionar" CommandName="selected" CommandArgument='<%#Bind("cod_barras") %>' UseSubmitBehavior="False"></asp:Button>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">&nbsp;</div>
                </div>
            </asp:Panel>


            <div class="row col-lg-offset-2 col-lg-8">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="gvImpresion" AutoGenerateColumns="False" GridLines="Vertical" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="cod_barras" HeaderText="Codigo barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                <asp:BoundField DataField="precio_venta" HeaderText="Precio Venta" SortExpression="precio_venta" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnLimpiar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnVisible" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvImpresion" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel-footer">
            <asp:Button Text="Etiqueta adherible" CssClass="active btn btn-primary " ID="btnAdherible" runat="server" OnClick="btnAdherible_Click" />
            <asp:Button Text="Etiqueta Anaquel" CssClass="active btn btn-info " ID="btnAnaquel" runat="server" OnClick="btnAnaquel_Click" />
            <asp:Button Text="Reiniciar" CssClass="active btn btn-default" ID="btnLimpiar" runat="server" OnClick="btnLimpiar_Click" />
        </div>
    </div>
    <br />
</asp:Content>

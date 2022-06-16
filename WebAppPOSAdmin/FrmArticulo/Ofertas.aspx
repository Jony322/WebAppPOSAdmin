<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ofertas.aspx.cs" Inherits="WebAppPOSAdmin.FrmArticulo.Ofertas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-success quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Visor de Ofertas</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="form-group col-lg-12">
                        <div class="input-group">
                            Filtrar ofertas por:&nbsp;&nbsp;
                                <asp:RadioButton runat="server" GroupName="filterBy" ID="rbtAll" Text="Ver todas" Checked="true" OnCheckedChanged="rbtFilter_CheckedChanged" AutoPostBack="True" />&nbsp;&nbsp;
                                <asp:RadioButton runat="server" GroupName="filterBy" ID="rbtAvailable" Text="Sólo disponibles" OnCheckedChanged="rbtFilter_CheckedChanged" AutoPostBack="True" />
                            <asp:RadioButton runat="server" GroupName="filterBy" ID="rbtSuspended" Text="Sólo suspendidas" OnCheckedChanged="rbtFilter_CheckedChanged" AutoPostBack="True" />
                            <asp:RadioButton runat="server" GroupName="filterBy" ID="rbtCanceled" Text="Sólo canceladas" OnCheckedChanged="rbtFilter_CheckedChanged" AutoPostBack="True" />
                        </div>
                    </div>

                    <asp:GridView ID="gvOferta" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvOferta_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="num_oferta" HeaderText="Clave" ReadOnly="True" SortExpression="num_oferta" ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                            <asp:BoundField DataField="fecha_ini" HeaderText="Fecha inicial" ReadOnly="True" SortExpression="fecha_ini" ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="fecha_fin" HeaderText="Fecha final" ReadOnly="True" SortExpression="fecha_fin" ItemStyle-HorizontalAlign="center" />
                            <asp:BoundField DataField="days_expires" HeaderText="Caduca" ReadOnly="True" SortExpression="days_expires" ItemStyle-HorizontalAlign="center" />
                            <asp:TemplateField HeaderText="Estatus" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image ImageUrl='<%# Eval("status_oferta","~/Images/{0}.png") %>' runat="server" ID="imgStatus" ToolTip='<%# Eval("status_oferta") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ImageUrl="~/Images/view.png" runat="server" ID="ibtView" CommandName="view" CommandArgument='<%#Bind("id_oferta") %>' ToolTip="Ver detalle" data-toggle="modal" data-target="#myModal" />&nbsp;
                                    <asp:ImageButton ImageUrl="~/Images/editar.png" runat="server" ID="ibtUpdate" CommandName="recover" CommandArgument='<%#Bind("id_oferta") %>' ToolTip="Recuperar oferta" OnClientClick="return confirm('Desea recuperar esta Oferta?')" />&nbsp;
                                    <asp:ImageButton ImageUrl="~/Images/cancel.png" runat="server" ID="ibtDelete" CommandName="cancel" CommandArgument='<%#Bind("id_oferta") %>' ToolTip="Cancelar oferta" OnClientClick="return confirm('Esta seguro de cancelar esta Oferta?')" />
                                    <asp:ImageButton ImageUrl="~/Images/adhesive.png" runat="server" ID="ibtAdherible" CommandName="adherible" CommandArgument='<%# Eval("id_oferta") %>' ToolTip="Etiqueta adherible" OnClientClick="return confirm('Desea imprimir la oferta en etiquetas adheribles?')" />&nbsp;
                                    <asp:ImageButton ImageUrl="~/Images/anaquel.png" runat="server" ID="ibtAnaquel" CommandName="anaquel" CommandArgument='<%# Eval("id_oferta") %>' ToolTip="Etiqueta de anaquel" OnClientClick="return confirm('Desea imprimir la oferta en etiquetas de anaquel?')" />&nbsp;
                                    <asp:ImageButton ImageUrl="~/Images/estadistica.png" runat="server" ID="ibtEstadistica" CommandName="estadistica" CommandArgument='<%# Eval("id_oferta") %>' ToolTip="Estadística de Oferta" OnClientClick="return confirm('Desea ver el reporte de estadísticas de la oferta?')" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvOferta" EventName="RowUpdating" />
                    <asp:AsyncPostBackTrigger ControlID="gvOferta" EventName="RowCancelingEdit" />
                </Triggers>
            </asp:UpdatePanel>
            <!-- MODAL WINDOW -->
            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="myModalLabel">Detalle de Oferta</h4>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvDetailOferta" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvDetailOferta_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="cod_barras" HeaderText="Clave" ReadOnly="True" SortExpression="id_oferta" ItemStyle-HorizontalAlign="center" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                            <asp:BoundField DataField="precio_oferta" HeaderText="Precio Oferta" ReadOnly="True" SortExpression="fecha_ini" ItemStyle-HorizontalAlign="center" />
                                            <asp:TemplateField HeaderText="Estatus" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Image ImageUrl='<%# Eval("status_oferta","~/Images/{0}.png") %>' runat="server" ID="Image1" ToolTip='<%# Eval("status_oferta") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/Images/adhesive.png" runat="server" ID="ImageButton1" CommandName="adherible" CommandArgument='<%# Eval("id_oferta") + "," + Eval("cod_barras") %>' ToolTip="Etiqueta adherible" OnClientClick="return confirm('Desea imprimir esta etiqueta adherible?')" />&nbsp;
                                                    <asp:ImageButton ImageUrl="~/Images/anaquel.png" runat="server" ID="ImageButton2" CommandName="anaquel" CommandArgument='<%# Eval("id_oferta") + "," + Eval("cod_barras") %>' ToolTip="Etiqueta de anaquel" OnClientClick="return confirm('Desea imprimir esta etiqueta de anaquel?')" />&nbsp;
                                                    <asp:ImageButton ImageUrl="~/Images/delete.png" runat="server" ID="ImageButton3" CommandName="delete" CommandArgument='<%# Eval("id_oferta") + "," + Eval("cod_barras") %>' ToolTip="Eliminar oferta" OnClientClick="return confirm('Esta seguro de eliminar esta artículo en Oferta?')" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvDetailOferta" EventName="RowUpdating" />
                                    <asp:AsyncPostBackTrigger ControlID="gvDetailOferta" EventName="RowDeleting" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>


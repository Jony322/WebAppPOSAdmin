<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VisorArticulos.aspx.cs" Inherits="WebAppPOSAdmin.Articulos.VisorArticulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Visor de articulos</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-md-3">
                <asp:UpdatePanel ID="uPanelTree" runat="server">
                    <contenttemplate>
                        <asp:TreeView runat="server" ID="treeViewArticulos" ExpandDepth="1" PopulateNodesFromClient="False"
                            OnTreeNodePopulate="treeViewArticulos_TreeNodePopulate" OnSelectedNodeChanged="treeViewArticulos_SelectedNodeChanged" ShowLines="True">
                        </asp:TreeView>
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>

            <div class="col-md-9">
                <strong>
                    <asp:Label Text="" CssClass="label-danger" ID="lblMensaje" runat="server" />
                </strong>
                <asp:UpdatePanel runat="server">
                    <contenttemplate>
                        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                            <columns>
                                <asp:TemplateField HeaderText="Código barras" ItemStyle-HorizontalAlign="Center">
                                    <itemtemplate><%#Eval("cod_barras") %></itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Código interno" ItemStyle-HorizontalAlign="Center">
                                    <itemtemplate><%#Eval("cod_interno") %></itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <itemtemplate><%#Eval("descripcion_larga") %></itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="U.M." ItemStyle-HorizontalAlign="Center">
                                    <itemtemplate><%#Eval("unidad") %></itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Proveedor">
                                    <itemtemplate><%#Eval("proveedor") %></itemtemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo artículo" ItemStyle-HorizontalAlign="Center">
                                    <itemtemplate><%#Eval("tipo_articulo") %></itemtemplate>
                                </asp:TemplateField>
                            </columns>
                        </asp:GridView>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="treeViewArticulos" EventName="SelectedNodeChanged" />
                    </triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel-footer">
            <asp:Label Text="" ID="label" runat="server" />
            <asp:ImageButton ImageUrl="~/Images/Excel.png" CssClass="btn btn-default" ToolTip="Exportar a excel" runat="server" ID="btnExportarExcel" OnClick="btnExportarExcel_Click" />
        </div>
    </div>
    <br />
</asp:Content>

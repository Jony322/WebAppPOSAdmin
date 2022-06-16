<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmRelacionOfertas.aspx.cs" Inherits="WebAppPOSAdmin.FrmArticulo.frmRelacionOfertas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Relación Ofertas</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-md-3">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:TreeView runat="server" ID="treeViewOfertas" ExpandDepth="1" PopulateNodesFromClient="False" ShowLines="True" OnSelectedNodeChanged="treeViewOfertas_SelectedNodeChanged" OnTreeNodePopulate="treeViewOfertas_TreeNodePopulate">
                        </asp:TreeView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-md-9">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                         <asp:GridView ID="gvOfertas" runat="server" AutoGenerateColumns="False" OnRowCommand="gvOfertas_RowCommand" CssClass="table table-striped" >
                             <Columns>
                                 <asp:BoundField DataField="cod_barras" HeaderText="Cod. Barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                 <asp:BoundField DataField="descripcion_larga" HeaderText="Descripcion" ReadOnly="True" SortExpression="descripcion_larga" />
                                 <asp:BoundField DataField="precio_oferta" HeaderText="Precio Oferta" ReadOnly="True" SortExpression="precio_oferta" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F2}"/>
                                 <asp:BoundField DataField="fecha_emision" HeaderText="Fecha de emisión" ReadOnly="True" SortExpression="fecha_emision" ItemStyle-HorizontalAlign="Center" />
                                 <asp:BoundField DataField="usuario" HeaderText="Responsable" ReadOnly="True" SortExpression="usuario" ItemStyle-HorizontalAlign="Center" />
                                 <asp:TemplateField HeaderText="Cancelar" ItemStyle-HorizontalAlign="Center">
                                     <ItemTemplate>
                                         <asp:ImageButton ImageUrl="~/Images/cancel.png" CssClass="btnEditar" runat="server" ToolTip="Editar" ID="imgEditar" CommandName="Eliminar" CommandArgument='<%#Bind("cod_barras") %>' />
                                     </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" />
                                 </asp:TemplateField>
                             </Columns>
                         </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="treeViewOfertas" EventName="SelectedNodeChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel-footer">
             <asp:ImageButton ImageUrl="~/Images/Excel.png" CssClass="btn btn-default" ToolTip="Exportar a excel" runat="server" ID="btnExportarExcel" OnClick="btnExportarExcel_Click"/>
            <asp:Button Text="Cancelar Oferta" CssClass="btn btn-danger" runat="server" id="btnEliminarOferta" OnClick="btnEliminarOferta_Click"/>
        </div>
    </div>
    <br />
</asp:Content>
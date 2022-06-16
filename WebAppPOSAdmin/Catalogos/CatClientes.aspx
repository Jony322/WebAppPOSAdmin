<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CatClientes.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.CatClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-success quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-pencil"></span>&nbsp&nbsp Administrador de Clientes</h4>
            </div>
        </div>
        <div class="panel-body">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">Cliente</a></li>
                <li role="presentation"><a href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab" onclick="changeTab(2); return false">Buscar</a></li>
            </ul>
            <!--BEGIN TABS -->
            <div class="tab-content col-lg-12">
                <!-- TAB PARA REGISTRAR PROVEEDOR -->
                <div role="tabpanel" class="tab-pane fade in active" id="tab1">
                    <asp:UpdatePanel runat="server">
                        <contenttemplate>
                            <div class="form-group col-lg-2">
                                <asp:Label Text="R.F.C.:" runat="server" />
                                <asp:TextBox runat="server" ID="txtRFC" CssClass="form-control text-center" />
                            </div>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Razón social:" runat="server" />
                                <asp:TextBox runat="server" ID="txtRazonSocial" CssClass="form-control" />
                            </div>
                            <div class="form-group col-lg-3">
                                <asp:Label Text="Correo electrónico:" runat="server" />
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-envelope"></span></span>
                                    <asp:TextBox runat="server" ID="txtCorreo" CssClass="form-control" TextMode="Email" />
                                </div>
                            </div>
                            <div class="form-group col-lg-3">
                                <asp:Label Text="Correo electrónico alterno:" runat="server" />
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-envelope"></span></span>
                                    <asp:TextBox runat="server" ID="txtCorreoAlt" CssClass="form-control" TextMode="Email" />
                                </div>
                            </div>
                            <div class="form-group col-lg-2">
                                <div>&nbsp;</div>
                                <asp:Button Text="Guardar" CssClass="btn btn-success btn-block" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" />
                            </div>
                            <div class="row"></div>
                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
                <!-- TAB PARA REGISTRAR CRÉDITO -->
                <div role="tabpanel" class="tab-pane fade" id="tab2">
                    <div class="row">&nbsp;</div>

                    <asp:UpdatePanel ID="upBuscador" runat="server">
                        <contenttemplate>
                            <div class="form-group col-lg-12">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <asp:RadioButton runat="server" GroupName="findBy" ID="rbtAll" Text="Todos" />
                                        &nbsp;&nbsp;
                                        <asp:RadioButton runat="server" GroupName="findBy" ID="rbtRFC" Text="R.F.C." />
                                        &nbsp;&nbsp;
                                        <asp:RadioButton runat="server" GroupName="findBy" ID="rbtRazonSocial" Text="Razón Social" Checked="true" />
                                    </span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtFind" />
                                    <span class="input-group-btn">
                                        <asp:Button runat="server" ID="btnFind" Text="Buscar" Placeholder="Escribe lo que estás buscando" CssClass="btn btn-lg btn-default" Style="font-size: 14px" OnClick="btnFind_Click" UseSubmitBehavior="False" />
                                    </span>
                                </div>
                            </div>

                            <asp:GridView ID="gvCliente" runat="server" AutoGenerateColumns="False" OnRowCommand="gvCliente_RowCommand" CssClass="table table-striped">
                                <columns>
                                    <asp:BoundField DataField="rfc" HeaderText="RFC" ReadOnly="True" SortExpression="rfc" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="razon_social" HeaderText="Razón social" ReadOnly="True" SortExpression="razon_social" />
                                    <asp:BoundField DataField="contacto" HeaderText="Contacto" ReadOnly="True" SortExpression="contacto" />
                                    <asp:BoundField DataField="e_mail" HeaderText="EMail" ReadOnly="True" SortExpression="e_mail" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                        <itemtemplate>
                                            <asp:ImageButton ImageUrl="~/Images/editar.png" runat="server" ID="ibtUpdate" CommandName="update" CommandArgument='<%#Bind("id") %>' ToolTip="Actualizar cliente" />
                                            &nbsp;
                                            <asp:ImageButton ImageUrl="~/Images/delete.png" runat="server" ID="ibtDelete" CommandName="delete" CommandArgument='<%#Bind("id") %>' ToolTip="Eliminar cliente" OnClientClick="return confirm('Esta seguro de eliminar a este Cliente?')" />
                                            &nbsp;
                                        </itemtemplate>
                                    </asp:TemplateField>
                                </columns>
                            </asp:GridView>

                        </contenttemplate>
                        <triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvCliente" EventName="RowUpdating" />
                            <asp:AsyncPostBackTrigger ControlID="gvCliente" EventName="RowDeleting" />
                        </triggers>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
    </div>
    <br />
</asp:Content>


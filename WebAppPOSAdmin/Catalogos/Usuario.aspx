<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Usuario.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.Usuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-success quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-user"></span>&nbsp&nbsp Administrador de Usuarios</h4>
            </div>
        </div>
        <div class="panel-body">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">Usuarios</a></li>
                <li role="presentation"><a href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab" onclick="changeTab(2); return false">Buscar</a></li>
            </ul>
            <!--BEGIN TABS -->
            <div class="tab-content col-lg-12">
                <!-- TAB PARA REGISTRAR NUEVO ARTÍCULO -->
                <div role="tabpanel" class="tab-pane fade in active" id="tab1">
                    <asp:UpdatePanel runat="server">
                        <contenttemplate>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Nombre:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtNombre" Placeholder="Nombre de la persona" />
                            </div>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Apellido Paterno:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtApaterno" Placeholder="Apellido paterno" />
                            </div>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Apellido Materno:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtAmaterno" Placeholder="Apellido materno" />
                            </div>
                            <div class="form-group col-lg-6">
                                <asp:Label Text="Nombre de usuario:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtUser" Text="" />
                            </div>
                            <div class="form-group col-lg-6">
                                <asp:Label Text="Contraseña:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control" ID="txtPswd" Text="" />
                            </div>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Cambio de precio:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlChangePrice">
                                    <asp:ListItem Value="0">M&iacute;nimo</asp:ListItem>
                                    <asp:ListItem Value="1">M&iacute;n. & M&aacute;x.</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Descuento en línea:" runat="server" />
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDescuentoLinea" />
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                            <div class="form-group col-lg-4">
                                <asp:Label Text="Descuento global:" runat="server" />
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDescuentoGlobal" />
                                    <span class="input-group-addon">%</span>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <asp:RadioButtonList runat="server" ID="rbtPermisos" AutoPostBack="True" OnSelectedIndexChanged="rbtPermisos_SelectedIndexChanged">
                                    <asp:ListItem Text="Administrador" Value="pos_admin" />
                                    <asp:ListItem Text="Cajas" Value="pos_caja" />
                                    <asp:ListItem Text="Colectora" Value="pos_colector" />
                                </asp:RadioButtonList>
                            </div>
                            <div class="row">&nbsp;</div>
                            <div class="form-group col-lg-5">
                                <asp:ListBox runat="server" CssClass="list-group-item-info" ID="lb_permisos" Height="200" Width="415" AutoPostBack="True"></asp:ListBox>
                            </div>
                            <div class="col-lg-2">
                                <asp:LinkButton CssClass="btn btn-default btn-block" runat="server" ID="btnMoverDerecha" OnClick="btnMoverDerecha_Click"><span class="glyphicon glyphicon-step-forward"></span></asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-default btn-block" runat="server" ID="btnMoverTodoDerecha" OnClick="btnMoverTodoDerecha_Click"><span class="glyphicon glyphicon-fast-forward"></span></asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-default btn-block" runat="server" ID="btnMoverIzquierda" OnClick="btnMoverIzquierda_Click"><span class="glyphicon glyphicon-step-backward"></span></asp:LinkButton>
                                <asp:LinkButton CssClass="btn btn-default btn-block" runat="server" ID="btnMoverTodoIzquierda" OnClick="btnMoverTodoIzquierda_Click"><span class="glyphicon glyphicon-fast-backward"></span></asp:LinkButton>
                            </div>
                            <div class="form-group col-lg-5">
                                <asp:ListBox runat="server" CssClass="list-group-item-info" ID="lb_permiso_usuario" Height="200" Width="415" AutoPostBack="True"></asp:ListBox>
                            </div>
                            <div class="form-control col-lg-4">
                                <asp:Button Text="Nuevo usuario" CssClass="btn btn-primary" ID="btnNewUser" runat="server" OnClick="btnNewUser_Click" />
                                &nbsp;
                        <asp:Button Text="Guardar usuario" CssClass="btn btn-success" ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" UseSubmitBehavior="False" />
                            </div>
                        </contenttemplate>
                        <triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnMoverIzquierda" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnMoverTodoIzquierda" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="rbtPermisos" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="btnMoverDerecha" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnMoverTodoDerecha" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="gvUsers" EventName="RowUpdating" />
                            <asp:AsyncPostBackTrigger ControlID="gvUsers" EventName="RowEditing" />
                            <asp:AsyncPostBackTrigger ControlID="gvUsers" EventName="RowDeleting" />
                        </triggers>
                    </asp:UpdatePanel>
                </div>

                <div role="tabpanel" class="tab-pane fade" id="tab2">
                    <asp:UpdatePanel runat="server">
                        <contenttemplate>
                            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvUsers_RowCommand">
                                <columns>
                                    <asp:BoundField DataField="user_name" HeaderText="Nombre usuario" ReadOnly="True" SortExpression="user_name" ItemStyle-HorizontalAlign="center" />
                                    <asp:BoundField DataField="nombre" HeaderText="Nombre personal" ReadOnly="True" SortExpression="nombre" ItemStyle-HorizontalAlign="center" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                        <itemtemplate>
                                            <asp:ImageButton ImageUrl="~/Images/editar.png" runat="server" ID="ibtEdit" CommandName="edit" CommandArgument='<%# Eval("id_empleado") %>' ToolTip="Editar usuario" />
                                        </itemtemplate>
                                    </asp:TemplateField>
                                </columns>
                            </asp:GridView>
                        </contenttemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="panel-footer">&nbsp;</div>
    </div>
</asp:Content>


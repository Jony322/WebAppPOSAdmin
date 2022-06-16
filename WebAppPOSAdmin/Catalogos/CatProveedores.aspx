<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CatProveedores.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.CatProveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-success quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-pencil"></span>&nbsp&nbsp Administrador de Proveedores</h4>
            </div>
        </div>
        <div class="panel-body">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">Proveedor</a></li>
                <li role="presentation"><a href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab" onclick="changeTab(2); return false">Buscar</a></li>
            </ul>
            <!--BEGIN TABS -->
            <div class="tab-content col-lg-12">

                <!-- TAB PARA REGISTRAR PROVEEDOR -->
                <div role="tabpanel" class="tab-pane fade in active" id="tab1">
                    <div class="form-group col-lg-2">
                        <asp:Label Text="R.F.C.:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtRFC" />
                    </div>
                    <div class="form-group col-lg-6">
                        <asp:Label Text="Razón social:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtRazonSocial" />
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Label Text="Nombre de contacto:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtContacto" />
                        </div>
                    </div>
                    <!--DIRECCIÓN-->
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Calle:" runat="server" />
                        <asp:TextBox runat="server" ID="txtCalle" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-1">
                        <asp:Label Text="No int.:" runat="server" />
                        <asp:TextBox runat="server" ID="txtNumInterior" CssClass="form-control text-center" />
                    </div>
                    <div class="form-group col-lg-1">
                        <asp:Label Text="No ext.:" runat="server" />
                        <asp:TextBox runat="server" ID="txtNumExterior" CssClass="form-control text-center" />
                    </div>
                    <div class="form-group col-lg-3">
                        <asp:Label Text="Colonia:" runat="server" />
                        <asp:TextBox runat="server" ID="txtColonia" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Label Text="Localidad/Delegación:" runat="server" />
                        <asp:TextBox runat="server" ID="txtLocalidad" CssClass="form-control" />
                    </div>

                    <div class="form-group col-lg-4">
                        <asp:UpdatePanel runat="server" ID="UpEntidad">
                            <ContentTemplate>
                                <asp:Label Text="Entidad:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlEntidades" AutoPostBack="True" OnSelectedIndexChanged="ddlEntidades_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label Text="Municipio:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlMunicipios"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlEntidades" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="País:" runat="server" />
                        <asp:TextBox runat="server" ID="txtPais" CssClass="form-control" />
                    </div>
                    <div class="form-group col-lg-2">
                        <asp:Label Text="Código postal:" runat="server" />
                        <asp:TextBox runat="server" ID="txtCodPostal" CssClass="form-control text-center" />
                    </div>

                    <div class="form-group col-lg-4">
                        <asp:Label Text="Correo electrónico:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-envelope"></span></span>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtEmail" TextMode="Email" />
                        </div>
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Label Text="Teléfono:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-phone-alt"></span></span>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtTelefono" />
                        </div>
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Label Text="Estatus:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlStatus">
                            <asp:ListItem Value="activo">Activo</asp:ListItem>
                            <asp:ListItem Value="baja">Baja</asp:ListItem>
                            <asp:ListItem Value="moroso">Moroso</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="row"></div>
                    <div class="col-lg-12 ">
                        <asp:Button Text="Guardar/Actualizar" CssClass="btn btn-success " ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" />
                    </div>
                </div>

                <!-- TAB PARA BUSCAR ARTÍCULOS -->
                <div role="tabpanel" class="tab-pane fade" id="tab2">
                    <div class="row">&nbsp;</div>

                    <asp:UpdatePanel ID="upBuscador" runat="server">
                        <ContentTemplate>
                            <div class="form-group col-lg-12">
                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <asp:RadioButton runat="server" GroupName="findBy" ID="rbtAll" Text="Todos" />&nbsp;&nbsp;
                                        <asp:RadioButton runat="server" GroupName="findBy" ID="rbtRFC" Text="R.F.C." />&nbsp;&nbsp;
                                        <asp:RadioButton runat="server" GroupName="findBy" ID="rbtRazonSocial" Text="Razón Social" Checked="true" />
                                    </span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtFind" />
                                    <span class="input-group-btn">
                                        <asp:Button runat="server" ID="btnFind" Text="Buscar" Placeholder="Escribe lo que estás buscando" CssClass="btn btn-lg btn-default" Style="font-size: 14px" OnClick="btnFind_Click" UseSubmitBehavior="False" />
                                    </span>
                                </div>
                            </div>

                            <asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="False" OnRowCommand="gvProveedores_RowCommand" CssClass="table table-striped">
                                <Columns>
                                    <asp:BoundField DataField="rfc" HeaderText="RFC" ReadOnly="True" SortExpression="rfc" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="razon_social" HeaderText="Razón social" ReadOnly="True" SortExpression="razon_social" />
                                    <asp:BoundField DataField="nombre_contacto" HeaderText="Contacto" ReadOnly="True" SortExpression="nombre_contacto" />
                                    <asp:BoundField DataField="tel_principal" HeaderText="Teléfono" ReadOnly="True" SortExpression="tel_principal" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="~/Images/editar.png" runat="server" ID="ibtUpdate" CommandName="update" CommandArgument='<%#Bind("id_proveedor") %>' ToolTip="Actualizar proveedor" />&nbsp;
                                            <asp:ImageButton ImageUrl="~/Images/delete.png" runat="server" ID="ibtDelete" CommandName="delete" CommandArgument='<%#Bind("id_proveedor") %>' ToolTip="Eliminar proveedor" OnClientClick="return confirm('Esta seguro de eliminar a este Proveedor?')" />&nbsp;
                                            <asp:ImageButton ImageUrl="~/Images/report.png" runat="server" ID="ibtReport" CommandName="report" CommandArgument='<%#Bind("id_proveedor") %>' ToolTip="Generar reporte"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvProveedores" EventName="RowUpdating" />
                            <asp:AsyncPostBackTrigger ControlID="gvProveedores" EventName="RowDeleting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
    </div>
    <br />
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CatDepartamentos.aspx.cs" Inherits="WebAppPOSAdmin.Catalogos.CatDepartamentos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Controles/notificaciones.ascx" TagPrefix="uc1" TagName="notificaciones" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Literal ID="ltlCategoryList" runat="server"></asp:Literal>
    <script type="text/javascript">
        $(function () {
            $("#<%= txtBuscador.ClientID %>").autocomplete({
                source: availableCategories
            });

            var indexAccordin = parseInt("<%= lblMensaje.Text %>");

            if (indexAccordin > 0) deployAccordionDptos(indexAccordin);


        });
    </script>
    <uc1:notificaciones runat="server" ID="notificaciones" />
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list"></span>&nbsp&nbsp Clasificador de artículos (Departamentos, categorías, sub-categorías y líneas).</h4>
            </div>
        </div>
        <div class="panel-body">
            <asp:Label runat="server" ID="lblId" />
            <div id="accordion">
                <h3>Buscador</h3>
                <div>
                    <div class="form-group col-md-12">
                        <div class="input-group">
                            <asp:TextBox runat="server" ID="txtBuscador" ValidateRequestMode="Enabled" placeholder="Escribe la clasificación que buscas" name="txtBuscador" class="form-control" ClientIDMode="Static" ValidationGroup="findDepartment" />
                            <span class="input-group-btn">
                                <asp:LinkButton runat="server" CssClass="btn btn-info btn-lg" ID="lbtFindCategory" OnClick="btnBuscar_Click"><span class="glyphicon glyphicon-search"></span></asp:LinkButton>
                            </span>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <strong>
                            <asp:Label Text="0" ID="lblMensaje" runat="server" CssClass="label-info" Visible="false" />
                        </strong>
                    </div>
                    <div class="row"></div>
                </div>

                <h3>Departamentos</h3>
                <div>
                    <div class="form-group col-lg-12">
                        <asp:Label Text="Departamento:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDepartamento" placeholder="Departamento" />
                    </div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="form-group col-lg-2">
                                <asp:Button Text="Guardar Departamento" ID="btnDepartamento" CssClass="btn btn-success" runat="server" OnClick="btnDepartamento_Click" UseSubmitBehavior="False" />
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Button Text="Actualizar Departamento" ID="btnEditarDepartamento" CssClass="btn btn-info" runat="server" OnClick="btnEditarDepartamento_Click" UseSubmitBehavior="False" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row"></div>
                </div>
                <h3>Categorías</h3>
                <div>
                    <div class="form-group">
                        <asp:Label Text="Departamento:" runat="server" />
                        <asp:UpdatePanel runat="server" ID="pUpdateDepartamento">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDepartamento"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnDepartamento" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group ">
                        <asp:Label Text="Categoría:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" Placeholder="Categoría" ID="txtCategoria" />
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Button Text="Guardar" CssClass="btn btn-success btn-block" ID="btnCategoria" runat="server" OnClick="btnCategoria_Click" UseSubmitBehavior="False" />
                    </div>
                    <div class="form-group col-lg-4">
                        <asp:Button Text="Actualizar" CssClass="btn btn-info btn-block" ID="btnEditarCategoria" runat="server" OnClick="btnEditarCategoria_Click" UseSubmitBehavior="False" />
                    </div>
                    <div class="row"></div>
                </div>
                <h3>Sub Categorías</h3>
                <div>
                    <div class="form-group ">
                        <asp:UpdatePanel runat="server" ID="pUpdateCategoria_ddlDepartamento">
                            <ContentTemplate>
                                <asp:Label Text="Departamento:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDepartamento_SubCategoria" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartamento_SubCategoria_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnDepartamento" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group ">
                        <asp:Label Text="Categoría" runat="server" />
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCategoria_SubCategoria"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlDepartamento_SubCategoria" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group ">
                        <asp:Label Text=" Sub-Categoría:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" Placeholder="Sub-Categoria" ID="txtSubcategoria" />
                    </div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="form-group col-lg-2">
                                <asp:Button Text="Guardar Departamento" ID="btnSubcategoria" CssClass="btn btn-success" runat="server" OnClick="btnSubcategoria_Click" UseSubmitBehavior="False" />
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Button Text="Actualizar" CssClass="btn btn-info btn-block" ID="btnEditarSubcategoria" runat="server" OnClick="btnEditarSubcategoria_Click" UseSubmitBehavior="False" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row"></div>
                </div>
                <h3>Líneas</h3>
                <div>
                    <div class="form-group ">
                        <asp:UpdatePanel runat="server" ID="pUpdateLineas_ddlDepartamento">
                            <ContentTemplate>
                                <asp:Label Text="Departamento:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDepartamento_Linea" OnSelectedIndexChanged="ddlDepartamento_Linea_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnDepartamento" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group ">
                        <asp:UpdatePanel runat="server" ID="uPanelCategoria_linea">
                            <ContentTemplate>
                                <asp:Label Text="Categoría" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCategoria_Linea" AutoPostBack="True" OnSelectedIndexChanged="ddlCategoria_Linea_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlDepartamento_Linea" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group ">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label Text="Sub-Categoría" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSubcategoria_linea"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCategoria_Linea" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                    <div class="form-group ">
                        <asp:Label Text="Línea:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control" Placeholder="" ID="txtLinea" />
                    </div>
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="form-group col-lg-2">
                                <asp:Button Text="Guardar Departamento" ID="btnLinea" CssClass="btn btn-success" runat="server" OnClick="btnLinea_Click" UseSubmitBehavior="False" />
                            </div>
                            <div class="form-group col-lg-2">
                                <asp:Button Text="Actualizar" CssClass="btn btn-info btn-block" ID="btnEditarLinea" runat="server" OnClick="btnEditarLinea_Click" UseSubmitBehavior="False" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row"></div>
                </div>

            </div>
        </div>
        <div class="panel-footer">
            <asp:Button Text="Generar Reporte" CssClass="btn btn-primary" ID="btnReporte" runat="server" OnClick="btnReporte_Click" UseSubmitBehavior="False" />
        </div>
    </div>
    <br />
</asp:Content>


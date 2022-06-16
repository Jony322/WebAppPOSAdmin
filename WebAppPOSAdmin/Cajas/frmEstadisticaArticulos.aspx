<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmEstadisticaArticulos.aspx.cs" Inherits="WebAppPOSAdmin.Cajas.frmEstadisticaArticulos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Estadística de Artículos</h4>
            </div>
        </div>
        <div class="panel-body" style="min-height: 410px">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab1" aria-controls="tab1" role="tab" data-toggle="tab" onclick="changeTab(1); return false">Acumulada</a></li>
                <li role="presentation"><a href="#tab2" aria-controls="tab2" role="tab" data-toggle="tab" onclick="changeTab(2); return false">Mensual</a></li>
                <li role="presentation"><a href="#tab3" aria-controls="tab3" role="tab" data-toggle="tab" onclick="changeTab(3); return false">Diaria</a></li>
            </ul>
            <!--BEGIN TABS -->
            <div class="tab-content col-lg-12" style="min-height: 250px">
                <!-- TAB - Estadistica Acumulada -->
                <div role="tabpanel" class="tab-pane fade in active" id="tab1">
                    <div class="col-lg-12">
                        <asp:Label Text="Proveedor:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlProveedores" />
                    </div>
                    <div class="col-lg-3">
                        <asp:Label Text="Departamento:" runat="server" />
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlDepartamento" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged" />
                    </div>
                    <div class="col-lg-3">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label Text="Categoría:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCategoria" OnSelectedIndexChanged="ddlCategoria_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Text="--SELECCIONAR--" />
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlDepartamento" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-lg-3">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label Text="Sub-Categoría:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlSubCategoria" OnSelectedIndexChanged="ddlSubCategoria_SelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Text="--SELECCIONAR--" />
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCategoria" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-lg-3">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label Text="Línea:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlLinea">
                                    <asp:ListItem Text="--SELECCIONAR--" />
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlSubCategoria" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="row"></div>
                    <div class="col-lg-3">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label Text="Fecha inicial:" runat="server" />
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFechaIni" placeholder="dd/mm/aaaa" />
                                    <span class="input-group-addon">
                                        <asp:Image ID="imgCalendarIni" ImageUrl="~/Images/calendar.png" runat="server" />
                                    </span>
                                </div>
                                <atk:CalendarExtender ID="calFecha_ini" runat="server" PopupButtonID="imgCalendarIni" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" ControlToValidate="txtFechaIni" ErrorMessage="Fecha Requerida" ForeColor="Red" ValidationGroup="enventoVer"></asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-lg-3">
                        <asp:Label Text="Hora Inicial" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtHoraIni" Text="07:00:00" />
                        <asp:RequiredFieldValidator ID="rfvHoraIni" runat="server" ErrorMessage="*" ControlToValidate="txtHoraIni" ForeColor="Red"></asp:RequiredFieldValidator>
                        <atk:MaskedEditExtender ID="medHoraIni" AcceptAMPM="False" TargetControlID="txtHoraIni" runat="server" MaskType="Time" Mask="99:99:99" />
                    </div>
                    <div class="col-lg-3">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:Label Text="Fecha final:" runat="server" />
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFechaFin" placeholder="dd/mm/aaaa" />
                                    <span class="input-group-addon">
                                        <asp:Image ID="imgCalendarFin" ImageUrl="~/Images/calendar.png" runat="server" />
                                    </span>
                                </div>
                                <atk:CalendarExtender ID="calFecha_fin" runat="server" PopupButtonID="imgCalendarFin" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" ErrorMessage="Fecha Requerida" ForeColor="Red" ValidationGroup="enventoVer"></asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-lg-3">
                        <asp:Label Text="Hora Final" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtHoraFin" Text="22:00:00" />
                        <asp:RequiredFieldValidator ID="rfvHoraFin" runat="server" ErrorMessage="*" ControlToValidate="txtHoraFin" ForeColor="Red"></asp:RequiredFieldValidator>
                        <atk:MaskedEditExtender ID="medHoraFin" runat="server" AcceptAMPM="False" TargetControlID="txtHoraFin" MaskType="Time" Mask="99:99:99" />
                    </div>

                    <div class="col-lg-3">
                        <asp:Label Text="Código de barras:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtCodBarras"></asp:TextBox>
                    </div>
                    <div class="col-lg-3">
                        <asp:Label Text="Filtrar por descripción:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFiltrarDescripcion"></asp:TextBox>
                    </div>
                    <div class="col-lg-2">
                        <asp:Label Text="Mostrar:" runat="server" />
                        <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtTop" Text="0" TextMode="Number"></asp:TextBox>
                    </div>
                    <div class="col-lg-2">
                        <asp:Label Text="Ordenar por:" runat="server" />
                        <asp:RadioButtonList ID="OrderBy" runat="server">
                            <asp:ListItem Value="cantidad" Text="Cantidad" Selected="True" />
                            <asp:ListItem Value="total" Text="Total" />
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-lg-2">
                        <div>&nbsp;</div>
                        <asp:Button runat="server" CssClass="form-control btn btn-primary" ID="btnShowStaticAcum" OnClick="btnShowStaticAcum_Click" UseSubmitBehavior="False" Text="Consultar"></asp:Button>
                    </div>
                </div>
                <div role="tabpanel" class="tab-pane fade" id="tab2">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="col-lg-3">
                                <asp:Label Text="Código de barras:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtBarCodeMes"></asp:TextBox>
                            </div>
                            <div class="col-lg-3">
                                <asp:Label Text="Mes:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlMes">
                                    <asp:ListItem Value="1" Text="Enero" />
                                    <asp:ListItem Value="2" Text="Febrero" />
                                    <asp:ListItem Value="3" Text="Marzo" />
                                    <asp:ListItem Value="4" Text="Abril" />
                                    <asp:ListItem Value="5" Text="Mayo" />
                                    <asp:ListItem Value="6" Text="Junio" />
                                    <asp:ListItem Value="7" Text="Julio" />
                                    <asp:ListItem Value="8" Text="Agosto" />
                                    <asp:ListItem Value="9" Text="Septiembre" />
                                    <asp:ListItem Value="10" Text="Octubre" />
                                    <asp:ListItem Value="11" Text="Noviembre" />
                                    <asp:ListItem Value="12" Text="Diciembre" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-3">
                                <asp:Label Text="Año:" runat="server" />
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlAnio" />
                            </div>
                            <div class="col-lg-3">
                                <div>&nbsp;</div>
                                <asp:Button runat="server" CssClass="form-control btn btn-primary" ID="btnEstadisticaMes" UseSubmitBehavior="False" Text="Consultar" OnClick="btnEstadisticaMes_Click"></asp:Button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div role="tabpanel" class="tab-pane fade" id="tab3">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="col-lg-3">
                                <asp:Label Text="Fecha inicial:" runat="server" />
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFechaIni2" placeholder="dd/mm/aaaa" />
                                    <span class="input-group-addon">
                                        <asp:Image ID="imgCalendarIni2" ImageUrl="~/Images/calendar.png" runat="server" />
                                    </span>
                                </div>
                                <atk:CalendarExtender ID="calFecha_ini2" runat="server" PopupButtonID="imgCalendarIni2" TargetControlID="txtFechaIni2" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="rfvFechaIni2" runat="server" ControlToValidate="txtFechaIni2" ErrorMessage="Fecha Requerida" ForeColor="Red" ValidationGroup="enventoVer"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-3">
                                <asp:Label Text="Hora Inicial" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtHoraIni2" Text="07:00:00" />
                                <asp:RequiredFieldValidator ID="rfvHoraIni2" runat="server" ErrorMessage="*" ControlToValidate="txtHoraIni2" ForeColor="Red"></asp:RequiredFieldValidator>
                                <atk:MaskedEditExtender ID="medHoraIni2" AcceptAMPM="False" TargetControlID="txtHoraIni2" runat="server" MaskType="Time" Mask="99:99:99" />
                            </div>
                            <div class="col-lg-3">
                                <asp:Label Text="Fecha final:" runat="server" />
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFechaFin2" placeholder="dd/mm/aaaa" />
                                    <span class="input-group-addon">
                                        <asp:Image ID="imgCalendarFin2" ImageUrl="~/Images/calendar.png" runat="server" />
                                    </span>
                                </div>
                                <atk:CalendarExtender ID="calFecha_fin2" runat="server" PopupButtonID="imgCalendarFin2" TargetControlID="txtFechaFin2" Format="dd/MM/yyyy" />
                                <asp:RequiredFieldValidator ID="rfvFechaFin2" runat="server" ControlToValidate="txtFechaFin2" ErrorMessage="Fecha Requerida" ForeColor="Red" ValidationGroup="enventoVer"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-3">
                                <asp:Label Text="Hora Final" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtHoraFin2" Text="22:00:00" />
                                <asp:RequiredFieldValidator ID="rfvHoraFin2" runat="server" ErrorMessage="*" ControlToValidate="txtHoraFin2" ForeColor="Red"></asp:RequiredFieldValidator>
                                <atk:MaskedEditExtender ID="medHoraFin2" runat="server" AcceptAMPM="False" TargetControlID="txtHoraFin2" MaskType="Time" Mask="99:99:99" />
                            </div>
                            <div class="col-lg-3">
                                <asp:Label Text="Código de barras:" runat="server" />
                                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtBarCode2"></asp:TextBox>
                            </div>
                            <div class="col-lg-3">
                                <div>&nbsp;</div>
                                <asp:Button runat="server" CssClass="form-control btn btn-primary" ID="btnEstadisticaDiaria" UseSubmitBehavior="False" Text="Consultar" OnClick="btnEstadisticaDiaria_Click"></asp:Button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="row">&nbsp;</div>
            <div class="col-lg-8 col-lg-offset-2">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvEstadistica" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="cod_barras" HeaderText="Código de barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                <asp:BoundField DataField="medida" HeaderText="Unidad" ReadOnly="True" SortExpression="medida" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" ReadOnly="True" SortExpression="cantidad" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                <asp:BoundField DataField="total" HeaderText="Total" ReadOnly="True" SortExpression="total" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F2}" />
                            </Columns>
                        </asp:GridView>
                        <asp:GridView ID="gvEstadisticaDiaria" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="cod_barras" HeaderText="Código de barras" ReadOnly="True" SortExpression="cod_barras" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" ReadOnly="True" SortExpression="descripcion" />
                                <asp:BoundField DataField="medida" HeaderText="Unidad" ReadOnly="True" SortExpression="medida" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="True" SortExpression="fecha" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" ReadOnly="True" SortExpression="cantidad" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:G9}" />
                                <asp:BoundField DataField="total" HeaderText="Total" ReadOnly="True" SortExpression="total" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:F2}" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="panel-footer">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnPrintReport" UseSubmitBehavior="False" Text="Imprimir Reporte" Enabled="False" OnClick="btnPrintReport_Click"></asp:Button>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <br />
</asp:Content>

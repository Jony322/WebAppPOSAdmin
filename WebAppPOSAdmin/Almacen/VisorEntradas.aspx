<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VisorEntradas.aspx.cs" Inherits="WebAppPOSAdmin.Almacen.VisorEntradas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Visor Entrada</h4>
            </div>
        </div>
        <div class="panel-body" style="min-height: 280px">
            <asp:Panel runat="server" DefaultButton="btnVer">
                <div class="form-group col-lg-2" >
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Label Text="Del...:" runat="server" />
                            <div class="input-group">
                                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFechaIni" placeholder="dd/mm/aaaa" />
                                <span class="input-group-addon">
                                    <asp:Image ID="imgCalendarIni" ImageUrl="~/Images/calendar.png" runat="server" />
                                </span>
                            </div>
                            <cc1:CalendarExtender ID="calFecha_ini" runat="server" PopupButtonID="imgCalendarIni" TargetControlID="txtFechaIni" Format="dd/MM/yyyy" />
                            <asp:RequiredFieldValidator ID="rfvFechaIni" runat="server" ControlToValidate="txtFechaIni" ErrorMessage="Fecha Requerida" ForeColor="Red" ValidationGroup="enventoVer"></asp:RequiredFieldValidator>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form-group col-lg-2">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Label Text="Hasta...:" runat="server" />
                            <div class="input-group">
                                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtFechaFin" placeholder="dd/mm/aaaa" />
                                <span class="input-group-addon">
                                    <asp:Image ID="imgCalendarFin" ImageUrl="~/Images/calendar.png" runat="server" />
                                </span>
                            </div>
                            <cc1:CalendarExtender ID="calFecha_fin" runat="server" PopupButtonID="imgCalendarFin" TargetControlID="txtFechaFin" Format="dd/MM/yyyy" />
                            <asp:RequiredFieldValidator ID="rfvFechaFin" runat="server" ControlToValidate="txtFechaFin" ErrorMessage="Fecha Requerida" ForeColor="Red" ValidationGroup="enventoVer"></asp:RequiredFieldValidator>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form-group col-lg-4">
                    <asp:Label Text="Observaciones:" runat="server" />
                    <asp:TextBox runat="server" PlaceHolder="Observaciones..." CssClass="form-control" ID="txtObservaciones" />
                </div>
                <div class="form-group col-lg-2">
                    <asp:Label Text="Cod. Barras:" runat="server" />
                    <asp:TextBox runat="server" PlaceHolder="Entrada" CssClass="form-control" ID="txtCodBarras" />
                </div>
                <div class="form-group col-lg-2">
                    <div>&nbsp;</div>
                    <asp:Button Text="Ver" CssClass="form-control btn btn-block btn-primary" ID="btnVer" runat="server" OnClick="btnVer_Click" ValidationGroup="enventoVer" />
                </div>
            </asp:Panel>
            <div class="form-group">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvEntrada" runat="server" AutoGenerateColumns="False" OnRowCommand="gvEntrada_RowCommand" CssClass="table table-striped">
                            <Columns>
                                <asp:BoundField DataField="num_entrada" HeaderText="Entrada" ReadOnly="True" SortExpression="id_entrada" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="fecha_entrada" HeaderText="Fecha Entrada" ReadOnly="True" SortExpression="fecha_entrada" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="observacion" HeaderText="Observación" ReadOnly="True" SortExpression="observacion" />
                                <asp:BoundField DataField="user_name" HeaderText="Responsable" ReadOnly="True" SortExpression="user_name" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Reporte" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgReporte" runat="server" CommandArgument='<%#Bind("id_entrada") %>' CommandName="Reporte" ToolTip="Imprimir reporte" ImageUrl="~/Images/report.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvEntrada" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="btnVer" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <br />
</asp:Content>

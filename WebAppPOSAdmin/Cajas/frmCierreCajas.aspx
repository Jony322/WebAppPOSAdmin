<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCierreCajas.aspx.cs" Inherits="WebAppPOSAdmin.Cajas.frmCierreCajas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="atk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-primary quitar-margin">
        <div class="panel-heading">
            <div class="panel-title">
                <h4><span class="glyphicon glyphicon-list-alt"></span>&nbsp&nbsp Cierre Cajas</h4>
            </div>
        </div>
        <div class="panel-body">
            <div class="col-lg-2 col-lg-offset-2">
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
            <div class="col-lg-2">
                <asp:Label Text="Hora Inicial" runat="server" />
                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtHoraIni" Text="07:00:00" />
                <asp:RequiredFieldValidator ID="rfvHoraIni" runat="server" ErrorMessage="*" ControlToValidate="txtHoraIni" ForeColor="Red"></asp:RequiredFieldValidator>
                <atk:maskededitextender ID="medHoraIni" AcceptAMPM="False" TargetControlID="txtHoraIni" runat="server" MaskType="Time" Mask="99:99:99" />
            </div>

            <div class="col-lg-4">
                <asp:Label Text="Cajero" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCajeros" />
            </div>
            <div class="row"></div>
            <div class="col-lg-2 col-lg-offset-2">
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
            <div class="col-lg-2">
                <asp:Label Text="Hora Final" runat="server" />
                <asp:TextBox runat="server" CssClass="form-control text-center" ID="txtHoraFin" Text="22:00:00" />
                <asp:RequiredFieldValidator ID="rfvHoraFin" runat="server" ErrorMessage="*" ControlToValidate="txtHoraFin" ForeColor="Red"></asp:RequiredFieldValidator>
                <atk:maskededitextender ID="medHoraFin" runat="server" AcceptAMPM="False" TargetControlID="txtHoraFin" MaskType="Time" Mask="99:99:99" />
            </div>
            <div class="col-lg-2">
                <asp:Label Text="Caja" runat="server" />
                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCaja" />
            </div>
            <div class="col-lg-2">
                <div>&nbsp;</div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button Text="Ver" CssClass="form-control btn btn-primary" runat="server" ID="btnVer" OnClick="btnVer_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
            <div class="row">&nbsp;</div>
        </div>
        <div class="panel-footer">&nbsp;</div>
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebAppPOSAdmin.Principal.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Acceder al sistema</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-header col-md-8 col-md-offset-2">
            <h2>Su Plaza, Ingresa al sistema</h2>
        </div>
        <div class="col-md-8 col-md-offset-2">
            <div class="panel panel-default">
                <div class="page-header">
                    <div class="panel-title">
                          <h4><span class="glyphicon glyphicon-log-in"></span>&nbsp&nbsp Administrador</h4>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <asp:Label Text="Usuario:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtUsuario" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label Text="Contraseña:" runat="server" />
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></span>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtPassword" TextMode="Password" />
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:Button Text="Acceder" runat="server"  ID="btnAcceder" CssClass="btn btn-success" OnClick="btnAcceder_Click"/>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
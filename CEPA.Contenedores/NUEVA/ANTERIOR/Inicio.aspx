<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="CEPA.CCO.UI.Web.Inicio" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Iniciar Sesión</title>
    <link href="CSS/InicioStyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="wrap-shell">
            <div class="loginCont">
                <div id="brandContent" class="floatLeft">
                    <div class="brandImg">
                        <img src="CSS/Images/Photo0810p.png" style="margin: 0" />
                    </div>
                    <div class="brandText">
                        <h1>
                            CEPA / Acajutla</h1>
                        <p>
                            Ahora puedes cargar tus archivos de importación desde cualquier lugar con tan solo unos click,
                            mediante ingresando tu nombre de usuario y contraseña autorizados por el Puerto de Acajutla.</p>
                    </div>
                </div>
                <div id="signInTD" class="floatLeft">
                    <div class="signInHeader">
                        <img src="CSS/images/CEPA_LOGO.gif" />
                    </div>
                    <div class="signInCont">
                        <asp:TextBox ID="inputTxtandPassw" runat="server" placeholder="Nombre de Usuario"
                            Font-Size="Medium" Height="21px" AutoCompleteType="Disabled" ></asp:TextBox>
                        <asp:TextBox ID="inputTxtandPassw2" runat="server" placeholder="Contraseña" TextMode="Password"
                            Font-Size="Medium" Height="21px" Width="320px"></asp:TextBox>
                        <div class="floatLeft">
                        </div>
                        <div class="LoginField">
                            <asp:Button ID="Button1" runat="server" Text="Iniciar Sesión" 
                                value="Iniciar Sesión" onclick="Button1_Click"/>
                            <br />
                            <br />
                            <asp:Label ID="lblError" runat="server" Text="" Style="color: #FF0000; font-weight: 700;"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div style="height: 50px; clear: both;">
            </div>
            <div id="index-footer">
                <div class="floatLeft">
                    <div style="color: #666;">
                        © 2013 CEPA / Puerto de Acajutla, El Salvador</div>
                </div>
                <div class="floatLeft" style="margin-left: 45px;">
                    <a href="#">Términos y Condiciones de Uso</a>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

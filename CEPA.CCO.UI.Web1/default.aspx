<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CEPA.CCO.UI.Web._default" %>

<!DOCTYPE html>
<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7"> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8"> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9"> <![endif]-->
<!--[if gt IE 8]><!-->
<html class="no-js">
<!--<![endif]-->
<head runat="server">
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">    
    <!--[if IE]><![endif]-->
    <meta name="description" content="">
    <meta name="keywords" content="">
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
   <%-- <link rel="stylesheet" href="css/style.css">--%>

   <%-- <link href="CSS/CEPA_CSS.css" rel="Stylesheet" />
    <link href="CSS/Meny.css" rel="stylesheet" type="text/css" />--%>


    <link href="content/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="content/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <title>Bienvenid@s</title>
    <script src="Scripts/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="Scripts/JScript.js" type="text/javascript"></script>
    <style type="text/css">
        .style2
        {
            font-size: large;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="contenedor">
        <div id="masthead">
            <div class="logo">
                <a href="#">CEPA - Contenedores</a></div>
            <p>
                <asp:LoginName ID="LoginName1" runat="server" 
                    style="font-size: large; font-weight: 700" />
                &nbsp&nbsp
                <asp:LoginStatus ID="LoginStatus1" LogoutPageUrl="~/Inicio.aspx" LogoutAction="RedirectToLoginPage"
                    runat="server" ForeColor="White" LoginText="Iniciar Sesión" 
                    LogoutText="Cerrar Sesión" style="font-size: large" />
            </p>
        </div>
        <div id="page_content">
            <div id="sidebar">
                <div id='cssmenu'>
                       <asp:Menu ID="MenuPrincipal" MaximumDynamicDisplayLevels="20" runat="server" Height="42px"
                        Orientation="Vertical" StaticSubMenuIndent="" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="12px" ForeColor="#1584CE" DynamicVerticalOffset="2">
                        <DynamicHoverStyle BackColor="#1584CE" ForeColor="White" />
                        <DynamicMenuItemStyle HorizontalPadding="10px" VerticalPadding="7px" BackColor="#F7F6F3"
                            ForeColor="#1584CE" />
                        <DynamicMenuStyle BackColor="#F7F6F3" HorizontalPadding="10px" VerticalPadding="7px" />
                        <DynamicSelectedStyle BackColor="#F7F6F3" ForeColor="#1584CE" HorizontalPadding="10px"
                            ItemSpacing="6px" />
                        <StaticHoverStyle BackColor="#1584CE" ForeColor="White" />
                        <StaticMenuItemStyle HorizontalPadding="10px" VerticalPadding="10px" BackColor="#F7F6F3"
                            ForeColor="#1584CE" />
                        <StaticMenuStyle BackColor="#F7F6F3" />
                        <StaticSelectedStyle BackColor="#F7F6F3" />
                    </asp:Menu>
                </div>
            </div>
        </div>
    </div>
    <div id="content" style="height: 580px">
        <div style="float: left; width: 72%">
            <h2>
                Sistema De Carga de Archivos de Importación CEPA / Acajutla</h2>
            <br />
            <h3>
                Bienvenid@
                <asp:Label ID="lblNUser" runat="server"></asp:Label>
            </h3>
            <span style="text-align: justify;">Con este Sistema podrán cargar los archivos de importación,
                enviados por las navieras para cada buque anunciado de esa manera, todas las dependencias
                que se alimenten con está información podrán entrar y acceder a la información.</span>
            <br />
            <br />
            <span style="text-align: justify;">Consiguiendo que el proceso sea más ágil, y obtener
                tiempos de respuestas más efectivos, para la manipulación posterior de está información
                por parte de dichas dependencias</span>
            <br />
            <br />
        </div>
        <div style="float: right; width: 27%">
            <h2>
                <<
            </h2>
            <h3 id="titleTop">
                Buques Anunciados</h3>
            <div>
                <asp:Button ID="Button1" runat="server" Text="Prueba" onclick="Button1_Click" 
                    Visible="false" /><br />
                <asp:LinkButton ID="LinkButton2" runat="server">Ver mas</asp:LinkButton>
            </div>
            <h3 id="titleTop">
                Archivos Cargados</h3>
            <div>
                <asp:LinkButton ID="LinkButton1" runat="server">Ver mas</asp:LinkButton>
            </div>
        </div>
    </div>
    <div id="footer">
        <p>
            © 2012 CEPA / Acajutla - Todos los derechos reservados.</p>
    </div>
    </form>
</body>
</html>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CEPA.CCO.UI.Web._default" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <link href="CSS/CEPA_CSS.css" rel="Stylesheet" />
    <link href="CSS/Meny.css" rel="stylesheet" type="text/css" />
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
                    <ul>
                        <li><a href="/default.aspx"><span>Inicio</span></a></li>
                        <li class='has-sub'><a href='#'><span>Navieras </span></a>
                            <ul>
                                <li><a href="Navieras/wfPrincipalNavi.aspx"><span>Cargar Listado de Importación</span></a></li>                              
                                <li class='last'><a href="Navieras/wfConsultaCancel.aspx"><span>Cancelar Contenedores</span></a></li>                                
                                <li class='last'><a href="Navieras/wfCargarBooking.aspx"><span>Cargar Booking</span></a></li>
                                <li class='last'><a href="Navieras/wfConsultaBooking.aspx"><span>Consultar Booking</span></a></li>
                               <%-- <li class='last'><a href="Navieras/wfDespacho.aspx"><span>Registrar Despacho de Contenedor</span></a></li>                           
                                <li class='last'><a href="Navieras/wfRegistrarSalida.aspx"><span>Registrar Exportación de Contenedor</span></a></li>--%>
                            </ul>
                        </li>
                        <li class='has-sub'><a href='#'><span>Aduana </span></a>
                            <ul>
                                <li><a href="Aduana/wfConsultaBuques.aspx"><span>Autorizar Listado de Importación</span></a></li>                             
                                <%--<li><a href="#"><span>Autorizar Listado de Importación</span></a></li> --%>
                            </ul>
                        </li>
                        <li class='has-sub'><a href='#'><span>DAN</span></a>
                            <ul>
                                <li><a href="DAN/wfPrincipalDAN.aspx"><span>Retener Contenedores</span></a></li>   
                                <li><a href="DAN/wfPrincipalDANL.aspx"><span>Liberar Contenedores</span></a></li>   
                                <%--<li><a href="#"><span>Retener Contenedores</span></a></li>   
                                <li><a href="#"><span>Liberar Contenedores</span></a></li>   --%>                        
                            </ul>
                        </li>
                        <li><a href='#'><span>Acerca De</span></a></li>
                        <li class='last'><a href='#'><span>Contacto</span></a></li>
                    </ul>
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

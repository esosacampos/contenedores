﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CEPA.CCO.UI.Web._default" %>

<!DOCTYPE html>
<html lang="es">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title id="titulo" runat="server">CEPA - Contenedores</title>
    <!-- Bootstrap -->
    <link rel="icon" href="~/CSS/Img/26270.png">
    <link href="~/bootstrap/csss/bootstrap.min.css" rel="stylesheet">
    <link href="~/bootstrap/csss/todc-bootstrap.min.css" rel="stylesheet">
    <link href="~/bootstrap/csss/footable.metro.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/footable.core.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/footable-demos.css" rel="stylesheet" type="text/css" />
    <%--    <link href="~/bootstrap/csss/normalize.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/vertical-responsive-menu.min.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/demo.css" rel="stylesheet" type="text/css" />--%>
    <style type="text/css">
        .
        
        html
        {
            position: relative;
            min-height: 98%;
        }
        body
        {
            /*Margin bottom by footer height*/
            margin-bottom: 50px;
            overflow-x: hidden;
        }
        
        
        .footer
        {
            position: fixed;
            bottom: 0;
            width: 100%;
            height: 60px;
            background-color: #f5f5f5;
            left: 0;
            margin-top: 5%;
        }
        .footer > .container
        {
            padding-right: 15px;
            padding-left: 15px;
        }
        
        
        
        @media screen and (min-width: 768px)
        {
            .h4, h4
            {
                /*margin-top:5%;*/
            }
            
             .pull-left
            {
                /*padding-top:5%;    */
            }
            .row-offcanvas
            {
                position: relative;
                -webkit-transition: all .25s ease-out;
                -moz-transition: all .25s ease-out;
                transition: all .25s ease-out;
            }
        
            .row-offcanvas-right
            {
                right: 25%;
            }
        
            .row-offcanvas-left
            {
                left: 25%;
            }
        
            .row-offcanvas-right .sidebar-offcanvas
            {
                right: -25%; /* 3 columns */
                background-color: rgb(255, 255, 255);
            }
        
            .row-offcanvas-left .sidebar-offcanvas
            {
                left: -25%; /* 3 columns */
                background-color: rgb(255, 255, 255);
            }
        
            .row-offcanvas-right.active
            {
                right: 0; /* 3 columns */
            }
        
            .row-offcanvas-left.active
            {
                left: 0; /* 3 columns */
            }
        
            .row-offcanvas-right.active .sidebar-offcanvas
            {
                background-color: rgb(254, 254, 254);
            }
            .row-offcanvas-left.active .sidebar-offcanvas
            {
                background-color: rgb(254, 254, 254);
            }
        
            .row-offcanvas .content
            {
                width: 75%; /* 9 columns */
                -webkit-transition: all .25s ease-out;
                -moz-transition: all .25s ease-out;
                transition: all .25s ease-out;
            }
        
            .row-offcanvas.active .content
            {
                width: 100%; /* 12 columns */
            }
        
            .sidebar-offcanvas
            {
                position: absolute;
                top: 0;
                width: 25%; /* 3 columns */
            }
        }
        @media screen and (max-width: 767px)
        {
             .h4, h4
            {
                margin-top:4%;
            }
             .pull-left
            {
                padding-top:5%;    
            }
            .row-offcanvas
            {
                position: relative;
                -webkit-transition: all .25s ease-out;
                -moz-transition: all .25s ease-out;
                transition: all .25s ease-out;
            }
        
            .row-offcanvas-right
            {
                right: 0;
            }
        
            .row-offcanvas-left
            {
                left: 0;
            }
        
            .row-offcanvas-right .sidebar-offcanvas
            {
                right: -50%; /* 6 columns */
            }
        
            .row-offcanvas-left .sidebar-offcanvas
            {
                left: -50%; /* 6 columns */
            }
        
            .row-offcanvas-right.active
            {
                right: 50%; /* 6 columns */
            }
        
            .row-offcanvas-left.active
            {
                left: 50%; /* 6 columns */
            }
        
            .sidebar-offcanvas
            {
                position: absolute;
                top: 0;
                width: 50%; /* 6 columns */
            }
        }        
       
    </style>
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
	  <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"/>
	  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"/>
	<![endif]-->
</head>
<body>
    <!--[if lt IE 7]>
			<p class="chromeframe">Usted está utilizando un <strong>navegador</strong> obsoleto. Por favor <a href="http://browsehappy.com/">actualice su navegador</a> o <a href="http://www.google.com/chromeframe/?redirect=true">active Google Chrome Frame</a> para mejorar su experiencia.</p>
		<![endif]-->
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
        EnablePageMethods="true">
        <%--EnablePartialRendering="true" AsyncPostBackTimeout="600"--%>
        <Scripts>
            <asp:ScriptReference Path="~/bootstrap/js/jquery-1.11.2.min.js" />
            <asp:ScriptReference Path="~/bootstrap/js/footable.js" />
            <asp:ScriptReference Path="~/bootstrap/js/footable.filter.js" />
            <asp:ScriptReference Path="~/bootstrap/js/footable.paginate.js" />
            <asp:ScriptReference Path="~/bootstrap/js/bootstrap.min.js" />
            <asp:ScriptReference Path="~/bootstrap/js/bootbox.min.js" />
            <asp:ScriptReference Path="~/bootstrap/js/ie10-viewport.js" />
            <%--<asp:ScriptReference Path="~/bootstrap/js/vertical-responsive-menu.min.js" />--%>
        </Scripts>
    </asp:ScriptManager>
    <nav class="navbar navbar-masthead navbar-default navbar-fixed-top" style="background-color: #1771F8;
        margin-bottom: 25%;">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar"
                    aria-expanded="false" aria-controls="navbar">
                    <span class="sr-only">Toggle navigation</span> <span class="icon-bar"></span><span
                        class="icon-bar"></span><span class="icon-bar"></span>
                </button>
                <%--<div class="col-xs-2">
                        <a href="default.aspx"><img class="img-responsive" src="../CSS/Img/262701.gif" alt="imagen"></a>
                    </div>--%>
                <a class="navbar-brand" href="default.aspx" style="color: white">CEPA - Contenedores</a>
            </div>
            <div id="navbar" class="navbar-collapse collapse">
                <ul class="nav navbar-nav navbar-right">
                    <li class="dropdown"><a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button"
                        aria-expanded="true" style="background-color: LightSkyBlue;"><span class="glyphicon glyphicon-user"
                            style="color: White;"></span>
                        <label runat="server" style="padding-left: 5px; color: White;" id="sessionInput" />
                        <span class="caret" style="color: White;"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="wfConfiguracion.aspx">Configuracion</a></li>
                            <li class="divider"></li>
                            <li><a id="link1" href="javascript:__doPostBack('btnLogOut', '');">Cerrar Sesión</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
            <!--/.nav-collapse -->
        </div>
    </nav>
    <div class="container-fluid">
        <div class="row row-offcanvas row-offcanvas-left" style="margin-top: 5%;">
            <%--  <div class="col-xs-3 col-md-3" style="background-color: #f5f5f5; float: left; height: 100%;
                position: fixed; overflow-y: auto; overflow-x: hidden; padding-left: 0px; padding-right: 0px;">--%>
            <div class="col-xs-6 col-sm-3 sidebar-offcanvas" id="sidebar" role="navigation">
                <div class="well sidebar-nav" id="collapse-nav" style="top: 0; background-color: rgb(255, 255, 255);">
                    <asp:Menu ID="MenuPrincipal" MaximumDynamicDisplayLevels="20" CssClass="nav nav-list"
                        IncludeStyleBlock="false" StaticMenuStyle-CssClass="nav" StaticSelectedStyle-CssClass="active"
                        DynamicMenuStyle-CssClass="dropdown-menu" runat="server" Orientation="Vertical"
                        StaticSubMenuIndent="">
                    </asp:Menu>
                </div>
            </div>
            <div class="col-xs-12 col-sm-9 content">
                <p class="pull-left">
                    <button type="button" class="btn btn-primary btn-xs" data-toggle="offcanvas" id="btnMenu">
                        Menu</button>
                </p>
                <br /><br />
                <h4>
                    Sistema De Carga de Archivos de Importación CEPA / Acajutla</h4>
                <hr>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Bienvenid@s
                    </div>
                    <div class="panel-body">
                        <p>
                            Con este Sistema podrán cargar los archivos de importación, enviados por las navieras
                            para cada buque anunciado de esa manera, todas las dependencias que se alimenten
                            con está información podrán entrar y acceder a la información.</p>
                        <p>
                            Consiguiendo que el proceso sea más ágil, y obtener tiempos de respuestas más efectivos,
                            para la manipulación posterior de está información por parte de dichas dependencias
                        </p>
                    </div>
                    <div class="panel-footer">
                        © 2013 CEPA / Puerto de Acajutla, El Salvador v2.0 para Soporte Técnico <span class="span2 label-primary">
                            <a href="#" style="color: White;">Elsa B. Sosa - Sección Informática elsa.sosa@cepa.gob.sv</a></span>
                    </div>
                </div>
                <br />
                <br />
                <br />
                <br />
                <br />
            </div>
        </div>
    </div>
    <footer class="footer">
        <div class="container">
            <p class="text-muted">
                © 2013 CEPA / Puerto de Acajutla, El Salvador v2.0 para Soporte Técnico <a href="#">
                    Elsa B. Sosa - Sección Informática elsa.sosa@cepa.gob.sv</a>
            </p>
        </div>
    </footer>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>--%>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script type="text/javascript">
        $(function () {



            //to fix collapse mode width issue
            $(".nav li,.nav li a,.nav li ul").removeAttr('style');

            //            //for dropdown menu
            $(".dropdown-menu").parent().removeClass().addClass('dropdown');
            $("#MenuPrincipal .dropdown>a").removeClass().addClass('dropdown-toggle').append('<b class="caret"></b>').attr('data-toggle', 'dropdown');

            //            //remove default click redirect effect           
            $('.dropdown-toggle').attr('onclick', '').off('click');

            $('#btnMenu').click(function () {
                if ($('.sidebar-offcanvas').css('background-color') == 'rgb(255, 255, 255)') {
                    $('.dropdown').attr('tabindex', '-1');
                } else {
                    $('.dropdown').attr('tabindex', '');
                }
                $('.row-offcanvas').toggleClass('active');

            });

        });
    </script>
    <%--  <script src="<%= ResolveUrl("~/bootstrap/js/footable.js") %>" type="text/javascript"></script>
        <script src="<%= ResolveUrl("~/bootstrap/js/footable.filter.js") %>" type="text/javascript"></script>
        <script src="<%= ResolveUrl("~/bootstrap/js/footable.paginate.js") %>" type="text/javascript"></script>
        <script src="<%= ResolveUrl("~/bootstrap/js/bootstrap.min.js") %>" type="text/javascript"></script>
        <script src="<%= ResolveUrl("~/bootstrap/js/ie10-viewport.js") %>" type="text/javascript"></script>--%>
    </form>
</body>
</html>
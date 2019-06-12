﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfTrackingClient.aspx.cs" Inherits="CEPA.CCO.UI.Web.CLIENTES.wfTrackingClient" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title id="titulo" runat="server">CEPA - Contenedores</title>
    <!-- Bootstrap -->
    <link rel="icon" href="~/CSS/Img/26270.png">
    <link href="~/bootstrap/csss/bootstrap.min.css" rel="stylesheet">
    <link href="~/bootstrap/csss/bootstrap-select.css" rel="stylesheet">
    <link href="~/bootstrap/csss/todc-bootstrap.min.css" rel="stylesheet">
    <link href="~/bootstrap/csss/footable.metro.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/footable.core.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/footable-demos.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/bootstrap-datetimepicker.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html {
            position: relative;
            min-height: 98%;
        }

        body {
            /*Margin bottom by footer height*/
            margin-bottom: 50px;
            overflow-x: hidden;
        }


        .footer {
            position: fixed;
            bottom: 0;
            width: 100%;
            height: 60px;
            background-color: #f5f5f5;
            left: 0;
            margin-top: 5%;
        }

            .footer > .container {
                padding-right: 15px;
                padding-left: 15px;
            }

        @media screen and (min-width: 768px) {
            .pull-left {
                /*padding-top:5%;    */
            }

            .row-offcanvas {
                position: relative;
                -webkit-transition: all .25s ease-out;
                -moz-transition: all .25s ease-out;
                transition: all .25s ease-out;
            }

            .row-offcanvas-right {
                right: 25%;
            }

            .row-offcanvas-left {
                left: 25%;
            }

            .row-offcanvas-right .sidebar-offcanvas {
                right: -25%; /* 3 columns */
                background-color: rgb(255, 255, 255);
            }

            .row-offcanvas-left .sidebar-offcanvas {
                left: -25%; /* 3 columns */
                background-color: rgb(255, 255, 255);
            }

            .row-offcanvas-right.active {
                right: 0; /* 3 columns */
            }

            .row-offcanvas-left.active {
                left: 0; /* 3 columns */
            }

            .row-offcanvas-right.active .sidebar-offcanvas {
                background-color: rgb(254, 254, 254);
            }

            .row-offcanvas-left.active .sidebar-offcanvas {
                background-color: rgb(254, 254, 254);
            }

            .row-offcanvas .content {
                width: 75%; /* 9 columns */
                -webkit-transition: all .25s ease-out;
                -moz-transition: all .25s ease-out;
                transition: all .25s ease-out;
            }

            .row-offcanvas.active .content {
                width: 100%; /* 12 columns */
            }

            .sidebar-offcanvas {
                position: absolute;
                top: 0;
                width: 25%; /* 3 columns */
            }
        }

        @media screen and (max-width: 767px) {
            .h4, h4 {
                margin-top: 4%;
            }

            .pull-left {
                padding-top: 5%;
            }

            .row-offcanvas {
                position: relative;
                -webkit-transition: all .25s ease-out;
                -moz-transition: all .25s ease-out;
                transition: all .25s ease-out;
            }

            .row-offcanvas-right {
                right: 0;
            }

            .row-offcanvas-left {
                left: 0;
            }

            .row-offcanvas-right .sidebar-offcanvas {
                right: -50%; /* 6 columns */
            }

            .row-offcanvas-left .sidebar-offcanvas {
                left: -50%; /* 6 columns */
            }

            .row-offcanvas-right.active {
                right: 50%; /* 6 columns */
            }

            .row-offcanvas-left.active {
                left: 50%; /* 6 columns */
            }

            .sidebar-offcanvas {
                position: absolute;
                top: 0;
                width: 50%; /* 6 columns */
            }
        }
    </style>
</head>
<body>
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
                <asp:ScriptReference Path="~/bootstrap/js/moment-with-locales.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/bootstrap-datetimepicker.js" />
                <asp:ScriptReference Path="~/bootstrap/js/bootbox.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/ie10-viewport.js" />
                <asp:ScriptReference Path="~/bootstrap/js/bootstrap-select.min.js" />
            </Scripts>
        </asp:ScriptManager>
        <nav class="navbar navbar-masthead navbar-default navbar-fixed-top" style="background-color: #1771F8;">
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
                    <a class="navbar-brand" href="default.aspx" style="color: white">Tracking Contenedores</a>
                </div>

                <!--/.nav-collapse NAV CERRAR SESION -->
            </div>
        </nav>
        <div class="container-fluid">
            <div class="row row-offcanvas row-offcanvas-left" style="margin-top: 5%;">
                <%--  <div class="col-xs-3 col-md-3" style="background-color: #f5f5f5; float: left; height: 100%;
                position: fixed; overflow-y: auto; overflow-x: hidden; padding-left: 0px; padding-right: 0px;">--%>
                <div class="col-xs-6 col-sm-3 sidebar-offcanvas" id="sidebar" role="navigation">
                    <div class="well sidebar-nav" id="collapse-nav" style="top: 0; background-color: rgb(255, 255, 255);">
                        <%-- MENU --%>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-9 content">
                    <h2>Tracking Contenedores Importacion</h2>
                    <br />
                    <hr />
                    <img runat="server" src="CSS/Img/construccion.jpg" />
                  
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
    </form>
</body>
</html>

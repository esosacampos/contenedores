﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfVencimiento.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfVencimiento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
    <link href="~/bootstrap/csss/autocompleteText.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/Loaders.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .html {
            position: relative;
            min-height: 98%;
        }

        body {
            /*Margin bottom by footer height*/
            margin-bottom: 50px;
            overflow-x: hidden;
            height: 100%;
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

        .carousel {
            padding: 5px;
            max-height: 350px;
        }

        .carousel-indicators li {
            background-color: #1E72BD;
            border: 1px solid #c2c2c2;
        }


        .logo {
            /* background-color: #555299; */
            color: #fff;
            border-bottom: 0 solid transparent;
        }

        .logo {
            /* -webkit-transition: width .3s ease-in-out; */
            -o-transition: width .3s ease-in-out;
            /* transition: width .3s ease-in-out; */
            display: block;
            float: left;
            height: 80px;
            font-size: 20px;
            line-height: 50px;
            text-align: center;
            width: 100%;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
            padding: 0 40px;
            font-weight: 300;
            overflow: hidden;
            /* background-size: 54px 54px; */
            /* background-position: 49px 20px; */
        }



        /* Remove the navbar's default margin-bottom and rounded borders */
        .navbar {
            margin-bottom: 0;
            border-radius: 0;
        }

        /* Set height of the grid so .sidenav can be 100% (adjust as needed) */
        .row.content {
            height: 450px;
        }

        /* Set gray background color and 100% height */
        .sidenav {
            padding-top: 20px;
            background-color: #f1f1f1;
            height: 100%;
            min-height: 1200px;
        }

        #anim {
            font-size: 2em;
            font-family: helvetica, sans-serif;
            font-weight: 800;
            color: dodgerblue;
            text-align: center;
        }



        @-webkit-keyframes slidein {
            from {
                margin-left: 100%;
                width: 300%;
            }

            to {
                margin-left: 0%;
                width: 100%;
            }
        }

        @-webkit-keyframes movimiento-diagonal {
            from {
                left: 0px;
            }

            to {
                left: 100%;
            }
        }

        #anim {
            -webkit-animation-name: slidein;
            -webkit-animation-duration: 20s;
            -webkit-animation-iteration-count: infinite;
            -webkit-animation-direction: alternate; /*para que vuelva a su posicion inicial */
            width: 100%;
            background-color: gray;
            color: #fff;
            position: relative;
            padding: 2px;
        }

        /* Set black background color, white text and some padding */
        footer {
            background-color: #555;
            color: white;
            padding: 15px;
        }

        .dropdown-menu {
            padding: 5px 0;
            margin: -3px 28px 0;
            font-size: 13px;
            background-color: #fff;
            border: 1px solid #ccc;
            border: 1px solid rgba(0,0,0,.2);
            border-radius: 0;
            -webkit-box-shadow: 0 2px 4px rgba(0,0,0,.2);
            box-shadow: 0 2px 4px rgba(0,0,0,.2);
        }

            .dropdown-menu li > a:focus, .dropdown-menu li > a:hover, .dropdown-submenu:focus > a, .dropdown-submenu:hover > a {
                color: #15c;
                background-color: #eee;
                background-image: -webkit-linear-gradient(top,#eee 0,#eee 100%);
                background-image: -o-linear-gradient(top,#eee 0,#eee 100%);
                background-image: -webkit-gradient(linear,left top,left bottom,from(#eee),to(#eee));
                background-image: linear-gradient(to bottom,#eee 0,#eee 100%);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffeeeeee', endColorstr='#ffeeeeee', GradientType=0);
                background-repeat: repeat-x;
            }

        .centrar {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            top: 50%;
            left: 40%;
            /*determinamos una anchura*/
            width: 400px;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: 0px;
            /*determinamos una altura*/
            height: 300px;
            /*indicamos que el margen superior, es la mitad de la altura*/
            margin-top: 0px;
            /*border: 1px solid #808080;*/
            padding: 5px;
        }


        ul#mySetting.dropdown-menu > li > a {
            font-size: 1.3em;
        }

        .nav {
            font-size: 1em;
            font-family: sans-serif;
        }

        ul#mySetting.dropdown-menu > li > a {
            position: relative;
            padding: 3px 65px;
        }


        ul#mySetting.dropdown-menu {
            padding: 5px 0;
            margin: -3px 4px 0;
            font-size: 13px;
            background-color: #fff;
            border: 1px solid #ccc;
            border: 1px solid rgba(0,0,0,.2);
            border-radius: 0;
            -webkit-box-shadow: 0 2px 4px rgba(0,0,0,.2);
            box-shadow: 0 2px 4px rgba(0,0,0,.2);
        }

        .footable {
            /* border-collapse: collapse; */
            max-width: 98%;
            margin-left: 15px;
            margin-bottom: 5%;
            font-family: "Segoe UI", "Open Sans", serif;
            border-right: 0px transparent;
            border-collapse: collapse;
        }

        .footable-last-column {
            border-right: 3px solid #1771f8;
        }

        .footable > thead > tr > th:last-child {
            border-right: 3px solid #1771f8;
        }

        .footable > thead > tr > th {
            font-weight: bold;
        }


        /*todc-bootstrap.min.css:1254*/
        /* On small screens, set height to 'auto' for sidenav and grid */
        @media screen and (max-width: 767px) {
            .sidenav {
                height: auto;
                padding: 15px;
            }

            .row.content {
                height: auto;
            }
        }
    </style>
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
            <script src="bootstrap/js/html5shiv.min.js"></script>
            <script src="bootstrap/js/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <!--[if lt IE 7]>
			<p class="chromeframe">Usted está utilizando un <strong>navegador</strong> obsoleto. Por favor <a href="http://browsehappy.com/">actualice su navegador</a> o <a href="http://www.google.com/chromeframe/?redirect=true">active Google Chrome Frame</a> para mejorar su experiencia.</p>
		<![endif]-->
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
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
                <asp:ScriptReference Path="~/bootstrap/js/autocomplete.js" />
                <asp:ScriptReference Path="~/bootstrap/js/jquery.session.js" />

            </Scripts>
        </asp:ToolkitScriptManager>
        <nav class="navbar navbar-inverse">
            <div class="container-fluid" style="background-color: #1771F8; padding-bottom: 10px;">
                <div class="navbar-header" style="margin-top: 3px;">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <button type="button" class="btn btn-primary" style="float: left; margin-top: 5px; color: white; font-size: 1.5em;" data-toggle="offcanvas" id="btnMenu1">
                        <span class="glyphicon glyphicon-menu-hamburger" style="color: white;"></span>
                    </button>
                    <a class="navbar-brand" href="default.aspx" style="color: white; font-weight: bold; font-family: helvetica; font-size: 2.05em; margin-top: 5px; display: inline-block; margin-left: 0; float: left;">CEPA - Contenedores</a>
                </div>
                <div class="collapse navbar-collapse" id="myNavbar">
                    <%--   <ul class="nav navbar-nav navbar-right" style="font-size: large; margin-top: 3px;" id="myConfig">                        
                        <li>
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" id="miSession"
                                aria-expanded="true" style="background-color: LightSkyBlue;"><span class="glyphicon glyphicon-user"
                                    style="color: White;"></span>
                                <label runat="server" style="padding-left: 5px; color: White;" id="sessionInput" />
                                <span class="caret" style="color: White;"></span></a>
                            <ul class="dropdown-menu" role="menu" id="mySetting">
                                <li style="font-size: 1.2em;"><a href="wfConfiguracion.aspx"><span class="glyphicon glyphicon-cog" style="margin-top: 5px;"></span>Configuracion</a></li>
                                <li class="divider"></li>
                                <li style="font-size: 1.2em;"><a id="link1" href="javascript:__doPostBack('btnLogOut', '');"><span class="glyphicon glyphicon-log-out" style="margin-top: 5px;"></span>Cerrar Sesión</a></li>
                            </ul>
                        </li>
                    </ul>--%>
                </div>
            </div>
        </nav>
        <!-- main <%--style="padding-right: 97px;"--%>-->
        <div class="container-fluid text-left">
            <div class="row content">
                <div class="col-sm-2 sidenav navegacion" id="mySideNav">
                    <%--<img class="logo" src="CSS/Imag/cepa.png" alt="CEPA Puerto de Acajutla">--%>
                    <div style="background-color: #f1f1f1; opacity: 0.6; background-image: url(/CSS/Imag/cepa.png); background-repeat: no-repeat; /* background-color: white; */background-size: 100% auto; height: 96px; width: 200px;"></div>
                    <%-- <asp:Menu ID="MenuPrincipal" MaximumDynamicDisplayLevels="20" CssClass="nav nav-list"
                        IncludeStyleBlock="false" StaticMenuStyle-CssClass="nav" StaticSelectedStyle-CssClass="active"
                        DynamicMenuStyle-CssClass="dropdown-menu" runat="server" Orientation="Vertical"
                        StaticSubMenuIndent="">
                    </asp:Menu>--%>
                </div>
                <div class="col-sm-10 text-left principal centrar" style="top: 12px;">
                    <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="true" />
                    <asp:SiteMapPath ID="SiteMapPath1" runat="server" PathSeparator=" > " RenderCurrentNodeAsLink="false">
                    </asp:SiteMapPath>
                    <hr />
                    <h2>Cambio de Contraseña</h2>
                    <hr />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="col-lg-12 centered">
                                <div class="form-group">
                                    <div class="input-group">
                                        <label for="txtOldPass" class="control-label input-lg" style="width: 174.56px">
                                            Contraseña Actual:</label>
                                        <asp:TextBox ID="txtOldPass" runat="server" class="form-control input-lg" autocomplete="off" TextMode="Password"
                                            placeholder="Ingrese contraseña actual"></asp:TextBox>
                                    </div>
                                    <div class="input-group">
                                        <label for="txtNuevaPass" class="control-label input-lg" style="width: 174.56px">
                                            Nueva Contraseña:</label>
                                        <asp:TextBox ID="txtNuevaPass" runat="server" class="form-control input-lg" autocomplete="off" TextMode="Password"
                                            placeholder="Ingrese nueva contraseña"></asp:TextBox>
                                    </div>
                                    <div class="input-group">
                                        <label for="txtConfirmar" class="control-label input-lg">
                                            Confirmar Contraseña:</label>
                                        <asp:TextBox ID="txtConfirmar" runat="server" class="form-control input-lg" autocomplete="off" TextMode="Password"
                                            placeholder="Confirmar contraseña"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <nav>
                                <ul class="pager">
                                    <li class="previous">
                                        <asp:Button ID="btnReg" runat="server" class="btn btn-primary btn-lg" Text="Guardar" />
                                    <li class="next">
                                        <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" OnClick="btnRegresar_Click" Text="<< Regresar" /></li>
                                </ul>
                            </nav>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnReg" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <footer class="footer">
            <div class="container">
                <p class="text-muted">
                    © 2013 CEPA / Puerto de Acajutla, El Salvador v3.0 para Soporte Técnico <a href="#">Elsa B. Sosa - Sección Informática elsa.sosa@cepa.gob.sv</a>
                    <span id="siteseal" style="padding-left: 100px;">
                        <script async type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=M3U1QZ69toEvGnm5vsHcmLOlgtC6rSd11HzwnBCF9eDXbwiU7WtfLrjr6st5"></script>
                    </span>
                    <%--<span id="siteseal1"><script async type="text/javascript" src="https://cdn.ywxi.net/js/1.js"></script></span>--%>
                </p>

            </div>
        </footer>
        <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
        <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>--%>
        <!-- Include all compiled plugins (below), or include individual files as needed -->
        <script type="text/javascript">
            var contar = 0;
            var filtered = false;
            var y = 0;
            var size = 0;

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

                if (navigator.userAgent.match(/IEMobile\/10\.0/)) {
                    var msViewportStyle = document.createElement("style")

                    msViewportStyle.appendChild(
                        document.createTextNode("@-ms-viewport{width:auto!important}")
                    )

                    document.getElementsByTagName("head")[0].appendChild(msViewportStyle)
                }

            });

            $('#btnMenu1').click(function () {
                var s = $('.navegacion').css('background-color');
                if ($('.navegacion').css('background-color') == 'rgb(241, 241, 241)') {
                    $('#mySideNav').css('display', 'none');
                    $('.navegacion').css('background-color', 'rgb(255, 255, 255)');
                    $('#carouselExampleIndicators').css('position', 'fixed');
                    $('.principal').css('width', '98%');
                    //$('#sidebar').css('display', 'none');
                    //$('#btnMenu1').css('margin-left', '0');
                    //$('.container').css('margin-left', '0');

                } else {
                    $('.navegacion').css('background-color', 'rgb(241, 241, 241)');
                    $('.navegacion').css('display', '');
                    $('#carouselExampleIndicators').css('position', '');
                    $('.principal').css('width', '80%');
                    //$('#btnMenu1').css('margin-left', '240px');
                    //$('.container').css('margin-left', '');
                    //$('#sidebar').show();
                }



                //$('.principal').toggleClass('active');

            });

        </script>
        <%--  <script src="<%= ResolveUrl("~/bootstrap/js/footable.js") %>" type="text/javascript"></script>
        <script src="<%= ResolveUrl("~/bootstrap/js/footable.filter.js") %>" type="text/javascript"></script>
        <script src="<%= ResolveUrl("~/bootstrap/js/footable.paginate.js") %>" type="text/javascript"></script>
        <script src="<%= ResolveUrl("~/bootstrap/js/bootstrap.min.js") %>" type="text/javascript"></script>
        <script src="<%= ResolveUrl("~/bootstrap/js/ie10-viewport.js") %>" type="text/javascript"></script>--%>
        <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
        <script type="text/javascript">

            //On UpdatePanel Refresh.
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        SetAutoComplete();
                        SaveValidacion();
                        ClearText();
                    }
                    $.unblockUI();
                });
            };

            prm.add_beginRequest(function OnBeginRequest(sender, args) {
                $.blockUI({
                    message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#424242',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#fff'
                    }
                });
            });

            $(function () {

                SaveValidacion();
            });

            function validatePass(campo) {
                var RegExPattern = /(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$/;
                if ((campo.match(RegExPattern)) && (campo.value != '')) {
                    return true;
                } else {
                    return false;
                }
            }

            function SaveValidacion() {
                $('#<%= btnReg.ClientID %>').click(function (e) {
                    $.blockUI({
                        message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#424242',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#fff'
                    }
                });
                    setTimeout($.unblockUI, 2000);

                    var username = '<%= Session["c_usuario"] %>';
                    var actual = document.getElementById("<%= txtOldPass.ClientID %>").value;
                    var password = document.getElementById("<%= txtNuevaPass.ClientID %>").value;
                    var confirmPassword = document.getElementById("<%= txtConfirmar.ClientID %>").value;

                    //if (validatePass(password)) {
                    //    bootbox.alert("Contraseña debe poseer Entre 6 y 15 caracteres, por lo menos un digito y un alfanumérico, y no puede contener caracteres espaciales");
                    //    return false;
                    //}


                    if (password.length > 0 && confirmPassword.length > 0 && actual.length > 0) {
                        if (password != confirmPassword) {
                            bootbox.alert("Contraseñas no coinciden!!");
                            return false;
                        }
                    }
                    else {
                        bootbox.alert("Ingrese la información requerida");
                        return false;
                    }

                    var params = new Object();
                    params.oldPass = actual;
                    params.newPass = password;
                    params.userName = username;
                    params = JSON.stringify(params);

                    $.ajax({
                        url: '<%=ResolveUrl("/wfVencimiento.aspx/setPassword") %>',
                    data: params,
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var fields = data.d.split("|");

                        if (fields[0] == "0") {

                        }

                        $.unblockUI
                        document.getElementById("<%= txtOldPass.ClientID %>").value = '';
                            document.getElementById("<%= txtNuevaPass.ClientID %>").value = '';
                            document.getElementById("<%= txtConfirmar.ClientID %>").value = '';


                            //bootbox.alert(fields[1]);

                            //setTimeout(function () { bootbox.alert("Inicie sesion nuevamente") }, 3300);

                            bootbox.alert({
                                size: "small",
                                title: "CEPA-Contenedores",
                                message: fields[1] + "<br/>Inicie sesion nuevamente",
                                callback: function () {
                                    $.ajax({
                                        url: '<%=ResolveUrl("/wfVencimiento.aspx/logOut") %>',
                                        success: function (data) {
                                            window.location.href = "/Inicio.aspx";
                                        },
                                        error: function (response) {
                                            bootbox.alert(response.responseText);
                                        },
                                        failure: function (response) {
                                            bootbox.alert(response.responseText);
                                        }
                                    });
                                }
                            })



                    },
                    error: function (response) {
                        bootbox.alert(response.responseText);
                    },
                    failure: function (response) {
                        bootbox.alert(response.responseText);
                    }
                });


                });
            }
        </script>
    </form>
</body>
</html>

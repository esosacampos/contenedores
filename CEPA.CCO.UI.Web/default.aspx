<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CEPA.CCO.UI.Web._default" %>

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
            padding-top: 5px;
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
            -webkit-animation-duration: 6s;
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

        .nav {
            font-size: 1em;
            font-family: sans-serif;
        }

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
                    <ul class="nav navbar-nav navbar-right" style="font-size: large; margin-top: 3px;" id="myConfig">
                        <%--<li><a href="#"><span class="glyphicon glyphicon-log-in"></span>Login</a></li>--%>
                        <li>
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" id="miSession"
                                aria-expanded="true" style="background-color: LightSkyBlue;"><span class="glyphicon glyphicon-user"
                                    style="color: White;"></span>
                                <label runat="server" style="padding-left: 5px; color: White;" id="sessionInput" />
                                <span class="caret" style="color: White;"></span></a>
                            <ul class="dropdown-menu" role="menu" id="mySetting">
                                <li style="font-size: 1.2em;"><a href="../wfConfiguracion.aspx"><span class="glyphicon glyphicon-cog" style="margin-top: 5px;"></span>Configuracion</a></li>
                                <li class="divider"></li>
                                <li style="font-size: 1.2em;"><a id="link1" href="javascript:__doPostBack('btnLogOut', '');"><span class="glyphicon glyphicon-log-out" style="margin-top: 5px;"></span>Cerrar Sesión</a></li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <div class="container-fluid text-left">
            <div class="row content">
                <div class="col-sm-2 sidenav navegacion" id="mySideNav">
                    <%--<img class="logo" src="CSS/Imag/cepa.png" alt="CEPA Puerto de Acajutla">--%>
                    <div style="background-color: #f1f1f1; opacity: 0.75; background-image: url(CSS/Imag/cepa.png); background-repeat: no-repeat; /* background-color: white; */background-size: 100% auto; height: 85px; width: 185px;"></div>
                    <asp:Menu ID="MenuPrincipal" MaximumDynamicDisplayLevels="20" CssClass="nav nav-list"
                        IncludeStyleBlock="false" StaticMenuStyle-CssClass="nav" StaticSelectedStyle-CssClass="active"
                        DynamicMenuStyle-CssClass="dropdown-menu" runat="server" Orientation="Vertical"
                        StaticSubMenuIndent="">
                    </asp:Menu>
                </div>
                <div class="col-sm-10 text-left principal">
                    <h4 id="anim">Plataforma Web de Transferencia de Información CEPA / Puerto de Acajutla</h4>
                    <hr>



                    <div id="carouselExampleIndicators" class="carousel slide" data-ride="carousel">
                        <ol class="carousel-indicators">
                            <li data-target="#carouselExampleIndicators" data-slide-to="0" class="active"></li>
                            <li data-target="#carouselExampleIndicators" data-slide-to="1"></li>
                            <li data-target="#carouselExampleIndicators" data-slide-to="2"></li>
                            <li data-target="#carouselExampleIndicators" data-slide-to="3"></li>
                            <li data-target="#carouselExampleIndicators" data-slide-to="4"></li>
                        </ol>
                        <div class="carousel-inner" role="listbox">
                            <div class="item active">
                                <%--<img class="d-block img-fluid" src="CSS/Imag/001.jpg" alt="First slide">--%>
                                <img class="img-responsive" src="CSS/Imag/explaing.png" alt="First slide" style="margin: 30px -5px 52px 1px; width: 100%;">
                            </div>
                            <div class="item">
                                <%--<img class="d-block img-fluid" src="CSS/Imag/001.jpg" alt="First slide">--%>
                                <img class="img-responsive" src="CSS/Imag/001.jpg" alt="First slide" style="margin: -129px -10px -10px 1px; width: 100%;">
                            </div>
                            <div class="item">
                                <img class="img-responsive" src="CSS/Imag/004.jpg" alt="Third slide" style="margin: -219px -10px -10px -5px; width: 100%;">
                            </div>
                            <div class="item">
                                <img class="img-responsive" src="CSS/Imag/005.jpg" alt="Third slide" style="margin: -152px -36px -17px 1px; height: 100%; width: 100%;">
                            </div>
                            <div class="item">
                                <img class="img-responsive" src="CSS/Imag/006.jpg" alt="Third slide" style="margin: -84px -5px -5px 1px; width: 100%;">
                            </div>
                        </div>
                        <a class="carousel-control-prev" href="#carouselExampleIndicators" role="button" data-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="sr-only">Previous</span>
                        </a>
                        <a class="carousel-control-next" href="#carouselExampleIndicators" role="button" data-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="sr-only">Next</span>
                        </a>
                    </div>
                    <hr />
                </div>
            </div>
        </div>
        <footer class="footer">
            <div class="container">
              <div class="form-inline">
                    <div class="form-group">
                        <%--<div id="DigiCertClickID_0bcATRpn3"></div>--%>
                    </div>
                    <div class="form-group">
                        <p class="text-muted">
                            © 2013 CEPA / Puerto de Acajutla, El Salvador v3.0 para Soporte Técnico <a href="#">Elsa B. Sosa - Sección Informática elsa.sosa@cepa.gob.sv - (+503) 7070 - 8256</a>                           
                        </p>
                    </div>
                </div>
            </div>
        </footer>
        <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
        <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>--%>
        <!-- Include all compiled plugins (below), or include individual files as needed -->
        <script type="text/javascript">
            var __dcid = __dcid || []; __dcid.push(["DigiCertClickID_0bcATRpn3", "10", "s", "white", "0bcATRpn"]); (function () { var cid = document.createElement("script"); cid.async = true; cid.src = "//seal.digicert.com/seals/cascade/seal.min.js"; var s = document.getElementsByTagName("script"); var ls = s[(s.length - 1)]; ls.parentNode.insertBefore(cid, ls.nextSibling); }());
            $(function () {


                $('li[data-toggle="collapse"]').each(function () {
                    if ($(this).next('ul').length == 0) {
                        $(this).removeAttr('data-toggle').removeAttr('data-target').removeClass('collapsed');
                        $(this).find('span.arrow').remove();
                    }
                });
                $('ul#menu-content > ul').on('show.bs.collapse', function (e, obj) {
                    $("ul#menu-content > ul").not(this).removeClass("in");
                    var currentHead = $(this).prev("li");
                    $("ul#menu-content > li").not(currentHead).removeClass("active");
                    $(currentHead).addClass("active");
                })


                //to fix collapse mode width issue
                $(".nav li,.nav li a,.nav li ul").removeAttr('style');

                //            //for dropdown menu
                $(".dropdown-menu").parent().removeClass().addClass('dropdown');
                $("#MenuPrincipal .dropdown>a").removeClass().addClass('dropdown-toggle').append('<b class="caret"></b>').attr('data-toggle', 'dropdown');

                //            //remove default click redirect effect           
                $('.dropdown-toggle').attr('onclick', '').off('click');

                $('#btnMenu').click(function () {
                    if ($('.sidebar-offcanvas').css('background-color') == 'rgb(255, 255, 255)') {
                        //$('#sidebar').attr('tabindex', '-1');
                        $('#sidebar').css('display', 'none');
                    } else {
                        //$('#sidebar').attr('tabindex', '');
                        $('#sidebar').show();
                    }
                    $('.row-offcanvas').toggleClass('active');

                });

                $('#btnMenu1').click(function () {
                    var s = $('.navegacion').css('background-color');
                    if ($('.navegacion').css('background-color') == 'rgb(241, 241, 241)') {
                        $('#mySideNav').css('display', 'none');
                        $('.navegacion').css('background-color', 'rgb(255, 255, 255)');
                        $('#carouselExampleIndicators').css('position', 'fixed');
                        $('#carouselExampleIndicators').css('width', '98%');
                        //$('#sidebar').css('display', 'none');
                        //$('#btnMenu1').css('margin-left', '0');
                        //$('.container').css('margin-left', '0');

                    } else {
                        $('.navegacion').css('background-color', 'rgb(241, 241, 241)');
                        $('.navegacion').css('display', '');
                        $('#carouselExampleIndicators').css('position', '');
                        $('#carouselExampleIndicators').css('width', '100%');
                        //$('#btnMenu1').css('margin-left', '240px');
                        //$('.container').css('margin-left', '');
                        //$('#sidebar').show();
                    }



                    $('.principal').toggleClass('active');

                });

                $('.carousel').carousel({
                    interval: 2500
                })

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

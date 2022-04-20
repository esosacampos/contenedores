<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mfDefault.aspx.cs" Inherits="CEPA.CCO.MobilPatio.mfDefault" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="css/mobilCEPA.css" />
    <link rel="stylesheet" href="css/jquery.mobile.icons.min.css" />
    <link rel="stylesheet" href="css/jquery.mobile.structure-1.4.5.min.css" />
    <title>CEPA-Contenedore Movil</title>
    <style type="text/css">
        .ui-overlay-a, .ui-page-theme-a, .ui-page-theme-a .ui-panel-wrapper {
            background-image: url('images/fondo_principal.png');
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 100% 100%;
            background-attachment: fixed;
            border-color: #378ac8 /*{a-page-border}*/;
            color: #ffffff /*{a-page-color}*/;
            text-shadow: 0 /*{a-page-shadow-x}*/ 1px /*{a-page-shadow-y}*/ 0 /*{a-page-shadow-radius}*/ #444444 /*{a-page-shadow-color}*/;
        }

        .ui-overlay-b, .ui-page-theme-b, .ui-page-theme-b .ui-panel-wrapper {
            background-image: url('images/fond_cepa.png');
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 100% 100%;
            background-attachment: fixed;
            color: black /*{a-page-color}*/;
            text-shadow: 0 /*{a-page-shadow-x}*/ 1px /*{a-page-shadow-y}*/ 0 /*{a-page-shadow-radius}*/ #444444 /*{a-page-shadow-color}*/;
        }

        .ui-header, .ui-footer {
            border: 0px;
        }

        /*.ui-header {
            height: 40px;
            width: 100%;
            z-index: 1;
            position: fixed;
        }

        .ui-footer {
            width: 100%;
            height: 100px;
            position: absolute;
            bottom: 0;
        }*/

        .ui-bar-a, .ui-page-theme-a .ui-bar-inherit {
            background: none;
        }
         [data-role=footer]{bottom:0; position:absolute !important; top: auto !important; width:100%;} 
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div data-role="page" id="page1" data-theme="b">
            <div data-role="header" id="headerA" data-position="fixed">
                <%--<a href="#" class="ui-btn ui-btn-left ui-corner-all ui-shadow ui-icon-home ui-btn-icon-left">Home</a>--%>
             <%--   <h1>Contenedores Movil</h1>--%>
            </div>
            <div data-role="content" id="content">
                <h2 style="margin-top:10%;font-weight:800;color:#313945;">Menu Principal</h2>
                <div data-role="collapsible" data-inset="false">
                    <h4>Importación Contenedores</h4>
                    <ul data-role="listview" data-theme="c">
                        <li><a href="mfRecepcionImport.aspx">Recepción Contenedores</a></li>
                    </ul>
                </div>

                <div data-role="collapsible" data-inset="false">
                    <h4>Exportación Contenedores</h4>
                    <ul data-role="listview" data-theme="c">
                        <li><a href="#">Construccion</a></li>
                           <%-- <li><a href="#">Bob</a></li>--%>
                    </ul>
                </div>
            </div>

            <div data-role="footer" data-position="fixed">
               <%-- <div align="center">
                    <h4>© 2016 CEPA / Puerto de Acajutla, El Salvador v1.0</h4>
                </div>--%>
            </div>
        </div>

        <script src="js/jquery-1.11.1.min.js"></script>
        <script src="bootstrap/js/bootstrap.min.js"></script>
        <script src="bootstrap/js/bootbox.min.js"></script>
        <script>
            $(document).bind("mobileinit", function () {
                $.mobile.ajaxEnabled = false;
            });
        </script>
        <script src="js/jquery.mobile-1.4.5.min.js"></script>
        <script>
            $(function () {
                setTimeout(function () {
                    $(':mobile-pagecontainer').pagecontainer('change', '#page2', {
                        transition: 'flip',
                        changeHash: false,
                        reverse: true,
                        showLoadMsg: true
                    });
                }, 1000);
            });
        </script>

    </form>
</body>
</html>


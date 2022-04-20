<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mfInicio.aspx.cs" Inherits="CEPA.CCO.MobilPatio.mfInicio" %>

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
        <div>
            <div data-role="page" id="page1" data-theme="a">
                <div data-role="header" id="headerA" data-position="fixed">
                </div>
                <div data-role="content" id="content">
                </div>
                <div data-role="footer" data-position="fixed">
                </div>
            </div>
            <div data-role="page" id="page2" data-theme="b">
                <div data-role="header">
                   <%-- <img src="images/cepa_logo_blanco.png" style="width: 100px;height: 30px;left: 13%;top: 5px;" />
                    <h1>Contenedores Movil</h1>--%>
                </div>
                <div data-role="content">
                    <div class="ui-field-contain" style="margin-top: 15%;">
                        <label for="txtCodigo">Codigo de Marcacion</label>
                        <input type="password" name="txtCodigo" data-clear-btn="true" id="txtCodigo">
                    </div>                    

                    <div align="right">
                    <asp:Button ID="btnAcceso" data-inline="true" class="ui-btn" type="submit" runat="server" data-mini="false"
                        Text="Iniciar sesión" OnClick="btnAcceso_Click" /></div>                    
                </div>
                <div data-role="footer">
                    <div align="center">
                    <h4>© 2021 CEPA Movil Patio / Puerto de Acajutla, El Salvador v1.0</h4></div>
                </div>
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

                $('#page1').click(function (event) {
                    setTimeout(function () {
                        $(':mobile-pagecontainer').pagecontainer('change', '#page2', {
                            transition: 'flip',
                            changeHash: false,
                            reverse: true,
                            showLoadMsg: true
                        });
                    }, 1000);
                });
            });
        </script>

    </form>
</body>
</html>

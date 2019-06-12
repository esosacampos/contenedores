﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfInicioE.aspx.cs" Inherits="CEPA.Estilo.wfInicioE" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="icon" href="../CSS/Img/26270.png">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>CEPA - Contenedores</title>
    <!-- Bootstrap -->
    <link href="bootstrap/csss/bootstrap.min.css" rel="stylesheet">
    <link href="bootstrap/csss/bootstrap-theme.min.css" rel="stylesheet">
    <link href="bootstrap/csss/estiloLogin.css" rel="stylesheet" />
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <style type="text/css">
        body {
            min-height: 200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="col-md-12">
                <div class="col-md-4">
                </div>
                <div class="col-md-4" id="login">
                    <div class="form-signin" role="form" style="margin: 10px;">
                        <div class="text-center">
                            <img id="avatar" src="images/nadie.png" alt="avatar">
                        </div>
                        <input id="inputTxtandPassw" type="text" class="form-control" autocomplete="off" placeholder="Código de Marcación" runat="server">
                        <asp:Button ID="Button1" class="btn btn-lg btn-primary btn-block" type="submit" runat="server"
                            Text="Iniciar sesión" OnClick="Button1_Click" />
                    </div>
                </div>
                <div class="col-md-4">
                </div>
            </div>
        </div>
        <div id="nebaris">
            © 2014 CEPA / Puerto de Acajutla, El Salvador v2.0 para Soporte Técnico <a href="#">Elsa B. Sosa - Sección Informática elsa.sosa@cepa.gob.sv</a>
        </div>
        <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
        <script src="bootstrap/js/jquery-1.11.2.min.js"></script>
        <!-- Include all compiled plugins (below), or include individual files as needed -->
        <script src="bootstrap/js/bootstrap.min.js"></script>
        <script src="bootstrap/js/bootbox.min.js"></script>
        <script src="bootstrap/js/jquery.placeholder.js"></script>
        <script type="text/javascript">
            var nua = navigator.userAgent;
            var isAndroid = (nua.indexOf('Mozilla/5.0') > -1 && nua.indexOf('Android ') > -1 && nua.indexOf('AppleWebKit') > -1 && nua.indexOf('Chrome') === -1);
            if (isAndroid) {
                $('select.form-control').removeClass('form-control').css('width', '100%');
            }
        </script>
    </form>
</body>
</html>

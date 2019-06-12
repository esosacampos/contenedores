<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="CEPA.CCO.UI.Web.Inicio" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>CEPA - Contenedores</title>
    <!-- Bootstrap -->
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="bootstrap/css/bootstrap-theme.min.css" rel="stylesheet">
    <link href="bootstrap/css/estiloLogin.css" rel="stylesheet" />
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <div class="col-md-12">
            <div class="col-md-4">
            </div>
            <div class="col-md-4" id="login">
                <div class="form-signin" role="form" style="margin:10px;">
                    <div class="text-center">
                        <img id="avatar" src="../CSS/Img/nadie.png" alt="avatar">    
                    </div>                  
                    <input id="inputTxtandPassw" type="text" class="form-control" placeholder="Nombre de usuario">
                    <input type="password" class="form-control" placeholder="Clave de acceso" id="inputTxtandPassw2">
                    <asp:Button id="Button1" class="btn btn-lg btn-primary btn-block" type="submit" 
                        runat="server" Text="Iniciar sesión" onclick="Button1_Click"/>                        
                </div>
            </div>
            <div class="col-md-4">
            </div>
        </div>
    </div>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="bootstrap/js/bootstrap.min.js"></script>
    </form>
</body>
</html>

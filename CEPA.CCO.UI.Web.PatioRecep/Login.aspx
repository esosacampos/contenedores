<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CEPA.CCO.UI.Web.PatioRecep.Login" %>

<!DOCTYPE html>

<html lang="es-sv" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="content-language" content="es-sv" />
    <title>CEPA - PATIO | Inicio de Sesion</title>

    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css" />
    <!-- icheck bootstrap -->
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="shortcut icon" href="favicon.png" />
</head>
<body class="hold-transition login-page">
    <form id="form1" runat="server">
        <div class="login-box">
            <div class="login-logo" style="font-weight: 400; font-size: 1.8rem; margin-bottom: 0.2rem;">
                <a href="#"><b class="brand-link">
                    <img src="dist/img/cepa.png" alt="CEPA Logo" class="brand-image" style="opacity: .8; margin-top: 20px; width: 6rem; max-height: 50px;" /></b>|CYMApp - Acajutla</a>
            </div>
            <!-- /.login-logo -->
            <div class="card">
                <div class="card-body login-card-body">
                    <p class="login-box-msg brand-link"></p>

                    <div class="input-group mb-3">
                        <input type="password" id="txtCodMarcacion" class="form-control" placeholder="Código de Marcación" maxlength="4" />
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-lock"></span>
                            </div>
                        </div>
                    </div>
                    <div class="input-group mb-3">
                        <label class="form-control" id="lblNombre" placeholder="Nombre de usuario">Nombre de usuario</label>
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-user"></span>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <%-- <div class="icheck-primary">
                                <input type="checkbox" id="remember"/>
                                <label for="remember">
                                    Remember Me
                                </label>
                            </div>--%>
                        </div>
                        <!-- /.col -->
                        <div class="col-6">
                            <button type="submit" class="btn btn-primary btn-block">Iniciar sesión</button>
                        </div>
                        <!-- /.col -->
                    </div>

                </div>
                <!-- /.login-card-body -->
            </div>
        </div>
        <!-- /.login-box -->
    </form>
    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- Bootbox Alert -->
    <script src="plugins/bootstrap/js/bootbox.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#txtCodMarcacion").keyup(function () {
                if ($(this).val().length == 4) {
                    var params = new Object();
                    params.codigo = $("#txtCodMarcacion").val();
                    params = JSON.stringify(params);
                    $.ajax({
                        url: '<%=ResolveUrl("~/Login.aspx/getNombre") %>',
                        data: params,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            var nombre = response.d;
                            if (nombre.indexOf("Error") == -1) {
                                $("#lblNombre").text(nombre.replace(/['"]+/g, ''));
                                console.log($("#lblNombre").text().length);
                                if ($("#lblNombre").text().length > 0) {
                                    var params = new Object();
                                    params.codigo = $("#txtCodMarcacion").val();
                                    params = JSON.stringify(params);
                                    $.ajax({
                                        url: '<%=ResolveUrl("~/Login.aspx/setAccess") %>',
                                        data: params,
                                        dataType: "json",
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        success: function (response) {
                                            var respuesta = response.d.replace(/['"]+/g, '');
                                            if (respuesta == "Aceptado") {
                                                window.location.href = "Default.aspx";
                                            }
                                            else
                                                bootbox.alert("El código utilizado no posee acceso");
                                        },
                                        error: function (data, success, error) {
                                            bootbox.alert(error);
                                        },
                                        failure: function (response) {
                                            bootbox.alert(response.responseText);
                                        }
                                    });
                                }
                            }
                            else
                                bootbox.alert(nombre.replace(/['"]+/g, ''));
                        },
                        error: function (data, success, error) {
                            bootbox.alert(error);
                        },
                        failure: function (response) {
                            bootbox.alert(response.responseText);
                        }
                    });
                }
            });
        });
    </script>
</body>
</html>

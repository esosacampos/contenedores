<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CEPA.CCO.UI.Web.PatioRecep.Default" %>

<!DOCTYPE html>

<html lang="es-sv" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>CEPA-PATIO | Inicio</title>

    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.min.css" />
    <link rel="shortcut icon" href="favicon.png" />
</head>
<body class="hold-transition sidebar-mini">
    <form id="form1" runat="server">
        <div class="wrapper">
            <!-- Navbar -->
            <nav class="main-header navbar navbar-expand navbar-white navbar-light">
                <!-- Left navbar links -->
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
                    </li>
                    <li class="nav-item d-none d-sm-inline-block">
                        <a href="Default.aspx" class="nav-link">Inicio</a>
                    </li>
                    <%--<li class="nav-item d-none d-sm-inline-block">
                        <a href="#" class="nav-link">Contact</a>
                    </li>--%>
                </ul>

                <!-- Right navbar links -->
                <ul class="navbar-nav ml-auto">
                    <!-- Notifications Dropdown Menu -->
                    <li class="nav-item">
                        <a class="nav-link" href="Login.aspx">
                            <i class="far fa-user"></i>
                            Cerrar Sesion
                        </a>
                    </li>
                </ul>
            </nav>
            <!-- /.navbar -->

            <!-- Main Sidebar Container -->
            <aside class="main-sidebar sidebar-dark-primary elevation-4">
                <!-- Brand Logo -->
                <a href="../../index3.html" class="brand-link">
                    <img src="dist/img/cepa_blanco.png" alt="CEPA Logo" class="brand-image" style="opacity: .8; margin-left: 0.2rem; margin-right: 0.2rem;" />
                    <span class="brand-text font-weight-light" style="font-size: 1rem;">| CYMApp - Acajutla</span>
                </a>

                <!-- Sidebar -->
                <div class="sidebar">
                    <!-- Sidebar user (optional) -->
                    <div class="user-panel mt-3 pb-3 mb-3 d-flex">
                        <div class="image">
                            <%--<img src="dist/img/user2-160x160.jpg" class="img-circle elevation-2" alt="User Image"/>--%>
                        </div>
                        <div class="info">
                            <a href="#" class="d-block">
                                <p style="margin-bottom: 0;">Bienvenid@:</p>
                                <p id="tNombre"></p>
                            </a>
                        </div>
                    </div>
                    <!-- Sidebar Menu -->
                    <nav class="mt-2">
                        <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
                            <li class="nav-item">
                                <a href="#" class="nav-link">
                                    <i class="nav-icon fas fa-tachometer-alt"></i>
                                    <p>
                                        Proceso Entradas Patio
                                        <i class="right fas fa-angle-left"></i>
                                    </p>
                                </a>
                                <ul class="nav nav-treeview">
                                    <li class="nav-item">
                                        <a href="Ubicacion.aspx" class="nav-link">
                                            <i class="far fa-circle nav-icon"></i>
                                            <p>Confirmación de Entrada a Patio</p>
                                        </a>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </nav>
                    <!-- /.sidebar-menu -->
                </div>
                <!-- /.sidebar -->
            </aside>

            <!-- Content Wrapper. Contains page content -->
            <div class="content-wrapper">
                <!-- Content Header (Page header) -->
                <section class="content-header">
                    <div class="container-fluid">
                        <div class="row mb-2">
                            <div class="col-sm-6">
                                <h1>CYMApp - Acajutla</h1>
                            </div>
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="Default.aspx">Inicio</a></li>
                                    <li class="breadcrumb-item active">CYMApp - Acajutla</li>
                                </ol>
                            </div>
                        </div>
                    </div>
                    <!-- /.container-fluid -->
                </section>

                <!-- Main content -->
                <section class="content">
                    <div class="container-fluid">
                        <div class="row">
                            <!-- left column -->
                            <div class="col-md-12">
                                <!-- general form elements -->
                                <div class="card card-primary">
                                    <div class="card-header">
                                        <h3 class="card-title">CEPA | CYMApp - Acajutla</h3>
                                    </div>
                                    <!-- /.card-header -->
                                    <!-- form start -->
                                    <div>
                                        <div class="card-body">
                                            <p>CYMApp - Acajutla (Container Yard Management Aplication -> Aplicación de Administración para Patio de Contenedores)  pretende ser una aplicación modular que se especialice en la gestión de los procesos operativos del patio de contenedores. Por el momento será una aplicación que ayude a la ubicación del contenedor en patio.</p>
                                            <p>Esta solución pretende generar agilidad en el posicionamiento de los contendores en tiempo real, será alimentado de la plataforma web, que es donde las navieras proveen la información necesaria para la desestiba de contenedores.</p>
                                        </div>
                                        <!-- /.card-body -->

                                        <div class="card-footer">
                                            
                                        </div>
                                    </div>
                                </div>
                                <!-- /.card -->
                            </div>
                            <!-- /.row -->
                        </div>
                        <!-- /.container-fluid -->
                    </div>
                </section>
                <!-- /.content -->
            </div>
            <!-- /.content-wrapper -->
            <footer class="main-footer">
                <div class="float-right d-none d-sm-block">
                    <b>Version</b> 1.0.0
                </div>
                <strong>Copyright &copy; 2013-2022 <a href="https://www.cepa.gob.sv/">CEPA - Acajutla</a>.</strong> All rights reserved.
            </footer>

            <!-- Control Sidebar -->
            <%--<aside class="control-sidebar control-sidebar-dark">
                <!-- Control sidebar content goes here -->
            </aside>--%>
            <!-- /.control-sidebar -->
        </div>
        <!-- ./wrapper -->
    </form>
    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- bs-custom-file-input -->
    <script src="plugins/bs-custom-file-input/bs-custom-file-input.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js"></script>
    <!-- Page specific script -->
    <script>
        $(function () {
            bsCustomFileInput.init();
            var name = ' <%= Session["NombreC"] %>'
            $('#tNombre').text(name)
        });
    </script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ubicacion.aspx.cs" Inherits="CEPA.CCO.UI.Web.PatioRecep.Ubicacion" %>

<!DOCTYPE html>

<html lang="es-sv" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>CYMApp - Acajutla | Ubicacion Contenedor</title>

    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
    <!-- daterange picker -->
    <link rel="stylesheet" href="plugins/daterangepicker/daterangepicker.css">
    <!-- iCheck for checkboxes and radio inputs -->
    <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
    <!-- Bootstrap Color Picker -->
    <link rel="stylesheet" href="plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css">
    <!-- Tempusdominus Bootstrap 4 -->
    <link rel="stylesheet" href="plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css">
    <!-- Select2 -->
    <link rel="stylesheet" href="plugins/select2/css/select2.min.css">
    <link rel="stylesheet" href="plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css">
    <!-- Bootstrap4 Duallistbox -->
    <link rel="stylesheet" href="plugins/bootstrap4-duallistbox/bootstrap-duallistbox.min.css">
    <!-- BS Stepper -->
    <link rel="stylesheet" href="plugins/bs-stepper/css/bs-stepper.min.css">
    <!-- dropzonejs -->
    <link rel="stylesheet" href="plugins/dropzone/min/dropzone.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.min.css">
    <link rel="stylesheet" href="dist/css/autocompleteText.css" />
    <link rel="shortcut icon" href="favicon.png" />
    <style type="text/css">
        .clsFilas {
            font-weight: 600;
            text-align: right;
        }

        span.error, input.error {
            outline: none;
            border: 1px solid #800000;
            box-shadow: 0 0 5px 1px #800000;
        }

        .error {
            color: #800000;
        }

        .embed-responsive-16by9::before {
            padding-top: 1%;
        }

        .embed-responsive {
            position: relative;
            display: block;
            width: 100%;
            padding: 0;
            overflow: auto;
        }
    </style>
</head>
<body class="hold-transition sidebar-mini">
    <form id="frmUbicacion" runat="server">
        <div class="wrapper">
            <!-- Navbar -->
            <nav class="main-header navbar navbar-expand navbar-white navbar-light">
                <!-- Left navbar links -->
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
                    </li>
                    <li class="nav-item d-sm-inline-block">
                        <a href="Default.aspx" class="nav-link">Inicio</a>
                    </li>
                    <li class="nav-item d-sm-inline-block">
                        <a href="#" id="btnResumen" class="nav-link">Resumen</a>
                    </li>
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
                                            <p>Confirmación de Entrada</p>
                                        </a>
                                    </li>
                                     <li class="nav-item">
                                        <a href="Registros.aspx" class="nav-link">
                                            <i class="far fa-circle nav-icon"></i>
                                            <p>Registros de Entrada</p>
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
                                <h1>Confirmación de Entrada a Patio</h1>
                            </div>
                            <div class="col-sm-6">
                                <ol class="breadcrumb float-sm-right">
                                    <li class="breadcrumb-item"><a href="Default.aspx">Inicio</a></li>
                                    <li class="breadcrumb-item active">Confirmación de Entrada</li>
                                </ol>
                            </div>
                        </div>
                    </div>
                    <!-- /.container-fluid -->
                </section>

                <!-- Main content -->
                <section class="content">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card card-primary">
                                <div class="card-header">
                                    <h3 class="card-title">DATOS GENERALES</h3>
                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse" title="Collapse">
                                            <i class="fas fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="form-group">
                                        <label for="inputName">CONTENEDOR</label>
                                        <input type="search" name="txtConte" id="search" value="" class="form-control" placeholder="Ultimos digitos contenedor">
                                        <table data-role="table" class="table table-striped" id="myTable">
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="card card-green">
                                <div class="card-header">
                                    <h3 class="card-title">UBICACION</h3>
                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse" title="Collapse">
                                            <i class="fas fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="form-group">
                                        <label for="inputStatus">ZONA</label>
                                        <select id="slZona" class="select2" name="slZona" data-dropdown-css-class="select2-primary" style="width: 100%;" data-placeholder="Seleccionar Zona">
                                        </select>
                                    </div>
                                    <div class="row">
                                        <div class="col-4 col-md-4">
                                            <label for="inputClientCompany">CARRIL</label>
                                            <select id="slCarril" name="slCarril" class="select2" data-placeholder="Seleccionar" style="width: 100%;">
                                            </select>
                                        </div>
                                        <div class="col-4 col-md-4">
                                            <label for="inputClientCompany">POSICION</label>
                                            <select id="slPosicion" name="slPosicion" class="select2" data-placeholder="Seleccionar" style="width: 100%;">
                                            </select>
                                        </div>
                                        <div class="col-4 col-md-4">
                                            <label for="inputClientCompany">NIVEL</label>
                                            <select id="slNivel" name="slNivel" class="select2" data-placeholder="Seleccionar" style="width: 100%;">
                                            </select>
                                        </div>
                                    </div>
                                    <div class="form-group" style="margin-top: 1%;">
                                        <label for="slGrua">GRUA</label>
                                        <select id="slGrua" name="slGrua" class="select2" data-dropdown-css-class="select2-primary" style="width: 100%;" data-placeholder="Seleccionar Grua">
                                        </select>
                                    </div>
                                    <div id="Mensaje" class="form-group" style="text-align: center; font-weight: 500; font-size: 1.3em; margin-top: 1%; line-height: 1em; word-break: break-word; opacity: 0.7;">
                                        <br />
                                        <label id="inDesc"></label>
                                    </div>
                                </div>
                                <!-- /.card-body -->
                            </div>
                            <!-- /.card -->
                            <div class="card card-primary">
                                <div class="card-header">
                                    <h3 class="card-title">CONDICION DEL CONTENEDOR</h3>
                                    <div class="card-tools">
                                        <button type="button" class="btn btn-tool" data-card-widget="collapse" title="Collapse">
                                            <i class="fas fa-minus"></i>
                                        </button>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="custom-control custom-checkbox">
                                        <input class="custom-control-input" name="checkedAll" type="checkbox" id="ckSobre">
                                        <label for="ckSobre" class="custom-control-label text-maroon" style="opacity: 0.6;">¿SOBREDIMENSIONADO?</label>
                                    </div>
                                    <br />
                                    <div class="card card-primary card-outline">
                                        <div class="card-header">
                                            <div class="card-title">ABOLLADO</div>
                                        </div>
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input" name="checkedAll" type="checkbox" id="checkedAll">
                                                        <label for="checkedAll" class="custom-control-label text-maroon">TODOS</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingle" name="checkedAll" type="checkbox" id="ck1">
                                                        <label for="ck1" class="custom-control-label">DERECHO</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingle" name="checkedAll" type="checkbox" id="ck2">
                                                        <label for="ck2" class="custom-control-label">IZQUIERDO</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingle" name="checkedAll" type="checkbox" id="ck3">
                                                        <label for="ck3" class="custom-control-label">DELANTERO</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingle" type="checkbox" name="checkedAll" id="ck4">
                                                        <label for="ck4" class="custom-control-label">TRASERO</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingle" type="checkbox" name="checkedAll" id="ck5">
                                                        <label for="ck5" class="custom-control-label">TECHO</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card card-warning card-outline">
                                        <div class="card-header">
                                            <div class="card-title">ROTO</div>
                                        </div>
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input" name="checkedAllR" type="checkbox" id="checkedAllR">
                                                        <label for="checkedAllR" class="custom-control-label text-maroon">TODOS</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingleR" name="checkedAllR" type="checkbox" id="cr1">
                                                        <label for="cr1" class="custom-control-label">DERECHO</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingleR" name="checkedAllR" type="checkbox" id="cr2">
                                                        <label for="cr2" class="custom-control-label">IZQUIERDO</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingleR" name="checkedAllR" type="checkbox" id="cr3">
                                                        <label for="cr3" class="custom-control-label">DELANTERO</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingleR" type="checkbox" name="checkedAllR" id="cr4">
                                                        <label for="cr4" class="custom-control-label">TRASERO</label>
                                                    </div>
                                                </div>
                                                <div class="col-4 col-md-2">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input checkSingleR" type="checkbox" name="checkedAllR" id="cr5">
                                                        <label for="cr5" class="custom-control-label">TECHO</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <label id="lblAbo"></label>
                                    <label id="lblRoto"></label>
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer">
                                    <button type="submit" class="btn btn-success" id="btnGuardar">
                                        <i class="fas fa-save"></i>
                                        <br />
                                        Guardar</button>
                                    <button type="submit" class="btn btn-default float-right" id="btnCancelar">
                                        <i class="fas fa-times-circle"></i>
                                        <br />
                                        Cancelar</button>
                                </div>
                            </div>
                            <!-- /.card -->
                        </div>
                        <!-- row -->
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
        </div>
        <!-- ./wrapper -->
        <div class="modal fade" tabindex="-1" role="dialog" id="myModalEsta" aria-labelledby="myLargeModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header" style="line-height: 10px; display: block;">
                        <button type="button" class="close" id="myCloseD" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 class="modal-title" style="font-weight: 900;">BUQUE: 
                        <label id="buque" style="color: #2F5CC6; font-weight: 800;"></label>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <h4 class="modal-title" style="font-weight: 900;">RESUMEN</h4>
                        <div align="center" class='embed-responsive embed-responsive-16by9'>
                            <table class="table table-striped table-bordered table-hover text-center" id="myTableRe" style="margin-bottom: 5px;font-size: 0.85rem;">
                                <thead>
                                    <tr>
                                        <th>TAMAÑO</th>
                                        <th>MANIFESTADOS</th>
                                        <th>CANCELADOS</th>
                                        <th>TOTAL</th>
                                        <th>RECIBIDOS</th>
                                        <th>PENDIENTES</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal" style="font-weight: 900;">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <!-- jQuery -->
    <script src="plugins/jquery/jquery.min.js"></script>
    <!-- jQuery Migrate -->
    <script src="plugins/jquery/jquery-migrate-3.0.0.js"></script>
    <!-- Bootstrap 4 -->
    <script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- Validacion Formulario -->
    <script src="plugins/jquery-validation/jquery.validate.min.js"></script>
    <!-- Metodos adicionales -->
    <script src="plugins/jquery-validation/additional-methods.min.js"></script>
    <!-- Numeros -->
    <script src="build/js/jquery.number.min.js"></script>
    <!-- Select2 -->
    <script src="plugins/select2/js/select2.full.min.js"></script>
    <!-- Bootbox Alert -->
    <script src="plugins/bootstrap/js/bootbox.min.js"></script>
    <!-- bs-custom-file-input -->
    <script src="plugins/bs-custom-file-input/bs-custom-file-input.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <!-- Autocomplete -->
    <script src="build/js/autocomplete.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="dist/js/demo.js"></script>
    <!-- Page specific script -->
    <script>
        $(function () {
            bsCustomFileInput.init();
            var name = ' <%= Session["NombreC"] %>'
            $('#tNombre').text(name);


            $.validator.setDefaults({
                submitHandler: function () {
                    bootbox.alert("Enviado con éxito!!");
                }
            });

            // Validacion de formulario
            $("#frmUbicacion").validate({
                //onkeyup: false,
                ignore: 'input[type=hidden], .select2-input, .select2-focusser',
                //debug: true,
                rules: {
                    txtConte: {
                        required: true
                    },
                    txtTara: {
                        required: true
                    },
                    slZona: {
                        required: true
                    },
                    slGrua: {
                        required: true
                    },
                    slPosicion: {
                        required: true
                    },
                    slCarril: {
                        required: true
                    },
                    slNivel: {
                        required: true
                    }

                },
                messages: {
                    txtConte: {
                        required: "Por favor ingrese el número de contenedor"
                    },
                    txtTara: {
                        required: "La tara no puede estar en blanco"
                    },
                    slZona: {
                        required: "Por favor indique la zona"
                    },
                    slGrua: {
                        required: "Por favor indique la grúa para la operación"
                    },
                    slPosicion: {
                        required: "Por favor indique la posicion"
                    },
                    slCarril: {
                        required: "Por favor indique el carril"
                    },
                    slNivel: {
                        required: "Por favor indique el nivel"
                    }
                },
                highlight: function (element, errorClass, validClass) {
                    var elem = $(element);
                    if (elem.hasClass("select2-hidden-accessible")) {
                        $("#select2-" + elem.attr("id") + "-container").parent().addClass(errorClass);
                    } else {
                        elem.addClass(errorClass);
                    }
                },
                unhighlight: function (element, errorClass, validClass) {
                    var elem = $(element);
                    if (elem.hasClass("select2-hidden-accessible")) {
                        $("#select2-" + elem.attr("id") + "-container").parent().removeClass(errorClass);
                    } else {
                        elem.removeClass(errorClass);
                    }
                },
                errorPlacement: function (error, element) {
                    var elem = $(element);
                    if (elem.hasClass("select2-hidden-accessible")) {
                        element = $("#select2-" + elem.attr("id") + "-container").parent();
                        error.insertAfter(element);
                    } else {
                        error.insertAfter(element);
                    }
                }
            });


            $('.modal-content').resizable({
                //alsoResize: ".modal-dialog",
                minHeight: 300,
                minWidth: 300
            });
            $('.modal-dialog').draggable();

            $('#myModalEsta').on('modal', function () {
                $(this).find('.modal-body').css({
                    'max-height': '100%',
                    'overflow': 'auto'
                });
            });


            SetAutoComplete();

            //Initialize Select2 Elements
            var $select = $('.select2').select2({
                allowClear: true
            });

            $select.on('change', function () {
                if ($(this).val() != "0")
                    $(this).valid();
                else
                    $(this).valid() = false;
                //$(this).trigger('blur');
            });

            $('.select2').select2().change(function () {
                if ($(this).val() != "0")
                    $(this).valid();
                else
                    $(this).valid() = false;
            });

            $(".select2")
                .select2()
                .on('select2:select', function () {
                    $(this).trigger('blur');
                });

            //todo abollado
            $("#checkedAll").change(function () {
                var langPref = [];
                if (this.checked) {
                    $(".checkSingle").each(function () {
                        this.checked = true;
                        var selectedValue = $(this)[0].id;
                        var text = $('label[for="' + selectedValue + '"]').text();
                        langPref.push(text);
                        $('#lblAbo').text("ABOLLADO DEL LADO: " + langPref.join(", "));
                    })
                } else {
                    $(".checkSingle").each(function () {
                        this.checked = false;
                        langPref = [];
                        $('#lblAbo').text("");
                    })
                }
                var a = $("#lblAbo").text();
                var a = a.length;
                if (a > 0)
                    $("#lblAbo").addClass("alert-success");
                else
                    $("#lblAbo").removeClass("alert-success");
            });

            //individual abollado
            $(".checkSingle").click(function () {
                var langPref = [];
                if ($(this).is(":checked")) {
                    var isAllChecked = 0;
                    $(".checkSingle").each(function () {
                        if (!this.checked) {
                            isAllChecked = 1;
                        }
                        else {
                            var selectedValue = $(this)[0].id;
                            var text = $('label[for="' + selectedValue + '"]').text();
                            langPref.push(text);
                            if (langPref.length > 0) {
                                $('#lblAbo').text("ABOLLADO DEL LADO: " + langPref.join(", "));
                                $("#lblAbo").addClass("alert-success");
                            }
                            else {
                                $('#lblAbo').text("");
                                $("#lblAbo").removeClass("alert-success");
                            }
                        }
                    })
                    if (isAllChecked == 0) { $("#checkedAll").prop("checked", true); }
                } else {
                    $("#checkedAll").prop("checked", false);
                    var selectedValue = $(this)[0].id;
                    var text = $('label[for="' + selectedValue + '"]').text();
                    var cadena = $('#lblAbo').text();
                    cadena = cadena.replace(text, '');
                    cadena = cadena.replace(',', '');
                    var i = cadena.length;

                    if (i > 21) {
                        $('#lblAbo').text(cadena);
                        $("#lblAbo").addClass("alert-success");
                    }
                    else {
                        $('#lblAbo').text("");
                        $("#lblAbo").removeClass("alert-success");
                    }
                }
            });

            // todos roto
            $("#checkedAllR").change(function () {
                var langPref = [];
                if (this.checked) {
                    $(".checkSingleR").each(function () {
                        this.checked = true;
                        var selectedValue = $(this)[0].id;
                        var text = $('label[for="' + selectedValue + '"]').text();
                        langPref.push(text);
                        $('#lblRoto').text(" /ROTO DEL LADO: " + langPref.join(", "));
                    })
                } else {
                    $(".checkSingleR").each(function () {
                        this.checked = false;
                        langPref = [];
                        $('#lblRoto').text("");
                    })
                }
                var a = $("#lblRoto").text();
                var a = a.length;
                if (a > 0)
                    $("#lblRoto").addClass("alert-success");
                else
                    $("#lblRoto").removeClass("alert-success");
            });

            //individual roto
            $(".checkSingleR").click(function () {
                var langPref = [];
                if ($(this).is(":checked")) {
                    var isAllChecked = 0;
                    $(".checkSingleR").each(function () {
                        if (!this.checked) {
                            isAllChecked = 1;
                            //$('#lblCondi').text("");

                        }
                        else {
                            var selectedValue = $(this)[0].id;
                            var text = $('label[for="' + selectedValue + '"]').text();
                            langPref.push(text);
                            if (langPref.length > 0) {
                                $('#lblRoto').text(" /ROTO DEL LADO: " + langPref.join(", "));
                                $("#lblRoto").addClass("alert-success");
                            }
                            else {
                                $('#lblRoto').text("");
                                $("#lblAbo").removeClass("alert-success");
                            }
                        }
                    })
                    if (isAllChecked == 0) { $("#checkedAllR").prop("checked", true); }
                } else {
                    $("#checkedAllR").prop("checked", false);
                    var selectedValue = $(this)[0].id;
                    var text = $('label[for="' + selectedValue + '"]').text();
                    var cadena = $('#lblRoto').text();
                    cadena = cadena.replace(text, '');
                    cadena = cadena.replace(',', '');
                    var i = cadena.length;
                    console.log(i);
                    if (i > 21) {
                        $('#lblRoto').text(cadena);
                        $("#lblAbo").addClass("alert-success");
                    }
                    else {
                        $('#lblRoto').text("");
                        $("#lblAbo").removeClass("alert-success");
                    }
                }
            });

            // texto ubicacion
            $("#slNivel").change(function () {

                var zona = $("#slZona").find("option:selected").text();
                var carril = $("#slCarril").find("option:selected").text();
                var posicion = $("#slPosicion").find("option:selected").text();
                var nivel = $("#slNivel").find("option:selected").text();
                if (zona) {
                    var cadena = zona + " CARRIL " + carril + " POSICION " + posicion + " NIVEL " + nivel;
                    $("#inDesc").text(cadena);
                    $("#Mensaje").addClass("bg-teal");
                }
            });

            $("#slCarril").change(function () {

                var zona = $("#slZona").find("option:selected").text();
                var carril = $("#slCarril").find("option:selected").text();
                var posicion = $("#slPosicion").find("option:selected").text();
                var nivel = $("#slNivel").find("option:selected").text();
                if (zona) {
                    var cadena = zona + " CARRIL " + carril + " POSICION " + posicion + " NIVEL " + nivel;
                    $("#inDesc").text(cadena);
                    $("#Mensaje").addClass("bg-teal");
                }
            });

            $("#slPosicion").change(function () {

                var zona = $("#slZona").find("option:selected").text();
                var carril = $("#slCarril").find("option:selected").text();
                var posicion = $("#slPosicion").find("option:selected").text();
                var nivel = $("#slNivel").find("option:selected").text();
                if (zona) {
                    var cadena = zona + " CARRIL " + carril + " POSICION " + posicion + " NIVEL " + nivel;
                    $("#inDesc").text(cadena);
                    $("#Mensaje").addClass("bg-teal");
                }
            });

            // llenado de selects
            $("#slZona").change(function () {

                $("#slCarril").empty();
                var zonaId = $("#slZona").val();
                var params = new Object();
                params.pIdZona = zonaId;
                params = JSON.stringify(params);

                $.ajax({
                    async: true,
                    type: "POST",
                    url: "Ubicacion.aspx/getCarriles",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var datos = $.parseJSON(msg.d);
                        $("#slCarril").append('<option value=0>' + 'Seleccionar' + '</option>');
                        $(datos).each(function () {
                            $("#slCarril").append('<option value=' + this.IdUbication + '>' + this.IdUbication + '</option>');
                        });
                    },
                    error: function (msg) {
                        bootbox.alert("Error al llenar el combo");
                    }
                });

                $("#slPosicion").empty();
                $.ajax({
                    async: true,
                    type: "POST",
                    url: "Ubicacion.aspx/getPosicion",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var datos = $.parseJSON(msg.d);
                        $("#slPosicion").append('<option value=0>' + 'Seleccionar' + '</option>');
                        $(datos).each(function () {
                            $("#slPosicion").append('<option value=' + this.IdUbication + '>' + this.IdUbication + '</option>');
                        });
                    },
                    error: function (msg) {
                        bootbox.alert("Error al llenar el combo");
                    }
                });
                $("#slNivel").empty();

                $.ajax({
                    async: true,
                    type: "POST",
                    url: "Ubicacion.aspx/getNivel",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var datos = $.parseJSON(msg.d);
                        $("#slNivel").append('<option value=0>' + 'Seleccionar' + '</option>');
                        $(datos).each(function () {
                            $("#slNivel").append('<option value=' + this.IdUbication + '>' + this.IdUbication + '</option>');
                        });
                    },
                    error: function (msg) {
                        bootbox.alert("Error al llenar el combo");
                    }
                });

                var zona = $("#slZona").find("option:selected").text();
                var carril = $("#slCarril").find("option:selected").text();
                var posicion = $("#slPosicion").find("option:selected").text();
                var nivel = $("#slNivel").find("option:selected").text();
                if (zona) {
                    var cadena = zona + " CARRIL " + carril + " POSICION " + posicion + " NIVEL " + nivel;
                    $("#inDesc").text(cadena);
                    $("#Mensaje").addClass("bg-teal");
                }

            });

            //busqueda de datos generales del contenedor
            $("#search").keypress(function (e) {

                if (e.which == 13) {
                    e.preventDefault();
                    if ($('#search').val() == '') {
                        bootbox.alert("Indicar los ultimos 4 dígitos del # de contenedor");
                    }
                    else {
                        var params = new Object();
                        params.n_contenedor = $('#search').val();
                        params = JSON.stringify(params);
                        $.ajax({
                            url: '<%=ResolveUrl("Ubicacion.aspx/GetConteInfo") %>',
                            data: params,
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                var pagos = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;

                                $("#myTable").empty();

                                if (pagos.length > 0) {

                                    for (var i = 0; pagos.length; i++) {

                                        $("#myTable").append('<tbody><tr><td class="clsFilas" colspan="2">CONTENEDOR</td><td colspan="2"><input type="hidden" id="hIdDeta" /> #' + pagos[i].c_correlativo + " - " + $('#search').val() + '</td></tr >'
                                            + '< tr><td class="clsFilas" colspan="2">BUQUE</td><td colspan="2"><input type="hidden" id="hLlegada" />' + pagos[i].c_buque + '</td></tr >'
                                            + '<tr><td class="clsFilas" colspan="2">NAVIERA</td><td colspan="2">' + pagos[i].c_cliente + '</td></tr>'
                                            + '<tr><td class="clsFilas" colspan="2">TARA</td><td colspan="2"><input type="text" id="txtTara" maxlength="4" name="txtTara" value=' + pagos[i].v_tara + '></td></tr>'
                                            + '<tr><td class="clsFilas" colspan="2">PESO (Kgs.)</td><td colspan="2">' + $.number(parseFloat(pagos[i].v_peso).toFixed(2), 2, '.', ',') + '</td></tr>'
                                            + '<tr><td class="clsFilas" colspan="2">PESO TOTAL (PESO + TARA)</td><td colspan="2">' + $.number(parseFloat(pagos[i].v_peso) + pagos[i].v_tara, 2, '.', ',') + '</td></tr>'
                                            + '<tr><td class="clsFilas" colspan="2">TAMAÑO</td><td colspan="2">' + pagos[i].c_tamaño + '</td></tr>'
                                            + '<tr><td class="clsFilas" colspan="2">TRAFICO</td><td colspan="2"><input type="hidden" id="hDirecto" />' + pagos[i].c_trafico + '</td></tr>'
                                            + '<tr><td class="clsFilas" colspan="2">ESTADO</td><td colspan="2">' + pagos[i].c_estado + '</td></tr>'
                                            + '</tbody>');
                                        $("#hIdDeta").val(pagos[i].IdDeta);
                                        $("#hLlegada").val(pagos[i].c_llegada);
                                        //if (pagos[i].c_trafico.indexOf("DIRECTO") != -1) {
                                        //    $("#hDirecto").val("1");
                                        //}
                                        //else {
                                        //    $("#hDirecto").val("0");
                                        //}
                                        cargarUbicaciones();
                                        //$('#filter-menu').selectpicker('mobile');
                                        //$('#filter-menu-button').hide();

                                        break;


                                    }

                                    /*$('#btnAcceso').attr("disabled", false);*/
                                }
                            },
                            error: function (response) {
                                bootbox.alert("CEPA - Contenedores: Alerta!! Se ha producido un error vuelva a intertarlo o reporte a Informática");
                            },
                            failure: function (response) {
                                bootbox.alert(response.responseText);
                            }
                        });
                    }
                }
            });

            $("#btnResumen").click(function () {

                var llegada = $("#hLlegada").val();

                if (llegada != undefined && llegada != null && llegada != "") {

                    $('#myModalEsta').modal('show');
                    var params = new Object();
                    params.c_llegada = llegada;
                    params = JSON.stringify(params);

                    var row = "";
                    $.ajax({
                        async: true,
                        cache: false,
                        type: "POST",
                        url: "Ubicacion.aspx/getResumen",
                        data: params,
                        contentType: "application/json; charset=utf8",
                        dataType: "json",
                        success: function (response) {
                            console.log("Entro");
                            var resumen = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;

                            if (resumen.length > 0) {

                                $('#myTableRe tbody').empty();
                                $('#myTableRe tfoot').empty();

                                $('#myTableRe').append('<tbody>');

                                $.each(JSON.parse(response.d), function (i, v) {
                                    $("#buque").text(v.d_buque);
                                    row += "<tr><td>" + v.c_tamaño + "</td><td class='clsMani'>" + v.manifestados + "</td><td class='clsCancelados'>" + v.cancelados + "</td><td class='clsTotal'>" + v.total + "</td><td class='clsRecibidos'>" + v.recibidos + "</td><td class='clsPendientes'>" + v.pendientes + "</td></tr>";
                                });

                                $("#myTableRe").append(row);

                                $('#myTableRe').append('<tfoot>'
                                    + '<tr>'
                                    + '<th>TOTALES</th>'
                                    + '<th class="totalMani"></th>'
                                    + '<th class="totalCance"></th>'
                                    + '<th class="totalTo"></th>'
                                    + '<th class="totalReci"></th>'
                                    + '<th class="totalPendi"></th>'
                                    + '</tr>'
                                    + '</tfoot>');


                                var sMani = 0, sCancel = 0, sTotal = 0, sReci = 0, sPendi = 0;


                                $('#myTableRe tr').each(function () {

                                    $(this).find('.clsMani').each(function () {
                                        var combat = parseInt($(this).text());
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sMani += parseInt(combat);
                                        }
                                    });
                                    $('.totalMani', this).html(sMani);


                                    $(this).find('.clsCancelados').each(function () {
                                        var combat = parseInt($(this).text());
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sCancel += parseInt(combat);
                                        }
                                    });
                                    $('.totalCance', this).html(sCancel);

                                    $(this).find('.clsTotal').each(function () {
                                        var combat = parseInt($(this).text());
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sTotal += parseInt(combat);
                                        }
                                    });
                                    $('.totalTo', this).html(sTotal);

                                    $(this).find('.clsRecibidos').each(function () {
                                        var combat = parseInt($(this).text());
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sReci += parseInt(combat);
                                        }
                                    });
                                    $('.totalReci', this).html(sReci);

                                    $(this).find('.clsPendientes').each(function () {
                                        var combat = parseInt($(this).text());
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sPendi += parseInt(combat);
                                        }
                                    });
                                    $('.totalPendi', this).html(sPendi);
                                });
                            }
                        },
                        error: function (data, success, error) {
                            bootbox.alert(error);
                        }
                    });
                } else {
                    bootbox.alert("El resumen podrá acceder al indicar un número de contenedor");
                }
            });

            $('#btnGuardar').click(function (e) {
                e.preventDefault();

                var zonaId = $("#slZona").val();
                var gruaId = $("#slGrua").val();
                var carril = $("#slCarril").val();
                var posicion = $("#slPosicion").val();
                var nivel = $("#slNivel").val();
                var s_condicion = $("#lblAbo").text() + $("#lblRoto").text();
                var c_marcacion = ' <%= Session["c_marcacion"] %>';
                var v_tara = $("#txtTara").val();
                var b_sobre = $("#ckSobre")[0].checked == true ? 1 : 0;
                var IdDeta = $("#hIdDeta").val();

                if ($("#frmUbicacion").valid()) {
                    if (zonaId == "0" || gruaId == "0" || (carril == "0" || carril == null) || (posicion == "0" || posicion == null) || (nivel == "0" || nivel == null) || v_tara == '' || $("#search").val() == '') {
                        bootbox.alert("Debe completar la información de confirmación de contenedor en patio de contenedores");
                    } else {
                        bootbox.confirm("¿Desea confirmar el ingreso a patio del contenedor #" + $("#search").val() + " ?", function (result) {
                            if (result) {



                                var params = new Object();
                                params.zonaId = zonaId;
                                params.gruaId = gruaId;
                                params.carril = carril;
                                params.posicion = posicion;
                                params.nivel = nivel;
                                params.s_condicion = s_condicion;
                                params.c_marcacion = c_marcacion;
                                params.v_tara = v_tara;
                                params.b_sobre = b_sobre;
                                params.IdDeta = IdDeta;
                                params = JSON.stringify(params);

                                $.ajax({
                                    url: '<%=ResolveUrl("Ubicacion.aspx/saveConfirmacion") %>',
                                    data: params,
                                    dataType: "json",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (response) {

                                        bootbox.alert(response.d.replace(/['"]+/g, ''), function () {
                                            location.reload();
                                        });
                                    },
                                    error: function (response) {
                                        bootbox.alert("CEPA - Contenedores: Alerta!! Se ha producido un error vuelva a intertarlo o reporte a Informática");
                                    },
                                    failure: function (response) {
                                        bootbox.alert(response.responseText);
                                    }
                                });
                                //$("#myTable").empty();
                                //$("#info").val('');
                                //$("#search").val('');
                                //$('#btnAcceso').attr("disabled", true);
                                setTimeout("location.reload()", 5000);
                            }
                            else {
                                //$("#myTable").empty();
                                //$("#info").val('');
                                //$("#search").val('');
                                //$('#btnAcceso').attr("disabled", true);
                            }
                        });
                    }
                }

            });

            $('#btnCancelar').click(function (e) {
                setTimeout("location.reload()", 1000);
            });
        });

        function cargarUbicaciones() {


            $.ajax({
                async: true,
                type: "POST",
                url: "Ubicacion.aspx/getUbicaciones",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $("#slZona").empty();
                    var datos = $.parseJSON(msg.d);
                    $("#slZona").append('<option value="0">' + 'Seleccionar Zona' + '</option>');
                    $(datos).each(function () {
                        $("#slZona").append('<option value=' + this.IdZona + '>' + this.Zona + '</option>');
                    });
                },
                error: function (msg) {
                    bootbox.alert("Error al llenar el combo");
                }
            });

            $.ajax({
                async: true,
                type: "POST",
                url: "Ubicacion.aspx/getGruas",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var datos = $.parseJSON(msg.d);
                    $("#slGrua").empty();
                    $("#slGrua").append('<option value=0>' + 'Seleccionar Grua' + '</option>');
                    $(datos).each(function () {
                        $("#slGrua").append('<option value=' + this.IdGrua + '>' + this.Nombre + '</option>');
                    });
                },
                error: function (msg) {
                    bootbox.alert("Error al llenar el combo");
                }
            });
        }

        function SetAutoComplete() {
            $("#search").autocomplete({
                minLength: 4,
                source: function (request, response) {
                    var params = new Object();
                    params.prefix = request.term;
                    params = JSON.stringify(params);
                    $.ajax({
                        url: '<%=ResolveUrl("Ubicacion.aspx/GetConte") %>',
                        data: params,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var lista = (typeof data.d) == "string" ? eval('(' + data.d + ')') : data.d;
                            if (lista.length > 0) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split('-')[0],
                                        val: item.split('-')[1]
                                    }
                                }))
                            }
                            else {
                                bootbox.alert("Busqueda no produce resultados");
                            }
                        },
                        error: function (response) {
                            bootbox.alert("CEPA - Contenedores: Alerta!! Se ha producido un error vuelva a intertarlo o reporte a Informática");
                        },
                        failure: function (response) {
                            bootbox.alert(response.responseText);
                        }
                    });
                }
            });
        }
    </script>
</body>
</html>

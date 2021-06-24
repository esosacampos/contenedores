<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfSolicitud.aspx.cs" Inherits="CEPA.CCO.UI.Web.Bodega.wfSolicitud" %>

<%@ Register Src="~/Controles/ucMultiFileUpload.ascx" TagPrefix="uc1" TagName="ucMultiFileUpload" %>


<!DOCTYPE html>
<html lang="es">

<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="Solicitud de Vaciado">
    <meta name="author" content="CEPA Puerto de Acajutla">

    <link rel="apple-touch-icon" sizes="57x57" href="/metas/apple-icon-57x57.png">
    <link rel="apple-touch-icon" sizes="60x60" href="/metas/apple-icon-60x60.png">
    <link rel="apple-touch-icon" sizes="72x72" href="/metas/apple-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="76x76" href="/metas/apple-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="114x114" href="/metas/apple-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="120x120" href="/metas/apple-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="144x144" href="/metas/apple-icon-144x144.png">
    <link rel="apple-touch-icon" sizes="152x152" href="/metas/apple-icon-152x152.png">
    <link rel="apple-touch-icon" sizes="180x180" href="/metas/apple-icon-180x180.png">
    <link rel="icon" type="image/png" sizes="192x192" href="/metas/android-icon-192x192.png">
    <link rel="icon" type="image/png" sizes="32x32" href="/metas/favicon-32x32.png">
    <link rel="icon" type="image/png" sizes="96x96" href="/metas/favicon-96x96.png">
    <link rel="icon" type="image/png" sizes="16x16" href="/metas/favicon-16x16.png">
    <%--    <link rel="manifest" href="/metas/manifest.json">--%>


    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="msapplication-TileImage" content="/metas/ms-icon-144x144.png">
    <meta name="theme-color" content="#ffffff">
    <title>Solicitud De Vaciado</title>

    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="vendor/metisMenu/metisMenu.min.css" rel="stylesheet">

    <link href="vendor/datatables-plugins/dataTables.bootstrap.css" rel="stylesheet">
    <!-- DataTables Responsive CSS -->
    <link href="vendor/datatables-responsive/dataTables.responsive.css" rel="stylesheet">

    <link href="dist/css/sb-admin-2.css" rel="stylesheet">
    <%--<link href="vendor/morrisjs/morris.css" rel="stylesheet">--%>
    <link href="vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">

    <link href="vendor/bootstrap/css/bootstrap.fd.css" rel="stylesheet" type="text/css" />

    <link href="vendor/select/bootstrap-select.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        #img1 {
            width: 7%;
            height: 35px;
            display: inline-block;
        }

        .form-group {
            margin-bottom: 5px;
        }

        #img2 {
            width: 65%;
            height: 30%;
            margin-left: 18%;
        }

        #img3 {
            width: 75%;
            height: 30%;
            margin-left: 12%;
        }

        #img4 {
            width: 80%;
            height: 30%;
            margin-left: 10%;
        }

        .page-header {
            padding-bottom: 9px;
            margin: 5px 0 5px;
            border-bottom: 1px solid #eee;
        }

        .h1, h1 {
            font-size: 20px;
        }

        div.dataTables_wrapper {
            width: 100%;
            margin: 0 auto;
        }

        .sidebar ul li {
            border-bottom: none;
        }

        .bootstrap-select:not([class*=col-]):not([class*=form-control]):not(.input-group-btn) {
            width: 100%;
        }

        /*.navbar .btn-navbar {
            display: none;
        }

        .nav-off {
            display: block !important
        }*/
        #txtContacto, #txtContenedor {
            text-transform: uppercase;
        }

        .navbar-brand {
            float: left;
            height: 50px;
            padding: 10px 5px;
            font-size: 25px;
            line-height: 10px;
            display: inline-block;
        }

        .panel-default > .panel-heading {
            color: #333;
            background-color: #f5f5f5;
            border-color: #ddd;
            font-weight: 900;
            font-size: 1.25em;
        }

        @media (min-width: 1200px) {
            div.col-lg-12 {
                padding-right: 6px;
            }
        }



        @media screen and (max-width: 700px) {
            .navbar .btn-navbar {
                display: block;
            }

            .navbar .nav-collapse {
                display: none;
            }

            .nav-off {
                display: none !important
            }
        }

        @media only screen and (max-width: 600px) {
            #img1 {
                width: 20%;
                height: 10%;
            }

            #img2 {
                width: 20%;
                height: 10%;
            }

            #img3 {
                width: 20%;
                height: 10%;
            }

            .h1, h1 {
                font-size: 18px;
            }
        }

        @media (min-width: 768px) {
            .sidebar {
                z-index: 1;
                position: absolute;
                width: 50px;
                margin-top: 51px;
            }

            #page-wrapper {
                position: inherit;
                margin: 0 0 0 50px;
                padding-left: 35px;
                border-left: 1px solid #e7e7e7;
                padding-right: 35px;
            }
        }
    </style>
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form id="frmSolicitud" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
            EnablePageMethods="true">
            <%--EnablePartialRendering="true" AsyncPostBackTimeout="600"--%>
            <Scripts>
                <asp:ScriptReference Path="~/vendor/jquery/jquery.min.js" />
                <asp:ScriptReference Path="~/js/jquery.inputmask.bundle.js" />
                <asp:ScriptReference Path="~/js/jquery.validate.js" />
                <asp:ScriptReference Path="~/js/additional-methods.min.js" />
                <asp:ScriptReference Path="~/vendor/bootstrap/js/bootstrap.min.js" />
                <asp:ScriptReference Path="~/vendor/bootstrap/js/bootbox.min.js" />
                <asp:ScriptReference Path="~/vendor/metisMenu/metisMenu.min.js" />
                <asp:ScriptReference Path="~/vendor/raphael/raphael.min.js" />
                <%--<asp:ScriptReference Path="~/vendor/morrisjs/morris.min.js" />
                <asp:ScriptReference Path="~/data_/morris-data.js" />--%>
                <asp:ScriptReference Path="~/vendor/datatables/js/jquery.dataTables.min.js" />
                <asp:ScriptReference Path="~/vendor/datatables-plugins/dataTables.bootstrap.min.js" />
                <asp:ScriptReference Path="~/vendor/datatables-responsive/dataTables.responsive.js" />
                <asp:ScriptReference Path="~/dist/js/sb-admin-2.js" />
                <asp:ScriptReference Path="~/vendor/bootstrap/js/bootstrap.fd.js" />
                <asp:ScriptReference Path="~/dist/js/jquery.blockui.js" />
                <asp:ScriptReference Path="~/js/bootstrap-select.min.js" />
                <asp:ScriptReference Path="~/js/bootstrap-filestyle.min.js" />
            </Scripts>
        </asp:ScriptManager>
        <div id="wrapper">
            <!-- Navigation -->
            <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="wfSolicitud.aspx" style="display: inline-block;">
                        <img src="vendor/bootstrap/Images/cepa_logo.png" id="img1" class="img-responsive" style="display: inline-block;"><span style="margin-left: 1%; font-weight: bold">Puerto de Acajutla - Solicitud de Vaciado/Llenado de Contenedores de Importación</span></a>
                </div>
                <!-- /.navbar-header -->

                <div class="navbar-default sidebar hidden-xs" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li>
                                <%--<img src="vendor/bootstrap/Images/cepa_logo.png" id="img1" class="img-responsive" />--%>
                            </li>
                            <li>
                                <br />
                            </li>
                            <%-- <li>
                                <%--<img src="vendor/bootstrap/Images/pnc_dan.png" id="img2" alt="" class="img-responsive" />
                            </li>
                            <li>
                                <br />
                            </li>
                            <li>
                               <%-- <img src="vendor/bootstrap/Images/ucc.png" alt="" id="img4" class="img-responsive" />
                            </li>
                            <li>
                                <br />
                            </li>
                            <li>
                                <%--<img src="vendor/bootstrap/Images/pnc_12.png" alt="" id="img3" class="img-responsive" />
                            </li>--%>
                        </ul>
                    </div>
                    <!-- /.sidebar-collapse -->
                </div>
                <!-- /.navbar-static-side -->
            </nav>
            <div id="page-wrapper" style="overflow: hidden;">
                <div class="row">
                    <div class="col-lg-12">
                        <h1 class="page-header" style="font-weight: 900;"></h1>
                    </div>
                    <!-- /.col-lg-12 -->
                </div>
                <!-- /.row -->
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Ingresar información conforme la solicitud presentada a DGA
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div role="form">
                                    <div class="form-group">
                                        <label># de Manifiesto</label>
                                        <asp:TextBox ID="txtMani" runat="server" class="form-control" autocomplete="off"
                                            placeholder="Ingrese # de Manifiesto 202X-XXXX" Text=""></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label># de Contenedor</label>
                                        <asp:TextBox ID="txtContenedor" runat="server" class="form-control" autocomplete="off"
                                            placeholder="Ingrese # Contenedor XYZUXXXXXX"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>Tipo de Solicitud</label>
                                        <asp:DropDownList ID="ddlTipoVac" runat="server" Style="width: 100%;" class="selectpicker show-tick seleccion" data-style="" title="Por favor seleccione el tipo de operación">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Nombre del Solicitante</label>
                                        <asp:TextBox ID="txtContacto" runat="server" CssClass="form-control" placeholder="ABC SA DE CV" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>Teléfono de Solicitante</label>
                                        <asp:TextBox ID="txtTel" runat="server" CssClass="form-control" placeholder="9999-9999" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>Correo Electrónico</label>
                                        <asp:TextBox ID="txtMail" runat="server" CssClass="form-control" placeholder="cuenta@correo.com" autocomplete="off"></asp:TextBox>
                                        <%-- <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                                            ControlToValidate="txtMail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" />--%>
                                    </div>
                                    <%--<div class="form-group">--%>
                                    <%--  <uc1:ucMultiFileUpload ID="CargarPDF" runat="server" />
                                        <input type="button" id="open_btn" class="btn btn-primary" value="Cargar Autorización">
                                    </div>--%>
                                    <div class="form-group">
                                        <label class="control-label" style="color: black !important;">Anexar Autorización de ADUANA</label>
                                        <input type="file" id="fileUpload" name="fileUpload" class="filestyle" data-text="Examinar" data-placeholder="Seleccione un archivo tipo pdf | jpg | png" data-btnclass="btn-success" data-dragdrop="true" />
                                    </div>
                                    <div class="form-group bg-primary">
                                        <p class="lead text-justify">El archivo que se adjunte en este campo debe ser un documento legalmente valido y no una falsificación o alteración con el ánimo de engañar a esta institución. Cualquier contravención representa un delito sujeto a la Ley Salvadoreña.</p>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <button type="submit" id="btnBuscar" class="btn btn-primary">Enviar</button>
                                    <asp:Button ID="btnClear" runat="server" class="btn btn-default" Text="Limpiar" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">

        $ = jQuery.noConflict();
        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

        function OnBeginRequest(sender, args) {
            $.blockUI({
                message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/vendor/bootstrap/Images/progress_bar.gif") %>" />',
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
        }

        function endRequest(sender, args) {
            $.unblockUI();
        }





        $.validator.addMethod('customphone', function (value, element) {
            return this.optional(element) || /^([2,6,7]{1}[0-9]{3}-[0-9]{4})$/.test(value);
        });




        $(document).ready(function () {


            $('#fileUpload').filestyle({
                iconName: "glyphicon glyphicon-inbox",
                onChange: function (param) {
                    console.log(param)
                    alert(param);
                }
            });

            $('#fileUpload').change(function (e) {
                filename = this.files[0].name
                if (filename.length > 0)
                    $(this).valid();
                else
                    $(this).valid() = false;

            });

            $('#txtMani').inputmask('9999-9999');
            $('#txtMail').inputmask('email');
            $('#txtTel').inputmask('9999-9999');



            $("#btnClear").click(function (event) {
                //document.frmSolicitud.reset();
                // $('#frmSolicitud').trigger("reset");
                //window.location = window.location.href;
                //$('#txtMani').val('');
                //$('#txtContenedor').val('');
                //$('#txtMail').val('');
                //$('#txtTel').val('');
                //$('#fileUpload').val('');
                //$('#ddlTipoVac').val('');

                location.reload(true);
            });

            $("#<%=ddlTipoVac.ClientID%>").change(function (e) {
                // $(this).valid();
                if ($(this).val() != "0")
                    $(this).valid();
                else
                    $(this).valid() = false;
            });


            $.validator.setDefaults({
                debug: true,
                submitHandler: function (form) {
                    //form.submit();
                    $("#btnBuscar").click();

                },
                onfocusout: function () { return true; }
            });



            $("#frmSolicitud").validate({
                debug: true,
                rules: {
                    txtMani: "required",
                    txtMail: "required",
                    txtContenedor: "required",
                    ddlTipoVac: {
                        required: true
                    },
                    txtContacto: {
                        required: true
                    },
                    txtTel: {
                        required: true,
                        customphone: true
                    },
                    fileUpload: {
                        required: true,
                        extension: "pdf|png|jpe?g|gif"
                    }
                },
                messages: {
                    txtMani: "Por favor ingrese el número de manifiesto",
                    txtMail: "Por favor ingrese correo electrónico",
                    txtContenedor: "Por favor ingrese número de contenedor",
                    txtContacto: {
                        required: "Por favor ingrese nombre de contacto"
                    },
                    txtTel:
                    {
                        required: "Por favor ingresar número de contacto",
                        customphone: "Por favor un número válido inicia con 2, 6 o 7"
                    },
                    fileUpload: {
                        required: "Por favor adjuntar carta de autorización",
                        extension: "Tipo de archivo no válido"
                    }
                },
                //submitHandler: function (form) {                    
                //        form.submit();                    
                //},
                errorElement: "em",
                errorPlacement: function (error, element) {
                    // Add the `help-block` class to the error element
                    //$.unblockUI();
                    error.addClass("help-block");
                    if (element.prop("type") === "checkbox" || element.prop("type") == "select") {
                        error.insertAfter(element.parent("label"));
                    }
                    else if (element.attr("name") == "ddlTipoVac") {
                        error.insertAfter(".bootstrap-select");
                    } else if (element.attr("name") == "fileUpload") {
                        error.insertAfter(".bootstrap-filestyle");
                    }
                    else {
                        error.insertAfter(element);
                    };

                },
                highlight: function (element, errorClass, validClass) {
                    // $.unblockUI();
                    $(element).parents(".form-group").addClass("has-error").removeClass("has-success");
                },
                unhighlight: function (element, errorClass, validClass) {
                    //$.unblockUI();
                    $(element).parents(".form-group").addClass("has-success").removeClass("has-error");
                }

            });



            $("#btnBuscar").click(function (event) {

                event.preventDefault();


                if ($("#frmSolicitud").valid()) {

                    $.blockUI({
                        message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/vendor/bootstrap/Images/progress_bar.gif") %>" />',
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

                    if ($('#fileUpload').prop('files')[0].name.length > 0) {

                        var data = new FormData();
                        data.append($('#fileUpload').prop('files')[0].name, $('#fileUpload').prop('files')[0]);

                        $.ajax({
                            url: '<%= ResolveClientUrl("~/FileUploadHandler.ashx") %>',
                            type: "POST",
                            data: data,
                            contentType: false,
                            processData: false,
                            success: function (result) {
                                var vaciado = new Object();
                                vaciado.pIdTipo = $("[id*=ddlTipoVac]").val();
                                vaciado.n_manifiesto = $("[id*=txtMani]").val();
                                vaciado.n_contenedor = $("[id*=txtContenedor]").val();
                                vaciado.n_contacto = $("[id*=txtContacto]").val();
                                vaciado.t_contacto = $("[id*=txtTel]").val();
                                vaciado.e_contacto = $("[id*=txtMail]").val();
                                vaciado.s_archivo = $('#fileUpload').prop('files')[0].name;

                                vaciado = JSON.stringify(vaciado);
                                $.ajax({
                                    type: "POST",
                                    cache: false,
                                    url: "wfSolicitud.aspx/saveVaciados",
                                    data: vaciado,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        $.unblockUI();
                                        bootbox.alert({
                                            message: response.d,
                                            callback: function () {
                                                location.reload(true);
                                            }
                                        })
                                    },
                                    error: function (request, status, error) {
                                        $.unblockUI();
                                        bootbox.alert(request.statusText);
                                    }
                                });
                            }
                        });
                        return false;
                    }
                }
                else {
                    $.unblockUI();
                }

            });
        });
    </script>
</body>
</html>

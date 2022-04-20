<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfTracking.aspx.cs" Inherits="CEPA.CCO.UI.Web.Tracking.wfTracking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="Consulta Tracking Importacion">
    <meta name="author" content="CEPA Puerto de Acajutla">
    <link rel="shortcut icon" type="image/x-icon" href="vendor/bootstrap/Images/favicon.ico">
    <title>Tracking Contenedores</title>
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">

    <link href="vendor/metisMenu/metisMenu.min.css" rel="stylesheet">

    <link href="vendor/datatables-plugins/dataTables.bootstrap.css" rel="stylesheet">
    <!-- DataTables Responsive CSS -->
    <link href="vendor/datatables-responsive/dataTables.responsive.css" rel="stylesheet">

    <link href="dist/css/sb-admin-2.css" rel="stylesheet">
    <%--<link href="vendor/morrisjs/morris.css" rel="stylesheet">--%>
    <link href="vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="vendor/bootstrap/css/bootstrap-datetimepicker.css" rel="stylesheet" />
    <link href="bootstrap/csss/checbox.css" rel="stylesheet" />
    <style type="text/css">
        #grvTracking tbody tr td {
            vertical-align: middle;
            text-align: center;
        }

        .sidebar ul li {
            border: none;
        }

        tr#rowF {
            display: none;
            background-color: rgb(23, 113, 248);
        }

        div.dataTables_wrapper {
            width: 100%;
            margin: 0 auto;
        }

        .table-hover > tbody > tr:hover {
            background-color: #CECEF6
        }

        #btnVEA {
            background-image: url(dist/logos/vea_logo.png);
            background-repeat: no-repeat;
            height: 37px;
            width: 117px;
            background-size: 115px;
            background-position: center;
        }

        #btnBancos {
            height: 35px;
            width: 100px;
            white-space: normal;
            padding: 1px;
            font-size: 12px;
        }

        table.table-bordered th:last-child,
        table.table-bordered td:last-child {
            border-right: 1px solid #1771f8;
        }

        table.dataTable thead > tr > th {
            padding-left: 5px;
            padding-right: 12px;
            background-color: #1771F8;
            text-align: center;
            color: #fff;
            font-weight: bold;
            border: hidden;
        }

        .badge-notify {
            background: red;
            position: relative;
            top: -22px;
            left: -27px;
        }

        .footer {
            position: fixed;
            bottom: 0;
            width: 100%;
            height: 55px;
            background-color: #f5f5f5;
            left: 0;
            margin-top: 5%;
        }

            .footer > .container {
                padding-right: 15px;
                padding-left: 0px;
            }

        .table-bordered > tbody > tr > td, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > td, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > thead > tr > th {
            border: none;
        }

        #grvTracking_dtTracking_0_grvProv {
            border: 1px solid #1771F8;
        }

        .table-bordered {
            border: 5px solid #1771F8;
        }

        #grvTracking_dtTracking_0_grvProv tbody tr th {
            color: white;
            background-color: #1771F8;
            text-align: center;
            border: 1px solid #1771F8;
        }

        .alignCells {
            text-transform: lowercase;
        }

        #grvTracking tbody tr td:first-letter {
            text-transform: uppercase;
        }

        .dataTables_filter, .dataTables_info, .dataTables_paginate, .dataTables_length {
            display: none;
        }

        #myTableModal td, th {
            text-align: center;
        }

        .DigiCertClickID_0bcATRpn3 {
            text-decoration: none;
            text-align: center;
            display: block;
            vertical-align: baseline;
            font-size: 100%;
            font-style: normal;
            text-indent: 0px;
            line-height: 1;
            width: auto;
            margin: 0px auto;
            padding: 0px;
            border: 0px;
            background: transparent;
            position: relative;
            inset: 8px;
            clear: both;
            float: left;
            cursor: default;
        }

            .DigiCertClickID_0bcATRpn3 img {
                text-decoration: none;
                text-align: left;
                display: block;
                vertical-align: baseline;
                font-size: 100%;
                font-style: normal;
                text-indent: 0px;
                line-height: 1;
                width: 50%;
                margin: 0px auto;
                padding: 0px;
                border: 0px;
                background: transparent;
                position: relative;
                inset: 0px;
                clear: both;
                float: left;
                cursor: pointer;
            }


        #myTableModal thead tr {
            background-color: #1771F8;
            color: white;
        }

        #myTableModal {
            font-size: 12px;
        }

            #myTableModal td.descri {
                text-align: left;
            }

        .label {
            font-size: 90%;
        }

        p {
            margin: 0 0 1px;
        }

        #myTableModal > tfoot > tr > th {
            text-align: right;
        }

        .badgeNO {
            box-sizing: border-box;
            font-family: 'Trebuchet MS', sans-serif;
            background: #ff0000;
            cursor: default;
            border-radius: 44%;
            color: #fff;
            font-weight: bold;
            font-size: 1rem;
            height: 2rem;
            letter-spacing: -.1rem;
            line-height: 1.55;
            margin-top: -2rem;
            margin-left: 1.1rem;
            border: .2rem solid #fff;
            text-align: center;
            display: inline-block;
            width: 3rem;
            box-shadow: 1px 1px 5px rgba(0,0,0, .2);
            animation: pulse 1.5s 1;
        }

            .badgeNO:after {
                content: '';
                position: absolute;
                top: -.1rem;
                left: -.1rem;
                border: 2px solid rgba(255,0,0,.5);
                opacity: 0;
                border-radius: 50%;
                width: 100%;
                height: 100%;
                animation: sonar 1.5s 1;
            }

        @keyframes sonar {
            0% {
                transform: scale(.9);
                opacity: 1;
            }

            100% {
                transform: scale(2);
                opacity: 0;
            }
        }

        @keyframes pulse {
            0% {
                transform: scale(1);
            }

            20% {
                transform: scale(1.4);
            }

            50% {
                transform: scale(.9);
            }

            80% {
                transform: scale(1.2);
            }

            100% {
                transform: scale(1);
            }
        }


        .badge {
            display: inline-block;
            min-width: 10px;
            padding: 3px 7px;
            font-size: 12px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #0A8ADB;
            border-radius: 10px;
        }

        .badgeR {
            display: inline-block;
            min-width: 10px;
            padding: 3px 7px;
            font-size: 12px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #C60000;
            border-radius: 10px;
        }

        .embed-responsive-16by9 {
            padding-bottom: 43.25%;
        }

        .badgeRP {
            display: inline-block;
            min-width: 10px;
            padding: 3px 6px;
            font-size: 16px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #AB0000;
            border-radius: 10px;
            margin-left: 5px;
            margin-right: 5px;
        }


        .badgeV {
            display: inline-block;
            min-width: 10px;
            padding: 3px 6px;
            font-size: 16px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #6495ed;
            border-radius: 10px;
            margin-left: 5px;
            margin-right: 5px;
        }

        /* .modal-dialog {
            width: 600px;
            margin: 3px auto;
        }*/

        .grecaptcha-badge {
            width: 256px;
            height: 60px;
            transition: right 0.3s ease;
            position: fixed;
            bottom: 61px !important;
            right: -186px;
            box-shadow: grey 0px 0px 5px;
        }

        div.checkbox.checkbox-primary.checkbox-inline span label {
            font-weight: bold;
        }
        /*table#grvTracking tr td:nth-child(7), table#_grvTracking tr th:nth-child(7) {
            display: none;
        }*/
    </style>

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
            EnablePageMethods="true" EnableHistory="true">
            <Scripts>
                <asp:ScriptReference Path="~/vendor/jquery/jquery.min.js" />
                <asp:ScriptReference Path="~/vendor/bootstrap/js/bootstrap.js" />
                <%--<asp:ScriptReference Path="~/vendor/bootstrap/js/bootbox.min.js" />--%>
                <%--<asp:ScriptReference Path="~/vendor/metisMenu/metisMenu.min.js" />
                <asp:ScriptReference Path="~/vendor/raphael/raphael.min.js" />--%>
                <%-- <asp:ScriptReference Path="~/vendor/morrisjs/morris.min.js" />
                <asp:ScriptReference Path="~/data_/morris-data.js" />--%>
                <asp:ScriptReference Path="~/vendor/datatables/js/jquery.dataTables.min.js" />
                <asp:ScriptReference Path="~/vendor/datatables-plugins/dataTables.bootstrap.min.js" />
                <asp:ScriptReference Path="~/vendor/datatables-responsive/dataTables.responsive.js" />
                <%--<asp:ScriptReference Path="~/dist/js/sb-admin-2.js" />--%>
                <asp:ScriptReference Path="~/vendor/bootstrap/js/moment-with-locales.min.js" />
                <asp:ScriptReference Path="~/vendor/bootstrap/js/bootstrap-datetimepicker.js" />
                <asp:ScriptReference Path="~/vendor/bootstrap/js/bootbox.min.js" />
                <asp:ScriptReference Path="https://www.google.com/recaptcha/api.js" />
                <asp:ScriptReference Path="~/dist/js/jquery.blockui.js" />
            </Scripts>
        </asp:ScriptManager>
        <div id="wrapper">
            <!-- Navigation 258647-->
            <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="wfTracking.aspx">
                        <img src="vendor/bootstrap/logo/cepa_logo.png" alt="" class="imageTitulo" /></a>
                    <p class="tituloCabecera">CEPA - Tracking Contenedores</p>
                </div>
                <ul class="nav navbar-top-links navbar-right list">
                    <li>
                        <center>
                            <a href="https://www.websmultimedia.com/contador-de-visitas-gratis" title="">
                                <img style="border: 0px solid; display: inline; width: 120px; height: 25px;" alt="" src="https://www.websmultimedia.com/contador-de-visitas.php?id=560"></a><br>
                            <a href='http://www.websmultimedia.com/registro-de-marcas-y-logotipos' style="color: #777; font-size: 20px; font-weight: bold;">Visitantes</a></center>
                    </li>
                </ul>
                <div class="navbar-default sidebar hidden-xs" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li></li>

                            <li>
                                <img src="dist/logos/aduana.png" alt="" style="margin-bottom: 5%; margin-left: 10%; margin-top: 20%; width: 120px;" class="img-responsive">
                            </li>
                            <li></li>
                            <li>
                                <img src="dist/logos/pnc.png" alt="" style="margin-bottom: 5%; margin-left: 10%; width: 120px; height: 150px; margin-top: 10%;" class="img-responsive">
                            </li>
                            <li></li>
                            <li>
                                <img src="dist/logos/mag.png" alt="" style="margin-bottom: 5%; margin-left: 10%; margin-top: 10%; width: 125px;" class="img-responsive">
                            </li>
                            <li></li>
                            <li>
                                <a href="wfAyuda.aspx" id="linkManual" data-toggle="tooltip" data-placement="bottom" title="Clic para ver Manual de Usuario" alt="Manual de Usuario">
                                    <img src="dist/logos/manual.png" alt="" style="margin-bottom: 5%; margin-left: 5%; width: 120px; height: 130px;" class="img-responsive">
                                </a>
                            </li>
                        </ul>
                    </div>
                    <!-- /.sidebar-collapse -->
                </div>
                <!-- /.navbar-static-side -->
            </nav>
            <div id="page-wrapper">
                <!-- /.row -->
                <br />
                <div class="panel panel-info">
                    <div class="panel-heading">
                        Ingresar Información
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div role="form">
                                    <div class="form-group">
                                        <label class="control-label" for="name"># Contenedor</label>
                                        <asp:TextBox required ID="txtBuscar" runat="server" CssClass="form-control" autocomplete="off" placeholder="# de contenedor sin guiones"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-8" style="width: 50%;">
                                <div class="form-inline">
                                    <div class="form-group" style="margin-right: 15px;">
                                        <label>Año Declaración</label><br />
                                        <asp:TextBox required ID="a_declaracion" size="10" runat="server" autocomplete="off" CssClass="form-control" placeholder="20XX"></asp:TextBox>
                                    </div>
                                    <div class="form-group" style="margin-left: 15px; margin-right: 15px;">
                                        <label># Serie</label><br />
                                        <asp:TextBox required ID="n_serial" runat="server" size="5" autocomplete="off" CssClass="form-control" placeholder="X"></asp:TextBox>
                                    </div>
                                    <div class="form-group" style="margin-left: 15px; margin-right: 15px;">
                                        <label># Correlativo</label><br />
                                        <asp:TextBox required ID="n_correlativo" runat="server" autocomplete="off" size="10" CssClass="form-control" placeholder="XXXXXX" Style="margin-right: 2px;"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <div class="checkbox checkbox-primary checkbox-inline" style="margin-top: 23%; padding-left: 5px;">
                                            <asp:CheckBox runat="server" ID="radio3" Text="Sidunea World" Style="font-weight: bold;" Checked="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4" style="width: 47%; padding-right: 4px; padding-top: 1.8%;">
                                <div class="form-inline" style="font-size: 12px;">
                                    <div id="googleRecaptchadiv">
                                        <!-- BEGIN: ReCAPTCHA implementation example. 6LcLJ0UeAAAAAG9ZafCHqikVMTLTOhvRI6yT14K0 anerio 6LfrpzEUAAAAAE7ID9J6bhWi26WXJuKFKGHRpE81-->
                                        <div id="recaptcha-demo" class="g-recaptcha" data-sitekey="6LcLJ0UeAAAAAG9ZafCHqikVMTLTOhvRI6yT14K0" data-callback="onSuccess" data-bind="recaptcha_demo_submit"></div>
                                        <script>
                                            var onSuccess = function (response) {
                                                debugger;
                                                var errorDivs = document.getElementsByClassName("recaptcha-error");
                                                if (errorDivs.length) {
                                                    errorDivs[0].className = "";
                                                }
                                                var errorMsgs = document.getElementsByClassName("recaptcha-error-message");
                                                if (errorMsgs.length) {
                                                    errorMsgs[0].parentNode.removeChild(errorMsgs[0]);
                                                }
                                                var clickButton = document.getElementById("<%= btnBuscar.ClientID %>");
                                                clickButton.click();
                                            };
                                        </script>
                                        <!-- Optional noscript fallback. -->
                                        <!-- END: ReCAPTCHA implementation example. -->
                                    </div>
                                    <div class="form-group">
                                        <asp:Button CssClass="btn btn-primary" runat="server" ID="recaptcha_demo_submit" Text="Consultar" OnClick="recaptcha_demo_submit_Click" Style="display: none;" />
                                        <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" Text="Consultar" OnClick="btnBuscar_Click" />
                                        <asp:Button ID="btnClrear" runat="server" CssClass="btn btn-success" Text="Limpiar" OnClick="btnLimpiar_Click" />
                                        <%--   <button id="btnClear" class="btn btn btn-success">Limpiar</button> class="d-inline-block material-tooltip-smaller"--%>
                                        <input type="button" id="exportpdf" value="Imprimir" class="btn btn-info">
                                        <button id="btnVEA" type="button" class="btn btn-success" data-toggle="tooltip" title="Clic ir a VEA de la DGA" onclick="shwwindow()" disabled></button>
                                        <%--<span id="badgeNN" class="badgeNO badge-notify">New</span>--%>
                                        <button id="btnBancos" type="button" class="btn btn-success" data-toggle="modal" data-target="#myModalBancos" disabled>Cuentas Colectoras</button>
                                        <span id="badgeNN" class="badgeNO badge-notify">New</span>
                                    </div>
                                    <%-- <div class="form-group" style="margin-bottom: 26px; margin-left: -3px;">
                                        <span class="label label-danger" style="">New</span>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="panel panel-info">
                            <div class="panel-heading">
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div id="printArea">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grvTracking" runat="server" Font-Size="0.75em" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvTracking_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hidden" runat="server" Value='<%#Eval("f_tarja")%>' />
                                                                    <asp:HiddenField ID="hPeso" runat="server" Value='<%#Eval("v_peso")%>' />
                                                                    <asp:HiddenField ID="hEstado" runat="server" Value='<%#Eval("b_cancelado")%>' />
                                                                    <asp:HiddenField ID="hTarjas" runat="server" Value='<%#Eval("c_tarjasn")%>' />
                                                                    <asp:HiddenField ID="HNTarjas" runat="server" Value='<%#Eval("con_tarjas")%>' />
                                                                    <img alt="" src="vendor/bootstrap/Images/plus.gif" iddeta="<%# Eval(" IdDeta") %>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="n_manifiesto" HeaderText="MANIFIESTO"></asp:BoundField>
                                                            <asp:BoundField DataField="c_llegada" HeaderText="LLEGADA"></asp:BoundField>
                                                            <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                                                            <asp:BoundField DataField="c_tarja" HeaderText="TARJA"></asp:BoundField>
                                                            <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                                                            <asp:BoundField DataField="b_estado" HeaderText="ESTADO S/MANIFIESTO"></asp:BoundField>
                                                            <asp:BoundField DataField="s_marchamo" HeaderText="# MARCHAMO"></asp:BoundField>
                                                            <asp:BoundField DataField="b_trafico" HeaderText="TRAFICO"></asp:BoundField>
                                                            <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA"></asp:BoundField>
                                                            <asp:BoundField DataField="d_buque" HeaderText="BUQUE"></asp:BoundField>
                                                            <asp:BoundField DataField="f_llegada" HeaderText="F/ATRAQUE" HtmlEncode="false" DataFormatString="{0:F}" ItemStyle-CssClass="alignCells"></asp:BoundField>
                                                            <asp:BoundField DataField="f_desatraque" HeaderText="F/DESATRAQUE" HtmlEncode="false" DataFormatString="{0:F}" ItemStyle-CssClass="alignCells"></asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <button type="button" class="btn btn-primary btn xs" onclick="return getBL(this)" id="tooltop" data-toggle="tooltip" data-placement="top" data-original-title="Consultar el estado para despachar su contenedor">
                                                                        <span class="glyphicon glyphicon-usd" style="cursor: pointer;"></span>
                                                                    </button>
                                                                    <tr id="rowF" iddeta="<%# Eval(" IdDeta") %>">
                                                                        <td colspan="14">
                                                                            <div style="position: relative;">
                                                                                <asp:DetailsView ID="dtTracking" runat="server" AutoGenerateRows="False" DataKeyNames="IdDeta" CssClass="table table-striped table-bordered table-hover" CellPadding="0" GridLines="None">
                                                                                    <Fields>
                                                                                        <asp:BoundField DataField="n_BL" HeaderText="BLs Asociados" ReadOnly="True" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="s_consignatario" HeaderText="Consignatario" ReadOnly="True" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ReadOnly="True" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_rep_naviera" HeaderText="Anuncio Contenedor a Bordo por NAVIERA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_aut_aduana" HeaderText="Autorización Desestiba por ADUANA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_recepA" HeaderText="Recepcion Contenedor" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_trans_aduana" HeaderText="Confirmación de Recepción para ADUANA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_recep_patio" HeaderText="Ubicacion de Contenedor por CEPA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_cancelado" HeaderText="Cancelación del Contenedor por la NAVIERA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_cambio" HeaderText="Cambio de Condición por NAVIERA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_reg_aduana" HeaderText="Registro de la Declaración de Mercancías" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:TemplateField HeaderStyle-Font-Bold="true">
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label Text="Selectividad de la Declaración de Mercancías" ID="lblFS" runat="server"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Label Text='<%# Bind("f_reg_selectivo") %>' ID="lblFechaP" runat="server"></asp:Label>
                                                                                                <asp:Label Text="" ID="lblSelectividad" runat="server"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="f_lib_aduana" HeaderText="Orden de Levante" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_ret_dan" HeaderText="Orden de Retención en Línea por DAN" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_tramite_dan" HeaderText="Cliente/Tramitador Confirma Retención por DAN" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_marchamo_dan" HeaderText="Corte de Marchamo por DAN" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_deta_dan" HeaderText="Liberación en Línea por DAN" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_ret_ucc" HeaderText="Orden de Retención en Línea por UCC" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_tramite_ucc" HeaderText="Cliente/Tramitador Confirma Retención por UCC" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_marchamo_ucc" HeaderText="Corte de Marchamo por UCC" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_deta_ucc" HeaderText="Liberación en Línea por UCC" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_ret_mag" HeaderText="Retención en Línea por MAG" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_lib_mag" HeaderText="Liberación por MAG" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_retencion_dga" HeaderText="Orden de Retención en Línea por DGA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_lib_dga" HeaderText="Orden de Liberación en Línea por DGA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_salida_carga" HeaderText="Emisión Salida de Carga por CEPA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_caseta" HeaderText="Solicitud de Ingreso del Transporte al Puerto" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_solic_ingreso" HeaderText="Asignación de Turno para Cargar en Patio CEPA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_auto_patio" HeaderText="Orden para Cargar en Patio CEPA" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:BoundField DataField="f_puerta1" HeaderText="Salida del Transporte del Puerto" ReadOnly="True" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:TemplateField HeaderStyle-Font-Bold="true">
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label Text="Ubicacion en Patio" ID="lblText" runat="server"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Label Text="" ID="lblUbica" runat="server"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="s_comentarios" HeaderText="Observaciones al Contenedor" ReadOnly="True" HeaderStyle-Font-Bold="true"></asp:BoundField>
                                                                                        <asp:TemplateField HeaderStyle-Font-Bold="true">
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label Text="Movimientos dentro del Recinto" ID="lblq" runat="server"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:GridView ID="grvProv" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField>
                                                                                                            <ItemTemplate>
                                                                                                                <img id="impProvi" alt="" src="vendor/bootstrap/Images/plus.gif" iddetap="<%# Eval(" IdDeta") %>" />
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Width="2%" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:BoundField DataField="Total" HeaderText="Total Movimientos">
                                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                        </asp:BoundField>
                                                                                                        <asp:TemplateField ItemStyle-CssClass="" HeaderText="Total Movimientos">
                                                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Total")%>'></asp:Label>
                                                                                                                <tr id="rowP" style="display: none; background-color: rgb(77, 97, 138);" iddetap="<%# Eval(" IdDeta") %>">
                                                                                                                    <td colspan="4">
                                                                                                                        <div style="position: relative;">
                                                                                                                            <asp:GridView ID="grvDetailProvi" runat="server" AutoGenerateColumns="False" DataKeyNames="c_llegada" CssClass="table table-striped table-bordered table-hover">
                                                                                                                                <Columns>
                                                                                                                                    <asp:BoundField DataField="fecha_prv" HeaderText="Fecha de Movimiento" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="tipo" HeaderText="Tipo"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="motorista_prv" HeaderText="Motorista"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="transporte_prv" HeaderText="Transporte"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="placa_prv" HeaderText="Placa"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="chasis_prv" HeaderText="Chasis"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="fec_reserva" HeaderText="Solicitud Ing. Patio" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells"></asp:BoundField>
                                                                                                                                    <asp:BoundField DataField="fec_valida" HeaderText="Autorización Ing. Patio" DataFormatString="{0:f}" ItemStyle-CssClass="alignCells"></asp:BoundField>
                                                                                                                                </Columns>
                                                                                                                                <EmptyDataTemplate>
                                                                                                                                    <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales
                                                                                                                                </EmptyDataTemplate>
                                                                                                                            </asp:GridView>
                                                                                                                        </div>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </ItemTemplate>
                                                                                                            <%--<ItemStyle CssClass="footable-visible footable-last-column" />
                                                                                                        <HeaderStyle CssClass="footable-visible footable-last-column" />--%>
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <EmptyDataTemplate>
                                                                                                        <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales
                                                                                                    </EmptyDataTemplate>
                                                                                                </asp:GridView>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Fields>
                                                                                </asp:DetailsView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    </span>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmptyMessage" Text="" runat="server" />
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnClrear" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <br />
                                    <br />

                                </div>
                            </div>
                            <!-- Fin Panel -->
                        </div>
                    </div>
                    <div class="alert alert-success" style="margin-bottom: 1%; font-weight: bold;" role="alert">* Se recuerda a todos los consignatarios que la DAN/UCC no realiza ningún cobro por las labores de inspección desarrolladas</div>
                    <div class="alert alert-danger" style="margin-bottom: 1%; font-weight: bold;" role="alert">* Los contenedores marcados en ROJO son CANCELADOS</div>
                    <div class="alert alert-info" style="margin-bottom: 1%; font-weight: bold;" role="alert">* Marcar la casilla Sidunea World para los contenedores manifestados en la nueva plataforma</div>
                </div>
            </div>
        </div>
        <br />
        <footer class="footer" style="margin-top: 0px; margin-left: 5%;">
            <div class="form-inline">
                <div class="form-group">
                    <%--<div id="DigiCertClickID_0bcATRpn3"></div>--%>
                    <%--<script type="text/javascript"> //<![CDATA[
                        var tlJsHost = ((window.location.protocol == "https:") ? "https://secure.trust-provider.com/" : "http://www.trustlogo.com/");
                        document.write(unescape("%3Cscript src='" + tlJsHost + "trustlogo/javascript/trustlogo.js' type='text/javascript'%3E%3C/script%3E"));
//]]></script>
                    <script type="text/javascript">
                        TrustLogo("https://sectigo.com/images/seals/sectigo_trust_seal_sm_2x.png", "SECEV", "none");
                    </script>--%>
                </div>
                <div class="form-group" style="margin-left: 1%;">
                    <p class="text-justify" style="font-size: 11px;">
                        © 2013 CEPA / Puerto de Acajutla, El Salvador v3.0
                    </p>
                    <p class="text-justify" style="font-size: 11px;">
                        Para mayor información contactar: Gerente Portuario <a href="#">Roberto de Jesús Mendoza - 7070-8013 - roberto.mendoza@cepa.gob.sv</a>
                    </p>
                    <p class="text-justify" style="font-size: 11px;">
                        <%--Soporte Técnico <a href="#">Elsa B. Sosa - Sección Informática - 7070-8256 - elsa.sosa@cepa.gob.sv</a> / Ultima actualización : Acajutla, 13 de Julio de 2021--%>
                        Para asistencia técnica - <a href="#">7070-8059 - informática.acajutla@cepa.gob.sv</a> / Ultima actualización : Acajutla, 28 de Marzo de 2022
                    </p>
                </div>
            </div>
        </footer>
        <!-- Modal HTML -->
        <div id="myModal" class="modal fade" tabindex="-1" data-focus-on="input:first">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header" style="line-height: 10px;">
                        <button type="button" class="close" id="myClose" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 class="modal-title">¿Puedo solicitar la salida de carga en CEPA?
                        </h4>
                        <br />
                        <span id="mensajeCEPA" style="font-weight: bold; padding: 8px;"></span>
                    </div>
                    <div class="modal-body">
                        <div role="form">
                            <input type="hidden" id="hTarja" />
                            <input type="hidden" id="hContenedor" />
                            <input type="hidden" id="hLlegada" />
                            <input type="hidden" id="hFtarja" />
                            <input type="hidden" id="hManifiesto" />
                            <input type="hidden" id="hvPeso" />
                            <input type="hidden" id="hTarjas" />
                            <input type="hidden" id="hNTarjas" />

                            <div class="form-group" style="line-height: 1.4;">
                                <div class="row">
                                    <div class="col-md-6">
                                        <label id="lblIngreso">Fecha Ingreso </label>
                                        <span class="label label-info" id="f_tarjaM"></span>
                                    </div>
                                    <div class="col-md-6" style="padding-left: 12px;">
                                        <label id="lblSalida" style="padding-left: 1px; margin-right: 2px;"></label>
                                        <span class="label label-success" id="f_salidaM"></span>
                                    </div>
                                </div>
                                <div class="row" id="calPa" style="margin-top: 5px; margin-bottom: 5px;">
                                    <div class="col-md-5" style="padding-top: 7px;">Fecha Proxima Programada</div>
                                    <div class="col-md-7">
                                        <div class="input-group date" id="dpRetiro" data-toggle="tooltip" data-placement="top" data-original-title="Doble clic fecha a seleccionar">
                                            <input type="text" class="form-control" />
                                            <span class="input-group-addon">
                                                <span class="glyphicon glyphicon-calendar"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="alert alert-info" role="alert" style="background-color: #d3d3d3; border-color: #ababab; color: #060606; margin-bottom: 5px; white-space: nowrap; overflow: overlay;">
                                    <p>
                                        Tarjas Asociadas<span class="badgeV" id="to_Tarjas"></span>:<span class="label label-default" id="d_tarjas" style="font-weight: normal; margin-left: 5px; padding-left: 2px; border-radius: 10px; padding: 7px; background-color: #6495ed; color: #fff;"></span>
                                    </p>
                                </div>
                                <div class="form-group" style="margin-bottom: 5px;">
                                    <table class="table table-striped table-bordered table-hover" id="myTableModal" style="margin-bottom: 5px;">
                                    </table>
                                </div>
                                <div class="alert alert-info" role="alert" style="font-size: 13px; margin-bottom: 5px;">
                                    <p>
                                        Los cálculos aquí plasmados, son con base a fecha de recepción del contenedor y
                                   fecha de salida si la posee, si no se usa la fecha actual. <span class="label label-warning" id="f_leyenda" style="font-weight: normal; margin-left: -4px; padding-left: 2px;"></span><span class="label label-warning" id="f_proxima" style="margin-left: -6px; padding-left: 2px;"></span>
                                    </p>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom: 5px;">
                                <h4 class="modal-title" style="margin-bottom: 5px;">¿Se encuentra solvente con PNC-DAN, UCC y ADUANA?</h4>
                                <span id="MensajeModal" style="font-weight: bold; padding: 4px; line-height: 1.5em;"></span>
                            </div>
                            <div class="form-group" style="margin-bottom: 5px;">
                                <h4 class="modal-title" style="margin-bottom: 5px;">¿El retiro del contenedor se encuentra autorizado por ADUANA?</h4>
                                <span id="mensajeADUANA" style="font-weight: bold; padding: 8px;"></span>
                                <%--<button type="button" class="btn btn-primary" id="btnDetalle" onclick="VerDetalle()">
                                Detalle</button>--%>
                                <%--<button class="btn btn-default" data-toggle="modal" href="#stack2">Launch modal</button>--%>
                            </div>
                            <div class="form-group" style="line-height: 1.4;">
                                <div class="alert alert-danger" role="alert" style="margin-bottom: 5px;">
                                    <p>
                                        Por favor retirar su contenedor, tan pronto le sea cargado caso contrario podrán generarse 
                                    cargos por parqueo dentro del recinto a razón de $4.43 + IVA por hora de permanencia
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" id="myOK">
                            Ok</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal HTML -->
        <div id="myModalD" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header" style="line-height: 10px;">
                        <button type="button" class="close" id="myCloseD1" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 class="modal-title">DETALLE ADUANA
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div role="form">

                            <div class="form-group" style="line-height: 1.4;">
                                <div class="table-responsive">
                                    <table class="table" id="myDecla">
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" id="myOKD">
                            Ok</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal Bancos -->
        <div class="modal fade" role="dialog" id="myModalBancos">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header" style="line-height: 10px;">
                        <button type="button" class="close" id="myCloseD" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 class="modal-title" style="font-weight: 900;">CUENTAS COLECTORAS
                        </h4>
                    </div>
                    <%--<iframe id="serviceFrameSend" src="bancos_htm.htm" width="100" height="100" frameborder="0">--%>
                    <div class="modal-body">
                        <div align="center" class='embed-responsive embed-responsive-16by9'>
                            <iframe class='embed-responsive-item' src="bancos_htm.htm"></iframe>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal" style="font-weight: 900;">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">

            //var __dcid = __dcid || []; __dcid.push(["DigiCertClickID_0bcATRpn3", "10", "s", "white", "0bcATRpn"]); (function () { var cid = document.createElement("script"); cid.async = true; cid.src = "//seal.digicert.com/seals/cascade/seal.min.js"; var s = document.getElementsByTagName("script"); var ls = s[(s.length - 1)]; ls.parentNode.insertBefore(cid, ls.nextSibling); }());


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



                $('#myModal').block({
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



            function doosomething() {
                document.getElementById("tooltop").enabled = true;
            }


            function endRequest(sender, args) {

                //$("#myOK").click(function () {
                //    $("#myModal").modal('hide');
                //});



                //$("#myOKD").click(function () {
                //    $("#myModalD").modal('hide');
                //});


                $.unblockUI();



                $('#myModal').unblock();


            }


            function getBL(lnk) {
                //window.open(myurl, '_blank');
                var row = lnk.parentNode.parentNode;
                var c_llegada = row.cells[2].innerHTML;
                var contenedor = row.cells[3].innerHTML;
                var c_tarja = row.cells[4].innerHTML;
                var hEstado = row.cells[0].childNodes[5].value;
                var url = 'llegada=' + c_llegada + '&contenedor=' + contenedor

                if (c_tarja != "&nbsp;")
                    if (hEstado != "CANCELADO") {
                        window.open('wfConsulBLNvo.aspx?' + url, '_blank');
                    }
                    else {
                        bootbox.alert("CEPA - Contenedores: no puede proceder porque este contenedor fue CANCELADO")
                    }
                else
                    bootbox.alert("CEPA - Contenedores: no puede proceder porque no se posee tarja vuelva intentar mas tarde.")
            }

            function GetSelectedRow(lnk) {
                var row = lnk.parentNode.parentNode;
                var c_llegada = row.cells[2].innerHTML;
                var contenedor = row.cells[3].innerHTML;
                var c_tarja = row.cells[4].innerHTML;
                var n_manifiesto = row.cells[1].innerHTML;
                //alert("LLegada: " + llegada + " Contenedor: " + contenedor);
                var f_tarja = row.cells[0].childNodes[1].value;
                var hTarjas = row.cells[0].childNodes[7].value;
                var NTarjas = row.cells[0].childNodes[9].value;
                var v_peso = $("[id*=hPeso]").val();

                if (c_tarja != "&nbsp;")
                    validar(c_tarja, contenedor, c_llegada, f_tarja, n_manifiesto, v_peso, hTarjas, NTarjas);
                else
                    bootbox.alert("CEPA - Contenedores: no puede proceder porque no se posee tarja vuelva intentar mas tarde.")
                //var a_mani = $("input[name*='a_declaracion']").val();
                return false;
            }

            function ClearText() {

                grecaptcha.reset();

            }




            function validarRetiro(tarja, contenedor, c_llegada, f_tarja, n_manifiesto, f_retiro, v_peso, hTarjas, con_tarjas) {
                if (tarja.length > 0 && contenedor.length > 0) {

                    $('#myModal').block({
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

                    var params = new Object();
                    params.c_tarja = tarja;
                    params.n_contenedor = contenedor;
                    params.c_llegada = c_llegada;
                    params.f_tarja = f_tarja;
                    params.n_manifiesto = n_manifiesto;
                    params.f_retiro = f_retiro;
                    params.v_peso = v_peso;
                    params = JSON.stringify(params);


                    $.ajax({
                        async: true,
                        cache: false,
                        type: "POST",
                        url: "wfTracking.aspx/ValidacionTarjaRe",
                        data: params,
                        contentType: "application/json; charset=utf8",
                        dataType: "json",
                        success: function (response) {

                            var pagos = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;


                            if (pagos.indexOf("Error") != 0) {
                                $("#myTableModal").empty();

                                if (pagos.length > 0) {

                                    for (var a = 0; a < pagos.length; a++) {

                                        $("#mensajeCEPA").text(pagos[a].validacion);

                                        var elM = document.getElementById('mensajeCEPA');
                                        elM.style.color = pagos[a].style_va;

                                        break;

                                    }

                                    $("#myTableModal").append('<thead><th>Estado</th>'
                                        + '<th>Servicio</th>'
                                        + '<th>Detalle</th>'
                                        + '<th>Naviero</th>'
                                        + '<th>Usuario</th>'
                                        + '<th>Pendiente</th>');

                                    $("#myTableModal").append('<tbody>');

                                    $("#myTableModal").append('<tr><td><img id="muelle" src="' + pagos[0].style_descripcion + '"/></td><td class="descri">' + pagos[0].descripcion + '</td><td style="text-align: left;">' + pagos[0].detalle + '</td><td>' + currencyFormat(pagos[0].style_naviero) + '</td><td>' + currencyFormat(pagos[0].style_cliente) + '</td><td>' + currencyFormat(pagos[0].style_pendiente) + '</td></tr>'
                                        + '<tr><td><img id="muelle" src="' + pagos[1].style_descripcion + '"/></td><td class="descri">' + pagos[1].descripcion + '</td><td style="text-align: left;">' + pagos[1].detalle + '</td><td>' + currencyFormat(pagos[1].style_naviero) + '</td><td>' + currencyFormat(pagos[1].style_cliente) + '</td><td>' + currencyFormat(pagos[1].style_pendiente) + '</td></tr>'
                                        + '<tr><td><img id="muelle" src="' + pagos[2].style_descripcion + '"/></td><td class="descri">' + pagos[2].descripcion + '</td><td style="text-align: left;">' + pagos[2].detalle + '</td><td>' + currencyFormat(pagos[2].style_naviero) + '</td><td>' + currencyFormat(pagos[2].style_cliente) + '</td><td>' + currencyFormat(pagos[2].style_pendiente) + '</td></tr>'
                                        + '<tr><td><img id="muelle" src="' + pagos[3].style_descripcion + '"/></td><td class="descri">' + pagos[3].descripcion + '</td><td style="text-align: left;">' + pagos[3].detalle + '</td><td>' + currencyFormat(pagos[3].style_naviero) + '</td><td>' + currencyFormat(pagos[3].style_cliente) + '</td><td>' + currencyFormat(pagos[3].style_pendiente) + '</td></tr>');


                                    $('#to_Tarjas').text(con_tarjas);
                                    $('#d_tarjas').text(hTarjas);

                                    var suma = 0;
                                    var ivaCal = 0;
                                    var totalCal = 0;

                                    var suma1 = 0;
                                    var ivaCal1 = 0;
                                    var totalCal1 = 0;

                                    var sumaPen = 0;
                                    var ivaCalPen = 0;
                                    var totalCalPen = 0;



                                    for (var i = 0; i < pagos.length; i++) {
                                        suma += parseFloat(pagos[i].style_naviero);
                                        suma1 += parseFloat(pagos[i].style_cliente);
                                        sumaPen += parseFloat(pagos[i].style_pendiente);
                                    }

                                    ivaCal = (suma * 0.13).toFixed(2);
                                    totalCal = (parseFloat(suma) + parseFloat(ivaCal)).toFixed(2);

                                    ivaCal1 = (suma1 * 0.13).toFixed(2);
                                    totalCal1 = (parseFloat(suma1) + parseFloat(ivaCal1)).toFixed(2);

                                    ivaCalPen = (sumaPen * 0.13).toFixed(2);
                                    totalCalPen = (parseFloat(sumaPen) + parseFloat(ivaCalPen)).toFixed(2);


                                    //<p id="subTotal1"></p>

                                    $("#myTableModal").append('</tbody>');
                                    $("#myTableModal").append('<tfoot>');
                                    $("#myTableModal").append('<tr><th></th><th></th>'
                                        + '<th style="text-align: right;">Subtotal</th>'
                                        + '<th class="descri"><span class="badge"><p id="subTotal"></p></span></th><th class="descri"><span class="badge"><p id="subTotal1"></p></span></th><th class="descri"><span class="badgeR"><p id="subTotalP"></p></span></th></tr>');
                                    $("#myTableModal").append('<tr><th></th><th></th><th style="text-align: right;">IVA</th>'
                                        + '<th class="descri"><span class="badge"><p id="iva"></p></span></th><th class="descri"><span class="badge"><p id="iva1"></p></span></th><th class="descri"><span class="badgeR"><p id="ivaP"></p></span></th></tr>');
                                    $("#myTableModal").append('<tr><th></th><th></th><th style="text-align: right;">Total</th>'
                                        + '<th class="descri"><span class="badge"><p id="total"></p></span></th><th class="descri"><span class="badge"><p id="total1"></p></span></th><th class="descri"><span class="badgeR"><p id="totalP"></p></span></th></tr>');

                                    $("#myTableModal").append('</tfoot>');


                                    $('#subTotal').text(suma.toFixed(2));
                                    $('#iva').text(ivaCal);
                                    $('#total').text(totalCal);

                                    $('#subTotal1').text(suma1.toFixed(2));
                                    $('#iva1').text(ivaCal1);
                                    $('#total1').text(totalCal1);

                                    $('#subTotalP').text(sumaPen.toFixed(2));
                                    $('#ivaP').text(ivaCalPen);
                                    $('#totalP').text(totalCalPen);

                                    for (var b = 0; b < pagos.length; b++) {
                                        $("#MensajeModal").text(pagos[b].b_danc);

                                        var el = document.getElementById('MensajeModal');
                                        el.style.color = pagos[b].style_dan;



                                        $("#mensajeADUANA").text(pagos[b].b_aduana);

                                        var elt = document.getElementById('mensajeADUANA');
                                        elt.style.color = pagos[b].style_aduana;


                                        $("#f_tarjaM").text(pagos[b].f_tarja);
                                        $("#f_salidaM").text(pagos[b].f_salida);
                                        var frp = validDate();
                                        $("#f_proxima").text(frp);

                                        var valDate = compaDate();

                                        if (valDate) {
                                            $("#f_leyenda").text("Ultima fecha libre de almacenaje fue:");
                                            if ($('#f_proxima').hasClass('label-warning') || $('#f_proxima').hasClass('label-danger')) {
                                                $('#f_proxima').removeClass('label-warning').removeClass('label-danger').addClass('label-danger');
                                                $('#f_leyenda').removeClass('label-warning').removeClass('label-danger').addClass('label-danger');
                                            }

                                        }
                                        else {
                                            $("#f_leyenda").text("Ultima fecha libre de almacenaje es:");
                                            if ($('#f_proxima').hasClass('label-warning') || $('#f_proxima').hasClass('label-danger')) {
                                                $('#f_proxima').removeClass('label-danger').removeClass('label-warning').addClass('label-warning');
                                                $('#f_leyenda').removeClass('label-danger').removeClass('label-warning').addClass('label-warning');
                                            }

                                        }


                                        break;
                                    }



                                    $('#dpRetiro').datetimepicker({
                                        locale: 'es',
                                        format: 'DD/MM/YYYY',
                                        minDate: moment()
                                    });


                                    $("#hTarja").val(tarja);
                                    $("#hContenedor").val(contenedor);
                                    $("#hLlegada").val(c_llegada);
                                    $("#hFtarja").val(f_tarja);
                                    $("#hManifiesto").val(n_manifiesto);
                                    $("#hvPeso").val(v_peso);
                                    $("#hTarjas").val(hTarjas);
                                    $("#hNTarjas").val(con_tarjas);

                                    if (pagos[0].b_salida == "Y") {
                                        $("#calPa").hide();
                                        $('#lblSalida').html("Fecha Retiro Efectiva");

                                        if ($('#f_salidaM').hasClass('label-danger') || $('#f_salidaM').hasClass('label-success')) {
                                            $('#f_salidaM').removeClass('label-danger').removeClass('label-success').addClass('label-success');
                                        }

                                    }
                                    else {
                                        $("#calPa").show();
                                        $('#lblSalida').html("Fecha Próxima Programada");
                                        if ($('#f_salidaM').hasClass('label-danger') || $('#f_salidaM').hasClass('label-success')) {
                                            $('#f_salidaM').removeClass('label-danger').removeClass('label-success').addClass('label-danger');
                                        }

                                    }

                                    if (pagos[0].style_aduana == "#18D318")
                                        $("#btnDetalle").show();
                                    else
                                        $("#btnDetalle").hide();
                                }



                                $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                                    "backdrop": "static",
                                    "keyboard": false,
                                    "show": true                     // ensure the modal is shown immediately
                                });

                                $('#myModal').unblock();
                            }
                            else {
                                $('#myModal').unblock();
                                bootbox.alert(pagos);

                            }
                        },
                        failure: function (response) {
                            bootbox.alert(response.d);
                        },
                        error: function (response) {
                            bootbox.alert(response.d);
                        }
                    });

                    $('#dpRetiro').on('dp.change', function (e) {

                        if (e.Date != "undefined" && e.oldDate != null) {


                            var tr = $("#hTarja").val();
                            var cn = $("#hContenedor").val();
                            var lle = $("#hLlegada").val();
                            var ft = $("#hFtarja").val();
                            var nm = $("#hManifiesto").val();
                            var vPeso = $("#hvPeso").val();
                            var fr = new Date(e.date);
                            var fre = fr.format("dd/MM/yy hh:mm:ss");
                            var hTarj = $("#hTarjas").val();
                            var nTar = $("#hNTarjas").val();
                            validarRetiro(tr, cn, lle, ft, nm, fre, vPeso, hTarj, nTar);

                        }
                    });
                }
            }

            function validDate() {
                var tt1 = $("#f_tarjaM").text();

                var tt = tt1.split("/");



                var date = new Date(tt[1] + '/' + tt[0] + '/' + tt[2]);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + 4);


                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                if (dd < 10) {
                    dd = '0' + dd
                }
                if (mm < 10) {
                    mm = '0' + mm
                }

                var someFormattedDate = dd + '/' + mm + '/' + y;
                return someFormattedDate;
            }


            function validar(tarja, contenedor, c_llegada, f_tarja, n_manifiesto, v_peso, hTarjas, con_tarjas) {
                if (tarja.length > 0 && contenedor.length > 0) {

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

                    var params = new Object();
                    params.c_tarja = tarja;
                    params.n_contenedor = contenedor;
                    params.c_llegada = c_llegada;
                    params.f_tarja = f_tarja;
                    params.n_manifiesto = n_manifiesto;
                    params.v_peso = v_peso;
                    params = JSON.stringify(params);


                    $.ajax({
                        async: true,
                        cache: false,
                        type: "POST",
                        url: "wfTracking.aspx/ValidacionTarja",
                        data: params,
                        contentType: "application/json; charset=utf8",
                        dataType: "json",
                        success: function (response) {

                            var pagos = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;


                            if (pagos.indexOf("Error") != 0) {
                                $("#myTableModal").empty();

                                if (pagos.length > 0) {

                                    for (var a = 0; a < pagos.length; a++) {

                                        $("#mensajeCEPA").text(pagos[a].validacion);

                                        var elM = document.getElementById('mensajeCEPA');
                                        elM.style.color = pagos[a].style_va;

                                        break;

                                    }

                                    $("#myTableModal").append('<thead><th>Estado</th>'
                                        + '<th>Servicio</th>'
                                        + '<th>Detalle</th>'
                                        + '<th>Naviero</th>'
                                        + '<th>Usuario</th>'
                                        + '<th>Pendiente</th>');

                                    $("#myTableModal").append('<tbody>');

                                    $("#myTableModal").append('<tr><td><img id="muelle" src="' + pagos[0].style_descripcion + '"/></td><td class="descri">' + pagos[0].descripcion + '</td><td style="text-align: left;">' + pagos[0].detalle + '</td><td>' + currencyFormat(pagos[0].style_naviero) + '</td><td>' + currencyFormat(pagos[0].style_cliente) + '</td><td>' + currencyFormat(pagos[0].style_pendiente) + '</td></tr>'
                                        + '<tr><td><img id="muelle" src="' + pagos[1].style_descripcion + '"/></td><td class="descri">' + pagos[1].descripcion + '</td><td style="text-align: left;">' + pagos[1].detalle + '</td><td>' + currencyFormat(pagos[1].style_naviero) + '</td><td>' + currencyFormat(pagos[1].style_cliente) + '</td><td>' + currencyFormat(pagos[1].style_pendiente) + '</td></tr>'
                                        + '<tr><td><img id="muelle" src="' + pagos[2].style_descripcion + '"/></td><td class="descri">' + pagos[2].descripcion + '</td><td style="text-align: left;">' + pagos[2].detalle + '</td><td>' + currencyFormat(pagos[2].style_naviero) + '</td><td>' + currencyFormat(pagos[2].style_cliente) + '</td><td>' + currencyFormat(pagos[2].style_pendiente) + '</td></tr>'
                                        + '<tr><td><img id="muelle" src="' + pagos[3].style_descripcion + '"/></td><td class="descri">' + pagos[3].descripcion + '</td><td style="text-align: left;">' + pagos[3].detalle + '</td><td>' + currencyFormat(pagos[3].style_naviero) + '</td><td>' + currencyFormat(pagos[3].style_cliente) + '</td><td>' + currencyFormat(pagos[3].style_pendiente) + '</td></tr>');

                                    $('#to_Tarjas').text(con_tarjas);
                                    $('#d_tarjas').text(hTarjas);

                                    var suma = 0;
                                    var ivaCal = 0;
                                    var totalCal = 0;

                                    var suma1 = 0;
                                    var ivaCal1 = 0;
                                    var totalCal1 = 0;

                                    var sumaPen = 0;
                                    var ivaCalPen = 0;
                                    var totalCalPen = 0;



                                    for (var i = 0; i < pagos.length; i++) {
                                        suma += parseFloat(pagos[i].style_naviero);
                                        suma1 += parseFloat(pagos[i].style_cliente);
                                        sumaPen += parseFloat(pagos[i].style_pendiente);
                                    }

                                    ivaCal = (suma * 0.13).toFixed(2);
                                    totalCal = (parseFloat(suma) + parseFloat(ivaCal)).toFixed(2);

                                    ivaCal1 = (suma1 * 0.13).toFixed(2);
                                    totalCal1 = (parseFloat(suma1) + parseFloat(ivaCal1)).toFixed(2);

                                    ivaCalPen = (sumaPen * 0.13).toFixed(2);
                                    totalCalPen = (parseFloat(sumaPen) + parseFloat(ivaCalPen)).toFixed(2);


                                    //<p id="subTotal1"></p>

                                    $("#myTableModal").append('</tbody>');
                                    $("#myTableModal").append('<tfoot>');
                                    $("#myTableModal").append('<tr><th></th><th></th>'
                                        + '<th style="text-align: right;">Subtotal</th>'
                                        + '<th class="descri"><span class="badge"><p id="subTotal"></p></span></th><th class="descri"><span class="badge"><p id="subTotal1"></p></span></th><th class="descri"><span class="badgeR"><p id="subTotalP"></p></span></th></tr>');
                                    $("#myTableModal").append('<tr><th></th><th></th><th style="text-align: right;">IVA</th>'
                                        + '<th class="descri"><span class="badge"><p id="iva"></p></span></th><th class="descri"><span class="badge"><p id="iva1"></p></span></th><th class="descri"><span class="badgeR"><p id="ivaP"></p></span></th></tr>');
                                    $("#myTableModal").append('<tr><th></th><th></th><th style="text-align: right;">Total</th>'
                                        + '<th class="descri"><span class="badge"><p id="total"></p></span></th><th class="descri"><span class="badge"><p id="total1"></p></span></th><th class="descri"><span class="badgeR"><p id="totalP"></p></span></th></tr>');

                                    $("#myTableModal").append('</tfoot>');


                                    $('#subTotal').text(suma.toFixed(2));
                                    $('#iva').text(ivaCal);
                                    $('#total').text(totalCal);

                                    $('#subTotal1').text(suma1.toFixed(2));
                                    $('#iva1').text(ivaCal1);
                                    $('#total1').text(totalCal1);

                                    $('#subTotalP').text(sumaPen.toFixed(2));
                                    $('#ivaP').text(ivaCalPen);
                                    $('#totalP').text(totalCalPen);

                                    for (var b = 0; b < pagos.length; b++) {
                                        $("#MensajeModal").text(pagos[b].b_danc);

                                        var el = document.getElementById('MensajeModal');
                                        el.style.color = pagos[b].style_dan;



                                        $("#mensajeADUANA").text(pagos[b].b_aduana);

                                        var elt = document.getElementById('mensajeADUANA');
                                        elt.style.color = pagos[b].style_aduana;


                                        $("#f_tarjaM").text(pagos[b].f_tarja);
                                        $("#f_salidaM").text(pagos[b].f_salida);
                                        var frp = validDate();
                                        $("#f_proxima").text(frp);

                                        var valDate = compaDate();

                                        if (valDate) {
                                            $("#f_leyenda").text("Ultima fecha libre de almacenaje fue:");
                                            if ($('#f_proxima').hasClass('label-warning') || $('#f_proxima').hasClass('label-danger')) {
                                                $('#f_proxima').removeClass('label-warning').removeClass('label-danger').addClass('label-danger');
                                                $('#f_leyenda').removeClass('label-warning').removeClass('label-danger').addClass('label-danger');
                                            }

                                        }
                                        else {
                                            $("#f_leyenda").text("Ultima fecha libre de almacenaje es:");
                                            if ($('#f_proxima').hasClass('label-warning') || $('#f_proxima').hasClass('label-danger')) {
                                                $('#f_proxima').removeClass('label-danger').removeClass('label-warning').addClass('label-warning');
                                                $('#f_leyenda').removeClass('label-danger').removeClass('label-warning').addClass('label-warning');
                                            }

                                        }


                                        break;
                                    }

                                    $('#dpRetiro').datetimepicker({
                                        locale: 'es',
                                        format: 'DD/MM/YYYY',
                                        minDate: moment()
                                    });



                                    $("#hTarja").val(tarja);
                                    $("#hContenedor").val(contenedor);
                                    $("#hLlegada").val(c_llegada);
                                    $("#hFtarja").val(f_tarja);
                                    $("#hManifiesto").val(n_manifiesto);
                                    $("#hvPeso").val(v_peso);
                                    $("#hTarjas").val(hTarjas);
                                    $("#hNTarjas").val(con_tarjas);

                                    if (pagos[0].b_salida == "Y") {
                                        $("#calPa").hide();
                                        $('#lblSalida').html("Fecha Retiro Efectiva");

                                        if ($('#f_salidaM').hasClass('label-danger') || $('#f_salidaM').hasClass('label-success')) {
                                            $('#f_salidaM').removeClass('label-danger').removeClass('label-success').addClass('label-success');
                                        }

                                    }
                                    else {
                                        $("#calPa").show();
                                        $('#lblSalida').html("Fecha Próxima Programada");
                                        if ($('#f_salidaM').hasClass('label-danger') || $('#f_salidaM').hasClass('label-success')) {
                                            $('#f_salidaM').removeClass('label-danger').removeClass('label-success').addClass('label-danger');
                                        }

                                    }

                                    if (pagos[0].style_aduana == "#18D318")
                                        $("#btnDetalle").show();
                                    else
                                        $("#btnDetalle").hide();

                                    $('#dpRetiro').on('dp.change', function (e) {

                                        if (e.Date != "undefined" && e.oldDate != null) {


                                            var tr = $("#hTarja").val();
                                            var cn = $("#hContenedor").val();
                                            var lle = $("#hLlegada").val();
                                            var ft = $("#hFtarja").val();
                                            var nm = $("#hManifiesto").val();
                                            var vp = $("#hManifiesto").val();
                                            var fr = new Date(e.date);
                                            var fre = fr.format("dd/MM/yy hh:mm:ss");
                                            var vPeso = $("#hvPeso").val();
                                            var hTarj = $("#hTarjas").val();
                                            var nTar = $("#hNTarjas").val();
                                            validarRetiro(tr, cn, lle, ft, nm, fre, vPeso, hTarj, nTar);


                                        }
                                    });



                                }

                                $.unblockUI();

                                $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                                    "backdrop": "static",
                                    "keyboard": false,
                                    "show": true                     // ensure the modal is shown immediately
                                });
                            }
                            else {
                                $.unblockUI();
                                bootbox.alert(pagos);
                            }
                        },
                        failure: function (response) {
                            bootbox.alert(response.d);
                        },
                        error: function (response) {
                            bootbox.alert(response.d);
                        }
                    });


                }
            }


            function btnCheck(valor) {
                if (valor == 1) {
                    document.getElementById("btnVEA").disabled = false;
                    document.getElementById("btnBancos").disabled = false;
                }
                else {
                    document.getElementById("btnVEA").disabled = true;
                    document.getElementById("btnBancos").disabled = true;
                }
            }




            function shwwindow() {
                //window.open(myurl, '_blank');
                var anio = document.getElementById('<%= a_declaracion.ClientID %>').value;
                var serial = document.getElementById('<%= n_serial.ClientID %>').value;
                var registro = document.getElementById('<%= n_correlativo.ClientID %>').value;
                var url = 'anio=' + anio + '&aduana=02&serial=' + serial + '&registro=' + registro
                window.open('https://aduana2.mh.gob.sv/VEA/free/InfoDm.do?' + url, '_blank');
            }




            var isOkay = true;
            function VerDetalle() {
                if (isOkay) {
                    var params = new Object();

                    params.n_contenedor = $("#hContenedor").val();;
                    params.n_mani = $("#hManifiesto").val();
                    params = JSON.stringify(params);

                    $.ajax({
                        async: true,
                        cache: false,
                        type: "POST",
                        url: "wfTracking.aspx/ObtenerDecla",
                        data: params,
                        contentType: "application/json; charset=utf8",
                        dataType: "json",
                        success: function (response) {
                            var declas = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;


                            if (declas.indexOf("posee") != 0) {

                                $("#myDecla").empty();

                                if (declas.length > 0) {

                                    $("#myDecla").append('<thead><th>Estado</th>'
                                        + '<th>Tipo Documento</th>'
                                        + '<th># Documento</th>');

                                    $("#myDecla").append('<tbody>');

                                    for (var a = 0; a < declas.length; a++) {


                                        $("#myDecla").append('<tr><td>' + declas[a].b_estado + '</td><td>' + declas[a].tipo_doc + '</td><td>' + declas[a].num_doc + '</td><td>'
                                            + '<tr><td>' + declas[a].b_estado + '</td><td class="descri">' + declas[a].tipo_doc + '</td><td>' + declas[a].num_doc + '</td><td>'
                                            + '<tr><td>' + declas[a].b_estado + '</td><td class="descri">' + declas[a].tipo_doc + '</td><td>' + declas[a].num_doc + '</td><td>'
                                            + '<tr><td>' + declas[a].b_estado + '</td><td class="descri">' + declas[a].tipo_doc + '</td><td>' + declas[a].num_doc + '</td><td>');
                                    }


                                    $("#myDecla").append('</tbody>');
                                    $("#myDecla").append('<tfoot>');

                                    $("#myDecla").append('</tfoot>');





                                    $("#myModalD").modal({                    // wire up the actual modal functionality and show the dialog
                                        "backdrop": "static",
                                        "keyboard": false,
                                        "show": true                     // ensure the modal is shown immediately
                                    });



                                }


                            }
                            else {
                                bootbox.alert(declas);
                            }




                        },
                        failure: function (response) {
                            bootbox.alert(response.d);
                        },
                        error: function (response) {
                            bootbox.alert(response.d);
                        }
                    });
                }
                isOkay = !isOkay;
            }

            function currencyFormat(num) {
                return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
            }

            function CaptchaReload() {
                FormValidation.AddOn.reCaptcha2.reset('captchaContainer1');

                $('#myform').formValidation('resetForm', true);
            }


            function btnLimpio() {
                window.href = "wfTracking.aspx";
            };

            function CalculosTabla() {
                var subtotal = 0;
                var ivaCal = 0;
                var totalCal = 0;
                $("#tblProducts tbody tr").each(function () {
                    subtotal = parseFloat($('td:eq(4)', $(this)).text());
                });

                ivaCal = subtotal * 0.13;
                totalCal = subtotal + ivaCal;

                $('#subTotal').html(subtotal);
                $('#iva').html(subtotal);
                $('#total').html(subtotal);

            }

            function HoraActual() {
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd
                }
                if (mm < 10) {
                    mm = '0' + mm
                }

                var hh = today.getHours();
                var mi = today.getMinutes();
                var ss = today.getSeconds();

                if (hh < 10) {
                    hh = '0' + hh
                }

                if (mi < 10) {
                    mi = '0' + mi
                }

                if (ss < 10) {
                    ss = '0' + ss
                }


                var today = dd + '/' + mm + '/' + yyyy + ' ' + hh + ':' + mi + ':' + ss;

                return today;
            }

            function compaDate() {
                var tt1 = $("#f_proxima").text();

                var tt = tt1.split("/");



                var date = new Date(tt[1] + '/' + tt[0] + '/' + tt[2]);
                var newdate = new Date(date);

                var hoy = new Date();

                if (hoy > newdate)
                    var so = true
                else
                    var so = false


                return so;
            }

            function pageLoad() {
                $(document).ready(function () {

                    $("#myOK").click(function () {
                        $("#myModal").modal('hide');
                    });

                    $('#grvTracking_dtTracking_0_grvProv img#impProvi').click(function () {
                        var imgPro = $(this)
                        var idDetaPro = $(this).attr('iddetap');

                        var trPro = $('#grvTracking_dtTracking_0_grvProv tr[iddetap =' + idDetaPro + ']')
                        trPro.toggle();

                        //if (img[0].src == 'CSS/Images/plus.gif')
                        //    img.attr('src', 'CSS/Images/minus.gif');
                        //else
                        //    img.attr('src', 'CSS/Images/plus.gif');

                        if (trPro.is(':visible'))
                            imgPro.attr('src', 'vendor/bootstrap/Images/minus.gif');
                        else
                            imgPro.attr('src', 'vendor/bootstrap/Images/plus.gif');

                    });




                    $('#<%=grvTracking.ClientID %> img').click(function () {

                        var img = $(this)
                        var idDeta = $(this).attr('iddeta');

                        var tr = $('#<%=grvTracking.ClientID %> tr[iddeta =' + idDeta + ']')
                        tr.toggle();

                        if (idDeta > 0) {

                            if (tr.is(':visible'))
                                img.attr('src', 'vendor/bootstrap/Images/minus.gif');
                            else
                                img.attr('src', 'vendor/bootstrap/Images/plus.gif');
                        }
                    });

                    $("#exportpdf").click(function () {
                        var HTMLContent = document.getElementById('printArea');
                        if (HTMLContent.innerText != "") {
                            var Popup = window.open('about:blank', 'printArea', 'width=700px,height=500');
                            var d = HoraActual();
                            var linkC = '<link href="bootstrap/csss/bootstrap.min.css" rel="stylesheet">' +
                                '<link href="bootstrap/csss/bootstrap-select.css" rel="stylesheet">' +
                                '<link href="bootstrap/csss/todc-bootstrap.min.css" rel="stylesheet">' +
                                '<link href="bootstrap/csss/footable.metro.css" rel="stylesheet" type="text/css">' +
                                '<link href="bootstrap/csss/footable.core.css" rel="stylesheet" type="text/css">' +
                                '<link href="bootstrap/csss/footable-demos.css" rel="stylesheet" type="text/css">';

                            var styleC = 'table#grvTracking_dtTracking_0_grvProv.footable tbody > tr > th { background-color: #1771F8; border: 1px solid #1771F8; color: #ffffff; border-top: none; border-left: none; font-weight: normal; text-align: center;font-size:12px}';
                            var styleC1 = '.table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th { padding: 2.5px;vertical-align: top;border-top: 1px solid #ddd;border: inset 0pt;}';
                            var styleC2 = 'table#grvTracking_dtTracking.footable tbody > tr > th { background-color: #1771F8;border: 1px solid #1771F8;color: #ffffff;border-top: none;border-left: none;font-weight: normal;text-align: center;font-size:12px;}';
                            var styleC3 = 'body{width:80%;}.footable > tbody > tr > td {border-top: 1px solid #dddddd;padding: 10px;text-align: center;border-left: none;font-size: 12px;}';
                            var styleC4 = '.footable-visible.footable-last-column {border-right: 3px solid #1771F8;}';
                            var styleC5 = 'tr#rowF{background-color: rgb(23, 113, 248);}';
                            var styleC6 = '.footable > tbody > tr > td { border-top: 1px solid #dddddd;padding: 10px;text-align: center;border-left: none;border-top: none;border-right: none;border-bottom: none;}';
                            var styleC7 = '.footable > thead > tr > th { border-bottom: 1px solid #dddddd;padding: 10px;text-align: left;font-size: 10.5px;}';
                            var styleC8 = ' .footable > thead > tr > th, .footable > thead > tr > td { background-color: #1771F8;border: 1px solid #1771F8;color: #ffffff;border-top: none;border-left: none;font-weight: bold;text-align: center;}';
                            var styleC9 = 'div.dataTables_wrapper { width: 100%;margin: 0 auto; }';
                            var styleC10 = '.table-hover > tbody > tr:hover { background-color: #CECEF6 }';
                            var styleC11 = 'table.table-bordered th:last-child, table.table-bordered td:last-child { border-right: 1px solid #1771f8; }';
                            var styleC12 = 'table.dataTable thead > tr > th { padding-left: 9px;padding-right: 18px;background-color: #1771F8;text-align:center;color: #fff;font-weight: bold;border: hidden;}';
                            var styleC13 = '.table-bordered > tbody > tr > td, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > td, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > thead > tr > th { border: none; }';
                            var styleC14 = '.table-bordered { border: 5px solid #1771F8; }';
                            var styleC15 = '.alignCells { text-transform: capitalize; }';
                            var styleC16 = '.dataTables_filter, .dataTables_info, .dataTables_paginate, .dataTables_length { display: none; }';
                            var styleC17 = '#grvTracking_dtTracking_0_grvProv tbody tr th { color: white; background-color: #1771F8; text-align: center; border: 1px solid #1771F8; }';

                            var javaScript1 = '<script type="text/javascript">' + '<br>' +
                                '$(function () { ' + '<br>' +
                                '$("[id*=btnExport]").click(function () { ' + '<br>' +
                                '$("[id*=hfGridHtml]").val($("#Grid").html());' + '<br>' +
                                '});' + '<br>' +
                                '});' + '<br>';


                            var scriptC = '<\%@ Page Language="C#" %>' + '<br>' +
                                '<\%@ Import Namespace="System" %>' + '<br>' +
                                '<\%@ Import Namespace="System.Collections.Generic" %>' + '<br>' +
                                '<\%@ Import Namespace="System.Linq" %>' + '<br>' +
                                '<\%@ Import Namespace="System.Web" %>' + '<br>' +
                                '<\%@ Import Namespace="System.Web.UI" %>' + '<br>' +
                                '<\%@ Import Namespace="System.Web.UI.WebControls" %>' + '<br>' +
                                '<\%@ Import Namespace="System.IO" %>' + '<br>' +
                                '<\%@ Import Namespace="iTextSharp.text" %>' + '<br>' +
                                '<\%@ Import Namespace="iTextSharp.text.html.simpleparser" %>' + '<br>' +
                                '<\%@ Import Namespace="iTextSharp.text.pdf" %>' + '<br>' +
                                '<\%@ Import Namespace="iTextSharp.tool.xml" %>' + '<br>';

                            var scriptC1 = 'public void ExportToPDF(object sender, EventArgs e)' + '<br>' +
                                '{' + '<br>' +
                                'StringReader sr = new StringReader(Request.Form[hfGridHtml.UniqueID]);' + '<br>' +
                                'Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);' + '<br>' +
                                'PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);' + '<br>' +
                                'pdfDoc.Open();' + '<br>' +
                                'XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);' + '<br>' +
                                'pdfDoc.Close();' + '<br>' +
                                'Response.ContentType = "application/pdf";' + '<br>' +
                                'Response.AddHeader("content-disposition", "attachment;filename=HTML.pdf");' + '<br>' +
                                'Response.Cache.SetCacheability(HttpCacheability.NoCache);' + '<br>' +
                                'Response.Write(pdfDoc);' + '<br>' +
                                'Response.End();' + '<br>' +
                                '}' + '<br>';

                            //var scriptCN = scriptC.replace('\\', '');



                            var content = '<div class="container-fluid" style="padding-top: 10px;padding-bottom: 10px;"><div class="row" style="margin-right: -439px;font-weight: bold;font-size: 16px;"><div class="col-md-6">Tracking Contenedor de Importación</div><div class="col-md-6">Fecha/Hora: ' + d + '</div></div></div>';

                            Popup.document.writeln('<html>');
                            //Popup.document.writeln(scriptCN);
                            Popup.document.writeln('<head>');
                            Popup.document.writeln(linkC.toString());
                            Popup.document.writeln('<style type="text/css">');
                            Popup.document.writeln('body{font-family: Trebuchet MS;}');
                            Popup.document.writeln('.no-print{display: none;}');
                            Popup.document.writeln(styleC.toString());
                            Popup.document.writeln(styleC1.toString());
                            Popup.document.writeln(styleC2.toString());
                            Popup.document.writeln(styleC3.toString());
                            Popup.document.writeln(styleC4.toString());
                            Popup.document.writeln(styleC5.toString());
                            Popup.document.writeln(styleC6.toString());
                            Popup.document.writeln(styleC7.toString());
                            Popup.document.writeln(styleC8.toString());
                            Popup.document.writeln(styleC9.toString());
                            Popup.document.writeln(styleC10.toString());
                            Popup.document.writeln(styleC11.toString());
                            Popup.document.writeln(styleC12.toString());
                            Popup.document.writeln(styleC13.toString());
                            Popup.document.writeln(styleC14.toString());
                            Popup.document.writeln(styleC15.toString());
                            Popup.document.writeln(styleC16.toString());
                            Popup.document.writeln(styleC17.toString());
                            Popup.document.writeln('</style>');
                            Popup.document.writeln('</head><body>');
                            //Popup.document.writeln('<asp:Button ID="btnExport" runat="server" CssClass="btn btn-success" Text="Imprimir"  />');
                            Popup.document.writeln('<input type="button" value="Imprimir" onclick="javascript:window.print()" />');
                            Popup.document.writeln('<div id="Grid">');
                            Popup.document.writeln(content.toString());
                            Popup.document.writeln(HTMLContent.innerHTML);
                            Popup.document.writeln('</div>');
                            Popup.document.writeln('<asp:HiddenField ID = "hfGridHtml" runat = "server" />');
                            Popup.document.writeln(javaScript1);
                            Popup.document.writeln('<//script>');
                            Popup.document.writeln('</body></html>');
                            Popup.document.close();
                            Popup.focus();
                            //OnClick="ExportToPDF"
                        }
                        else {
                            bootbox.alert("Realice una consulta, no se poseen valores que imprimir");
                        }
                    });


                    $("[id*=grvTracking]").DataTable({
                        paging: true,
                        lengthChange: false,
                        searching: false,
                        ordering: true,
                        info: true,
                        autoWidth: false,
                        scrollX: true
                    });



                    CalculosTabla();
                    $("#grvTracking tbody > tr#rowF").css("display", "none");


                    $("#myOKD").click(function () {
                        $("#myModalD").modal('hide');
                    });

                    $('#btnVEA').tooltip();
                    $('#btnVEA').tooltip();
                    $('#linkManual').tooltip();



                    //$('#<%=grvTracking.ClientID %> button').tooltip();

                    //$('#dpRetiro').tooltip();



                });
            }
        </script>

    </form>

</body>
</html>

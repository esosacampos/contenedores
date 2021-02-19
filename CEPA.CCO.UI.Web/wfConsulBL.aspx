<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsulBL.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsulBL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/datatables.min.css" rel="stylesheet" />

    <style type="text/css">
        .scrollme {
            /*width: 100%;
            height: 370px;
            overflow: scroll;*/
        }

        body {
            /*overflow: hidden;*/
        }

        div#grvFacturas_wrapper .dataTables_filter {
            float: right;
            text-align: right;
            visibility: hidden;
        }

        /*div#grvFacturas_wrapper > thead > tr.heading > th:nth-child(3) { width: 120px; min-width: 120px; }*/

        .dataTables_wrapper {
            text-align: center;
            vertical-align: middle;
            white-space: nowrap;
            margin-top: 30px;
            font-size: 0.75em;
            line-height: 1.5;
        }

        table.dataTable.compact tbody th, table.dataTable.compact tbody td {
            padding: 3px;
        }

        .totalFact {
            text-align: center;
        }

        table.dataTable.compact thead th, table.dataTable.compact thead td {
            /*padding: 4px 13px 3px 3px;*/
            padding: 2px;
            vertical-align: middle;
            /*background-color: #708090;
            color: #F5FFFA;*/
            text-align: center;
        }

        tr#cabeMain {
            background-color: #fff3cd;
            color: #856404;
        }



        table.dataTable.stripe tbody tr.odd, table.dataTable.display tbody tr.odd {
            background-color: #e3efff;
            color: #708090;
        }

        table.dataTable.stripe tbody tr.even, table.dataTable.display tbody tr.even {
            color: #708090;
        }

        table.dataTable thead th, table.dataTable thead td {
            border-bottom: 1px solid #111;
        }

        table.dataTable.cell-border tbody tr th:first-child, table.dataTable.cell-border tbody tr td:first-child {
            border-left: 1px solid #111;
        }

        table.dataTable.cell-border tbody tr th:first-child, table.dataTable.cell-border tbody tr td:last-child {
            border-right: 1px solid #111;
        }

        table#GridView1 tbody tr td.clsRTr.nav {
            border-left: 1px solid #111;
        }

        table#GridView1 tbody tr td.clsRTrP {
            border-right: 1px solid #111;
        }

        table#GridView1 tbody tr td.clsRDeP {
            border-right: 1px solid #111;
        }

        table#GridView1 tbody tr td.clsRMaP {
            border-right: 1px solid #111;
        }

        .clsTra {
            background-color: #FFFF;
            /*border: 1px solid #ddd;*/
            color: #708090;
        }

        .clsDes {
            background-color: #9aa7b5;
            /*border: 1px solid #ddd;*/
            color: #FFFF;
        }

        .clsMan {
            background-color: #FFFF;
            /*border: 1px solid #ddd;*/
            color: #708090;
        }

        .clsAlm {
            background-color: #9aa7b5;
            /*border: 1px solid #ddd;*/
            color: #FFFF;
        }

        .clsRTrP {
            background-color: #FFFF;
            font-weight: bold;
            color: #DC143C;
        }

        .clsRTr {
            background-color: #FFFF;
            color: #708090;
        }

        .clsRDeP {
            background-color: #9aa7b5;
            font-weight: bold;
            color: #FFFF;
        }

        .clsRDe {
            background-color: #9aa7b5;
            color: #FFFF;
        }

        .clsRMaP {
            background-color: #FFFF;
            font-weight: bold;
            color: #DC143C;
        }

        .clsRMa {
            background-color: #FFFF;
            color: #708090;
        }

        .clsRAlP {
            background-color: #9aa7b5;
            font-weight: bold;
            color: #FFFF;
        }

        .clsRAl {
            background-color: #9aa7b5;
            color: #FFFF;
        }

        .totalTra, .totalDes, .totalMan {
            text-align: center;
            vertical-align: middle;
        }

        #naviero, #cliente {
            float: right;
            width: 250px;
            height: 80px;
        }

        #detaClien tbody tr td, #detaNavi tbody tr td {
            vertical-align: middle;
            font-weight: bold;
        }

        #detaClien tr:nth-child(even) {
            background: #FFFAFA;
        }

        #detaNavi tr:nth-child(even) {
            background: #FFFAFA;
        }

        div#grvFacturas_wrapper {
            margin: 0 auto;
            width: auto;
            clear: both;
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }

        div#grvFacturas_wrapper { /*Esto para que acepte el width*/
            table-layout: fixed;
            /*Muy Importante para que cuadre la medida*/
            border-spacing: 0;
            width: 100%;
        }

            div#grvFacturas_wrapper td {
                /*Esto por el desbordamiento del texto*/
                word-break: break-all;
                text-align: center;
                padding: 2px;
            }

            div#grvFacturas_wrapper th {
                padding: 2px;
            }

        .table-responsive {
            min-height: .01%;
        }

        tr#cabeMain th.clsRTr {
            background-color: #fff3cd;
            color: #856404;
        }

        .dataTables_wrapper thead tr th.clsTra {
            background-color: #fff3cd;
            color: #856404;
            border: 1px solid #111;
        }

        tr#cabeMain th.clsRDe {
            background-color: #fff3cd;
            color: #856404;
        }

        .dataTables_wrapper thead tr th.clsDes {
            background-color: #fff3cd;
            color: #856404;
            border: 1px solid #111;
        }

        tr#cabeMain th.clsRMa {
            background-color: #fff3cd;
            color: #856404;
        }

        .dataTables_wrapper thead tr th.clsMan {
            background-color: #fff3cd;
            color: #856404;
            border: 1px solid #111;
        }

        tr#cabeMain th.clsRAl {
            background-color: #fff3cd;
            color: #856404;
        }

        .dataTables_wrapper thead tr th.clsAlm {
            background-color: #fff3cd;
            color: #856404;
            border: 1px solid #111;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>CONSULTA POR BL
    </h2>
    <br />
    <div class="row" style="margin-left: 100px; margin-bottom: 10px;">
        <div class="table-responsive">
            <table class="table">
                <tr>
                    <td>BL / # MANIFIESTO
                    </td>
                    <td>
                        <span class="c_bl" style="margin-left: 55px;"></span>
                    </td>
                </tr>
                <tr>
                    <td>TARJA
                    </td>
                    <td>
                        <span class="c_tarja" style="margin-left: 55px;"></span>
                    </td>
                </tr>
                <tr>
                    <td>TIPO TRAFICO
                    </td>
                    <td>
                        <span class="c_trafico" style="margin-left: 55px;"></span>
                    </td>
                </tr>
                <tr>
                    <td>FECHA DE INGRESO
                    </td>
                    <td>
                        <span class="f_ingreso" style="margin-left: 55px;"></span>
                    </td>
                </tr>
                <tr>
                    <td>ULTIMA FECHA LIBRE DE ALMACENAJE
                    </td>
                    <td>
                        <span class="f_libre" style="margin-left: 55px;"></span>
                    </td>
                </tr>
                <tr>
                    <td>ÚLTIMO RETIRO</td>
                    <td><span class="ultRetiro" style="margin-left: 55px;"></span></td>
                </tr>
                <tr>
                    <td>FECHA PROXIMO RETIRO</td>
                    <td>
                        <div role="form">
                            <div class="form-inline">
                                <div class="form-group" style="width: 50%; margin-left: 55px;">
                                    <div class="input-group date" id="datetimepicker2" style="width: 100%;">
                                        <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
                                        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-success" />
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-lg-3" style="padding-right: 1px;">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                    </div>
                    <div class="form-group" style="width: 82%;">
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-9" style="padding-left: 5px;">
            <div role="form">
                <div class="form-inline">
                    <%--<div class="form-group">
                        <span style="font-weight: bold;">Último Retiro:</span>
                    </div>
                    <div class="form-group" style="width: 50%;">
                        <span class="ultRetiro" style="margin-left: 55px;"></span>
                    </div>
                    <div class="form-group">
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-left: 100px; margin-bottom: 10px;">
        <div class="col-lg-3" style="padding-right: 1px; display: none;">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <span style="font-weight: bold;"># BL :</span>
                    </div>
                    <div class="form-group" style="width: 82%; display: none;">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Ingrese # de BL a consultar" autocomplete="off" Width="100%"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-9" style="padding-left: 5px;">
            <div role="form">
                <div class="form-inline">
                    <%--<div class="form-group">
                        <span style="font-weight: bold;">Fecha Próximo Retiro:</span>
                    </div>
                    <div class="form-group" style="width: 50%;">
                        <div class="input-group date" id="datetimepicker2" style="width: 100%;">
                            <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>--%>
                    <%--<div class="form-group">
                        <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-success" />
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
    <%--class="display compact cell-border row-border"--%>
    <table id="GridView1" class="display compact cell-border row-border" style="width: 100%;">
    </table>
    <div style="float: left; width: 50%; height: 80px;">
        <label style="font-size: 11px; margin-top: 5%; margin-bottom: 5px;" id="cadena"></label>
        <label style="font-size: 11px" id="ultFecha"></label>
    </div>
    <div id="naviero">
        <table id="detaNavi" class="table-bordered table-condensed" style="width: 100%">
        </table>
    </div>
    <div id="cliente">
        <table id="detaClien" class="table-bordered table-condensed" style="width: 100%">
        </table>
    </div>
    <h3 style="float: left; width: 50%; height: 10px;"></h3>
    <div id="datatablestest" style="width: 80%; margin: 0 auto;">
        <table id="grvFacturas" class="display compact cell-border row-border" style="width: 100%;">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/moment.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.number.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/datatables.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/bootstrap/js/dataTables.rowsGroup.js") %>"></script>


    <script type="text/javascript">      

        var d = new Date();

        function getDateDiff(time1, time2) {
            var str1 = time1.split('/');
            var str2 = time2.split('/');

            //                yyyy   , mm       , dd
            var t1 = new Date(str1[2], str1[1] - 1, str1[0]);
            var t2 = new Date(str2[2], str2[1] - 1, str2[0]);

            var diffMS = t1 - t2;
            //console.log(diffMS + ' ms');

            var diffS = diffMS / 1000;
            //console.log(diffS + ' ');

            var diffM = diffS / 60;
            //console.log(diffM + ' minutes');

            var diffH = diffM / 60;
            //console.log(diffH + ' hours');

            var diffD = diffH / 24;
            //console.log(diffD + ' days');
            return diffD;
        }

        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }

        // inicio
        function getFact(c_tarja) {
            if (c_tarja.length > 0) {
                //e.preventDefault();
                var params = new Object();
                params.c_tarja = c_tarja;
                params = JSON.stringify(params);

                var row = "";
                $.ajax({
                    type: "POST",
                    url: "wfConsulBL.aspx/getFact",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    async: "true",
                    cache: "false",
                    success: function (msg) {
                        $('#grvFacturas').empty();
                        var contF = 0;
                        var pagos = (typeof msg.d) == "string" ? eval('(' + msg.d + ')') : JSON.parse(msg.d);
                        if (pagos.length > 0) {
                            $('#grvFacturas tbody').empty();

                            $("h3").text("Detalle de Factura(s):");

                            $('#grvFacturas').append('<thead><tr style="background-color: #fff3cd;color: #866404;">'
                                + '<th>T. FACTURADO</th>'
                                + '<th># FACTURA</th>'
                                + '<th>PRE IMPRESO</th>'
                                + '<th>F. FACTURACION</th>'
                                + '<th>DETALLE</th>'
                                + '</tr></thead>');

                            $('#grvFacturas').append('<tbody>');

                            $.each(JSON.parse(msg.d), function (i, v) {
                                row += "<tr><td class='clsFact'>" + ($.number(v.t_factura, 2, '.', ',') == '0.00' ? '' : $.number(v.t_factura, 2, '.', ',')) + "</td><td>" + v.c_factura + "</td><td>" + v.c_preimpreso + "</td><td>" + v.fa_factura + "</td><td>" + v.s_detalle + "</td></tr>";
                            });

                            $("#grvFacturas").append(row);

                            $('#grvFacturas').append('<tfoot>'
                                + '<tr>'
                                + '<th class="totalFact"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '</tr>'
                                + '</tfoot>');

                            var sFact = 0.00;

                            $('#grvFacturas tr').each(function () {

                                $(this).find('.clsFact').each(function () {
                                    var prue = $(this).text();
                                    var combat = parseFloat($(this).text().replace(',', ''));
                                    if (!isNaN(combat) && combat.length !== 0) {
                                        sFact += parseFloat(combat);
                                    }
                                });
                                $('.totalFact', this).html('$ ' + $.number(sFact, 2, '.', ','));
                            });


                            var table = $('#grvFacturas').DataTable({
                                destroy: true,
                                autoWidth: false,
                                scrollY: 400,
                                scrollX: true,
                                scrollCollapse: true,
                                paging: false,
                                ordering: false,
                                columnDefs: [
                                    { width: "10px", targets: 3 }
                                ],
                                //fixedColumns: true,
                                //columns: [
                                //    { "data": "v_peso", render: $.fn.dataTable.render.number('.', ',', 2, '') }                                        
                                //],
                                language:
                                {
                                    "sProcessing": "Procesando...",
                                    "sLengthMenu": "Mostrar _MENU_ registros",
                                    "sZeroRecords": "No se encontraron facturas asociadas a esta tarja",
                                    "sEmptyTable": "No se encontraron facturas asociadas a esta tarja",
                                    "sInfo": "Registros del _START_ al _END_ de ( _TOTAL_ ) registros",
                                    "sInfoEmpty": "Registros del 0 al 0 de 0 registros",
                                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                                    "sInfoPostFix": "",
                                    "sUrl": "",
                                    "decimal": ".",
                                    "thousands": ",",
                                    "sLoadingRecords": "Cargando...",
                                    "oPaginate": {
                                        "sFirst": "Primero",
                                        "sLast": "Último",
                                        "sNext": "Siguiente",
                                        "sPrevious": "Anterior"
                                    },
                                    "oAria": {
                                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                                    }
                                },
                                fixedColumns: {
                                    leftColumns: 1
                                },
                                footerCallback: function (row, data, start, end, display) {

                                    //total = this.api()
                                    //    .column(1)//numero de columna a sumar
                                    //    //.column(1, {page: 'current'})//para sumar solo la pagina actual
                                    //    .data()
                                    //    .reduce(function (a, b) {
                                    //        return parseFloat(a) + parseFloat(b);
                                    //    }, 0);

                                    //$(this.api().column(1).footer()).html('$ ' + $.number(total, 2, '.', ','));

                                }
                            });

                            table.columns.adjust().draw();
                        }
                    },
                    error: function (msg) {
                        bootbox.alert(msg.d);
                    }
                });
            }
        }
        // fin
        function getBL(llegada, contenedor) {
            if (llegada.length > 0 && contenedor.length > 0) {
                //e.preventDefault();
                var params = new Object();
                params.c_llegada = llegada;
                params.n_contenedor = contenedor;
                params = JSON.stringify(params);


                $.blockUI({
                    message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
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



                var row = "";
                $.ajax({
                    type: "POST",
                    url: "wfConsulBL.aspx/getBLs",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    async: "true",
                    cache: "false",
                    success: function (msg) {
                        $('#GridView1').empty();
                        var contF = 0;
                        var pagos = (typeof msg.d) == "string" ? eval('(' + msg.d + ')') : JSON.parse(msg.d);
                        if (pagos.length > 0) {
                            $('#GridView1 tbody').empty();
                            $('#detaNavi tbody').empty();
                            $('#detaClien tbody').empty();

                            $('.c_bl').html(pagos[0].n_BL + " / " + pagos[0].n_manifiesto);
                            $('.c_tarja').html(pagos[0].c_tarja + " - " + pagos[0].f_tarja);
                            $('.c_trafico').html(pagos[0].c_trafico);

                            $('.f_ingreso').html(pagos[0].f_tarja);

                            var ultRetirado = moment(pagos[0].f_tarja, "DD/MM/YYYY").add(4, 'd').format("DD/MM/YYYY");
                            $('.f_libre').html(ultRetirado);


                            $('#GridView1').append('<thead><tr>'
                                + '<th colspan="7"></th>'
                                + '<th class="clsTra" colspan="3">TRANSFERENCIA</th>'
                                + '<th class="clsDes" colspan="3" style="border-left:0;">DESPACHO</th>'
                                + '<th class="clsMan" colspan="3" style="border-left:0;">MANEJO</th>'
                                + '<th class="clsAlm" colspan="3" style="border-left:0;">ALMACENAJE</th>'
                                + '</tr>'
                                + '<tr id="cabeMain">'
                                + '<th style="border-left: 1px solid #111;">CONTENEDOR</th>'
                                + '<th>TAMAÑO</th>'
                                + '<th>TEUS</th>'
                                + '<th>PESO (Kg.)</th>'
                                + '<th class="clsFSal">F. SALIDA</th>'
                                + '<th>T. RETENCION</th>'
                                + '<th>F. RETENCION</th>'
                                + '<th class="clsRTr" style="border-left: 1px solid #111;">NAVIERO</th>'
                                + '<th class="clsRTr">CLIENTE</th>'
                                + '<th class="clsRTr">PENDIENTE</th>'
                                + '<th class="clsRDe" style="border-left: 1px solid #111;">NAVIERO</th>'
                                + '<th class="clsRDe">CLIENTE</th>'
                                + '<th class="clsRDe">PENDIENTE</th>'
                                + '<th class="clsRMa" style="border-left: 1px solid #111;">NAVIERO</th>'
                                + '<th class="clsRMa">CLIENTE</th>'
                                + '<th class="clsRMa">PENDIENTE</th>'
                                + '<th class="clsRAl" style="border-left: 1px solid #111;">NAVIERO</th>'
                                + '<th class="clsRAl">CLIENTE</th>'
                                + '<th class="clsRAl" style="border-right: 1px solid #111;">PENDIENTE</th>'
                                + '</thead>');

                            $('#GridView1').append('<tbody>');



                            var f_ultima = null;

                            f_ultima = pagos[0].f_salidas;
                            var peso_despachado = 0.00;

                            $.each(JSON.parse(msg.d), function (i, v) {
                                peso_despachado += v.peso_entregado;
                                if ((v.f_salidas.length > 0 && v.f_salidas != '01/01/1900 00:00') || v.b_cancelado == 'Y') {
                                    row += "<tr style='background-color:#d4edda;color:#155724;border-color:#c3e6cb;'><td>" + v.n_contenedor + "</td><td>" + v.c_tamaño + "</td><td>" + v.v_teus + "</td><td class='pesoKg'>" + ($.number(v.v_peso, 2, '.', ',') == '0.00' ? '' : $.number(v.v_peso, 2, '.', ',')) + "</td><td>" + (v.f_salidas == '01/01/1900 00:00' ? '' : v.f_salidas) + "</td><td>" + v.t_retencion + "</td><td>" + "" + "</td><td class='clsRTr nav'>" + v.n_transfer + "</td><td class='clsRTr cli'>" + v.c_transfer + "</td><td class='clsRTrP'>" + ((v.p_transfer == '0' || f_ultima.length > 0) ? '' : v.p_transfer) + "</td><td class='clsRDe nav'>" + v.n_desp + "</td><td class='clsRDe cli'>" + v.c_desp + "</td><td class='clsRDeP'>" + ((v.p_desp == '0' || f_ultima.length > 0) ? '' : v.p_desp) + "</td><td class='clsRMa nav'>" + v.n_manejo + "</td><td class='clsRMa cli'>" + v.c_manejo + "</td><td class='clsRMaP'>" + ((v.p_manejo == '0' || f_ultima.length > 0) ? '' : $.number(v.p_manejo, 2, '.', ',')) + "</td><td class='clsRAl nav'>" + v.n_alm + "</td><td class='clsRAl cli'>" + v.c_alm + "</td><td class='clsRAlP'></td></tr>";
                                }
                                else {
                                    contF += 1;
                                    row += "<tr><td>" + v.n_contenedor + "</td><td>" + v.c_tamaño + "</td><td>" + v.v_teus + "</td><td class='pesoKg'>" + ($.number(v.v_peso, 2, '.', ',') == '0.00' ? '' : $.number(v.v_peso, 2, '.', ',')) + "</td><td>" + (v.f_salidas == '01/01/1900 00:00' ? '' : v.f_salidas) + "</td><td>" + v.t_retencion + "</td><td>" + "" + "</td><td class='clsRTr nav'>" + v.n_transfer + "</td><td class='clsRTr cli'>" + v.c_transfer + "</td><td class='clsRTrP'>" + (v.p_transfer == 0 ? '' : '$ ' + $.number(v.p_transfer, 2, '.', ',')) + "</td><td class='clsRDe nav'>" + v.n_desp + "</td><td class='clsRDe cli'>" + v.c_desp + "</td><td class='clsRDeP'>" + (v.p_desp == 0 ? '' : '$ ' + $.number(v.p_desp, 2, '.', ',')) + "</td><td class='clsRMa nav'>" + v.n_manejo + "</td><td class='clsRMa cli'>" + v.c_manejo + "</td><td class='clsRMaP'>" + (v.p_manejo == 0 ? '' : '$ ' + $.number(v.p_manejo, 2, '.', ',')) + "</td><td class='clsRAl nav'>" + v.n_alm + "</td><td class='clsRAl cli'>" + v.c_alm + "</td><td class='clsRAlP'></td></tr>";
                                }

                            });


                            $('.ultRetiro').html(f_ultima == null || f_ultima == '01/01/1900 00:00' ? '' : f_ultima);



                            if (contF == 0) {
                                $("#ContentPlaceHolder1_txtDOB").prop('disabled', true);
                                $("#ContentPlaceHolder1_btnBuscar").prop('disabled', true);
                            }

                            //$('#datetimepicker2').datepicker('disable');



                            $("#GridView1").append(row);



                            $('#GridView1').append('<tfoot>'
                                + '<tr>'
                                + '<th class="totConte"></th>'
                                + '<th></th>'
                                + '<th style="text-align: right;padding:0px;">Total (Kg.)</th>'
                                + '<th class="totalPeso"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th class="totalTra"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th class="totalDes"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th class="totalMan"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th class="totalAlm"></th>'
                                + '</tr>'
                                + '<tr>'
                                + '<th colspan="3" style="text-align: right;">TM</th>'
                                + '<th class="tm"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th class="totalTra1"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th class="totalTra2"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th class="totalTra3"></th>'
                                + '<th></th>'
                                + '<th></th>'
                                + '<th class="totalTra4"></th>'
                                + '</tr>'
                                + '</tfoot>');

                            var sumT = 0;
                            var sumD = 0;
                            var sumM = 0;
                            var sumTe = 0;
                            var SumPe = 0;

                            var traNavi = "";
                            var desNavi = "";
                            var manNavi = "";
                            var almNavi = "";

                            var traClie = "";
                            var desClie = "";
                            var manClie = "";
                            var almClie = "";

                            var subTotalNavi = 0.00;
                            var subTotalClie = 0.00;




                            $('#GridView1 tr').each(function () {

                                $('.totConte', this).html('Cant. Contenedores: ' + pagos.length);

                                $(this).find('.clsRTrP').each(function () {
                                    var combat = parseFloat($(this).text().replace('$', '').replace(' ', ''));
                                    if (!isNaN(combat) && combat.length !== 0) {
                                        sumT += parseFloat(combat);
                                    }
                                });
                                $('.totalTra', this).html('$ ' + $.number(sumT, 2, '.', ','));


                                $(this).find('.clsRTr.nav').each(function () {
                                    if ($(this).text() == "X") {
                                        traNavi = "X";
                                        traClie = "";
                                    } else {
                                        traClie = "X";
                                        traNavi = "";
                                    }
                                });



                                $(this).find('.clsRDeP').each(function () {
                                    var combat = parseFloat($(this).text().replace('$', '').replace(' ', '').replace('0', ''));
                                    if (!isNaN(combat) && combat.length !== 0) {
                                        sumD += parseFloat(combat);
                                    }
                                });
                                $('.totalDes', this).html('$ ' + $.number(sumD, 2, '.', ','));

                                $(this).find('.clsRDe.nav').each(function () {
                                    if ($(this).text() == "X") {
                                        desNavi = "X";
                                        desClie = "";
                                    } else {
                                        desClie = "X";
                                        desNavi = "";
                                    }
                                });


                                $(this).find('.clsRMaP').each(function () {
                                    var combat = parseFloat($(this).text().replace('$', '').replace(' ', ''));
                                    if (!isNaN(combat) && combat.length !== 0) {
                                        sumM += parseFloat(combat);
                                    }
                                });
                                $('.totalMan', this).html('$ ' + $.number(sumM, 2, '.', ','));


                                $(this).find('.clsRMa.nav').each(function () {
                                    if ($(this).text() == "X") {
                                        manNavi = "X";
                                        manClie = "";
                                    } else {
                                        manClie = "X";
                                        manNavi = "";
                                    }
                                });

                                $(this).find('.pesoKg').each(function () {
                                    var combat1 = parseFloat($(this).text().replace(',', ''));
                                    if (!isNaN(combat1) && combat1.length !== 0) {
                                        SumPe += parseFloat(combat1);
                                    }
                                });

                                if (peso_despachado > 0.00)
                                    $('.totalPeso', this).html($.number((SumPe - peso_despachado), 2, '.', ','));
                                else
                                    $('.totalPeso', this).html($.number(SumPe, 2, '.', ','));

                                $(this).find('.totalPeso').each(function () {
                                    var combat = parseFloat($(this).text().replace(',', ''));
                                    if (!isNaN(combat) && combat.length !== 0) {
                                        sumTe = parseFloat(combat) / 1000;
                                    }
                                });
                                $('.tm', this).html($.number(sumTe, 2, '.', ','));


                            });

                            var uli;

                            var d = moment($('#ContentPlaceHolder1_txtDOB').val(), "DD/MM/YYYY");


                            $("#ultFecha").text("Ultima fecha de retiro libre de almacenaje: " + ultRetirado);


                            var ul = null;

                            var jj = moment(f_ultima, 'DD/MM/YYYY');
                            var aa = jj.format("MM-DD-YYYY");


                            if (f_ultima == null || f_ultima == "01/01/1900 00:00")
                                ul = moment(pagos[0].f_tarja, "DD/MM/YYYY").format("DD/MM/YYYY");
                            else
                                ul = moment(f_ultima, "DD/MM/YYYY").format("DD/MM/YYYY");

                            if (ul > ultRetirado)
                                ul = ul;
                            else
                                ul = ultRetirado;

                            d = moment(d, "DD/MM/YYYY").format("DD/MM/YYYY");

                            if (d == ul) {
                                if (contF > 0) {
                                    var dias = 0;

                                    var total_Alm = 0.00;
                                    val_Alm = pagos[0].ta_alm;

                                    var dAn = 0;

                                    //if (f_ultima == null || f_ultima == "01/01/1900 00:00") {
                                    //    if (dias > 5) {
                                    //        total_Alm = sumTe * val_Alm * (dias - 5);
                                    //        dAn = (dias - 5);
                                    //    }
                                    //    else {
                                    //        total_Alm = sumTe * val_Alm * dias;
                                    //        dAn = dias;
                                    //    }
                                    //} else {
                                    //    total_Alm = sumTe * val_Alm * dias;
                                    //    dAn = dias;
                                    //}

                                    $('.totalAlm').html('$ ' + $.number(total_Alm, 2, '.', ','));

                                    var cadena = "F. Ing.: " + pagos[0].f_tarja + " F. Ini. Calculo: " + ul + " F. Ret.:" + moment(d, "DD/MM/YYYY").format("DD/MM/YYYY") + " P. Fact.: " + $.number(sumTe, 2, '.', ',') + " D. Almac.: " + dAn;
                                    $("#cadena").text(cadena);
                                }
                            } else if (d > ul) {
                                if (contF > 0) {

                                    val_Alm = pagos[0].ta_alm;

                                    var dias = getDateDiff(d, ul);

                                    var total_Alm = 0.00;

                                    //if (ul == ultRetirado)
                                    //    dias = dias - 1;


                                    var dAn;

                                    total_Alm = sumTe * val_Alm * dias;



                                    $('.totalAlm').html('$ ' + $.number(total_Alm, 2, '.', ','));

                                    var cadena = "F. Ing.: " + pagos[0].f_tarja + " F. Ini. Calculo: " + ul + " F. Ret.:" + d + " P. Fact.: " + $.number(sumTe, 2, '.', ',') + " D. Almac.: " + dias;
                                    $("#cadena").text(cadena);
                                }
                            }



                            //$('.totalAlm').html('$ ' + $.number("0", 2, '.', ','));

                            if (traNavi == "X") {
                                var co = $('.totalTra').html();
                                subTotalNavi += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                            } else if (traClie == "X") {
                                var co = $('.totalTra').html();
                                subTotalClie += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                            }


                            if (desNavi == "X") {
                                var co = $('.totalDes').html();
                                subTotalNavi += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                            } else if (desClie == "X") {
                                var co = $('.totalDes').html();
                                subTotalClie += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                            }

                            if (manNavi == "X") {
                                var co = $('.totalMan').html();
                                subTotalNavi += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                            } else if (manClie == "X") {
                                var co = $('.totalMan').html();
                                subTotalClie += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                            }

                            var alm = $('.totalAlm').html();
                            subTotalClie += parseFloat(alm.replace('$', '').replace(' ', '').replace(',', ''));


                            $('#detaClien').append('<tbody><tr>'
                                + '<tr>'
                                + '<td rowspan="3" style="color:#F0FFFF;text-align: center;background-color:#708090;font-weight:bold;">Cliente</td>'
                                + '<td style="color:#708090;text-align: right;">Subtotal</td>'
                                + '<td class="subClie" style="color:#708090;text-align: right;"></td>'
                                + '</tr>'
                                + '<tr>'
                                + '<td style="color:#708090;text-align: right;">IVA</td>'
                                + '<td class="ivaClie" style="color:#708090;text-align: right;"></td>'
                                + '</tr>'
                                + '<tr>'
                                + '<td style="color:#708090;text-align: right;">Total</td>'
                                + '<td class="totClie" style="color:#708090;text-align: right;"></td>'
                                + '</tr>'
                                + '</tbody>');

                            $('#detaNavi').append('<tbody><tr>'
                                + '<tr>'
                                + '<td rowspan="3" style="color:#F0FFFF;text-align: center;background-color:#708090;font-weight:bold;">Naviero</td>'
                                + '<td style="color:#708090;text-align: right;">Subtotal</td>'
                                + '<td class="subNavi" style="color:#708090;text-align: right;"></td>'
                                + '</tr>'
                                + '<tr>'
                                + '<td style="color:#708090;text-align: right;">IVA</td>'
                                + '<td class="ivaNavi" style="color:#708090;text-align: right;"></td>'
                                + '</tr>'
                                + '<tr>'
                                + '<td style="color:#708090;text-align: right;">Total</td>'
                                + '<td class="totNavi" style="color:#708090;text-align: right;"></td>'
                                + '</tr>'
                                + '</tbody>');

                            if (subTotalNavi > 0) {
                                $('.subNavi').html('$ ' + $.number(subTotalNavi, 2, '.', ','));

                                var co = $('.subNavi').html();
                                $('.ivaNavi').html('$ ' + $.number((parseFloat(co.replace('$', '').replace(' ', '').replace(',', '')) * 0.13), 2, '.', ','));

                                var co = $('.subNavi').html();
                                var co1 = $('.ivaNavi').html();
                                var val1 = parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                var val2 = parseFloat(co1.replace('$', '').replace(' ', '').replace(',', ''));

                                var co2 = val1 + val2;
                                $('.totNavi').html('$ ' + $.number(co2, 2, '.', ','))
                            }
                            else {
                                $('.subNavi').html('$ 0.00');
                                $('.ivaNavi').html('$ 0.00');
                                $('.totNavi').html('$ 0.00');
                            }

                            if (subTotalClie > 0) {
                                $('.subClie').html('$ ' + $.number(subTotalClie, 2, '.', ','));

                                var co = $('.subClie').html();
                                $('.ivaClie').html('$ ' + $.number((parseFloat(co.replace('$', '').replace(' ', '').replace(',', '')) * 0.13), 2, '.', ','));

                                var co = $('.subClie').html();
                                var co1 = $('.ivaClie').html();
                                var val1 = parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                var val2 = parseFloat(co1.replace('$', '').replace(' ', '').replace(',', ''));

                                var co2 = val1 + val2;
                                $('.totClie').html('$ ' + $.number(co2, 2, '.', ','))
                            }
                            else {
                                $('.subClie').html('$ 0.00');
                                $('.ivaClie').html('$ 0.00');
                                $('.totClie').html('$ 0.00');
                            }

                            // conteo de contenedores
                            // validacion sadfi pagos
                            // detalle de facturacion

                            var table = $('#GridView1').DataTable({
                                destroy: true,
                                paging: false,
                                info: false,
                                autoWidth: true,
                                ordering: false,
                                scrollX: true,
                                stateSave: true,
                                //columns: [
                                //    { "data": "v_peso", render: $.fn.dataTable.render.number('.', ',', 2, '') }                                        
                                //],
                                language:
                                {
                                    "sProcessing": "Procesando...",
                                    "sLengthMenu": "Mostrar _MENU_ registros",
                                    "sZeroRecords": "No se encontraron resultados",
                                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                                    "sInfo": "Registros del _START_ al _END_ de ( _TOTAL_ ) registros",
                                    "sInfoEmpty": "Registros del 0 al 0 de 0 registros",
                                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                                    "sInfoPostFix": "",
                                    "sSearch": "Buscar:",
                                    "sUrl": "",
                                    "decimal": ".",
                                    "thousands": ",",
                                    "sLoadingRecords": "Cargando...",
                                    "oPaginate": {
                                        "sFirst": "Primero",
                                        "sLast": "Último",
                                        "sNext": "Siguiente",
                                        "sPrevious": "Anterior"
                                    },
                                    "oAria": {
                                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                                    }
                                },
                                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                                    if (aData[0] == contenedor) {
                                        $('td', nRow).css('background-color', '#1155cc');
                                        $('td', nRow).css('color', '#ffff');
                                    }
                                }
                            });

                            getFact(pagos[0].c_tarja);
                        }
                        $.unblockUI();
                    },
                    error: function (msg) {
                        $.unblockUI();
                        bootbox.alert(msg.d);
                    }
                });
            }
        }

        function toDate(dateStr) {
            var parts = dateStr.split("/")
            return new Date(parts[2], parts[1] - 1, parts[0])
        }

        function addDays(date, days) {
            var result = Date.parse(date);
            var ta = new Date(result);
            ta.setDate(ta.getDate() + days);
            return ta;
        }

        var defaultDate = moment().toDate();

        $(document).ready(function () {
            $('#datetimepicker2').datetimepicker({
                defaultDate: new Date(),
                locale: 'es',
                format: "L",
                widgetPositioning: {
                    horizontal: 'left',
                    vertical: 'top'
                },
                minDate: new Date()
            });


            var llegada = GetParameterValues('llegada');
            var contenedor = GetParameterValues('contenedor');
            getBL(llegada, contenedor);


            $("#<%=btnBuscar.ClientID %>").click(function (e) {
                if (llegada.length > 0 && contenedor.length > 0) {
                    e.preventDefault();
                    getBL(llegada, contenedor);
                }
            });
        });
    </script>
</asp:Content>

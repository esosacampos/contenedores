<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsulBL.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsulBL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/dt-1.10.18/fc-3.2.5/fh-3.1.4/r-2.2.2/sc-2.0.0/sl-1.3.0/datatables.min.css" />


    <style type="text/css">
        .scrollme {
            /*width: 100%;
            height: 370px;
            overflow: scroll;*/
        }

        body {
            /*overflow: hidden;*/
        }

        .dataTables_wrapper {
            text-align: center;
            vertical-align: middle;
            white-space: nowrap;
            margin-top: 30px;
        }

        table.dataTable.compact thead th, table.dataTable.compact thead td {
            padding: 4px 13px 3px 3px;            
            vertical-align: middle;
            /*background-color: #708090;
            color: #F5FFFA;*/
            text-align: center;
        }

        tr#cabeMain{
            background-color:#708090;
            color:#F0FFFF;
        }
        
        table.dataTable.stripe tbody tr.odd, table.dataTable.display tbody tr.odd {
            background-color: #FFFAFA;
            color:#708090;
        }
        table.dataTable.stripe tbody tr.even, table.dataTable.display tbody tr.even {
            color:#708090;
        }
             

        .clsTra {
            background-color: #FAFAD2;
            border: 1px solid #ddd;
            color:#708090;
        }

        .clsDes {
            background-color: #F0FFFF;
            border: 1px solid #ddd;
            color:#708090;
        }

        .clsMan {
            background-color: #F0FFF0;
            border: 1px solid #ddd;
            color:#708090;
        }

        .clsAlm {
            background-color: #FFF5EE;
            border: 1px solid #ddd;
            color:#708090;
        }

        .clsRTrP {
            background-color: #FAFAD2;
            font-weight: bold;
            color: #DC143C;
        }

        .clsRTr {
            background-color: #FAFAD2;
            color:#708090;
        }

        .clsRDeP {
            background-color: #F0FFFF;
            font-weight: bold;
            color: #DC143C;
        }

        .clsRDe {
            background-color: #F0FFFF;
            color:#708090;
        }

        .clsRMaP {
            background-color: #F0FFF0;
            font-weight: bold;
            color: #DC143C;
        }

        .clsRMa {
            background-color: #F0FFF0;
            color:#708090;
        }

        .clsRAlP {
            background-color: #FFF5EE;
            font-weight: bold;
            color: #DC143C;
        }

        .clsRAl {
            background-color: #FFF5EE;
            color:#708090;
        }

        .totalTra, .totalDes, .totalMan {
            text-align: center;
            vertical-align: middle;
        }

        #naviero, #cliente {
            float: right;
            width: 250px;
            height: 250px;
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Consulta Por BL
    </h2>
    <br />
    <div class="row" style="margin-left: 100px; margin-bottom: 10px;">
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
                    <div class="form-group">
                        <span style="font-weight: bold;">Último Retiro:</span>
                    </div>
                    <div class="form-group" style="width: 50%;">
                        <span class="ultRetiro" style="margin-left: 55px;"></span>
                    </div>
                    <div class="form-group">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-left: 100px; margin-bottom: 10px;">
        <div class="col-lg-3" style="padding-right: 1px;">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <span style="font-weight: bold;"># BL :</span>
                    </div>
                    <div class="form-group" style="width: 82%;">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Ingrese # de BL a consultar" autocomplete="off" Width="100%"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-9" style="padding-left: 5px;">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <span style="font-weight: bold;">Fecha Próximo Retiro:</span>
                    </div>
                    <div class="form-group" style="width: 50%;">
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
        </div>
    </div>
    <table id="GridView1" class="display compact cell-border row-border" style="width: 100%;">
    </table>
    <div id="naviero">
        <table id="detaNavi" class="table-bordered table-condensed" style="width: 100%">
        </table>
    </div>
    <div id="cliente">
        <table id="detaClien" class="table-bordered table-condensed" style="width: 100%">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.number.min.js") %>"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.18/fc-3.2.5/fh-3.1.4/r-2.2.2/sc-2.0.0/sl-1.3.0/datatables.min.js"></script>
       
    <script type="text/javascript">      

        var d = new Date();

        $(document).ready(function () {
            $('#datetimepicker2').datetimepicker({
                defaultDate: new Date(),
                locale: 'es',
                format: "L",
                minDate: new Date()
            });

            $("#<%=btnBuscar.ClientID %>").click(function (e) {
                if ($("#ContentPlaceHolder1_txtBuscar").length > 0) {
                    e.preventDefault();
                    var params = new Object();
                    params.buscar = $("#ContentPlaceHolder1_txtBuscar").val();
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
                            //$('#GridView1').empty();
                            var pagos = (typeof msg.d) == "string" ? eval('(' + msg.d + ')') : JSON.parse(msg.d);
                            if (pagos.length > 0) {
                                $('#GridView1 tbody').empty();
                                $('#detaNavi tbody').empty();
                                $('#detaClien tbody').empty();
                                $('.ultRetiro').html('06/06/2019');

                                $('#GridView1').append('<thead><tr>'
                                    + '<th colspan="9"></th>'
                                    + '<th class="clsTra" colspan="3">TRANSFERENCIA</th>'
                                    + '<th class="clsDes" colspan="3">DESPACHO</th>'
                                    + '<th class="clsMan" colspan="3">MANEJO</th>'
                                    + '<th class="clsAlm" colspan="3">ALMACENAJE</th>'
                                    + '</tr>'
                                    + '<tr id="cabeMain">'
                                    + '<th># MANIFIESTO</th>'
                                    + '<th>CONTENEDOR</th>'
                                    + '<th>TAMAÑO</th>'
                                    + '<th>TEUS</th>'
                                    + '<th>PESO (Kg.)</th>'
                                    + '<th>F. TARJA</th>'
                                    + '<th>TARJA</th>'
                                    + '<th>F. SALIDA</th>'
                                    + '<th>TRAFICO</th>'
                                    + '<th class="clsRTr">NAVIERO</th>'
                                    + '<th class="clsRTr">CLIENTE</th>'
                                    + '<th class="clsRTr">PENDIENTE</th>'
                                    + '<th class="clsRDe">NAVIERO</th>'
                                    + '<th class="clsRDe">CLIENTE</th>'
                                    + '<th class="clsRDe">PENDIENTE</th>'
                                    + '<th class="clsRMa">NAVIERO</th>'
                                    + '<th class="clsRMa">CLIENTE</th>'
                                    + '<th class="clsRMa">PENDIENTE</th>'
                                    + '<th class="clsRAl">NAVIERO</th>'
                                    + '<th class="clsRAl">CLIENTE</th>'
                                    + '<th class="clsRAl">PENDIENTE</th>'
                                    + '</thead>');

                                $('#GridView1').append('<tbody>');

                                $.each(JSON.parse(msg.d), function (i, v) {
                                    row += "<tr><td>" + v.n_manifiesto + "</td><td>" + v.n_contenedor + "</td><td>" + v.c_tamaño + "</td><td>" + v.v_teus + "</td><td class='pesoKg'>" + $.number(v.v_peso, 2, '.', ',') + "</td><td>"+ v.f_tarja +"</td><td>"+ v.c_tarja +"</td><td>" + v.f_salidas + "</td><td>" + v.c_trafico + "</td><td class='clsRTr nav'>" + v.n_transfer + "</td><td class='clsRTr cli'>" + v.c_transfer + "</td><td class='clsRTrP'>$ 22.07</td><td class='clsRDe nav'>" + v.n_desp + "</td><td class='clsRDe cli'>" + v.c_desp + "</td><td class='clsRDeP'>$ 11.56</td><td class='clsRMa nav'>" + v.n_manejo + "</td><td class='clsRMa cli'>" + v.c_manejo + "</td><td class='clsRMaP'>$ 34.50</td><td class='clsRAl nav'>" + v.n_alm + "</td><td class='clsRAl cli'>" + v.c_alm + "</td><td class='clsRAlP'></td></tr>";
                                });

                                $("#GridView1").append(row);

                                $('#GridView1').append('<tfoot>'
                                    + '<tr>'
                                    + '<th colspan="4" style="text-align: right;">Total (Kg.)</th>'
                                    + '<th class="totalPeso"></th>'
                                    + '<th></th>'
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
                                    + '<th></th>'
                                    + '</tr>'
                                    + '<tr>'
                                    + '<th colspan="4" style="text-align: right;">TM</th>'
                                    + '<th class="tm" style="text-align: right;"></th>'
                                    + '<th></th>'
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
                                    + '<th></th>'
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
                                    $(this).find('.clsRTrP').each(function () {
                                        var combat = parseFloat($(this).text().replace('$', '').replace(' ', ''));
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sumT += parseFloat(combat);
                                        }
                                    });
                                    $('.totalTra', this).html('$ ' + $.number(sumT, 2, '.', ','));

                                    if ($(this).find('.clsRTr.nav').html() == "X") {
                                        traNavi = "X";
                                        traClie = "";
                                    }
                                    else {
                                        traClie = "X";
                                        traNavi = "";
                                    }

                                    $(this).find('.clsRDeP').each(function () {
                                        var combat = parseFloat($(this).text().replace('$', '').replace(' ', ''));
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sumD += parseFloat(combat);
                                        }
                                    });
                                    $('.totalDes', this).html('$ ' + $.number(sumD, 2, '.', ','));

                                    if ($(this).find('.clsRDe.nav').html() == "X") {
                                        desNavi = "X";
                                        desClie = "";
                                    }
                                    else {
                                        desClie = "X";
                                        desNavi = "";
                                    }

                                    $(this).find('.clsRMaP').each(function () {
                                        var combat = parseFloat($(this).text().replace('$', '').replace(' ', ''));
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sumM += parseFloat(combat);
                                        }
                                    });
                                    $('.totalMan', this).html('$ ' + $.number(sumM, 2, '.', ','));

                                    if ($(this).find('.clsRMa.nav').html() == "X") {
                                        manNavi = "X";
                                        manClie = "";
                                    }
                                    else {
                                        manClie = "X";
                                        manNavi = "";
                                    }

                                    $(this).find('.pesoKg').each(function () {
                                        var combat1 = parseFloat($(this).text().replace(',', ''));
                                        if (!isNaN(combat1) && combat1.length !== 0) {
                                            SumPe += parseFloat(combat1);
                                        }
                                    });
                                    $('.totalPeso', this).html($.number(SumPe, 2, '.', ','));

                                    $(this).find('.totalPeso').each(function () {
                                        var combat = parseFloat($(this).text().replace(',', ''));
                                        if (!isNaN(combat) && combat.length !== 0) {
                                            sumTe = parseFloat(combat) / 1000;
                                        }
                                    });
                                    $('.tm', this).html($.number(sumTe, 2, '.', ','));


                                });

                                if (traNavi == "X") {
                                    var co = $('.totalTra').html();
                                    subTotalNavi += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                }

                                if (traClie == "X") {
                                    var co = $('.totalTra').html();
                                    subTotalClie += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                }

                                if (desNavi == "X") {
                                    var co = $('.totalDes').html();
                                    subTotalNavi += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                }

                                if (desClie == "X") {
                                    var co = $('.totalDes').html();
                                    subTotalClie += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                }

                                if (manNavi == "X") {
                                    var co = $('.totalMan').html();
                                    subTotalNavi += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                }

                                if (manClie == "X") {
                                    var co = $('.totalMan').html();
                                    subTotalClie += parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                }


                                $('#detaNavi').append('<tbody><tr>'
                                    + '<tr>'
                                    + '<td rowspan="3" style="color:#F0FFFF;text-align: center;background-color:#708090;font-weight:bold;">Clientes</td>'
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

                                $('#detaClien').append('<tbody><tr>'
                                    + '<tr>'
                                    + '<td rowspan="3" style="color:#F0FFFF;text-align: center;background-color:#708090;font-weight:bold;">Naviero</td>'
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

                                if (subTotalNavi > 0) {
                                    $('.subNavi').html('$ ' + $.number(subTotalNavi, 2, '.', ','));

                                    var co = $('.subNavi').html();
                                    var co1 = parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                    var val1 = co * 0.13;

                                    $('.ivaNavi').html('$ ' + $.number(val1, 2, '.', ','));

                                    var co = $('.subNavi').html();
                                    var co1 = $('.ivaNavi').html();

                                    var val1 = parseFloat(co.replace('$', '').replace(' ', '').replace(',', ''));
                                    var val2 = parseFloat(co1.replace('$', '').replace(' ', '').replace(',', ''));

                                    var co2 = val1 + val2;
                                    $('.totNavi').html('$ ' + $.number(co2, 2, '.', ','));
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
                                

                                $('#GridView1').DataTable({
                                    destroy: true,
                                    paging: false,
                                    info: false,
                                    autoWidth: true,
                                    ordering: false,
                                    scrollY: 300,
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
                                    }
                                });
                            }
                            $.unblockUI();
                        },
                        error: function (msg) {
                            $.unblockUI();
                            bootbox.alert(msg.d);
                        }
                    });


                }

            });
        });
    </script>
</asp:Content>

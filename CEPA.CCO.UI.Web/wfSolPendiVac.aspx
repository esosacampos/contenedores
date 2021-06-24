<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfSolPendiVac.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfSolPendiVac" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/select.dataTables.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>PENDIENTE DE AUTORIZACIÓN DE VACIADO
    </h2>
    <br />
    <table id="grvSolicitudes" class="table table-striped table-bordered" style="width: 100%;">
    </table>
    <br />
    <table id="grvDes" class="table table-striped table-bordered" style="width: 100%;">
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%--   <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/moment.min.js") %>"></script>--%>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <%--    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.number.min.js") %>"></script>--%>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/JS_DT/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/JS_DT/dataTables.bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/JS_DT/dataTables.select.min.js") %>"></script>
    <%--    <script type="text/javascript" src="<%= ResolveUrl("~/bootstrap/js/dataTables.rowsGroup.js") %>"></script>--%>

    <script type="text/javascript">      
        $ = jQuery.noConflict();

        function getSolicitudes() {

            //e.preventDefault();             

            var row = "";
            $.ajax({
                type: "POST",
                url: "wfSolPendiVac.aspx/getSoli",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                async: "true",
                cache: "false",
                success: function (msg) {
                    $('#grvSolicitudes').empty();
                    var contF = 0;
                    var solis = (typeof msg.d) == "string" ? eval('(' + msg.d + ')') : JSON.parse(msg.d);
                    if (solis.length > 0) {
                        $('#grvSolicitudes tbody').empty();

                        //$("h3").text("Detalle de Factura(s) de Almacenaje:");

                        $('#grvSolicitudes').append('<thead><tr">'
                            + '<th>TIPO SOLICITUD</th>'
                            + '<th># MANIFIESTO</th>'
                            + '<th>CONTENEDOR</th>'
                            + '<th>BL MASTER</th>'
                            + '<th>NOMBRE</th>'
                            + '<th>TELEFONO</th>'
                            + '<th>CORREO</th>'
                            + '<th>RETENCION</th>'
                            + '</tr></thead>');

                        $('#grvSolicitudes').append('<tbody>');

                        $.each(JSON.parse(msg.d), function (i, v) {
                            row += "<tr><td>" + v.t_solicitud + "</td><td>" + v.num_mani + "</td><td>" + v.n_contenedor + "</td><td>" + v.bl_master + "</td><td>" + v.n_contacto + "</td><td>" + v.t_contacto + "</td><td>" + v.e_contacto + "</td><td>" + v.t_retencion + "</td></tr>";
                        });

                        $("#grvSolicitudes").append(row);



                        var table = $('#grvSolicitudes').DataTable({
                            select: {
                                style: "single"
                            },
                            language:
                            {
                                "sProcessing": "Procesando...",
                                "sLengthMenu": "Mostrar _MENU_ registros",
                                "sZeroRecords": "No se encontraron resultados",
                                "sEmptyTable": "Ningún dato disponible en esta tabla",
                                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                                "sSearch": "Buscar:",
                                "sInfoThousands": ",",
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
                                },
                                select: {
                                    rows: {                                      
                                        0: "Haga clic en una fila para seleccionarla",
                                        1: "1 fila seleccionada"
                                    }
                                }
                            }
                        });



                        // now do what you need to do wht the row data

                        $('#grvSolicitudes tbody').on('click', 'tr', function () {                            
                            getBLs(table.row(this).data()[2], table.row(this).data()[1]);                            
                        });
                        table.columns.adjust().draw();

                    }
                   
                
                },
                error: function (msg) {
                    bootbox.alert(msg.d);
                }
            });
        }

        function getBLs(n_contenedor, n_manifiesto) {
            var row = "";

            var params = new Object();
            params.n_contenedor = n_contenedor;
            params.n_manifiesto = n_manifiesto
            params = JSON.stringify(params);

            $.ajax({
                type: "POST",
                url: "wfSolPendiVac.aspx/getBLes",
                data: params,
                contentType: "application/json; charset=utf-8",
                async: "true",
                cache: "false",
                success: function (msg) {
                    $('#grvDes').empty();
                    var contF = 0;
                    var descon = (typeof msg.d) == "string" ? eval('(' + msg.d + ')') : JSON.parse(msg.d);
                    if (descon.length > 0) {
                        $('#grvDes tbody').empty();

                        //$("h3").text("Detalle de Factura(s) de Almacenaje:");

                        $('#grvDes').append('<thead><tr">'                         
                            + '<th>CONTENEDOR</th>'
                            + '<th>BL HIJO</th>'
                            + '<th>TAMAÑO</th>'
                            + '<th>CONSIGNATARIO</th>'
                            + '</tr></thead>');

                        $('#grvDes').append('<tbody>');

                        $.each(JSON.parse(msg.d), function (i, v) {
                            if(v.t_bl != "BLC")
                                row += "<tr><td>" + v.n_contenedor + "</td><td>" + v.bl_hijo + "</td><td>" + v.c_tamaño + "</td><td>" + v.s_consignatario + "</td></tr>";
                        });

                        $("#grvDes").append(row);



                        var table = $('#grvDes').DataTable({
                            select: {
                                style: "single"
                            },
                            language:
                            {
                                "sProcessing": "Procesando...",
                                "sLengthMenu": "Mostrar _MENU_ registros",
                                "sZeroRecords": "No se encontraron resultados",
                                "sEmptyTable": "Ningún dato disponible en esta tabla",
                                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                                "sSearch": "Buscar:",
                                "sInfoThousands": ",",
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
                                },
                                select: {
                                    rows: {
                                        0: "Haga clic en una fila para seleccionarla",
                                        1: "1 fila seleccionada"
                                    }
                                }
                            }
                        });
                    }
                },
                error: function (msg) {
                    bootbox.alert(msg.d);
                }
            });
        }

        $(document).ready(function () {
            getSolicitudes();

            
        });
    </script>
</asp:Content>

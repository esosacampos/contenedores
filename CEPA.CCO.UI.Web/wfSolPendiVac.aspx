<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfSolPendiVac.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfSolPendiVac" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/select.dataTables.min.css" rel="stylesheet" />
    <style type="text/css">
        .navbar {
            position: relative;
            min-height: 50px;
            margin-bottom: 0;
            border: 1px solid transparent;
        }

        #mySideNav {
            min-height: 640px;
        }

        table.dataTable td, th {
            word-break: normal;
            text-align: center;
            vertical-align: middle;
        }

        .col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {
            position: relative;
            min-height: initial;
            padding-right: 15px;
            padding-left: 15px;
        }

        div.dataTables_wrapper div.dataTables_length select {
            width: 40px;
            display: inline-block;
        }

        .btn-view {
            display: inline-block;
            padding: 1px 10px;
            margin-bottom: 0;
            font-size: 12px;
            font-weight: 700;
            line-height: 2;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 5px;
        }

        table.dataTable {
            clear: both;
            margin-top: 2px !important;
            margin-bottom: 2px !important;
            max-width: none !important;
            border-collapse: separate !important;
        }

        hr {
            margin-top: 1px;
            margin-bottom: 3px;
            border: 0;
            border-top: 1px solid #eee;
        }

        .principal {
            top: 5px;
        }

        .table {
            width: 100%;
            max-width: 100%;
            margin-bottom: 5px;
        }

        @media (min-width: 768px) {
            .form-inline .form-control {
                display: inline-block;
                width: auto;
                vertical-align: middle;
            }
        }

        @media (min-width: 768px) {
            .form-inline .form-control, .navbar-form .form-control {
                display: inline-block;
                width: auto;
                vertical-align: middle;
            }
        }

        @media (min-width: 768px) {
            .form-inline .form-control {
                display: inline-block;
                width: auto;
                vertical-align: middle;
            }


            select.input-sm {
                height: 26px;
                line-height: 26px;
            }

            select.input-sm {
                height: 26px;
                line-height: 26px;
            }

            select.input-sm {
                height: 30px;
                line-height: 30px;
            }

            .input-sm {
                height: 30px;
                padding: 1px 10px;
                font-size: 12px;
                line-height: 1.5;
                border-radius: 3px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-sm-11">
            <div role="form">
                <div class="form-group">
                    <h2 style="font-size: 1.08em; font-weight: 700; margin-top: 5px; margin-bottom: 5px;">PENDIENTE DE AUTORIZACIÓN DE VACIADO
                    </h2>
                </div>
            </div>
        </div>
        <div class="col-sm-1">
            <div role="form">
                <div class="form-group">
                    <button type="submit" id="btnKeep" class="btn btn-primary">Saltar</button>
                </div>
            </div>
        </div>
    </div>

    <table id="grvSolicitudes" class="table table-striped table-bordered compact" style="width: 100%; font-size: 0.82em;">
    </table>
    <br />
    <table id="grvDes" class="table table-striped table-bordered compact" style="width: 100%; font-size: 0.82em;">
    </table>
    <br />
    <div class="row">
        <div class="col-sm-6" style="min-height: 250px;">
            <div role="form">
                <div class="form-group">
                    <label>Observaciones</label><br />
                    <asp:TextBox ID="txtObservaciones" TextMode="multiline" Columns="50" Rows="5" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-sm-6" style="min-height: 250px;">
            <div role="form">
                <div class="form-group">
                    <label class="control-label" style="color: black !important;">Anexar Mandamiento de Pago</label>
                    <input type="file" id="fileUpload" name="fileUpload" class="filestyle" data-text="Examinar" data-placeholder="Seleccione un archivo tipo pdf | jpg | png" data-btnclass="btn-success" data-dragdrop="true" />
                </div>
                <div class="form-group">
                    <div runat="server" id="myRadio">
                        <div>
                            <asp:CheckBox class="label-success" runat="server" ID="radio3" />
                            <label for="radio3">Iniciar Proceso</label>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <button type="submit" id="btnSend" class="btn btn-primary">Enviar</button>
                    <asp:Button ID="btnClear" runat="server" class="btn btn-default" Text="Regresar" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%--   <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/moment.min.js") %>"></script>--%>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <%--    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.number.min.js") %>"></script>--%>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/JS_DT/jquery.dataTables.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/JS_DT/dataTables.bootstrap.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/JS_DT/dataTables.select.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/JS_DT/bootstrap-filestyle.min.js") %>"></script>
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

                    var contF = 0;
                    var solis = (typeof msg.d) == "string" ? eval('(' + msg.d + ')') : JSON.parse(msg.d);
                    if (solis.length > 0) {
                        $('#grvSolicitudes').empty();
                        $('#grvSolicitudes tbody').empty();

                        //$("h3").text("Detalle de Factura(s) de Almacenaje:");

                        $('#grvSolicitudes').append('<thead><tr">'
                            + '<th># SOLICITUD</th>'
                            + '<th>TIPO SOLICITUD</th>'
                            + '<th># MANIFIESTO</th>'
                            + '<th>CONTENEDOR</th>'
                            + '<th>BL MASTER</th>'
                            + '<th>NOMBRE</th>'
                            + '<th>TELEFONO</th>'
                            + '<th>CORREO</th>'
                            + '<th>RETENCION</th>'
                            + '<th></th>'
                            + '</tr></thead>');

                        $('#grvSolicitudes').append('<tbody>');

                        var orderId = new Array();

                        $.each(JSON.parse(msg.d), function (i, v) {
                            orderId[i] = v.IdTipoVa;
                            row += "<tr><td>" + v.IdTipoVa + "</td><td>" + v.t_solicitud + "</td><td>" + v.num_mani + "</td><td>" + v.n_contenedor + "</td><td>" + v.bl_master + "</td><td>" + v.n_contacto + "</td><td>" + v.t_contacto + "</td><td>" + v.e_contacto + "</td><td>" + v.t_retencion + "</td><td>" + "" + "</td></tr>";
                        });

                        $("#grvSolicitudes").append(row);

                        var minimo = Math.min(...orderId);


                        var table = $('#grvSolicitudes').DataTable({
                            searching: false,
                            responsive: true,
                            select: {
                                style: "single"
                            },
                            ordering: false,
                            bLengthChange: false,
                            pageLength: 5,
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
                            },
                            columnDefs: [{
                                targets: -1,
                                data: null,
                                defaultContent: '<button class="btn btn-success btn-view" type="button">Ver</button>'
                            }]
                            //columns: [
                            //    { data: "# SOLICITUD", className: "dt[-head|-body]-center" },
                            //    { data: "TIPO SOLICITUD" },
                            //    { data: "# MANIFIESTO" },
                            //    { data: "CONTENEDOR", className: "uniqueClassName" },
                            //    { data: "BL MASTER", className: "uniqueClassName" },
                            //    { data: "NOMBRE", className: "uniqueClassName" },
                            //    { data: "TELEFONO", className: "uniqueClassName" },
                            //    { data: "CORREO", className: "uniqueClassName" },
                            //    { data: "RETENCION", className: "uniqueClassName" }
                            //]
                        });



                        // now do what you need to do wht the row data

                        //$('#grvSolicitudes tbody').on('click', 'tr', function () {
                        //    getBLs(table.row(this).data()[2], table.row(this).data()[1]);
                        //});

                        $('#grvSolicitudes tbody').on('click', '.btn-view', function (e) {
                            var data = table.row($(this).parents('tr')).data();
                            //console.log(data);
                            if (data[0] == minimo || keepVar == 1)
                                getBLs(data[3], data[2]);
                            else
                                bootbox.alert("Se debe respetar el orden de la solicitud, de lo contrario dar click en boton Saltar");
                        });


                        table.columns.adjust().draw();
                        //table.destroy();
                    }


                },
                error: function (msg) {
                    bootbox.alert(msg.d);
                }
            });
        }



        function getBLs(n_contenedor, n_manifiesto) {
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

                    var contF = 0;
                    var descon = (typeof msg.d) == "string" ? eval('(' + msg.d + ')') : JSON.parse(msg.d);
                    console.log(JSON.parse(msg.d));
                    console.log((msg.d).indexOf("HTTP"));
                    if ((msg.d).indexOf("HTTP") == -1) {
                        if (descon.length > 0) {
                            $('#grvDes').empty();
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
                                if (v.t_bl != "BLC")
                                    row += "<tr><td>" + v.n_contenedor + "</td><td>" + v.bl_hijo + "</td><td>" + v.c_tamaño + "</td><td>" + v.s_consignatario + "</td></tr>";
                            });

                            $("#grvDes").append(row);



                            var table = $('#grvDes').DataTable({
                                searching: false,
                                responsive: true,
                                select: {
                                    style: "single"
                                },
                                ordering: false,
                                bLengthChange: false,
                                pageLength: 5,
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
                            table.destroy();
                            table.columns.adjust().draw();

                        }
                        $.unblockUI();
                    }
                    else {
                        $.unblockUI();
                        bootbox.alert(msg.d);
                    }

                },
                error: function (msg) {
                    $.unblockUI();
                    bootbox.alert(msg.d);
                }
            });
            keepVar = 0;
        }
        var keepVar = 0;
        $(document).ready(function () {

            getSolicitudes();

            $('#fileUpload').filestyle({
                iconName: "glyphicon glyphicon-inbox",
                onChange: function (param) {
                    console.log(param);
                }
            });


            $('#btnKeep').click(function (e) {
                e.preventDefault();
                if (keepVar == 0) {
                    bootbox.confirm("¿Seguro(a) que desea saltar el orden de solicitudes? ", function (result) {
                        if (result) {
                            keepVar = 1;
                            bootbox.alert("Proceda con validar solicitud");
                        } else {
                            keppVar = 0;
                            bootbox.alert("De no continuar con la liberación puede dar F5 para actualizar");
                        }
                    });
                 }
            });


        });
    </script>
</asp:Content>

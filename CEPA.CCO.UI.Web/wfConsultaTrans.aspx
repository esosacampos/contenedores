<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfConsultaTrans.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsultaTrans" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bs-pagination {
            background-color: #1771F8;
        }

        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 7px;
            text-align: center;
            border-left: none;
            font-family: 'trebuchet MS', 'Lucida sans', Arial;
        }

        .embed-responsive-16by9 {
            padding-bottom: 90%;
        }

        .badge {
            font-size: 1.25em;
        }

        #myTableRe thead tr {
            background-color: #2F5CC6;
            color: white;
        }

            #myTableRe thead tr th, tfoot tr th {
                text-align: center;
            }

        #myTableRe tfoot tr {
            background-color: #5DADE2;
            color: white;
        }

        #myTableDeta thead tr {
            background-color: #2F5CC6;
            color: white;
        }

            #myTableDeta thead tr th, tfoot tr th {
                text-align: center;
            }

        #myTableDeta tfoot tr {
            background-color: #5DADE2;
            color: white;
        }

        .glyphicon-refresh-animate {
            -animation: spin .7s infinite linear;
            -webkit-animation: spin2 .7s infinite linear;
        }

        @-webkit-keyframes spin2 {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            from {
                transform: scale(1) rotate(0deg);
            }

            to {
                transform: scale(1) rotate(360deg);
            }
        }

        @media screen {
            #printSection {
                display: none;
            }
        }

        @media print {

            body * {
                visibility: hidden;
            }

            html, body {
                height: 100vh;
                margin: 0 !important;
                padding: 0 !important;
                overflow: hidden;
            }

            #printSection, #printSection * {
                visibility: visible;
                overflow: visible;
            }

            #printSection {
                position: absolute;
                left: 0;
                top: 0;
                page-break-after: always;
            }

            .modal {
                position: absolute;
                left: 0;
                top: 0;
                margin: 0;
                padding: 0;
                visibility: visible;
                /**Remove scrollbar for printing.**/
                overflow: visible !important;
            }

            .modal-dialog {
                visibility: visible !important;
                /**Remove scrollbar for printing.**/
                overflow: visible !important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Consulta Contenedores Transmitidos</h2>
    <hr />
    <div class="table-responsive">
        <asp:HiddenField ID="hBuque" runat="server" />
        <asp:HiddenField ID="hLlegada" runat="server" />
        <table class="table">
            <tr>
                <td colspan="2">IMO
                </td>
                <td colspan="2">
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Nombre del Buque
                </td>
                <td colspan="2">

                    <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Código de Llegada
                </td>
                <td colspan="2">
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Fecha de llegada
                </td>
                <td colspan="2">
                    <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Total C. Importados
                </td>
                <td colspan="2">
                    <span class="alert alert-warning">
                        <asp:Label ID="tot_lineasa" runat="server" Text="" class="label-warning badge"></asp:Label>
                        <strong>C. Anunciados </strong></span>
                    <span class="glyphicon glyphicon-minus"></span>
                    <span class="alert alert-danger">
                        <asp:Label ID="tot_cancel" runat="server" Text="" class="label-danger badge"></asp:Label>
                        <strong>C. Cancelados</strong></span>
                    <span class="glyphicon glyphicon-equal"></span>
                    <span class="alert alert-success">
                        <asp:Label ID="tot_imp" runat="server" Text="" class="label-success badge"></asp:Label>
                        <strong>C. Importados</strong></span>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <button id="btnDetalla" type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModalEsta">Ver Detalle</button>
                </td>
            </tr>
            <tr>
                <td>Total C. Recibidos En Patio
                </td>
                <td>
                    <asp:Label ID="tot_trans" runat="server" Text="" class="label-primary badge"></asp:Label>
                </td>
                <td>Total C. Pendientes En Patio
                </td>
                <td>
                    <asp:Label ID="lblPP" runat="server" Text="" class="label-info badge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Total C. Recibidos En Arco OIRSA
                </td>
                <td>
                    <asp:Label ID="total_arco" runat="server" Text="" class="label-primary badge"></asp:Label>
                </td>
                <td>Total C. Pendientes En Arco OIRSA
                </td>
                <td>
                    <asp:Label ID="lblPO" runat="server" Text="" class="label-info badge"></asp:Label>
                </td>
            </tr>
        </table>

    </div>
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="filtrar"
                DataKeyNames="c_correlativo" CssClass="footable" Style="margin-left: 15px; margin-bottom: 5%;"
                data-filter="#filter" data-page-size="10" ShowFooter="true" data-paging-count-format="{CP} of {TP}"
                OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging"
                OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="c_cliente" HeaderText="AGENCIA"></asp:BoundField>
                    <asp:BoundField DataField="c_manifiesto" HeaderText="# MANIFESTO"></asp:BoundField>
                    <asp:BoundField DataField="c_correlativo" HeaderText="CORR."></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="b_estado" HeaderText="ESTADO"></asp:BoundField>
                    <asp:BoundField DataField="c_tarja" HeaderText="# TARJA"></asp:BoundField>
                    <asp:BoundField DataField="b_requiere" HeaderText="ENTREGA"></asp:BoundField>
                    <asp:BoundField DataField="b_recepcion_c" HeaderText="TRANS. ARCO"></asp:BoundField>
                    <asp:BoundField DataField="f_dan" HeaderText="FECHA DE TRANS. ARCO"></asp:BoundField>
                    <asp:BoundField DataField="f_recep" HeaderText="FECHA DE RECEP. PATIO"></asp:BoundField>
                    <asp:BoundField DataField="b_trans" HeaderText="TRANS. ADUANA"></asp:BoundField>
                    <asp:BoundField DataField="f_trans" HeaderText="FECHA DE TRANS ADUANA"></asp:BoundField>

                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Modal Bancos -->

    <div class="modal fade" tabindex="-1" role="dialog" id="myModalEsta" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div id="printThis" style="min-height: 900px;">
                <div class="modal-content" style="min-height: 900px;">
                    <div class="modal-header" style="line-height: 10px;">
                        <button type="button" class="close" id="myCloseD" data-dismiss="modal" aria-hidden="true">
                            ×</button>
                        <h4 class="modal-title" style="font-weight: 900;">BUQUE: 
                        <label id="buque" style="color: #2F5CC6; font-weight: 800;"></label>
                        </h4>
                    </div>
                    <div class="modal-body">

                        <h4 class="modal-title" style="font-weight: 900;">RESUMEN</h4>
                        <div align="center" class='embed-responsive embed-responsive-16by9'>
                            <table class="table table-striped table-bordered table-hover text-center" id="myTableRe" style="margin-bottom: 5px;">
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
                            <hr />
                            <h4 class="modal-title" style="font-weight: 900; text-align: left;">DETALLE</h4>
                            <table class="table table-striped table-bordered table-hover text-center" id="myTableDeta" style="margin-bottom: 5px;">
                                <thead>
                                    <tr>
                                        <th>NAVIERA</th>
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
                        <button type="button" class="btn btn-primary" id="btnPrint">Imprimir</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal" style="font-weight: 900;">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <nav>
        <ul class="pager">
            <li class="previous"></li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar"
                    OnClick="btnRegresar_Click" />
            </li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.number.min.js") %>"></script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                }
                $.unblockUI();
            });
        };

        prm.add_beginRequest(function OnBeginRequest(sender, args) {
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
        });

        $('#printButton').on('click', function () {
            if ($('.modal').is(':visible')) {
                var modalId = $(event.target).closest('.modal').attr('id');
                $('body').css('visibility', 'hidden');
                $("#" + modalId).css('visibility', 'visible');
                $('#' + modalId).removeClass('modal');
                window.print();
                $('body').css('visibility', 'visible');
                $('#' + modalId).addClass('modal');
            } else {
                window.print();
            }
        });

        $("#btnDetalla").click(function () {
            var text = $("#<%= hBuque.ClientID %>").val();
            $("#buque").text(text);
            var llegada = $("#<%= hLlegada.ClientID %>").val();

            var params = new Object();
            params.c_llegada = llegada;
            params = JSON.stringify(params);


            var row = "";
            $.ajax({
                async: true,
                cache: false,
                type: "POST",
                url: "wfConsultaTrans.aspx/getResumen",
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

            /* DETALLE */
            var row1 = "";
            $.ajax({
                async: true,
                cache: false,
                type: "POST",
                url: "wfConsultaTrans.aspx/getDetalle",
                data: params,
                contentType: "application/json; charset=utf8",
                dataType: "json",
                success: function (msg) {
                    console.log("Entro");
                    var detalle = (typeof msg.d) == "string" ? eval('(' + msg.d + ')') : msg.d;

                    if (detalle.length > 0) {
                        $('#myTableDeta tbody').empty();
                        $('#myTableDeta tfoot').empty();

                        $('#myTableDeta').append('<tbody>');

                        $.each(JSON.parse(msg.d), function (i, v) {
                            row1 += "<tr><td>" + v.c_naviera + "</td><td>" + v.c_tamaño + "</td><td class='clsMani'>" + v.manifestados + "</td><td class='clsCancelados'>" + v.cancelados + "</td><td class='clsTotal'>" + v.total + "</td><td class='clsRecibidos'>" + v.recibidos + "</td><td class='clsPendientes'>" + v.pendientes + "</td></tr>";
                        });

                        $("#myTableDeta").append(row1);

                        $('#myTableDeta').append('<tfoot>'
                            + '<tr>'
                            + '<th colspan="2">TOTALES</th>'
                            + '<th class="totalMani"></th>'
                            + '<th class="totalCance"></th>'
                            + '<th class="totalTo"></th>'
                            + '<th class="totalReci"></th>'
                            + '<th class="totalPendi"></th>'
                            + '</tr>'
                            + '</tfoot>');


                        var sMani = 0, sCancel = 0, sTotal = 0, sReci = 0, sPendi = 0;


                        $('#myTableDeta tr').each(function () {

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

        });


        function convertToPagination(obj) {
            var liststring = '<ul class="pagination">';
            $(obj).find("tbody tr").each(function () {
                $(this).children().map(function () {
                    liststring = liststring + "<li>" + $(this).html() + "</li>";
                });
            });
            liststring = liststring + "</ul>";
            var list = $(liststring);
            list.find('span').parent().addClass('active');
            $(obj).replaceWith(list);
            var s = $(obj).find("tbody tr").length;
            var a = "prueba";
        }


        function myFunction() {
            setTimeout(function () { location.reload(); }, 900000);
        }

        $(document).ready(function () {
            $('#ContentPlaceHolder1_GridView1').footable({
            });

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

            $('.bs-pagination td table').each(function (index, obj) {
                convertToPagination(obj);
            });

            //$('.label-default').html(x + " " + y);

            document.getElementById("btnPrint").onclick = function () {
                printElement(document.getElementById("printThis"));
            }

            function printElement(elem) {
                var domClone = elem.cloneNode(true);

                var $printSection = document.getElementById("printSection");

                if (!$printSection) {
                    var $printSection = document.createElement("div");
                    $printSection.id = "printSection";
                    document.body.appendChild($printSection);
                }

                $printSection.innerHTML = "";
                $printSection.appendChild(domClone);
                window.print();
            }

            myFunction();
        });
    </script>
</asp:Content>

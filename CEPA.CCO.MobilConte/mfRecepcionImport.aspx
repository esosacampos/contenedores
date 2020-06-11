<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mfRecepcionImport.aspx.cs" Inherits="CEPA.CCO.MobilConte.mfRecepcionImport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="css/mobilCEPA.css" />
    <link rel="stylesheet" href="css/jquery.mobile.icons.min.css" />
    <link rel="stylesheet" href="css/jquery.mobile.structure-1.4.5.min.css" />
    <link href="~/bootstrap/csss/bootstrap.min.css" rel="stylesheet">
    <link href="bootstrap/csss/autocompleteText.css" rel="stylesheet">
    <link href="~/bootstrap/csss/footable.metro.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/footable.core.css" rel="stylesheet" type="text/css" />
    <link href="~/bootstrap/csss/footable-demos.css" rel="stylesheet" type="text/css" />
    <title>CEPA-Contenedore Movil</title>
    <style type="text/css">
        .ui-overlay-a, .ui-page-theme-a, .ui-page-theme-a .ui-panel-wrapper {
            background-image: url('images/fondo_principal.png');
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 100% 100%;
            background-attachment: fixed;
            border-color: #378ac8 /*{a-page-border}*/;
            color: #ffffff /*{a-page-color}*/;
            text-shadow: 0 /*{a-page-shadow-x}*/ 1px /*{a-page-shadow-y}*/ 0 /*{a-page-shadow-radius}*/ #444444 /*{a-page-shadow-color}*/;
        }

        .ui-overlay-b, .ui-page-theme-b, .ui-page-theme-b .ui-panel-wrapper {
            background-image: url('images/fond_cepa.png');
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 100% 100%;
            background-attachment: fixed;
            color: black /*{a-page-color}*/;
            text-shadow: 0 /*{a-page-shadow-x}*/ 1px /*{a-page-shadow-y}*/ 0 /*{a-page-shadow-radius}*/ #444444 /*{a-page-shadow-color}*/;
        }

        .ui-header, .ui-footer {
            border: 0px;
        }

        div.pager {
            text-align: center;
            margin: 1em 0;
        }

            div.pager span {
                display: inline-block;
                width: 1.8em;
                height: 1.8em;
                line-height: 1.8;
                text-align: center;
                cursor: pointer;
                background: #000;
                color: #fff;
                margin-right: 0.5em;
            }

                div.pager span.active {
                    background: #c00;
                }

        /*.ui-header {
            height: 40px;
            width: 100%;
            z-index: 1;
            position: fixed;
        }

        .ui-footer {
            width: 100%;
            height: 100px;
            position: absolute;
            bottom: 0;
        }*/

        .ui-bar-a, .ui-page-theme-a .ui-bar-inherit {
            background: none;
        }

        .bs-pagination {
            background-color: #1771F8;
        }

        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 7px;
            text-align: center;
            border-left: none;
            /*font-family: 'trebuchet MS', 'Lucida sans', Arial;*/
        }

        table#myTable.ui-responsive.ui-table.ui-table-reflow tbody tr td {
            /*text-align:center; */
            vertical-align: middle;
            font-size: 19px;
            border-collapse: separate;
            border-spacing: 10px 5px;
        }

        body {
            background: #66999 url(background-photo.jpg) center center cover no-repeat fixed;
        }

        /*html {
            overflow: hidden;
        }*/

        body {
            margin: 0;
            box-sizing: border-box;
        }

        html, body {
            height: 100%;
        }
        [data-role=footer]{bottom:0; position:absolute !important; top: auto !important; width:100%;} 
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
            EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/js/jquery-1.11.1.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/footable.js" />
                <asp:ScriptReference Path="~/bootstrap/js/footable.filter.js" />
                <asp:ScriptReference Path="~/bootstrap/js/footable.paginate.js" />
                <asp:ScriptReference Path="~/bootstrap/js/bootstrap.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/bootbox.min.js" />
                <asp:ScriptReference Path="~/js/jquery.mobile-1.4.5.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/autocomplete.js" />
            </Scripts>
        </asp:ToolkitScriptManager>
        <div data-role="page" id="page1" data-theme="b">
            <div data-role="header" id="headerA" data-position="fixed" style="margin-top: 8%; background-color: #313945; padding: 2%;">
                <a href="mfDefault.aspx" class="ui-btn ui-btn-left ui-corner-all ui-shadow ui-icon-home ui-btn-icon-left">Menu</a>
                <h1></h1>
                <a href="mfInicio.aspx" class="ui-btn ui-btn-right ui-corner-all ui-shadow ui-icon-power ui-btn-icon-right">Salir</a>
            </div>
            <div data-role="content" id="content" style="margin-top: 9%">
                <asp:HiddenField ID="hMarcacion" runat="server" Value="" />
                <label for="search"># Contenedor:</label>
                <input type="search" name="txtConte" id="search" value="" class="form-control" placeholder="Ultimos digitos contenedor">
                <table data-role="table" class="ui-responsive" id="myTable">
                </table>

                <div class="ui-grid-b ui-responsive" style="margin: 0 auto 0 auto; text-align: center">
                    <button type="button" class="ui-shadow ui-btn ui-corner-all ui-btn-inline ui-icon-check ui-btn-icon-left ui-btn-a" id="btnAcceso">
                        Confirmar</button>

                    <button type="button" class="ui-shadow ui-btn ui-corner-all ui-btn-inline ui-icon-delete ui-btn-icon-left ui-btn-b" id="btnCancelar">
                        Cancelar</button>

                    <button type="button" class="ui-shadow ui-btn ui-corner-all ui-btn-inline ui-icon-grid ui-btn-icon-left ui-btn-c" id="btnResumen">
                        Resumen</button>

                    <button type="button" class="ui-shadow ui-btn ui-corner-all ui-btn-inline ui-icon-action ui-btn-icon-left ui-btn-c" id="btnRecepcion">
                        Recepcion</button>

                </div>
            </div>

            <div data-role="footer" data-position="fixed">
                <div align="center">
                    <h4>© 2016 CEPA / Puerto de Acajutla, El Salvador v1.0</h4>
                </div>
            </div>
        </div>

        <div data-role="page" id="page2" data-theme="c">
            <div data-role="header" id="headerC" data-position="fixed">
                <a href="#page1" class="ui-btn ui-btn-left ui-corner-all ui-shadow ui-icon-home ui-btn-icon-left">Regresar</a>
                <h1>Resumen Recepcion Importacion</h1>
                <a href="mfInicio.aspx" class="ui-btn ui-btn-right ui-corner-all ui-shadow ui-icon-power ui-btn-icon-right">Salir</a>
            </div>
            <div data-role="content" id="content2">
                <table data-role="table" class="ui-responsive" id="myResumen">
                </table>
            </div>

            <div data-role="footer" data-position="fixed">
                <div align="center">
                    <h4>© 2016 CEPA / Puerto de Acajutla, El Salvador v1.0</h4>
                </div>
            </div>
        </div>
        <div data-role="page" id="page3" data-theme="b">
            <div data-role="header" id="headerD" data-position="fixed" style="margin-top:9%;background-color: #313945; padding: 2%;">
                <a href="#page1" class="ui-btn ui-btn-left ui-corner-all ui-shadow ui-icon-home ui-btn-icon-left">Regresar</a>
                <h1></h1>
                <a href="mfInicio.aspx" class="ui-btn ui-btn-right ui-corner-all ui-shadow ui-icon-power ui-btn-icon-right">Salir</a>
            </div>
            <div data-role="content" id="content3" style="margin-top:9%;">
                <input id="filter" data-type="search" class="form-control">
                <table data-role="table" class="ui-responsive filtrar" id="myRecepcion" data-filter="#filter">
                    <thead>
                        <th># CONTENEDOR</th>
                        <th>F/H RECEPCION</th>
                        <th>COD. MARCACION</th>
                    </thead>
                    <tbody>
                        <tr>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="5">
                                <div class="pagination pagination-centered hide-if-no-paging"></div>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>

            <div data-role="footer" data-position="fixed">
                <div align="center">
                    <h4>© 2016 CEPA / Puerto de Acajutla, El Salvador v1.0</h4>
                </div>
            </div>
        </div>
        <script>
            $(document).bind("mobileinit", function () {
                $.mobile.ajaxEnabled = false;
            });
        </script>

        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        SetAutoComplete();
                        ClearText();
                    }

                });
            };




            function stopRKey(evt) {
                var evt = (evt) ? evt : ((event) ? event : null);
                var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
                if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
            }
            document.onkeypress = stopRKey;

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
            }

            function llenarRecepcion() {
                $.ajax({

                    type: "POST",
                    url: '<%=ResolveUrl("~/mfRecepcionImport.aspx/getDataRecep") %>',
                    data: {},
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var recibidos = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;
                        var trHTML = '';

                        if (recibidos.length > 0) {
                            $.each(recibidos, function (i, item) {
                                trHTML += '<tr><td>' + recibidos[i].n_contenedor + '</td><td>' + recibidos[i].f_recepcion + '</td><td>' + recibidos[i].c_marcacion + '</td></tr>';
                            });
                        }
                        else {

                            bootbox.alert("No hay información de recepcion que mostrar");

                        }


                        $('#myRecepcion').append(trHTML);



                    },

                    error: function (msg) {

                        bootbox.alert("CEPA - Contenedores: Alerta!! Se ha producido un error vuelva a intertarlo o reporte a Informática");
                    }
                });

            }




            $(function () {
                SetAutoComplete();

                $("#search").keypress(function (e) {
                    if (e.which == 13) {
                        if ($('#search').val() == '') {
                            bootbox.alert("Indicar los ultimos 4 dígitos del # de contenedor");
                        }
                        else {
                            var params = new Object();
                            params.n_contenedor = $('#search').val();
                            params = JSON.stringify(params);
                            $.ajax({
                                url: '<%=ResolveUrl("~/mfRecepcionImport.aspx/GetConteInfo") %>',
                                data: params,
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (response) {
                                    var pagos = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;

                                    $("#myTable").empty();

                                    if (pagos.length > 0) {
                                        for (var i = 0; pagos.length; i++) {

                                            $("#myTable").append('<tbody><tr><td style="font-weight: bold;" colspan="2">Buque: </td><td colspan="2"><input type="hidden" id="hIdDeta" />' + pagos[i].c_buque + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Naviera</td><td colspan="2">' + pagos[i].c_cliente + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Tara</td><td colspan="2">' + pagos[i].v_tara + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Tamaño</td><td colspan="2">' + pagos[i].c_tamaño + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Tráfico</td><td colspan="2"><input type="hidden" id="hDirecto" />' + pagos[i].c_trafico + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Estado</td><td colspan="2">' + pagos[i].c_estado + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;width:30%">Retenido Por La DAN</td><td style="color:' + pagos[i].b_style + ';font-weight: bold;width:5%">' + pagos[i].b_detenido + '</td><td style="font-weight: bold;;width:30%">Solicitado Por La DGA</td><td style="color:' + pagos[i].b_staduana + ';font-weight: bold;width:14%">' + pagos[i].b_aduana + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2" rowspan="2">Observaciones</td><td colspan="2" rowspan="2"><textarea name="addinfo" id="info" style="margin: 0;width: 90%;height: 209px;"></textarea></td></tr>'
                                                + '</tbody>');
                                            $("#hIdDeta").val(pagos[i].IdDeta);
                                            if (pagos[i].c_trafico.indexOf("DIRECTO") != -1) {
                                                $("#hDirecto").val("1");
                                            }
                                            else {
                                                $("#hDirecto").val("0");
                                            }

                                            break;
                                        }

                                        $('#btnAcceso').attr("disabled", false);
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

                $("#btnAcceso").click(function () {
                    bootbox.confirm("¿Desea confirmar la recepción del contenedor #" + $("#search").val() + " ?", function (result) {
                        if (result) {
                            var params = new Object();
                            params.IdDeta = $("#hIdDeta").val();
                            params.s_observaciones = $("#info").val();
                            params.c_marcacion = $("#hMarcacion").val();
                            params.b_directo = $("#hDirecto").val();
                            params = JSON.stringify(params);
                            $.ajax({
                                url: '<%=ResolveUrl("~/mfRecepcionImport.aspx/SaveConfir") %>',
                                data: params,
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (response) {
                                    bootbox.alert(response.d);
                                },
                                error: function (response) {
                                    bootbox.alert("CEPA - Contenedores: Alerta!! Se ha producido un error vuelva a intertarlo o reporte a Informática");
                                },
                                failure: function (response) {
                                    bootbox.alert(response.responseText);
                                }
                            });
                            $("#myTable").empty();
                            $("#info").val('');
                            $("#search").val('');
                            $('#btnAcceso').attr("disabled", true);
                        }
                        else {
                            $("#myTable").empty();
                            $("#info").val('');
                            $("#search").val('');
                            $('#btnAcceso').attr("disabled", true);
                        }
                    });
                })

                $("#btnCancelar").click(function () {
                    $("#myTable").empty();
                    $("#info").val('');
                    $("#search").val('');
                    $('#btnAcceso').attr("disabled", true);

                });

                $('#btnAcceso').attr("disabled", true);


                $("#btnRecepcion").click(function () {
                    $(':mobile-pagecontainer').pagecontainer('change', '#page3', {
                        transition: 'flip',
                        changeHash: false,
                        reverse: true,
                        showLoadMsg: true
                    });

                    llenarRecepcion();

                    $('#myRecepcion').footable();

                    $('#myRecepcion tbody').trigger('footable_redraw');

                    $('.bs-pagination td table').each(function (index, obj) {
                        convertToPagination(obj);
                    });



                });

                $("#btnResumen").click(function () {
                    $(':mobile-pagecontainer').pagecontainer('change', '#page2', {
                        transition: 'flip',
                        changeHash: false,
                        reverse: true,
                        showLoadMsg: true
                    });

                    $.ajax({
                        url: '<%=ResolveUrl("~/mfRecepcionImport.aspx/showSummary") %>',
                        data: {},
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            var resumen = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;
                            $("#myResumen").empty();
                            if (resumen.length > 0) {

                                $("#myResumen").append('<thead><th>Llegada</th>'
                                    + '<th>Buque</th>'
                                    + '<th>Total</th>'
                                    + '<th>Recepcionados</th>'
                                    + '<th>Pendientes</th></thead>');

                                $("#myResumen").append('<tbody>');


                                for (var b = 0; b < resumen.length; b++) {
                                    $("#myResumen").append('<tr><td>' + resumen[b].c_llegada + '</td><td>' + resumen[b].c_buque + '</td><td>' + resumen[b].Total + '</td><td>' + resumen[b].OIRSA + '</td><td>' + resumen[b].PO + '</td></tr>');
                                }

                                $("#myResumen").append('</tbody>');
                            }
                            else {
                                bootbox.alert("No existe buques pendientes");
                            }
                        },
                        error: function (response) {
                            bootbox.alert("CEPA - Contenedores: Alerta!! Se ha producido un error vuelva a intertarlo o reporte a Informática");
                        },
                        failure: function (response) {
                            bootbox.alert(response.responseText);
                        }
                    });

                });

            });



            function ClearText() {
                $("#search").val('');

            }
            function SetAutoComplete() {
                $("#search").autocomplete({
                    minLength: 3,
                    source: function (request, response) {
                        var params = new Object();
                        params.prefix = request.term;
                        params = JSON.stringify(params);
                        $.ajax({
                            url: '<%=ResolveUrl("~/mfRecepcionImport.aspx/GetConte") %>',
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

    </form>
</body>
</html>



 b<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mfRecepcionImport.aspx.cs" Inherits="CEPA.CCO.MobilPatio.mfRecepcionImport" %>

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
    <link href="~/bootstrap/csss/bootstrap-select.css" rel="stylesheet" type="text/css" />
    <title>CEPA-Contenedore Movil - Patio</title>
    <style type="text/css">
        legend {
            display: block;
            width: 100%;
            padding: 0;
            margin-bottom: 20px;
            font-size: 21px;
            line-height: inherit;
            color: #fff;
            border: 0;
            border-bottom: 1px solid #e5e5e5;
        }

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

        .bootstrap-select:not([class*="col-"]):not([class*="form-control"]):not(.input-group-btn) {
            width: 310px;
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

        [data-role=footer] {
            bottom: 0;
            position: absolute !important;
            top: auto !important;
            width: 100%;
        }
    </style>
    <script>
       
        (function ($) {

            function pageIsSelectmenuDialog(page) {
                var isDialog = false,
                    id = page && page.attr("id");

                $(".filterable-select").each(function () {
                    if ($(this).attr("id") + "-dialog" === id) {
                        isDialog = true;
                        return false;
                    }
                });

                return isDialog;
            }

            $.mobile.document

                // Upon creation of the select menu, we want to make use of the fact that the ID of the
                // listview it generates starts with the ID of the select menu itself, plus the suffix "-menu".
                // We retrieve the listview and insert a search input before it.
                .on("selectmenucreate", ".filterable-select", function (event) {
                    var input,
                        selectmenu = $(event.target),
                        list = $("#" + selectmenu.attr("id") + "-menu"),
                        form = list.jqmData("filter-form");

                    // We store the generated form in a variable attached to the popup so we avoid creating a
                    // second form/input field when the listview is destroyed/rebuilt during a refresh.
                    if (!form) {
                        input = $("<input data-type='search'></input>");
                        form = $("<form></form>").append(input);

                        input.textinput();

                        list
                            .before(form)
                            .jqmData("filter-form", form);
                        form.jqmData("listview", list);
                    }

                    // Instantiate a filterable widget on the newly created selectmenu widget and indicate that
                    // the generated input form element is to be used for the filtering.
                    selectmenu
                        .filterable({
                            input: input,
                            children: "> option[value]"
                        })

                        // Rebuild the custom select menu's list items to reflect the results of the filtering
                        // done on the select menu.
                        .on("filterablefilter", function () {
                            selectmenu.selectmenu("refresh");
                        });
                })

                // The custom select list may show up as either a popup or a dialog, depending on how much
                // vertical room there is on the screen. If it shows up as a dialog, then the form containing
                // the filter input field must be transferred to the dialog so that the user can continue to
                // use it for filtering list items.
                .on("pagecontainerbeforeshow", function (event, data) {
                    var listview, form;

                    // We only handle the appearance of a dialog generated by a filterable selectmenu
                    if (!pageIsSelectmenuDialog(data.toPage)) {
                        return;
                    }

                    listview = data.toPage.find("ul");
                    form = listview.jqmData("filter-form");

                    // Attach a reference to the listview as a data item to the dialog, because during the
                    // pagecontainerhide handler below the selectmenu widget will already have returned the
                    // listview to the popup, so we won't be able to find it inside the dialog with a selector.
                    data.toPage.jqmData("listview", listview);

                    // Place the form before the listview in the dialog.
                    listview.before(form);
                })

                // After the dialog is closed, the form containing the filter input is returned to the popup.
                .on("pagecontainerhide", function (event, data) {
                    var listview, form;

                    // We only handle the disappearance of a dialog generated by a filterable selectmenu
                    if (!pageIsSelectmenuDialog(data.toPage)) {
                        return;
                    }

                    listview = data.prevPage.jqmData("listview"),
                        form = listview.jqmData("filter-form");

                    // Put the form back in the popup. It goes ahead of the listview.
                    listview.before(form);
                });

        });
        
	</script>
	<style>
		.ui-selectmenu.ui-popup .ui-input-search {
			margin-left: .5em;
			margin-right: .5em;
		}
		.ui-selectmenu.ui-dialog .ui-content {
			padding-top: 0;
		}
		.ui-selectmenu.ui-dialog .ui-selectmenu-list {
			margin-top: 0;
		}
		.ui-selectmenu.ui-popup .ui-selectmenu-list li.ui-first-child .ui-btn {
			border-top-width: 1px;
			-webkit-border-radius: 0;
			border-radius: 0;
		}
		.ui-selectmenu.ui-dialog .ui-header {
			border-bottom-width: 1px;
		}
	</style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
            EnablePageMethods="true">
            <Scripts>
                <asp:ScriptReference Path="~/js/jquery.js" />
                <asp:ScriptReference Path="~/js/index.js" />
               <%-- <asp:ScriptReference Path="~/js/jquery-1.11.3.min.js" />--%>
                <asp:ScriptReference Path="~/js/jquery.mobile-1.4.5.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/bootstrap.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/bootbox.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/autocomplete.js" />
                <asp:ScriptReference Path="~/bootstrap/js/bootstrap-select.min.js" />
                <asp:ScriptReference Path="~/bootstrap/js/footable.js" />
                <asp:ScriptReference Path="~/bootstrap/js/footable.filter.js" />
                <asp:ScriptReference Path="~/bootstrap/js/footable.paginate.js" />
            </Scripts>
        </asp:ToolkitScriptManager>
        <div data-role="page" id="page1" data-theme="b">
            <div data-role="header" id="headerA" data-position="fixed" style="margin-top: 10%; background-color: #313945; padding: 2%;">
                <a href="mfDefault.aspx" class="ui-btn ui-btn-left ui-corner-all ui-shadow ui-icon-home ui-btn-icon-left">Menu</a>
                <h1></h1>
                <a href="mfInicio.aspx" class="ui-btn ui-btn-right ui-corner-all ui-shadow ui-icon-power ui-btn-icon-right">Salir</a>
            </div>
            <div data-role="content" id="content" style="margin-top: 11%">
                <asp:HiddenField ID="hMarcacion" runat="server" Value="" />
                <label for="search"># Contenedor:</label>
                <input type="search" name="txtConte" id="search" value="" class="form-control" placeholder="Ultimos digitos contenedor">
                <table data-role="table" class="ui-responsive" id="myTable">
                </table>
                <br />
                <div id="ubicacion" class="ui-content">
                    <div class="ui-btn ui-btn-inline">
                        <label for="filter-menu">ZONA</label>
                        <%--<select id="filter-menu" data-native-menu="false" data-filter="true" class="ui-shadow ui-btn ui-corner-all ui-btn-c">--%>
                        <select id="filter-menu" data-native-menu="false" class="filterable-select">                        
                            <option value="0">Seleccionar Zona</option>
                        </select>
                    </div>
                    <div class="ui-btn ui-btn-inline" style="height: 126px;">
                        <label for="txtCarril" style="margin-top: 10px">CARRIL</label>
                        <input type="number" name="txtCarril" id="txtCarril" value="" class="ui-shadow ui-btn ui-corner-all ui-btn-c">
                    </div>
                    <div class="ui-btn ui-btn-inline" style="height: 126px;">
                        <label for="txtPosicion" style="margin-top: 10px">POSICION</label>
                        <input type="number" name="txtPosicion" id="txtPosicion" value="" class="ui-shadow ui-btn ui-corner-all ui-btn-c">
                    </div>
                    <div class="ui-btn ui-btn-inline" style="height: 126px;">
                        <label for="txtNivel" style="margin-top: 10px">NIVEL</label>
                        <input type="number" name="txtNivel" id="txtNivel" value="" class="ui-shadow ui-btn ui-corner-all ui-btn-c">
                    </div>
                    <div id="condicion" class="ui-content text-center">
                        <div class="ui-btn ui-btn-inline" style="margin-bottom:auto;">
                            <fieldset data-role="controlgroup" data-type="horizontal">
                                <legend>TIPO DAÑO</legend>
                                <input type="checkbox" name="checkbox-h-2a[]" id="checkbox-h-2a">
                                <label>ABOLLADO</label>
                                <input type="checkbox" name="checkbox-h-2a[]" id="checkbox-h-2b">
                                <label>ROTO</label>
                            </fieldset>
                        </div>
                        <div class="ui-btn ui-btn-inline" style="margin-bottom:auto;">
                            <fieldset data-role="controlgroup" data-type="horizontal">
                                <legend>UBICACION DAÑO:</legend>
                                <input type="checkbox" name="checkbox-h-2a1" id="checkbox-h-2a1" />
                                <label for="checkbox-h-2a1">DERECHO</label>
                                <input type="checkbox" name="checkbox-h-2b2" id="checkbox-h-2b2" />
                                <label for="checkbox-h-2b2">IZQUIERDO</label>
                                <input type="checkbox" name="checkbox-h-2a3" id="checkbox-h-2a3" />
                                <label for="checkbox-h-2a3">DELANTERO</label>
                                <input type="checkbox" name="checkbox-h-2b4" id="checkbox-h-2b4" />
                                <label for="checkbox-h-2b4">TRASERO</label>
                                <input type="checkbox" name="checkbox-h-2b5" id="checkbox-h-2b5" />
                                <label for="checkbox-h-2b5">TECHO</label>
                            </fieldset>
                        </div>                       
                        <div class="ui-btn ui-btn-inline" style="left: 10.6%;width: 558.54px;margin-top:auto;">
                            <fieldset data-role="controlgroup" data-type="horizontal">
                                <legend></legend>
                                <input type="checkbox" name="checkbox-h-2a6" id="checkbox-h-2a6">
                                <label for="checkbox-h-2a6">PISO</label>
                                <input type="checkbox" name="checkbox-h-2b7" id="checkbox-h-2b7">
                                <label for="checkbox-h-2b7">AMBOS LADOS</label>
                                <input type="checkbox" name="checkbox-h-2a8" id="checkbox-h-2a8">
                                <label for="checkbox-h-2a8">BARRAS</label>
                                <input type="checkbox" name="checkbox-h-2b9" id="checkbox-h-2b9">
                                <label for="checkbox-h-2b9">OTROS</label>
                            </fieldset>
                            <div class="ui-btn ui-btn-inline">
                                <textarea name="txtCondicion" rows="20" cols="60" style="height:85px"></textarea>
                                <label for="txtCondicion">COMENTARIOS</label>
                            </div>

                        </div>
                    </div>
                </div>
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
                    <h4>© 2021 CEPA / Puerto de Acajutla, El Salvador v1.0</h4>
                </div>
            </div>
        </div>
        <%--PAGINA RESUMEN RECEPCION--%>
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
        <%--PAGINA DETALLE RECEPCION--%>
        <div data-role="page" id="page3" data-theme="b">
            <div data-role="header" id="headerD" data-position="fixed" style="margin-top: 9%; background-color: #313945; padding: 2%;">
                <a href="#page1" class="ui-btn ui-btn-left ui-corner-all ui-shadow ui-icon-home ui-btn-icon-left">Regresar</a>
                <h1></h1>
                <a href="mfInicio.aspx" class="ui-btn ui-btn-right ui-corner-all ui-shadow ui-icon-power ui-btn-icon-right">Salir</a>
            </div>
            <div data-role="content" id="content3" style="margin-top: 9%;">
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
                    <h4>© 2021 CEPA / Puerto de Acajutla, El Salvador v3.0</h4>
                </div>
            </div>
        </div>

        <script>
             /* $ = jQuery.noConflict();*/
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

            function cargarUbicaciones() {
                $.ajax({
                    async: true,
                    type: "POST",
                    url: "mfRecepcionImport.aspx/getUbicaciones",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var datos = $.parseJSON(msg.d);

                        $(datos).each(function () {
                            $("#filter-menu").append('<option value=' + this.IdZona + '>' + this.Zona + '</option>');
                        });
                    },
                    error: function (msg) {
                        bootbox.alert("Error al llenar el combo");
                    }
                });
            }


            $(function () {

                $("#ubicacion").hide();
                SetAutoComplete();

                $('input[type="checkbox"]').on('change', function () {
                    $('input[name="' + this.name + '"]').not(this).prop('checked', false);                  
                    
                    $('label[for="'+ this.id +'"]').removeAttr("ui-checkbox-on").removeAttr("ui-btn-active");
                });

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
                                        $("#ubicacion").show();



                                        for (var i = 0; pagos.length; i++) {

                                            $("#myTable").append('<tbody><tr><td style="font-weight: bold;" colspan="2">Buque: </td><td colspan="2"><input type="hidden" id="hIdDeta" />' + pagos[i].c_buque + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Naviera</td><td colspan="2">' + pagos[i].c_cliente + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Tara</td><td colspan="2">' + pagos[i].v_tara + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Tamaño</td><td colspan="2">' + pagos[i].c_tamaño + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Tráfico</td><td colspan="2"><input type="hidden" id="hDirecto" />' + pagos[i].c_trafico + '</td></tr>'
                                                + '<tr><td style="font-weight: bold;" colspan="2">Estado</td><td colspan="2">' + pagos[i].c_estado + '</td></tr>'
                                                + '</tbody>');
                                            $("#hIdDeta").val(pagos[i].IdDeta);
                                            if (pagos[i].c_trafico.indexOf("DIRECTO") != -1) {
                                                $("#hDirecto").val("1");
                                            }
                                            else {
                                                $("#hDirecto").val("0");
                                            }
                                            cargarUbicaciones();
                                            $('#filter-menu').selectpicker('mobile');
                                            $('#filter-menu-button').hide();

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

                function cargarCombo() {
                    $.ajax({
                        async: true,
                        type: "POST",
                        url: "mfRecepcionImport.aspx/Llenar",
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var datos = $.parseJSON(msg.d);

                            $(datos).each(function () {
                                var option = $(document.createElement('option'));

                                option.text(this.nombre);
                                option.val(this.expediente);


                                $("#combobox").append(option);
                            });
                        },
                        error: function (msg) {
                            bootbox.alert("Error al llenar el combo");
                        }
                    });
                }



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



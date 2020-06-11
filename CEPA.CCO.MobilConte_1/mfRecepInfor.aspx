<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mfRecepInfor.aspx.cs" Inherits="CEPA.CCO.MobilConte.mfRecepInfor" %>

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
            font-family: 'trebuchet MS', 'Lucida sans', Arial;
        }
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

        <div data-role="page" id="page" data-theme="b">
            <div data-role="header" id="headerD" data-position="fixed">
                <a href="#page1" class="ui-btn ui-btn-left ui-corner-all ui-shadow ui-icon-home ui-btn-icon-left">Regresar</a>
                <h1>Resumen Recepcion Importacion</h1>
                <a href="mfInicio.aspx" class="ui-btn ui-btn-right ui-corner-all ui-shadow ui-icon-power ui-btn-icon-right">Salir</a>
            </div>
            <div data-role="content" id="content3">
                <input id="filter" data-type="search" class="form-control">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="filtrar"
                    DataKeyNames="c_marcacion" CssClass="footable" Style="margin-bottom: 5%;"
                    data-filter="#filter" data-page-size="10" ShowFooter="true"
                    OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging">
                    <Columns>
                        <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR"></asp:BoundField>
                        <asp:BoundField DataField="f_recepcion" HeaderText="F/H RECEPCION"></asp:BoundField>
                        <asp:BoundField DataField="c_marcacion" HeaderText="COD. MARCACION"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>

            <div data-role="footer" data-position="fixed">
                <%-- <div align="center">
                    <h4>© 2016 CEPA / Puerto de Acajutla, El Salvador v1.0</h4>
                </div>--%>
            </div>
        </div>
        <script>
            $(document).bind("mobileinit", function () {
                $.mobile.ajaxEnabled = false;
            });
        </script>

        <script type="text/javascript">
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



            $(document).ready(function () {
                $('table.footable').footable();

                $('table.footable tbody').trigger('footable_redraw');

                $('.bs-pagination td table').each(function (index, obj) {
                    convertToPagination(obj);
                });

               
            });


        </script>

    </form>
</body>
</html>


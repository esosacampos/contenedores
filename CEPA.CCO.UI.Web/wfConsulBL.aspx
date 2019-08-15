<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsulBL.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsulBL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .scrollme {
            overflow-y: auto;
        }

        .tg {
            border-collapse: collapse;
            border-spacing: 0;
            border-color: #ccc;
            margin: 0px auto;
        }

            .tg td {
                font-family: Arial, sans-serif;
                font-size: 14px;
                padding: 0px 5px;
                border-style: solid;
                border-width: 0px;
                overflow: hidden;
                word-break: break-all;
                border-top-width: 1px;
                border-bottom-width: 1px;
                border-color: #ccc;
                color: #333;
                background-color: #fff;
            }

            .tg th {
                font-family: Arial, sans-serif;
                font-size: 14px;
                font-weight: normal;
                padding: 0px 5px;
                border-style: solid;
                border-width: 0px;
                overflow: hidden;
                word-break: normal;
                border-top-width: 1px;
                border-bottom-width: 1px;
                border-color: #ccc;
                color: #333;
                background-color: #f0f0f0;
            }

            .tg .tg-f9zi {
                font-weight: bold;
                color: #1f05fd;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-sj7j {
                background-color: #f0f0f0;
                font-weight: bold;
                color: #333333;
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-4t8i {
                font-weight: bold;
                color: #fe0000;
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-c3ow {
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-2obr {
                background-color: #f9f9f9;
                font-weight: bold;
                border-color: inherit;
                text-align: right;
                vertical-align: top
            }

            .tg .tg-0pky {
                border-color: inherit;
                text-align: left;
                vertical-align: top
            }

            .tg .tg-7btt {
                font-weight: bold;
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-bxsk {
                color: #1f05fd;
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-abip {
                background-color: #f9f9f9;
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-n97h {
                background-color: #f9f9f9;
                font-weight: bold;
                color: #fe0000;
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-agb9 {
                background-color: #f9f9f9;
                color: #1f05fd;
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-amwm {
                font-weight: bold;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-btxf {
                background-color: #f9f9f9;
                border-color: inherit;
                text-align: left;
                vertical-align: top
            }

            .tg .tg-zwlc {
                background-color: #f9f9f9;
                font-weight: bold;
                border-color: inherit;
                text-align: center;
                vertical-align: top
            }

            .tg .tg-6ic8 {
                font-weight: bold;
                border-color: inherit;
                text-align: right;
                vertical-align: top
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Consulta Por BL
    </h2>
    <br />
    <div class="col-lg-12">
        <div class="form-inline">
            <div class="form-group" style="width: 75%">
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Ingrese # de BL a consultar" autocomplete="off" Width="100%"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-default" OnClick="btnBuscar_Click" />
            </div>
        </div>
    </div>
    <%--<div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>--%>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="scrollme">
                <asp:GridView ID="GridView1" runat="server" Width="99%" AutoGenerateColumns="False" class="tg" Style="table-layout: fixed; width: 1559px;margin-left: 15px; margin-bottom: 5%; margin-top: 2%;"
                    DataKeyNames="n_contenedor">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <colgroup>
                                    <col style="width: 92px;">
                                    <col style="width: 113px">
                                    <col style="width: 65px;">
                                    <col style="width: 87px">
                                    <col style="width: 101px">
                                    <col style="width: 86px">
                                    <col style="width: 70px">
                                    <col style="width: 78px;">
                                    <col style="width: 104px">
                                    <col style="width: 68px;">
                                    <col style="width: 66px;">
                                    <col style="width: 84px;">
                                    <col style="width: 68px;">
                                    <col style="width: 65px;">
                                    <col style="width: 85px;">
                                    <col style="width: 62px;">
                                    <col style="width: 60px">
                                    <col style="width: 75px">
                                    <col style="width: 74px">
                                    <col style="width: 68px">
                                    <col style="width: 76px;">
                                </colgroup>
                                <tr>
                                    <td class="tg-0pky" colspan="9"></td>
                                    <th class="tg-7btt" colspan="3">TRANSFERENCIA</th>
                                    <th class="tg-7btt" colspan="3">DESPACHO</th>
                                    <th class="tg-7btt" colspan="3">MANEJO</th>
                                    <th class="tg-7btt" colspan="3">ALMACENAJE</th>
                                </tr>
                                <tr>
                                    <th class="tg-sj7j"># MANIFIESTO</th>
                                    <th class="tg-sj7j">CONTENEDOR</th>
                                    <th class="tg-sj7j">TAMAÑO</th>
                                    <th class="tg-sj7j">TEUS</th>
                                    <th class="tg-sj7j">PESO</th>
                                    <th class="tg-sj7j">F. TARJA</th>
                                    <th class="tg-sj7j">TARJA</th>
                                    <th class="tg-sj7j">F. SALIDA</th>
                                    <th class="tg-sj7j">TRAFICO</th>
                                    <th class="tg-sj7j">NAVIERO</th>
                                    <th class="tg-sj7j">CLIENTE</th>
                                    <th class="tg-sj7j">PENDIENTE</th>
                                    <th class="tg-sj7j">NAVIERO</th>
                                    <th class="tg-sj7j">CLIENTE</th>
                                    <th class="tg-sj7j">PENDIENTE</th>
                                    <th class="tg-sj7j">NAVIERO</th>
                                    <th class="tg-sj7j">CLIENTE</th>
                                    <th class="tg-sj7j">PENDIENTE</th>
                                    <th class="tg-sj7j">NAVIERO</th>
                                    <th class="tg-sj7j">CLIENTE</th>
                                    <th class="tg-sj7j">PENDIENTE</th>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="tg-c3ow"><%# Eval("n_manifiesto")%></td>
                                    <td class="tg-c3ow"><%# Eval("n_contenedor")%></td>
                                    <td class="tg-c3ow"><%# Eval("c_tamaño")%></td>
                                    <td class="tg-c3ow"><%# Eval("v_teus")%></td>
                                    <td class="tg-c3ow"><%# Eval("v_peso")%></td>
                                    <td class="tg-c3ow">12/07/2019 02:12:11 p.m.</td>
                                    <td class="tg-c3ow">4.20198548</td>
                                    <td class="tg-c3ow"><%# Eval("f_salida")%></td>
                                    <td class="tg-c3ow"><%# Eval("c_trafico")%></td>
                                    <td class="tg-c3ow"><%# Eval("n_transfer")%></td>
                                    <td class="tg-c3ow"><%# Eval("c_transfer")%></td>
                                    <td class="tg-bxsk">22.07</td>
                                    <td class="tg-c3ow"><%# Eval("n_desp")%></td>
                                    <td class="tg-c3ow"><%# Eval("c_desp")%></td>
                                    <td class="tg-bxsk">11.56</td>
                                    <td class="tg-c3ow"><%# Eval("n_manejo")%></td>
                                    <td class="tg-c3ow"><%# Eval("c_manejo")%></td>
                                    <td class="tg-bxsk">34.50</td>
                                    <td class="tg-c3ow"><%# Eval("n_alm")%></td>
                                    <td class="tg-c3ow"><%# Eval("c_alm")%></td>
                                    <td class="tg-bxsk"></td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">
        var confirmed = false;
        function confirmaSave(controlID) {
            if (confirmed) { return true; }

            var dialog = bootbox.dialog({
                message: "<p class='text-center mb-0'><span class='glyphicon glyphicon-refresh glyphicon-refresh-animate'></span> Su solicitud esta siendo procesada..</p>",
                closeButton: false
            });

            dialog.init(function () {
                setTimeout(function () {
                    dialog.find('.bootbox-body').html('Su solicitud esta siendo procesada..');
                    dialog.modal('hide');
                }, 3500);
            });

            // do something in the background
            //dialog.modal('hide');             
            /*bootbox.dialog({
                message  : "Por favor esperar mientras su solicitud se procesa",
                timeOut : 2000
            });*/
            if (controlID != null) {
                var controlToClick = document.getElementById(controlID);
                if (controlToClick != null) {
                    confirmed = true;
                    controlToClick.click();
                    confirmed = false;
                }
            }

            return false;
        }

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

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

        var postbackElement;

        function OnBeginRequest(sender, args) {
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

            //$('#ContentPlaceHolder1_grvTracking').footable();


        }


        function endRequest(sender, args) {

            if (sender._postBackSettings.panelsToUpdate != null) {

            }

            var a = postbackElement;

            var a = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {

                var testGrid = $get('<%= GridView1.ClientID %>');

                pageLoad();
                //$("#GridView1").load(location.href + " #GridView1");
            }

            $.unblockUI();

        }



        function pageLoad() {
            $(document).ready(function () {
                //$('#ContentPlaceHolder1_GridView1').footable();

            });
        }
    </script>
</asp:Content>

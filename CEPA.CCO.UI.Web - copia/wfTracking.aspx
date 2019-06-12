<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfTracking.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfTracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        tr#rowF
        {
            display: none;
        }
        table#ContentPlaceHolder1_grvTracking tr td:nth-child(7), table#ContentPlaceHolder1_grvTracking tr th:nth-child(7)
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Tracking Contenedores Importacion
    </h2>
    <br />
    <div class="col-lg-9">
        <div class="input-group">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="# de contenedor sin guiones"></asp:TextBox>
            <span class="input-group-btn">
                <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-default"
                    OnClick="btnBuscar_Click" />
            </span>
        </div>
    </div>
    <br />
    <br />
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="grvTracking" runat="server" AutoGenerateColumns="False" DataKeyNames="IdDeta"
                CssClass="footable" Style="margin-top: 5%; max-width: 98%; margin-left: 15px;"
                OnRowDataBound="grvTracking_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img alt="" src="CSS/Images/plus.gif" iddeta="<%# Eval("IdDeta") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdDeta" HeaderText="Id"></asp:BoundField>
                    <asp:BoundField DataField="n_manifiesto" HeaderText="# Manifiesto"></asp:BoundField>
                    <asp:BoundField DataField="c_llegada" HeaderText="Cod. Llegada"></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tarja" HeaderText="# TARJA"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="b_estado" HeaderText="ESTADO"></asp:BoundField>
                    <asp:BoundField DataField="b_trafico" HeaderText="TRAFICO"></asp:BoundField>
                    <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="f_llegada" HeaderText="F. LLEGADA" HtmlEncode="false"
                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <button type="button" class="btn btn-primary btn xs" onclick="return GetSelectedRow(this)">
                                <span class="glyphicon glyphicon-search" style="cursor: pointer;"></span>
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="" ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="c_naviera" HeaderText="" ReadOnly="True"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr id="rowF" iddeta="<%# Eval("IdDeta") %>">
                                <td colspan="100%">
                                    <div style="position: relative; padding-left: 30px;">
                                        <asp:DetailsView ID="dtTracking" runat="server" AutoGenerateRows="False" DataKeyNames="IdDeta"
                                            CssClass="footable" CellPadding="0" GridLines="None" Width="100%">
                                            <Fields>
                                                <asp:BoundField DataField="s_consignatario" HeaderText="Consignatario" ReadOnly="True">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ReadOnly="True">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="f_rep_naviera" HeaderText="F. Reporto Naviera" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_aut_aduana" HeaderText="F. Autorizado por ADUANA" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_recep_patio" HeaderText="F. Recepción en Patio" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_trans_aduana" HeaderText="F. Transmision ADUANA" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_ret_dan" HeaderText="F. Retención DAN" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_tramite_dan" HeaderText="F. Tramite DAN" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_liberado_dan" HeaderText="F. Liberación DAN" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_salida_carga" HeaderText="F. Salida de Carga" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_caseta" HeaderText="F. Ingreso Puerta #1" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_solic_ingreso" HeaderText="F. Solicitud de Ingreso a Patio"
                                                    ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_auto_patio" HeaderText="F. Autorización en Patio" ReadOnly="True"
                                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="f_puerta1" HeaderText="F. Confirmación Salida Puerta #1"
                                                    ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                <asp:BoundField DataField="ubicacion" HeaderText="Ubicacion en Patio" ReadOnly="True">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="s_comentarios" HeaderText="Observaciones" ReadOnly="True">
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label Text="Provisionales" ID="lblq" runat="server"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:GridView ID="grvProv" runat="server" AutoGenerateColumns="False" CssClass="footable"
                                                            Style="margin-top: 5%; max-width: 98%; margin-left: 15px;" DataKeyNames="">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <img alt="" src="CSS/Images/plus.gif" iddetap="<%# Eval("IdDeta") %>" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="2%" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Total" HeaderText="Total">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <tr style="display: none;" iddetap="<%# Eval("IdDeta") %>">
                                                                            <td colspan="100%">
                                                                                <div style="position: relative; padding-left: 20px;">
                                                                                    <asp:GridView ID="grvDetailProvi" runat="server" AutoGenerateColumns="False" DataKeyNames="c_llegada"
                                                                                        CssClass="footable" Style="margin-top: 5%; max-width: 98%; margin-left: 15px;">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="fecha_prv" HeaderText="Fecha Provisional" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="tipo" HeaderText="Tipo"></asp:BoundField>
                                                                                            <asp:BoundField DataField="motorista_prv" HeaderText="Motorista"></asp:BoundField>
                                                                                            <asp:BoundField DataField="transporte_prv" HeaderText="Transporte"></asp:BoundField>
                                                                                            <asp:BoundField DataField="placa_prv" HeaderText="Placa"></asp:BoundField>
                                                                                            <asp:BoundField DataField="chasis_prv" HeaderText="Chasis"></asp:BoundField>
                                                                                            <asp:BoundField DataField="fec_reserva" HeaderText="Fecha Ing. Patio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField DataField="fec_valida" HeaderText="Fecha Aut. Patio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                                                            </asp:BoundField>
                                                                                        </Columns>
                                                                                        <EmptyDataTemplate>
                                                                                            <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales</EmptyDataTemplate>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales</EmptyDataTemplate>
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
                    <asp:Label ID="lblEmptyMessage" Text="" runat="server" /></EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <!-- Modal HTML -->
    <div id="myModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" id="myClose" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 class="modal-title">
                        ¿Puedo solicitar la salida de carga en CEPA?
                    </h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <div class="form-group">
                            <div class="table-responsive">
                                <table class="table" id="myTableModal">
                                </table>
                            </div>
                            <span id="MensajeModal" style="color: #FF0000; font-weight: bold;"></span>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

        function OnBeginRequest(sender, args) {
            $.blockUI({
                message: '<img src="<%= ResolveClientUrl("~/CSS/Img/ajax-loader.gif") %>" /><h1>Procesando...</h1>',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#1771F8',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            $('#ContentPlaceHolder1_grvTracking').footable();

            // $('#ContentPlaceHolder1_grvTracking tbody > tr#rowF').css('display', 'none');

            //$('#ContentPlaceHolder1_grvTracking').trigger('footable_redraw');





        }


        function endRequest(sender, args) {

            $.unblockUI();

            //            $('#ContentPlaceHolder1_grvTracking').footable();

            //            $('#ContentPlaceHolder1_grvTracking tbody').trigger('footable_redraw');

        }

        function GetSelectedRow(lnk) {
            var row = lnk.parentNode.parentNode;
            var c_llegada = row.cells[3].innerHTML;
            var contenedor = row.cells[4].innerHTML;
            var c_tarja = row.cells[5].innerHTML;
            //alert("LLegada: " + llegada + " Contenedor: " + contenedor);
            validar(c_tarja, contenedor, c_llegada);
            return false;
        }

        function validar(tarja, contenedor, c_llegada) {
            if (tarja.length > 0 && contenedor.length > 0) {

                $.blockUI({
                    message: '<img src="<%= ResolveClientUrl("~/CSS/Img/ajax-loader.gif") %>" /><h1>Procesando...</h1>',
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#1771F8',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#fff'
                    }
                });

                var params = new Object();
                params.c_tarja = tarja;
                params.n_contenedor = contenedor;
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

                        $("#myTableModal").empty();

                        if (pagos.length > 0) {
                            for (var i = 0; pagos.length; i++) {

                                $("#myTableModal").append('<tbody><tr><td colspan="2" style="' + pagos[i].style_va + ';font-weight: bold;">' + pagos[i].validacion + '</td></tr>'
                                + '<tr><td>Transferencia</td><td><img id="muelle" src="' + pagos[i].style_transfer + '"/></td></tr>'
                                + '<tr><td>Despacho</td><td><img id="muelle" src="' + pagos[i].style_despac + '"/></td></tr>'
                                + '<tr><td>Manejo</td><td><img id="muelle" src="' + pagos[i].style_manejo + '"/></td></tr>'
                                + '<tr><td>Almacenaje</td><td><img id="muelle" src="' + pagos[i].style_alma + '"/></td></tr>'
                                + '</tbody>');
                                break;
                            }
                            $("#MensajeModal").text("Si el contenedor esta retenido por la DAN no puede ser emitida la salida");
                        }

                        $.unblockUI();

                        $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                            "backdrop": "static",
                            "keyboard": false,
                            "show": true                     // ensure the modal is shown immediately
                        });
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

        function pageLoad() {
            $(document).ready(function () {



                $('#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv img').click(function () {
                    var img = $(this)
                    var idDeta = $(this).attr('iddetap');

                    var tr = $('#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv tr[iddetap =' + idDeta + ']')
                    tr.toggle();

                    if (img.src == 'CSS/Images/plus.gif')
                        img.attr('src', 'CSS/Images/minus.gif');
                    else
                        img.attr('src', 'CSS/Images/plus.gif');

                    if (tr.is(':visible'))
                        img.attr('src', 'CSS/Images/minus.gif');
                    else
                        img.attr('src', 'CSS/Images/plus.gif');

                });



                $('#<%=grvTracking.ClientID %> img').click(function () {

                    var img = $(this)
                    var idDeta = $(this).attr('iddeta');

                    var tr = $('#<%=grvTracking.ClientID %> tr[iddeta =' + idDeta + ']')
                    tr.toggle();


                    if (tr.is(':visible'))
                        img.attr('src', 'CSS/Images/minus.gif');
                    else
                        img.attr('src', 'CSS/Images/plus.gif');
                });

                $('#ContentPlaceHolder1_grvTracking').footable();
                $("#ContentPlaceHolder1_grvTracking tbody > tr#rowF").css("display", "none");

                $("#myOK").click(function () {
                    $("#myModal").modal('hide');
                });

            });
        }
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfTracking.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfTracking" ValidateRequest="false" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        tr#rowF {
            display: none;
            background-color: rgb(23, 113, 248);
        }

        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 3px;
            text-align: center;
            border-left: none;
            border-top: none;
            border-right: none;
            border-bottom: none;
        }

        .footable > thead > tr > th {
            border-bottom: 1px solid #dddddd;
            padding: 10px;
            text-align: left;
            font-size: 10.5px;
        }

        .footable > thead > tr > th, .footable > thead > tr > td {
            background-color: #1771F8;
            border: 1px solid #1771F8;
            color: #ffffff;
            border-top: none;
            border-left: none;
            font-weight: bold;
            text-align: center;
        }

        .form-group {
            margin-bottom: 10px;
            line-height: 10px;
        }

        .ancho {
            width: 15%;
        }


        table#ContentPlaceHolder1_grvTracking tr td:nth-child(7), table#ContentPlaceHolder1_grvTracking tr th:nth-child(7) {
            display: none;
        }

        #sthoverbuttons #sthoverbuttonsMain {
            position: relative;
            z-index: 1000000;
            width: 30px;
            padding: 7px;
        }

        table#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv.footable tbody > tr > th {
            background-color: #1771F8;
            border: 1px solid #1771F8;
            color: #ffffff;
            border-top: none;
            border-left: none;
            font-weight: normal;
            text-align: center;
        }

        table#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv.footable {
            border-collapse: initial;
        }

        #myTableModal td, th {
            text-align: center;
        }

            #myTableModal td.descri {
                text-align: left;
            }



        .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
            padding: 2.5px;
            /* line-height: 1.42857143; */
            vertical-align: top;
            border-top: 1px solid #ddd;
            border: inset 0pt;
        }

        .label {
            font-size: 90%;
        }

        p {
            margin: 0 0 1px;
        }

        #myTableModal > tfoot > tr > th {
            text-align: right;
        }

        .footable-visible.footable-last-column {
            border-right: 3px solid #1771F8;
        }

        th.footable-visible.footable-last-column {
            border-right: 3px solid #1771F8;
        }

        .badge {
            display: inline-block;
            min-width: 10px;
            padding: 3px 7px;
            font-size: 12px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #0A8ADB;
            border-radius: 10px;
        }

        .badgeR {
            display: inline-block;
            min-width: 10px;
            padding: 3px 7px;
            font-size: 12px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #C60000;
            border-radius: 10px;
        }

        .badgeRP {
            display: inline-block;
            min-width: 10px;
            padding: 3px 6px;
            font-size: 16px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #AB0000;
            border-radius: 10px;
            margin-left: 5px;
            margin-right: 5px;
        }

        .badgeV {
            display: inline-block;
            min-width: 10px;
            padding: 3px 6px;
            font-size: 16px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #6495ed;
            border-radius: 10px;
            margin-left: 5px;
            margin-right: 5px;
        }

        .modal-dialog {
            width: 600px;
            margin: 3px auto;
        }

        table#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv.footable > tbody > tr > th:last-child {
            border-right: 3px solid #1771f8;
        }

        table#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv.footable > tbody > tr > td:last-child {
            border-right: 3px solid #1771f8;
        }

        table#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv_grvDetailProvi_0.footable > tbody > tr > th:last-child {
            border-right: 3px solid #1771f8;
        }

        table#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv_grvDetailProvi_0.footable > tbody > tr > td:last-child {
            border-right: 3px solid #1771f8;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Tracking Contenedores Importacion
    </h2>
    <br />
    <div class="col-lg-12">
        <div class="form-inline">
            <div class="form-group" style="width: 65%">
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="# de contenedor sin guiones" autocomplete="off" Width="100%"></asp:TextBox>
            </div>
            <div class="form-group">
                <div runat="server" id="myRadio">
                    <div>
                        <asp:CheckBox class="label-success" runat="server" ID="radio3" />
                        <label for="radio3">Shipper Own</label>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-default"
                    OnClick="btnBuscar_Click" />
                <asp:Button ID="btnConsultar" runat="server" Text="D/T Asociados" CssClass="btn btn-success"
                     Visible="false" OnClientClick="return confirmaSave(this.id);" />                 
                <%--<button runat="server" id="btnConsultar" onclick="window.open('_blank', 'wfConsulDecla.aspx', 'width=100,height=100');">Asociados</button>--%>
                <%--<input type="button" id="exportpdf" value="Imprimir" class="btn btn-info">  OnClientClick="return confirmaSave(this.id);"--%>
                <%--<asp:Button ID="btnImprime" runat="server" Text="Imprimir" CssClass="btn btn-info" OnClick="btnImprime_Click"  />--%>
                <asp:HiddenField ID="txtPrint" runat="server" />
                <asp:HiddenField ID="txtPrint1" runat="server" />
                <asp:HiddenField ID="txtPrint2" runat="server" />
            </div>
        </div>
    </div>
    <br />
    <br />
    <div id="printArea" runat="server">
        <span>
            <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grvTracking" runat="server" AutoGenerateColumns="False" DataKeyNames="IdDeta"
                        CssClass="footable" Style="margin-top: 5%; max-width: 98%; margin-left: 15px;"
                        OnRowDataBound="grvTracking_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hidden" runat="server" Value='<%#Eval("f_tarja")%>' />
                                    <asp:HiddenField ID="hPeso" runat="server" Value='<%#Eval("v_peso")%>' />
                                    <asp:HiddenField ID="hEstado" runat="server" Value='<%#Eval("b_cancelado")%>' />
                                    <asp:HiddenField ID="hTarjas" runat="server" Value='<%#Eval("c_tarjasn")%>' />
                                    <asp:HiddenField ID="HNTarjas" runat="server" Value='<%#Eval("con_tarjas")%>' />
                                    <img alt="" src="CSS/Images/plus.gif" iddeta="<%# Eval("IdDeta") %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="IdDeta" HeaderText="Id"></asp:BoundField>--%>
                            <asp:BoundField DataField="n_manifiesto" HeaderText="# MANIFIESTO"></asp:BoundField>
                            <asp:BoundField DataField="c_llegada" HeaderText="COD. Llegada"></asp:BoundField>
                            <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR"></asp:BoundField>
                            <asp:BoundField DataField="c_tarja" HeaderText="# TARJA"></asp:BoundField>
                            <asp:BoundField DataField="b_requiere" HeaderText="ENTREGA"></asp:BoundField>
                            <asp:BoundField DataField="b_shipper" HeaderText="SHIPPER OWNER"></asp:BoundField>
                            <asp:BoundField DataField="pais_origen" HeaderText="PAÍS DE ORIGEN"></asp:BoundField>
                            <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                            <asp:BoundField DataField="b_estado" HeaderText="ESTADO S/MANIFIESTO"></asp:BoundField>
                            <asp:BoundField DataField="s_marchamo" HeaderText="# MARCHAMO"></asp:BoundField>
                            <asp:BoundField DataField="b_trafico" HeaderText="TRAFICO"></asp:BoundField>
                            <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA"></asp:BoundField>
                            <asp:BoundField DataField="d_buque" HeaderText="BUQUE"></asp:BoundField>
                            <asp:BoundField DataField="f_llegada" HeaderText="F. ATRAQUE" HtmlEncode="false"
                                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                            <asp:BoundField DataField="f_desatraque" HeaderText="F. DESATRAQUE" HtmlEncode="false"
                                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%--<button type="button" runat="server" class="btn btn-primary btn xs" onclick="return GetSelectedRow(this)" id="tooltop" data-toggle="tooltip" data-placement="top" data-original-title="Consultar el estado para despachar su contenedor">
                                        <span class="glyphicon glyphicon-usd" style="cursor: pointer;"></span>
                                    </button>--%>
                                     <button type="button" id="fact" runat="server" class="btn btn-primary btn xs" onclick="return getBL(this)" >
                                        <span class="glyphicon glyphicon-usd" style="cursor: pointer;"></span>
                                    </button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- <asp:TemplateField>
                                <ItemTemplate>
                                    <button type="button" class="btn btn-primary btn xs" onclick="return goDecla(this)" id="tooltop" data-toggle="tooltip" data-placement="top" data-original-title="Consultar el estado para despachar su contenedor">
                                        <span class="glyphicon glyphicon-barcode" style="cursor: pointer;"></span>
                                    </button>                                    
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--      <asp:BoundField DataField="n_contenedor" HeaderText="" ReadOnly="True"></asp:BoundField>
                            <asp:BoundField DataField="c_naviera" HeaderText="" ReadOnly="True"></asp:BoundField>--%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <tr id="rowF" iddeta="<%# Eval("IdDeta") %>">
                                        <td colspan="100%">
                                            <div style="position: relative; padding-left: 30px;">
                                                <asp:DetailsView ID="dtTracking" runat="server" AutoGenerateRows="False" DataKeyNames="IdDeta"
                                                    CssClass="footable" CellPadding="0" GridLines="None" Width="100%">
                                                    <Fields>
                                                        <asp:BoundField DataField="s_consignatario" HeaderText="Consignatario" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ReadOnly="True"></asp:BoundField>
                                                        <asp:BoundField DataField="f_rep_naviera" HeaderText="F. Reporto Naviera" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_aut_aduana" HeaderText="F. Autorizado por ADUANA" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_recepA" HeaderText="F. Recepción en Arco" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_trans_aduana" HeaderText="F. Transmision ADUANA" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_recep_patio" HeaderText="F. Recepción en Patio" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_cancelado" HeaderText="F. Cancelación" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_cambio" HeaderText="F. Cambio Condición" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_ret_dan" HeaderText="F. Orden de Retención DAN" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_tramite_dan" HeaderText="F. Tramite DAN" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_marchamo_dan" HeaderText="F. Corte Marchamo DAN" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_deta_dan" HeaderText="F. Liberación DAN" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_ret_ucc" HeaderText="F. Orden de Retención UCC" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_tramite_ucc" HeaderText="F. Tramite UCC" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_marchamo_ucc" HeaderText="F. Corte Marchamo UCC" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_deta_ucc" HeaderText="F. Liberación UCC" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_retencion_dga" HeaderText="F. Orden de Retención DGA" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_lib_dga" HeaderText="F. Liberación DGA" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_salida_carga" HeaderText="F. Salida de Carga" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_caseta" HeaderText="F. Solicitud Ingreso Puerta #1" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_solic_ingreso" HeaderText="F. Solicitud de Ingreso a Patio"
                                                            ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_auto_patio" HeaderText="F. Autorización en Patio" ReadOnly="True"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_puerta1" HeaderText="F. Confirmación Salida Puerta #1"
                                                            ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label Text="Ubicacion en Patio" ID="lblText" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label Text="" ID="lblUbica" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="s_comentarios" HeaderText="Observaciones" ReadOnly="True"></asp:BoundField>
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
                                                                                <img alt="" src="CSS/Images/plus.gif" iddetap="<%# Eval("IdDeta") %>" class="imageSecun" />
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
                                                                                                    <asp:BoundField DataField="Fecha_Prv" HeaderText="Fecha Provisional" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Motorista_Prv" HeaderText="Motorista"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Transporte_Prv" HeaderText="Transporte"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Placa_Prv" HeaderText="Placa"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Chasis_Prv" HeaderText="Chasis"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Fec_Reserva" HeaderText="Fecha Ing. Patio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="Fec_Valida" HeaderText="Fecha Aut. Patio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                                </Columns>
                                                                                                <EmptyDataTemplate>
                                                                                                    <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales
                                                                                                </EmptyDataTemplate>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales
                                                                    </EmptyDataTemplate>
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
                            <asp:Label ID="lblEmptyMessage" Text="Búsqueda no produjó resultados intentarlo de nuevo o llamar a Informática" runat="server" />
                        </EmptyDataTemplate>
                    </asp:GridView>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnConsultar" EventName="Click" />
                    <%--<asp:PostBackTrigger ControlID="btnImprime"/>--%>
                </Triggers>
            </asp:UpdatePanel>
    </div>
    <br />
    <br />
    <asp:Label ID="Label3" runat="server" Text="* Se recuerda a todos los consignatarios que la DAN/UCC no realiza ningún cobro por las labores de inspección desarrolladas" CssClass="alert-danger" Style="margin-left: 15px;"></asp:Label>
    <br />
    <asp:Label ID="lblMensaje" runat="server" Text="* Los contenedores marcados en ROJO son CANCELADOS" CssClass="alert-danger" Style="margin-left: 15px;"></asp:Label>
    <br />
    <asp:Label ID="Label1" runat="server" Text="* Ordenados desde la última vez que visita el puerto hasta la primera" CssClass="alert-danger" Style="margin-left: 15px;"></asp:Label>
    <br />
    <asp:Label ID="Label2" runat="server" Text="* La columna Entrega indica con RT si requiere tarja y en blanco si no la requiere" CssClass="alert-danger" Style="margin-left: 15px;"></asp:Label>
    <hr />
    <!-- Modal HTML -->
    <div id="myModal" class="modal fade" tabindex="-1" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="line-height: 10px;">
                    <button type="button" class="close" id="myClose" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 class="modal-title">¿Puedo solicitar la salida de carga en CEPA?
                    </h4>
                    <br />
                    <span id="mensajeCEPA" style="font-weight: bold; padding: 8px;"></span>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <input type="hidden" id="hTarja" />
                        <input type="hidden" id="hContenedor" />
                        <input type="hidden" id="hLlegada" />
                        <input type="hidden" id="hFtarja" />
                        <input type="hidden" id="hManifiesto" />
                        <input type="hidden" id="hvPeso" />
                        <input type="hidden" id="hTarjas" />
                        <input type="hidden" id="hNTarjas" />

                        <div class="form-group" style="line-height: 1.4;">
                            <div class="row">
                                <div class="col-md-6">
                                    <label id="lblIngreso">Fecha Ingreso </label>
                                    <span class="label label-info" id="f_tarjaM"></span>
                                </div>
                                <div class="col-md-6">
                                    <label id="lblSalida" style="padding-left: 10px; margin-right: 2px;"></label>
                                    <span class="label label-success" id="f_salidaM"></span>
                                </div>
                            </div>
                            <div class="row" id="calPa" style="margin-top: 6px; margin-bottom: 10px;">
                                <div class="col-md-5" style="padding-top: 7px;">Fecha Proxima Programada</div>
                                <div class="col-md-7">
                                    <div class="input-group date" id="dpRetiro" data-toggle="tooltip" data-placement="top" data-original-title="Doble clic fecha a seleccionar">
                                        <input type="text" class="form-control" />
                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="alert alert-info" role="alert" style="background-color: #d3d3d3; border-color: #ababab; color: #060606;">
                                <p>
                                    Tarjas Asociadas<span class="badgeV" id="to_Tarjas"></span>:<span class="label label-default" id="d_tarjas" style="font-weight: normal; margin-left: 5px; padding-left: 2px; border-radius: 10px; padding: 7px; background-color: #6495ed; color: #fff;"></span>
                                </p>
                            </div>
                            <div class="table-responsive">
                                <table class="table" id="myTableModal">
                                </table>
                            </div>
                            <div class="alert alert-info" role="alert">
                                <p>
                                    Los cálculos aquí plasmados, son con base a fecha de recepción del contenedor y
                                   fecha de salida si la posee, si no se usa la fecha actual. <span class="label label-warning" id="f_leyenda" style="font-weight: normal; margin-left: -4px; padding-left: 2px;"></span><span class="label label-warning" id="f_proxima" style="margin-left: -6px; padding-left: 2px;"></span>
                                </p>
                            </div>
                        </div>
                        <div class="form-group">
                            <h4 class="modal-title">¿Se encuentra solvente con PNC-DAN, UCC y ADUANA ?</h4>
                            <br />
                            <span id="MensajeModal" style="font-weight: bold; padding: 4px; line-height: 1.5em;"></span>
                        </div>
                        <div class="form-group">
                            <h4 class="modal-title">¿El retiro del contenedor se encuentra autorizado por ADUANA?</h4>
                            <br />
                            <span id="mensajeADUANA" style="font-weight: bold; padding: 8px;"></span>
                            <%--<button type="button" class="btn btn-primary" id="btnDetalle" onclick="VerDetalle()">
                                Detalle</button>--%>
                            <%--<button class="btn btn-default" data-toggle="modal" href="#stack2">Launch modal</button>--%>
                        </div>
                        <div class="form-group" style="line-height: 1.4;">
                            <div class="alert alert-danger" role="alert">
                                <p>
                                    Por favor retirar su contenedor, tan pronto le sea cargado caso contrario podrán generarse 
                                    cargos por parqueo dentro del recinto a razón de $4.43 + IVA por hora de permanencia
                                </p>
                            </div>
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
    <!-- Modal HTML -->


    <!--Modal Declaraciones -->

    <div id="stack2" class="modal fade" tabindex="-1" data-focus-on="input:first" style="display: none;">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h4 class="modal-title">Stack Two</h4>
        </div>
        <div class="modal-body">
            <div role="form">

                <div class="form-group" style="line-height: 1.4;">
                    <div class="table-responsive">
                        <table class="table" id="myDecla">
                            <tr>
                                <th>Estado</th>
                                <th>Tipo Documento</th>
                                <th># Documento</th>
                            </tr>
                            <tr>
                                <td>Jill</td>
                                <td>Smith</td>
                                <td>50</td>
                            </tr>
                            <tr>
                                <td>Eve</td>
                                <td>Jackson</td>
                                <td>94</td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <button type="button" data-dismiss="modal" class="btn btn-default">Cerrar</button>
            <button type="button" class="btn btn-primary">Ok</button>
        </div>

    </div>
    <!-- Modal Declaraciones -->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        //Page.add_endRequest(endRequest);

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

            $('#ContentPlaceHolder1_grvTracking').footable();



            // $('#ContentPlaceHolder1_grvTracking tbody > tr#rowF').css('display', 'none');

            //$('#ContentPlaceHolder1_grvTracking').trigger('footable_redraw');


            $('#myModal').block({
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


        }

        var postbackElement;

        if (Page != null) {
            Page.add_endRequest(function (sender, e) {
                //llenarSelect();

                if (sender._postBackSettings.panelsToUpdate != null) {
                    //llenarSelect();
                }

                if (e.get_error() != undefined) {
                    e.set_errorHandled(true);
                }

                var a = postbackElement;

                $('#ContentPlaceHolder1_grvTracking').footable();
                $("#ContentPlaceHolder1_grvTracking tbody > tr#rowF").css("display", "none");

                var a = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
                if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
                    //__doPostBack('btnCargar', '');

                    //alert(testGrid.rows[1].cells[0].innerHTML);

                    //setTimeout(function () { location.reload(); }, 1000);
                    //pageLoad();
                    //$("#GridView1").load(location.href + " #GridView1");
                    $('#ContentPlaceHolder1_grvTracking').footable();
                    $("#ContentPlaceHolder1_grvTracking tbody > tr#rowF").css("display", "none");
                }

                $.unblockUI();

                $('#myModal').unblock();


            });
        };

        function iniciaVariable() {
            confirmed = false;
        }


        var confirmed = false;

        function confirmaSave(controlID) {
            if (confirmed) { return true; }

            var HTMLContent = document.getElementById('printArea');
            var a = HTMLContent.innerHTML.length;
            var b = a / 5;
            var x = Math.floor(b);
            var c = HTMLContent.innerHTML.substr(0, x);
            var d = HTMLContent.innerHTML.substr((x + 1), (x + x));
            var e = HTMLContent.innerHTML.substr((x + x + 1), x);
            var acumula;

            bootbox.confirm("¿Seguro que desea consultar documentos asociados?", function (result) {
                if (result) {
                    if (controlID != null) {
                        var controlToClick = document.getElementById(controlID);
                        if (controlToClick != null) {
                            confirmed = true;
                            $("#<%= txtPrint.ClientID %>").val(c.toString());
                            $("#<%= txtPrint1.ClientID %>").val(d.toString());
                            $("#<%= txtPrint2.ClientID %>").val(d.toString());
                            controlToClick.click();
                            confirmed = false;
                        }
                    }
                } else {
                    bootbox.alert("De no continuar con la liberación puede dar F5 para actualizar")
                }

            });

            return false;
        }

        function getBL(lnk) {
            //window.open(myurl, '_blank');
            var row = lnk.parentNode.parentNode;
            var c_llegada = row.cells[2].innerHTML;
            var contenedor = row.cells[3].innerHTML;
            var c_tarja = row.cells[4].innerHTML;
            var url = 'llegada=' + c_llegada + '&contenedor=' + contenedor
            
            if (c_tarja != "&nbsp;")
                window.open('wfConsulBL.aspx?' + url, '_blank');
            else
                bootbox.alert("CEPA - Contenedores: no puede proceder porque no se posee tarja vuelva intentar mas tarde.")
        }
       

        function goDecla(lnk) {
            var row = lnk.parentNode.parentNode;
            var contenedor = row.cells[3].innerHTML;
            $.session.set("n_conte", contenedor);
            bootbox.alert($.session.get("n_conte"));
            window.open('wfConsulDecla.aspx', 'popup', 'width=800,height=500');


        }

        function GetSelectedRow(lnk) {
            var row = lnk.parentNode.parentNode;
            var c_llegada = row.cells[2].innerHTML;
            var contenedor = row.cells[3].innerHTML;
            var c_tarja = row.cells[4].innerHTML;
            var n_manifiesto = row.cells[1].innerHTML;
            //alert("LLegada: " + llegada + " Contenedor: " + contenedor);
            var f_tarja = row.cells[0].childNodes[2].value;
            var hTarjas = row.cells[0].childNodes[8].value;
            var NTarjas = row.cells[0].childNodes[10].value;
            var v_peso = $("[id*=hPeso]").val();

            if (c_tarja != "&nbsp;")
                validar(c_tarja, contenedor, c_llegada, f_tarja, n_manifiesto, v_peso, hTarjas, NTarjas);
            else
                bootbox.alert("CEPA - Contenedores: no puede proceder porque no se posee tarja vuelva intentar mas tarde.")
            //var a_mani = $("input[name*='a_declaracion']").val();
            return false;
        }

        function Imprimir() {
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
            var HTMLContent = document.getElementById('printArea');
            if (HTMLContent.innerText != "") {
                var s = $("#<%= txtBuscar.ClientID %>").val();
                var params = new Object();
                params.htmlOut = HTMLContent.innerHTML;
                params.n_contenedor = $("#<%= txtBuscar.ClientID %>").val();
                params = JSON.stringify(params);


                $.ajax({
                    async: true,
                    cache: false,
                    type: "POST",
                    url: "wfTracking.aspx/Imprime",
                    data: params,
                    contentType: "application/json; charset=utf8",
                    dataType: "json",
                    success: function (response) {

                        //bootbox.alert(response.d);
                        $.unblockUI();


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

        function validarRetiro(tarja, contenedor, c_llegada, f_tarja, n_manifiesto, f_retiro, v_peso, hTarjas, con_tarjas) {
            if (tarja.length > 0 && contenedor.length > 0) {

                $('#myModal').block({
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

                var params = new Object();
                params.c_tarja = tarja;
                params.n_contenedor = contenedor;
                params.c_llegada = c_llegada;
                params.f_tarja = f_tarja;
                params.n_manifiesto = n_manifiesto;
                params.f_retiro = f_retiro;
                params.v_peso = v_peso;
                params = JSON.stringify(params);


                $.ajax({
                    async: true,
                    cache: false,
                    type: "POST",
                    url: "wfTracking.aspx/ValidacionTarjaRe",
                    data: params,
                    contentType: "application/json; charset=utf8",
                    dataType: "json",
                    success: function (response) {

                        var pagos = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;


                        if (pagos.indexOf("Error") != 0) {
                            $("#myTableModal").empty();

                            if (pagos.length > 0) {

                                for (var a = 0; a < pagos.length; a++) {

                                    $("#mensajeCEPA").text(pagos[a].validacion);

                                    var elM = document.getElementById('mensajeCEPA');
                                    elM.style.color = pagos[a].style_va;

                                    break;

                                }

                                $("#myTableModal").append('<thead><th>Estado</th>'
                                    + '<th>Servicio</th>'
                                    + '<th>Detalle</th>'
                                    + '<th>Naviero</th>'
                                    + '<th>Usuario</th>'
                                    + '<th>Pendiente</th>');

                                $("#myTableModal").append('<tbody>');

                                $("#myTableModal").append('<tr><td><img id="muelle" src="' + pagos[0].style_descripcion + '"/></td><td class="descri">' + pagos[0].descripcion + '</td><td style="text-align: left;">' + pagos[0].detalle + '</td><td>' + currencyFormat(pagos[0].style_naviero) + '</td><td>' + currencyFormat(pagos[0].style_cliente) + '</td><td>' + currencyFormat(pagos[0].style_pendiente) + '</td></tr>'
                                    + '<tr><td><img id="muelle" src="' + pagos[1].style_descripcion + '"/></td><td class="descri">' + pagos[1].descripcion + '</td><td style="text-align: left;">' + pagos[1].detalle + '</td><td>' + currencyFormat(pagos[1].style_naviero) + '</td><td>' + currencyFormat(pagos[1].style_cliente) + '</td><td>' + currencyFormat(pagos[1].style_pendiente) + '</td></tr>'
                                    + '<tr><td><img id="muelle" src="' + pagos[2].style_descripcion + '"/></td><td class="descri">' + pagos[2].descripcion + '</td><td style="text-align: left;">' + pagos[2].detalle + '</td><td>' + currencyFormat(pagos[2].style_naviero) + '</td><td>' + currencyFormat(pagos[2].style_cliente) + '</td><td>' + currencyFormat(pagos[2].style_pendiente) + '</td></tr>'
                                    + '<tr><td><img id="muelle" src="' + pagos[3].style_descripcion + '"/></td><td class="descri">' + pagos[3].descripcion + '</td><td style="text-align: left;">' + pagos[3].detalle + '</td><td>' + currencyFormat(pagos[3].style_naviero) + '</td><td>' + currencyFormat(pagos[3].style_cliente) + '</td><td>' + currencyFormat(pagos[3].style_pendiente) + '</td></tr>');


                                $('#to_Tarjas').text(con_tarjas);
                                $('#d_tarjas').text(hTarjas);

                                var suma = 0;
                                var ivaCal = 0;
                                var totalCal = 0;

                                var suma1 = 0;
                                var ivaCal1 = 0;
                                var totalCal1 = 0;

                                var sumaPen = 0;
                                var ivaCalPen = 0;
                                var totalCalPen = 0;



                                for (var i = 0; i < pagos.length; i++) {
                                    suma += parseFloat(pagos[i].style_naviero);
                                    suma1 += parseFloat(pagos[i].style_cliente);
                                    sumaPen += parseFloat(pagos[i].style_pendiente);
                                }

                                ivaCal = (suma * 0.13).toFixed(2);
                                totalCal = (parseFloat(suma) + parseFloat(ivaCal)).toFixed(2);

                                ivaCal1 = (suma1 * 0.13).toFixed(2);
                                totalCal1 = (parseFloat(suma1) + parseFloat(ivaCal1)).toFixed(2);

                                ivaCalPen = (sumaPen * 0.13).toFixed(2);
                                totalCalPen = (parseFloat(sumaPen) + parseFloat(ivaCalPen)).toFixed(2);


                                //<p id="subTotal1"></p>

                                $("#myTableModal").append('</tbody>');
                                $("#myTableModal").append('<tfoot>');
                                $("#myTableModal").append('<tr><th></th><th></th>'
                                    + '<th style="text-align: right;">Subtotal</th>'
                                    + '<th class="descri"><span class="badge"><p id="subTotal"></p></span></th><th class="descri"><span class="badge"><p id="subTotal1"></p></span></th><th class="descri"><span class="badgeR"><p id="subTotalP"></p></span></th></tr>');
                                $("#myTableModal").append('<tr><th></th><th></th><th style="text-align: right;">IVA</th>'
                                    + '<th class="descri"><span class="badge"><p id="iva"></p></span></th><th class="descri"><span class="badge"><p id="iva1"></p></span></th><th class="descri"><span class="badgeR"><p id="ivaP"></p></span></th></tr>');
                                $("#myTableModal").append('<tr><th></th><th></th><th style="text-align: right;">Total</th>'
                                    + '<th class="descri"><span class="badge"><p id="total"></p></span></th><th class="descri"><span class="badge"><p id="total1"></p></span></th><th class="descri"><span class="badgeR"><p id="totalP"></p></span></th></tr>');

                                $("#myTableModal").append('</tfoot>');


                                $('#subTotal').text(suma.toFixed(2));
                                $('#iva').text(ivaCal);
                                $('#total').text(totalCal);

                                $('#subTotal1').text(suma1.toFixed(2));
                                $('#iva1').text(ivaCal1);
                                $('#total1').text(totalCal1);

                                $('#subTotalP').text(sumaPen.toFixed(2));
                                $('#ivaP').text(ivaCalPen);
                                $('#totalP').text(totalCalPen);

                                for (var b = 0; b < pagos.length; b++) {
                                    $("#MensajeModal").text(pagos[b].b_danc);

                                    var el = document.getElementById('MensajeModal');
                                    el.style.color = pagos[b].style_dan;



                                    $("#mensajeADUANA").text(pagos[b].b_aduana);

                                    var elt = document.getElementById('mensajeADUANA');
                                    elt.style.color = pagos[b].style_aduana;


                                    $("#f_tarjaM").text(pagos[b].f_tarja);
                                    $("#f_salidaM").text(pagos[b].f_salida);
                                    var frp = validDate();
                                    $("#f_proxima").text(frp);

                                    var valDate = compaDate();
                                    if (valDate) {
                                        $("#f_leyenda").text("Ultima fecha libre de almacenaje fue:");
                                        if ($('#f_proxima').hasClass('label-warning') || $('#f_proxima').hasClass('label-danger')) {
                                            $('#f_proxima').removeClass('label-warning').removeClass('label-danger').addClass('label-danger');
                                            $('#f_leyenda').removeClass('label-warning').removeClass('label-danger').addClass('label-danger');
                                        }

                                    }
                                    else {
                                        $("#f_leyenda").text("Ultima fecha libre de almacenaje es:");
                                        if ($('#f_proxima').hasClass('label-warning') || $('#f_proxima').hasClass('label-danger')) {
                                            $('#f_proxima').removeClass('label-danger').removeClass('label-warning').addClass('label-warning');
                                            $('#f_leyenda').removeClass('label-danger').removeClass('label-warning').addClass('label-warning');
                                        }

                                    }


                                    break;
                                }



                                $('#dpRetiro').datetimepicker({
                                    locale: 'es',
                                    format: 'DD/MM/YYYY',
                                    minDate: moment()
                                });



                                $("#hTarja").val(tarja);
                                $("#hContenedor").val(contenedor);
                                $("#hLlegada").val(c_llegada);
                                $("#hFtarja").val(f_tarja);
                                $("#hManifiesto").val(n_manifiesto);
                                $("#hvPeso").val(v_peso);
                                $("#hTarjas").val(hTarjas);
                                $("#hNTarjas").val(con_tarjas);

                                if (pagos[0].b_salida == "Y") {
                                    $("#calPa").hide();
                                    $('#lblSalida').html("Fecha Retiro Efectiva");

                                    if ($('#f_salidaM').hasClass('label-danger') || $('#f_salidaM').hasClass('label-success')) {
                                        $('#f_salidaM').removeClass('label-danger').removeClass('label-success').addClass('label-success');
                                    }

                                }
                                else {
                                    $("#calPa").show();
                                    $('#lblSalida').html("Fecha Próxima Programada");
                                    if ($('#f_salidaM').hasClass('label-danger') || $('#f_salidaM').hasClass('label-success')) {
                                        $('#f_salidaM').removeClass('label-danger').removeClass('label-success').addClass('label-danger');
                                    }

                                }

                                if (pagos[0].style_aduana == "#18D318")
                                    $("#btnDetalle").show();
                                else
                                    $("#btnDetalle").hide();



                            }



                            $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                                "backdrop": "static",
                                "keyboard": false,
                                "show": true                     // ensure the modal is shown immediately
                            });

                            $('#myModal').unblock();
                        }
                        else {
                            $('#myModal').unblock();
                            bootbox.alert(pagos);

                        }


                    },
                    failure: function (response) {
                        bootbox.alert(response.d);
                    },
                    error: function (response) {
                        bootbox.alert(response.d);
                    }
                });

                $('#dpRetiro').on('dp.change', function (e) {

                    if (e.Date != "undefined" && e.oldDate != null) {


                        var tr = $("#hTarja").val();
                        var cn = $("#hContenedor").val();
                        var lle = $("#hLlegada").val();
                        var ft = $("#hFtarja").val();
                        var nm = $("#hManifiesto").val();
                        var vPeso = $("#hvPeso").val();
                        var fr = new Date(e.date);
                        var fre = fr.format("dd/MM/yy hh:mm:ss");
                        var hTarj = $("#hTarjas").val();
                        var nTar = $("#hNTarjas").val();
                        validarRetiro(tr, cn, lle, ft, nm, fre, vPeso, hTarj, nTar);

                    }
                });
            }
        }

        function validDate() {
            var tt1 = $("#f_tarjaM").text();

            var tt = tt1.split("/");



            var date = new Date(tt[1] + '/' + tt[0] + '/' + tt[2]);
            var newdate = new Date(date);

            newdate.setDate(newdate.getDate() + 4);

            var dd = newdate.getDate();
            var mm = newdate.getMonth() + 1;
            var y = newdate.getFullYear();

            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }

            var someFormattedDate = dd + '/' + mm + '/' + y;
            return someFormattedDate;
        }


        function validar(tarja, contenedor, c_llegada, f_tarja, n_manifiesto, v_peso, hTarjas, con_tarjas) {
            if (tarja.length > 0 && contenedor.length > 0) {

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

                var params = new Object();
                params.c_tarja = tarja;
                params.n_contenedor = contenedor;
                params.c_llegada = c_llegada;
                params.f_tarja = f_tarja;
                params.n_manifiesto = n_manifiesto;
                params.v_peso = v_peso;
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


                        if (pagos.indexOf("Error") != 0) {
                            $("#myTableModal").empty();

                            if (pagos.length > 0) {

                                for (var a = 0; a < pagos.length; a++) {

                                    $("#mensajeCEPA").text(pagos[a].validacion);

                                    var elM = document.getElementById('mensajeCEPA');
                                    elM.style.color = pagos[a].style_va;

                                    break;

                                }

                                $("#myTableModal").append('<thead><th>Estado</th>'
                                    + '<th>Servicio</th>'
                                    + '<th>Detalle</th>'
                                    + '<th>Naviero</th>'
                                    + '<th>Usuario</th>'
                                    + '<th>Pendiente</th>');

                                $("#myTableModal").append('<tbody>');

                                $("#myTableModal").append('<tr><td><img id="muelle" src="' + pagos[0].style_descripcion + '"/></td><td class="descri">' + pagos[0].descripcion + '</td><td style="text-align: left;">' + pagos[0].detalle + '</td><td>' + currencyFormat(pagos[0].style_naviero) + '</td><td>' + currencyFormat(pagos[0].style_cliente) + '</td><td>' + currencyFormat(pagos[0].style_pendiente) + '</td></tr>'
                                    + '<tr><td><img id="muelle" src="' + pagos[1].style_descripcion + '"/></td><td class="descri">' + pagos[1].descripcion + '</td><td style="text-align: left;">' + pagos[1].detalle + '</td><td>' + currencyFormat(pagos[1].style_naviero) + '</td><td>' + currencyFormat(pagos[1].style_cliente) + '</td><td>' + currencyFormat(pagos[1].style_pendiente) + '</td></tr>'
                                    + '<tr><td><img id="muelle" src="' + pagos[2].style_descripcion + '"/></td><td class="descri">' + pagos[2].descripcion + '</td><td style="text-align: left;">' + pagos[2].detalle + '</td><td>' + currencyFormat(pagos[2].style_naviero) + '</td><td>' + currencyFormat(pagos[2].style_cliente) + '</td><td>' + currencyFormat(pagos[2].style_pendiente) + '</td></tr>'
                                    + '<tr><td><img id="muelle" src="' + pagos[3].style_descripcion + '"/></td><td class="descri">' + pagos[3].descripcion + '</td><td style="text-align: left;">' + pagos[3].detalle + '</td><td>' + currencyFormat(pagos[3].style_naviero) + '</td><td>' + currencyFormat(pagos[3].style_cliente) + '</td><td>' + currencyFormat(pagos[3].style_pendiente) + '</td></tr>');

                                $('#to_Tarjas').text(con_tarjas);
                                $('#d_tarjas').text(hTarjas);

                                var suma = 0;
                                var ivaCal = 0;
                                var totalCal = 0;

                                var suma1 = 0;
                                var ivaCal1 = 0;
                                var totalCal1 = 0;

                                var sumaPen = 0;
                                var ivaCalPen = 0;
                                var totalCalPen = 0;



                                for (var i = 0; i < pagos.length; i++) {
                                    suma += parseFloat(pagos[i].style_naviero);
                                    suma1 += parseFloat(pagos[i].style_cliente);
                                    sumaPen += parseFloat(pagos[i].style_pendiente);
                                }

                                ivaCal = (suma * 0.13).toFixed(2);
                                totalCal = (parseFloat(suma) + parseFloat(ivaCal)).toFixed(2);

                                ivaCal1 = (suma1 * 0.13).toFixed(2);
                                totalCal1 = (parseFloat(suma1) + parseFloat(ivaCal1)).toFixed(2);

                                ivaCalPen = (sumaPen * 0.13).toFixed(2);
                                totalCalPen = (parseFloat(sumaPen) + parseFloat(ivaCalPen)).toFixed(2);


                                //<p id="subTotal1"></p>

                                $("#myTableModal").append('</tbody>');
                                $("#myTableModal").append('<tfoot>');
                                $("#myTableModal").append('<tr><th></th><th></th>'
                                    + '<th style="text-align: right;">Subtotal</th>'
                                    + '<th class="descri"><span class="badge"><p id="subTotal"></p></span></th><th class="descri"><span class="badge"><p id="subTotal1"></p></span></th><th class="descri"><span class="badgeR"><p id="subTotalP"></p></span></th></tr>');
                                $("#myTableModal").append('<tr><th></th><th></th><th style="text-align: right;">IVA</th>'
                                    + '<th class="descri"><span class="badge"><p id="iva"></p></span></th><th class="descri"><span class="badge"><p id="iva1"></p></span></th><th class="descri"><span class="badgeR"><p id="ivaP"></p></span></th></tr>');
                                $("#myTableModal").append('<tr><th></th><th></th><th style="text-align: right;">Total</th>'
                                    + '<th class="descri"><span class="badge"><p id="total"></p></span></th><th class="descri"><span class="badge"><p id="total1"></p></span></th><th class="descri"><span class="badgeR"><p id="totalP"></p></span></th></tr>');

                                $("#myTableModal").append('</tfoot>');


                                $('#subTotal').text(suma.toFixed(2));
                                $('#iva').text(ivaCal);
                                $('#total').text(totalCal);

                                $('#subTotal1').text(suma1.toFixed(2));
                                $('#iva1').text(ivaCal1);
                                $('#total1').text(totalCal1);

                                $('#subTotalP').text(sumaPen.toFixed(2));
                                $('#ivaP').text(ivaCalPen);
                                $('#totalP').text(totalCalPen);

                                for (var b = 0; b < pagos.length; b++) {
                                    $("#MensajeModal").text(pagos[b].b_danc);

                                    var el = document.getElementById('MensajeModal');
                                    el.style.color = pagos[b].style_dan;



                                    $("#mensajeADUANA").text(pagos[b].b_aduana);

                                    var elt = document.getElementById('mensajeADUANA');
                                    elt.style.color = pagos[b].style_aduana;


                                    $("#f_tarjaM").text(pagos[b].f_tarja);
                                    $("#f_salidaM").text(pagos[b].f_salida);
                                    var frp = validDate();
                                    $("#f_proxima").text(frp);

                                    var valDate = compaDate();

                                    if (valDate) {
                                        $("#f_leyenda").text("Ultima fecha libre de almacenaje fue:");
                                        if ($('#f_proxima').hasClass('label-warning') || $('#f_proxima').hasClass('label-danger')) {
                                            $('#f_proxima').removeClass('label-warning').removeClass('label-danger').addClass('label-danger');
                                            $('#f_leyenda').removeClass('label-warning').removeClass('label-danger').addClass('label-danger');
                                        }

                                    }
                                    else {
                                        $("#f_leyenda").text("Ultima fecha libre de almacenaje es:");
                                        if ($('#f_proxima').hasClass('label-warning') || $('#f_proxima').hasClass('label-danger')) {
                                            $('#f_proxima').removeClass('label-danger').removeClass('label-warning').addClass('label-warning');
                                            $('#f_leyenda').removeClass('label-danger').removeClass('label-warning').addClass('label-warning');
                                        }

                                    }


                                    break;
                                }

                                $('#dpRetiro').datetimepicker({
                                    locale: 'es',
                                    format: 'DD/MM/YYYY',
                                    minDate: moment()
                                });



                                $("#hTarja").val(tarja);
                                $("#hContenedor").val(contenedor);
                                $("#hLlegada").val(c_llegada);
                                $("#hFtarja").val(f_tarja);
                                $("#hManifiesto").val(n_manifiesto);
                                $("#hvPeso").val(v_peso);
                                $("#hTarjas").val(hTarjas);
                                $("#hNTarjas").val(con_tarjas);

                                if (pagos[0].b_salida == "Y") {
                                    $("#calPa").hide();
                                    $('#lblSalida').html("Fecha Retiro Efectiva");

                                    if ($('#f_salidaM').hasClass('label-danger') || $('#f_salidaM').hasClass('label-success')) {
                                        $('#f_salidaM').removeClass('label-danger').removeClass('label-success').addClass('label-success');
                                    }

                                }
                                else {
                                    $("#calPa").show();
                                    $('#lblSalida').html("Fecha Próxima Programada");
                                    if ($('#f_salidaM').hasClass('label-danger') || $('#f_salidaM').hasClass('label-success')) {
                                        $('#f_salidaM').removeClass('label-danger').removeClass('label-success').addClass('label-danger');
                                    }

                                }

                                if (pagos[0].style_aduana == "#18D318")
                                    $("#btnDetalle").show();
                                else
                                    $("#btnDetalle").hide();

                                $('#dpRetiro').on('dp.change', function (e) {

                                    if (e.Date != "undefined" && e.oldDate != null) {


                                        var tr = $("#hTarja").val();
                                        var cn = $("#hContenedor").val();
                                        var lle = $("#hLlegada").val();
                                        var ft = $("#hFtarja").val();
                                        var nm = $("#hManifiesto").val();
                                        var vp = $("#hManifiesto").val();
                                        var fr = new Date(e.date);
                                        var fre = fr.format("dd/MM/yy hh:mm:ss");
                                        var vPeso = $("#hvPeso").val();
                                        var hTarj = $("#hTarjas").val();
                                        var nTar = $("#hNTarjas").val();
                                        validarRetiro(tr, cn, lle, ft, nm, fre, vPeso, hTarj, nTar);


                                    }
                                });



                            }

                            $.unblockUI();

                            $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                                "backdrop": "static",
                                "keyboard": false,
                                "show": true                     // ensure the modal is shown immediately
                            });
                        }
                        else {
                            $.unblockUI();
                            bootbox.alert(pagos);
                        }
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





        var isOkay = true;
        function VerDetalle() {
            if (isOkay) {
                var params = new Object();

                params.n_contenedor = $("#hContenedor").val();;
                params.n_mani = $("#hManifiesto").val();
                params = JSON.stringify(params);

                $.ajax({
                    async: true,
                    cache: false,
                    type: "POST",
                    url: "wfTracking.aspx/ObtenerDecla",
                    data: params,
                    contentType: "application/json; charset=utf8",
                    dataType: "json",
                    success: function (response) {
                        var declas = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;


                        if (declas.indexOf("posee") != 0) {

                            $("#myDecla").empty();

                            if (declas.length > 0) {

                                $("#myDecla").append('<thead><th>Estado</th>'
                                    + '<th>Tipo Documento</th>'
                                    + '<th># Documento</th>');

                                $("#myDecla").append('<tbody>');

                                for (var a = 0; a < declas.length; a++) {


                                    $("#myDecla").append('<tr><td>' + declas[a].b_estado + '</td><td>' + declas[a].tipo_doc + '</td><td>' + declas[a].num_doc + '</td><td>'
                                        + '<tr><td>' + declas[a].b_estado + '</td><td class="descri">' + declas[a].tipo_doc + '</td><td>' + declas[a].num_doc + '</td><td>'
                                        + '<tr><td>' + declas[a].b_estado + '</td><td class="descri">' + declas[a].tipo_doc + '</td><td>' + declas[a].num_doc + '</td><td>'
                                        + '<tr><td>' + declas[a].b_estado + '</td><td class="descri">' + declas[a].tipo_doc + '</td><td>' + declas[a].num_doc + '</td><td>');
                                }


                                $("#myDecla").append('</tbody>');
                                $("#myDecla").append('<tfoot>');

                                $("#myDecla").append('</tfoot>');





                                $("#myModalD").modal({                    // wire up the actual modal functionality and show the dialog
                                    "backdrop": "static",
                                    "keyboard": false,
                                    "show": true                     // ensure the modal is shown immediately
                                });

                            }


                        }
                        else {
                            bootbox.alert(declas);
                        }




                    },
                    failure: function (response) {
                        bootbox.alert(response.d);
                    },
                    error: function (response) {
                        bootbox.alert(response.d);
                    }
                });
            }
            isOkay = !isOkay;
        }

        function currencyFormat(num) {
            return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
        }



        function CalculosTabla() {
            var subtotal = 0;
            var ivaCal = 0;
            var totalCal = 0;
            $("#tblProducts tbody tr").each(function () {
                subtotal = parseFloat($('td:eq(4)', $(this)).text());
            });

            ivaCal = subtotal * 0.13;
            totalCal = subtotal + ivaCal;

            $('#subTotal').html(subtotal);
            $('#iva').html(subtotal);
            $('#total').html(subtotal);

        }

        function HoraActual() {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!

            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }

            var hh = today.getHours();
            var mi = today.getMinutes();
            var ss = today.getSeconds();

            if (hh < 10) {
                hh = '0' + hh
            }

            if (mi < 10) {
                mi = '0' + mi
            }

            if (ss < 10) {
                ss = '0' + ss
            }


            var today = dd + '/' + mm + '/' + yyyy + ' ' + hh + ':' + mi + ':' + ss;

            return today;
        }

        function compaDate() {
            var tt1 = $("#f_proxima").text();

            var tt = tt1.split("/");



            var date = new Date(tt[1] + '/' + tt[0] + '/' + tt[2]);
            var newdate = new Date(date);

            var hoy = new Date();

            if (hoy > newdate)
                var so = true
            else
                var so = false


            return so;
        }

        var confirmed = false;
        function confirmaSave(controlID) {
            if (confirmed) { return true; }

            bootbox.confirm("En este momento se cargan las declaraciones o transitos asociados ¿Desea continuar?", function (result) {
                if (result) {
                    if (controlID != null) {
                        var controlToClick = document.getElementById(controlID);
                        if (controlToClick != null) {
                            confirmed = true;
                            //controlToClick.click();
                            var contenedor = document.getElementById('<%= txtBuscar.ClientID %>').value;
                            var url = 'contenedor=' + contenedor; 
                            window.open('wfConsulDecla.aspx?' + url, 'popup');
                            confirmed = false;
                        }
                    }
                } else {
                    bootbox.alert("De no continuar con el cambio puede dar F5 para actualizar")
                }

            });

            return false;
        }

        function pageLoad() {
            $(document).ready(function () {



                $('#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv .imageSecun').click(function () {

                    var img = $(this)
                    var idDeta = $(this).attr('iddetap');

                    var pathname = window.location.pathname; // Returns path only
                    var url = window.location.origin;

                    var tr = $('#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv tr[iddetap =' + idDeta + ']')
                    tr.toggle();

                    var imagen = img[0].src;
                    var foundWord = imagen.indexOf('plus.gif');

                    //if (foundWord != -1) {
                    //    var urlImag = 'CSS/Images/minus.gif';
                    //    $(this).attr('src', "'" + urlImag + "?timestamp=" + "'" + new Date().getTime());
                    //}
                    //else {
                    //    var urlImag = 'CSS/Images/plus.gif';
                    //    $(this).attr('src', "'" + urlImag + "?timestamp=" + "'" + new Date().getTime());
                    //}
                    //var uNew = url + '/CSS/Images/minus.gif';
                    //var nuevaImge = img[0].src;

                    if (tr.is(':visible')) {
                        $(this).removeAttr("src").attr('src', 'CSS/Images/minus.gif');
                    }
                    else {
                        $(this).removeAttr("src").attr('src', 'CSS/Images/minus.gif');
                    }

                });

                //$("#exportpdf").click(function (event) {
                //    event.preventDefault();
                //    Imprimir();
                //});

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

                $("#myClose").click(function () {
                    $("#myModal").modal('hide');
                });

                $("#myOKD").click(function () {
                    $("#myModalD").modal('hide');
                });


                iniciaVariable();

                CalculosTabla();

                $('#<%=grvTracking.ClientID %> button').tooltip();

                $('#dpRetiro').tooltip();

            });
        }
    </script>
</asp:Content>

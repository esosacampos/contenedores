<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsultaDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfConsultaDAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 10px;
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

        @media only screen and (min-device-width: 960px) {
           
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <img src="CSS/Images/CEPA_LOGO.gif" alt="" style="margin-top: 35px; width: 100px; height: 60px;" />
    <br />
    <br />
    <img src="CSS/Images/dan_logo.png" alt="" style="margin-top: 10px; width: 100px; height: 100px;" />
    <br />
    <br />
    <img src="CSS/Images/pncsv.jpg" alt="" style="margin-top: 10px; width: 100px; height: 100px;" />
    <br />
    <br />
    <%--<a href="wfAyuda.aspx" alt="Manual de Usuario">
        <img src="CSS/Images/manual_icon.png" alt="" style="margin-top: 10px; width: 80px; height: 100px; margin-left: 15px;" /></a>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row myform">
        <div class="col-lg-12 col-xs-12">
            <div class="col-lg-12 col-xs-12">
                <div class="form-inline">
                    <div class="input-group col-lg-5 col-xs-5" style="width: 44%;">
                        <asp:TextBox ID="Datepicker" runat="server" class="form-control" autocomplete="off"
                            placeholder="Ingrese año del manifiesto" Text=""></asp:TextBox>
                    </div>
                    <div class="input-group col-lg-6 col-xs-6" style="width: 51%;">
                        <asp:TextBox ID="txtMani" runat="server" class="form-control" autocomplete="off"
                            placeholder="Ingrese # Manifiesto"></asp:TextBox>
                    </div>

                </div>
            </div>
            <div class="col-lg-12 col-xs-12">
                <div class="form-inline" style="margin-top: 10%">
                    <div class="input-group col-lg-7 col-xs-7" style="width: 2;">
                        <asp:TextBox ID="txtContenedor" runat="server" CssClass="form-control" placeholder="Ingrese el # Contenedor" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="input-group col-lg-2 col-xs-2" style="width: 0%;">
                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary btn-lg" Text="Consultar" OnClick="btnBuscar_Click" />
                    </div>
                    <div class="input-group col-lg-3 col-xs-3" style="width: 0%;">
                        <asp:Button ID="btnClear" runat="server" class="btn btn-primary btn-lg" Text="Nueva Consulta" OnClick="btnClear_Click" />
                    </div>
                </div>
            </div>
            <br />
            <br />
        </div>
    </div>
    <div id="printArea">
        <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="col-lg-10 alert alert-info" style="margin-left: 15px; margin-top: 10px; width: 92.7%; margin-bottom: 15px;">
                    <strong>ESTADO DEL CONTENEDOR:</strong><asp:Label ID="b_retenido" runat="server" Text="" Style="margin-left: 15px;"></asp:Label>
                </div>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="n_contenedor"
                    CssClass="footable" Style="margin-top: 5%; max-width: 98%; margin-left: 15px;" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="n_folio" HeaderText="# OFICIO"></asp:BoundField>
                        <asp:BoundField DataField="TipoRe" HeaderText="TIPO"></asp:BoundField>
                        <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR"></asp:BoundField>
                        <asp:TemplateField HeaderText="TAMAÑO">
                            <ItemTemplate>
                                <asp:HiddenField ID="hEstado" runat="server" Value='<%#Eval("b_estado")%>' />
                                <asp:Label ID="lblTamaño" runat="server" Text='<%#Eval("c_tamaño")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="f_recepcion" HeaderText="F. RECEP. ARCO" HtmlEncode="false"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                        <asp:BoundField DataField="f_recep_patio" HeaderText="F. RECEP. PATIO" HtmlEncode="false"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                        <asp:BoundField DataField="f_retenido" HeaderText="F. RETENIDO" HtmlEncode="false"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                        <asp:BoundField DataField="f_cancelado" HeaderText="F. CANCELADO" HtmlEncode="false"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                        <asp:BoundField DataField="f_tramite" HeaderText="F. TRAMITE" HtmlEncode="false"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                        <asp:BoundField DataField="f_ini_dan" HeaderText="F. REVISION" HtmlEncode="false"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                        <asp:BoundField DataField="TipoRevision" HeaderText="T. REVISION"></asp:BoundField>
                        <asp:BoundField DataField="f_liberado" HeaderText="F. LIBERADO" HtmlEncode="false"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                        <asp:BoundField DataField="c_correlativo" HeaderText="TOTAL HORAS"></asp:BoundField>
                        <asp:BoundField DataField="CalcDiasD" HeaderText="TIEMPO (Días)" DataFormatString="{0:F2}"
                            HtmlEncode="false"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />--%>
                <%--<asp:PostBackTrigger ControlID="btnClrear" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="lblMensaje" runat="server" Text="* Los contenedores marcados en ROJO son CANCELADOS" CssClass="alert-danger lead" Style="margin-left: 15px;"></asp:Label>
    <hr />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

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

            $('#ContentPlaceHolder1_GridView1').footable();
        }
        function endRequest(sender, args) {

            $.unblockUI();

            $('#ContentPlaceHolder1_GridView1').footable();

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

        }

        function pageLoad() {
            $(document).ready(function () {

                $('#ContentPlaceHolder1_GridView1').footable();

            });
        }
    </script>
</asp:Content>

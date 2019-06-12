<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfConsultaDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsultaDAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Contenedores Retenidos DAN</h2>
    <hr />
    <div class="col-lg-9">
        <div class="input-group">
            <asp:TextBox ID="txtBuscar" runat="server" class="form-control" placeholder="# de contenedor sin guiones"></asp:TextBox>
            <span class="input-group-btn">
                <asp:Button ID="btnBuscar" runat="server" class="btn btn-default" Text="Consultar"
                    OnClick="btnBuscar_Click" />
            </span>
        </div>
        <!-- /input-group -->
    </div>
    <!-- /.col-lg-6 -->
    <br />
    <br />
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="n_contenedor"
                CssClass="footable" Style="margin-top:5%;max-width: 98%; margin-left: 15px;" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="n_folio" HeaderText="# OFICIO"></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="f_recep_patio" HeaderText="F. RECEP. PATIO" HtmlEncode="false"
                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                    <asp:BoundField DataField="f_recepcion" HeaderText="F. RETENIDO" HtmlEncode="false"
                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                    <asp:BoundField DataField="f_tramite" HeaderText="F. TRAMITE" HtmlEncode="false"
                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                    <asp:BoundField DataField="f_liberado" HeaderText="F. LIBERADO" HtmlEncode="false"
                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                    <asp:BoundField DataField="c_correlativo" HeaderText="TOTAL HORAS"></asp:BoundField>
                    <asp:BoundField DataField="CalcDiasD" HeaderText="TIEMPO (Días)" DataFormatString="{0:F2}"
                        HtmlEncode="false"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
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

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfConsultaDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsultaDAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Contenedores Retenidos DAN / UCC</h2>
    <hr />
    <div class="col-lg-9">
        <div class="input-group">
            <asp:TextBox ID="txtBuscar" runat="server" class="form-control" placeholder="# de contenedor sin guiones" autocomplete="off"></asp:TextBox>
            <span class="input-group-btn">
                <asp:Button ID="btnBuscar" runat="server" class="btn btn-default" Text="Consultar"
                    OnClick="btnBuscar_Click" />
            </span>
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional" style="margin-top: 30px; margin-bottom: 60px;">
        <ContentTemplate>
            <div class="col-lg-9 alert alert-info" style="margin-left: 15px;">
                <strong>ESTADO DEL CONTENEDOR:</strong><asp:Label ID="b_retenido" runat="server" Text="" Style="margin-left: 15px;"></asp:Label>
            </div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="n_contenedor"
                CssClass="footable" Style="margin-top: 5%; max-width: 98%; margin-left: 15px;" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="TipoRe" HeaderText="RETENCION"></asp:BoundField>
                    <asp:BoundField DataField="n_folio" HeaderText="# OFICIO"></asp:BoundField>
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
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:Label ID="lblMensaje" runat="server" Text="* Los contenedores marcados en ROJO son CANCELADOS" CssClass="alert-danger lead" Style="margin-left: 15px;"></asp:Label>
    <br />
    <asp:Label ID="Label1" runat="server" Text="* Ordenados desde la última retención a la primera" CssClass="alert-danger lead" Style="margin-left: 15px;"></asp:Label>
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
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

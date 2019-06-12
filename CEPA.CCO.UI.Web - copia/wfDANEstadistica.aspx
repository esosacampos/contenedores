<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfDANEstadistica.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfDANEstadistica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">       
        tr
        {
            display: table-row !important;
        }        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <h2>
        Reporte Consolidado Contenedores Retenidos DAN</h2>
    <hr />
    <div class="col-lg-9">
        <div class="input-group">
            <asp:TextBox ID="txtBuscar" runat="server" MaxLength="4" class="form-control" placeholder="introducir año (mínimo permitido 2014)"></asp:TextBox>
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
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="mes"
                CssClass="footable" Style="margin-top:5%;max-width: 98%; margin-left: 15px;" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="mes" HeaderText="MES"></asp:BoundField>
                    <asp:BoundField DataField="retenidos" HeaderText="CONT. RETENIDOS"></asp:BoundField>
                    <asp:BoundField DataField="liberados" HeaderText="CONT. LIBERADOS"></asp:BoundField>
                    <asp:BoundField DataField="pendientes" HeaderText="CONT. PENDIENTES *"></asp:BoundField>                   
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br /><br />
    <asp:Label ID="lblMensaje" runat="server" Text="* Contenedores pendientes de liberar con base al mes de su retención" CssClass="alert-danger lead" style="margin-left: 15px;" ></asp:Label>
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

            $('#ContentPlaceHolder1_GridView1 tr').addClass('footable-detail-show');
            //$('.footable-row-detail').css("display", "table-row");
        }
        function endRequest(sender, args) {

            $.unblockUI();

            $('#ContentPlaceHolder1_GridView1').footable();
            $('#ContentPlaceHolder1_GridView1 tr').addClass('footable-detail-show');
           // $('.footable-row-detail').css("display", "table-row");

           // $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

        }

        function pageLoad() {
            $(document).ready(function () {

                $('#ContentPlaceHolder1_GridView1').footable();

                $('#ContentPlaceHolder1_GridView1 tr').addClass('footable-detail-show');
                //$('.footable-detail-show').css("display", "table-row");

            });
        }
    </script>
</asp:Content>

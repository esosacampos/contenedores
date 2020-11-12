<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfTipoCliente.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfTipoCliente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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

        #ContentPlaceHolder1_txtCliente {
            text-transform:uppercase;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-row">
        <div class="col-md-8">      
            <%--<input type="text" runat="server" name="txtCliente" id="txtCliente2" value="" class="form-control" placeholder="Nombre del cliente">--%>
            <asp:TextBox runat="server" ID="txtCliente" CssClass="form-control" placeholder="Nombre del Cliente" autocomplete="off"></asp:TextBox>
        </div>      
        <div class="col-md-2">
            <asp:Button ID="btnBuscar" runat="server" class="form-control btn btn-success btn-md" Text="Buscar" OnClick="btnBuscar_Click" />
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional" style="margin-top: 15px;">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                DataKeyNames="c_cliente" CssClass="footable" Style="margin-left: 15px; margin-bottom: 5%;"
                data-filter="#filter" data-page-size="10" ShowFooter="true" data-paging-count-format="{CP} of {TP}"
                OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging">
                <Columns>
                    <asp:BoundField DataField="c_cliente" HeaderText="COD. CLIENTE"></asp:BoundField>
                    <asp:BoundField DataField="c_cliente_light" HeaderText="COD. C. LIGHT"></asp:BoundField>
                    <asp:BoundField DataField="s_nombre_comercial" HeaderText="NOMBRE COMERCIAL"></asp:BoundField>
                    <asp:BoundField DataField="s_razon_social" HeaderText="RAZON SOCIAL"></asp:BoundField>
                    <asp:BoundField DataField="s_numero_registro" HeaderText="# REGISTRO"></asp:BoundField>
                    <asp:BoundField DataField="s_dui" HeaderText="DUI"></asp:BoundField>
                    <asp:BoundField DataField="s_nit" HeaderText="NIT"></asp:BoundField>
                    <asp:BoundField DataField="tipoCliente" HeaderText="TIPO CLIENTE"></asp:BoundField>
                    <asp:BoundField DataField="facilidadPago" HeaderText="FACILIDAD DE PAGO"></asp:BoundField>
                    <asp:BoundField DataField="s_direccion" HeaderText="DIRECCION"></asp:BoundField>          
                </Columns>
                <EmptyDataRowStyle CssClass="alert alert-info" />
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptyMessage" Text="Búsqueda no produjó resultados intentarlo de nuevo" runat="server" />
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <nav>
        <ul class="pager">
            <li class="previous"></li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar"
                    OnClick="btnRegresar_Click" />
            </li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
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
        }
        function endRequest(sender, args) {

            $.unblockUI();

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


        $(document).ready(function () {
            $('#ContentPlaceHolder1_GridView1').footable();

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

            $('.bs-pagination td table').each(function (index, obj) {
                convertToPagination(obj);
            });   
        });
     </script>
</asp:Content>

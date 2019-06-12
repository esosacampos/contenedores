<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfConsultaTrans.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsultaTrans" %>

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

        .badge {
            font-size: 1.25em;
        }
        .glyphicon-refresh-animate {
            -animation: spin .7s infinite linear;
            -webkit-animation: spin2 .7s infinite linear;
        }

        @-webkit-keyframes spin2 {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            from {
                transform: scale(1) rotate(0deg);
            }

            to {
                transform: scale(1) rotate(360deg);
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Consulta Contenedores Transmitidos</h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
            <tr>
                <td colspan="2">IMO
                </td>
                <td colspan="2">
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Nombre del Buque
                </td>
                <td colspan="2">
                    <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Código de Llegada
                </td>
                <td colspan="2">
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Fecha de llegada
                </td>
                <td colspan="2">
                    <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Total C. Importados
                </td>
                <td colspan="2">
                    <span class="alert alert-warning">
                        <asp:Label ID="tot_lineasa" runat="server" Text="" class="label-warning badge"></asp:Label>
                        <strong>C. Anunciados </strong></span>
                    <span class="glyphicon glyphicon-minus"></span>
                    <span class="alert alert-danger">
                        <asp:Label ID="tot_cancel" runat="server" Text="" class="label-danger badge"></asp:Label>
                        <strong>C. Cancelados</strong></span>
                    <span class="glyphicon glyphicon-equal"></span>
                    <span class="alert alert-success">
                        <asp:Label ID="tot_imp" runat="server" Text="" class="label-success badge"></asp:Label>
                        <strong>C. Importados</strong></span>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Total C. Recibidos En Patio
                </td>
                <td>
                    <asp:Label ID="tot_trans" runat="server" Text="" class="label-primary badge"></asp:Label>
                </td>
                <td>Total C. Pendientes En Patio
                </td>
                <td>
                    <asp:Label ID="lblPP" runat="server" Text="" class="label-info badge"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Total C. Recibidos En Arco OIRSA
                </td>
                <td>
                    <asp:Label ID="total_arco" runat="server" Text="" class="label-primary badge"></asp:Label>
                </td>
                <td>Total C. Pendientes En Arco OIRSA
                </td>
                <td>
                    <asp:Label ID="lblPO" runat="server" Text="" class="label-info badge"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="filtrar"
                DataKeyNames="c_correlativo" CssClass="footable" Style="margin-left: 15px; margin-bottom: 5%;"
                data-filter="#filter" data-page-size="10" ShowFooter="true" data-paging-count-format="{CP} of {TP}"
                OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging"
                OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="c_cliente" HeaderText="AGENCIA"></asp:BoundField>
                    <asp:BoundField DataField="c_manifiesto" HeaderText="# MANIFESTO"></asp:BoundField>
                    <asp:BoundField DataField="c_correlativo" HeaderText="CORR."></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="b_estado" HeaderText="ESTADO"></asp:BoundField>
                    <asp:BoundField DataField="c_tarja" HeaderText="# TARJA"></asp:BoundField>
                    <asp:BoundField DataField="b_requiere" HeaderText="ENTREGA"></asp:BoundField>
                    <asp:BoundField DataField="b_recepcion_c" HeaderText="TRANS. ARCO"></asp:BoundField>
                    <asp:BoundField DataField="f_dan" HeaderText="FECHA DE TRANS. ARCO"></asp:BoundField>
                    <asp:BoundField DataField="f_recep" HeaderText="FECHA DE RECEP. PATIO"></asp:BoundField>
                    <asp:BoundField DataField="b_trans" HeaderText="TRANS. ADUANA"></asp:BoundField>
                    <asp:BoundField DataField="f_trans" HeaderText="FECHA DE TRANS ADUANA"></asp:BoundField>

                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
                        
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
            var s = $(obj).find("tbody tr").length;
            var a = "prueba";
        }


        function myFunction() {
            setTimeout(function () { location.reload(); }, 900000);
        }

        $(document).ready(function () {
            $('#ContentPlaceHolder1_GridView1').footable({
            });

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

            $('.bs-pagination td table').each(function (index, obj) {
                convertToPagination(obj);
            });

            //$('.label-default').html(x + " " + y);

            myFunction();
        });
    </script>
</asp:Content>

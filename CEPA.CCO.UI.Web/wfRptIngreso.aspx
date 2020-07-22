<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfRptIngreso.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfRptIngreso" %>

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
    <h2>Generar Reporte De Ingreso De Contenedores De Importación</h2>
    <br />
    <asp:HiddenField ID="h_manifiesto" runat="server" />
    <div class="col-lg-12">
        <div class="form-inline">
            <div class="input-group" style="width: 15%;">
                <label for="texto" style="margin-top: 5px; padding-left: 5px;">
                    Año</label>
                <asp:DropDownList ID="ddlYear" runat="server" class="selectpicker show-tick seleccion" data-style="btn-default">
                </asp:DropDownList>
            </div>
            <div class="input-group" style="width: 15%;">
                <label for="texto" style="margin-top: 5px; padding-left: 10px;">
                    Mes</label>
                <asp:DropDownList ID="ddlMeses" runat="server" class="selectpicker show-tick seleccion" data-style="btn-default">
                </asp:DropDownList>
            </div>
            <div class="input-group" style="width: 5%;display:none;">
                        <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-success" OnClick="btnBuscar_Click" OnClientClick="return confirmaSave(this.id);" />
                    </div>
            <div class="input-group" style="width: 5%;">
                        <asp:Button ID="btnGenerar" runat="server" Text="Generar Excel" CssClass="btn btn-success" OnClick="btnGenerar_Click" OnClientClick="return confirmaSave(this.id);" />
                    </div>
        </div>

        <div class="form-group" style="margin-left: 15px;display:none;" >
            <label for="texto">
                Buscar</label>
            <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
        </div>
        <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="filtrar"
                    DataKeyNames="IdDeta" CssClass="footable" Style="margin-left: 15px; margin-bottom: 5%;"
                    data-filter="#filter" data-page-size="10" ShowFooter="true" data-paging-count-format="{CP} of {TP}"
                    OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging">                <Columns>                                                
                        <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                        <asp:BoundField DataField="p_procedencia" HeaderText="P. PROCEDENCIA"></asp:BoundField>
                        <asp:BoundField DataField="d_buque" HeaderText="BUQUE"></asp:BoundField>
                        <asp:BoundField DataField="navicorto" HeaderText="NAVIERA"></asp:BoundField>
                        <asp:BoundField DataField="s_consignatario" HeaderText="CONSIGNATARIO"></asp:BoundField>
                        <asp:BoundField DataField="s_mercaderia" HeaderText="MERCADERIA"></asp:BoundField>
                        <asp:BoundField DataField="f_retencion" HeaderText="F/H RETENCION"></asp:BoundField>
                        <asp:BoundField DataField="f_liberacion" HeaderText="F/H LIBERACION"></asp:BoundField>
                        <asp:BoundField DataField="b_cancelado" HeaderText="CONT. CANCELADOS"></asp:BoundField>                       
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
    </div>
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
                }, 6000);
            });
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

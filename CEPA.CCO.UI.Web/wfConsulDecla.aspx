<%@ Page Title="Consulta Declaraciones" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsulDecla.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsulDecla" %>

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

        .btn-group-lg > .btn, .btn-lg {
            padding: 5px 14px;
            font-size: 14px;
            line-height: 1.3;
            border-radius: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Consulta Contenedores / Declaraciones </h2>
    <hr />
    <br />
    <%--<div class="col-lg-10">--%>
    <%--<div class="form-inline">
            <div class="input-group">
                
            </div>
            <div class="input-group">
                
            </div>
            <div class="input-group">
                
            </div>
            <!-- /input-group -->
        </div>--%>
    <%-- <div class="row">
            <div class="col">
               
            </div>
            <div class="col">
               
            </div>
            <div class="col">
               <asp:Button ID="btnFiltrar" runat="server" class="btn btn-primary btn-lg" Text="Filtrar" />
            </div>
        </div>--%>
    <%--</div>--%>
    <div class="form-group" style="margin-left: 15px;">
        <label for="inputAddress">Año</label>
           <asp:HiddenField ID="hYear" runat="server" />
        <asp:TextBox ID="txtYear" runat="server" class="form-control" autocomplete="off"
            placeholder="Año de búsqueda" Text=""></asp:TextBox>
    </div>
    <div class="form-row">
        <div class="form-group col-md-6">
            <%--<asp:TextBox ID="txtContenedor" runat="server" class="form-control" autocomplete="off"
                placeholder="Ingrese Últimos Dígitos del Contenedor"></asp:TextBox>--%>
            <input type="search" name="txtConte" id="search" value="" class="form-control" placeholder="Ultimos digitos contenedor">
        </div>
        <div class="form-group col-md-4">
            <input type="search" name="txtDecla" id="decla" value="" class="form-control" placeholder="Ingrese Últimos #'s Declaracion (Opcional)">
        </div>
        <div class="form-group col-md-2">
            <asp:Button ID="btnFiltrar" runat="server" class="form-control btn btn-primary btn-lg" Text="Filtrar" OnClick="btnFiltrar_Click" />
        </div>
    </div>
    <br />
    <br />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida luego de filtrar">
    </div>
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional" style="margin-top: 15px;">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                DataKeyNames="IdRegAduana" CssClass="footable" Style="margin-left: 15px; margin-bottom: 5%;"
                data-filter="#filter" data-page-size="10" ShowFooter="true" data-paging-count-format="{CP} of {TP}"
                OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="n_manifiesto" HeaderText="# MANIFESTO"></asp:BoundField>
                    <asp:BoundField DataField="n_declaracion" HeaderText="# DECLARACION"></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="TipoEstado" HeaderText="ESTADO"></asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="SELECTIVIDAD"></asp:BoundField>
                    <asp:BoundField DataField="f_aduana" HeaderText="FECHA DE TRANS. ADUANA"></asp:BoundField>
                </Columns>
                <EmptyDataRowStyle CssClass="alert alert-info" />
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptyMessage" Text="Búsqueda no produjó resultados intentarlo de nuevo" runat="server"/>
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:Label ID="lblMensaje" runat="server" Text="* Debe indicar # contenedor o # de declración" CssClass="alert-danger lead" Style="margin-left: 15px;"></asp:Label>
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

        function SetAutoComplete() {
            $("#search").autocomplete({
                minLength: 3,
                source: function (request, response) {
                    var params = new Object();
                    params.prefix = request.term;
                    params.year = $("#<%= txtYear.ClientID %>").val();
                    params = JSON.stringify(params);
                    $.ajax({
                        url: '<%=ResolveUrl("~/wfConsulDecla.aspx/GetConte") %>',
                        data: params,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var lista = (typeof data.d) == "string" ? eval('(' + data.d + ')') : data.d;
                            if (lista.length > 0) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split('-')[0],
                                        val: item.split('-')[1]
                                    }
                                }))
                            }
                            else {
                                bootbox.alert("Busqueda no produce resultados");
                            }
                        },
                        error: function (response) {
                            bootbox.alert("CEPA - Contenedores: Alerta!! Se ha producido un error vuelva a intertarlo o reporte a Informática");
                        },
                        failure: function (response) {
                            bootbox.alert(response.responseText);
                        }
                    });
                }
            });
        }

        function SetDeclaracion() {
            $("#decla").autocomplete({
                minLength: 3,
                source: function (request, response) {
                    var params = new Object();
                    params.prefix = request.term;
                    params.year = $("#<%= txtYear.ClientID %>").val();
                    params = JSON.stringify(params);
                    $.ajax({
                        url: '<%=ResolveUrl("~/wfConsulDecla.aspx/GetDecla") %>',
                        data: params,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var lista = (typeof data.d) == "string" ? eval('(' + data.d + ')') : data.d;
                            if (lista.length > 0) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split('/')[0],
                                        val: item.split('/')[1]
                                    }
                                }))
                            }
                            else {
                                bootbox.alert("Busqueda no produce resultados");
                            }
                        },
                        error: function (response) {
                            bootbox.alert("CEPA - Contenedores: Alerta!! Se ha producido un error vuelva a intertarlo o reporte a Informática");
                        },
                        failure: function (response) {
                            bootbox.alert(response.responseText);
                        }
                    });
                }
            });
        }

        function getYear() {
            var yearD = (new Date).getFullYear();
            $("#<%=hYear.ClientID %>").val(yearD);
            bootbox.alert($("#<%=hYear.ClientID %>").val());
        }

        $(document).ready(function () {
            $('#ContentPlaceHolder1_GridView1').footable();

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

            $('.bs-pagination td table').each(function (index, obj) {
                convertToPagination(obj);
            });

            SetAutoComplete();

            SetDeclaracion();

            



        });
    </script>
</asp:Content>

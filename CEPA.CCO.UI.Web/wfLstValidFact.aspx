<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfLstValidFact.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfLstValidFact" %>

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

        .footable.breakpoint > tbody > tr > td.footable-row-detail-cell {
            background: #fff;
            padding-left: 280px;
        }

        .footable > tbody > tr:hover:not(.footable-row-detail-cell) > td {
            color: #31708f;
            font-weight: bold;
            background-color: #bce8f1;
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

        .footable > tbody > tr > td:last-child {
            border-right: 3px solid #1771F8;
        }

        .footable > tbody > tr > td {
            font-size: 12px;
            line-height: 20px;
        }

        th.footable-last-column {
            border-right: 3px solid #1771F8;
        }

        .form-group {
            margin-bottom: 10px;
            line-height: 10px;
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
    <h2>Consulta De Validaciones Manuales
    </h2>
    <br />
  
            <div class="row">
                <div class="col-lg-10" style="padding-right: 1px;">
                    <div role="form">
                        <div class="form-inline">
                            <div class="form-group">
                                <span>Fecha: </span>
                            </div>
                            <div class="form-group">
                                <div class="input-group date" id="datetimepicker2">
                                    <%--<input type="text" class="form-control">--%>
                                    <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-success" OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    
    <div class="form-group" style="margin-left: 15px; margin-top: 25px; width: 98%;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- <asp:Timer ID="Timer1" runat="server" Interval="500000" OnTick="Timer1_Tick">
            </asp:Timer>--%>
            <asp:GridView ID="grvValidaciones" CssClass="footable" runat="server" AutoGenerateColumns="False" DataKeyNames=""
                data-filter="#filter"
                ShowFooter="true" OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging"
                data-page-size="10" OnRowDataBound="grvValidaciones_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="# MANIFIESTO">
                        <ItemTemplate>
                            <asp:Label ID="lblMani" runat="server" Text='<%# Eval("Amanifiesto").ToString() +"-"+Eval("Nmanifiesto").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ncontenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="Observa" HeaderText="JUSTIFICACIÓN"></asp:BoundField>
                    <asp:BoundField DataField="Registra" HeaderText="FECHA DE VALIDACIÓN"></asp:BoundField>
                    <asp:BoundField DataField="Usuario" HeaderText="USUARIO"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnBuscar"/>
        </Triggers>
    </asp:UpdatePanel>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
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
        }

        var d = new Date();
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
        $(document).ready(function () {

            $('#<%=grvValidaciones.ClientID %>').footable({
                breakpoints: {
                    phone: 480,
                    tablet: 1024
                }
            });

            //$('ul.pagination.pagination-centered.hide-if-no-paging').each(function (index, obj) {
            //    convertToPagination(obj);
            //});

            $('#datetimepicker2').datetimepicker({
                defaultDate: new Date(),
                locale: 'es',
                format: "L",
                maxDate: new Date()
            });

        });
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfAlertaDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfAlertaDAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .footable-visible.footable-last-column {
            border-right: 3px solid #1771F8;
        }

        th.footable-visible.footable-last-column {
            border-right: 3px solid #1771F8;
        }

        div.col-sm-10.text-left.principal {
            top: 12px;
            width: 83.3%;
            max-width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div style="margin: 20px;">
        <a class="btn btn-primary btn-xs" href="wfAlertaDAN.aspx?importType=PDF">Importar</a>
    </div> --%>
    <div class="input-group date" id="datetimepicker2" style="margin-right: 34%; margin-left: 5%;">
        <%--<input type="text" class="form-control">--%>

        <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
        </span>
    </div>
    <div style="/*margin-right: 5%; margin-top: 5%;*/" id="showHtml">
        <%= ShowHtml() %>
    </div>
    <br />
    <asp:Button ID="btnExport" runat="server" Style="margin-bottom: 5%; margin-left: 10%; width: 88%" Text="Exportar" OnClick="btnExport_Click" class="btn btn-primary btn-lg btn-block"></asp:Button>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>

    <script type="text/javascript">


        var d = new Date();


        $(document).ready(function () {
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
            setTimeout($.unblockUI, 4000);

            $('table').footable();

            $('#datetimepicker2').datetimepicker({
                locale: 'es',
                format: "L",
                maxDate: new Date()
            });

            $("#datetimepicker2").on("dp.change", function (e) {
                var params = new Object();
                params.c_periodo = $("#ContentPlaceHolder1_txtDOB").val();
                params = JSON.stringify(params);

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

                $.ajax({
                    async: true,
                    type: "POST",
                    url: "wfAlertaDAN.aspx/Llenar",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //var datos = $.parseJSON(msg.d);

                        //$(datos).each(function () {
                        //    var option = $(document.createElement('option'));

                        //    option.text(this.nombre);
                        //    option.val(this.expediente);


                        //    $("#combobox").append(option);
                        //});
                        //bootbox.alert(msg.d);
                        $.unblockUI();
                        $("#showHtml").html(msg.d);
                    },
                    error: function (msg) {
                        $.unblockUI();
                        bootbox.alert(msg.d);
                    }
                });
            });
        });

    </script>
</asp:Content>

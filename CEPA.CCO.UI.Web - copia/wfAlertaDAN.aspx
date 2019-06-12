<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfAlertaDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfAlertaDAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div style="margin: 20px;">
        <a class="btn btn-primary btn-xs" href="wfAlertaDAN.aspx?importType=PDF">Importar</a>
    </div> --%>
    <div class="input-group date" id="datetimepicker2" style="margin-right: 25%; margin-left:3%;">
        <%--<input type="text" class="form-control">--%>
        
        <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
        <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
        </span>
    </div>
    <div style="margin-right: 10%; margin-top: 5%;" id="showHtml">
        <%= ShowHtml() %>
    </div>
    <br />
        <asp:Button ID="btnExport" runat="server" style="margin-bottom: 5%; margin-left:4%;width:88%" Text="Exportar" OnClick="btnExport_Click" class="btn btn-primary btn-lg btn-block"></asp:Button>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>

    <script type="text/javascript">

        
        var d = new Date();
       

        $(document).ready(function () {
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
            setTimeout($.unblockUI, 4000);

       
            $('#datetimepicker2').datetimepicker({
                locale: 'es',
                format: "L",
                maxDate : new Date()              
            });

            $("#datetimepicker2").on("dp.change", function (e) {
                var params = new Object();                
                params.c_periodo = $("#ContentPlaceHolder1_txtDOB").val();
                params = JSON.stringify(params);

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

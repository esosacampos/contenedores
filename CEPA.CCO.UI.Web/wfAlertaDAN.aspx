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
    <div class="row" style="margin-left: 30px;">
        <div class="col-lg-10" style="padding-right: 1px;">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <span>Fecha: </span>
                    </div>
                    <div class="form-group">

                        <div class="input-group date" id="datetimepicker2" style="margin-left: 5%;">

                            <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
        </div>
    </div>
    </div>
    <div style="/*margin-right: 5%; margin-top: 5%; */" id="showHtml">
        <%= ShowHtml() %>
    </div>
    <br />
    <asp:Button ID="btnExport" runat="server" Style="margin-bottom: 5%; margin-left: 10%; width: 88%" Text="Exportar" OnClick="btnExport_Click" class="btn btn-primary btn-lg btn-block" OnClientClick="return confirmaSave(this.id);"></asp:Button>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>

    <script type="text/javascript">


        var d = new Date();

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
                }, 7500);
            });

            // do something in the background
            //dialog.modal('hide');             
            /*bootbox.dialog({
                message  : "Por favor esperar mientras su solicitud se procesa",
                timeOut : 2000
            });*/
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

            $('.table').footable();

            $('#datetimepicker2').datetimepicker({
                defaultDate: new Date(),
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
                        $('.table').footable();
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

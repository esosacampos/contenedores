<%@ Page Title="Cargar Archivos" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="wfCargarBookings.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfCargarBookings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/bootstrap/csss/bootstrap.fd.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Cargar Listado Bookings</h2>
    <hr />
    <div class="table-responsive" style="padding-left:125px;width:80%;">
        <table class="table">
            <tr>
                <td>NAVIERA
                </td>
                <td>
                    <asp:Label ID="c_naviera" runat="server" Text=""></asp:Label>
                </td>
            </tr>          
        </table>
    </div>
    <nav>
        <ul class="pager">
            <li class="previous">
                <input type="button" id="open_btn" class="btn btn-primary btn-lg" value="Cargar Listado">
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar"
                    OnClick="btnRegresar_Click" /></li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="<%= ResolveUrl("~/bootstrap/js/bootstrap.fd.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        function createRequestObject() {
            var http;
            if (window.XMLHttpRequest) { // Mozilla, Safari, IE7 ...
                http = new XMLHttpRequest();
            }
            else if (window.ActiveXObject) { // Internet Explorer 6
                http = new ActiveXObject("Microsoft.XMLHTTP");
            }
            return http;
        }

        // Funcion Principal
        $(document).ready(function () {
            $("#open_btn").click(function () {
                $.FileDialog({ multiple: true }).on('files.bs.filedialog', function (ev) {
                    var files = ev.files;
                    var data = new FormData();
                    for (var i = 0; i < files.length; i++) {
                        data.append(files[i].name, files[i]);
                    }

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
                        url: '<%= ResolveClientUrl("~/FileUploadHandlerBook.ashx") %>',
                        type: "POST",
                        data: data,
                        contentType: false,
                        processData: false,
                        success: function (result) {                            
                            bootbox.alert(result);
                            $.unblockUI();
                        }
                    });
                }).on('cancel.bs.filedialog', function (ev) {
                    bootbox.alert("Cancelado!");
                    $.unblockUI();
                });
            });
        });


        function almacenando() {
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
            setTimeout(function () {
                $.unblockUI({
                    onUnblock: function () { alert('Registrado Correctamente'); }
                });
            }, 2000);
        }
    </script>
</asp:Content>

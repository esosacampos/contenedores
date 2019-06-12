<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfCargarExport.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfCargarExport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/bootstrap/csss/bootstrap.fd.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Carga de Archivo de Exportación</h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
            <tr>
                <td>
                    IMO
                </td>
                <td>
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Nombre del Buque
                </td>
                <td>
                    <asp:Label ID="d_buque_c" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Código de Llegada
                </td>
                <td>
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha de llegada
                </td>
                <td>
                    <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
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

                    var buque = document.getElementById("<%= d_buque_c.ClientID %>").textContent;
                    var c_imo = document.getElementById("<%= c_imo.ClientID %>").textContent;
                    var c_llegada = document.getElementById("<%= c_llegada.ClientID %>").textContent;
                    var f_llegada = document.getElementById("<%= f_llegada.ClientID %>").textContent;

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
                        url: '<%= ResolveClientUrl("~/FileUploadHandler.ashx") %>' + '?d_buque=' + buque + '&c_imo=' + c_imo + '&c_llegada=' + c_llegada + '&f_llegada=' + f_llegada + '&susti=0' + '&aduana=0' + '&proceso=2',
                        type: "POST",
                        data: data,
                        contentType: false,
                        processData: false,
                        success: function (result) {
                            $.unblockUI();
                            bootbox.alert(result);
                        },
                        error: function (err) {
                            $.unblockUI();
                            bootbox.alert(err.statusText);
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
                message: '<img src="CSS/Img/ajax-loader.gif" /><h1>Procesando...</h1>',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#1584ce',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            //            setTimeout($.unblockUI, 2000);

            setTimeout(function () {
                $.unblockUI({
                    onUnblock: function () { alert('Registrado Correctamente'); }
                });
            }, 2000);
        }
    </script>
</asp:Content>

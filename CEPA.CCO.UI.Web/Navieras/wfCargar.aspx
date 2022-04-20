<%@ Page Title="Cargar Archivos" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="wfCargar.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfCargar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/bootstrap/csss/bootstrap.fd.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Carga de Archivo de Importación</h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
              <tr>
                <td>AGENCIA
                </td>
                <td>
                 <asp:HiddenField ID="hNaviera" runat="server" Value="" />
                    <asp:HiddenField ID="hIsoNavi" runat="server" Value="" />
                    <asp:Label ID="d_agencia" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>IMO
                </td>
                <td>
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="hSidunea" runat="server" />
                    <asp:HiddenField ID="h_iBooking" runat="server" />                 
                </td>
            </tr>
            <tr>
                <td>Nombre del Buque
                </td>
                <td>
                    <asp:Label ID="d_buque_c" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Código de Llegada
                </td>
                <td>
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de llegada
                </td>
                <td>
                    <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>                    
                    <div runat="server" id="myRadio">
                        <div>
                            <asp:CheckBox class="label-success" runat="server" ID="radio3" Checked="true" />
                            <label for="radio3">Sidunea World</label>
                        </div>
                    </div>
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

                    var buque = document.getElementById("<%= d_buque_c.ClientID %>").textContent;
                    var c_imo = document.getElementById("<%= c_imo.ClientID %>").textContent;
                    var c_llegada = document.getElementById("<%= c_llegada.ClientID %>").textContent;
                    var f_llegada = document.getElementById("<%= f_llegada.ClientID %>").textContent;

                    var b_sid = document.getElementById('<%= hSidunea.ClientID %>').value
                    var b_booking = document.getElementById('<%= h_iBooking.ClientID %>').value

                    if (b_sid == 1)
                        var check = document.getElementById("<%= radio3.ClientID %>").checked;
                    else
                        var check = false;

                    var c_naviera = document.getElementById("<%= hNaviera.ClientID %>").value;
                    var c_iso_navi = document.getElementById("<%= hIsoNavi.ClientID %>").value;


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
                        url: '<%= ResolveClientUrl("~/FileUploadHandler.ashx") %>' + '?d_buque=' + buque + '&c_imo=' + c_imo + '&c_llegada=' + c_llegada + '&f_llegada=' + f_llegada + '&susti=0' + '&aduana=0' + '&proceso=1' + '&sidunea=true' + '&booking=' + b_booking + '&c_naviera=' + c_naviera + '&c_iso_navi=' + c_iso_navi,
                        type: "POST",
                        data: data,
                        contentType: false,
                        processData: false,
                        success: function (result) {
                            $.unblockUI();
                            bootbox.alert(result);
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

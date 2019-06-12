<%@ Page Title="Sustituir Archivos" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="wfSustituirArch.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfSustituirArch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/bootstrap/csss/bootstrap.fd.css") %>" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Sustituir Archivos</h2>
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
    <br />
    <br />
    <span class="style12">Seleccionar archivo a sustituir</span>
    <br />
    <br />
    <div style="margin: 0 auto 0 auto; width: 690px; text-align: center;">
        <asp:ListBox ID="lblArchivos" runat="server" OnSelectedIndexChanged="lblArchivos_SelectedIndexChanged" class="list-group"
            Width="80%"></asp:ListBox>
    </div>
    <br />
    <br />
    <nav>
        <ul class="pager">
            <li class="previous">
                <input type="button" id="open_btn" class="btn btn-primary btn-lg" value="Cargar Listado">
            <li class="next">
                <asp:Button ID="Button1" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar"
                    OnClick="btnRegresar_Click" /></li>
        </ul>
    </nav>
   <%-- <div style="float: left; width: 45%">
        <asp:Button ID="btnCargar" runat="server" Text="Cargar Archivo" OnClick="btnCargar_Click"
            OnClientClick="return UserDeleteConfirmation();" />
        
    </div>
    <div style="float: right; width: 45%">
        <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" OnClick="btnRegresar_Click" />
    </div>
    <br />
    <br />
    <br />
    <div id="dialogContent">
    </div>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="<%= ResolveUrl("~/bootstrap/js/bootstrap.fd.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#open_btn").click(function () {

                var error = 0;
                var listbox = document.getElementById('<%=lblArchivos.ClientID%>');


                if (document.getElementById('<%=lblArchivos.ClientID%>').selectedIndex == -1) {
                    alert("Debe seleccionar listado a sustituir");
                }

                


                if (error == 0) {

                    if (confirm("Esta opción eliminara el contenido del documento seleccionado ¿Desea continuar con la sustitución del archivo : " + listbox.options[listbox.selectedIndex].text + "?")) {

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
                            var arch_susti = listbox.options[listbox.selectedIndex].text;

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
                                url: '<%= ResolveClientUrl("~/FileUploadHandler.ashx") %>' + '?d_buque=' + buque + '&c_imo=' + c_imo + '&c_llegada=' + c_llegada + '&f_llegada=' + f_llegada + '&arch_susti=' + arch_susti + '&susti=1' + "&aduana=0" + '&proceso=1',
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
                    }
                }
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

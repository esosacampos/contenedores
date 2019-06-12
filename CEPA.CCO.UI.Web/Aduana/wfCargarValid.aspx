<%@ Page Title="Cargar Validacion" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="wfCargarValid.aspx.cs" Inherits="CEPA.CCO.UI.Web.Aduana.wfCargarValid" %>

<%@ Register Src="~/Controles/ucMultiFileUpload.ascx" TagName="ucMultiFileUpload"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= ResolveUrl("~/bootstrap/csss/bootstrap.fd.css") %>" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Cargar Manifiesto Eléctronico
    </h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
            <tr>
                <td>
                    No. Manifiesto
                </td>
                <td>
                    <asp:Label ID="n_manif" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="IdRegP" runat="server" />
                    <asp:HiddenField ID="IdDocP" runat="server" />
                    <asp:HiddenField ID="a_mani" runat="server" />
                </td>
            </tr>
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
                <td class="style11">
                    <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style10">
                    Código de Llegada
                </td>
                <td class="style11">
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style10">
                    Fecha de llegada
                </td>
                <td class="style11">
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

        $(document).ready(function () {
            $("#open_btn").click(function () {


                $.FileDialog({ multiple: true }).on('files.bs.filedialog', function (ev) {
                    var files = ev.files;

                    var data = new FormData();
                    for (var i = 0; i < files.length; i++) {
                        data.append(files[i].name, files[i]);
                    }

                    var buque = document.getElementById("<%= d_buque.ClientID %>").textContent;
                    var IdRegPP = document.getElementById("<%= IdRegP.ClientID %>").value
                    var IdDocRP = document.getElementById("<%= IdDocP.ClientID %>").value
                    var a_maniP = document.getElementById("<%= a_mani.ClientID %>").value
                    var n_manif = document.getElementById("<%= n_manif.ClientID %>").textContent;

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
                        url: '<%= ResolveClientUrl("~/FileUploadHandler.ashx") %>' + '?d_buque=' + buque + '&IdRegC=' + IdRegPP + '&IddocC=' + IdDocRP + '&a_maniC=' + a_maniP + '&n_manif=' + n_manif + "&aduana=1",
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

            //            setTimeout($.unblockUI, 2000);

            setTimeout(function () {
                $.unblockUI({
                    onUnblock: function () { alert('Registrado Correctamente'); }
                });
            }, 2000);
        }
    </script>
</asp:Content>

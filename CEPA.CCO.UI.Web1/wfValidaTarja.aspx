<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfValidaTarja.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfValidaTarja" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">


        .style1
        {
            width: 1095px;
            height: 92px;
        }
        .style8
        {
            height: 23px;
            width: 138px;
        }
        .style9
        {
            height: 23px;
            width: 980px;
        }
        .style10
        {
            height: 46px;
            width: 138px;
        }
        .style11
        {
            height: 46px;
            width: 537px;
        }
        .style12
        {
            height: 46px;
            width: 439px;
        }
        .style13
        {
            height: 45px;
            width: 138px;
        }
        .style14
        {
            height: 45px;
            width: 980px;
        }
    </style>
    <script type="text/javascript" src="Scripts/jquery-11.js"></script>
    <!-- include BlockUI -->
    <script type="text/javascript" src="Scripts/jquery.blockui.js"></script>
    <script type="text/javascript">


        function alertError() {
            $.blockUI({
                message: '<img src="CSS/Img/ajax-loader.gif" /><h1>Buscando...</h1>',
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

            setTimeout($.unblockUI, 2000);
        }


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

        function ErrorAlert() {
            $.blockUI({
                message: '<img src="CSS/Img/red-error.gif" /><h3>Ingrese el número de tarja a validar y sus observaciones</h3>',
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

            setTimeout($.unblockUI, 2000);
        }

        function ErrorCantidad(c_tarja) {
            $.blockUI({
                message: '<img src="CSS/Img/attention03.gif" /><h3>El número de tarja ' + c_tarja + ' ya se encuentra validado</h3>',
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

            setTimeout($.unblockUI, 2000);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Autorización Manual de Nota de Tarja</h2>
    <br />
    <table class="style1">
        <tr>
            <td class="style10">
                N° DE TARJA
            </td>
            <td class="style12">
                <asp:TextBox ID="txtTarja" runat="server" Width="419px"></asp:TextBox>
            </td>
            <td class="style11">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            </td>
        </tr>
        <tr>
            <td class="style13">
                N° DE CONTENEDOR
            </td>
            <td class="style14" colspan="2">
                <asp:TextBox ID="txtContenedor" runat="server" Width="419px" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style8">
                OBSERVACIONES
            </td>
            <td class="style9" colspan="2">
                <asp:TextBox ID="txtObserva" runat="server" TextMode="MultiLine" Height="107px" Width="412px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style8">
                &nbsp;
            </td>
            <td class="style9" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style8">
                &nbsp;
            </td>
            <td class="style9" colspan="2">
                <asp:Button ID="btnReg" runat="server" Text="Guardar" OnClick="btnReg_Click" />
            </td>
        </tr>
    </table>
    <br />
    <div id="domMessage" style="display: none;">
        <h1>
            Buscando...</h1>
    </div>
</asp:Content>

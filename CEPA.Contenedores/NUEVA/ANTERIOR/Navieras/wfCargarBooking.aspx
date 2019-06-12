<%@ Page Title="Cargar Booking" Culture="es-SV" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfCargarBooking.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfCargarBooking" %>

<%@ Register Src="~/Controles/ucMultiFileUpload.ascx" TagName="ucMultiFileUpload"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 1095px;
            height: 46px;
        }
        .style10
        {
            height: 23px;
            width: 154px;
            font-weight: bold;
        }
        .style11
        {
            height: 23px;
            width: 956px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Cargar Booking</h2>
    <br />
    <table class="style1">
        <tr>
            <td class="style10">
                Naviera</td>
            <td class="style11">
                <asp:Label ID="d_naviera" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">
                Fecha
            </td>
            <td class="style11">
                <asp:Label ID="f_registro" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        </table>
    <br />
    <br />
    Seleccionar archivo para booking<br />
    <br />
    <uc1:ucMultiFileUpload ID="ucMultiFileUpload1" runat="server" DestinationFolder="~/Archivos"
        ViewStateMode="Enabled" />
    <br />
    <asp:Button ID="btnCargar" runat="server" Text="Cargar Booking" OnClick="btnCargar_Click" />
    <br />
    <br />
</asp:Content>

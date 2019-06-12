<%@ Page Title="Cargar Archivos" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="wfCargar.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfCargar" %>

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
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Carga de Archivo de Importación</h2>
    <br />
    <table class="style1">
        <tr>
            <td class="style10">
                IMO
            </td>
            <td class="style11">
                <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">
                Nombre del Buque
            </td>
            <td class="style11">
                <asp:Label ID="d_buque_c" runat="server"></asp:Label>
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
    <br />
    <br />
    Seleccionar archivo de importación
    <br />
    <br />
    <br />
    <uc1:ucMultiFileUpload ID="ucMultiFileUpload1" runat="server" DestinationFolder="~/Archivos"
        ViewStateMode="Enabled" />
    <br />
    <div style="float: left; width: 45%">
        <asp:Button ID="btnCargar" runat="server" Text="Cargar Archivo" OnClick="btnCargar_Click" />
    </div>
    <div style="float: right; width: 45%">
        <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" OnClick="btnRegresar_Click" />
    </div>
    <br />
    <br />
</asp:Content>

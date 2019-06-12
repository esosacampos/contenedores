<%@ Page Title="Sustituir Archivos" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="wfSustituirArch.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfSustituirArch" %>

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
        .style12
        {
            font-size: large;
        }
    </style>
     <script type="text/javascript">
         function Confirmar() {
             var response = confirm("¿Desea sustituir este archivo?");
             return response;
         }

         function UserDeleteConfirmation() {
             var error = 0;
             var listbox = document.getElementById('<%=lblArchivos.ClientID%>');


             if (listbox == '') {
                 error = 1;
             }

             if (error == 0) {                 
                 
                 if (confirm("Esta opción eliminara el contenido del documento seleccionado ¿Desea continuar con la sustitución del archivo : " + listbox.options[listbox.selectedIndex].text + "?"))
                     return true;
                 else {                     
                     return false;
                 }
             }
         }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Sustituir Archivos</h2>
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
    <br />
    <br />
    <span class="style12">Seleccionar archivo a sustituir</span>
    <br />
    <br />
    <div style="margin: 0 auto 0 auto; width: 690px; text-align: center;">
        <asp:ListBox ID="lblArchivos" runat="server" OnSelectedIndexChanged="lblArchivos_SelectedIndexChanged"
            Width="80%"></asp:ListBox>
    </div>
    <br />
    <br />
    <uc1:ucMultiFileUpload ID="ucMultiFileUpload1" runat="server" DestinationFolder="~/Archivos"
        ViewStateMode="Enabled" />
    <br />
    <div style="float: left; width: 45%">
        <asp:Button ID="btnCargar" runat="server" Text="Cargar Archivo" 
            onclick="btnCargar_Click" OnClientClick="return UserDeleteConfirmation();"/>
        <%--<input type="button" value="Saludar" id="btSaludar" onclick="javascript:if(Confirmar())" runat="server" />--%>
    </div>
    <div style="float: right; width: 45%">
        <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" OnClick="btnRegresar_Click" />
    </div>
    <br />
    <br />   
    <br />
    <div id="dialogContent">
    </div>
</asp:Content>

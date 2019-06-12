<%@ Page Title="Recepcion Contenedor" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfRegistrarSalida.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfRegistrarSalida" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 60%;
            height: 230px;
            position: relative;
        }
        .style9
        {
            height: 23px;
            width: 307px;
            text-align: right;
            font-size: large;
        }
        .style10
        {
            height: 23px;
            width: 925px;
            font-size: xx-small;
        }
        .style12
        {
            height: 21px;
            width: 307px;
        }
        .style13
        {
            height: 21px;
            width: 925px;
        }
        .style14
        {
            height: 61px;
            width: 307px;
        }
        .style15
        {
            height: 61px;
            width: 925px;
            text-align: right;
        }
        .style16
        {
            height: 61px;
            text-align: left;
        }
        .style17
        {
            height: 61px;
            width: 925px;
            text-align: left;
        }
        .style18
        {
            color: red;
            font-size: xx-large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2>Registrar Recepción De Contenedor</h2>
<br />
    <table class="style1">
        <tr>
            <td class="style9">
                Fecha :
            </td>
            <td class="style10" colspan="3">
                <asp:Label ID="lblFecha" runat="server" style="font-size: large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style9">
                Condición :</td>
            <td class="style10" colspan="3">
                <asp:DropDownList ID="ddlCondicion" runat="server" Width="311px">
                    <asp:ListItem Value="0">&lt;-- Seleccionar --&gt;</asp:ListItem>
                    <asp:ListItem Value="1">Lleno</asp:ListItem>
                    <asp:ListItem Value="1">Vacío</asp:ListItem>
                </asp:DropDownList>
                <span class="style18">*</span></td>
        </tr>
        <tr>
            <td class="style9">
                Tamaño :</td>
            <td class="style10" colspan="3">
                <asp:DropDownList ID="ddlSize" runat="server" Width="311px">
                    <asp:ListItem Value="0">&lt;-- Seleccionar --&gt;</asp:ListItem>
                    <asp:ListItem Value="1">20&#39; ST</asp:ListItem>
                    <asp:ListItem Value="2">40&#39; ST</asp:ListItem>
                    <asp:ListItem Value="3">40&#39; REEF</asp:ListItem>
                    <asp:ListItem Value="4">45&#39; HC</asp:ListItem>
                </asp:DropDownList>
                <span class="style18">*</span></td>
        </tr>
        <tr>
            <td class="style9">
                Contenedor</td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtContenedor" runat="server" Width="288px" Height="27px" placeholder="Ingrese número de contenedor"></asp:TextBox>
                <span class="style18">*</span></td>
                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                TargetControlID="txtContenedor"
                FilterType="Numbers,UppercaseLetters">
                </cc1:FilteredTextBoxExtender>
             </tr>
        <tr>
            <td class="style9">
                Piloto :</td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtPiloto" runat="server" Width="288px" Height="27px" placeholder="Ingrese nombre del piloto"></asp:TextBox>
                <span class="style18">*</span></td>
        </tr>
        <tr>
            <td class="style9">
                Transportista :</td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtTranspor" runat="server" Width="288px" Height="27px" placeholder="Ingrese nombre de transportistas"></asp:TextBox>
                <span class="style18">*</span></td>
        </tr>
        <tr>
            <td class="style9">
                Placa</td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtTranspor0" runat="server" Width="288px" Height="27px" 
                    placeholder="Ingrese número de placa"></asp:TextBox>
                <span class="style18">*</span></td>
        </tr>
        <tr>
            <td class="style9">
                Tara</td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtTranspor1" runat="server" Width="288px" Height="27px" 
                    placeholder="Ingrese tara"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style9">
                Destino</td>
            <td class="style10" colspan="3">
                <asp:DropDownList ID="DropDownList1" runat="server" Width="311px">
                    <asp:ListItem Value="0">&lt;-- Seleccionar --&gt;</asp:ListItem>
                    <asp:ListItem Value="1">Balboa</asp:ListItem>
                    <asp:ListItem Value="2">Lazaro Cardenas</asp:ListItem>
                    <asp:ListItem Value="3">Los Angeles</asp:ListItem>
                </asp:DropDownList>
                <span class="style18">*</span></td>
        </tr>
        <tr>
            <td class="style9">
                Booking</td>
            <td class="style10" colspan="3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style12">
                &nbsp;</td>
            <td class="style13" colspan="3">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style14">
                &nbsp;</td>
            <td class="style15">
                <asp:Button ID="btnSave" runat="server" Text="Registrar" />
            </td>
            <td class="style16">
                &nbsp;</td>
            <td class="style17">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
            </td>
        </tr>
    </table>
<br />

</asp:Content>

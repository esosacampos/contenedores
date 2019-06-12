<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfDespacho.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfDespacho" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 31px;
        }
        .style2
        {
            height: 35px;
        }
        .style3
        {
            color: red;
            font-weight: bold;
            font-size: xx-large;
        }
        .style4
        {
            font-size: large;
        }
        .style5
        {
            height: 35px;
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2>Registro Despacho de Contenedor</h2>
<br />

    <table class="style1" align="center">
        <tr>
            <td class="style5">
                Fecha :
            </td>
            <td class="style2" colspan="3">
                <asp:Label ID="lblFecha" runat="server" Style="font-size: large"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4">
                Contenedor
            </td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtContenedor" runat="server" Width="288px" Height="27px" placeholder="Ingrese número de contenedor"></asp:TextBox>
                <span class="style3">*</span>
            </td>
        </tr>
        <tr>
            <td class="style4">
                Booking</td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtContenedor0" runat="server" Width="288px" Height="27px" 
                    placeholder="Ingrese número de contenedor"></asp:TextBox>
                </td>
        </tr>
        <tr>
            <td class="style4">
                Piloto :
            </td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtPiloto" runat="server" Width="288px" Height="27px" placeholder="Ingrese nombre del piloto"></asp:TextBox>
                <span class="style3">*</span></td>
        </tr>
        <tr>
            <td class="style4">
                Transportista :
            </td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtTranspor" runat="server" Width="288px" Height="27px" placeholder="Ingrese nombre de transportistas"></asp:TextBox>
                <span class="style3">*</span></td>
        </tr>
        <tr>
            <td class="style4">
                Placa
                Cabezal</td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtTranspor0" runat="server" Width="288px" Height="27px" placeholder="Ingrese número de placa"></asp:TextBox>
                <span class="style3">*</span>
            </td>
        </tr>
        <tr>
            <td class="style4">
                No. Chasis 
            </td>
            <td class="style10" colspan="3">
                <asp:TextBox ID="txtTranspor1" runat="server" Width="288px" Height="27px" placeholder="Ingrese tara"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style4">
               Destino
            </td>
            <td class="style10" colspan="3">
                <asp:DropDownList ID="DropDownList1" runat="server" Width="311px">
                    <asp:ListItem Value="0">&lt;-- Seleccionar --&gt;</asp:ListItem>
                    <asp:ListItem Value="1">El Salvador</asp:ListItem>
                    <asp:ListItem Value="2">Guatemala</asp:ListItem>
                    <asp:ListItem Value="3">Honduras</asp:ListItem>
                    <asp:ListItem Value="4">Nicaragua</asp:ListItem>
                </asp:DropDownList>
                <span class="style3">*</span>
            </td>
        </tr>
        <tr>
            <td class="style4">
                No. Genset&nbsp;
             </td>
            <td class="style10" colspan="3">
                &nbsp;<asp:TextBox ID="txtTranspor2" runat="server" Width="288px" Height="27px" 
                    placeholder="Ingrese tara"></asp:TextBox>
            &nbsp;</td>
        </tr>
        <tr>
            <td class="style12">
                &nbsp;
            </td>
            <td class="style13" colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style14">
                &nbsp;
            </td>
            <td class="style15">
                <asp:Button ID="btnSave" runat="server" Text="Registrar" />
            </td>
            <td class="style16">
                &nbsp;
            </td>
            <td class="style17">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

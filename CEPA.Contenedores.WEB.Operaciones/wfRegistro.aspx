<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfRegistro.aspx.cs" Inherits="CEPA.CCO.WEB.Operaciones.wfRegistro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 1045px;
            height: 214px;
        }
        .style6
        {
            height: 23px;
            width: 161px;
        }
        .style9
        {
            height: 23px;
            width: 161px;
            text-align: right;
        }
        .style10
        {
            height: 23px;
            width: 998px;
        }
        .style11
        {
            font-size: 18pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Registro De Operaciones</h2>
<br />
    <table class="style1" align="center">
        <tr>
            <td class="style9">
                <asp:Label ID="Label1" runat="server" Text="Grupo" CssClass="style11"></asp:Label>
            </td>
            <td class="style10">
                <asp:DropDownList ID="ddlGrupo" runat="server" Width="342px">
                    <asp:ListItem Value="0">-- Seleccionar Grupo --</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style9">
                <asp:Label ID="Label2" runat="server" Text="Grúa" CssClass="style11"></asp:Label>
            </td>
            <td class="style10">
                <asp:DropDownList ID="ddlGrua" runat="server" Width="342px">
                    <asp:ListItem Value="0">-- Seleccionar Grúa --</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>                    
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style6">
                &nbsp;</td>
            <td class="style10">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style6">
                &nbsp;</td>
            <td class="style10">
                <asp:Button ID="btnContinuar" runat="server" Text="Continuar >>" 
                    onclick="btnContinuar_Click" Height="48px" Width="176px" style="text-align:center;" ForeColor="White" Font-Bold="true"   
                    BackColor="#1584CE" />
            </td>
        </tr>
    </table>
<br />

</asp:Content>

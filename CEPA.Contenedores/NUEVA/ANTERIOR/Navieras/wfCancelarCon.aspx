﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfCancelarCon.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfCancelarCon" %>

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
        .style13
        {
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Cancelar Contenedores</h2>
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
    <asp:Panel runat="server" ID="Panel1" Width="100%" ScrollBars="vertical" Height="300px">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
            DataKeyNames="IdDeta" Font-Names="Segoe UI Light">
            <Columns>
                <asp:BoundField DataField="IdDeta" HeaderText="ID">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR">
                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="CANCELAR">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OBSERVACIONES">
                    <ItemTemplate>
                        <asp:TextBox ID="txtObservaciones" runat="server" Height="55%" Width="209px" placeholder="Justifique cancelación"></asp:TextBox>
                        <span class="style13">*</span>
                    </ItemTemplate>
                    <ItemStyle Height="45%" Width="10%" />
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
            <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
    <br />
    <asp:Label ID="Label1" runat="server" Text="* Indicar el porque la cancelación si no, la cancelación no será ejecutada"
        Style="color: #FF0000; font-weight: 700"></asp:Label>
    <br />
    <br />
    <div style="float: left; width: 45%">
        <asp:Button ID="btnCargar" runat="server" Text="Cancelar" OnClick="btnCargar_Click" />
        <%--<input type="button" value="Saludar" id="btSaludar" onclick="javascript:if(Confirmar())" runat="server" />--%>
    </div>
    <div style="float: right; width: 45%">
        <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" OnClick="btnRegresar_Click" />
    </div>
</asp:Content>

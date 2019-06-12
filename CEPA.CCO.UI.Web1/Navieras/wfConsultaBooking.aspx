<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsultaBooking.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfConsultaBooking" %>
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
        Consultar Booking</h2>
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
                <asp:BoundField DataField="n_contenedor" HeaderText="ESTADO">
                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>             
            </Columns>
            <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
            <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
    <br />
    <br />
    <br />
</asp:Content>

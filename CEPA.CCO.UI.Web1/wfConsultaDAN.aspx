<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsultaDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsultaDAN" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 831px;
        }
        .style2
        {
            width: 402px;
        }
        .style4
        {
            width: 360px;
        }
        .style5
        {
            width: 102px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2>Contenedores Retenidos DAN</h2>
<br />
    <table class="style1">
        <tr>
            <td class="style5">
                # Contenedor</td>
            <td class="style2">
                <asp:TextBox ID="txtBuscar" runat="server" Width="350px" 
                    placeholder="# de contenedor sin guiones"></asp:TextBox>
            </td>
            <td class="style4">
                <asp:Button ID="btnBuscar" runat="server" Text="Consultar" Height="35px" 
                    Font-Size="12px" onclick="btnBuscar_Click" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                    DataKeyNames="n_contenedor" Font-Names="Segoe UI Light" 
        onrowdatabound="GridView1_RowDataBound">
                    <Columns>
                         <asp:BoundField DataField="n_folio" HeaderText="# OFICIO">
                            <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR">
                            <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                         <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                            <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="f_recepcion" HeaderText="F. RETENIDO" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                            <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                         <asp:BoundField DataField="f_tramite" HeaderText="F. TRAMITE" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                            <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="f_liberado" HeaderText="F. LIBERADO" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                            <ItemStyle Width="7%" VerticalAlign="Middle" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="c_correlativo" 
                            HeaderText="TOTAL HORAS">
                            <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CalcDiasD" HeaderText="TIEMPO (Días)" DataFormatString="{0:F2}" HtmlEncode="false">
                            <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>                      
                    </Columns>
                    <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
                    <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
                   <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="16px" />
                    <EmptyDataTemplate>
                       <asp:Label ID = "lblEmptyMessage" Text="" runat="server" /></EmptyDataTemplate>
                </asp:GridView>
<br />

</asp:Content>

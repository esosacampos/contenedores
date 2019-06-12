<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfConsultaDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.Consulta.wfConsultaDAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Contenedores Retenidos DAN</h2>
    <br />
    <table style="width:100%;">
        <tr>
                <td># Contenedor</td>
        </tr>
        <tr style="width:98%;">
            
            <td style="width:85%;">
                <asp:TextBox ID="txtBuscar" runat="server" style="width:98%;" placeholder="# de contenedor sin guiones"></asp:TextBox>
            </td>
            <td style="width:10%;">
                <asp:LinkButton ID="btnBuscar" runat="server" CssClass="button" OnClick="btnBuscar_Click">...</asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />
    <br />
  <%--  <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
        DataKeyNames="n_contenedor" Font-Names="Arial Narrow" OnRowDataBound="GridView1_RowDataBound"
        Font-Size="9pt">
        <Columns>
            <asp:BoundField DataField="n_folio" HeaderText="OFICIO">
                <ItemStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR">
                <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="f_recepcion" HeaderText="F. RETENIDO" HtmlEncode="false"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="f_tramite" HeaderText="F. TRAMITE" HtmlEncode="false"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="f_liberado" HeaderText="F. LIBERADO" HtmlEncode="false"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="c_correlativo" HeaderText="TOTAL HORAS">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="CalcDiasD" HeaderText="TIEMPO (Días)" DataFormatString="{0:F2}"
                HtmlEncode="false">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
        </Columns>
        <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
        <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
        <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="6px" />
        <EmptyDataTemplate>
            <asp:Label ID="lblEmptyMessage" Text="" runat="server" /></EmptyDataTemplate>
    </asp:GridView>--%>
    <asp:DetailsView ID="DetailsView1" runat="server" Width="98%" 
        AutoGenerateRows="False" ondatabound="DetailsView1_DataBound">
        <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />        
        <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
        <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="6px" />
        <EmptyDataTemplate>
            <asp:Label ID="lblEmptyMessage" Text="" runat="server" style="font-size:20px;" /></EmptyDataTemplate>        
        <Fields>
            <asp:BoundField DataField="n_folio" HeaderText="OFICIO">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true" />
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_navi" HeaderText="NAVIERA">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
             <asp:BoundField DataField="s_comodity" HeaderText="DESCRIPCIÓN">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="f_recepcion" HeaderText="F. RETENIDO" HtmlEncode="false"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="f_tramite" HeaderText="F. TRAMITE" HtmlEncode="false"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="f_liberado" HeaderText="F. LIBERADO" HtmlEncode="false"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                <HeaderStyle Width="7%"  HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="c_correlativo" HeaderText="TOTAL HORAS">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="CalcDiasD" HeaderText="TIEMPO (Días)" DataFormatString="{0:F2}"
                HtmlEncode="false">
                <HeaderStyle Width="7%" HorizontalAlign="Left" Font-Bold="true"/>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            </asp:BoundField>
        </Fields>
    </asp:DetailsView>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder7" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder5" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ContentPlaceHolder6" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>

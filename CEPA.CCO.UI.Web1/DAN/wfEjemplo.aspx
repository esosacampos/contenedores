<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfEjemplo.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfEjemplo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 1095px;
        }
        .style2
        {
            width: 125px;
        }
        .style3
        {
            width: 840px;
        }
        .style4
        {
            width: 160px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Generar Oficio</h2>
    <br />
    <table class="style1">
        <tr>
            <td class="style2">
                Ingrese No. de Oficio
            </td>
            <td class="style4">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
            <td class="style3">
                <asp:Button ID="brnReporte" runat="server" Text="Generar" OnClick="brnReporte_Click" />
                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                    Font-Size="Medium"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <rsweb:reportviewer id="ReportViewer1" runat="server" width="100%" height="900px"
        asyncrendering="true" sizetoreportcontent="true" font-names="Verdana" font-size="8pt"
        interactivedeviceinfos="(Collection)" waitmessagefont-names="Verdana" waitmessagefont-size="14pt">
       <%-- <LocalReport ReportPath="DAN\rptOficio.rdlc">
        </LocalReport>--%>
    </rsweb:reportviewer>
</asp:Content>

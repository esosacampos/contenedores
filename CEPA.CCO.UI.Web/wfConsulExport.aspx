<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsulExport.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsulExport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Consulta Contenedores De Exportación</h2>
    <hr />
    <iframe width="100%" height="400px" id="iFrameEjemplo" src="https://10.1.4.20:8081/wf_fa_cont_exp.aspx"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

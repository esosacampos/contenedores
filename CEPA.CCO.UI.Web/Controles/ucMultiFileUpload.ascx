<%@ Control Language="C#" AutoEventWireup="true" Inherits="CEPA.Tareas.UI.ASP.Controls.MultiFileUpload" Codebehind="ucMultiFileUpload.ascx.cs" %>
<!--    2011 Marcelo Calderón - www.lengasoft.com
        Este código es de uso y distribución libre bajo los términos de GPLv2
        siempre que se conserve este texto. -->

<fieldset  ID="campo" class="fUploadControl" runat="server" style="height: auto; width: 95%">
    <legend>
        <asp:Label ID="lblUploadFilesTitle" Text="Autorización ADUANA" runat="server"></asp:Label>
    </legend>
    <asp:Label ID="lblNote" Text="" CssClass="comment" runat="server" />
    <asp:Panel ID="pnlUpload" runat="server" Width="80%">
        <asp:FileUpload ID="upldFile_0" CssClass="fileInput" runat="server" />
    </asp:Panel>
   <%-- <asp:HyperLink ID="lnkAddFileUpload" CssClass="AddNewButton" 
        NavigateUrl="javascript:addFileUploadCtrl();" Text="Agregar" runat="server" 
        Visible="False" />--%>
    <asp:Button ID="btnUpload" Text="Cargar Archivo" ToolTip="Upload files" runat="server" onclick="btnUpload_Click" />
    <asp:Label ID="lblInfo" Text="" CssClass="mssgOK" runat="server" />
</fieldset>

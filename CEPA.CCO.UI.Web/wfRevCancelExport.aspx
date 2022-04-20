<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfRevCancelExport.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfRevCancelExport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">
        #form1 {
            height: 246px;
            /*text-align: center;*/
        }
        .auto-style1 {
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Revertir Cancelación de Contenedores de Exportación</h2>
    <hr />
         <div class="container-fluid" id="form1">
         <br />
            <div class="col-lg-4 col-md-6">
                <label><strong>N° Contenedor:</strong></label>
                <input type="text" id="txt_Contenedor" name="txt_Contenedor" maxlength="11" required pattern="([A-Z]{3})U([0-9]{7})" autocomplete="off" runat="server" placeholder="N° de Contenedor" class="form-control" />
                <asp:Button id="btn_Buscar" onClick="btn_Buscar_Click" runat="server" Text="Buscar..." class="btn btn-primary btn-lg"/>
                <br /><br />
                <label><strong><span class="auto-style1">Justificación de Reversión:</span><br />
                </strong></label>
                &nbsp;<asp:TextBox id="txt_justificar" rows="5" TextMode="multiline" runat="server" Height="93px" Width="100%" placeholder="Justificar..." requiered/>
                <br />            <br />
                <asp:Button id="guardar" OnClick="btn_Guardar_Click" runat="server" Text="Guardar" class="btn btn-success btn-lg" />
                <asp:Button  id="cancelar" OnClick="btn_Cancelar_Click" runat="server" Text="Limpiar" class="btn btn-primary btn-lg" />
            </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">  
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfExportacion.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfExportacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Lista de Buques Anunciados Exportación</h2>
    <hr />
     <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese nombre de buque">
    </div>
  
    <!-- Tabla -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:Timer ID="Timer1" runat="server" Interval="4000" OnTick="Timer1_Tick">
                </asp:Timer>--%>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="footable"
                DataKeyNames="c_buque" OnRowDataBound="GridView1_RowDataBound" data-filter="#filter" ShowFooter="true" data-page-size="10" OnRowCreated="onRowCreate">
                <Columns>
                    <asp:BoundField DataField="c_imo" HeaderText="COD. IMO"></asp:BoundField>
                    <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="f_llegada" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                        HeaderText="FECHA DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="CantExport" HeaderText="# DE ARCHIVOS"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="Link" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"c_buque", "/Navieras/wfSustituirArch.aspx?c_buque={0}&c_llegada=" + DataBinder.Eval(Container.DataItem, "c_llegada")) %>'
                                Text="Sustituir"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="Link1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"c_buque", "/Navieras/wfCargarEx.aspx?c_buque={0}&c_llegada=" + DataBinder.Eval(Container.DataItem, "c_llegada")) %>'
                                Text="Agregar"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
     <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_GridView1').footable();
        });
    </script>
</asp:Content>

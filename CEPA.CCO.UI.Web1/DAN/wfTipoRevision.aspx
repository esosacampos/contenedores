<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfTipoRevision.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfTipoRevision" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function abrirModal(pagina) {
            var vReturnValue;
            vReturnValue = window.showModalDialog(pagina, "", "dialogHeight: 300px; dialogWidth: 600px; edge: Raised; center: Yes; help: No; resizable: No; status: No; ");

            if (vReturnValue != null && vReturnValue == true) {
                //                            __doPostBack('CargarProceso', '');
                // window.opener.location.href = window.opener.location.href;
                window.location.reload(true);
                return vReturnValue
            }
            else {                
                return false;
            }
        }
    </script>
    <h2>
        Tipo de Revisión</h2>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%"
                DataKeyNames="IdRevision" Style="font-size: 14px;" BorderColor="Black"
                OnRowDataBound="GridView2_RowDataBound">
                <Columns>                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lkButton" runat="server" Font-Size="Medium">Editar</asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="2%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdRevision" HeaderText="ID" SortExpression="IdRevision"
                        FooterStyle-HorizontalAlign="Center" FooterStyle-VerticalAlign="Middle" ReadOnly="True">
                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Clave" HeaderText="CLAVE REVISION" SortExpression="Clave"
                        ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle Width="35%" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="TIPO" SortExpression="Tipo"
                        ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle Width="35%" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Habilitado" HeaderText="ESTADO" SortExpression="Habilitado"
                        ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#045FB4" Font-Bold="False" ForeColor="White" />
                <RowStyle Wrap="True" />
                <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="14px" />
                <EmptyDataTemplate>
                    No se encontraron registros</EmptyDataTemplate>
                <SelectedRowStyle BackColor="#045FB4" Font-Bold="False" ForeColor="White" />
                <EditRowStyle BackColor="#045FB4" ForeColor="White" />
            </asp:GridView>
            <br />
            <asp:Button ID="Button2" runat="server" Text="Agregar Revision" OnClientClick="javascript:abrirModal('wfAddTipo.aspx?even=insertar')" />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

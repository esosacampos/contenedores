<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfPerfil.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfPerfil" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
    function abrirModal(pagina) {
        var vReturnValue;
        vReturnValue = window.showModalDialog(pagina, "", "dialogHeight: auto; dialogWidth: 600px; edge: Raised; center: Yes; help: No; resizable: No; status: No; ");

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
        Perfiles de Usuarios</h2>
    <br />
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%"
                DataKeyNames="IdPerfil" Style="font-size: small; font-family: 'Arial Narrow'"
                BorderColor="Black" 
                OnSelectedIndexChanged="GridView2_SelectedIndexChanged" 
                onrowdatabound="GridView2_RowDataBound">
                <Columns>
                    <asp:CommandField SelectText="Seleccionar" ShowSelectButton="True" ItemStyle-Width="4%"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lkButton" runat="server" Font-Size="Medium">Editar</asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="2%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdPerfil" HeaderText="# DE PERFIL" SortExpression="IdPerfil"
                        FooterStyle-HorizontalAlign="Center" FooterStyle-VerticalAlign="Middle" ReadOnly="True">
                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NombrePerfil" HeaderText="NOMBRE DEL PERFIL" SortExpression="NombrePerfil"
                        ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle Width="35%" HorizontalAlign="Left" VerticalAlign="Middle" Font-Bold="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Habilitado" HeaderText="ESTADO" SortExpression="Habilitado"
                        ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#0066CC" Font-Bold="False" ForeColor="White" />
                <RowStyle Wrap="True" />
                <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="14px" />
                <EmptyDataTemplate>
                    No se encontraron registros</EmptyDataTemplate>
                <SelectedRowStyle BackColor="#045FB4" Font-Bold="False" ForeColor="White" />
                <EditRowStyle BackColor="Azure" ForeColor="White" />                
            </asp:GridView>
            <br />
            <asp:Button ID="Button1" runat="server" Text="Agregar" OnClientClick="javascript:abrirModal('wfAddPerfil.aspx?even=insertar')" />
            <br />
            <h3 style="text-align:center;">
                Opciones de Perfil</h3>
            <div style="margin:10px 250px;">
                <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" ShowLines="true" Font-Size="12px"
                    Width="50%">
                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                    <NodeStyle Font-Names="Verdana" ForeColor="Black" HorizontalPadding="7px" NodeSpacing="0px"
                        VerticalPadding="0px" />
                    <ParentNodeStyle Font-Bold="False" />
                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                        VerticalPadding="0px" />
                </asp:TreeView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

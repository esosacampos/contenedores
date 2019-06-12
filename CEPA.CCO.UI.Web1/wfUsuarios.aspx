<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfUsuarios.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
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
        Usuarios del Sistema</h2>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%"
                DataKeyNames="c_usuario" Style="font-size: 14px;"
                BorderColor="Black" 
                onselectedindexchanged="GridView2_SelectedIndexChanged" 
                onrowdatabound="GridView2_RowDataBound">
                <Columns>
                    <asp:CommandField SelectText="Seleccionar" ShowSelectButton="True" ItemStyle-Width="4%"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="4%" />
                    </asp:CommandField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lkButton" runat="server" Font-Size="Medium">Editar</asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="2%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="c_usuario" HeaderText="USUARIO" SortExpression="c_usuario"
                        FooterStyle-HorizontalAlign="Center" FooterStyle-VerticalAlign="Middle" ReadOnly="True">
                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle Width="7%" HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="d_usuario" HeaderText="NOMBRE DEL USUARIO" SortExpression="d_usuario"
                        ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="True" />
                        <ItemStyle Width="32%" HorizontalAlign="Left" VerticalAlign="Middle" Font-Bold="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="d_naviera" HeaderText="CLIENTE" SortExpression="d_naviera"
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
                <HeaderStyle BackColor="#045FB4" Font-Bold="False" ForeColor="White" />
                <RowStyle Wrap="True" />
                <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="14px" />
                <EmptyDataTemplate>
                    No se encontraron registros</EmptyDataTemplate>
                <SelectedRowStyle BackColor="#045FB4" Font-Bold="False" ForeColor="White" />
                <EditRowStyle BackColor="#045FB4" ForeColor="White" />
            </asp:GridView>
            <br />
            <asp:Button ID="Button2" runat="server" Text="Agregar Usuario" OnClientClick="javascript:abrirModal('wfAddUser.aspx?even=insertar')" />
            <br />
            <h3>
                Perfiles Asignados</h3>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                DataKeyNames="IdPerfil" Style="font-size: small; font-family: 'Arial Narrow'"
                BorderColor="Black" 
                onselectedindexchanged="GridView1_SelectedIndexChanged">
                <Columns>
                    <asp:CommandField SelectText="Seleccionar" ShowSelectButton="True" ItemStyle-Width="4%"
                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
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
            <asp:Button ID="Button1" runat="server" Text="Menu Completo" 
                onclick="Button1_Click" />
            <h3>
                Opciones del Perfil</h3>
            <div style="margin: 10px 250px;">
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
</asp:Content>

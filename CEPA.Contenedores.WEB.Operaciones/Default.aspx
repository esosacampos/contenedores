<%@ Page Title="Principal" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="CEPA.Contenedores.WEB.Operaciones._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Listado de Buques</h2>
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
        DataKeyNames="IdReg" Font-Names="Segoe UI Light" OnRowDataBound="GridView1_RowDataBound">
        <Columns>
            <asp:BoundField DataField="IdReg" HeaderText="Id">
                <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_imo" HeaderText="COD. IMO">
                <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA">
                <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE">
                <ItemStyle Width="10%" />
            </asp:BoundField>
            <asp:BoundField DataField="f_llegada" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                HeaderText="FECHA DE ARRIBO">
                <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="Link1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "wfRegistro.aspx?IdReg={0}&") %>'
                        Text="Seleccionar" ForeColor="Blue"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>
        </Columns>
        <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
        <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
</asp:Content>

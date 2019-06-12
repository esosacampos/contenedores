<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfConsultaBuques.aspx.cs" Inherits="CEPA.CCO.UI.Web.Aduana.wfConsultaBuques" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 1095px;
        }
        .style2
        {
            width: 238px;
        }
        .style3
        {
            width: 972px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Lista de Buques Anunciados</h2>
    <br />
    <br>
    <table class="style1">
        <tr>
            <td class="style2">
                Nombre del Buque
            </td>
            <td class="style3">
                <asp:TextBox ID="TextBox1" runat="server" Width="89%"></asp:TextBox>
            </td>
            <td class="style3">
                &nbsp;</td>
        </tr>
    </table>
    <br />
    <asp:Timer ID="Timer1" runat="server" Interval="50000" OnTick="Timer1_Tick">
    </asp:Timer>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="Timer1" />      
            <asp:PostBackTrigger ControlID="btnCargar" />      
        </Triggers>
        
        <ContentTemplate>
            <asp:Panel runat="server" ID="Panel1" Width="100%" ScrollBars="vertical" Height="300px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                    DataKeyNames="IdReg" Font-Names="Segoe UI Light" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="IdReg" HeaderText="ID">
                            <ItemStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                         <asp:BoundField DataField="num_manif" HeaderText="No. Manifiesto">
                            <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA">
                            <ItemStyle Width="22%" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                            HeaderText="FECHA DE LLEGADA">
                            <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CantArchivo" HeaderText="# DE ARCHIVOS">
                            <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="AUTORIZAR TODOS">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="Link1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "/Aduana/wfAutorizarContenedores.aspx?IdReg={0}") %>'
                                    Text="Detallar" ForeColor="Blue"></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="Link2" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "/Aduana/wfCargarValid.aspx?IdReg={0}") %>'
                                    Text="Validar" ForeColor="Blue"></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
                    <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </asp:Panel>
            <br />
            <br />
            <asp:Button ID="btnCargar" runat="server" Text="Autorizar" OnClick="btnCargar_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

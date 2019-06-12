<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfRecepcion.aspx.cs" Inherits="CEPA.CCO.WEB.Operaciones.wfRecepcion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Lista De Contenedores Recibidos En Puerto</h2>
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
        DataKeyNames="IdReg" Font-Names="Segoe UI Light">
        <Columns>
            <asp:BoundField DataField="IdReg" HeaderText="Id">
                <ItemStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR">
                <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="b_estadoF" HeaderText="ESTADO">
                <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="v_tara" HeaderText="TARA">
                <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_correlativo" HeaderText="NUMERAL">
                <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>            
            <asp:BoundField DataField="grupo" HeaderText="GRUPO">
                <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="grua" HeaderText="GRUA">
                <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
             <asp:BoundField DataField="b_chasisc" HeaderText="CHASIS">
                <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="b_rastrac" HeaderText="RASTRA">
                <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="f_recepcion" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                HeaderText="FECHA DE RECEPCIÓN">
                <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" 
                Font-Size="9pt" />
            </asp:BoundField>
            <%--<asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="Link1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "wfRegistro.aspx?IdReg={0}&") %>'
                        Text="Seleccionar" ForeColor="Blue"></asp:HyperLink>
                </ItemTemplate>
                <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:TemplateField>--%>
        </Columns>
        <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
        <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
    <br />
    <br />
    <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" 
        BackColor="#1584CE" ForeColor="White" onclick="btnRegresar_Click" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

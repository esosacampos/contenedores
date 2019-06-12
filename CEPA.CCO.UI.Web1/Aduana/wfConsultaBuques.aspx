<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfConsultaBuques.aspx.cs" Inherits="CEPA.CCO.UI.Web.Aduana.wfConsultaBuques" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type = "text/javascript">
        function abrirModal(pagina) {
            var vReturnValue;
            //vReturnValue = window.showModalDialog(pagina, "", "dialogHeight: 350px; dialogWidth: 650px; edge: Raised; center: Yes; help: No; resizable: No; status: No; ");
            vReturnValue = window.open(pagina, "", "height=350,width=650,status=yes,toolbar=no,menubar=no,location=no , top=40, left=300");
           

//            if (vReturnValue != null && vReturnValue == true) {
//                //                            __doPostBack('CargarProceso', '');
//                // window.opener.location.href = window.opener.location.href;
//                window.location.reload(true);
//                return vReturnValue
//            }
//            else {
//                return false;
//            }
        }

        function Confirm(manif) {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("¿Quieres omitir la validación?")) {
                abrirModal('wfOmitir.aspx?nmanif=' + manif)
            } else {
                confirm_value.value = "No";
            }
            document.getElementById("<%= HiddenField1.ClientID %>").value = confirm_value;
        }

        function Confirm1(manif) {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("¿El manifiesto #"+ manif +"no posee validación desea continuar?")) {
                abrirModal('wfOmitir.aspx?nmanif=' + manif)
            } else {
                confirm_value.value = "No";
            }
            document.getElementById("<%= HiddenField1.ClientID %>").value = confirm_value;
        }

        function ChkClick() {
            var checkBox1 = document.getElementById('ContentPlaceHolder1_GridView1_CheckBox1_0');


            if (confirm('Are you sure?')) {
                __doPostBack('ctl00$ContentPlaceHolder1$GridView1$ctl02$CheckBox1', '');

            }
            else {
                return false;
            }

        };
    </script>
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
    <asp:HiddenField ID="HiddenField1" runat="server" />
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
    <asp:Timer ID="Timer1" runat="server" Interval="30000" OnTick="Timer1_Tick">
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
                         <asp:BoundField DataField="num_manif" HeaderText="No. MANIFIESTO">
                            <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" 
                            Font-Bold="True" Font-Size="Medium" />
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
                        <asp:TemplateField HeaderText="AUTORIZAR TODOS">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" 
                                   oncheckedchanged="CheckBox1_CheckedChanged" />
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="Link1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "/Aduana/wfAutorizarContenedores.aspx?IdReg={0}&n_manif=" + DataBinder.Eval(Container.DataItem, "num_manif")) %>'
                                    Text="Detallar" ForeColor="Blue"></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <%--<asp:HyperLinkField DataNavigateUrlFields="IdReg,num_manif"
                            DataNavigateUrlFormatString="~/Aduana/wfCargarValid.aspx?IdReg={0}&amp;n_manif={1}"                            
                            Text="Validar" HeaderText="" ItemStyle-Font-Size="12px"
                            ControlStyle-Font-Size="12px" ControlStyle-Font-Bold="false" 
                            ControlStyle-Font-Names="Arial">
                            <ControlStyle Font-Bold="False" Font-Names="Arial" Font-Size="12px"></ControlStyle>
                            <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle CssClass="example1" HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:HyperLinkField>    --%>       
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="Link2" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "/Aduana/wfCargarValid.aspx?IdReg={0}&n_manif=" +DataBinder.Eval(Container.DataItem, "num_manif")) %>'
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

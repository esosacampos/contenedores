<%@ Page Title="Detalle Contenedores" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="wfDetalle.aspx.cs" Inherits="CEPA.CCO.WEB.Operaciones.wfDetalle" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">



        function validarEnter(evt, buttonid) {
            var carCode;
            var tx = document.getElementById('<%= txtBusqueda.ClientID %>').value;

            if (window.event)
                carCode = window.event.keyCode; //IE
            else
                carCode = e.which; //firefox

            if (carCode == 13) {
                var bt = document.getElementById(buttonid);

                if (bt != null) { //If we find the button click it
                    bt.click();
                    event.keyCode = 0
                }
            }
        }

        function UserDeleteConfirmation() {
            var error = 0;

            if (document.getElementById('<%=txtBusqueda.ClientID %>').value == '') {
                error = 1;
            }

            if (error == 0) {
                var contenedor = document.getElementById('<%= lblContenedor.ClientID %>');
                contenedor.innerText;
                if (confirm("¿Desea continuar con actualización #Contenedor : " + contenedor.innerHTML + "?"))
                    return true;
                else {
                    ClearAllControls();
                    return false;
                }
            }
        }

        function ClearAllControls() {
            for (i = 0; i < document.forms[0].length; i++) {
                doc = document.forms[0].elements[i];
                switch (doc.type) {
                    case "label":
                        doc.value = "";
                        break;
                    case "text":
                        doc.value = "";
                        break;
                    case "checkbox":
                        doc.checked = false;
                        break;
                    case "radio":
                        doc.checked = false;
                        break;
                    case "select-one":
                        doc.options[doc.selectedIndex].selected = false;
                        break;
                    case "select-multiple":
                        while (doc.selectedIndex != -1) {
                            indx = doc.selectedIndex;
                            doc.options[indx].selected = false;
                        }
                        doc.selected = false;
                        break;

                    default:
                        break;
                }

            }

            var corre = document.getElementById('<%= lblCorrelativo.ClientID %>');
            corre.innerText = "";
            var conte = document.getElementById('<%= lblContenedor.ClientID %>');
            conte.innerText = "";
            var estado = document.getElementById('<%= lblEstado.ClientID %>');
            estado.innerText = "";
            var march = document.getElementById('<%= lblMarchamo.ClientID %>');
            march.innerText = "";

            document.getElementById('<%=txtBusqueda.ClientID %>').focus();

        }

        $(function () {
            // conseguir todas las casillas de verificación
            var $tblChkBox = $("table.cbl input:checkbox");

            // agregar un controlador haga clic en cada casilla
            $tblChkBox.click(function () {
                // obtener el id de la casilla seleccionada
                var selectedId = this.id;

                // desactive todas las casillas excepto la seleccionada
                $tblChkBox.each(function () {
                    if (this.id != selectedId) this.checked = false;
                });
            });
        });
    </script>
    <style type="text/css">
        .style1
        {
            height: 114px; /*border-bottom-color:Blue; border-bottom-style:solid; border-bottom-width:2px;"*/
        }
        .style4
        {
            font-weight: bold;
            color: blue;
            font-size: 12pt;
        }
        .styleC
        {
            font-weight: bold;
            color: Red;
            font-size: 16pt;
        }
        .style5
        {
            width: 464px;
            text-align: right;
            height: 60px;
        }
        .style6
        {
            width: 180px;
            text-align: left;
            height: 48px;
        }
        .style8
        {
            font-size: 12pt;
        }
        .style9
        {
            width: 584px;
            height: 325px;
        }
        .style30
        {
            height: 50px;
            width: 800px;
            text-align: right;
            font-size: 5pt;
        }
        .style36
        {
            height: 50px;
            width: 269px;
            text-align: right;
            font-size: 5pt;
        }
        .style42
        {
            text-align: right;
        }
        .style45
        {
            height: 50px;
            width: 198px;
            font-size: 5pt;
        }
        .style69
        {
            width: 464px;
            text-align: right;
            height: 23px;
        }
        .style70
        {
            height: 23px;
            width: 441px;
        }
        .style73
        {
            width: 79px;
            height: 60px;
        }
        .style74
        {
            width: 180px;
            height: 60px;
        }
        .style76
        {
            height: 23px;
            width: 4px;
        }
        .style77
        {
            height: 2px;
            width: 800px;
            text-align: right;
        }
        .style78
        {
            height: 2px;
            width: 269px;
            text-align: right;
        }
        .style79
        {
            height: 2px;
            width: 198px;
        }
        .style80
        {
            height: 2px;
            width: 131px;
        }
        .style81
        {
            width: 800px;
            text-align: right;
        }
        .style82
        {
            width: 269px;
            text-align: right;
        }
        .style85
        {
            width: 131px;
            height: 50px;
        }
        .style86
        {
            width: 800px;
            text-align: right;
            height: 25px;
        }
        .style87
        {
            width: 269px;
            text-align: right;
            height: 25px;
        }
        .style88
        {
            width: 198px;
            height: 25px;
            font-size: 6px;
        }
        .style89
        {
            width: 131px;
            height: 25px;
        }
        .style91
        {
            width: 198px;
        }
        .style93
        {
            width: 131px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Listado De Contenedores Por Importación</h2>
    <table class="style1">
        <tr>
            <td class="style69">
                <asp:Label ID="Label22" runat="server" Text="Buque:" CssClass="style8"></asp:Label>
            </td>
            <td class="style76">
                &nbsp;
            </td>
            <td class="style70">
                <asp:Label ID="Label23" runat="server" Text="" CssClass="style4"></asp:Label>
            </td>
            <td class="style6" rowspan="3">
                <asp:Button ID="btnModificar" runat="server" OnClick="btnModificar_Click" Text="Modificar Grupo/Grúa"
                    Width="173px" BackColor="#1584CE" ForeColor="White" Font-Bold="True" />
            </td>
        </tr>
        <tr>
            <td class="style69">
                <asp:Label ID="Label2" runat="server" Text="Grupo:" CssClass="style8"></asp:Label>
            </td>
            <td class="style76">
                &nbsp;
            </td>
            <td class="style70">
                <asp:Label ID="Label4" runat="server" Text="" CssClass="style4"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style69">
                <asp:Label ID="Label3" runat="server" Text="Grúa:" CssClass="style8"></asp:Label>
            </td>
            <td class="style76">
            </td>
            <td class="style70">
                <asp:Label ID="Label5" runat="server" Text="" CssClass="style4"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style5">
                <asp:Label ID="Label1" runat="server" Text="# de Contenedor:" Style="font-size: 14pt"></asp:Label>
            </td>
            <td class="style73" colspan="2">
                <asp:TextBox ID="txtBusqueda" runat="server" Width="252px" Style="text-transform: uppercase;
                    font-size: 16pt; text-align:left;" OnTextChanged="txtBusqueda_TextChanged"></asp:TextBox>
                <asp:AutoCompleteExtender ServiceMethod="ObtenerContenedores" MinimumPrefixLength="2"
                    CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" TargetControlID="txtBusqueda"
                    UseContextKey="true" ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false">
                </asp:AutoCompleteExtender>
            </td>
            <td class="style74">
                <asp:Button ID="btnBuscar" runat="server" BackColor="#1584CE" ForeColor="White" Text="Buscar"
                    Width="117px" Font-Bold="True" Style="text-align: center;" 
                    OnClick="btnBuscar_Click" />
            </td>
        </tr>
    </table>
    <h2>
        Detalle Del Contenedor</h2>
    <table class="style9">
        <tr>
            <td class="style77">
                &nbsp;
            </td>
            <td class="style78">
                <asp:Label ID="Label14" runat="server" Text="Numeral:" CssClass="style8"></asp:Label>
            </td>
            <td class="style79">
                <asp:Label ID="lblCorrelativo" runat="server" Text="" CssClass="style4"></asp:Label>
            </td>
            <td class="style80">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style81">
                &nbsp;
            </td>
            <td class="style82">
                <asp:Label ID="Label15" runat="server" Text="No. Contenedor:" CssClass="style8"></asp:Label>
            </td>
            <td class="style91">
                <asp:Label ID="lblContenedor" runat="server" Text="" CssClass="styleC"></asp:Label>
            </td>
            <td class="style93">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style86">
                &nbsp;
            </td>
            <td class="style87">
                <asp:Label ID="Label16" runat="server" Text="Tamaño:" CssClass="style8"></asp:Label>
            </td>
            <td class="style88">
                <asp:TextBox ID="txtSize" runat="server" Width="178px" style="text-transform: uppercase;"></asp:TextBox>
<%--                <img id="imgAjax" src="CSS/images/ajax-loading.gif" alt="Procesando" title="Validando Tamaño" class="ajaxLoader" />
                <img id="imgAjax1" src="CSS/images/check_icon.gif" alt="Procesando" title="Validado" class="ajaxLoader1" />--%>
            </td>
            <td class="style89">
                &nbsp;
               <%-- <asp:Button ID="btnEjecutar" runat="server" Text="" Height="10px" Width="16px" />--%>
            </td>
        </tr>
        <tr>
            <td class="style81">
                &nbsp;
            </td>
            <td class="style82">
                <asp:Label ID="Label17" runat="server" Text="Tara:" CssClass="style8"></asp:Label>
            </td>
            <td class="style91">
                <asp:TextBox ID="txtTara" runat="server" Width="178px"></asp:TextBox>
            </td>
            <td class="style93">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style81">
                &nbsp;
            </td>
            <td class="style82">
                <asp:Label ID="Label18" runat="server" Text="Estado:" CssClass="style8"></asp:Label>
                .
            </td>
            <td class="style91">
                <asp:Label ID="lblEstado" runat="server" Text="" CssClass="style4"></asp:Label>
            </td>
            <td class="style93">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style30">
                &nbsp;
            </td>
            <td class="style36">
                <asp:Label ID="Label19" runat="server" Text="Chasis:" CssClass="style8"></asp:Label>
                <br />
                <asp:Label ID="Label20" runat="server" Text="Rastra:" CssClass="style8"></asp:Label>
            </td>
            <td class="style45">
                <asp:CheckBoxList ID="ckList" runat="server" class="cbl">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                </asp:CheckBoxList>
            </td>
            <td class="style85">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style42">
                &nbsp;
            </td>
            <td class="style82">
                <asp:Label ID="Label21" runat="server" Text="No. de Marchamo:" CssClass="style8"></asp:Label>
            </td>
            <td class="style91">
                <asp:Label ID="lblMarchamo" runat="server" Text="" CssClass="style4"></asp:Label>
            </td>
            <td class="style93">
                <asp:Button ID="btnActualizar" runat="server" BackColor="#1584CE" Font-Bold="True"
                    Font-Size="Medium" ForeColor="White" Text="Actualizar &gt;&gt;" Width="131px"
                    OnClick="btnActualizar_Click" 
                    OnClientClick="return UserDeleteConfirmation();" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Label ID="lblError" Style="font-size: 16pt; font-weight: bold; text-align: center"
                    runat="server" Text="" CssClass="styleC"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h2>
        <<
    </h2>
    <h3 id="titleTop" style="font-size: 12pt;">
        Ultimos Contenedores Recibidos</h3>
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" Width="100%"
            AllowPaging="True" PageSize="5" Font-Size="12px" DataKeyNames="IdReg">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#045FB4" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#045FB4" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#045FB4" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="IdReg,n_contenedor" DataNavigateUrlFormatString="~\wfObservacionesaspx.aspx?IdTareaDiaria={0}&amp;c_expediente_soli={1}"
                    DataTextField="n_contenedor" HeaderText="Contenedor" ItemStyle-Font-Size="12px"
                    ControlStyle-Font-Size="12px" ControlStyle-Font-Bold="false" ControlStyle-Font-Names="Arial">
                    <ControlStyle Font-Bold="False" Font-Names="Arial" Font-Size="12px"></ControlStyle>
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemStyle CssClass="example1" HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="n_contenedor" HeaderText="Contenedor" SortExpression="IdReg"
                    Visible="False" />
                <asp:BoundField DataField="f_recepcion" HeaderText="Fecha/Hora Recepcion" SortExpression="IdReg"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}">
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemStyle Width="40%" VerticalAlign="Middle" HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" />
            <EmptyDataTemplate>
                No se encontraron registros</EmptyDataTemplate>
        </asp:GridView>
        <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click">Ver mas</asp:LinkButton>
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfAddUser.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfAddUser" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="CSS/CEPA_CSS.css" rel="Stylesheet" />
    <title>Usuarios del Sistema</title>
    <meta http-equiv="pragma" content="no-cache" />
    <base target="_self"></base>
    <script type="text/javascript">
        function CerrarConEvento() {
            window.returnValue = true;
            self.close();
        }

        function CerrarSinEvento() {
            window.returnValue = false;
            self.close();
        }
    </script>
    <style type="text/css">
        .style1
        {
            height: 60px;
        }
        body
        {
            border-style: none;
            border-color: inherit;
            border-width: 0;
            margin: 0;
            padding: 0; /*font-family: Segoe UI Light;*/
            font-family: Arial, Verdana, Sans-Serif;
            color: black;
            font-size: 14px;
        }
        .centrar
        {
            width: 270px;
            height: 150px;
            position: absolute;
            left: 50%;
            top: 50%;
            margin: -75px 0 0 -135px;
        }
        * Forms button, input, select, textarea
        {
            overflow: visible; /*vertical-align: baseline;*/
            outline: none;
            margin-left: 0px;
            margin-top: 0;
            margin-bottom: 0;
        }
        input[type=submit]
        {
            background-color: #045FB4;
            color: #FFF;
            font-weight: 700;
            font-size: 18px;
            text-align: center;
        }
        
        label, DropDownList
        {
            font: normal 17px "Segue UI Light" , Segoe UI, Arial, Helvetica, Sans-serif;
        }
        button, input, select, textarea, label
        {
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            line-height: normal;
            font-family: "Segoe UI Light";
        }
    </style>
    <script language="javascript" type="text/javascript">
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }


        function abrirModal(pagina) {
            var vReturnValue;
            vReturnValue = window.showModalDialog(pagina, "", "dialogHeight: 350px; dialogWidth: 550px; edge: Raised; center: Yes; help: No; resizable: No; status: No; ");

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
        function Confirm(even) {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Quieres agregar esta naviera?")) {
                abrirModal('wfUserNavi.aspx?even=' + even)
            } else {
                confirm_value.value = "No";
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="height: 600px">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <table align="center" style="font-size: 12px; text-transform: uppercase; font-family: Verdana;"
                    class="style4">
                    <tr>
                        <td colspan="2" align="center">
                            Mantenimiento
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style7">
                            NOMBRE COMPLETO:
                        </td>
                        <td class="style11">
                            <asp:TextBox ID="txtNombre" runat="server" Width="320px" Style="text-transform: uppercase;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre"
                                ErrorMessage="Hi" Style="color: #FF0000">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style7">
                            NOMBRE USUARIO:
                        </td>
                        <td align="center" class="style11">
                            <asp:TextBox ID="txtUser" runat="server" Width="320px" Style="text-transform: lowercase;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUser"
                                ErrorMessage="Hi" Style="color: #FF0000">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style9">
                            CLIENTE/EMPRESA:
                        </td>
                        <td class="style12">
                            <asp:DropDownList ID="ddlCliente" runat="server" Width="326px" AppendDataBoundItems="True"
                                AutoPostBack="True" 
                                onselectedindexchanged="ddlCliente_SelectedIndexChanged">
                                <asp:ListItem>
                            -- Seleccionar Cliente --
                                </asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style9">
                            ESTADO:
                        </td>
                        <td class="style12">
                            <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" Width="326px">
                                <asp:ListItem>
                            -- Seleccionar Estado --
                                </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DropDownList1"
                                ErrorMessage="Hi" Style="color: #FF0000">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h3>
                                Seleccionar Perfil(es):</h3>
                        </td>
                    </tr>
                </table>
                <br />
                <div style="margin: 0px 150px">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%"
                        DataKeyNames="IdPerfil" Style="font-size: small; font-family: 'Arial Narrow'"
                        BorderColor="Black" OnRowDataBound="GridView2_RowDataBound">
                        <Columns>
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
                            <asp:TemplateField HeaderText="ESTADO">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                </ItemTemplate>
                                <ItemStyle Width="4%" VerticalAlign="Middle" HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#045FB4" Font-Bold="False" ForeColor="White" />
                        <RowStyle Wrap="True" />
                        <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="14px" />
                        <EmptyDataTemplate>
                            No se encontraron registros</EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#045FB4" Font-Bold="False" ForeColor="White" />
                        <EditRowStyle BackColor="Azure" ForeColor="White" />
                    </asp:GridView>
                    <br />
                    <h2 style="text-align: center;">
                        Opciones de Menú</h2>
                    <%-- <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" ShowLines="true" Font-Size="12px"
                        Width="50%">
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <NodeStyle Font-Names="Verdana" ForeColor="Black" HorizontalPadding="7px" NodeSpacing="0px"
                            VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                            VerticalPadding="0px" />
                    </asp:TreeView>--%>
                    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" ShowLines="true" Font-Size="12px"
                        Width="30%" Style="font-family: Arial, Helvetica, sans-serif">
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <NodeStyle Font-Names="Verdana" ForeColor="Black" HorizontalPadding="10px" NodeSpacing="3px"
                            VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="3px"
                            VerticalPadding="2   px" />
                    </asp:TreeView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table align="center" style="font-size: 12px; text-transform: uppercase; font-family: Verdana;">
            <tr>
                <td align="center">
                    <asp:Button ID="BtnAceptar" runat="server" Text="Aceptar" OnClick="BtnAceptar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="CerrarSinEvento();" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

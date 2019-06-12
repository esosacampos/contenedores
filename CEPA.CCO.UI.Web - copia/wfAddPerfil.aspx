<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfAddPerfil.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfAddPerfil" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--  <link href="CSS/Reset.css" rel="stylesheet" type="text/css" media="all" />
    <link href="CSS/CEPA_CSS.css" rel="Stylesheet" type="text/css" />
    <link href="CSS/Meny.css" rel="stylesheet" type="text/css" />--%>
    <title>Perfil de Usuario</title>
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
    <script type="text/javascript">
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

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        <table align="center" style="font-size: 12px; text-transform: uppercase; font-family: Verdana;
            width: 500px;">
            <tr>
                <td colspan="2" align="center">
                    Mantenimiento
                </td>
            </tr>
            <tr>
                <td align="right">
                    NOMBRE:
                </td>
                <td>
                    <asp:TextBox ID="TxtLastName" style="text-transform :uppercase" runat="server" Width="316px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtLastName"
                        ErrorMessage="Hi">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" class="style1">
                    estado:
                </td>
                <td class="style1">
                    <asp:DropDownList ID="DropDownList1" runat="server" Width="326px" AppendDataBoundItems="true"
                        AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        <asp:ListItem>
                            -- Selecionar Estado --
                        </asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DropDownList1"
                        ErrorMessage="Hi">*</asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <br />
        <div style="margin:10px 150px;text-align: center;">
            <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" ShowLines="true" Font-Size="16px"
                Width="30%" ShowCheckBoxes="All" 
                style="font-family: Arial, Helvetica, sans-serif">
                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                <NodeStyle Font-Names="Verdana" ForeColor="Black" HorizontalPadding="7px" NodeSpacing="3px"
                    VerticalPadding="0px" />
                <ParentNodeStyle Font-Bold="False" />
                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="3px"
                    VerticalPadding="2   px" />
            </asp:TreeView>
        </div>
        <br />
        <table align="center">
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="BtnAceptar" runat="server" Text="Aceptar" OnClick="BtnAceptar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="CerrarSinEvento();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>


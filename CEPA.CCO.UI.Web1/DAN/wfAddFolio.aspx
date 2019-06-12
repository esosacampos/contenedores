<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfAddFolio.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfAddFolio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">    
     <link href"../CSS/CEPA_CSS.css" rel="Stylesheet" />
    <title>Oficio DAN</title>
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <br />
            <br />
            <table align="center" style="font-size: 12px; text-transform: uppercase; font-family: Verdana;
                width: 500px;">
                <tr>
                    <td colspan="2" align="center">
                        OFICIO DAN
                    </td>
                </tr>
                <tr>
                    <td align="right" class="style1">
                        No. Oficio:
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="TxtLastName0" Style="text-transform: uppercase" runat="server" Width="316px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table align="center">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="BtnAceptar" runat="server" Text="Aceptar" 
                            onclick="BtnAceptar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="CerrarSinEvento();" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

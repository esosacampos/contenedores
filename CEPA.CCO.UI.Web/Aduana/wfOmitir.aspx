<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfOmitir.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfOmitir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        body
        {
            border-style: none;
            border-color: inherit;
            border-width: 0;
            margin: 0px 0px;
            padding: 0; /*font-family: Segoe UI Light;*/
            font-family: Arial, Verdana, Sans-Serif;
            color: black;
            font-size: 14px;
        }
        .centrar
        {
            width: 370px;
            height: 175px;
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
         .style2
         {
             width: 642px;
             height: 100px;
         }
         .style6
         {
             height: 20px;
         }
         .style11
         {             text-align: center;
         }
         .style13
         {
             width: 458px;
         }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table class="style2">
            <tr>
                <td class="style6" colspan="2">
                    <h1 style="text-align: center">Omitir Validación</h1></td>
            </tr>
            <tr>
                <td class="style11">
                    &nbsp;</td>
                <td class="style13">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style11" colspan="2">
                    Indicar el porque se omitirá la validación.</td>
            </tr>
            <tr>
                <td class="style11" colspan="2">
                    <asp:TextBox ID="TextBox1" runat="server" Height="100px" Width="450px" 
                        TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style11">
                    &nbsp;</td>
                <td class="style13">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style11" colspan="2">
                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" 
                        onclick="btnAceptar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="CerrarSinEvento();" />
                </td>
            </tr>
        </table>
        <br />

    </div>
    </form>
</body>
</html>

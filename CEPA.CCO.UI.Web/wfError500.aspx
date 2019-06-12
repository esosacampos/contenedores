<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfError500.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfError500" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .newStyle1
        {
            font-weight: 200;
            font-size: x-large;
        }
        .style1
        {
            font-weight: bold;
            font-size: larger;
            color: #1584CE;
        }
        .style2
        {
            color: #000066;
            font-size: large;
            text-align: center;
        }
    </style>
</head>
<body style="background: #1584CE; font-family: Segoe UI Ligth; font-weight: lighter;
    font-size: 14px; color: white;">
    <form id="form1" runat="server" 
    style="border-style: none; border-color: inherit; border-width: 0; position: relative; margin: 0 auto 0 auto; width: 100%; text-align: center; background-color: #fff; top: 62px; left: 6px;">
    <div style="display: block; width: 475px; color: #FFF; width: 100%">
        <div style="height: 245px; float: left; width: 49%;">
            <center>
                <img alt="Imagen" src="CSS/images/CEPA_LOGO.gif" width="80%" height="200px" />
            </center>
        </div>
        <div style="height: 50%; float: right; width: 50%;">
            <h1>
                <span class="style1">HTTP Error 500 </span></h1>
            <h2 style="color: #003366; font-weight: 700">
                CEPA - Contenedores</h2>
            <p class="style2">
                Está página aún se encuentra en construcción.
            </p>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="<< Regresar" BackColor="#1584CE" ForeColor="White"
                OnClick="Button1_Click" Font-Bold="False" Font-Names="Segoe UI" Height="50px"
                Style="font-family: 'Segoe UI Semibold'; font-size: large" />
            <br />
            <br />
        </div>
        <div style="float: left; border-top: 1px solid #ccc; width: 98%; padding: 10px 10px 10px 10px;
            font-size: 0.8em; color: #000066;">
            <p style="font-family: Segoe UI; font-size: larger;">
                © 2013 CEPA / Acajutla, El Salvador - Todos los derechos reservados.
            </p>
        </div>
    </div>
    </form>
</body>
</html>

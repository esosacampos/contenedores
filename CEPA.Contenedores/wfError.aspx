<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfError.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfError" %>

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
            text-align: left;
        }
    </style>
</head>
<body style="background: #1584CE; font-family: Segoe UI Ligth; font-weight: lighter;
    font-size: 14px; color: white;">
    <form id="form1" runat="server">
    <div style="margin: 0 auto 0 auto; width: 70%; text-align: center; background-color: #fff;
        border: 0;">
        <div style="display: block; width: 475px; color: #FFF; width: 100%">
            <div style="line-height: 100px; height: 245px; float: left; width: 49%;">
               <center>
                    <img alt="Imagen" src="CSS/images/CEPA_LOGO.gif" width="80%" height="200px" />
                </center>
            </div>
            <div style="height: 50%; float: right; width: 50%;">
                <h1>
                    <span class="style1">HTTP Error 400 </span>ola</h1>
                <h2 style="color: #003366; font-weight: 700">
                    CEPA - Contenedores</h2>
                <p class="style2">
                    Solicitud Incorrecta : Página o servicio solicitado no es correcta</p>
                <br />
                <br />
                <asp:Button ID="Button1" runat="server" Text="<< Regresar" BackColor="#1584CE" 
                    ForeColor="White" onclick="Button1_Click" />
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
    </div>
    </form>
</body>
</html>

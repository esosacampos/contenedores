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
<body> 
    <form id="form1" runat="server">
    <div style="margin: 0 auto 0 auto; width: 70%; text-align: center; background-color: #fff;
        border: 0;">
        <div style="display: block; width: 475px; color: #FFF; width: 100%">
            <div style="line-height: 100px; height: 245px; float: left; width: 49%;">
               <center>
                    <img alt="Imagen" src="CSS/Imag/cepa.png" width="80%" height="200px" />
                </center>
            </div>
            <div style="height: 50%; float: right; width: 50%;">
                <h1 style="color: #003366; font-weight: 700">
                    CEPA - Contenedores</h1>
                <h2>
                    <span class="style1">Error al procesar la solicitud.</span></h2>
                <h3>
                <span class="style2">
                    Solicitud Incorrecta : Página o servicio solicitado no es correcta</span>
                    </h3>
                <br />
                <br />
                <asp:Button ID="Button1" runat="server" Text="Regresar" CssClass="btn btn-success"
                    ForeColor="Black" OnClick="Button1_Click" />
<%--                <asp:Button ID="Button1" runat="server" Text="<< Regresar" BackColor="#1584CE" 
                    ForeColor="White" onclick="Button1_Click" />--%>
                <br />
                <br />
            </div>
            <div style="float: left; border-top: 1px solid #ccc; width: 98%; padding: 10px 10px 10px 10px;
                font-size: 0.8em; color: #000066;">
                <p style="font-family: Segoe UI; font-size: larger;">
                     © 2013 CEPA / Puerto de Acajutla, El Salvador v3.0 - Para asistencia técnica favor escribir al correo <a href="#">informática.acajutla@cepa.gob.sv</a>
                </p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

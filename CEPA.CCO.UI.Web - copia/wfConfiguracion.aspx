<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConfiguracion.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConfiguracion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .centered {
            text-align: center;
            font-size: 0;
        }

            .centered > div {
                float: none;
                display: inline-block;
                text-align: left;
                font-size: 13px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Cambio de Contraseña</h2>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="col-lg-12 centered">
                <div class="form-group">
                    <div class="input-group">
                        <label for="txtOldPass" class="control-label input-lg" style="width: 174.56px">
                            Contraseña Actual:</label>
                        <asp:TextBox ID="txtOldPass" runat="server" class="form-control input-lg" autocomplete="off" TextMode="Password"
                            placeholder="Ingrese contraseña actual"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <label for="txtNuevaPass" class="control-label input-lg" style="width: 174.56px">
                            Nueva Contraseña:</label>
                        <asp:TextBox ID="txtNuevaPass" runat="server" class="form-control input-lg" autocomplete="off" TextMode="Password"
                            placeholder="Ingrese nueva contraseña"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <label for="txtConfirmar" class="control-label input-lg">
                            Confirmar Contraseña:</label>
                        <asp:TextBox ID="txtConfirmar" runat="server" class="form-control input-lg" autocomplete="off" TextMode="Password"
                            placeholder="Confirmar contraseña"></asp:TextBox>
                    </div>
                </div>
            </div>
            <nav>
                <ul class="pager">
                    <li class="previous">
                        <asp:Button ID="btnReg" runat="server" class="btn btn-primary btn-lg" Text="Guardar" />
                    <li class="next">
                        <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" OnClick="btnRegresar_Click" Text="<< Regresar" /></li>
                </ul>
            </nav>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnReg" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        //On UpdatePanel Refresh.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    SetAutoComplete();
                    SaveValidacion();
                    ClearText();
                }
                $.unblockUI();
            });
        };

        prm.add_beginRequest(function OnBeginRequest(sender, args) {
            $.blockUI({
                message: '<img src="<%= ResolveClientUrl("~/CSS/Img/ajax-loader.gif") %>" /><h1>Procesando...</h1>',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#1771F8',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });
        });

            $(function () {

                SaveValidacion();
            });

            function validatePass(campo) {
                var RegExPattern = /(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$/;                
                if ((campo.match(RegExPattern)) && (campo.value != '')) {
                    return true;
                } else {                    
                    return false;
                }
            }

            function SaveValidacion() {
                $('#<%= btnReg.ClientID %>').click(function (e) {
                    $.blockUI({
                        message: '<img src="<%= ResolveClientUrl("~/CSS/Img/ajax-loader.gif") %>" /><h1>Procesando...</h1>',
                        css: {
                            border: 'none',
                            padding: '15px',
                            backgroundColor: '#1771F8',
                            '-webkit-border-radius': '10px',
                            '-moz-border-radius': '10px',
                            opacity: .5,
                            color: '#fff'
                        }
                    });
                    setTimeout($.unblockUI, 2000);
                   
                    var username = '<%= Session["c_usuario"] %>';
                    var actual = document.getElementById("<%= txtOldPass.ClientID %>").value;
                    var password = document.getElementById("<%= txtNuevaPass.ClientID %>").value;
                    var confirmPassword = document.getElementById("<%= txtConfirmar.ClientID %>").value;

                    //if (validatePass(password)) {
                    //    bootbox.alert("Contraseña debe poseer Entre 6 y 15 caracteres, por lo menos un digito y un alfanumérico, y no puede contener caracteres espaciales");
                    //    return false;
                    //}


                    if (password.length > 0 && confirmPassword.length > 0 && actual.length > 0) {
                        if (password != confirmPassword) {
                            bootbox.alert("Contraseñas no coinciden!!");
                            return false;
                        }
                    }
                    else {
                        bootbox.alert("Ingrese la información requerida");
                        return false;
                    }
                    
                    var params = new Object();
                    params.oldPass = actual;
                    params.newPass = password;
                    params.userName = username;
                    params = JSON.stringify(params);

                    $.ajax({
                        url: '<%=ResolveUrl("/wfConfiguracion.aspx/setPassword") %>',
                        data: params,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var fields = data.d.split("|");

                            if (fields[0] == "0") {

                            }

                            $.unblockUI
                            document.getElementById("<%= txtOldPass.ClientID %>").value = '';
                            document.getElementById("<%= txtNuevaPass.ClientID %>").value = '';
                            document.getElementById("<%= txtConfirmar.ClientID %>").value = '';
                            

                            //bootbox.alert(fields[1]);

                            //setTimeout(function () { bootbox.alert("Inicie sesion nuevamente") }, 3300);

                            bootbox.alert({
                                size: "small",
                                title: "CEPA-Contenedores",
                                message: fields[1] + "<br/>Inicie sesion nuevamente",
                                callback: function () {
                                    $.ajax({
                                        url: '<%=ResolveUrl("/wfConfiguracion.aspx/logOut") %>',
                                        success: function (data) {
                                            window.location.href = "/Inicio.aspx";
                                        },
                                        error: function (response) {
                                            bootbox.alert(response.responseText);
                                        },
                                        failure: function (response) {
                                            bootbox.alert(response.responseText);
                                        }
                                    });
                                }
                            })

                            
                            
                        },
                        error: function (response) {
                            bootbox.alert(response.responseText);
                        },
                        failure: function (response) {
                            bootbox.alert(response.responseText);
                        }
                    });


                });
            }
    </script>
</asp:Content>

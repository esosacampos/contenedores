<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfValidaTarja.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfValidaTarja" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="bootstrap/csss/autocompleteText.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Autorización Manual Para Despacho De Contenedor</h2>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="col-lg-10">
                <div class="form-inline">
                    <div class="input-group">
                        <asp:TextBox ID="txtMani" runat="server" class="form-control" autocomplete="off" MaxLength="4"
                            placeholder="9999" onkeydown="return jsDecimals(event);"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <asp:TextBox ID="Datepicker" runat="server" class="form-control" autocomplete="off"
                            MaxLength="4" placeholder="20XX" Text="" onkeydown="return jsDecimals(event);"></asp:TextBox>
                    </div>
                    <!-- /input-group -->
                </div>
            </div>
            <br />
            <br />
            <div class="col-lg-10">
                <div class="form-inline">
                    <div class="input-group" style="width: 88%;">
                        <asp:TextBox ID="txtContenedor" runat="server" CssClass="form-control" placeholder="ingrese los ultimos 4 digitos del contenedor" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </div>
                    <!-- /input-group -->
                    <div class="form-group">
                        <div runat="server" id="myRadio">
                            <div>
                                <asp:CheckBox class="label-success" runat="server" ID="radio3" />
                                <label for="radio3">Shipper Own</label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <div class="col-lg-10">
                <div class="form-group">
                    <label for="recipient-name" class="control-label">
                        Observaciones:</label>
                    <asp:TextBox ID="txtObserva" runat="server" class="form-control" TextMode="multiline" Rows="5" MaxLength="254" placeholder="Ingrese comentarios"
                        Style="text-transform: uppercase;" OnLoad="txtObserva_Load"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-10 alert-danger">
                <p style="font-weight: bold;">
                    OBSERVACIONES:
                </p>
                <ul>
                    <li>Si la búsqueda no devuelve resultados se recomienda consultar en el tracking ya que ese contenedor puedo haber sido cancelado o el número de manifiesto ingresado es incorrecto</li>
                </ul>
            </div>
            <nav style="margin-top: 25%;">
                <ul class="pager">
                    <li class="previous">
                        <asp:Button ID="btnReg" runat="server" class="btn btn-primary btn-lg" Text="Guardar" />
                    <li class="next">
                        <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" OnClick="btnRegresar_Click" Text="<< Regresar" /></li>
                </ul>
            </nav>
            <div id="domMessage" style="display: none;">
                <h1>Buscando...</h1>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnReg" EventName="Click" />

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">
        function ValidarCaracteres(textareaControl, maxlength) {
            if (textareaControl.value.length > maxlength) {
                textareaControl.value = textareaControl.value.substring(0, maxlength);
                bootbox.alert("Debe ingresar hasta un maximo de " + maxlength + " caracteres");
            }
        }

        function jsDecimals(e) {

            var evt = (e) ? e : window.event;
            var key = (evt.keyCode) ? evt.keyCode : evt.which;
            if (key != null) {
                key = parseInt(key, 10);
                if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
                    //Aca tenemos que reemplazar "Decimals" por "NoDecimals" si queremos que no se permitan decimales
                    if (!jsIsUserFriendlyChar(key, "NoDecimals")) {
                        return false;
                    }
                }
                else {
                    if (evt.shiftKey) {
                        return false;
                    }
                }
            }
            return true;
        }

        // Función para las teclas especiales
        //------------------------------------------
        function jsIsUserFriendlyChar(val, step) {
            // Backspace, Tab, Enter, Insert, y Delete
            if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
                return true;
            }
            // Ctrl, Alt, CapsLock, Home, End, y flechas
            if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
                return true;
            }
            if (step == "Decimals") {
                if (val == 190 || val == 110) {  //Check dot key code should be allowed
                    return true;
                }
            }
            // The rest
            return false;
        }

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
                message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#424242',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });
        });





        function alertError() {
            $.blockUI({
                message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#424242',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            setTimeout($.unblockUI, 2000);
        }


        function almacenando() {
            $.blockUI({
                message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#424242',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });
            setTimeout(function () {
                $.unblockUI({
                    onUnblock: function () { bootbox.alert('Registrado Correctamente'); }
                });
            }, 2000);
        }

        function ErrorAlert() {
            $.blockUI({
                message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#424242',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            setTimeout($.unblockUI, 2000);
        }

        function ErrorCantidad(c_tarja) {
            $.blockUI({
                message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#424242',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            setTimeout($.unblockUI, 2000);
        }



        $(function () {
            SetAutoComplete();

            SaveValidacion();
        });


        function ClearText() {

            var fecha = new Date();
            var ano = fecha.getFullYear();

            document.getElementById("<%= txtMani.ClientID %>").value = '';
            document.getElementById("<%= Datepicker.ClientID %>").value = ano;
            document.getElementById("<%= txtObserva.ClientID %>").value = '';
            document.getElementById("<%= txtContenedor.ClientID %>").value = '';
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


                var n_mani = document.getElementById("<%= txtMani.ClientID %>").value;
                var a_mani = document.getElementById("<%= Datepicker.ClientID %>").value;
                var b_observa = document.getElementById("<%= txtObserva.ClientID %>").value;
                var n_contenedor = document.getElementById("<%= txtContenedor.ClientID %>").value;
                var check = document.getElementById("<%= radio3.ClientID %>").checked;

                var shipper = 0;

                if (check == true)
                    shipper = 1;
                else
                    shipper = 0;


                if (n_mani.length > 0 && a_mani.length > 0 && b_observa.length > 0 && n_contenedor.length > 0) {
                    var params = new Object();
                    params.n_manifiesto = n_mani;
                    params.a_mani = a_mani;
                    params.b_observa = b_observa;
                    params.n_contenedor = n_contenedor;
                    params.radio3 = shipper;
                    params = JSON.stringify(params);

                    $.ajax({
                        url: '<%=ResolveUrl("~/wfValidaTarja.aspx/SaveValid") %>',
                        data: params,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var fields = data.d.split("|");

                            if (fields[0] == "0") {

                            }

                            $.unblockUI
                            var fecha = new Date();
                            var ano = fecha.getFullYear();
                            document.getElementById("<%= txtMani.ClientID %>").value = '';
                            document.getElementById("<%= Datepicker.ClientID %>").value = ano;
                            document.getElementById("<%= txtObserva.ClientID %>").value = '';
                            document.getElementById("<%= txtContenedor.ClientID %>").value = '';
                            document.getElementById("<%= radio3.ClientID %>").checked = false;
                            bootbox.alert(fields[1]);


                        },
                        error: function (response) {
                            bootbox.alert(response.responseText);
                        },
                        failure: function (response) {
                            bootbox.alert(response.responseText);
                        }
                    });
                }
                else {
                    var fecha = new Date();
                    var ano = fecha.getFullYear();

                    document.getElementById("<%= txtMani.ClientID %>").value = "";
                    document.getElementById("<%= Datepicker.ClientID %>").value = ano;
                    document.getElementById("<%= txtObserva.ClientID %>").value = "";
                    document.getElementById("<%= txtContenedor.ClientID %>").value = "";
                    document.getElementById("<%= radio3.ClientID %>").checked = false;
                    bootbox.alert("Llenar el formulario antes de almacenar");
                }
            });
        }



        function SetAutoComplete() {
            $("#<%=txtContenedor.ClientID%>").autocomplete({
                minLength: 1,
                source: function (request, response) {
                    var n_mani = document.getElementById("<%= txtMani.ClientID %>").value;
                    var a_mani = document.getElementById("<%= Datepicker.ClientID %>").value;
                    if (n_mani.length > 0 && a_mani.length > 0) {
                        var params = new Object();
                        params.prefix = request.term;
                        params.n_manifiesto = n_mani;
                        params.a_mani = a_mani;
                        params = JSON.stringify(params);
                        $.ajax({
                            url: '<%=ResolveUrl("~/wfValidaTarja.aspx/GetConte") %>',
                            data: params,
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split('-')[0],
                                        val: item.split('-')[1]
                                    }
                                }))
                            },
                            error: function (response) {
                                bootbox.alert(response.responseText + "Verificar que ese contenedor no se encuentre cancelado y que el número de manifiesto ingresado sea el correcto");
                            },
                            failure: function (response) {
                                bootbox.alert(response.responseText);
                            }
                        });
                    }
                    else {
                        var fecha = new Date();
                        var ano = fecha.getFullYear();
                        document.getElementById("<%= txtMani.ClientID %>").value = "";
                        document.getElementById("<%= Datepicker.ClientID %>").value = ano;
                        document.getElementById("<%= txtMani.ClientID %>").focus();
                        document.getElementById("<%= txtContenedor.ClientID %>").value = "";
                        bootbox.alert("Ingresar el # de manifiesto y su año");


                    }
                }
            });
        }
    </script>
</asp:Content>

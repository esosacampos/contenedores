﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
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
                        <asp:TextBox ID="txtMani" runat="server" class="form-control" autocomplete="off"
                            placeholder="Ingrese # Manifiesto"></asp:TextBox>
                    </div>
                    <div class="input-group">
                        <asp:TextBox ID="Datepicker" runat="server" class="form-control" autocomplete="off"
                            placeholder="Ingrese año del manifiesto" Text="2016"></asp:TextBox>
                    </div>
                    <!-- /input-group -->
                </div>
            </div>
            <br />
            <br />
            <div class="col-lg-10">
                <%--<div class="form-group">--%>
                <asp:TextBox ID="txtContenedor" runat="server" CssClass="form-control" placeholder="ingrese los ultimos 4 digitos del contenedor" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                <%--  </div>--%>
                <!-- /input-group -->
            </div>
            <br />
            <br />
            <div class="col-lg-10">
                <div class="form-group">
                    <label for="recipient-name" class="control-label">
                        Observaciones:</label>
                    <asp:TextBox ID="txtObserva" runat="server" class="form-control" TextMode="multiline" Rows="3" placeholder="Ingrese comentarios"
                        Style="text-transform: uppercase;"></asp:TextBox>
                </div>
            </div>
            <nav style="margin-top: 20%;">
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





            function alertError() {
                $.blockUI({
                    message: '<img src="CSS/Img/ajax-loader.gif" /><h1>Buscando...</h1>',
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#1584ce',
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
                    message: '<img src="CSS/Img/ajax-loader.gif" /><h1>Procesando...</h1>',
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#1584ce',
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
                    message: '<img src="CSS/Img/red-error.gif" /><h3>Ingrese el número de contenedor a validar y sus observaciones</h3>',
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#1584ce',
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
                    message: '<img src="CSS/Img/attention03.gif" /><h3>El número de contenedor ' + c_tarja + ' ya se encuentra validado</h3>',
                    css: {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#1584ce',
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
                document.getElementById("<%= txtMani.ClientID %>").value = '';
                document.getElementById("<%= Datepicker.ClientID %>").value = '2016';
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

                if (n_mani.length > 0 && a_mani.length > 0 && b_observa.length > 0 && n_contenedor.length > 0) {
                    var params = new Object();
                    params.n_manifiesto = n_mani;
                    params.a_mani = a_mani;
                    params.b_observa = b_observa;
                    params.n_contenedor = n_contenedor;
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
                            document.getElementById("<%= txtMani.ClientID %>").value = '';
                            document.getElementById("<%= Datepicker.ClientID %>").value = '2016';
                            document.getElementById("<%= txtObserva.ClientID %>").value = '';
                            document.getElementById("<%= txtContenedor.ClientID %>").value = '';

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
                    document.getElementById("<%= txtMani.ClientID %>").value = "";
                    document.getElementById("<%= Datepicker.ClientID %>").value = "2016";
                    document.getElementById("<%= txtObserva.ClientID %>").value = "";
                    document.getElementById("<%= txtContenedor.ClientID %>").value = "";
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
                                    bootbox.alert(response.responseText);
                                },
                                failure: function (response) {
                                    bootbox.alert(response.responseText);
                                }
                            });
                        }
                        else {
                            document.getElementById("<%= txtMani.ClientID %>").value = "";
                            document.getElementById("<%= Datepicker.ClientID %>").value = "2016";
                            document.getElementById("<%= txtMani.ClientID %>").focus();
                            document.getElementById("<%= txtContenedor.ClientID %>").value = "";
                            bootbox.alert("Ingresar el # de manifiesto y su año");


                        }
                    }
                });
            }
    </script>
</asp:Content>

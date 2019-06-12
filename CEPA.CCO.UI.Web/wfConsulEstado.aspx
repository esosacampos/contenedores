<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsulEstado.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsulEstado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="bootstrap/csss/autocompleteText.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Consulta Estado Contenedor En ADUANA</h2>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="form-inline">
                <div class="input-group" style="width: 35%">
                    <asp:TextBox ID="Datepicker" runat="server" class="form-control" autocomplete="off"
                        placeholder="Ingrese año del manifiesto" Text=""></asp:TextBox>
                    <div class="input-group-btn">                        
                    </div>
                </div>
                <div class="input-group" style="width: 35%">
                    <asp:TextBox ID="txtMani" runat="server" class="form-control" autocomplete="off"
                        placeholder="Ingrese # Manifiesto"></asp:TextBox>
                </div>
            </div>
            <div class="input-group" style="width: 70.3%; margin-top: 5px;">
                <asp:TextBox ID="txtContenedor" runat="server" CssClass="form-control" placeholder="ingrese los ultimos 4 digitos del contenedor" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                <div class="input-group-btn">
                    <asp:Button ID="btnReg" runat="server" class="btn btn-primary" Text="Consultar" OnClick="btnReg_Click" />
                </div>
            </div>

            <br />
            <br />
            <div class="col-lg-10">
                <div class="form-group">
                    <strong>AUTORIZACIÓN DE ADUANA:</strong><asp:Label ID="b_autorizado" runat="server" Text="" Style="margin-left: 15px;"></asp:Label>
                </div>
            </div>
            <div class="col-lg-10" id="div_dga" runat="server">
                <div class="form-group">
                    <strong># DECLARACIÓN (ES) ASOCIADAS:</strong><asp:Label ID="n_declaraciones" runat="server" Text="" Style="margin-left: 15px;"></asp:Label>
                </div>
            </div>

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
                document.getElementById("<%= txtMani.ClientID %>").value = '';
                document.getElementById("<%= Datepicker.ClientID %>").value = '2018';
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
                                url: '<%=ResolveUrl("~/wfConsulEstado.aspx/GetConte") %>',
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
                            document.getElementById("<%= Datepicker.ClientID %>").value = "2018";
                            document.getElementById("<%= txtMani.ClientID %>").focus();
                            document.getElementById("<%= txtContenedor.ClientID %>").value = "";
                            bootbox.alert("Ingresar el # de manifiesto y su año");


                        }
                    }
                });
            }
    </script>
</asp:Content>

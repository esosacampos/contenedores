<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfDocumentos.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfDocumentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .glyphicon-refresh-animate {
            -animation: spin .7s infinite linear;
            -webkit-animation: spin2 .7s infinite linear;
        }

        @-webkit-keyframes spin2 {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            from {
                transform: scale(1) rotate(0deg);
            }

            to {
                transform: scale(1) rotate(360deg);
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Anexos Estadía Contenedores Importación
    </h2>
    <br />

    <div class="row">
        <div class="col-lg-12">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <span># Factura :
                        <%--<asp:Label ID="lblDoc" Text="04CCF000000000" runat="server"></asp:Label>--%></span>
                    </div>
                    <div class="form-group" style="width: 40%;">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" Width="100%" Text="04CCF000000000XXXXXX" autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnBuscar" runat="server" Text="Generar Anexo" CssClass="btn btn-success" OnClick="btnBuscar_Click" OnClientClick="return confirmaSave(this.id);" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">
 var confirmed = false;
function confirmaSave(controlID) {
            if (confirmed) { return true; }          
                    var dialog = bootbox.dialog({                                    
                                    message: "<p class='text-center mb-0'><span class='glyphicon glyphicon-refresh glyphicon-refresh-animate'></span> Su solicitud esta siendo procesada..</p>",
                                    closeButton: false
                                });
                   
                    dialog.init(function(){
                            setTimeout(function(){
                                dialog.find('.bootbox-body').html('Su solicitud esta siendo procesada..');
                                dialog.modal('hide');
                            }, 17500);
                    });
                    
                                // do something in the background
                    //dialog.modal('hide');             
                    /*bootbox.dialog({
                        message  : "Por favor esperar mientras su solicitud se procesa",
                        timeOut : 2000
                    });*/
                    if (controlID != null) {
                        var controlToClick = document.getElementById(controlID);
                        if (controlToClick != null) {
                            confirmed = true;
                            controlToClick.click();
                            confirmed = false;
                        }
                    }          

            return false;
        }

        //On UpdatePanel Refresh.
        <%--var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                   $('#btnBuscar').click();
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


    $(document).ready(function(){         
        $('#btnBuscar').click(function() {
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

    });--%>

    </script>
</asp:Content>

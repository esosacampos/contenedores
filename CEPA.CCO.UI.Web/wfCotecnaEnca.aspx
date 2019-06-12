<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfCotecnaEnca.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfCotecnaEnca" %>

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

        .bs-pagination {
            background-color: #1771F8;
        }

        .btn-sample {
            color: #ffffff;
            background-color: #1D0EED;
            border-color: #021069;
        }

            .btn-sample:hover,
            .btn-sample:focus,
            .btn-sample:active,
            .btn-sample.active,
            .open .dropdown-toggle.btn-sample {
                color: #ffffff;
                background-color: #242B7A;
                border-color: #021069;
            }

            .btn-sample:active,
            .btn-sample.active,
            .open .dropdown-toggle.btn-sample {
                background-image: none;
            }

            .btn-sample.disabled,
            .btn-sample[disabled],
            fieldset[disabled] .btn-sample,
            .btn-sample.disabled:hover,
            .btn-sample[disabled]:hover,
            fieldset[disabled] .btn-sample:hover,
            .btn-sample.disabled:focus,
            .btn-sample[disabled]:focus,
            fieldset[disabled] .btn-sample:focus,
            .btn-sample.disabled:active,
            .btn-sample[disabled]:active,
            fieldset[disabled] .btn-sample:active,
            .btn-sample.disabled.active,
            .btn-sample[disabled].active,
            fieldset[disabled] .btn-sample.active {
                background-color: #1D0EED;
                border-color: #021069;
            }

            .btn-sample .badge {
                color: #1D0EED;
                background-color: #ffffff;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Lista de Buques de Importacion Autorizados</h2>
    <hr />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%-- <asp:Timer ID="Timer1" runat="server" Interval="500000" OnTick="Timer1_Tick">
            </asp:Timer>--%>
            <asp:GridView ID="GridView1" CssClass="footable" runat="server" AutoGenerateColumns="False" DataKeyNames="c_llegada"
                data-filter="#filter"
                ShowFooter="true" OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging"
                data-page-size="10" OnRowCommand="GridView1_RowCommand">
                <Columns>
                    <asp:BoundField DataField="c_imo" HeaderText="COD. IMO"></asp:BoundField>
                    <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="f_llegada" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                        HeaderText="FECHA DE LLEGADA"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--  <asp:HyperLink ID="lnkOpe" runat="server" CssClass="btn btn-primary btn xs" NavigateUrl="javascript:;"
                                Text="L. Operaciones"></asp:HyperLink>--%>
                            <asp:Button ID="lnkOpe" runat="server" OnClientClick="return confirmaSave(this.id);" CausesValidation="false" CommandName="btnOpen" CssClass="btn btn-primary btn xs"
                                Text="L. Operaciones" CommandArgument='<%# Eval("c_llegada")+ ","+ Eval("f_llegada") + "," + Eval("d_buque") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
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
                            }, 10000);
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


        function convertToPagination(obj) {
            var liststring = '<ul class="pagination">';
            $(obj).find("tbody tr").each(function () {
                $(this).children().map(function () {
                    liststring = liststring + "<li>" + $(this).html() + "</li>";
                });
            });
            liststring = liststring + "</ul>";
            var list = $(liststring);
            list.find('span').parent().addClass('active');
            $(obj).replaceWith(list);
        }

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

        var postbackElement;

        function OnBeginRequest(sender, args) {
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

            $('#ContentPlaceHolder1_grvTracking').footable();


        }


        function endRequest(sender, args) {

            if (sender._postBackSettings.panelsToUpdate != null) {

            }

            var a = postbackElement;

            var a = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
            if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {

                var testGrid = $get('<%= GridView1.ClientID %>');

                pageLoad();
                $("#GridView1").load(location.href + " #GridView1");
            }

            $.unblockUI();

        }



        function pageLoad() {
            $(document).ready(function () {
                $('#ContentPlaceHolder1_GridView1').footable();

                $('.bs-pagination td table').each(function (index, obj) {
                    convertToPagination(obj);
                });
            });
        }
    </script>
</asp:Content>

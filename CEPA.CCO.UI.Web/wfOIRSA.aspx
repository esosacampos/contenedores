<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfOIRSA.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfOIRSA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bs-pagination {
            background-color: #1771F8;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Lista de Buques de Importacion</h2>
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
                            <asp:Button ID="lnkOpe" runat="server" CausesValidation="false" OnClientClick="return confirmaSave(this.id);" CommandName="btnOpen" CssClass="btn btn-primary btn xs"
                                Text="L. Operaciones" CommandArgument='<%# Eval("c_llegada") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="lnkCobro" runat="server" OnClientClick="return confirmaSave(this.id);" CausesValidation="false" CommandName="btnCobro" CssClass="btn btn-success btn xs"
                                Text="L. Cobros" CommandArgument='<%# Eval("c_llegada") %>' />
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
                            }, 3500);
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

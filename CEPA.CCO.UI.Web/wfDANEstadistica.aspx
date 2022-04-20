<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfDANEstadistica.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfDANEstadistica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        tr {
            display: table-row !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Reporte Consolidado Contenedores Retenidos DAN</h2>
    <hr />
    
            <div class="col-lg-12">
                <div class="form-inline">
                    <div class="form-group" style="width: 35%">
                        <asp:TextBox ID="txtBuscar" runat="server" MaxLength="4" class="form-control" autocomplete="off" placeholder="introducir año (mínimo permitido 2014)" Style="width: 100%"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="ddlClaves" runat="server" AutoPostBack="false" class="selectpicker show-tick seleccion" data-style="btn-default">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-default" Text="Consultar"
                            OnClick="btnBuscar_Click" />
                    </div>
                </div>
            </div>
            <!-- /.col-lg-6 -->
            <br />
            <br />

    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="mes"
                CssClass="footable" Style="margin-top: 5%; max-width: 98%; margin-left: 15px;" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="mes" HeaderText="MES"></asp:BoundField>
                    <asp:BoundField DataField="retenidos" HeaderText="CONT. RETENIDOS"></asp:BoundField>
                    <asp:BoundField DataField="liberados" HeaderText="CONT. LIBERADOS"></asp:BoundField>
                    <asp:BoundField DataField="pendientes" HeaderText="CONT. PENDIENTES *"></asp:BoundField>
                    <asp:BoundField DataField="libmes" HeaderText="CONT. LIBERADOS MES *"></asp:BoundField>
                    <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="hMes" runat="server" Value='<%#Eval("nmes")%>' />
                            <asp:HiddenField ID="hYear" runat="server" Value='<%#Eval("ayear")%>' />
                            <asp:LinkButton ID="LinkButton1" Text="<%#Eval("pendientes")%>" runat="server" OnClick="return GetSelectedRow(this)"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>   --%>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:Label ID="lblMensaje" runat="server" Text="* Contenedores pendientes de liberar con base al mes de su retención" CssClass="alert-danger lead" Style="margin-left: 15px;"></asp:Label>
    <!-- Modal HTML -->
    <div id="myModalD" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="line-height: 10px;">
                    <button type="button" class="close" id="myCloseD" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 class="modal-title">DETALLE CONTENEDORES RETENIDOS PENDIENTES
                    </h4>
                </div>
                <div class="modal-body">
                    <div role="form">

                        <div class="form-group" style="line-height: 1.4;">
                            <div class="table-responsive">
                                <table class="table" id="myDetail">
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="myOK">
                        Ok</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

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

            $('#ContentPlaceHolder1_GridView1').footable();

            $('#ContentPlaceHolder1_GridView1 tr').addClass('footable-detail-show');
            //$('.footable-row-detail').css("display", "table-row");
        }
        function endRequest(sender, args) {

            $.unblockUI();

            $('#ContentPlaceHolder1_GridView1').footable();
            $('#ContentPlaceHolder1_GridView1 tr').addClass('footable-detail-show');
            // $('.footable-row-detail').css("display", "table-row");

            // $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

        }

        function GetSelectedRow(lnk) {
            var row = lnk.parentNode.parentNode;

            var nmes = $("[id*=hMes]").val();
            var nyear = $("[id*=hYear]").val();
            validar(nmes, nyear);

            return false;
        }



        function pageLoad() {
            $(document).ready(function () {

                $('#ContentPlaceHolder1_GridView1').footable();

                $('#ContentPlaceHolder1_GridView1 tr').addClass('footable-detail-show');
                //$('.footable-detail-show').css("display", "table-row");

            });
        }
    </script>
</asp:Content>

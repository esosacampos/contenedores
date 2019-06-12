<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfTEUS.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfTEUS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        #ContentPlaceHolder1_GridView1 tr
        {
            display:table-row;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <h2>
        TEUS MOVILIZADOS POR LINEA NAVIERA EN EL PUERTO DE ACAJUTLA</h2>
    <hr />
    <div class="col-lg-9">
        <div class="input-group">
            <asp:TextBox ID="txtBuscar" runat="server" MaxLength="4" class="form-control" placeholder="introducir año (mínimo permitido 2014)"></asp:TextBox>
            <span class="input-group-btn">
                <asp:Button ID="btnBuscar" runat="server" class="btn btn-default" Text="Consultar"
                    OnClick="btnBuscar_Click" />
            </span>
        </div>
        <!-- /input-group -->
    </div>
    <!-- /.col-lg-6 -->
    <br />
    <br />
     <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="d_linea"
                CssClass="footable" Style="margin-top:5%;margin-left: 15px;" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="d_linea" HeaderText="LINEA NAVIERA"></asp:BoundField>
                    <asp:BoundField DataField="teu2" HeaderText=""></asp:BoundField>
                    <asp:BoundField DataField="t2" HeaderText="" DataFormatString="{0}%"></asp:BoundField>
                     <asp:BoundField DataField="teu1" HeaderText=""></asp:BoundField>
                    <asp:BoundField DataField="t" HeaderText="" DataFormatString="{0}%"></asp:BoundField>        
                     <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Image ID="Image1" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="10px"
                            Font-Names="Arial Narow" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>        
                    <asp:BoundField DataField="c_agencia" HeaderText=""></asp:BoundField>        
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br /><br /><br /><br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
 <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

        function OnBeginRequest(sender, args) {
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

            //$('#ContentPlaceHolder1_GridView1').footable();
           // $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');
        }
        function endRequest(sender, args) {
            $.unblockUI();         
        }

        function pageLoad() {
            $(document).ready(function () {

                //$('#ContentPlaceHolder1_GridView1').footable();

            });
        }
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfCancelarCon.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfCancelarCon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Cancelar Contenedores</h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
            <tr>
                <td>AGENCIA
                </td>
                <td>
                 <asp:HiddenField ID="hNaviera" runat="server" Value="" />
                    <asp:HiddenField ID="hIsoNavi" runat="server" Value="" />
                    <asp:Label ID="c_prefijo_txt" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>IMO
                </td>
                <td>
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Nombre del Buque
                </td>
                <td>
                    <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Código de Llegada
                </td>
                <td class="style11">
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de llegada
                </td>
                <td>
                    <asp:HiddenField ID="n_manif" runat="server" />
                    <asp:HiddenField ID="c_viaje" runat="server" />
                    <asp:HiddenField ID="c_prefijo" runat="server" />
                    <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <hr />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <hr />
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="IdDeta"
                data-filter="#filter" ShowFooter="true"
                data-page-size="10" CssClass="footable" OnRowCreated="onRowCreate">
                <Columns>
                    <asp:BoundField DataField="c_correlativo" HeaderText="No."></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>                    
                    <asp:TemplateField HeaderText="CANCELAR">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="OBSERVACIONES">
                        <ItemTemplate>
                            <asp:HiddenField ID="hCorre" runat="server" Value='<%#Eval("c_correlativo")%>'></asp:HiddenField>
                            <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" placeholder="Justifique cancelación requerido"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:Label ID="Label1" runat="server" Text="* Indicar el porque la cancelación si no, la cancelación no será ejecutada"
        Style="color: #FF0000; font-weight: 700"></asp:Label>
    <br />
    <br />
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:Button ID="btnCargar" runat="server" Text="Cancelar" class="btn btn-primary btn-lg"
                    OnClick="btnCargar_Click" />
            </li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" class="btn btn-primary btn-lg"
                    OnClick="btnRegresar_Click" />
            </li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">
        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";
            }
            else {
                row.style.backgroundColor = "#efefef";
            }
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
        }
        function endRequest(sender, args) {

            $.unblockUI();

            $('#ContentPlaceHolder1_GridView1').footable();

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

        }



        $(document).ready(function () {
            $('#ContentPlaceHolder1_GridView1').footable();

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');
        });




    </script>
</asp:Content>

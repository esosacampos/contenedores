<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfAutoExport.aspx.cs" Inherits="CEPA.CCO.UI.Web.Aduana.wfAutoExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bs-pagination {
            background-color: #1771F8;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Generar Listado de Exportación de Contenedores</h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
            <tr>
                <td>IMO
                </td>
                <td>
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="h_manifiesto" runat="server" />
                </td>
            </tr>
            <tr>
                <td># de Viaje
                </td>
                <td>
                    <asp:Label ID="viaje" runat="server"></asp:Label>
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
                <td>Agencia Naviera
                </td>
                <td>
                    <asp:Label ID="c_naviera" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Código de Llegada
                </td>
                <td>
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de llegada
                </td>
                <td>
                    <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="IdDeta"
                CssClass="footable" Style="max-width: 98%; margin-left: 15px;" data-filter="#filter"
                data-page-size="10" ShowFooter="true" OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging">
                <Columns>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR">
                        <ItemStyle Width="25%" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="v_peso" HeaderText="PESO" DataFormatString="{0:N}">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="c_pais_destino" HeaderText="DESTINO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="c_detalle_pais" HeaderText="PUERTO DESTINO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="AUTORIZAR">
                        <HeaderTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" onclick="checkAll(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HiddenField ID="Hidden3" runat="server" Value='<%#Eval("IdDeta")%>' />
                            <asp:HiddenField ID="Hidden2" runat="server" Value='<%#Eval("IdDoc")%>' />
                            <asp:HiddenField ID="Hidden1" runat="server" Value='<%#Eval("IdReg")%>' />
                            <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />
                        </ItemTemplate>
                        <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <hr />
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Autorizar" OnClick="btnCargar_Click" />
            </li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar" />
            </li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/jquery.dynDateTime.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        function Check_Click(objRef) {
            var row1 = objRef.parentNode.parentNode;

            if (objRef.checked) {
                row1.style.backgroundColor = "aqua";
            }
            else {
                row1.style.backgroundColor = "#efefef";
            }

            //Get the reference of GridView
            var GridView = row1.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                //var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox") {
                    if (inputList[i].checked == false) {
                        checked = false;
                        break;
                    }
                }
            }
            //headerCheckBox.checked = checked;


        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        row.style.backgroundColor = "#efefef";
                        inputList[i].checked = false;
                    }
                }
            }
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

            $('.bs-pagination td table').each(function (index, obj) {
                convertToPagination(obj);
            });

            $(".tip-top").tooltip({
                placement: 'top'
            });


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

        function pageLoad() {
            $(document).ready(function () {


                $(".nav li,.nav li a,.nav li ul").removeAttr('style');

                //            //for dropdown menu
                $(".form-control.dropdown").parent().removeClass().addClass('dropdown');
                $(".form-control.dropdown>a").removeClass().addClass('dropdown-toggle').append('<b class="caret"></b>').attr('data-toggle', 'dropdown');

                //            //remove default click redirect effect           
                $('.dropdown-toggle').attr('onclick', '').off('click');

                $('#ContentPlaceHolder1_GridView1').footable();

                $('.bs-pagination td table').each(function (index, obj) {
                    convertToPagination(obj);
                });

                $(".tip-top").tooltip({
                    placement: 'top'
                });

            });
        }
    </script>
</asp:Content>

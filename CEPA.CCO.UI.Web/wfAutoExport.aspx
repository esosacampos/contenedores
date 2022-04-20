<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfAutoExport.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfAutoExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .sidenav {
            padding-top: 5px;
            background-color: rgb(241, 241, 241);
            height: calc(100% - 50px);
            min-height: 1050px;
        }

        .h2, h2 {
            font-size: 25px;
        }

        .h1, .h2, .h3, h1, h2, h3 {
            margin-top: 15px;
            margin-bottom: 10px;
        }

        .bs-pagination {
            background-color: #1771F8;
        }

        .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
            padding: 5px;
            line-height: 1.42857143;
            vertical-align: top;
            border-top: 1px solid #ddd;
        }

        .pager {
            margin: 10px 0;
        }

        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 1px;
            text-align: center;
            border-left: none;
        }

        .footable {
            /* border-collapse: collapse; */
            max-width: 98%;
            margin-left: 15px;
            margin-bottom: 3%;
            font-family: "Segoe UI", "Open Sans", serif;
            border-right: 0px transparent;
            border-collapse: collapse;
        }

        body {
            /* font-family: Arial,Helvetica,sans-serif; */
            font-family: Segoe UI, Open Sans, sans-serif, serif;
            font-size: 11px;
            line-height: 1.4;
            color: #333;
            background-color: #fff;
        }

        .footer {
            position: fixed;
            bottom: 0;
            width: 100%;
            height: 50px;
            background-color: #f5f5f5;
            left: 0;
            margin-top: 2%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Generar Listado de Exportación de Contenedores</h2>
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
                data-page-size="10" ShowFooter="true" OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="c_correlativo" HeaderText="No.">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>                    
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="s_consignatario" HeaderText="CONSIGNATARIO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="v_tara" HeaderText="TARA" DataFormatString="{0:N}">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="v_peso" HeaderText="PESO" DataFormatString="{0:N}">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>               
                    <asp:BoundField DataField="c_tipo_doc" HeaderText="TIPO DOCUMENTO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="n_documento" HeaderText="# DOCUMENTO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="c_pais_destino" HeaderText="DESTINO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="c_puerto_trasbordo" HeaderText="PUERTO TRASBORDO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="s_almacenaje" HeaderText="ALMACENAJE">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="s_manejo" HeaderText="MANEJO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="s_posicion" HeaderText="UBICACION">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="n_sello" HeaderText="MARCHAMO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="s_pe" HeaderText="PE">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="INCONSISTENCIA POR:">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlTipoAuto" runat="server" class="selectpicker show-tick seleccion" onchange="changeInput(this)" data-style="btn-primary">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hTipoAuto" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
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
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Autorizar" OnClientClick="return confirmaSave(this.id);" OnClick="btnCargar_Click" />
            </li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar" OnClick="btnRegresar_Click" />
            </li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/jquery.dynDateTime.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">
        function iniciaVariable() {
            $.session.set("_conteVar", '');
            confirmed = false;
        }

        toggleSelectList = function (el, selectID) {
            var selectElement = document.getElementById(selectID);
            if (selectElement) {
                selectElement.disabled = !el.checked;
            }
        }

        function changeInput(objDropDown) {
            var parentRow = objDropDown.parentNode.parentNode.parentNode;

            var idHi = objDropDown.id.replace('ddlTipoAuto', 'hTipoAuto');;
            var hiddenField = document.getElementById(idHi);

            $('input[id=' + idHi + ']').val(objDropDown.value);
        }


        var confirmed = false;

        function chkChange(obj) {
            var ddl = obj.siblings("input[type='select']");

            if ($('this').is(':checked')) {
                ddl.attr('disabled', false);
            }
            else {
                ddl.attr('disabled', true);
            }
        };

        function Check_Click(objRef) {

            //var selectElement = objRef.replace("input", "select").replace("CheckBox1", "ddlTipoAuto");            
            //selectElement.disabled = !objRef.checked;



            var row1 = objRef.parentNode.parentNode;

            if (objRef.checked) {
                row1.style.backgroundColor = "aqua";
                /* $('[id$="ddlTipoAuto"]').attr('disabled', 'disabled');*/
            }
            else {
                row1.style.backgroundColor = "#efefef";

                /*$('[id$="ddlTipoAuto"]').removeAttr("disabled");*/

                //var str2 = objRef.id.replace('CheckBox1', 'ddlTipoAuto');

                //var txtAmountReceive = $("select#" + str2);
                //if (txtAmountReceive[0].selectedIndex == 0) {
                //    bootbox.alert("Indique tipo de denegado");
                //    objRef.checked = false;
                //    row.style.backgroundColor = "#efefef";
                //    return false;
                //}                                         


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

        function confirmaSave(controlID) {
            if (confirmed) { return true; }


            var con = $('#ContentPlaceHolder1_GridView1 tbody tr td :input[type="checkbox"]');
            var cadena = con.filter(':checked').length;

            bootbox.confirm("¿Está seguro de proceder con la autorización de listado de " + cadena + " contenedores de exportación? ", function (result) {
                if (result) {
                    if (controlID != null) {
                        var controlToClick = document.getElementById(controlID);
                        if (controlToClick != null) {
                            confirmed = true;
                            controlToClick.click();
                            confirmed = false;
                        }
                    }
                } else {
                    bootbox.alert("De no continuar con la autorización puede dar F5 para actualizar")
                }

            });

            return false;
        }


        function checkAll(objRef) {

            //var selectElement = objRef.replace("input", "select").replace("CheckBox1", "ddlTipoAuto");
            //selectElement.disabled = !objRef.checked;

            var GridView = objRef.parentNode.parentNode.parentNode.parentNode;
            var contar = $("#ContentPlaceHolder1_GridView1 tbody tr").length
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

        function changeInput(objDropDown) {
            var parentRow = objDropDown.parentNode.parentNode.parentNode;

            var idHi = objDropDown.id.replace('ddlTipoAuto', 'hTipoAuto');;
            var hiddenField = document.getElementById(idHi);


            $('input[id=' + idHi + ']').val(objDropDown.value);

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



            if (args.get_error() != undefined) {
                args.set_errorHandled(true); location.reload(true);
            }
            pageLoad();

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

        function alertMess(mensaje) {          
            bootbox.alert({
                message: mensaje,
                callback: function () {
                    location.reload();
                }
            });
        }

        function pageLoad() {
            $(document).ready(function () {
                $('.selectpicker').selectpicker();


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

                iniciaVariable();

            });
        }
    </script>
</asp:Content>

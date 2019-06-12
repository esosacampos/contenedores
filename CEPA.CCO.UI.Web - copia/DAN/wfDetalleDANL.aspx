<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfDetalleDANL.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfDetalleDANL" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/calendar-blue.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .bootstrap-select:not([class*=col-]):not([class*=form-control]):not(.input-group-btn) {
            width: 140px;
        }

        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 7px;
            text-align: center;
            border-left: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Liberar Contenedores</h2>
    <br />
    <%--<div class="table-responsive">
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
                <td>Nombre del Buque
                </td>
                <td>
                    <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
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
    </div>--%>
    <asp:HiddenField ID="h_manifiesto" runat="server" />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <asp:UpdatePanel runat="server" ID="Employe" UpdateMode="Conditional" >
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="f_retenido,f_recep_patio,c_navi,c_tipo_doc,c_manifiesto"
                CssClass="footable" Style="max-width: 98%; margin-left: 15px;" data-filter="#filter" OnRowDataBound="GridView1_RowDataBound"
                data-page-size="5" ShowFooter="true" OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging">
                <Columns>
                    <asp:BoundField DataField="IdDeta" HeaderText="ID"></asp:BoundField>
                    <asp:BoundField DataField="n_folio" HeaderText="OFICIO"></asp:BoundField>
                    <asp:BoundField DataField="c_cliente" HeaderText="AGENCIA" ></asp:BoundField>
                    <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR" ></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:TemplateField HeaderText="F. INICIO TRAMITE" >
                        <ItemTemplate>
                            <div class="input-group date now" id="datetimepicker1">
                                <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="F. INICIO REVISION">
                        <ItemTemplate>
                            <div class="input-group date other" id="datetimepicker2">
                                <asp:TextBox ID="txtDOB1" runat="server" class="form-control"></asp:TextBox>
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="REVISION" >
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlRevision" runat="server" class="selectpicker seleccion" onchange="changeInput(this)" data-style="btn-primary">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hRevision" runat="server" />
                        </ItemTemplate>                      
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LIBERAR">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="f_retenido" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                        Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="f_recep_patio" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                        Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="c_navi" HtmlEncode="false" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="c_tipo_doc" HtmlEncode="false" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="c_manifiesto" HtmlEncode="false" Visible="false"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click" />           
           <%-- <asp:PostBackTrigger ControlID="GridView1" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <hr />
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Liberar"
                    OnClick="btnCargar_Click" />
                <%--<input type="button" id="btnCargar" class="btn btn-primary btn-lg" value="Retener Contenedores">--%>
            </li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar"
                    OnClick="btnRegresar_Click" />
            </li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%-- <script src="<%= ResolveUrl("~/Scripts/jquery.dynDateTime.min.js") %>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/calendar-en.min.js") %>" type="text/javascript"></script>
    <script src="<%= ResolveUrl("~/Scripts/calendar-es.js") %>" type="text/javascript"></script>--%>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";

                var str = objRef.id.replace('CheckBox1', 'txtDOB');

                var txtAmountReceive = $("input#" + str);
                if (txtAmountReceive[0].value == '') {
                    bootbox.alert("Indique fecha inicio de tramite");
                    objRef.checked = false;
                    row.style.backgroundColor = "#efefef";
                };

                var str1 = objRef.id.replace('CheckBox1', 'txtDOB1');

                var txtAmountReceive = $("input#" + str1);
                if (txtAmountReceive[0].value == '') {
                    bootbox.alert("Indique fecha inicio de revision");
                    objRef.checked = false;
                    row.style.backgroundColor = "#efefef";
                };


                var str2 = objRef.id.replace('CheckBox1', 'ddlRevision');

                var txtAmountReceive = $("select#" + str2);
                if (txtAmountReceive[0].selectedIndex == 0) {
                    bootbox.alert("Indique tipo de revision");
                    objRef.checked = false;
                    row.style.backgroundColor = "#efefef";
                }

            }
            else {
                row.style.backgroundColor = "#efefef";
            }








        }


        function changeInput(objDropDown) {
            var parentRow = objDropDown.parentNode.parentNode.parentNode;
            //var hiddenField = parentRow.find('input[id$=hRevision]');

            var idHi = objDropDown.id.replace('ddlRevision', 'hRevision');;
            var hiddenField = document.getElementById(idHi);

           
            $('input[id=' + idHi + ']').val(objDropDown.value);
           
            //bootbox.alert($('input[id=' + idHi + ']').val());
        }


        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        

        if (Page != null) {
            Page.add_endRequest(function (sender, e) {
                //llenarSelect();
                
                if (sender._postBackSettings.panelsToUpdate != null) {
                    //llenarSelect();
                }
                $.unblockUI();
                setTimeout(function () { location.reload(); }, 1000);
            });
        };

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
            
            
            
        }


        //function endRequest(sender, args) {

        //    $.unblockUI();
        //    llenarSelect();

        //}

      

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

        function llenarSelect() {
            $.ajax({
                type: "POST",
                url: "wfDetalleDANL.aspx/LlenarRevision",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {

                    var ddlCustomers = $(".selectpicker.seleccion");
                    ddlCustomers.empty();
                    ddlCustomers.empty().append('<option selected="selected" value="0">-- Seleccionar --</option>');
                    $.each(r.d, function () {
                        ddlCustomers.append($("<option></option>").val(this['IdRevision']).html(this['t_revision']));
                    });

                    //ddlCustomers.addClass("selectpicker seleccion");
                    //ddlCustomers.attr('data-style', 'btn-primary');
                }
            });
        }

        function pageLoad() {
            $(document).ready(function () {

                //llenarSelect();

                $('#datetimepicker1.now').datetimepicker({
                    //maxDate: new Date(),
                    locale: 'es',
                    format: 'DD/MM/YYYY HH:mm'
                });

                $('#datetimepicker2.other').datetimepicker({
                    locale: 'es',
                    format: 'DD/MM/YYYY HH:mm',
                    maxDate: moment(),
                    useCurrent: false
                });

                $('#datetimepicker1.now').on('dp.change', function (e) {
                    $('#datetimepicker2.other').data('DateTimePicker').minDate(e.date);
                });

                $('#datetimepicker2.other').on('dp.change', function (e) {
                    $('#datetimepicker1.now').data('DateTimePicker').maxDate(e.date);


                    //var f1 = new Date(e.date);
                    //var hoy = new Date();

                    //if (f1 > hoy) {                       
                    //    bootbox.alert('Error: no puede seleccionar dia superior al actual ');
                    //    $('div#datetimepicker2.input-group.date.other').data('DateTimePicker').date(null);
                    //}

                });

                


                $('#ContentPlaceHolder1_GridView1').footable();

                $('.bs-pagination td table').each(function (index, obj) {
                    convertToPagination(obj);
                });

            });
        }
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfDetalleUCCL.aspx.cs" Inherits="CEPA.CCO.UI.Web.UCC.wfDetalleUCCL" EnableEventValidation="false" %>

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
    <div class="col-lg-12">
        <div class="form-inline">
            <div class="input-group" style="width: 25%;">
                <label for="texto">
                    Año</label>
                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" class="selectpicker show-tick seleccion" data-style="btn-default" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="input-group" style="width: 74%; padding-bottom: 22px;">
                <label for="texto">
                    Buscar</label>
                <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida" autocomplete="off">
            </div>
        </div>
    </div>
    <br />
    <asp:HiddenField ID="h_manifiesto" runat="server" />
    <asp:UpdatePanel runat="server" ID="Employe" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="f_retenidoc,f_recep_patioc,c_navi,c_tipo_doc,c_manifiesto"
                CssClass="footable" data-filter="#filter" OnRowDataBound="GridView1_RowDataBound"
                data-page-size="10" ShowFooter="true" OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging" OnPreRender="GridView1_PreRender">
                <Columns>
                    <asp:BoundField DataField="n_folio" HeaderText="OFICIO"></asp:BoundField>
                    <asp:BoundField DataField="c_cliente" HeaderText="AGENCIA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="b_estado" HeaderText="ESTADO"></asp:BoundField>
                    <asp:BoundField DataField="f_tramite_s" HeaderText="F. INICIO TRAMITE"></asp:BoundField>
                    <%--<asp:TemplateField HeaderText="F. INICIO TRAMITE">
                        <ItemTemplate>
                            <div class="input-group date now" id="datetimepicker1">
                                <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="F. INICIO REVISION">
                        <ItemTemplate>
                            <div class="input-group date other" id="datetimepicker2">
                                <asp:TextBox ID="txtDOB1" runat="server" class="form-control"></asp:TextBox>
                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="REVISION">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlRevision" runat="server" class="selectpicker seleccion" onchange="changeInput(this)" data-style="btn-primary">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hRevision" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DET. LIBERACION">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlDetalle" runat="server" class="selectpicker show-tick seleccion" onchange="changeInput2(this)" data-style="btn-info">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hDetalle" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="# MARCHAMO">
                        <ItemTemplate>
                          </span><asp:TextBox ID="txtMarchamo" runat="server" class="form-control"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="LIBERAR">
                        <ItemTemplate>
                            <asp:HiddenField ID="hId" runat="server" Value='<%#Eval("IdDeta")%>' />
                            <asp:HiddenField ID="hLlegada" runat="server" Value='<%#Eval("c_llegada")%>' />
                            <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="f_retenidoc"></asp:BoundField>
                    <asp:BoundField DataField="f_recep_patioc" HtmlEncode="false"
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
    <asp:Label ID="Label1" runat="server" Text="* ESTADO --> DN = Despacho Normal / RD = Retiro Directo / F = Lleno / E = Vacío" CssClass="alert-warning lead" Style="margin-left: 15px;"></asp:Label><br />
    <asp:Label ID="lblMensaje" runat="server" Text="* Los contenedores marcados en ROJO son CANCELADOS" CssClass="alert-danger lead" Style="margin-left: 15px;"></asp:Label>
    <hr />
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:HiddenField ID="txtConfirma" runat="server" />
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Liberar"
                    OnClientClick="return confirmaSave(this.id);" OnClick="btnCargar_Click" />
                <%--OnClientClick="return confirmaSave(); return false;" OnClientClick="javascript:return confirmaSave()" <input type="button" id="btnCargar" class="btn btn-primary btn-lg" value="Retener Contenedores">--%>
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

        function iniciaVariable() {
            $.session.set("_conteVar", '');
            confirmed = false;
        }


        var confirmed = false;


        function confirmaSave(controlID) {
            if (confirmed) { return true; }

            var _cadena = $.session.get("_conteVar");
            _cadena = _cadena.substr(0, _cadena.length - 2)

            bootbox.confirm("Seguro de liberar los siguientes contenedores " + _cadena, function (result) {
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
                    bootbox.alert("De no continuar con la liberación puede dar F5 para actualizar")
                }

            });

            return false;
        }

        function buttonDisable() {
            var s = $.session.get("_conteVar");
            if (s != '')
                $('[id$="btnCargar"]').removeAttr("disabled");
            else
                $('[id$="btnCargar"]').attr('disabled', 'disabled');

        }

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            var _conte = '';
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";

                var str = objRef.id.replace('CheckBox1', 'txtDOB');


                if (row.children[6].innerText == '') {
                    bootbox.alert("Indique fecha inicio de tramite");
                    objRef.checked = false;
                    row.style.backgroundColor = "#efefef";
                    return false;
                };

                var str1 = objRef.id.replace('CheckBox1', 'txtDOB1');

                var txtAmountReceive = $("input#" + str1);
                if (txtAmountReceive[0].value == '') {
                    bootbox.alert("Indique fecha inicio de revision");
                    objRef.checked = false;
                    row.style.backgroundColor = "#efefef";
                    return false;
                };


                var str2 = objRef.id.replace('CheckBox1', 'ddlRevision');

                var txtAmountReceive = $("select#" + str2);
                if (txtAmountReceive[0].selectedIndex == 0) {
                    bootbox.alert("Indique tipo de revision");
                    objRef.checked = false;
                    row.style.backgroundColor = "#efefef";
                    return false;
                }

                var str3 = objRef.id.replace('CheckBox1', 'ddlDetalle');

                var txtAmountReceive = $("select#" + str3);
                if (txtAmountReceive[0].selectedIndex == 0) {
                    bootbox.alert("Indique detalles de liberación");
                    objRef.checked = false;
                    row.style.backgroundColor = "#efefef";
                    return false;
                }

                //var str4 = objRef.id.replace('CheckBox1', 'txtMarchamo');

                //var txtAmountReceive = $("input#" + str4);
                //if (txtAmountReceive[0].value == '') {
                //    bootbox.alert("Indique # de marchamo");
                //    objRef.checked = false;
                //    row.style.backgroundColor = "#efefef";
                //    return false;
                //}

                var conte = row.children[3].innerText + '/ ';
                var _conte = $.session.get("_conteVar") + conte;

                $.session.set("_conteVar", _conte);

                buttonDisable();
            }
            else {
                row.style.backgroundColor = "#efefef";
                var str = objRef.id.replace('CheckBox1', 'txtDOB');

                var txtAmountReceive = $("input#" + str);

                //$('[' + txtAmountReceive + ']').datetimepicker({
                //    //maxDate: new Date(),
                //    locale: 'es',
                //    format: 'DD/MM/YYYY HH:mm'
                //});

                txtAmountReceive[0].value = '';

                txtAmountReceive.datetimepicker({
                    //maxDate: new Date(),
                    locale: 'es',
                    format: 'DD/MM/YYYY HH:mm'
                });

                var str1 = objRef.id.replace('CheckBox1', 'txtDOB1');

                var txtAmountReceive = $("input#" + str1);
                txtAmountReceive[0].value = '';

                var str2 = objRef.id.replace('CheckBox1', 'ddlRevision');
                var txtAmountReceive = $("select#" + str2 + " option:first");


                txtAmountReceive.removeAttr('selected');

                txtAmountReceive.attr('selected', 'selected').trigger('change');

                var str3 = objRef.id.replace('CheckBox1', 'ddlDetalle');
                var txtAmountReceive1 = $("select#" + str3 + " option:first");


                txtAmountReceive1.removeAttr('selected');

                txtAmountReceive1.attr('selected', 'selected').trigger('change');

                //var str4 = objRef.id.replace('CheckBox1', 'txtMarchamo');

                //var txtAmountReceive = $("input#" + str4);
                //txtAmountReceive[0].value = '';



                var conte = row.children[3].innerText + '/ ';
                var _conte = $.session.get("_conteVar");
                var newSession = _conte.replace(conte, '');
                $.session.set("_conteVar", '');
                $.session.set("_conteVar", newSession);

                buttonDisable();
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

        function changeInput2(objDropDown) {
            var parentRow = objDropDown.parentNode.parentNode.parentNode;
            //var hiddenField = parentRow.find('input[id$=hRevision]');

            var idHi = objDropDown.id.replace('ddlDetalle', 'hDetalle');;
            var hiddenField = document.getElementById(idHi);


            $('input[id=' + idHi + ']').val(objDropDown.value);

            //bootbox.alert($('input[id=' + idHi + ']').val());
        }


        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        var postbackElement;

        if (Page != null) {
            Page.add_endRequest(function (sender, e) {
                //llenarSelect();

                if (sender._postBackSettings.panelsToUpdate != null) {
                    //llenarSelect();
                }

                var a = postbackElement;

                var a = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
                if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
                    //__doPostBack('btnCargar', '');

                    var testGrid = $get('<%= GridView1.ClientID %>');
                    var textD = $get('<%= ddlYear.ClientID %>');
                    //alert(testGrid.rows[1].cells[0].innerHTML);

                    //setTimeout(function () { location.reload(); }, 1000);
                    pageLoad();
                    $("#GridView1").load(location.href + " #GridView1");
                    $("#ddlYear").load(location.href + " #ddlYear");
                }

                    $.unblockUI();

                });
        };

        function OnBeginRequest(sender, args) {

            postbackElement = args.get_postBackElement();
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
                    url: "wfDetalleUCCL.aspx/LlenarRevision",
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

                    $('[id$="btnCargar"]').attr('disabled', 'disabled');

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




                    $('#datetimepicker2.other').on('dp.change', function (e) {
                        $('#datetimepicker1.now').data('DateTimePicker').maxDate(e.date);


                        //var f1 = new Date(e.date);
                        //var hoy = new Date();

                        //if (f1 > hoy) {                       
                        //    bootbox.alert('Error: no puede seleccionar dia superior al actual ');
                        //    $('div#datetimepicker2.input-group.date.other').data('DateTimePicker').date(null);
                        //}

                    });

                    $('#datetimepicker1.now').on('dp.change', function (e) {
                        $('#datetimepicker2.other').data('DateTimePicker').minDate(e.date);
                    });

                    iniciaVariable();


                    $('#ContentPlaceHolder1_GridView1').footable();

                    $('.bs-pagination td table').each(function (index, obj) {
                        convertToPagination(obj);
                    });

                    $('.selectpicker').selectpicker();
                });
            }
    </script>
</asp:Content>

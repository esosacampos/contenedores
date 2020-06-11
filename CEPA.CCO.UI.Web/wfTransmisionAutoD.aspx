<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfTransmisionAutoD.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfTransmisionAutoD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bs-pagination {
            background-color: #1771F8;
        }

        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 7px;
            text-align: center;
            border-left: none;
            font-family: 'trebuchet MS', 'Lucida sans', Arial;
        }

        .badge {
            font-size: 1.25em;
        }
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
    <h2>Seleccionar Contenedores para Recepción Automática a la DGA</h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
            <tr>
                <td colspan="2">IMO
                </td>
                <td colspan="2">
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Nombre del Buque
                </td>
                <td colspan="2">
                    <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Código de Llegada
                </td>
                <td colspan="2">
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">Fecha de llegada
                </td>
                <td colspan="2">
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
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" class="filtrar"
                DataKeyNames="c_cliente" CssClass="footable" Style="margin-left: 15px; margin-bottom: 5%;"
                data-filter="#filter" data-page-size="10" ShowFooter="true" data-paging-count-format="{CP} of {TP}"
                OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging"
                OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="c_cliente" HeaderText="AGENCIA"></asp:BoundField>
                    <asp:BoundField DataField="c_manifiesto" HeaderText="# MANIFESTO"></asp:BoundField>
                    <asp:BoundField DataField="c_correlativo" HeaderText="CORR."></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="v_peso" HeaderText="PESO (Kg.)" DataFormatString="{0:F2}"></asp:BoundField>
                    <asp:BoundField DataField="b_estado" HeaderText="ESTADO"></asp:BoundField>                   
                    <asp:BoundField DataField="s_consignatario" HeaderText="CONSIGNATARIO"></asp:BoundField>  
                    <asp:BoundField DataField="b_despacho" HeaderText="DESPACHO"></asp:BoundField>  
                    <asp:BoundField DataField="b_manejo" HeaderText="MANEJO"></asp:BoundField>  
                    <asp:BoundField DataField="b_transferencia" HeaderText="TRANSFERENCIA"></asp:BoundField>  
                    <asp:TemplateField HeaderText="T. AUTO">
                        <ItemTemplate>
                            <asp:HiddenField ID="hId" runat="server" Value='<%#Eval("IdDeta")%>' />
                            <asp:HiddenField ID="hLlegada" runat="server" Value='<%#Eval("c_llegada")%>' />
                            <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>                        
  <nav>
        <ul class="pager">
            <li class="previous">
                <asp:HiddenField ID="txtConfirma" runat="server" />
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Guardar"
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

            bootbox.confirm("Los siguientes contenedores serán marcados como transmisión automática:  " + _cadena, function (result) {
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
                    bootbox.alert("De no continuar con el cambio puede dar F5 para actualizar")
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


                //if (row.children[6].innerText == '') {
                //    bootbox.alert("Indique fecha inicio de tramite");
                //    objRef.checked = false;
                //    row.style.backgroundColor = "#efefef";

                //    return false;
                //};

                //var str1 = objRef.id.replace('CheckBox1', 'txtDOB1');

                //var txtAmountReceive = $("input#" + str1);
                //if (txtAmountReceive[0].value == '') {
                //    bootbox.alert("Indique fecha inicio de revision");
                //    objRef.checked = false;
                //    row.style.backgroundColor = "#efefef";
                //    txtAmountReceive.style.backgroundColor = "#efefef";
                //    return false;
                //};

                var str2 = objRef.id.replace('CheckBox1', 'ddlRevision');

                //var txtAmountReceive = $("select#" + str2);
                //if (txtAmountReceive[0].selectedIndex == 0) {
                //    bootbox.alert("Indique tipo de revision");
                //    objRef.checked = false;
                //    row.style.backgroundColor = "#efefef";
                //    return false;
                //}

                //var str3 = objRef.id.replace('CheckBox1', 'ddlDetalle');

                //var txtAmountReceive = $("select#" + str3);
                //if (txtAmountReceive[0].selectedIndex == 0) {
                //    bootbox.alert("Indique detalles de liberación");
                //    objRef.checked = false;
                //    row.style.backgroundColor = "#efefef";
                //    return false;
                //}

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
                    locale: 'es',
                    format: 'DD/MM/YYYY HH:mm'
                });

                //var str1 = objRef.id.replace('CheckBox1', 'txtDOB1');

                //var txtAmountReceive = $("input#" + str1);
                //txtAmountReceive[0].value = '';

                //var str2 = objRef.id.replace('CheckBox1', 'ddlRevision');
                //var txtAmountReceive = $("select#" + str2 + " option:first");


                //txtAmountReceive.removeAttr('selected');

                //txtAmountReceive.attr('selected', 'selected').trigger('change');

                //var str3 = objRef.id.replace('CheckBox1', 'ddlDetalle');
                //var txtAmountReceive1 = $("select#" + str3 + " option:first");


                //txtAmountReceive1.removeAttr('selected');

                //txtAmountReceive1.attr('selected', 'selected').trigger('change');

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



        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        var postbackElement;

        if (Page != null) {
            Page.add_endRequest(function (sender, e) {
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

                $('[id$="btnCargar"]').attr('disabled', 'disabled');

                iniciaVariable();

                $('#ContentPlaceHolder1_GridView1').footable();

                $('.bs-pagination td table').each(function (index, obj) {
                    convertToPagination(obj);
                });
                                
            });
        }
    </script>
</asp:Content>

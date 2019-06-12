<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfDetalleDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfDetalleDAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bs-pagination {
            background-color: #1771F8;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Retener Contenedores</h2>
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
                <td># de Manifiesto
                </td>
                <td class="style11">
                    <asp:Label ID="manif" runat="server"></asp:Label>
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
            <tr>
                <td>No. de Oficio
                </td>
                <td>
                    <asp:Label ID="i_oficio" runat="server" Text="" Font-Bold="true"></asp:Label>
                    <asp:HiddenField ID="h_oficio" runat="server" />
                    <asp:HiddenField ID="IdRegP" runat="server" />
                    <%-- <asp:LinkButton ID="btnOFicio" runat="server" class="btn btn-primary btn-xs" data-title="Oficio"
                        data-toggle="modal" data-target="#myModal"><span class="glyphicon glyphicon-pencil tip-top"  data-toggle="tooltip" data-original-title="Modificar # de Oficio" style="cursor:pointer;"></span></asp:LinkButton>--%>
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
                CssClass="footable" data-filter="#filter"
                data-page-size="10" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound" OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging">
                <Columns>
                    <asp:BoundField DataField="IdDeta" HeaderText="ID"></asp:BoundField>
                    <asp:BoundField DataField="c_correlativo" HeaderText="CORRELATIVO"></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="b_rdt" HeaderText="ESTADO"></asp:BoundField>
                    <asp:BoundField DataField="c_pais_origen" HeaderText="ORIGEN"></asp:BoundField>
                    <asp:BoundField DataField="f_salidas" HeaderText="F. B/SALIDA"></asp:BoundField>
                    <asp:TemplateField HeaderText="TIPO REVISION">
                        <ItemTemplate>
                            <asp:HiddenField ID="_hb_bloqueo" runat="server" Value='<%#Eval("b_bloqueo")%>' />
                            <asp:DropDownList ID="ddlRevision" runat="server" class="form-control dropdown" data-style="btn-inverse">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ESCANER">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" class="checkbox-inline" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RETENER">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" class="checkbox-inline" onclick="Check_Click(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="n_BL" HeaderText=""></asp:BoundField>
                    <asp:BoundField DataField="s_consignatario" HeaderText=""></asp:BoundField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <br />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            La columna <b>ESTADO</b> indica lo siguiente : Si el contenedor es <b>RD</b> es
            Retiro Directo y si es <b>T</b> es Trasbordo.</label> <br />
          <label for="texto" class="alert-success">
            La columna <b>F. B/SALIDA</b> indica si un contenedor ya posee boleta de salida </label>
    </div>
    <hr />
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Retener"
                    OnClick="btnCargar_Click" />
                <%--<input type="button" id="btnCargar" class="btn btn-primary btn-lg" value="Retener Contenedores">--%>
            </li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar"
                    OnClick="btnRegresar_Click" />
            </li>
        </ul>
    </nav>
    <!-- Modal HTML -->
    <div id="myModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" id="myClose" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 class="modal-title">Registrar No. Oficio</h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">
                                # Oficio:</label>
                            <input type="text" class="form-control" id="nfolio" placeholder="Ingrese numero y de clic en Registrar">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" id="myCancel">
                        Cancelar</button>
                    <button type="button" class="btn btn-primary" id="myOK">
                        Registrar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/jquery.dynDateTime.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        function Check_Click(objRef) {
            var row1 = objRef.parentNode.parentNode.parentNode;

            if (objRef.checked) {
                row1.style.backgroundColor = "aqua";

                var row2 = objRef.parentNode.parentNode.parentNode;

                var rowIndex1 = row2.rowIndex - 1;
                var IdDeta = row2.cells[0].innerText;
                var n_contenedor = row2.cells[2].innerText;

                var params2 = new Object();
                params2.pIdDeta = IdDeta;
                params2 = JSON.stringify(params2);

                $.ajax({
                    type: "POST",
                    url: "wfDetalleDAN.aspx/RetenValid",
                    data: params2,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.d != "LIBRE" && result.d != "") {
                            objRef.checked = false;
                            row1.style.backgroundColor = "#efefef"
                            bootbox.alert("Este contenedor #" + n_contenedor + " ya no se encuentra en el puerto");
                        };
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        bootbox.alert(textStatus + ": " + XMLHttpRequest.responseText);
                    }
                });
            }
            else {
                row1.style.backgroundColor = "#efefef";
            }





        }


        function ValidacionCheck(row1) {

        }

        function validar() {
            if (document.getElementById("<%= h_oficio.ClientID %>").value.length > 0) {
                $('#<%= i_oficio.ClientID %>').text(document.getElementById("<%= h_oficio.ClientID %>").value);
            } else {
                $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                    "backdrop": "static",
                    "keyboard": false,
                    "show": true                     // ensure the modal is shown immediately
                });
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

            //validar();

            $("#myOK").click(function () {
                if (document.getElementById('nfolio').value.length == 0) {
                    bootbox.alert("Introduzca el número de oficio");
                }
                else {
                    var oficio = $('#nfolio').val();
                    oficio.replace(" ", "");
                    if (oficio.length > 0) {
                        $('#<%= i_oficio.ClientID %>').text(oficio);
                        document.getElementById("<%= h_oficio.ClientID %>").value = oficio;
                        $("#myModal").modal('hide');
                    }
                }
            });


            $("#myClose").click(function () {
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

                //validar();

                $("#myOK").click(function () {
                    if (document.getElementById('nfolio').value.length == 0) {
                        bootbox.alert("Introduzca el número de oficio");
                    }
                    else {
                        var oficio = $('#nfolio').val();
                        oficio.replace(" ", "");
                        if (oficio.length > 0) {
                            $('#<%= i_oficio.ClientID %>').text(oficio);
                                document.getElementById("<%= h_oficio.ClientID %>").value = oficio;
                                $("#myModal").modal('hide');
                            }
                        }
                    });

                    $("#myCancel").click(function () {
                        window.location = '<%= ResolveUrl("~/DAN/wfPrincipalDAN.aspx") %>';
                    });

                    $("#myClose").click(function () {
                        window.location = '<%= ResolveUrl("~/DAN/wfPrincipalDAN.aspx") %>';
                    });

                });
            }
    </script>
</asp:Content>

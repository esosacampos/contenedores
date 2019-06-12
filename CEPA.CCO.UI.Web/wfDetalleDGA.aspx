<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfDetalleDGA.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfDetalleDGA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bs-pagination {
            background-color: #1771F8;
        }

        .lead {
            margin-bottom: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Contenedores Solicitados DGA</h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
            <tr>
                <td style="font-weight: bold;">IMO
                </td>
                <td>
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="h_manifiesto" runat="server" />
                </td>
                <td style="font-weight: bold;"># de Viaje
                </td>
                <td>
                    <asp:Label ID="viaje" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Año de Manifiesto
                </td>
                <td class="style11">
                    <asp:Label ID="a_manif" runat="server"></asp:Label>
                </td>
                <td style="font-weight: bold;"># de Manifiesto
                </td>
                <td class="style11">
                    <asp:Label ID="manif" runat="server"></asp:Label>
                </td>

            </tr>
            <tr>
                <td style="font-weight: bold;">Nombre del Buque
                </td>
                <td>
                    <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
                </td>
                <td style="font-weight: bold;">Naviera</td>
                <td>
                    <asp:Label ID="d_cliente" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Código de Llegada
                </td>
                <td>
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
                <td style="font-weight: bold;">Fecha de llegada
                </td>
                <td>
                    <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr style="font-weight: bold;">
                <td colspan="2">No. de Solicitud
                </td>
                <td colspan="2">
                    <asp:Label ID="i_oficio" runat="server" Text="" Font-Bold="true"></asp:Label>
                    <asp:HiddenField ID="h_oficio" runat="server" />
                    <asp:HiddenField ID="IdRegP" runat="server" />
                    <asp:HiddenField ID="IdDocP" runat="server" />
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
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="b_rdt" HeaderText="ESTADO"></asp:BoundField>
                    <asp:BoundField DataField="c_pais_origen" HeaderText="ORIGEN"></asp:BoundField>
                    <asp:BoundField DataField="b_retenido" HeaderText="R. DAN"></asp:BoundField>
                    <asp:BoundField DataField="b_aduanas" HeaderText="S. DGA"></asp:BoundField>
                    <asp:TemplateField HeaderText="ESCANEAR">
                        <ItemTemplate>
                            <asp:HiddenField ID="hIdDeta" runat="server" Value='<%#Eval("IdDeta")%>' />
                            <asp:HiddenField ID="hCancelado" runat="server" Value='<%#Eval("b_cancelado")%>' />
                            <asp:HiddenField ID="hAduanas" runat="server" Value='<%#Eval("b_aduanas")%>' />
                            <asp:HiddenField ID="hConte" runat="server" />
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
    <asp:HiddenField ID="hConte" runat="server" />
    <br />
    <br />
    <div class="form-group" style="margin-left: 15px; margin-top: 5px;">
        <label for="texto" class="alert-danger lead">
            * Los contenedores marcados en <b>ROJO</b> son <b>CANCELADOS</b>.</label><br />
        <label for="texto" class="alert-success lead">
            * Los marcados en <b>VERDE</b> son solicitados por <b>PNC-DAN</b>.</label><br />
        <label for="texto" class="alert-warning lead">
            * Los marcados en <b>AMARILLO</b> son solicitados por <b>DGA</b>.</label><br />
        <label for="texto" class="alert-info lead">
            La columna <b>ESTADO</b> indica lo siguiente : Si el contenedor es <b>RD</b> es
            Retiro Directo y si es <b>TRA</b> es Trasbordo.</label>
    </div>
    <hr />
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Solicitar"
                    OnClick="btnCargar_Click" OnClientClick="return confirmaSave(this.id);" />
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

        function iniciaVariable() {
            $.session.set("_conteVar", 0);
            confirmed = false;
        }


        var confirmed = false;


        function confirmaSave(controlID) {
            if (confirmed) { return true; }

            var _cadena = parseInt($.session.get("_conteVar"));

            if (_cadena == 0) {
                bootbox.confirm("<b>CEPA - Contenedores</b> <br> No se encontraron contenedores seleccionados para escanear <br> ¿Desea Continuar?", function (result) {
                    if (result) {
                        if (controlID != null) {
                            var controlToClick = document.getElementById(controlID);
                            if (controlToClick != null) {
                                confirmed = true;
                                document.getElementById('<%=hConte.ClientID%>').value = _cadena;
                                controlToClick.click();
                                confirmed = false;
                            }
                        }
                    } else {
                        bootbox.alert("De no continuar con la selección y actualizar puede presionar F5")
                    }

                });
            }
            else if (_cadena > 0) {
                bootbox.confirm("<b>CEPA - Contenedores</b> <br> Se encontraron <b>" + _cadena + "</b> contenedor(es) seleccionado(s) para escanear <br> ¿Desea Continuar?", function (result) {
                    if (result) {
                        if (controlID != null) {
                            var controlToClick = document.getElementById(controlID);
                            if (controlToClick != null) {
                                confirmed = true;
                                document.getElementById('<%=hConte.ClientID%>').value = _cadena;
                                controlToClick.click();
                                confirmed = false;
                            }
                        }
                    } else {
                        bootbox.alert("De no continuar con la selección y actualizar puede presionar F5")
                    }

                });
            }

        return false;
    }

    function Check_Click(objRef) {
        var row1 = objRef.parentNode.parentNode.parentNode;

        if (objRef.checked) {
            row1.style.backgroundColor = "aqua";

            var _conte = parseInt($.session.get("_conteVar")) + 1;
            $.session.set("_conteVar", _conte);
        }
        else {
            row1.style.backgroundColor = "#efefef";

            var nValor = parseInt($.session.get("_conteVar")) - 1;
            $.session.set("_conteVar", nValor);

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

            iniciaVariable();
            //validar();

            <%--$("#myOK").click(function () {
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
            });--%>

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


                iniciaVariable();
                //validar();

                    <%--$("#myOK").click(function () {
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
                    });--%>

                });
            }
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfPrinciExport.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfPrinciExport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Listados De Exportación Pendientes De Validación</h2>
    <hr />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <br />
    <%--  <asp:Timer ID="Timer1" runat="server" Interval="30000" OnTick="Timer1_Tick">
    </asp:Timer>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="footable"
                data-filter="#filter" Style="max-width: 98%; margin-left: 15px;" DataKeyNames="IdReg">
                <Columns>
                    <asp:BoundField DataField="IdReg" HeaderText="ID"></asp:BoundField>                    
                    <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA"></asp:BoundField>
                    <asp:BoundField DataField="c_imo" HeaderText="COD. IMO"></asp:BoundField>                  
                    <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="f_llegada" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                        HeaderText="FECHA DE LLEGADA"></asp:BoundField>
                   <%-- <asp:TemplateField HeaderText="AUTORIZAR TODOS">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>      --%>            
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="Link2" runat="server" CssClass="btn btn-primary btn xs" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "wfAutoExport.aspx?IdReg={0}&IdDoc=" + DataBinder.Eval(Container.DataItem, "IdDoc")) %>'
                                Text="Validar"><span class="glyphicon glyphicon-pencil" style="cursor:pointer;"></span></asp:HyperLink>
                        </ItemTemplate>
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
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Autorizar"/>
                <%--<input type="button" id="btnCargar" class="btn btn-primary btn-lg" value="Retener Contenedores">--%>
            </li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar" />
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
                    <h4 class="modal-title">
                        Omitir Validación</h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">
                                Motivo:</label>
                            <textarea cols="6" id="txtJusti" class="form-control" rows="3" placeholder="Indicar el porque se omitirá la validación"></textarea>
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

        function ValidacionCheck(btn) {
            var contador = 0;
            var gridview = document.getElementById('<%=GridView1.ClientID %>');
            var row1 = gridview.parentNode.parentNode;
            for (var row = 1; row < gridview.rows.length; row++) {
                var checkbox = document.getElementById("ContentPlaceHolder1_GridView1_CheckBox1_" + (row - 1).toString());
                if (checkbox.checked) {                    
                    var IdREG = gridview.rows[row].cells[0].innerText;
                    var IdDoC = gridview.rows[row].cells[4].innerText;

                    if (btn == "Omitir") {
                        if (document.getElementById('txtJusti').value.length == 0) {
                            bootbox.alert("Introduzca la justificación");
                        }
                        else {
                            var params2 = new Object();                            
                            params2.IdReg = IdREG;                            
                            params2.Comentarios = $('#txtJusti').val();
                            params2 = JSON.stringify(params2);

                            $.ajax({
                                type: "POST",
                                url: "wfConsultaBuques.aspx/OmitirValid",
                                data: params2,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (result) {
                                    var boxi = bootbox.alert(result.d);
                                    bootbox.alert("Proceder a autorizar");
                                    $("#myModal").modal('hide');
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    checkbox.checked = false;
                                    gridview.rows[row].style.backgroundColor = "#efefef"
                                    $("#myModal").modal('hide');
                                    bootbox.alert(textStatus + ": " + XMLHttpRequest.responseText);
                                }
                            });

                        }
                    }
                    else if (btn == "Check") {
                        var params = new Object();                       
                        params.IdReg = IdREG;
                        params.IdDoc = IdDoC;
                        params = JSON.stringify(params);
                        contador = 1;

                        $.ajax({
                            type: "POST",
                            url: "wfPrinciExport.aspx/RevisarManif",
                            data: params,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                var resu = result.d;
                                if (resu == 0) {
                                    bootbox.confirm("¿Quiere omitir la validación?", function (resulta) {
                                        if (resulta == true) {
                                            $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                                                "backdrop": "static",
                                                "keyboard": false,
                                                "show": true                     // ensure the modal is shown immediately
                                            });
                                        }
                                        else {
                                            checkbox.checked = false;
                                            gridview.rows[row].style.backgroundColor = "#efefef"
                                            bootbox.alert("Debe seleccionar la opcion Validar para el PASO 3 de 3 y autorizar listados");
                                        }
                                    });
                                }
                                else if (resu == 2) {
                                    bootbox.confirm("¿El manifiesto #" + manifiesto + " no posee validación desea continuar?", function (resulta) {
                                        if (resulta == true) {
                                            $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                                                "backdrop": "static",
                                                "keyboard": false,
                                                "show": true                     // ensure the modal is shown immediately
                                            });
                                        }
                                        else {
                                            checkbox.checked = false;
                                            gridview.rows[row].style.backgroundColor = "#efefef";
                                            bootbox.alert("Debe seleccionar la opcion Validar para el PASO 3 de 3 y autorizar listados");
                                        }

                                    });
                                }                                
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                bootbox.alert(textStatus + ": " + XMLHttpRequest.responseText);
                            }
                        });
                    }
                    else if (btn == "Button") {
                        var params1 = new Object();
                        params1.IdReg = IdREG;
                        params1.cant = gridview.rows.length - 1;
                        params1 = JSON.stringify(params1);
                        contador = 1;
                        $.ajax({
                            type: "POST",
                            url: "wfPrinciExport.aspx/GenerarListadoExcel",
                            data: params1,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                bootbox.alert({
                                    message: result.d,
                                    callback: function () {
                                        location.reload();
                                    }
                                });
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                bootbox.alert({
                                    message: textStatus + ": " + XMLHttpRequest.responseText,
                                    callback: function () {
                                        $.unblockUI();
                                    }
                                });
                            }
                        });

                    }
                    break;
                }

                if (contador == 0 && btn == "Button") {
                    bootbox.alert("Debe seleccionar listado a autorizar");
                }
            }
        }

        function Salida() {
            $("#myModal").modal('hide');
            location.reload();
        }


        function Check_Click(objRef) {
            var row1 = objRef.parentNode.parentNode;

            if (objRef.checked) {
                row1.style.backgroundColor = "aqua";
            }
            else {
                row1.style.backgroundColor = "#efefef";
            }


            var btn = "Check";
            ValidacionCheck(btn);

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


            $("#myCancel").click(function () {
                e.preventDefault();
                Salida();
            });

            //            $("#ContentPlaceHolder1_btnCargar").click(function () {
            //                //e.preventDefault();
            //                var btn = "Button";
            //                ValidacionCheck(btn);
            //            });

            $("#ContentPlaceHolder1_btnCargar").click(function (e) {
                e.preventDefault();

                var btn = "Button";
                ValidacionCheck(btn);
            });

            $("#myClose").click(function (e) {
                e.preventDefault();
                Salida();
            });

        }



        function pageLoad() {
            $(document).ready(function () {
                $('#ContentPlaceHolder1_GridView1').footable();

                $("#myCancel").click(function (e) {
                    e.preventDefault();
                    bootbox.alert("No puede generar listado si no realiza la validación (PASO 3 de 4) o emite justificación de validación");
                    Salida();
                });

                $("#myClose").click(function (e) {
                    e.preventDefault();
                    Salida();
                });

                $("#ContentPlaceHolder1_btnCargar").click(function (e) {
                    e.preventDefault();
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
                    var btn = "Button";
                    ValidacionCheck(btn);
                });


                $("#myOK").click(function (e) {
                    e.preventDefault();
                    var btn = "Omitir";
                    ValidacionCheck(btn);
                });


            });


        }
    </script>
</asp:Content>

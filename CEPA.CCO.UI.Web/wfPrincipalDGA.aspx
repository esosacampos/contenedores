<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfPrincipalDGA.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfPrincipalDGA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .badgeV {
            display: inline-block;
            min-width: 10px;
            padding: 5px 7px;
            font-size: 12px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #0BF532;
            border-radius: 10px;
        }

        .badgeR {
            display: inline-block;
            min-width: 10px;
            padding: 5px 7px;
            font-size: 12px;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #F71E05;
            border-radius: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Lista de Buques Anunciados</h2>
    <hr />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="footable"
                DataKeyNames="n_manifiesto" OnRowDataBound="GridView1_RowDataBound"
                data-filter="#filter" ShowFooter="true"
                data-page-size="10" OnRowCreated="onRowCreate">
                <Columns>

                    <asp:BoundField DataField="n_manifiesto" HeaderText="# MANIFIESTO"></asp:BoundField>
                    <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="f_atraque" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                        HeaderText="FECHA DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="t_contenedores" HeaderText="T. CNTRS"></asp:BoundField>
                    <asp:TemplateField HeaderText="ESTATUS">
                        <ItemTemplate>
                            <asp:HiddenField ID="hEnvia" runat="server" Value='<%#Eval("b_solidga")%>' />
                            <asp:Label runat="server" ID="lblTdga" Text='<%#Eval("t_dga")%>' CssClass=""></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--                    <asp:BoundField DataField="t_dga" HeaderText="S. DGA"></asp:BoundField>--%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="hNaviera" runat="server" Value='<%#Eval("c_cliente")%>' />
                            <asp:HiddenField ID="hViaje" runat="server" Value='<%#Eval("c_voyage")%>' />
                            <asp:HiddenField ID="hDoc" runat="server" Value='<%#Eval("IdDoc")%>' />
                            <%--<asp:HyperLink ID="Link1" runat="server" CssClass="btn btn-primary btn xs" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"n_manifiesto", "/wfDetalleDGA.aspx?n_manifiesto={0}&c_cliente=" + DataBinder.Eval(Container.DataItem, "c_cliente")) %>' Text="Detallar"><span class="glyphicon glyphicon-pencil" style="cursor:pointer;"></span></asp:HyperLink>--%>
                            <button type="button" class="btn btn-info btn xs" onclick="return GetSelectedRow(this)" id="tooltop" data-toggle="tooltip" data-placement="top" data-original-title="Seleccionar contenedores">
                                <span class="glyphicon glyphicon-edit" style="cursor: pointer;"></span>
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <div class="form-group" style="margin-left: 15px;">
        <span class="badgeV"># </span>
        <label for="text" class="alert-success lead" style="margin-left: 5px">Manifiestos analizados / # Cntrs Solicitados </label>
        <br />
        <span class="badgeR"># </span>
        <label for="text" class="alert-danger lead" style="margin-left: 5px">Manifiestos sin analizar / # Cntrs Solicitados </label>
        <br />
    </div>
    <hr />
    <!-- Modal -->
    <div id="myModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="line-height: 10px; background-color: #d9edf7; border-color: #d9edf7; color: #31708f">
                    <button type="button" class="close" id="myClose" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 class="modal-title">SOLICITUD DE CONTENEDORES A ESCANEAR SEGUN DGA
                    </h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <input type="hidden" id="hManifiesto" />
                        <input type="hidden" id="hCliente" />
                        <input type="hidden" id="hVoyage" />
                        <input type="hidden" id="hBuque" />
                        <input type="hidden" id="hTotal" />
                        <input type="hidden" id="hDOCM" />
                        <div class="form-group" style="line-height: 1.4;">
                            <div class="lead alert-dismissible" role="alert">
                                <p>
                                    ¿Solicitará contenedores a escanear de este manifiesto?                                    
                                </p>
                            </div>
                        </div>
                        <div class="form-group" style="line-height: 1.4;">
                            <div class="alert alert-success" role="alert" style="margin-bottom: 5px;">
                                <p>
                                    <span style="font-size: 1.5em;"><b>SI</b></span> Esta opción lo trasladara a seleccionar los contenedores a escanear
                                </p>
                            </div>
                            <div class="alert alert-danger" role="alert">
                                <p>
                                    <span style="font-size: 1.5em;"><b>NO</b></span> Esta opción enviará alerta de manifiesto sin contenedores a escanear
                                </p>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" id="myNO">
                        No</button>
                    <button type="button" class="btn btn-success" id="myOK">
                        Si</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal HTML -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/jquery.dynDateTime.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">


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
                    //alert(testGrid.rows[1].cells[0].innerHTML);

                    //setTimeout(function () { location.reload(); }, 1000);
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



        function GetSelectedRow(lnk) {
            var row = lnk.parentNode.parentNode;
            var n_manifiesto = row.cells[0].innerText;
            var c_llegada = row.cells[1].innerHTML;
            var c_cliente = row.cells[7].children[0].value;
            var c_voyage = row.cells[7].children[1].value;
            var d_buque = row.cells[3].innerText;
            var total = row.cells[5].innerText;
            var idDoc = row.cells[7].children[2].value;

            modalValores(n_manifiesto, c_cliente, c_voyage, d_buque, total, idDoc);



            //validar(c_tarja, contenedor, c_llegada, f_tarja, n_manifiesto, v_peso);
            //var a_mani = $("input[name*='a_declaracion']").val();
            return false;
        }

        function modalValores(n_manifiesto, c_cliente, c_voyage, d_buque, total, idDoc) {

            $("#hManifiesto").val(n_manifiesto);
            $("#hCliente").val(c_cliente);
            $("#hVoyage").val(c_voyage);
            $("#hBuque").val(d_buque);
            $("#hTotal").val(total);
            $("#hDOCM").val(idDoc);

            $("#myModal").modal({                    // wire up the actual modal functionality and show the dialog
                "backdrop": "static",
                "keyboard": false,
                "show": true                     // ensure the modal is shown immediately
            });



        }

        function pageLoad() {
            $(document).ready(function () {
                $('#ContentPlaceHolder1_GridView1').footable();


                $("#myOK").click(function () {
                    $("#myModal").modal('hide');
                    var a = $("#hManifiesto").val();
                    var b = $("#hCliente").val();
                    window.location.href = "/wfDetalleDGA.aspx?n_manifiesto=" + a + "&c_cliente=" + b;

                });

                $("#myNO").click(function () {
                    $("#myModal").modal('hide');


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

                        var params = new Object();
                        params.d_buque = $("#hBuque").val();
                        params.c_cliente = "219";
                        params.nviaje = $("#hVoyage").val();
                        params.nmani = $("#hManifiesto").val();
                        params.c_naviera = $("#hCliente").val();
                        params.pTotal = $("#hTotal").val();
                        params.pIdDoc = $("#hDOCM").val();

                        params = JSON.stringify(params);


                        $.ajax({
                            async: true,
                            cache: false,
                            type: "POST",
                            url: "wfPrincipalDGA.aspx/EnviarCorreo",
                            data: params,
                            contentType: "application/json; charset=utf8",
                            dataType: "json",
                            success: function (response) {

                                var pagos = (typeof response.d) == "string" ? eval('(' + response.d + ')') : response.d;


                                if (pagos.indexOf("Error") != 0) {

                                    $.unblockUI();

                                    //bootbox.alert(pagos);

                                    bootbox.alert(pagos, function () { location.reload(); });

                                    //setTimeout(function () { location.reload(); }, 3000);
                                }
                                else {
                                    $.unblockUI();
                                    bootbox.alert(pagos);

                                }


                            },
                            failure: function (response) {
                                bootbox.alert(response.d);
                            },
                            error: function (response) {
                                bootbox.alert(response.d);
                            }
                        });

                    });

                    //$('#ContentPlacerHolder1_GridView1 [data-toggle="tooltip"]').tooltip();

                    //$("#ContentPlaceHolder1_GridView1 tbody tr td #tooltop").tooltip();

                    //$('#<%=GridView1.ClientID%> :button').tooltip();


                    // $('[data-toggle="tooltip"]').tooltip();

                });
            }
    </script>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfTracking.aspx.cs" Inherits="CEPA.CCO.UI.Web.CLIENTES.wfTracking" %>

<!DOCTYPE html>
<html lang="es">

<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>Tracking Contenedores</title>
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="vendor/metisMenu/metisMenu.min.css" rel="stylesheet">

    <link href="vendor/datatables-plugins/dataTables.bootstrap.css" rel="stylesheet">
    <!-- DataTables Responsive CSS -->
    <link href="vendor/datatables-responsive/dataTables.responsive.css" rel="stylesheet">

    <link href="dist/css/sb-admin-2.css" rel="stylesheet">
    <%--<link href="vendor/morrisjs/morris.css" rel="stylesheet">--%>
    <link href="vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
            EnablePageMethods="true">
            <%--EnablePartialRendering="true" AsyncPostBackTimeout="600"--%>
            <Scripts>
                <asp:ScriptReference Path="~/vendor/jquery/jquery.min.js" />
                <asp:ScriptReference Path="~/vendor/bootstrap/js/bootstrap.min.js" />
                <asp:ScriptReference Path="~/vendor/bootstrap/js/bootbox.min.js" />
                <asp:ScriptReference Path="~/vendor/metisMenu/metisMenu.min.js" />
                <asp:ScriptReference Path="~/vendor/raphael/raphael.min.js" />
                <asp:ScriptReference Path="~/vendor/morrisjs/morris.min.js" />
                <asp:ScriptReference Path="~/data_/morris-data.js" />
                <asp:ScriptReference Path="~/vendor/datatables/js/jquery.dataTables.min.js" />
                <asp:ScriptReference Path="~/vendor/datatables-plugins/dataTables.bootstrap.min.js" />
                <asp:ScriptReference Path="~/vendor/datatables-responsive/dataTables.responsive.js" />
                <asp:ScriptReference Path="~/dist/js/sb-admin-2.js" />
                <asp:ScriptReference Path="~/dist/js/jquery.blockui.js" />
            </Scripts>
        </asp:ScriptManager>
        <div id="wrapper">
            <!-- Navigation -->
            <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="wfTracking.aspx">CEPA - Tracking Contenedores</a>
                </div>

                <div class="navbar-default sidebar hidden-xs" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li>
                                <img src="vendor/bootstrap/logo/cepa_logo.png" alt="" style="margin-bottom: 15%; margin-left: 35%" class="img-responsive" />
                            </li>
                            <li>
                                <img src="vendor/bootstrap/logo/dga_lo.png" alt="" style="margin-bottom: 15%; margin-left: 35%" class="img-responsive" />
                            </li>
                            <li>
                                <br />
                            </li>
                            <li>
                                <img src="vendor/bootstrap/logo/pncsv.png" alt="" style="margin-bottom: 15%; margin-left: 30%" class="img-responsive" />
                            </li>
                            <li>
                                <br />
                            </li>
                            <li>
                                <img src="vendor/bootstrap/logo/manual_icons.png" alt="" style="margin-bottom: 15%; margin-left: 30%" class="img-responsive" />
                            </li>
                        </ul>
                    </div>
                    <!-- /.sidebar-collapse -->
                </div>
                <!-- /.navbar-static-side -->
            </nav>
            <div id="page-wrapper">
                <div class="row">
                    <div class="col-lg-12">
                        <h1 class="page-header">Retenciones PNC-DAN/UCC</h1>
                    </div>
                    <!-- /.col-lg-12 -->
                </div>
                <!-- /.row -->
                <div class="panel panel-default">
                    <div class="panel-heading">
                        Ingresar Información
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div role="form">
                                    <div class="form-inline">
                                        <div class="form-group">
                                            <label class="control-label" for="name"># Contenedor</label>
                                            <asp:TextBox required ID="txtBuscar" runat="server" CssClass="form-control" autocomplete="off" placeholder="# de contenedor sin guiones"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12">
                                <div role="form">
                                    <div class="form-inline">
                                        <div class="form-group">
                                            <label>Año Declaración</label>
                                            <asp:TextBox required ID="a_declaracion" size="10" runat="server" autocomplete="off" CssClass="form-control" placeholder="20XX"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label># Serie</label>
                                            <asp:TextBox required ID="n_serial" runat="server" size="5" autocomplete="off" CssClass="form-control" placeholder="X"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label># Correlativo</label>
                                            <asp:TextBox required ID="n_correlativo" runat="server" autocomplete="off" size="10" CssClass="form-control" placeholder="XXXXXX" Style="margin-right: 2px;"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label>
                                                <asp:CheckBox CssClass="success" runat="server" ID="radio3" />
                                                Sidunea World
                                            </label>
                                        </div>

                                        <div class="form-group">
                                            <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary" Text="Consultar" OnClick="btnBuscar_Click" />
                                            <asp:Button ID="btnClear" runat="server" class="btn btn-primary" Text="Nueva Consulta" OnClick="btnClear_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="panel panel-default">
                            <div class="panel-heading">
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="grvTracking" runat="server" AutoGenerateColumns="False" DataKeyNames="IdDeta" data-toggle="table" data-show-toggle="true" data-cookie="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hidden" runat="server" Value='<%#Eval("f_tarja")%>' />
                                                                <asp:HiddenField ID="hPeso" runat="server" Value='<%#Eval("v_peso")%>' />
                                                                <asp:HiddenField ID="hEstado" runat="server" Value='<%#Eval("b_cancelado")%>' />
                                                                <asp:HiddenField ID="hTarjas" runat="server" Value='<%#Eval("c_tarjasn")%>' />
                                                                <asp:HiddenField ID="HNTarjas" runat="server" Value='<%#Eval("con_tarjas")%>' />
                                                                <img alt="" src="CSS/Images/plus.gif" iddeta="<%# Eval(" IdDeta") %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="n_manifiesto" HeaderText="MANIFIESTO"></asp:BoundField>
                                                        <asp:BoundField DataField="c_llegada" HeaderText="LLEGADA"></asp:BoundField>
                                                        <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                                                        <asp:BoundField DataField="c_tarja" HeaderText="TARJA"></asp:BoundField>
                                                        <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                                                        <asp:BoundField DataField="b_estado" HeaderText="ESTADO"></asp:BoundField>
                                                        <asp:BoundField DataField="b_trafico" HeaderText="TRAFICO"></asp:BoundField>
                                                        <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA"></asp:BoundField>
                                                        <asp:BoundField DataField="d_buque" HeaderText="BUQUE"></asp:BoundField>
                                                        <asp:BoundField DataField="f_llegada" HeaderText="F/LLEGADA" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <button type="button" class="btn btn-primary btn xs" onclick="return GetSelectedRow(this)" id="tooltop" data-toggle="tooltip" data-placement="top" data-original-title="Consultar el estado para despachar su contenedor">
                                                                    <span class="glyphicon glyphicon-usd" style="cursor: pointer;"></span>
                                                                </button>
                                                                <tr id="rowF" iddeta="<%# Eval(" IdDeta") %>">
                                                                    <td colspan="100%">
                                                                        <div style="position: relative;">
                                                                            <asp:DetailsView ID="dtTracking" runat="server" AutoGenerateRows="False" DataKeyNames="IdDeta" CssClass="footable" CellPadding="0" GridLines="None" Width="100%">
                                                                                <Fields>
                                                                                    <asp:BoundField DataField="s_consignatario" HeaderText="Consignatario" ReadOnly="True"></asp:BoundField>
                                                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ReadOnly="True"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_rep_naviera" HeaderText="Anuncio Contenedor a Bordo por NAVIERA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_aut_aduana" HeaderText="Autorización Desestiba por ADUANA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_recepA" HeaderText="Recepcion de Contenedor por CEPA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_recep_patio" HeaderText="Ubicacion de Contenedor por CEPA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_cancelado" HeaderText="Cancelación del Contenedor por la NAVIERA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_cambio" HeaderText="Cambio de Condición por NAVIERA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_trans_aduana" HeaderText="Confirmación de Recepción para ADUANA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_reg_aduana" HeaderText="Transmisión Electrónica de la DM" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <%-- <asp:BoundField DataField="f_reg_selectivo" HeaderText="Presentación Física de la DM" ReadOnly="True"
                                                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">                                                        
                                                    </asp:BoundField>--%>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label Text="Presentación Física de la DM" ID="lblFS" runat="server"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label Text='<%# Bind("f_reg_selectivo") %>' ID="lblFechaP" runat="server"></asp:Label>
                                                                                            <asp:Label Text="" ID="lblSelectividad" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="f_lib_aduana" HeaderText="Liberación de la DM" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_ret_dan" HeaderText="Orden de Retención en Línea por DAN" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_tramite_dan" HeaderText="Cliente/Tramitador Confirma Retención por DAN" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_marchamo_dan" HeaderText="Corte de Marchamo por DAN" ReadOnly="True"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_deta_dan" HeaderText="Liberación en Línea por DAN" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_ret_ucc" HeaderText="Orden de Retención en Línea por UCC" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_tramite_ucc" HeaderText="Cliente/Tramitador Confirma Retención por UCC" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_marchamo_ucc" HeaderText="Corte de Marchamo por UCC" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_deta_ucc" HeaderText="Liberación en Línea por UCC" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_ret_mag" HeaderText="Retención en Línea por MAG" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_lib_mag" HeaderText="Liberación por MAG" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_salida_carga" HeaderText="Emisión Salida de Carga por CEPA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_caseta" HeaderText="Ingreso del Transporte al Puerto" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_solic_ingreso" HeaderText="Asignación de Turno para Cargar en Patio CEPA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_auto_patio" HeaderText="Orden para Cargar en Patio CEPA" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="f_puerta1" HeaderText="Salida del Transporte del Puerto" ReadOnly="True" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                    <asp:BoundField DataField="ubicacion" HeaderText="Ubicacion en Patio CEPA" ReadOnly="True"></asp:BoundField>
                                                                                    <asp:BoundField DataField="s_comentarios" HeaderText="Observaciones al Contenedor" ReadOnly="True"></asp:BoundField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label Text="Movimientos dentro del Recinto" ID="lblq" runat="server"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:GridView ID="grvProv" runat="server" AutoGenerateColumns="False" CssClass="footable" Style="margin-top: 5%; max-width: 98%; margin-left: 15px;">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField>
                                                                                                        <ItemTemplate>
                                                                                                            <img id="impProvi" alt="" src="CSS/Images/plus.gif" iddetap="<%# Eval(" IdDeta") %>" />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="2%" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                    </asp:BoundField>
                                                                                                    <%--<asp:BoundField DataField="Total" HeaderText="Total Movimientos">
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:BoundField>--%>
                                                                                                    <asp:TemplateField ItemStyle-CssClass="footable-visible footable-last-column" HeaderText="Total Movimientos" HeaderStyle-CssClass="footable-visible footable-last-column">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Total")%>'></asp:Label>
                                                                                                            <tr id="rowP" style="display: none; background-color: #777A79" iddetap="<%# Eval(" IdDeta") %>">
                                                                                                                <td colspan="100%">
                                                                                                                    <div style="position: relative;">
                                                                                                                        <asp:GridView ID="grvDetailProvi" runat="server" AutoGenerateColumns="False" DataKeyNames="c_llegada" CssClass="footable" Style="">
                                                                                                                            <Columns>
                                                                                                                                <asp:BoundField DataField="fecha_prv" HeaderText="Fecha de Movimiento" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="tipo" HeaderText="Tipo"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="motorista_prv" HeaderText="Motorista"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="transporte_prv" HeaderText="Transporte"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="placa_prv" HeaderText="Placa"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="chasis_prv" HeaderText="Chasis"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="fec_reserva" HeaderText="Solicitud Ing. Patio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                                                                <asp:BoundField DataField="fec_valida" HeaderText="Autorización Ing. Patio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                                                                                            </Columns>
                                                                                                                            <EmptyDataTemplate>
                                                                                                                                <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales
                                                                                                                            </EmptyDataTemplate>
                                                                                                                        </asp:GridView>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle CssClass="footable-visible footable-last-column" />
                                                                                                        <HeaderStyle CssClass="footable-visible footable-last-column" />
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                                <EmptyDataTemplate>
                                                                                                    <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales
                                                                                                </EmptyDataTemplate>
                                                                                            </asp:GridView>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Fields>
                                                                            </asp:DetailsView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmptyMessage" Text="" runat="server" />
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <%--   <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                                <asp:AsyncPostBackTrigger ControlID="btnClear" />--%>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <br />
                                    <br />
                                   <div class="alert alert-success" style="margin-bottom: 1%; font-weight: bold;" role="alert">* Se recuerda a todos los consignatarios que la DAN/UCC no realiza ningún cobro por las labores de inspección desarrolladas</div>
                                   <div class="alert alert-danger" style="margin-bottom: 1%; font-weight: bold;" role="alert">* Los contenedores marcados en ROJO son CANCELADOS</div>
                                   <div class="alert alert-info" style="margin-bottom: 1%; font-weight: bold;" role="alert">* Marcar la casilla Sidunea World para los contenedores manifestados en la nueva plataforma</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

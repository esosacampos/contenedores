<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfConsulta.aspx.cs" Inherits="CEPA.CCO.UI.Web.DANUCC.wfConsulta" %>

<!DOCTYPE html>
<html lang="es">

<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="Consula Contenedores Retenidos">
    <meta name="author" content="CEPA Puerto de Acajutla">
    <title>Consulta Retenciones DAN/UCC</title>

    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="vendor/metisMenu/metisMenu.min.css" rel="stylesheet">

    <link href="vendor/datatables-plugins/dataTables.bootstrap.css" rel="stylesheet">
    <!-- DataTables Responsive CSS -->
    <link href="vendor/datatables-responsive/dataTables.responsive.css" rel="stylesheet">

    <link href="dist/css/sb-admin-2.css" rel="stylesheet">
    <%--<link href="vendor/morrisjs/morris.css" rel="stylesheet">--%>
    <link href="vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <style type="text/css">
        #img1 {
            width: 80%;
            height: 30%;
            margin-left: 15%;
        }

        #img2 {
            width: 50%;
            height: 30%;
            margin-left: 25%;
        }

        #img3 {
            width: 50%;
            height: 30%;
            margin-left: 25%;
        }

        div.dataTables_wrapper {
            width: 100%;
            margin: 0 auto;
        }

        /*.navbar .btn-navbar {
            display: none;
        }

        .nav-off {
            display: block !important
        }*/

        @media (min-width: 1200px) {
            div.col-lg-12 {
                padding-right: 6px;
            }
        }



        @media screen and (max-width: 700px) {
            .navbar .btn-navbar {
                display: block;
            }

            .navbar .nav-collapse {
                display: none;
            }

            .nav-off {
                display: none !important
            }
        }

        @media only screen and (max-width: 600px) {
            #img1 {
                width: 20%;
                height: 10%;
            }

            #img2 {
                width: 20%;
                height: 10%;
            }

            #img3 {
                width: 20%;
                height: 10%;
            }

            .h1, h1 {
                font-size: 28px;
            }
        }
    </style>
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
                    <a class="navbar-brand" href="wfConsulta.aspx">CEPA Puerto de Acajutla</a>
                </div>
                <!-- /.navbar-header -->

                <div class="navbar-default sidebar hidden-xs" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li>
                                <img src="vendor/bootstrap/Images/cepa_logo.png" id="img1" class="img-responsive" />
                            </li>
                            <li>
                                <br />
                            </li>
                            <li>
                                <img src="vendor/bootstrap/Images/dan_logo.png" id="img2" alt="" class="img-responsive" />
                            </li>
                            <li>
                                <br />
                            </li>
                            <li>
                                <img src="vendor/bootstrap/Images/pncsv.jpg" alt="" id="img3" class="img-responsive" />
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
                                            <label>Año</label>
                                            <asp:TextBox ID="Datepicker" runat="server" class="form-control" autocomplete="off"
                                                placeholder="Ingrese año del manifiesto" Text=""></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label>Manifiesto</label>
                                            <asp:TextBox ID="txtMani" runat="server" class="form-control" autocomplete="off"
                                                placeholder="Ingrese # Manifiesto"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label>Contenedor</label>
                                            <asp:TextBox ID="txtContenedor" runat="server" CssClass="form-control" placeholder="Ingrese el # Contenedor" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary" Text="Consultar" OnClick="btnBuscar_Click" />
                                            <asp:Button ID="btnClear" runat="server" class="btn btn-primary" Text="Nueva Consulta" OnClick="btnClear_Click" />
                                        </div>
                                    </div>
                                    <div class="col-lg-10 alert alert-info" style="margin-top: 1%; width: 93%; margin-bottom: 1.5%;">
                                        <strong>ESTADO DEL CONTENEDOR:</strong><asp:Label ID="b_retenido" runat="server" Text="" Style="margin-left: 15px;"></asp:Label>
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
                                                <asp:GridView ID="grvRetenciones" runat="server" AutoGenerateColumns="False" DataKeyNames="n_contenedor"
                                                    CssClass="table table-striped table-bordered table-hover" Style="margin-top: 5%;" OnRowDataBound="grvRetenciones_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="n_folio" HeaderText="# OFICIO"></asp:BoundField>
                                                        <asp:BoundField DataField="TipoRe" HeaderText="TIPO"></asp:BoundField>
                                                        <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="TAMAÑO">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hEstado" runat="server" Value='<%#Eval("b_estado")%>' />
                                                                <asp:Label ID="lblTamaño" runat="server" Text='<%#Eval("c_tamaño")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="f_recepcion" HeaderText="F. RECEP. ARCO" HtmlEncode="false"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_recep_patio" HeaderText="F. RECEP. PATIO" HtmlEncode="false"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_retenido" HeaderText="F. RETENIDO" HtmlEncode="false"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_cancelado" HeaderText="F. CANCELADO" HtmlEncode="false"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_tramite" HeaderText="F. TRAMITE" HtmlEncode="false"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="f_ini_dan" HeaderText="F. REVISION" HtmlEncode="false"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="TipoRevision" HeaderText="T. REVISION"></asp:BoundField>
                                                        <asp:BoundField DataField="f_liberado" HeaderText="F. LIBERADO" HtmlEncode="false"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"></asp:BoundField>
                                                        <asp:BoundField DataField="c_correlativo" HeaderText="TOTAL HORAS"></asp:BoundField>
                                                        <asp:BoundField DataField="CalcDiasD" HeaderText="TIEMPO (Días)" DataFormatString="{0:F2}"
                                                            HtmlEncode="false"></asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <%--   <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                                <asp:AsyncPostBackTrigger ControlID="btnClear" />--%>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

        function OnBeginRequest(sender, args) {
            $.blockUI({
                message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/vendor/bootstrap/Images/progress_bar.gif") %>" />',
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

        }

        $(document).ready(function() {            
            $('#grvRetenciones').DataTable({   
                responsive:true,             
                paging:false,
                ordering:false,
                searching:false,
                info:false,
                autoWidth:false
            });

            
            
        });
        
    </script>


</body>
</html>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfAlertSalidas.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfAlertSalidas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 10px;
            text-align: center;
            border-left: none;
            border-top: none;
            border-right: none;
            border-bottom: none;
        }

        .footable.breakpoint > tbody > tr > td.footable-row-detail-cell {
            background: #fff;
            padding-left: 280px;
        }

        .footable > tbody > tr:hover:not(.footable-row-detail-cell) > td {
            color: #31708f;
            font-weight: bold;            
            background-color:#bce8f1;
        }

        .footable > thead > tr > th {
            border-bottom: 1px solid #dddddd;
            padding: 10px;
            text-align: left;
            font-size: 10.5px;
        }

        .footable > thead > tr > th, .footable > thead > tr > td {
            background-color: #1771F8;
            border: 1px solid #1771F8;
            color: #ffffff;
            border-top: none;
            border-left: none;
            font-weight: bold;
            text-align: center;
        }

        .footable > tbody > tr > td:last-child {
            border-right: 3px solid #1771F8;
        }

        .footable > tbody > tr > td {
            font-size: 12px;
            line-height: 20px;
        }

        th.footable-last-column {
            border-right: 3px solid #1771F8;
        }

        .form-group {
            margin-bottom: 10px;
            line-height: 10px;
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
    <h2>Seguimiento Despacho Contenedores En Puerto
    </h2>
    <br />

    <div class="row">
        <div class="col-lg-3" style="padding-right: 1px;">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <span>Fecha: </span>
                    </div>
                    <div class="form-group" style="width: 82%;">
                        <div class="input-group date" id="datetimepicker2" style="width: 100%;">
                            <%--<input type="text" class="form-control">--%>

                            <asp:TextBox ID="txtDOB" runat="server" class="form-control"></asp:TextBox>
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-9" style="padding-left: 5px;">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <span># Contenedor :
                        <%--<asp:Label ID="lblDoc" Text="04CCF000000000" runat="server"></asp:Label>--%></span>
                    </div>
                    <div class="form-group" style="width: 50%;">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" Width="100%" Placeholder="Ingrese # de Contenedor" MaxLength="11" autocomplete="off" Style="text-transform: uppercase"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-success" OnClick="btnBuscar_Click" OnClientClick="return confirmaSave(this.id);" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-top: 25px;">
        <div class="col-lg-12">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <div style="overflow-x: auto; width: 1000px">
                            <asp:GridView ID="grvDatos" CssClass="footable" runat="server" AutoGenerateColumns="False" DataKeyNames="Contenedor" OnRowDataBound="grvDatos_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="Manifiesto" HeaderText="# MANIFIESTO"></asp:BoundField>
                                    <asp:BoundField DataField="Navcorto" HeaderText="NAVIERA"></asp:BoundField>
                                    <asp:BoundField DataField="Salida" HeaderText="N° SALIDA"></asp:BoundField>
                                    <asp:BoundField DataField="Contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                                    <asp:BoundField DataField="Condicion" HeaderText="ESTADO"></asp:BoundField>
                                    <asp:TemplateField HeaderText="TAMAÑO">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTama" runat="server" Text='<%# Eval("Tamaño") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Tipo_Despacho" HeaderText="TIPO DESPACHO"></asp:BoundField>
                                    <asp:BoundField DataField="Despacho" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                        HeaderText="F/H SALIDA DE CARGA(A)"></asp:BoundField>
                                    <asp:BoundField DataField="Fec_Ing_Puerta1" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                        HeaderText="F/H ING. CASETA-PUERTA1"></asp:BoundField>
                                    <asp:BoundField DataField="Fec_Caseta_Ing" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                        HeaderText="F/H ASIGNA TURNO DE CARGA(B)"></asp:BoundField>
                                    <asp:BoundField DataField="Tiempoba" HeaderText="TIEMPO (B - A)"></asp:BoundField>
                                    <asp:BoundField DataField="Fec_Caseta_Sal" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                        HeaderText="F/H AUTORIZA DE CARGA(C)"></asp:BoundField>
                                    <asp:BoundField DataField="Tiempocb" HeaderText="TIEMPO (C - B)"></asp:BoundField>
                                    <asp:BoundField DataField="Fecha_Salidap1" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                        HeaderText="F/H PUERTA N°1 (D)"></asp:BoundField>
                                    <asp:BoundField DataField="Tiempoda" HeaderText="TIEMPO (D - A)"></asp:BoundField>
                                    <asp:BoundField DataField="Nom_Consigna" HeaderText="CONSIGNATARIO"></asp:BoundField>
                                    <asp:BoundField DataField="Chasis" HeaderText="CHASIS"></asp:BoundField>
                                    <asp:BoundField DataField="Transporte" HeaderText="TRANSPORTE"></asp:BoundField>
                                    <asp:BoundField DataField="Tarja" HeaderText="NOTA(S) DE TARJA"></asp:BoundField>
                                    <asp:BoundField DataField="Poliza" HeaderText="POLIZA(S)"></asp:BoundField>
                                    <asp:BoundField DataField="Peso_Entregado" HeaderText="PESO MERCAD ENTREGADA (Kgs)" DataFormatString = "{0:N2}"></asp:BoundField>
                                    <asp:BoundField DataField="Ubica" HeaderText="UBICACION DE DESPACHO"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />

    <br />
    <asp:Label ID="Label1" runat="server" Text="* NOTA: TODA CONSULTA REQUIERE INDICAR FECHA" CssClass="alert-danger" Style="margin-left: 15px; font-size: 15px;"></asp:Label>
    <br />
    <asp:Label ID="Label3" runat="server" Text="* ALERTA! Para obtener un informe de confirmación de salida de contenedores por puerta #1, debe indicar la fecha a consultar y luego dar clic en CONSULTAR o ingresar directamente fecha y contenedor a buscar" CssClass="alert-success" Style="margin-left: 15px; font-size: 15px;"></asp:Label>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>

    <script type="text/javascript">
        var d = new Date();
        var confirmed = false;
        function confirmaSave(controlID) {
            if (confirmed) { return true; }
            var dialog = bootbox.dialog({
                message: "<p class='text-center mb-0'><span class='glyphicon glyphicon-refresh glyphicon-refresh-animate'></span> Su solicitud esta siendo procesada..</p>",
                closeButton: false
            });

            dialog.init(function () {
                setTimeout(function () {
                    dialog.find('.bootbox-body').html('Su solicitud esta siendo procesada..');
                    dialog.modal('hide');
                }, 6000);
            });
            if (controlID != null) {
                var controlToClick = document.getElementById(controlID);
                if (controlToClick != null) {
                    confirmed = true;
                    controlToClick.click();
                    confirmed = false;
                }
            }

            return false;
        }

        $(document).ready(function () {

            $('#<%=grvDatos.ClientID %>').footable({
                breakpoints: {
                    phone: 480,
                    tablet: 1024
                }
            });

            $('#datetimepicker2').datetimepicker({
                defaultDate: new Date(),
                locale: 'es',
                format: "L",
                maxDate: new Date()
            });

        });

    </script>

</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfAlertEstadia.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfAlertEstadia" %>

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
            font-size:12px;
            line-height:20px;
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
    <h2>Existencia De Contenedores En Puerto
    </h2>
    <br />

    <div class="row">
        <div class="col-lg-12">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <span># Contenedor :
                        <%--<asp:Label ID="lblDoc" Text="04CCF000000000" runat="server"></asp:Label>--%></span>
                    </div>
                    <div class="form-group" style="width: 40%;">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" Width="100%" Placeholder="Ingrese # de Contenedor" MaxLength="11" autocomplete="off" Style="text-transform: uppercase"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-success" OnClick="btnBuscar_Click" OnClientClick="return confirmaSave(this.id);" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-top:25px;">
        <div class="col-lg-12">
            <div role="form">
                <div class="form-inline">
                    <div class="form-group">
                        <asp:GridView ID="grvDatos" CssClass="footable" runat="server" AutoGenerateColumns="False" DataKeyNames="Contenedor" OnRowDataBound="grvDatos_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="MANIFIESTO">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hCat" runat="server" Value='<%#Eval("Categoria")%>' />
                                        <asp:Label ID="lblManifiesto" runat="server" Text='<%# Bind("Manifiesto") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Corto" HeaderText="NAVIERA"></asp:BoundField>
                                <asp:BoundField DataField="Contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                                <asp:BoundField DataField="Tipo" HeaderText="TAMAÑO/TIPO"></asp:BoundField>
                                <asp:TemplateField HeaderText="CONDICION">
                                    <ItemTemplate>            
                                        <asp:HiddenField ID="hCon" runat="server" Value='<%#Eval("Condicion")%>' />
                                        <asp:Label ID="lblCondicion" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Ingreso" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                    HeaderText="F. INGRESO"></asp:BoundField>
                                <asp:TemplateField HeaderText="F. DE VACIADO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVacio" runat="server" Text='<%# Bind("Fec_vacio", "{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Estadia" HeaderText="ESTADIA"></asp:BoundField>
                                <asp:BoundField DataField="Sitio" HeaderText="UBICACION"></asp:BoundField>
                                <asp:BoundField DataField="Observa" HeaderText="OBSERVACIONES"></asp:BoundField>
                                <asp:BoundField DataField="Cliente" HeaderText="CONSIGNATARIO"></asp:BoundField>
                                <asp:BoundField DataField="Estb3" HeaderText="ESTADIA BODEGA #3"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
    <asp:Label ID="Label3" runat="server" Text="* ALERTA! Para obtener un inventario de todos los contenedores ubicados en puerto solo dar clic en CONSULTAR o ingresar directamente el contenedor a buscar" CssClass="alert-success" Style="margin-left: 15px;font-size:15px;"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">
    var confirmed = false;
    function confirmaSave(controlID) {
        if (confirmed) { return true; }          
                var dialog = bootbox.dialog({                                    
                                message: "<p class='text-center mb-0'><span class='glyphicon glyphicon-refresh glyphicon-refresh-animate'></span> Su solicitud esta siendo procesada..</p>",
                                closeButton: false
                            });
                   
                dialog.init(function(){
                        setTimeout(function(){
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
    </script>
</asp:Content>

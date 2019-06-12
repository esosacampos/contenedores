<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfTracking.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfTracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery-11.js"></script>
    <!-- include BlockUI -->
    <script type="text/javascript" src="Scripts/jquery.blockui.js"></script>
    <script type="text/javascript">
        function alertError() {
            $.blockUI({
                message: '<img src="CSS/Img/ajax-loader.gif" /><h1>Buscando...</h1>',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#1584ce',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });

            setTimeout($.unblockUI, 2000);
        }

        $(function () {

            $('#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv img').click(function () {
                var img = $(this)
                var idDeta = $(this).attr('iddetap');

                var tr = $('#ContentPlaceHolder1_grvTracking_dtTracking_0_grvProv tr[iddetap =' + idDeta + ']')
                tr.toggle();

                if (img.src == 'CSS/Images/plus.gif')
                    img.attr('src', 'CSS/Images/minus.gif');
                else
                    img.attr('src', 'CSS/Images/plus.gif');

                //                if (img.src.indexOf('plus.gif')) {
                //                    alert('Success');
                //                    img.src.replace(/plus.gif/, 'minus.gif');
                //                } else {
                //                    alert('Fail');
                //                }

                if (tr.is(':visible'))
                    img.attr('src', 'CSS/Images/minus.gif');
                else
                    img.attr('src', 'CSS/Images/plus.gif');

            });

            $('#<%=grvTracking.ClientID %> img').click(function () {

                var img = $(this)
                var idDeta = $(this).attr('iddeta');

                var tr = $('#<%=grvTracking.ClientID %> tr[iddeta =' + idDeta + ']')
                tr.toggle();


                if (tr.is(':visible'))
                    img.attr('src', 'CSS/Images/minus.gif');
                else
                    img.attr('src', 'CSS/Images/plus.gif');
            });

        });


       
    
    </script>
    <style type="text/css">
        .style1
        {
            width: 831px;
        }
        .style2
        {
            width: 402px;
        }
        .style4
        {
            width: 360px;
        }
        .style5
        {
            width: 102px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Contenedores Tracking</h2>
    <br />
    <table class="style1">
        <tr>
            <td class="style5">
                # Contenedor
            </td>
            <td class="style2">
                <asp:TextBox ID="txtBuscar" runat="server" Width="350px" placeholder="# de contenedor sin guiones"></asp:TextBox>
            </td>
            <td class="style4">
                <asp:Button ID="btnBuscar" runat="server" Text="Consultar" Height="35px" Font-Size="12px"
                    OnClick="btnBuscar_Click" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:GridView ID="grvTracking" runat="server" AutoGenerateColumns="False" Width="100%"
        DataKeyNames="IdDeta" Font-Names="Segoe UI Light" OnRowDataBound="grvTracking_RowDataBound"
        Style="margin-bottom: 0px">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <img alt="" src="CSS/Images/plus.gif" iddeta="<%# Eval("IdDeta") %>" />
                </ItemTemplate>
                <ItemStyle Width="2%" />
            </asp:TemplateField>
            <asp:BoundField DataField="IdDeta" HeaderText="Id">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="n_contenedor" HeaderText="# CONTENEDOR">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="d_buque" HeaderText="BUQUE">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="f_llegada" HeaderText="F. LLEGADA" HtmlEncode="false"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="c_llegada" HeaderText="" ReadOnly="True">
                <ItemStyle Width="0%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:BoundField DataField="n_contenedor" HeaderText="" ReadOnly="True">
                <ItemStyle Width="0%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
              <asp:BoundField DataField="c_naviera" HeaderText="" ReadOnly="True">
                <ItemStyle Width="0%" HorizontalAlign="Center" VerticalAlign="Middle" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <tr style="display: none;" iddeta="<%# Eval("IdDeta") %>">
                        <td colspan="100%">
                            <div style="position: relative; padding-left: 30px;">
                                <asp:DetailsView ID="dtTracking" runat="server" Width="98%" AutoGenerateRows="False"
                                    DataKeyNames="IdDeta" CellPadding="0" ForeColor="#333333" GridLines="None">
                                    <AlternatingRowStyle BackColor="White" />
                                    <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
                                    <Fields>
                                        <asp:BoundField DataField="f_rep_naviera" HeaderText="F. Reporto Naviera" ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                            <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_aut_aduana" HeaderText="F. Autorizado por ADUANA" ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                            <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_recep_patio" HeaderText="F. Recepción en Patio" ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                            <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_ret_dan" HeaderText="F. Retención DAN" ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                           <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_tramite_dan" HeaderText="F. Tramite DAN" ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                            <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_liberado_dan" HeaderText="F. Liberación DAN" ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                            <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_salida_carga" HeaderText="F. Salida de Carga" ReadOnly="True">
                                          <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_solic_ingreso" HeaderText="F. Solicitud de Ingreso a Patio"
                                            ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                           <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_auto_patio" HeaderText="F. Autorización en Patio" ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                           <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="f_puerta1" HeaderText="F. Confirmación Salida Puerta #1"
                                            ReadOnly="True"  DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                           <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ubicacion" HeaderText="Ubicacion en Patio" ReadOnly="True">
                                            <HeaderStyle Width="22%" />
                                            <ItemStyle Width="78%" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label Text="Provisionales" ID="lblq" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:GridView ID="grvProv" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    DataKeyNames="" Font-Names="Segoe UI Light" Style="margin-bottom: 0px">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <img alt="" src="CSS/Images/plus.gif" iddetap="<%# Eval("IdDeta") %>" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Total" HeaderText="Total">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <tr style="display: none;" iddetap="<%# Eval("IdDeta") %>">
                                                                    <td colspan="100%">
                                                                        <div style="position: relative; padding-left: 20px;">
                                                                            <asp:GridView ID="grvDetailProvi" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                DataKeyNames="c_llegada" Font-Names="Segoe UI Light" Font-Size="12px" Style="margin-bottom: 0px">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="fecha_prv" HeaderText="Fecha Provisional" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="tipo" HeaderText="Tipo">
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="motorista_prv" HeaderText="Motorista">
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="transporte_prv" HeaderText="Transporte">
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="placa_prv" HeaderText="Placa">
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="chasis_prv" HeaderText="Chasis">
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                     <asp:BoundField DataField="fec_reserva" HeaderText="Fecha Ing. Patio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                     <asp:BoundField DataField="fec_valida" HeaderText="Fecha Aut. Patio" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                    </asp:BoundField>
                                                                                </Columns>
                                                                                <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
                                                                                <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
                                                                                <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="16px" />
                                                                                <EmptyDataTemplate>
                                                                                    <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales</EmptyDataTemplate>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
                                                    <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
                                                    <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="16px" />
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmptyMessage" Text="" runat="server" />No posee provisionales</EmptyDataTemplate>
                                                </asp:GridView>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                </asp:DetailsView>
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
        <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
        <EmptyDataRowStyle ForeColor="Red" Font-Bold="true" Font-Size="16px" />
        <EmptyDataTemplate>
            <asp:Label ID="lblEmptyMessage" Text="" runat="server" /></EmptyDataTemplate>
    </asp:GridView>
    <br />
</asp:Content>

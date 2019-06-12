<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfPrincipalDANL.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfPrincipalDANL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.dynDateTime.min.js" type="text/javascript"></script>
    <%--<script src="../Scripts/calendar-en.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/calendar-es.js" type="text/javascript"></script>
    <link href="../CSS/calendar-blue.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $(".filtrar tr:has(td)").each(function () {
                var t = $(this).text().toLowerCase();
                $("<td class='indexColumn'></td>")
                .hide().text(t).appendTo(this);
            });
            //Agregar el comportamiento al texto (se selecciona por el ID) 
            $("#texto").keyup(function () {
                var s = $(this).val().toLowerCase().split(" ");
                $(".filtrar tr:hidden").show();
                $.each(s, function () {
                    $(".filtrar tr:visible .indexColumn:not(:contains('"
                     + this + "'))").parent().hide();
                });

                if (s == "") {
                    $(".filtrar tr:hidden").show()
                    $.each(s, function () {
                        $(".filtrar tr:visible .indexColumn:not(:contains('a'))").parent().hide();
                    });
                }
            });

        });
    </script>
    <style type="text/css">
        .style1
        {
            width: 1095px;
        }
        .style2
        {
            width: 238px;
        }
        .style3
        {
            width: 972px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Lista de Buques Anunciados</h2>
    <br />
    <br>
     <input id="texto" type="text" style="width: 95%;" placeholder="Ingrese búsqueda rápida" />
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" Interval="500000" OnTick="Timer1_Tick">
            </asp:Timer>
            <asp:Panel runat="server" ID="Panel1" Width="100%" ScrollBars="vertical" Height="300px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" class="filtrar"
                    DataKeyNames="IdReg" Font-Names="Segoe UI Light">
                    <Columns>
                        <asp:BoundField DataField="IdReg" HeaderText="ID">
                            <ItemStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA">
                            <ItemStyle Width="22%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="c_imo" HeaderText="COD. IMO">
                            <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA">
                            <ItemStyle Width="6%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE">
                            <ItemStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="f_llegada" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                            HeaderText="FECHA DE LLEGADA">
                            <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CantArchivo" HeaderText="# DE ARCHIVOS">
                            <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="Link1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "/DAN/wfDetalleDANL.aspx?IdReg={0}") %>'
                                    Text="Detallar" ForeColor="Blue"></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="f_retencion" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                            HeaderText="FECHA DE RETENCION">
                            <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                           <asp:BoundField DataField="f_liberacion" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                            HeaderText="FECHA DE LIBERACION">
                            <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Tiempo" HeaderText="TOTAL DIAS">
                            <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField> --%>
                    </Columns>
                    <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
                    <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </asp:Panel>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfDetalleDANL.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfDetalleDANL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<%--   <script type="text/javascript" src="http://code.jquery.com/ui/1.11.3/jquery-ui.js"></script>--%>
    <script src="../Scripts/jquery.dynDateTime.min.js" type="text/javascript"></script>
    <script src="../Scripts/calendar-en.min.js" type="text/javascript"></script>
    <script src="../Scripts/calendar-es.js" type="text/javascript"></script>
    <link href="../CSS/calendar-blue.css" rel="stylesheet" type="text/css" />

<%--  <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css">
  <script type="text/javascript" src="http://code.jquery.com/jquery-1.10.2.js"></script>
  <script type="text/javascript" src="http://code.jquery.com/ui/1.11.3/jquery-ui.js"></script>--%>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".Calender").dynDateTime({
                            showsTime: true,
                            ifFormat: "%d/%m/%Y %H:%M",
                            daFormat: "%A, %d de %B de %Y",
                            weekNumbers: false,
                            align: "BR",
                            electric: false,
                            singleClick: false,
                            displayArea: ".siblings('.dtcDisplayArea')",
                            button: ".next()"
                        });

//            $("[id$=datepicker]").datepicker({
//                showOn: "button",
//                buttonImageOnly: true,
//                buttonImage: "../CSS/Images/calender.png"
//            });

           


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
                        $(".filtrar tr:visible .indexColumn:not(:contains('u'))").parent().hide();
                    });
                }
            });

        });        

    </script>
    <style type="text/css">
        .style1
        {
            width: 1095px;
            height: 46px;
        }
        .style10
        {
            height: 23px;
            width: 154px;
            font-weight: bold;
        }
        .style11
        {
            height: 23px;
            width: 956px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Liberar Contenedores</h2>
    <br />
    <table class="style1">
        <tr>
            <td class="style10">
                IMO
            </td>
            <td class="style11">
                <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">
                # de Viaje
            </td>
            <td class="style11">
                <asp:Label ID="viaje" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">
                # de Manifiesto
            </td>
            <td class="style11">
                <asp:Label ID="manif" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">
                Nombre del Buque
            </td>
            <td class="style11">
                <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">
                Código de Llegada
            </td>
            <td class="style11">
                <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">
                Fecha de llegada
            </td>
            <td class="style11">
                <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <input id="texto" type="text" style="width: 95%;" placeholder="Ingrese búsqueda rápida" />
    <br />
    <br />
    <asp:Panel runat="server" ID="Panel1" Width="100%" ScrollBars="vertical" Height="300px">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" class="filtrar"
            DataKeyNames="f_recepcion" Font-Names="Segoe UI Light" Font-Size="16px">
            <Columns>
                <asp:BoundField DataField="IdDeta" HeaderText="ID">
                    <ItemStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="n_folio" HeaderText="OFICIO">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR">
                    <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <%--<asp:TemplateField HeaderText="AGENTE DAN">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlRevision" runat="server" Width="250px" Height="39px">
                        </asp:DropDownList>
                    </ItemTemplate>
                    <ItemStyle Width="8%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>   --%>
                <asp:TemplateField HeaderText="F. INICIO TRAMITE">
                   <ItemTemplate>
                        <asp:TextBox ID="txtDOB" runat="server" class="Calender"></asp:TextBox>
                        <img src="../CSS/Images/calender.png" alt="Indique Fecha" />
                        <%--<input type="text" id="datepicker">--%>
                      <%--  <asp:TextBox ID="datepicker" runat="server" ReadOnly = "true"></asp:TextBox>--%>
                    </ItemTemplate>
                    <ItemStyle Width="12%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="LIBERAR">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:BoundField DataField="f_recepcion" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                    ItemStyle-Width="1px" Visible="false">
                    <ItemStyle Width="1px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>
            <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
            <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
    <br />
    <div style="float: left; width: 45%">
        <asp:Button ID="btnCargar" runat="server" Text="Liberar" OnClick="btnCargar_Click" />
        <%--<input type="button" value="Saludar" id="btSaludar" onclick="javascript:if(Confirmar())" runat="server" />--%>
    </div>
    <div style="float: right; width: 45%">
        <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" OnClick="btnRegresar_Click" />
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfConsultaTrans.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsultaTrans" %>

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
            width: 164px;
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
        Retener Contenedores</h2>
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
        <tr>
            <td class="style10">
                Total C. Importados
            </td>
            <td class="style11">
                <asp:Label ID="tot_imp" runat="server" Text="" Style="color: red; font-weight: bold;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">
                Total C. Transmitidos
            </td>
            <td class="style11">
                <asp:Label ID="tot_trans" runat="server" Text="" Style="color: red; font-weight: bold;"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <input id="texto" type="text" style="width: 95%;" placeholder="Ingrese búsqueda rápida" />
    <br />
    <br />
    <asp:Panel runat="server" ID="Panel1" Width="100%" ScrollBars="vertical" Height="300px">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
            class="filtrar" DataKeyNames="c_correlativo" Font-Names="Segoe UI Light" Font-Size="16px"
            OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:BoundField DataField="c_correlativo" HeaderText="CORRELATIVO">
                    <ItemStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                    <ItemStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                    <asp:BoundField DataField="f_recep" HeaderText="FECHA DE RECEP. PATIO">
                    <ItemStyle Width="8%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="b_trans" HeaderText="TRANSMITIDO">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="f_trans" HeaderText="FECHA DE TRANSMISION">
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>
            <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
            <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
    <br />
    <br />
    <div style="float: left; width: 45%">
        <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" OnClick="btnRegresar_Click" />
    </div>
    <div style="float: right; width: 45%">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

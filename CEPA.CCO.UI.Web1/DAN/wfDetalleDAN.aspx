<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfDetalleDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfDetalleDAN" %>

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

     function abrirModal(pagina, idreg) {
         var vReturnValue;
         vReturnValue = window.showModalDialog(pagina, "", "dialogHeight: 300px; dialogWidth: 500px; edge: Raised; center: Yes; help: No; resizable: No; status: No; ");

         if (vReturnValue != null && vReturnValue == true) {
             //                            __doPostBack('CargarProceso', '');
             // window.opener.location.href = window.opener.location.href;
             window.location.reload(true);
             return vReturnValue
         }
         else {
             return false;
         }
     }

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
                No. de Oficio
            </td>
            <td class="style11">
                <asp:LinkButton ID="n_oficio" runat="server" OnClick="n_oficio_Click" ToolTip="Click para modificar oficio"></asp:LinkButton>
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
            class="filtrar" DataKeyNames="IdDeta" Font-Names="Segoe UI Light" Font-Size="16px"
            OnRowDataBound="GridView1_RowDataBound">
            <Columns>
                <asp:BoundField DataField="IdDeta" HeaderText="ID">
                    <ItemStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="c_correlativo" HeaderText="CORRELATIVO">
                    <ItemStyle Width="4%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR">
                    <ItemStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO">
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="b_rdt" HeaderText="ESTADO">
                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="c_pais_origen" HeaderText="ORIGEN">
                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="TIPO REVISION">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlRevision" runat="server" Width="200px" Height="39px">
                        </asp:DropDownList>
                    </ItemTemplate>
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ESCANER">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RETENER">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:BoundField DataField="n_BL" HeaderText="">
                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="s_consignatario" HeaderText="">
                    <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>
            <AlternatingRowStyle BackColor="#E5E5E5" ForeColor="#284775" />
            <HeaderStyle BackColor="#0066CC" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
    <br />
    <br />
    <span>La columna <b>ESTADO</b> indica lo siguiente : Si el contenedor es <b>RD</b> es Retiro Directo y si es <b>T</b> es Trasbordo. </span>
    <br />
    <br />
    <div style="float: left; width: 45%">
        <asp:Button ID="btnCargar" runat="server" Text="Retener" OnClick="btnCargar_Click" />
        <%--<input type="button" value="Saludar" id="btSaludar" onclick="javascript:if(Confirmar())" runat="server" />--%>
    </div>
    <div style="float: right; width: 45%">
        <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" OnClick="btnRegresar_Click" />
    </div>
</asp:Content>

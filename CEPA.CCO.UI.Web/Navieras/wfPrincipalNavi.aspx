<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfPrincipalNavi.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfPrincipalNavi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Lista de Buques Anunciados Importación</h2>
    <hr />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese nombre de buque">
    </div>

    <!-- Tabla -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:Timer ID="Timer1" runat="server" Interval="4000" OnTick="Timer1_Tick">
                </asp:Timer>--%>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="footable"
                DataKeyNames="c_buque" OnRowDataBound="GridView1_RowDataBound" data-filter="#filter" ShowFooter="true" data-page-size="10" OnRowCreated="onRowCreate">
                <Columns>
                    <asp:BoundField DataField="c_imo" HeaderText="COD. IMO"></asp:BoundField>
                    <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="f_llegada" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                        HeaderText="FECHA DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="CantArchivo" HeaderText="# ARCH. IMPORTACIÓN"></asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <th colspan="2">IMPORTACIÓN</th>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <td>
                                <asp:HyperLink ID="Link" runat="server" Style="margin-right: 2px;" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"c_buque", "/Navieras/wfSustituirArch.aspx?c_buque={0}&c_llegada=" + DataBinder.Eval(Container.DataItem, "c_llegada")) %>'
                                    Text="">
                            <span class="glyphicon glyphicon-export" style="font-size: 17px; color: #1771f8; cursor: pointer;">
                                Sustituir
                                </span>
                                </asp:HyperLink></td>
                            <td>
                                <asp:HyperLink ID="Link1" runat="server" Style="margin-right: 2px;" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"c_buque", "/Navieras/wfCargar.aspx?c_buque={0}&c_llegada=" + DataBinder.Eval(Container.DataItem, "c_llegada")) %>'
                                    Text="">
                            <span class="glyphicon glyphicon-import" style="font-size: 17px; color: #1771f8; cursor: pointer;">
                                    Agregar
                            </span>
                                </asp:HyperLink></td>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <%-- <asp:BoundField DataField="CantRemo" HeaderText="# ARCH. IMP. REESTIBA"></asp:BoundField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <th colspan="2">I. REESTIBA</th>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <td>
                                <asp:HyperLink ID="Link" runat="server" Style="margin-right: 2px;" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"c_buque", "/Navieras/wfSustituirArch.aspx?c_buque={0}&c_llegada=" + DataBinder.Eval(Container.DataItem, "c_llegada")) %>'
                                    Text="">
                            <span class="glyphicon glyphicon-export" style="font-size: 17px; color: #1771f8; cursor: pointer;">
                                Sustituir
                                </span>
                                </asp:HyperLink></td>
                            <td>
                                <asp:HyperLink ID="Link1" runat="server" Style="margin-right: 2px;" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"c_buque", "/Navieras/wfCargarReestiba.aspx?c_buque={0}&c_llegada=" + DataBinder.Eval(Container.DataItem, "c_llegada")) %>'
                                    Text="">
                            <span class="glyphicon glyphicon-import" style="font-size: 17px; color: #1771f8; cursor: pointer;">
                                    Agregar
                            </span>
                                </asp:HyperLink></td>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_GridView1').footable();
        });
    </script>
</asp:Content>

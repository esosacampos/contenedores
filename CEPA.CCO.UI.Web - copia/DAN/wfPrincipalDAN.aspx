<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfPrincipalDAN.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfPrincipalDAN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Lista de Buques Anunciados</h2>
    <hr />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="footable"
                DataKeyNames="IdReg" OnRowDataBound="GridView1_RowDataBound"
                Style="max-width: 98%; margin-left: 15px;" data-filter="#filter" ShowFooter="true"
                data-page-size="5" OnRowCreated="onRowCreate">
                <Columns>
                    <asp:BoundField DataField="IdReg" HeaderText="ID"></asp:BoundField>
                    <asp:BoundField DataField="d_cliente" HeaderText="NAVIERA"></asp:BoundField>
                    <asp:BoundField DataField="c_imo" HeaderText="COD. IMO"></asp:BoundField>
                    <asp:BoundField DataField="c_llegada" HeaderText="COD. DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="d_buque" HeaderText="NOMBRE DEL BUQUE"></asp:BoundField>
                    <asp:BoundField DataField="f_llegada" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                        HeaderText="FECHA DE LLEGADA"></asp:BoundField>
                    <asp:BoundField DataField="CantArchivo" HeaderText="# DE ARCHIVOS"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="Link1" runat="server" CssClass="btn btn-primary btn xs" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"IdReg", "/DAN/wfDetalleDAN.aspx?IdReg={0}") %>' Text="Detallar"><span class="glyphicon glyphicon-pencil" style="cursor:pointer;"></span></asp:HyperLink>
                          <%--  <button class="btn btn-primary btn-xs" data-title="Oficio" data-toggle="modal" data-target="#myModal" id="btnModal">
                                <span class="glyphicon glyphicon-pencil"></span>--%>
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script src="<%= ResolveUrl("~/Scripts/jquery.dynDateTime.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_GridView1').footable();


            function abrirModal(pagina) {
                $('#myModal').modal('show');
            }

            $("#myOK").click(function () {
                if (document.getElementById('nfolio').value.length == 0) {
                    alert("Introduzca el número de oficio");
                }
                else {
                    var oficio = $('#nfolio').val();          

                }
            });
        });
    </script>
</asp:Content>

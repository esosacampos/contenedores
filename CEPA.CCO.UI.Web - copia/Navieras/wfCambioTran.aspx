﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfCambioTran.aspx.cs" Inherits="CEPA.CCO.UI.Web.Navieras.wfCambioTran" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Cancelar Contenedores</h2>
    <hr />
    <div class="table-responsive">
        <table class="table">
            <tr>
                <td>IMO
                </td>
                <td>
                    <asp:Label ID="c_imo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Nombre del Buque
                </td>
                <td>
                    <asp:Label ID="d_buque" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Código de Llegada
                </td>
                <td class="style11">
                    <asp:Label ID="c_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de llegada
                </td>
                <td>
                    <asp:HiddenField ID="n_manif" runat="server" />
                    <asp:HiddenField ID="c_viaje" runat="server" />
                    <asp:HiddenField ID="c_prefijo" runat="server" />
                    <asp:Label ID="f_llegada" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <hr />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida">
    </div>
    <hr />
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="c_correlativo"
                data-filter="#filter" Style="max-width: 98%; margin-left: 15px;" ShowFooter="true"
                data-page-size="5" CssClass="footable" OnRowCreated="onRowCreate">
                <Columns>
                    <asp:BoundField DataField="c_correlativo" HeaderText="No."></asp:BoundField>
                    <asp:BoundField DataField="n_contenedor" HeaderText="CONTENEDOR"></asp:BoundField>
                    <asp:BoundField DataField="c_tamaño" HeaderText="TAMAÑO"></asp:BoundField>
                    <asp:BoundField DataField="b_retenido" HeaderText="RETENCION"></asp:BoundField>
                    <asp:BoundField DataField="b_estado" HeaderText="TIPO DESPACHO"></asp:BoundField>
                    <asp:TemplateField HeaderText="CAMBIAR">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="OBSERVACIONES">
                        <ItemTemplate>                                                      
                            <asp:HiddenField ID="hIdDeta" runat="server" Value='<%#Eval("IdDeta")%>'></asp:HiddenField>
                            <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" placeholder="Justifique cambio de condición requerido" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:Label ID="Label1" runat="server" Text="* La condición cambiara a la inversa de la indicada en tipo de despacho."
        Style="color: #FF0000; font-weight: 700"></asp:Label>
    <br />
    <br />
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:Button ID="btnCargar" runat="server" Text="Guardar" class="btn btn-primary btn-lg"
                    OnClick="btnCargar_Click" />
            </li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" Text="<< Regresar" class="btn btn-primary btn-lg"
                    OnClick="btnRegresar_Click" />
            </li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">
        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";
            }
            else {
                row.style.backgroundColor = "#efefef";
            }
        }

        function convertToPagination(obj) {
            var liststring = '<ul class="pagination">';
            $(obj).find("tbody tr").each(function () {
                $(this).children().map(function () {
                    liststring = liststring + "<li>" + $(this).html() + "</li>";
                });
            });
            liststring = liststring + "</ul>";
            var list = $(liststring);
            list.find('span').parent().addClass('active');
            $(obj).replaceWith(list);
        }

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

        function OnBeginRequest(sender, args) {
            $.blockUI({
                message: '<img src="<%= ResolveClientUrl("~/CSS/Img/ajax-loader.gif") %>" /><h1>Procesando...</h1>',
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#1771F8',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                }
            });
        }
        function endRequest(sender, args) {

            $.unblockUI();

            $('#ContentPlaceHolder1_GridView1').footable();

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');

        }



        $(document).ready(function () {
            $('#ContentPlaceHolder1_GridView1').footable();

            $('#ContentPlaceHolder1_GridView1 tbody').trigger('footable_redraw');
        });




    </script>
</asp:Content>

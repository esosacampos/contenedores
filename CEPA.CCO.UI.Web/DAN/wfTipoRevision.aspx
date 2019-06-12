<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfTipoRevision.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfTipoRevision" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <h2>
        Tipo de Revisión</h2>
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="IdRevision"
                CssClass="footable" Style="margin-top: 5%; max-width: 98%;" OnRowDataBound="GridView2_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lkButton" runat="server" Font-Size="Medium">Editar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdRevision" HeaderText="ID" SortExpression="IdRevision">
                    </asp:BoundField>
                    <asp:BoundField DataField="Clave" HeaderText="CLAVE REVISION" SortExpression="Clave"
                        ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="TIPO" SortExpression="Tipo" ReadOnly="True">
                    </asp:BoundField>
                    <asp:BoundField DataField="Habilitado" HeaderText="ESTADO" SortExpression="Habilitado"
                        ReadOnly="True"></asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    No se encontraron registros</EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Button2" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:Button ID="Button2" runat="server" class="btn btn-primary" Text="Agregar Revision" />
    <br />
    <!-- Modal HTML -->
    <div id="myInsert" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" id="myCloseI" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 class="modal-title">
                        Registrar Revision</h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">
                                Clave:</label>
                            <asp:TextBox ID="txtClaveI" runat="server" class="form-control" placeholder="Ingrese clave"
                                Style="text-transform: uppercase;"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">
                                Tipo Revisión:</label>
                            <asp:TextBox ID="txtTipoI" runat="server" class="form-control" placeholder="Ingrese Tipo"
                                Style="text-transform: uppercase;"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" id="myCancelI">
                        Cancelar</button>
                    <button type="button" class="btn btn-primary" id="myOKI">
                        Registrar</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Modificar HTML -->
    <div id="myModify" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" id="myCloseM" data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h4 class="modal-title">
                        Modificar Revision</h4>
                </div>
                <div class="modal-body">
                    <div role="form">
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">
                                Clave:</label>
                            <asp:TextBox ID="txtClaveM" runat="server" class="form-control" placeholder="Ingrese clave"
                                Style="text-transform: uppercase;"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">
                                Tipo Revisión:</label>
                            <asp:TextBox ID="txtTipoM" runat="server" class="form-control" placeholder="Ingrese Tipo"
                                Style="text-transform: uppercase;"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="recipient-name" class="control-label">
                                Estado:</label>
                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" id="myCancelM">
                        Cancelar</button>
                    <button type="button" class="btn btn-primary" id="myOkM">
                        Modificar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        Page.add_endRequest(endRequest);

        function OnBeginRequest(sender, args) {
            $.blockUI({
                message: '<h1>Procesando</h1><img src="<%= ResolveClientUrl("~/CSS/Img/progress_bar.gif") %>" />',
                 css: {
                     border: 'none',
                     padding: '15px',
                     backgroundColor: '#424242',
                     '-webkit-border-radius': '10px',
                     '-moz-border-radius': '10px',
                     opacity: .5,
                     color: '#fff'
                 }
             });

            $('#ContentPlaceHolder1_GridView2').footable();
        }
        function endRequest(sender, args) {

            $.unblockUI();

            $('#ContentPlaceHolder1_GridView2').footable();

            $('#ContentPlaceHolder1_GridView2 tbody').trigger('footable_redraw');

            $("#myOKI").click(function (e) {
                e.preventDefault();

                var params = new Object();
                params.pClave = $('#ContentPlaceHolder1_txtClaveI').val();
                params.pTipo = $('#<%= txtTipoI.ClientID %>').val();
                params = JSON.stringify(params);

                $.ajax({
                    type: "POST",
                    url: "wfTipoRevision.aspx/SaveRevision",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        bootbox.alert(result.d);
                        event.preventDefault();
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        bootbox.alert(textStatus + ": " + XMLHttpRequest.responseText);
                    }
                });

                //location.reload();
                $("#myInsert").modal('hide');
                location.reload();
            });    

        }

        function LoadCiudades(result) {
            //quito los options que pudiera tener previamente el combo
            $("#<%=DropDownList1.ClientID%>").html("");

            $("#<%=DropDownList1.ClientID%>").append($("<option></option>").attr("value", "0").text("-- Seleccionar Estado --"));

            //recorro cada item que devuelve el servicio web y lo aÃ±ado como un opcion
            $.each(result.d, function () {
                $("#<%=DropDownList1.ClientID%>").append($("<option></option>").attr("value", this.IdProceso).text(this.Descripcion))
            });
        }

        function LoadTipos(result) {

            $.each(result.d, function () {
                $('#ContentPlaceHolder1_txtClaveM').val(this.Clave);
                $('#<%= txtTipoM.ClientID %>').val(this.Tipo);
                if (this.Habilitado == "Activo")
                    $("#<%=DropDownList1.ClientID%>").val(1);
                else
                    $("#<%=DropDownList1.ClientID%>").val(2);
            });
        }

        function CargarModi(IdEditar) {


            $.ajax({
                type: "POST",
                url: "wfTipoRevision.aspx/GetEstados",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCiudades,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    bootbox.alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

            // armo el objeto que servira de parametro, para ello utilizo una libreria de JSON
            //este parametro mapeara con el definido en el web service
            var params = new Object();
            params.Id = IdEditar;
            params = JSON.stringify(params);

            $.ajax({
                type: "POST",
                url: "wfTipoRevision.aspx/GetRevision",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadTipos,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    bootbox.alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

            $("#myModify").modal({                    // wire up the actual modal functionality and show the dialog
                "backdrop": "static",
                "keyboard": false,
                "show": true                     // ensure the modal is shown immediately
            });

        }
              

        function pageLoad() {
            $(document).ready(function () {

                $('#ContentPlaceHolder1_GridView2').footable();

                $("#ContentPlaceHolder1_Button2").click(function () {
                    $("#myInsert").modal({                    // wire up the actual modal functionality and show the dialog
                        "backdrop": "static",
                        "keyboard": false,
                        "show": true                     // ensure the modal is shown immediately
                    });
                });

               

                $("#myCancelI").click(function () {
                    location.reload();
                });

                $("#myCancelM").click(function () {
                    location.reload();
                });

            });
        }              
</script>
</asp:Content>

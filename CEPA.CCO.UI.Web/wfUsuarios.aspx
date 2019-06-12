<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfUsuarios.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bs-pagination
        {
            background-color: #1771F8;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Usuarios del Sistema</h2>
    <hr />
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="c_usuario"
                OnSelectedIndexChanged="GridView2_SelectedIndexChanged" CssClass="footable" Style="margin-top: 5%;
                max-width: 98%;" OnRowDataBound="GridView2_RowDataBound">
                <Columns>
                    <asp:CommandField SelectText="Seleccionar" ShowSelectButton="True"></asp:CommandField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lkButton" runat="server" Font-Size="Medium">Editar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="c_usuario" HeaderText="USUARIO" SortExpression="c_usuario">
                    </asp:BoundField>
                    <asp:BoundField DataField="d_usuario" HeaderText="NOMBRE DEL USUARIO" SortExpression="d_usuario"
                        ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="d_naviera" HeaderText="CLIENTE" SortExpression="d_naviera"
                        ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="Habilitado" HeaderText="ESTADO" SortExpression="Habilitado"
                        ReadOnly="True"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="Button2" runat="server" class="btn btn-primary" Text="Agregar Usuario" />
            <br />
            <h3>
                Perfiles Asignados</h3>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="IdPerfil"
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" CssClass="footable" Style="margin-top: 5%;
                max-width: 98%;">
                <Columns>
                    <asp:CommandField SelectText="Seleccionar" ShowSelectButton="True"></asp:CommandField>
                    <asp:BoundField DataField="IdPerfil" HeaderText="# DE PERFIL" SortExpression="IdPerfil">
                    </asp:BoundField>
                    <asp:BoundField DataField="NombrePerfil" HeaderText="NOMBRE DEL PERFIL" SortExpression="NombrePerfil"
                        ReadOnly="True"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="Button1" runat="server" class="btn btn-primary" Text="Menu Completo"
                OnClick="Button1_Click" />
            <h3>
                Opciones del Perfil</h3>
            <div style="margin: 10px 250px;">
                <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" ShowLines="true" Font-Size="12px"
                    Width="50%">
                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                    <NodeStyle Font-Names="Verdana" ForeColor="Black" HorizontalPadding="7px" NodeSpacing="0px"
                        VerticalPadding="0px" />
                    <ParentNodeStyle Font-Bold="False" />
                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                        VerticalPadding="0px" />
                </asp:TreeView>
            </div>
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
        </ContentTemplate>
    </asp:UpdatePanel>
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

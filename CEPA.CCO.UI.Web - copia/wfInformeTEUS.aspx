<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfInformeTEUS.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfInformeTEUS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        INFORME RESUMEN DE LINEAS Y CONTENEDORES MOVILIZADOS POR UNIDADES Y TEUS</h3>
    <hr />
    <div class="col-lg-9">
        <div class="input-group">
            <asp:TextBox ID="txtBuscar" runat="server" MaxLength="4" class="form-control" placeholder="introducir año (mínimo permitido 2013)"></asp:TextBox>
            <span class="input-group-btn">
                <asp:Button ID="btnBuscar" runat="server" class="btn btn-default" Text="Consultar"
                    OnClick="btnBuscar_Click" />
            </span>
        </div>
        <!-- /input-group -->
    </div>
    <!-- /.col-lg-6 -->
    <br />
    <br />
    <asp:UpdatePanel ID="EmployeesUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>            
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="d_linea"
                CssClass="footable" Style="margin-top: 5%; margin-left: 15px;" ShowFooter="true"
                OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="d_linea" HeaderText="LINEA NAVIERA"></asp:BoundField>
                    <asp:BoundField DataField="vi_luni" HeaderText="Cont. Importacion Llenos (UNI)" DataFormatString="{0:N0}"></asp:BoundField>                   
                    <asp:BoundField DataField="vi_lteus" HeaderText="Total TEUS Cont. Import. Llenos" DataFormatString="{0:N0}"></asp:BoundField> 
                    <asp:BoundField DataField="vi_vuni" HeaderText="Cont. Importacion Vacios (UNI)" DataFormatString="{0:N0}"></asp:BoundField>                   
                    <asp:BoundField DataField="vi_vteus" HeaderText="Total TEUS Cont. Import Vacios" DataFormatString="{0:N0}"></asp:BoundField>
                    <asp:BoundField DataField="t_import" HeaderText="Total TEUS Importacion" DataFormatString="{0:N0}" ItemStyle-Font-Bold="true"></asp:BoundField>                   
                    <asp:BoundField DataField="ve_luni" HeaderText="Cont. Exportacion Llenos (UNI)" DataFormatString="{0:N0}"></asp:BoundField>  
                    <asp:BoundField DataField="ve_lteus" HeaderText="Total TEUS Cont. Export. Llenos" DataFormatString="{0:N0}"></asp:BoundField>                 
                    <asp:BoundField DataField="ve_vuni" HeaderText="Cont. Exportacion Vacios (UNI)" DataFormatString="{0:N0}"></asp:BoundField>   
                    <asp:BoundField DataField="ve_vuni" HeaderText="Total TEUS Cont. Export. Vacios" DataFormatString="{0:N0}"></asp:BoundField>   
                    <asp:BoundField DataField="t_export" HeaderText="Total TEUS Exportacion" DataFormatString="{0:N0}" ItemStyle-Font-Bold="true"></asp:BoundField>  
                    <asp:BoundField DataField="t_uni" HeaderText="Total Contenedores" DataFormatString="{0:N0}" ItemStyle-Font-Bold="true"></asp:BoundField>  
                    <asp:BoundField DataField="t_teus" HeaderText="Total TEUS" DataFormatString="{0:N0}" ItemStyle-Font-Bold="true"></asp:BoundField> 
                    <%--<asp:BoundField DataField="c_agencia" HeaderText=""></asp:BoundField>--%>
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:Label ID="lblMensaje" runat="server" Text="" CssClass="alert-danger lead" style="margin-left: 15px;"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />            
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <br />    
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
</asp:Content>

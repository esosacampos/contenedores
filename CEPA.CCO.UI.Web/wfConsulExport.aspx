<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfConsulExport.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfConsulExport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        #table_exp {
            display:initial;
            margin-top: 15px;
            /*text-align: center;*/
        }
           /* display: initial;*/
           #Content2-section--div{
           padding-bottom: 25px;
           }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Consulta Contenedores De Exportación</h2>
    <hr />
    <%--<iframe width="100%" height="400px" id="iFrameEjemplo" src="https://10.1.4.20:8081/wf_fa_cont_exp.aspx"></iframe>--%>
    <div>
        <br />  
        <section>
            <div class="col-lg-4 col-sm-6" id="Content2-section--div">
                <label><strong>N° Contenedor:</strong></label>
                <input type="text" id="txt_Contenedor" name="txt_Contenedor" maxlength="11" required pattern="([A-Z]{3})U([0-9]{7})" autocomplete="off" runat="server" placeholder="N° de Contenedor" class="form-control" />                
                <asp:Button id="Button1" onClick="btn_Buscar_Click" runat="server" Text="Buscar..." class="btn btn-primary btn-lg"/>
            </div>
        </section>
            <div class="table-responsive"  id="table_exp" >
                <asp:Repeater ID="Rpt_lista" runat="server">
                    <HeaderTemplate>
                        <table class="table table-hover" summary="Contenedor de Exportación">
                            <thead>
                                <tr>
                                    <th scope="col">N° Contenedor</th>
                                    <th scope="col">Notificar</th>
                                    <th scope="col">Ingreso</th>
                                    <th scope="col">Exportado</th>
                                    <th scope="col">N° Días</th>
                                    <th scope="col">Peso Kg.</th>
                                    <th scope="col">Comentario</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Contenedor") %></td>
                            <td><%# Eval("Notificar") %></td>
                            <td><%# Eval("Ingreso", "{0:dd/MM/yyyy}") %></td>
                            <td><%# Eval("Exportacion") == DBNull.Value ? "NO" : Eval("Exportacion", "{0:dd/MM/yyyy}") %></td>
                            <td><%# Eval("Ndias") %></td>
                            <td><%# Eval("Peso") %></td>
                            <td><%# Eval("Leyenda") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="wfModPaises.aspx.cs" Inherits="CEPA.CCO.UI.Web.wfModPaises" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .bs-pagination {
            background-color: #1771F8;
        }

        .footable > tbody > tr > td {
            border-top: 1px solid #dddddd;
            padding: 7px;
            text-align: center;
            border-left: none;
            font-family: 'trebuchet MS', 'Lucida sans', Arial;
        }

        .hiddencol {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Países</h2>
    <hr />
    <div class="form-group" style="margin-left: 15px;">
        <label for="texto">
            Buscar</label>
        <input type="text" class="form-control" id="filter" placeholder="Ingrese búsqueda rápida (Coloque Verdadero para los países con MOSTYN o Falso países con CARBONATO)">
    </div>
    <asp:UpdatePanel runat="server" ID="Employe" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="CountryCode"
                CssClass="footable" data-filter="#filter" data-page-size="10"
                ShowFooter="true" OnRowCreated="onRowCreate" PagerStyle-CssClass="pagination pagination-centered hide-if-no-paging" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="CountryName" HeaderText="NOMBRE"></asp:BoundField>
                    <asp:BoundField DataField="s_oirsa" HeaderText="" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"></asp:BoundField>
                    <asp:TemplateField HeaderText="HABILITADO">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox1" runat="server" onclick="Check_Click(this)" Checked='<%# Eval("b_oirsa") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click" />
            <%-- <asp:PostBackTrigger ControlID="GridView1" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <hr />
    <nav>
        <ul class="pager">
            <li class="previous">
                <asp:Button ID="btnCargar" runat="server" class="btn btn-primary btn-lg" Text="Guardar"
                    OnClientClick="return confirmaSave(this.id);" OnClick="btnCargar_Click" />
            </li>
            <li class="next">
                <asp:Button ID="btnRegresar" runat="server" class="btn btn-primary btn-lg" Text="<< Regresar"
                    OnClick="btnRegresar_Click" />
            </li>
        </ul>
    </nav>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockui.js") %>"></script>
    <script type="text/javascript">

        function iniciaVariable() {
            $.session.set("_conteVar", '');
            confirmed = false;
        }


        var confirmed = false;


        function confirmaSave(controlID) {
            if (confirmed) { return true; }

            var _cadena = $.session.get("_conteVar");
            _cadena = _cadena.substr(0, _cadena.length - 2)

            bootbox.confirm("Se modificarán los países: " + _cadena, function (result) {
                if (result) {
                    if (controlID != null) {
                        var controlToClick = document.getElementById(controlID);
                        if (controlToClick != null) {
                            confirmed = true;
                            controlToClick.click();
                            confirmed = false;
                        }
                    }
                } else {
                    bootbox.alert("De no continuar con la actualización puede dar F5")
                }

            });

            return false;
        }

        //function buttonDisable() {
        //    var s = $.session.get("_conteVar");
        //    if (s != '')
        //        $('[id$="btnCargar"]').removeAttr("disabled");
        //    else
        //        $('[id$="btnCargar"]').attr('disabled', 'disabled');

        //}

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            var _conte = '';
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";

                var conte = row.children[0].innerText + '/ ';
                var _conte = $.session.get("_conteVar") + conte;

                $.session.set("_conteVar", _conte);

                //buttonDisable();
            }
            else {
                row.style.backgroundColor = "#efefef";

                var conte = row.children[0].innerText + '/ ';
                var _conte = $.session.get("_conteVar") + conte;

                $.session.set("_conteVar", _conte);

                //buttonDisable();
            }
        }





        Page = Sys.WebForms.PageRequestManager.getInstance();
        Page.add_beginRequest(OnBeginRequest);
        var postbackElement;

        if (Page != null) {
            Page.add_endRequest(function (sender, e) {
                //llenarSelect();

                if (sender._postBackSettings.panelsToUpdate != null) {
                    //llenarSelect();
                }

                var a = postbackElement;

                var a = Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack();
                if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
                    //__doPostBack('btnCargar', '');

                    var testGrid = $get('<%= GridView1.ClientID %>');
                    //alert(testGrid.rows[1].cells[0].innerHTML);

                    //setTimeout(function () { location.reload(); }, 1000);
                    pageLoad();
                    $("#GridView1").load(location.href + " #GridView1");
                }

                $.unblockUI();

            });
        };

        function OnBeginRequest(sender, args) {

            postbackElement = args.get_postBackElement();
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



        }


        //function endRequest(sender, args) {

        //    $.unblockUI();
        //    llenarSelect();

        //}



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



        function pageLoad() {
            $(document).ready(function () {



                //$('[id$="btnCargar"]').attr('disabled', 'disabled');

                iniciaVariable();

                $("#filter").val('');

                $('#ContentPlaceHolder1_GridView1').footable();

                $('.bs-pagination td table').each(function (index, obj) {
                    convertToPagination(obj);
                });


            });
        }
    </script>
</asp:Content>

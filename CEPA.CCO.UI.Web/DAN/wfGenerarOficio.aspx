<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="wfGenerarOficio.aspx.cs" Inherits="CEPA.CCO.UI.Web.DAN.wfGenerarOficio" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <script src="<%=Page.ResolveUrl("~/Scripts/jquery-1.7.1.min.js") %>" type="text/javascript"></script>--%>
    <style type="text/css">
        .style1
        {
            width: 1095px;
        }
        .style2
        {
            width: 165px;
        }
        .style3
        {
            width: 840px;
        }
        .style4
        {
            width: 160px;
        }
        
        #BtnImprimir
        {
            border: 1px solid transparent;
        }
        #BtnImprimir:hover
        {
            border: 1px solid rgb(51,102,153);
            background-color: rgb(221,238,247);
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
        Generar Oficio</h2>
    <hr />
    <div class="col-lg-6">
        <div class="form-inline">
            <div class="input-group">
                <asp:TextBox ID="TextBox1" runat="server" class="form-control" autocomplete="off"
                    placeholder="Ingrese # oficio"></asp:TextBox>               
            </div>
            <div class="input-group">
                <asp:TextBox ID="Datepicker" runat="server" class="form-control" autocomplete="off"
                    placeholder="Ingrese año del oficio" Text=""></asp:TextBox>
                 <span class="input-group-btn">
                    <asp:Button ID="brnReporte" runat="server" class="btn btn-default" Text="Generar"
                        OnClick="brnReporte_Click" OnClientClick="ocultar();" />
                </span>
            </div>
            <%--<!-- /input-group -->--%>
        </div>
    </div>
    <!-- /.col-lg-6 -->
    <br />
    <br />
    <%--    <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="False" ShowPrintButton="True"
        InteractivityPostBackMode="AlwaysSynchronous" Style="width: 100%; height: auto;">
    </rsweb:ReportViewer>--%>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowToolBar="True" Font-Names="Verdana"
        Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ClientIDMode="Static"
        PageCountMode="Actual" ShowPageNavigationControls="False" ShowPrintButton="False"
        ShowRefreshButton="False" ShowZoomControl="False" Style="width: 100%; height: auto;">
    </rsweb:ReportViewer>
    <%--<input id="printreport" type="button" value="Imprimir Reporte" />--%>
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
        }
        function endRequest(sender, args) {

            $.unblockUI();

        }
        //------------------------------------------------------------------
        // Cross-browser Multi-page Printing with ASP.NET ReportViewer
        // by Chtiwi Malek.
        // http://www.codicode.com
        //------------------------------------------------------------------

        // Linking the print function to the print button
        //        $('#printreport').click(function () {
        //            printReport('ctl00_ContentPlaceHolder1_ReportViewer1');
        //        });

        var urlImg = '<%=Page.ResolveUrl("~/CSS/Images/print.gif") %>';

        // Función que se ejecuta una vez se ha terminado de cargar el DOM de la página web en el navegador
        $(document).ready(function () {
            //  debugger;
            colocarBtnImprimir();    // Colocar el botón de imprimir en la barra de herramientas del ReportViewer
            $("#BtnImprimir").click(function () {                
                printReport('ctl00_ContentPlaceHolder1_ReportViewer1');
            });  // Asignando la función "imprimirDiv" al evento click del botón de impresión

        });




        // ctl00_ContentPlaceHolder1_ReportViewer1_ctl05
        //ctl00_ContentPlaceHolder1_ReportViewer1_ctl06
        // Esta función coloca el botón de imprimir en la barra de herramientas del ReportViewer
        function colocarBtnImprimir() {
            var jqoBarraRpt = $('div#ctl00_ContentPlaceHolder1_ReportViewer1_ctl06>div:first-child');    // Buscando el div que contiene la barra de herramientas del RportViewer

            if (jqoBarraRpt && jqoBarraRpt.length > 0    // Verificando que el DIV barra de herramientas fue encontrado,
                && jqoBarraRpt.find('#BtnImprimir').length <= 0) {    // y verificando que el botón de imprimir no existe ya

                // Colocando el botón de impresión, con una estructura similar a la que tiene el botón original en el ReportViewer
                jqoBarraRpt.append('<table cellpadding="0" cellspacing="0" ToolbarSpacer="true" style="display:inline-block;width:10px;"><tr><td></td></tr></table><div style="display:inline-block;font-family:Verdana;font-size:8pt;vertical-align:top;"><table cellpadding="0" cellspacing="0" style="display:inline-block;"><tr><td height="28px"><div"><div id="BtnImprimir"><table title="Print"><tr><td><img title="Print" src="' + urlImg + '" alt="Print" style="border-style:None;height:16px;width:16px;" /></td></tr></table></div><div disabled="disabled" style="display:none;border:1px transparent Solid;"><table title="Print"><tr><td><input type="image" disabled="disabled" title="Print" src="' + urlImg + '" alt="Print" style="border-style:None;height:16px;width:16px;cursor:default;" /></td></tr></table></div></div></td></tr></table></div>');
            }
        }

        // Print function (require the reportviewer client ID)
        function printReport(report_ID) {
            var rv1 = $('#' + report_ID);
            var iDoc = rv1.parents('html');

            // Reading the report styles
            var styles = iDoc.find("head style[id$='ReportControl_styles']").html();
            if ((styles == undefined) || (styles == '')) {
                iDoc.find('head script').each(function () {
                    var cnt = $(this).html();
                    var p1 = cnt.indexOf('ReportStyles":"');
                    if (p1 > 0) {
                        p1 += 15;
                        var p2 = cnt.indexOf('"', p1);
                        styles = cnt.substr(p1, p2 - p1);
                    }
                });
            }
            if (styles == '') { alert("Cannot generate styles, Displaying without styles.."); }
            styles = '<style type="text/css">' + styles + "</style>";

            // Reading the report html
            var table = rv1.find("div[id$='_oReportDiv']");
            if (table == undefined) {
                alert("Report source not found.");
                return;
            }

            // Generating a copy of the report in a new window
            var docType = '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/loose.dtd">';
            var docCnt = styles + table.parent().html();
            var docHead = '<head><title>Printing ...</title><style>body{margin:5;padding:0;}</style></head>';
            var winAttr = "location=yes,statusbar=no,directories=no,menubar=no,titlebar=no,toolbar=no,dependent=no,width=770,height=600,resizable=yes,screenX=250,screenY=250,personalbar=no,scrollbars=yes"; ;
            var newWin = window.open("", "_blank", winAttr);
            writeDoc = newWin.document;
            writeDoc.open();
            writeDoc.write(docType + '<html>' + docHead + '<body onload="window.print();">' + docCnt + '</body></html>');
            writeDoc.close();

            // The print event will fire as soon as the window loads
            newWin.focus();
            // uncomment to autoclose the preview window when printing is confirmed or canceled.
            // newWin.close();
        };

    </script>
</asp:Content>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using CEPA.CCO.Linq;

using System.IO;
using System.Globalization;
using MoreLinq;
using NReco.PdfGenerator;


using cxExcel = ClosedXML.Excel;
using System.Text.RegularExpressions;

namespace CEPA.CCO.UI.Web
{
    public partial class wfAlertaDAN : System.Web.UI.Page
    {

        public string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        public bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }


        protected void Page_Load(object sender, EventArgs e)
        {

              
        }

        public string ShowHtml()
        {
            string Html = null;
            DateTime _fecha;

             var query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.verdadero)
                             join b in EncaBuqueDAL.ObtenerBuquesJoinC(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                             select new AlertaDAN
                             {
                                 c_numeral = a.c_numeral,
                                 c_naviera = a.c_naviera,
                                 d_naviera = b.d_cliente,
                                 n_contenedor = a.n_contenedor,
                                 f_liberacion = a.f_liberacion,
                                 ClaveP = a.ClaveP,
                                 ClaveQ = a.ClaveQ,
                                 c_transporte = a.c_transporte
                             }).ToList();

             _fecha = DetaNavieraLINQ.FechaBD();

             if (query.Count > 0)
             {


                 Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                 Html += "<h2 style='font-size:24px;'>LISTADO DE CONTENEDORES REVISADOS Y LIBERADOS</h2><hr><br />";

                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; text-align:center; \">";
                 Html += "<tr>";
                 Html += "<center>";
                 Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                 Html += "<td width=\"50px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                 Html += "<td width=\"100px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>FECHA/HORA LIBERACION</font></th>";
                 Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>P</font></th>";
                 Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>Q</font></th>";
                 Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TRANSPORTE</font></th>";
                 Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>AGENCIA NAVIERA</font></th>";
                 Html += "</center>";
                 Html += "</tr>";

                 foreach (var itemC in query)
                 {
                     Html += "<tr>";
                     Html += "<center>";
                     Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.c_numeral + "</font></td>";
                     Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.n_contenedor + "</font></td>";
                     Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.f_liberacion + "</font></td>";
                     Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.ClaveP + "</font></td>";
                     Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.ClaveQ + "</font></td>";
                     Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.c_transporte + "</font></td>";
                     Html += "<td height=\"25\"><font size=2 color=blue>" + itemC.d_naviera + "</font></td>";
                     Html += "</center>";
                     Html += "</tr>";
                     Html += "</font>";
                 }

                 Html += "</table>";

                 Html += "<br /><br /><br />";
                 Html += "<font color='blue' size=4><b style='line-height: 1.6;'>NOTA:</b><br/><br/><i style='line-height:1.6;'>Favor gestionar con quien corresponda, la movilización inmediata de los contenedores listados, con el propósito de liberar espacios para el registro de otros contenedores, CEPA se reserva el derecho de movilizarlos con sus equipos y facturar el arrendamiento de estos.-</i></font><br />";
                 Html += "<br />";


                 Session["Html"] = Html;
             }
             else
             {
                 ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + string.Format("No se poseen liberaciones hasta la fecha {0}", _fecha.ToString("dd/MM/yyyy HH:mm")) + "');", true); 

                 Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                 Html += string.Format("<h2 style='font-size:24px;'>NO SE POSEEN LIBERACIONES HASTA LA FECHA {0} </h2><hr><br />", _fecha.ToString("dd/MM/yyyy HH:mm")); 
             }

                return Html;
        }

        [System.Web.Services.WebMethod]
        public static string Llenar(string c_periodo)
        {
            string query = ShowHtml(c_periodo);


            return query;

        }
        

        private static string ShowHtml(string fecha)
        {
            string Html = null;
           
            DateTime _fecha;

            var query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.verdadero, fecha)
                         join b in EncaBuqueDAL.ObtenerBuquesJoinC(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new AlertaDAN
                         {
                             c_numeral = a.c_numeral,
                             c_naviera = a.c_naviera,
                             d_naviera = b.d_cliente,
                             n_contenedor = a.n_contenedor,
                             f_liberacion = a.f_liberacion,
                             ClaveP = a.ClaveP,
                             ClaveQ = a.ClaveQ,
                             c_transporte = a.c_transporte
                         }).ToList();

            _fecha = DetaNavieraLINQ.FechaBD();

            if (query.Count > 0)
            {


                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<h2 style='font-size:24px;'>LISTADO DE CONTENEDORES REVISADOS Y LIBERADOS</h2><hr><br />";

               
                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; text-align:center; \">";
                Html += "<tr>";
                Html += "<center>";
                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                Html += "<td width=\"50px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                Html += "<td width=\"100px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>FECHA/HORA LIBERACION</font></th>";
                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>P</font></th>";
                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>Q</font></th>";
                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TRANSPORTE</font></th>";
                Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>AGENCIA NAVIERA</font></th>";
                Html += "</center>";
                Html += "</tr>";

                foreach (var itemC in query)
                {
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.c_numeral + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.n_contenedor + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.f_liberacion + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.ClaveP + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.ClaveQ + "</font></td>";
                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemC.c_transporte + "</font></td>";
                    Html += "<td height=\"25\"><font size=2 color=blue>" + itemC.d_naviera + "</font></td>";
                    Html += "</center>";
                    Html += "</tr>";
                    Html += "</font>";
                }

                Html += "</table>";

                Html += "<br /><br /><br />";
                Html += "<font color='blue' size=4><b style='line-height: 1.6;'>NOTA:</b><br/><br/><i style='line-height:1.6;'>Favor gestionar con quien corresponda, la movilización inmediata de los contenedores listados, con el propósito de liberar espacios para el registro de otros contenedores, CEPA se reserva el derecho de movilizarlos con sus equipos y facturar el arrendamiento de estos.-</i></font><br />";
                Html += "<br />";


                //Session["Html"] = Html;
            }
            else
            {
                

                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += string.Format("<h2 style='font-size:24px;'>NO SE POSEEN LIBERACIONES HASTA LA FECHA {0} </h2><hr><br />", _fecha.ToString("dd/MM/yyyy HH:mm"));

                //throw new Exception(string.Format("NO SE POSEEN LIBERACIONES HASTA LA FECHA {0}", fecha));
            }

            return Html;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(30000);   
            ExportarExcel();
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "blah", "closeLoading();", true);
            
        }


        public void ExportarExcel()
        {

            var query = new List<AlertaDAN>();
            if (txtDOB.Text.Trim().TrimEnd().TrimStart() == string.Empty || txtDOB.Text.Trim().TrimEnd().TrimStart().Length == 0)
            {
                query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.verdadero)
                         join b in EncaBuqueDAL.ObtenerBuquesJoinC(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new AlertaDAN
                         {
                             c_numeral = a.c_numeral,
                             c_naviera = a.c_naviera,
                             d_naviera = b.d_cliente,
                             n_contenedor = a.n_contenedor,
                             f_liberacion = a.f_liberacion,
                             ClaveP = a.ClaveP,
                             ClaveQ = a.ClaveQ,
                             c_transporte = a.c_transporte
                         }).ToList();
            }
            else
            {
                string fecha = Convert.ToDateTime(txtDOB.Text).ToString("dd/MM/yyyy");
                query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.verdadero, fecha)
                         join b in EncaBuqueDAL.ObtenerBuquesJoinC(DBComun.Estado.verdadero) on new { c_cliente = a.c_naviera, c_llegada = a.c_llegada } equals new { c_cliente = b.c_cliente, c_llegada = b.c_llegada }
                         select new AlertaDAN
                         {
                             c_numeral = a.c_numeral,
                             c_naviera = a.c_naviera,
                             d_naviera = b.d_cliente,
                             n_contenedor = a.n_contenedor,
                             f_liberacion = a.f_liberacion,
                             ClaveP = a.ClaveP,
                             ClaveQ = a.ClaveQ,
                             c_transporte = a.c_transporte
                         }).ToList();
            }

            int d_lineas = 0;

            if (query.Count > 0)
            {
                int ROWS_START = 5;
                int iRow = 1;
                d_lineas = query.Count;
                //Crear libro de Excel
                var oWB = new cxExcel.XLWorkbook();

                // Crear hoja de trabajo
                var oSheet = oWB.Worksheets.Add("ALERTA DAN");

                oSheet.Range("A2", "G2").Merge();
                oSheet.Range("A2", "G2").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A2", "G2").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Range("A2", "G2").Style.Font.FontSize = 16;

                oSheet.Cell("A2").Value = "LISTADO DE CONTENEDORES REVISADOS Y LIBERADOS";
             

                
                oSheet.Cell("A2").Style.Font.Bold = true;
                

                          
                oSheet.Cell(5, 1).Value = "No.";
                oSheet.Cell(5, 2).Value = "CONTENEDOR";
                oSheet.Cell(5, 3).Value = "FECHA/HORA LIBERACION";
                oSheet.Cell(5, 4).Value = "P";
                oSheet.Cell(5, 5).Value = "Q";
                oSheet.Cell(5, 6).Value = "TRANSPORTE";
                oSheet.Cell(5, 7).Value = "AGENCIA NAVIERA";


                oSheet.Column(1).Width = 11;
                oSheet.Column(2).Width = 22;
                oSheet.Column(3).Width = 25;
                oSheet.Column(4).Width = 2;
                oSheet.Column(5).Width = 2;
                oSheet.Column(6).Width = 38;
                oSheet.Column(7).Width = 60;

                oSheet.Range("A5:G5").Style.Font.Bold = true;
                oSheet.Range("A5:G5").Style.Font.FontColor = cxExcel.XLColor.White;
                oSheet.Range("A5:G5").Style.Fill.BackgroundColor = cxExcel.XLColor.FromArgb(21, 132, 206);
                oSheet.Range("A5:G5").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A5:G5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                               
                oSheet.Range("A5", string.Concat("G", ROWS_START + d_lineas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                oSheet.Range("A5", string.Concat("G", ROWS_START + d_lineas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;
                                                           
                oSheet.Range("A5", string.Concat("G", ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(cxExcel.XLColor.FromArgb(79, 129, 189));
                oSheet.Range("A5", string.Concat("G", ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.FromArgb(79, 129, 189));
                                                           
                oSheet.Range("A6", string.Concat("G", ROWS_START + d_lineas)).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);
                oSheet.Range("A6", string.Concat("G", ROWS_START + d_lineas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A6", string.Concat("G", ROWS_START + d_lineas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

               

                oSheet.Range("G6", string.Concat("G", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                oSheet.Range("F6", string.Concat("F", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);

                foreach (var item in query)
                {
                    int iCurrent = ROWS_START + iRow;
                    oSheet.Row(iCurrent).Height = 28;
                    oSheet.Cell(iCurrent, 1).Value = item.c_numeral;
                    oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                    oSheet.Cell(iCurrent, 3).Value = item.f_liberacion;
                    oSheet.Cell(iCurrent, 4).Value = item.ClaveP;
                    oSheet.Cell(iCurrent, 5).Value = item.ClaveQ;
                    oSheet.Cell(iCurrent, 6).Value = SanitizeXmlString(item.c_transporte);
                    oSheet.Cell(iCurrent, 7).Value = SanitizeXmlString(item.d_naviera);

                    iRow = iRow + 1;
                }


                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Value = "NOTA";
                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Style.Font.Bold = true;
                oSheet.Cell(string.Concat("A", ((ROWS_START + d_lineas) + 2))).Style.Font.FontSize = 16;
                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);


                oSheet.Range(string.Concat("A", ((ROWS_START + d_lineas) + 3)), string.Concat("G", ((ROWS_START + d_lineas) + 7))).Merge();

                oSheet.Range(string.Concat("A", ((ROWS_START + d_lineas) + 3)), string.Concat("G", ((ROWS_START+d_lineas) + 7))).Value = "Favor gestionar con quien corresponda la movilización inmediata de los contenedores listados con el propósito de liberar espacios para el registro de otros contenedores CEPA se reserva el derecho de movilizarlos con sus equipos y facturar el arrendamiento de estos";

                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("G", ((ROWS_START+d_lineas) + 7))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("G", ((ROWS_START+d_lineas) + 7))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("G", ((ROWS_START+d_lineas) + 7))).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);
                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("G", ((ROWS_START+d_lineas) + 7))).Style.Font.Italic = true;
                oSheet.Range(string.Concat("A", ((ROWS_START + d_lineas) + 3)), string.Concat("G", ((ROWS_START + d_lineas) + 7))).Style.Font.FontSize = 14;
                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("G", ((ROWS_START+d_lineas) + 7))).Style.Alignment.SetWrapText(true);


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Alerta.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    oWB.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('No se poseen liberaciones que exportar');", true);              

                
            }

        }


    }
}
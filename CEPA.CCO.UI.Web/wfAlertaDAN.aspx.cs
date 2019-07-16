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

using NReco.PdfGenerator;


using cxExcel = ClosedXML.Excel;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net;
using System.Data;

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
        public static string getTrans(string c_llegada, string c_contenedor, string c_naviera)
        {
            string _contenedores = "";
            string apiUrl = "http://138.219.156.210:83/api/Ejecutar/?Consulta=";
            Procedure proceso = new Procedure
            {
                NBase = "CONTENEDORES",
                Procedimiento = "Sqlvprovitransp", // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
                Parametros = new List<Parametros>()
            };
            proceso.Parametros.Add(new Parametros { nombre = "llegada", valor = c_llegada });
            proceso.Parametros.Add(new Parametros { nombre = "_contenedor", valor = c_contenedor });
            proceso.Parametros.Add(new Parametros { nombre = "navi", valor = c_naviera });

            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static string Conectar(string _contenedores, string apiUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            string file = string.Empty;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            //string idx = "{ "DBase":"CONTENEDORES","Servidor":null,"Procedimiento":"Sqlentllenos","Consulta":true,"Parametros":[{"nombre":"_dia","valor":"15-05-2019"}]}";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                file = sr.ReadToEnd();
                DataTable tabla = JsonConvert.DeserializeObject<DataTable>(file) as DataTable;
                if (tabla.Rows.Count > 0)
                {
                    if (!tabla.Rows[0][0].ToString().StartsWith("ERROR"))
                    {
                        _contenedores = tabla.Rows[0][0].ToString();
                    }
                }
            }
            return _contenedores;
        }
        public string ShowHtml()
        {
            string Html = null;
            try
            {
                
                DateTime _fecha;
                string c_tipo = null;
                if (Session["c_naviera"].ToString() == "11")
                    c_tipo = "DAN";                
                else if (Session["c_naviera"].ToString() == "216")
                    c_tipo = "UCC";               
                else
                    c_tipo = "CEP";

                var query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.verdadero, c_tipo)
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
                                 c_transporte = getTrans(a.c_llegada, a.n_contenedor, a.c_naviera),
                                 f_salida = a.f_salida,
                                 f_confir_salida = a.f_confir_salida,
                                 tipo = a.tipo
                             }).ToList();

                _fecha = DetaNavieraLINQ.FechaBD();

                if (query.Count > 0)
                {


                    Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                    Html += "<h2 style='font-size:24px;margin-left: 1%;'>LISTADO DE CONTENEDORES REVISADOS Y LIBERADOS</h2><hr><br />";

                    Html += "<div class=\"form-group\" style=\"margin-left:15px;\"><label for=\"texto\">Buscar</label><input type=\"text\" class=\"form-control\" id=\"filter\" placeholder=\"Ingrese búsqueda rápida\" style=\"width: 99%;\"></div>";
                    Html += "<br>";
                    Html += "<table class=\"table\" data-page-size=\"10\" data-filter=\"#filter\" style=\"border-collapse:collapse;\">";
                    Html += "<thead><tr>";
                    Html += "<th data-class=\"expand\" scope=\"col\" class=\"footable-visible footable-first-column\">No.</th>";
                    Html += "<th scope=\"col\" class=\"footable-visible\">TIPO</th>";
                    Html += "<th scope=\"col\" class=\"footable-visible\">CONTENEDOR</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">F/H LIBERACION</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">P</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">Q</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">TRANSPORTE</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">AGENCIA NAVIERA</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">F/H SALIDA</th>";
                    Html += "<th scope=\"col\" class=\"footable-visible footable-last-column\">F/H PUERTA #1</th>";
                    Html += "</tr></thead>";

                    Html += "<tbody>";
                    foreach (var itemC in query)
                    {
                        Html += "<tr style=\"display: table-row;\">";
                        Html += "<center>";
                        Html += "<td class=\"expand footable-visible footable-first-column\">" + itemC.c_numeral + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.tipo + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.n_contenedor + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.f_liberacion + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.ClaveP + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.ClaveQ + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.c_transporte + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.d_naviera + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.f_salida + "</td>";
                        Html += "<td class=\"footable-visible footable-last-column\">" + itemC.f_confir_salida + "</td>";
                        Html += "</center></tr>";

                    }
                    Html += "</tbody>";
                    Html += "<tfoot><tr><td text-align=\"center\" colspan=\"10\" class=\"footable-visible\">" + "<ul class='pagination pagination-centered hide-if-no-paging'></ul><div class='divider' style='margin-bottom: 15px;'></div></div><span class='label label-default pie' style='background-color: #dff0d8;border-radius: 25px;font-family: sans-serif;font-size: 18px;color: #468847;border-color: #d6e9c6;margin-top: 18px;'></span>";
                    Html += "</td></tr></tfoot>";
                    Html += "</table>";

                    Html += "<br /><br /><br />";
                    Html += "<font color='blue' size=4><b style='line-height: 1.6;'>NOTA:</b><br/><br/><i style='line-height:1.6;'>Favor gestionar con quien corresponda, la movilización inmediata de los contenedores listados, con el propósito de liberar espacios para el registro de otros contenedores, CEPA se reserva el derecho de movilizarlos con sus equipos y facturar el arrendamiento de estos.-</i></font><br />";
                    Html += "<br />";


                    Session["Html"] = Html;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + string.Format("No se poseen liberaciones hasta la fecha {0}", _fecha.ToString("dd/MM/yyyy HH:mm")) + "');", true);

                    Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.8em\">";
                    Html += string.Format("<h2 style='font-size:24px;'>NO SE POSEEN LIBERACIONES HASTA LA FECHA {0} </h2><hr><br />", _fecha.ToString("dd/MM/yyyy HH:mm"));
                }

                return Html;
            }
            catch (Exception)
            {
                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<h2 style='font-size:24px;'>ERROR: ESTE MODULO ESTA PRESENTANDO PROBLEMAS CONSULTE A SOPORTE TECNICO </h2><hr><br />";
                return Html;
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string Llenar(string c_periodo)
        {
            string c_tipo = null;
            if (HttpContext.Current.Session["c_naviera"].ToString() == "11")
                c_tipo = "DAN";
            else if (HttpContext.Current.Session["c_naviera"].ToString() == "216")
                c_tipo = "UCC";
            else
                c_tipo = "CEP";


            string query = ShowHtml(c_periodo, c_tipo);


            return query;

        }
        

        private static string ShowHtml(string fecha, string c_tipo)
        {
            string Html = null;
            try
            {
                DateTime _fecha;

                


                var query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.verdadero, fecha, c_tipo)
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
                                 c_transporte = getTrans(a.c_llegada, a.n_contenedor, a.c_naviera),
                                 f_salida = a.f_salida,
                                 f_confir_salida = a.f_confir_salida,
                                 tipo = a.tipo
                             }).ToList();

                _fecha = DetaNavieraLINQ.FechaBD();

                if (query.Count > 0)
                {


                    Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                    Html += "<h2 style='font-size:24px;margin-left: 1%;'>LISTADO DE CONTENEDORES REVISADOS Y LIBERADOS</h2><hr><br />";

                    Html += "<div class=\"form-group\" style=\"margin-left:15px;\"><label for=\"texto\">Buscar</label><input type=\"text\" class=\"form-control\" id=\"filter\" placeholder=\"Ingrese búsqueda rápida\" style=\"width: 99%;\"></div>";
                    Html += "<br>";

                    Html += "<table class=\"table\" data-page-size=\"10\" data-filter=\"#filter\" style=\"border-collapse:collapse;\">";
                    Html += "<thead><tr>";                    
                    Html += "<th data-class=\"expand\" scope=\"col\" class=\"footable-visible footable-first-column\">No.</th>";
                    Html += "<th scope=\"col\" class=\"footable-visible\">TIPO</th>";
                    Html += "<th scope=\"col\" class=\"footable-visible\">CONTENEDOR</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">F/H LIBERACION</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">P</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">Q</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">TRANSPORTE</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">AGENCIA NAVIERA</th>";
                    Html += "<th data-hide=\"phone\" scope=\"col\" class=\"footable-visible\">F/H SALIDA</th>";
                    Html += "<th scope=\"col\" class=\"footable-visible footable-last-column\">F/H PUERTA #1</th>";                    
                    Html += "</tr></thead>";

                    Html += "<tbody>";
                    foreach (var itemC in query)
                    {
                        Html += "<tr style=\"display: table-row;\">";
                        Html += "<center>";
                        Html += "<td class=\"expand footable-visible footable-first-column\">" + itemC.c_numeral + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.tipo + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.n_contenedor + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.f_liberacion + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.ClaveP + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.ClaveQ + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.c_transporte + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.d_naviera + "</td>";
                        Html += "<td class=\"footable-visible\">" + itemC.f_salida + "</td>";
                        Html += "<td class=\"footable-visible footable-last-column\">" + itemC.f_confir_salida + "</td>";
                        Html += "</center></tr>";
                                            
                    }
                    Html += "</tbody>";
                    Html += "<tfoot><tr><td text-align=\"center\" colspan=\"10\" class=\"footable-visible\">" + "<ul class='pagination pagination-centered hide-if-no-paging'></ul><div class='divider' style='margin-bottom: 15px;'></div></div><span class='label label-default pie' style='background-color: #dff0d8;border-radius: 25px;font-family: sans-serif;font-size: 18px;color: #468847;border-color: #d6e9c6;margin-top: 18px;'></span>";
                    Html +="</td></tr></tfoot>";
                    
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
            catch (Exception)
            {
                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<h2 style='font-size:24px;'>ERROR: ESTE MODULO ESTA PRESENTANDO PROBLEMAS CONSULTE A SOPORTE TECNICO </h2><hr><br />";
                return Html;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(30000);   
            ExportarExcel();
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "blah", "closeLoading();", true);
            
        }


        public void ExportarExcel()
        {
            string c_tipo = null;
            if (Session["c_naviera"].ToString() == "11")
                c_tipo = "DAN";
            else if (Session["c_naviera"].ToString() == "216")
                c_tipo = "UCC";
            else
                c_tipo = "CEP";
            var query = new List<AlertaDAN>();
            if (txtDOB.Text.Trim().TrimEnd().TrimStart() == string.Empty || txtDOB.Text.Trim().TrimEnd().TrimStart().Length == 0)
            {
                query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.verdadero, c_tipo)
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
                             c_transporte = getTrans(a.c_llegada, a.n_contenedor, a.c_naviera),
                             f_salida = a.f_salida,
                             f_confir_salida = a.f_confir_salida,
                             tipo = a.tipo
                         }).ToList();
            }
            else
            {
                string fecha = Convert.ToDateTime(txtDOB.Text).ToString("dd/MM/yyyy");
                query = (from a in AlertaDANDAL.ObtenerAlerta(DBComun.Estado.verdadero, fecha, c_tipo)
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
                             c_transporte = getTrans(a.c_llegada, a.n_contenedor, a.c_naviera),
                             f_salida = a.f_salida,
                             f_confir_salida = a.f_confir_salida,
                             tipo = a.tipo
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

                oSheet.Range("A2", "J2").Merge();
                oSheet.Range("A2", "J2").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A2", "J2").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Range("A2", "J2").Style.Font.FontSize = 16;

                oSheet.Cell("A2").Value = "LISTADO DE CONTENEDORES REVISADOS Y LIBERADOS";
             

                
                oSheet.Cell("A2").Style.Font.Bold = true;
                

                          
                oSheet.Cell(5, 1).Value = "No.";
                oSheet.Cell(5, 2).Value = "TIPO";
                oSheet.Cell(5, 3).Value = "CONTENEDOR";
                oSheet.Cell(5, 4).Value = "F/H LIBERACION";
                oSheet.Cell(5, 5).Value = "P";
                oSheet.Cell(5, 6).Value = "Q";
                oSheet.Cell(5, 7).Value = "TRANSPORTE";
                oSheet.Cell(5, 8).Value = "AGENCIA NAVIERA";
                oSheet.Cell(5, 9).Value = "F/H SALIDA";
                oSheet.Cell(5, 10).Value = "F/H PUERTA #1";


                oSheet.Column(1).Width = 11;
                oSheet.Column(2).Width = 8;
                oSheet.Column(3).Width = 22;
                oSheet.Column(4).Width = 25;
                oSheet.Column(5).Width = 2;
                oSheet.Column(6).Width = 2;
                oSheet.Column(7).Width = 38;
                oSheet.Column(8).Width = 60;
                oSheet.Column(9).Width = 25;
                oSheet.Column(10).Width = 25;

                oSheet.Range("A5:J5").Style.Font.Bold = true;
                oSheet.Range("A5:J5").Style.Font.FontColor = cxExcel.XLColor.White;
                oSheet.Range("A5:J5").Style.Fill.BackgroundColor = cxExcel.XLColor.FromArgb(21, 132, 206);
                oSheet.Range("A5:J5").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A5:J5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                               
                oSheet.Range("A5", string.Concat("J", ROWS_START + d_lineas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                oSheet.Range("A5", string.Concat("J", ROWS_START + d_lineas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;
                                                           
                oSheet.Range("A5", string.Concat("J", ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(cxExcel.XLColor.FromArgb(79, 129, 189));
                oSheet.Range("A5", string.Concat("J", ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.FromArgb(79, 129, 189));
                                                           
                oSheet.Range("A6", string.Concat("J", ROWS_START + d_lineas)).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);
                oSheet.Range("A6", string.Concat("J", ROWS_START + d_lineas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A6", string.Concat("J", ROWS_START + d_lineas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

               

                oSheet.Range("J6", string.Concat("J", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                oSheet.Range("I6", string.Concat("I", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                oSheet.Range("G6", string.Concat("G", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                oSheet.Range("H6", string.Concat("H", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);

                foreach (var item in query)
                {
                    int iCurrent = ROWS_START + iRow;
                    oSheet.Row(iCurrent).Height = 28;
                    oSheet.Cell(iCurrent, 1).Value = item.c_numeral;
                    oSheet.Cell(iCurrent, 2).Value = item.tipo;
                    oSheet.Cell(iCurrent, 3).Value = item.n_contenedor;
                    oSheet.Cell(iCurrent, 4).Value = item.f_liberacion;
                    oSheet.Cell(iCurrent, 5).Value = item.ClaveP;
                    oSheet.Cell(iCurrent, 6).Value = item.ClaveQ;
                    oSheet.Cell(iCurrent, 7).Value = SanitizeXmlString(item.c_transporte);
                    oSheet.Cell(iCurrent, 8).Value = SanitizeXmlString(item.d_naviera);
                    oSheet.Cell(iCurrent, 9).Value = item.f_salida;
                    oSheet.Cell(iCurrent, 10).Value = item.f_confir_salida;
                    iRow = iRow + 1;
                }


                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Value = "NOTA";
                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Style.Font.Bold = true;
                oSheet.Cell(string.Concat("A", ((ROWS_START + d_lineas) + 2))).Style.Font.FontSize = 16;
                oSheet.Cell(string.Concat("A", ((ROWS_START+d_lineas) + 2))).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);


                oSheet.Range(string.Concat("A", ((ROWS_START + d_lineas) + 3)), string.Concat("I", ((ROWS_START + d_lineas) + 7))).Merge();

                oSheet.Range(string.Concat("A", ((ROWS_START + d_lineas) + 3)), string.Concat("I", ((ROWS_START+d_lineas) + 7))).Value = "Favor gestionar con quien corresponda la movilización inmediata de los contenedores listados con el propósito de liberar espacios para el registro de otros contenedores CEPA se reserva el derecho de movilizarlos con sus equipos y facturar el arrendamiento de estos";

                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("I", ((ROWS_START+d_lineas) + 7))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("I", ((ROWS_START+d_lineas) + 7))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("I", ((ROWS_START+d_lineas) + 7))).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);
                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("I", ((ROWS_START+d_lineas) + 7))).Style.Font.Italic = true;
                oSheet.Range(string.Concat("A", ((ROWS_START + d_lineas) + 3)), string.Concat("I", ((ROWS_START + d_lineas) + 7))).Style.Font.FontSize = 14;
                oSheet.Range(string.Concat("A", ((ROWS_START+d_lineas) + 3)), string.Concat("I", ((ROWS_START+d_lineas) + 7))).Style.Alignment.SetWrapText(true);


                string _nombre = null;
                if (txtDOB.Text.Trim().TrimEnd().TrimStart()== string.Empty)
                {
                    _nombre = "Alerta_" + DateTime.Now.ToString("ddMMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + ".xlsx";
                }
                else
                {
                    _nombre = "Alerta_" + Convert.ToDateTime(txtDOB.Text).ToString("ddMMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + ".xlsx";
                }

                 
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", _nombre));

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
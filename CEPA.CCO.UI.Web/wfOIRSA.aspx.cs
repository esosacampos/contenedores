using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using CEPA.CCO.Linq;
using System.Threading;

using System.IO;
using System.Globalization;
using NReco.PdfGenerator;

using cxExcel = ClosedXML.Excel;
using System.Text.RegularExpressions;
using System.Text;

namespace CEPA.CCO.UI.Web
{
    public partial class wfOIRSA : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                try
                {

                    Cargar();

                }
                catch (Exception Ex)
                {
                    Response.Write("<script>bootbox.alert('" + Ex.Message + "');</script>");
                }
            }

            RegisterPostBackControl();
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.ObtenerDAN_Trans();
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;
        }


        public void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Cargar();
            }
            catch (Exception ex)
            {
                Response.Write("<script>bootbox.alert('" + ex.Message + "');</script>");
            }
        }

        protected void onRowCreate(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int colSpan = e.Row.Cells.Count;

                for (int i = (e.Row.Cells.Count - 1); i >= 1; i -= 1)
                {
                    e.Row.Cells.RemoveAt(i);
                    e.Row.Cells[0].ColumnSpan = colSpan;
                }

                e.Row.Cells[0].Controls.Add(new LiteralControl("<ul class='pagination pagination-centered hide-if-no-paging'></ul><div class='divider'  style='margin-bottom: 15px;'></div></div><span class='label label-default pie' style='background-color: #dff0d8;border-radius: 25px;font-family: sans-serif;font-size: 18px;color: #468847;border-color: #d6e9c6;margin-top: 18px;'></span>"));
            }
        }

        [System.Web.Services.WebMethod]
        public static string GenerarPDF(string c_llegada, string d_buque)
        {
            string mensaje = null;

            List<ArchivoAduana> _listaAOrdenar = DetaNavieraDAL.GetListOIRSA(c_llegada, DBComun.Estado.verdadero);




            return Newtonsoft.Json.JsonConvert.SerializeObject(mensaje);
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "btnOpen")
                {
                    string c_llegada = Convert.ToString(e.CommandArgument);
                    ExportarExcel(c_llegada, 1);
                    Cargar();
                }
                else if (e.CommandName == "btnCobro")
                {
                    string c_llegada = Convert.ToString(e.CommandArgument);
                    ExportarExcel(c_llegada, 2);
                    Cargar();

                }
                return;
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }

        //protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Button lnkOpe = (e.Row.FindControl("lnkOpe") as Button);


        //        ScriptManager current = ScriptManager.GetCurrent(Page);
        //        if (current != null)
        //            current.RegisterAsyncPostBackControl(lnkOpe);


        //        Button lnkCobro = (e.Row.FindControl("lnkCobro") as Button);


        //        current = ScriptManager.GetCurrent(Page);
        //        if (current != null)
        //            current.RegisterAsyncPostBackControl(lnkCobro);



        //    }
        //}


        public void ExportarExcel(string c_llegada, int pListado)
        {

            var query = new List<DetaNaviera>();
            if (c_llegada.Length > 0)
            {
                query = (from a in AlertaDANDAL.ObtenerOIRSAO(DBComun.Estado.verdadero, c_llegada, pListado)
                         join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_llegada = a.c_llegada, c_cliente = a.c_navi } equals new { c_llegada = b.c_llegada, c_cliente = b.c_cliente }
                         select new DetaNaviera
                         {
                             c_correlativo = a.c_correlativo,
                             c_cliente = a.c_cliente,
                             n_contenedor = a.n_contenedor,
                             c_tamaño = a.c_tamaño,
                             c_tamañoc = a.c_tamañoc,
                             c_pais_origen = a.c_pais_origen,
                             c_pais_destino = a.c_pais_destino,
                             c_tratamiento = a.c_tratamiento,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_trans = b.f_llegada.ToString("dd/MM/yyyy"),
                             c_voyage = a.c_voyage,
                             d_20 = a.d_20,
                             d_4045 = a.d_4045,
                             s_20 = a.s_20,
                             s_4045 = a.s_4045,
                             s_nombre = b.d_cliente,
                             c_navi = a.c_navi,
                             s_consignatario = a.s_consignatario
                         }).ToList();
            }

            int d_lineas = 0;

            if (query.Count > 0)
            {
                string d_barco = null;
                string f_llegada = null;
                string d_cliente = null;
                string corto = null;
                string voyage = null;
                int ROWS_START = 0;
                int iRow = 1;
                d_lineas = query.Count;
                //Crear libro de Excel
                var oWB = new cxExcel.XLWorkbook();

                foreach (var buque in query)
                {
                    d_barco = buque.d_buque;
                    f_llegada = buque.f_trans;
                    break;
                }

                if (pListado == 1)
                {
                    ROWS_START = 5;
                    #region "Tipo 1"
                    // Crear hoja de trabajo
                    var oSheet = oWB.Worksheets.Add(d_barco.ToUpper().Replace(" ", "_"));

                    oSheet.Range("A1", "H1").Merge();
                    oSheet.Range("A1", "H1").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A1", "H1").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                    oSheet.Range("A1", "H1").Style.Font.FontSize = 24;

                    oSheet.Cell("A1").Value = "ORGANISMO INTERNACIONAL REGIONAL DE SANIDAD AGROPECUARIA";
                    oSheet.Cell("A1").Style.Font.Bold = true;
                    oSheet.Row(1).Height = 53;

                    oSheet.Range("A2", "H2").Merge();
                    oSheet.Range("A2", "H2").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A2", "H2").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                    oSheet.Range("A2", "H2").Style.Font.FontSize = 24;

                    oSheet.Cell("A2").Value = d_barco.ToUpper() + " - " + f_llegada;
                    oSheet.Cell("A2").Style.Font.Bold = true;
                    oSheet.Row(2).Height = 43;



                    oSheet.Cell(5, 1).Value = "No.";
                    oSheet.Cell(5, 2).Value = "NAVIERA";
                    oSheet.Cell(5, 3).Value = "CONTENEDOR";
                    oSheet.Cell(5, 4).Value = "TIPO";
                    oSheet.Cell(5, 5).Value = "CONSIGNATARIO";
                    oSheet.Cell(5, 6).Value = "P. PROCEDENCIA";
                    oSheet.Cell(5, 7).Value = "P. DESTINO";
                    oSheet.Cell(5, 8).Value = "TRATAMIENTO";

                    oSheet.Column(1).Width = 5;
                    oSheet.Column(2).Width = 12;
                    oSheet.Column(3).Width = 23;
                    oSheet.Column(4).Width = 11;
                    oSheet.Column(5).Width = 50;
                    oSheet.Column(6).Width = 36;
                    oSheet.Column(7).Width = 16;
                    oSheet.Column(8).Width = 18;

                    oSheet.Range("A5:H5").Style.Font.Bold = true;
                    //oSheet.Range("A5:H5").Style.Font.FontColor = cxExcel.XLColor.White;
                    oSheet.Range("A5:H5").Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                    oSheet.Range("A5:H5").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A5:H5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                    oSheet.Range("A5", string.Concat("H", ROWS_START + d_lineas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                    oSheet.Range("A5", string.Concat("H", ROWS_START + d_lineas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                    oSheet.Range("A5", string.Concat("H", ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                    oSheet.Range("A5", string.Concat("H", ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);

                    //oSheet.Range("A6", string.Concat("H", ROWS_START + d_lineas)).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);
                    oSheet.Range("A6", string.Concat("H", ROWS_START + d_lineas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A6", string.Concat("H", ROWS_START + d_lineas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;


                    oSheet.Range("H6", string.Concat("H", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                    //oSheet.Range("F6", string.Concat("F", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                    oSheet.Range("A6", string.Concat("H", ROWS_START + d_lineas)).Style.Font.FontSize = 13;
                    foreach (var item in query)
                    {
                        int iCurrent = ROWS_START + iRow;
                        oSheet.Row(iCurrent).Height = 50;
                        oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                        oSheet.Cell(iCurrent, 2).Value = SanitizeXmlString(item.c_cliente);
                        oSheet.Cell(iCurrent, 3).Value = item.n_contenedor;
                        oSheet.Cell(iCurrent, 4).Value = item.c_tamaño;
                        oSheet.Cell(iCurrent, 5).Value = SanitizeXmlString(item.s_consignatario);
                        oSheet.Cell(iCurrent, 6).Value = SanitizeXmlString(item.c_pais_origen);
                        oSheet.Cell(iCurrent, 7).Value = SanitizeXmlString(item.c_pais_destino);
                        oSheet.Cell(iCurrent, 8).Value = SanitizeXmlString(item.c_tratamiento);

                        if (item.c_tratamiento == "CARBONATO")
                            oSheet.Range(string.Concat("A", iCurrent), string.Concat("H", iCurrent)).Style.Fill.BackgroundColor = cxExcel.XLColor.FromArgb(255, 255, 204);

                        iRow = iRow + 1;
                    }

                    oSheet.PageSetup.PageOrientation = cxExcel.XLPageOrientation.Portrait;
                    oSheet.PageSetup.AdjustTo(50);
                    oSheet.PageSetup.PaperSize = cxExcel.XLPaperSize.LetterPaper;
                    oSheet.PageSetup.VerticalDpi = 600;
                    oSheet.PageSetup.HorizontalDpi = 600;
                    oSheet.PageSetup.Margins.Top = 0.3;
                    oSheet.PageSetup.Margins.Header = 0.40;
                    oSheet.PageSetup.Margins.Footer = 0.20;
                    oSheet.PageSetup.SetRowsToRepeatAtTop(1, 5);

                    oSheet.Range("C6", string.Concat("C", ROWS_START + d_lineas)).Style.Font.FontSize = 17;
                    oSheet.Range("C6", string.Concat("C", ROWS_START + d_lineas)).Style.Font.Bold = true;

                    oSheet.Range("H6", string.Concat("H", ROWS_START + d_lineas)).Style.Font.FontSize = 17;
                    oSheet.Range("H6", string.Concat("H", ROWS_START + d_lineas)).Style.Font.Bold = true;

                    string _nombre = string.Concat(d_barco.Replace(" ", "_"), "_", c_llegada, ".xlsx");
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
                    #endregion
                }
                else
                {
                    ROWS_START = 6;

                    #region "Tipo 2"
                    var grupoNavi = (from a in query
                                     group a by a.c_navi into g
                                     select new
                                     {
                                         c_naviera = g.Key
                                     }).ToList();


                    foreach (var grpNavie in grupoNavi)
                    {
                        var quen = query.Where(a => a.c_navi == grpNavie.c_naviera).ToList();
                        d_lineas = quen.Count;
                        iRow = 1;
                        ROWS_START = 6;
                        foreach (var c in quen)
                        {
                            d_cliente = c.s_nombre;
                            corto = c.c_cliente;
                            voyage = c.c_voyage;
                            break;
                        }

                        // Crear hoja de trabajo

                        var oCelda = corto + "_" + d_barco.ToUpper().Replace(" ", "_") + "_" + voyage;
                        string nombreCelda = null;
                        if (oCelda.Length >= 31)
                            nombreCelda = (corto + "_" + d_barco.ToUpper().Replace(" ", "_") + "_" + voyage).Substring(0,31);
                        else
                            nombreCelda = corto + "_" + d_barco.ToUpper().Replace(" ", "_") + "_" + voyage;

                        var oSheet = oWB.Worksheets.Add(nombreCelda);


                        oSheet.Range("A1", "J1").Merge();
                        oSheet.Range("A1", "J1").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A1", "J1").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("A1", "J1").Style.Font.FontSize = 24;

                        oSheet.Cell("A1").Value = "ORGANISMO INTERNACIONAL REGIONAL DE SANIDAD AGROPECUARIA";
                        oSheet.Cell("A1").Style.Font.Bold = true;
                        oSheet.Row(1).Height = 40;

                        oSheet.Range("A2", "J2").Merge();
                        oSheet.Range("A2", "J2").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A2", "J2").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("A2", "J2").Style.Font.FontSize = 18;

                        oSheet.Cell("A2").Value = d_cliente.ToUpper();
                        oSheet.Cell("A2").Style.Font.Bold = true;
                        oSheet.Row(2).Height = 30;


                        oSheet.Range("A3", "J3").Merge();
                        oSheet.Range("A3", "J3").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A3", "J3").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("A3", "J3").Style.Font.FontSize = 24;

                        oSheet.Cell("A3").Value = d_barco.ToUpper() + " - V. " + voyage + " - F. " + f_llegada;
                        oSheet.Cell("A3").Style.Font.Bold = true;
                        oSheet.Row(3).Height = 30;

                        oSheet.Range("G5", "J5").Merge();
                        oSheet.Range("G5", "J5").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("G5", "J5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("G5", "J5").Style.Font.FontSize = 14;

                        oSheet.Cell("G5").Value = "TARIFAS A APLICAR";
                        oSheet.Cell("G5").Style.Font.Bold = true;
                        oSheet.Row(5).Height = 25;
                        oSheet.Row(6).Height = 25;

                        oSheet.Range("G5:J5").Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;

                        oSheet.Range("G5:J5").Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                        oSheet.Range("G5:J5").Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                        oSheet.Range("G5:J5").Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                        oSheet.Range("G5:J5").Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);

                        oSheet.Cell(ROWS_START, 1).Value = "No.";
                        oSheet.Cell(ROWS_START, 2).Value = "CONTENEDOR";
                        oSheet.Cell(ROWS_START, 3).Value = "TÑO";
                        oSheet.Cell(ROWS_START, 4).Value = "CONSIGNATARIO";
                        oSheet.Cell(ROWS_START, 5).Value = "P. DESTINO";
                        oSheet.Cell(ROWS_START, 6).Value = "P. ORIGEN";
                        oSheet.Cell(ROWS_START, 7).Value = "$ 16.00";
                        oSheet.Cell(ROWS_START, 8).Value = "$ 21.00";
                        oSheet.Cell(ROWS_START, 9).Value = "$ 6.00";
                        oSheet.Cell(ROWS_START, 10).Value = "$ 7.00";

                        oSheet.Column(1).Width = 5;
                        oSheet.Column(2).Width = 23;
                        oSheet.Column(3).Width = 10;
                        oSheet.Column(4).Width = 67;
                        oSheet.Column(5).Width = 18;
                        oSheet.Column(6).Width = 37;
                        oSheet.Column(7).Width = 16;
                        oSheet.Column(8).Width = 16;
                        oSheet.Column(9).Width = 16;
                        oSheet.Column(10).Width = 16;


                        oSheet.Range("A6:J6").Style.Font.Bold = true;
                        //oSheet.Range("A5:H5").Style.Font.FontColor = cxExcel.XLColor.White;
                        oSheet.Range("A6:J6").Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                        oSheet.Range("A6:J6").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A6:J6").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                        oSheet.Range("A6", string.Concat("J", ROWS_START + d_lineas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                        oSheet.Range("A6", string.Concat("J", ROWS_START + d_lineas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                        oSheet.Range("A6", string.Concat("J", ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                        oSheet.Range("A6", string.Concat("J", ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);

                        //oSheet.Range("A6", string.Concat("H", ROWS_START + d_lineas)).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);
                        oSheet.Range("A7", string.Concat("J", ROWS_START + d_lineas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A7", string.Concat("J", ROWS_START + d_lineas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;


                        oSheet.Range("J7", string.Concat("J", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                        //oSheet.Range("F6", string.Concat("F", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                        oSheet.Range("A7", string.Concat("J", ROWS_START + d_lineas)).Style.Font.FontSize = 13;
                        oSheet.Range("G7", string.Concat("J", (ROWS_START + d_lineas) + 1)).Style.NumberFormat.SetFormat("$ #,##0.00");
                        foreach (var item in quen)
                        {
                            int iCurrent = ROWS_START + iRow;
                            oSheet.Row(iCurrent).Height = 25;
                            oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                            oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                            oSheet.Cell(iCurrent, 3).Value = SanitizeXmlString(item.c_tamañoc);
                            oSheet.Cell(iCurrent, 4).Value = SanitizeXmlString(item.s_consignatario);
                            oSheet.Cell(iCurrent, 5).Value = SanitizeXmlString(item.c_pais_destino);
                            oSheet.Cell(iCurrent, 6).Value = SanitizeXmlString(item.c_pais_origen);
                            if (item.d_20 > 0.00)
                                oSheet.Cell(iCurrent, 7).Value = item.d_20;
                            else
                                oSheet.Cell(iCurrent, 7).Value = "";

                            if (item.d_4045 > 0.00)
                                oSheet.Cell(iCurrent, 8).Value = item.d_4045;
                            else
                                oSheet.Cell(iCurrent, 8).Value = "";

                            if (item.s_20 > 0.00)
                                oSheet.Cell(iCurrent, 9).Value = item.s_20;
                            else
                                oSheet.Cell(iCurrent, 9).Value = "";

                            if (item.s_4045 > 0.00)
                                oSheet.Cell(iCurrent, 10).Value = item.s_4045;
                            else
                                oSheet.Cell(iCurrent, 10).Value = "";


                            if (item.c_tratamiento == "CARBONATO")
                                oSheet.Range(string.Concat("A", iCurrent), string.Concat("J", iCurrent)).Style.Fill.BackgroundColor = cxExcel.XLColor.FromArgb(255, 255, 204);


                            iRow = iRow + 1;
                        }
                        int val = ROWS_START + d_lineas;
                        oSheet.Cell(val + 1, 7).SetFormulaA1(string.Format("=SUM(G7:G{0})", val.ToString()));
                        oSheet.Cell(val + 2, 7).SetFormulaA1(string.Format("=COUNTIF(G7:G{0},16)", val.ToString()));

                        oSheet.Cell(val + 1, 8).SetFormulaA1(string.Format("=SUM(H7:H{0})", val.ToString()));
                        oSheet.Cell(val + 2, 8).SetFormulaA1(string.Format("=COUNTIF(H7:H{0},21)", val.ToString()));

                        oSheet.Cell(val + 1, 9).SetFormulaA1(string.Format("=SUM(I7:I{0})", val.ToString()));
                        oSheet.Cell(val + 2, 9).SetFormulaA1(string.Format("=COUNTIF(I7:I{0},6)", val.ToString()));

                        oSheet.Cell(val + 1, 10).SetFormulaA1(string.Format("=SUM(J7:J{0})", val.ToString()));
                        oSheet.Cell(val + 2, 10).SetFormulaA1(string.Format("=COUNTIF(J7:J{0},7)", val.ToString()));


                        oSheet.Range(string.Concat("G", (val + 1)), string.Concat("J", val + 2)).Style.Font.FontSize = 13;

                        oSheet.Range(string.Concat("G", (val + 1)), string.Concat("J", val + 2)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range(string.Concat("G", (val + 1)), string.Concat("J", val + 2)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range(string.Concat("G", (val + 1)), string.Concat("J", val + 2)).Style.Font.FontColor = cxExcel.XLColor.FromArgb(156, 101, 0);
                        oSheet.Range(string.Concat("G", (val + 1)), string.Concat("J", val + 2)).Style.Fill.BackgroundColor = cxExcel.XLColor.FromArgb(255, 235, 156);



                        oSheet.Range(string.Concat("G", (val + 3)), string.Concat("I", val + 3)).Merge();
                        oSheet.Cell(string.Concat("G", (val + 3))).Value = "TOTAL  .   .   .   .";
                        oSheet.Cell(string.Concat("G", (val + 3))).Style.Font.Bold = true;
                        oSheet.Range(string.Concat("G", (val + 3)), string.Concat("J", val + 3)).Style.Font.FontSize = 16;

                        oSheet.Range(string.Concat("G", (val + 3)), string.Concat("I", val + 3)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range(string.Concat("G", (val + 3)), string.Concat("I", val + 3)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;

                        oSheet.Range(string.Concat("J", (val + 3)), string.Concat("J", val + 3)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range(string.Concat("J", (val + 3)), string.Concat("J", val + 3)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Cell(string.Concat("J", (val + 3))).Style.Font.Bold = true;

                        oSheet.Range(string.Concat("G", (val + 3)), string.Concat("J", val + 3)).Style.Font.FontColor = cxExcel.XLColor.Red;
                        oSheet.Range(string.Concat("G", (val + 3)), string.Concat("J", val + 3)).Style.Fill.BackgroundColor = cxExcel.XLColor.FromArgb(255, 235, 156);

                        oSheet.Cell(val + 3, 10).SetFormulaA1(string.Format("=SUM(G{0}:J{1})", (val + 1).ToString(), (val + 1).ToString()));
                        oSheet.Range(string.Concat("J", val + 3)).Style.NumberFormat.SetFormat("$ #,##0.00");


                        oSheet.Cell(string.Concat("A", val + 5)).Value = "Julio Francisco Flores";
                        oSheet.Cell(string.Concat("A", val + 6)).Value = "Facturación CEPA";
                        oSheet.Cell(string.Concat("A", val + 6)).Style.Font.Bold = true;



                        oSheet.Cell(string.Concat("D", val + 5)).Value = "Hilda Beatriz Pérez";
                        oSheet.Cell(string.Concat("D", val + 6)).Value = "Colectora SITC - Acajutla";
                        oSheet.Cell(string.Concat("D", val + 6)).Style.Font.Bold = true;
                        oSheet.Range(string.Concat("A", (val + 5)), string.Concat("D", val + 6)).Style.Font.FontSize = 13;


                        oSheet.PageSetup.PageOrientation = cxExcel.XLPageOrientation.Landscape;
                        oSheet.PageSetup.AdjustTo(53);
                        oSheet.PageSetup.PaperSize = cxExcel.XLPaperSize.LetterPaper;
                        oSheet.PageSetup.VerticalDpi = 600;
                        oSheet.PageSetup.HorizontalDpi = 600;
                        oSheet.PageSetup.Margins.Top = 0.3;
                        oSheet.PageSetup.Margins.Header = 0.40;
                        oSheet.PageSetup.Margins.Footer = 0.20;
                        //oSheet.PageSetup.SetRowsToRepeatAtTop(1, 5);



                    }

                    string _nombre = string.Concat("COBRO_" + d_barco.Replace(" ", "_"), "_", f_llegada.Replace("/", ""), ".xlsx");
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

                    #endregion
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('No se poseen registros para el listado seleccionado');", true);


            }

        }


        public void RegisterPostBackControl()
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                Button lnkFull = row.FindControl("lnkOpe") as Button;
                ScriptManager current = ScriptManager.GetCurrent(Page);
                if (current != null)
                    current.RegisterPostBackControl(lnkFull);

                Button lnkCobro = row.FindControl("lnkCobro") as Button;
                ScriptManager _current = ScriptManager.GetCurrent(Page);
                if (_current != null)
                    _current.RegisterPostBackControl(lnkCobro);
            }
        }

    }
}
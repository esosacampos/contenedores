using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ab = iTextSharp.text.pdf.draw;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using cxExcel = ClosedXML.Excel;
using Font = iTextSharp.text.Font;


namespace CEPA.CCO.UI.Web
{
    public partial class wfBuquesTrans : System.Web.UI.Page
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
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: " + Ex.Message + "');", true);
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
                Response.Write("<script>alert('" + ex.Message + "');</script>");
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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //try
            //{
                if (e.CommandName == "btnOpen")
                {
                    string c_llegada = Convert.ToString(e.CommandArgument);
                    ExportarExcel(c_llegada, 1);
                    Cargar();
                }
                else if (e.CommandName == "btnList")
                {
                    string c_llegada = Convert.ToString(e.CommandArgument);
                    ExportarExcel(c_llegada, 2);
                    Cargar();
                }
                else if (e.CommandName == "btnListExp")
                {
                    string c_llegada = Convert.ToString(e.CommandArgument);
                    ExportarExcel(c_llegada, 3);
                    Cargar();
                }
                else if (e.CommandName == "btnListExpED")
                {
                    string c_llegada = Convert.ToString(e.CommandArgument);
                    ExportarExcel(c_llegada, 4);
                    Cargar();
                }
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: Durante la ejecución recomendamos volverlo intentar, o reportar a Informatica" + "');", true);

            //}

            return;
        }
        public void ExportarExcel(string c_llegada, int pListado)
        {
            if (pListado == 1)
            {
                var query = new List<DetaNaviera>();
                if (c_llegada.Length > 0)
                {
                    query = (from a in AlertaDANDAL.getListESC(DBComun.Estado.verdadero, c_llegada)
                             join b in EncaBuqueDAL.ObtenerBuquesJoin(DBComun.Estado.verdadero) on new { c_llegada = a.c_llegada, c_cliente = a.c_cliente } equals new { c_llegada = b.c_llegada, c_cliente = b.c_cliente }
                             select new DetaNaviera
                             {
                                 c_correlativo = a.c_correlativo,
                                 c_navi = a.c_navi,
                                 n_contenedor = a.n_contenedor,
                                 b_estado = a.b_estado,
                                 c_tamaño = a.c_tamaño,
                                 b_retenido = a.b_retenido,
                                 b_aduanas = a.b_aduanas,
                                 d_buque = b.d_buque,
                                 f_trans = b.f_llegada.ToString("dd/MM/yyyy")
                             }).OrderBy(a => a.c_correlativo).ToList();

                }

                int d_lineas = 0;

                if (query.Count > 0)
                {
                    string d_barco = null;
                    string f_llegada = null;



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


                    ROWS_START = 6;
                    #region "Tipo 1"
                    // Crear hoja de trabajo
                    var oSheet = oWB.Worksheets.Add(d_barco.ToUpper().Replace(" ", "_"));

                    oSheet.Range("A1", "I1").Merge();
                    oSheet.Range("A1", "I1").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A1", "I1").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                    oSheet.Range("A1", "I1").Style.Font.FontSize = 28;

                    oSheet.Cell("A1").Value = "UNIDAD DE CONTENEDORES";
                    oSheet.Cell("A1").Style.Font.Bold = true;
                    oSheet.Row(1).Height = 30;

                    oSheet.Range("A2", "I2").Merge();
                    oSheet.Range("A2", "I2").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A2", "I2").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                    oSheet.Range("A2", "I2").Style.Font.FontSize = 20;

                    oSheet.Cell("A2").Value = "LISTADO DE CONTENEDORES A ESCANEAR SEGÚN DAN - DGA";
                    oSheet.Cell("A2").Style.Font.Bold = true;
                    oSheet.Row(2).Height = 23;

                    oSheet.Range("A3", "I3").Merge();
                    oSheet.Range("A3", "I3").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A3", "I3").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                    oSheet.Range("A3", "I3").Style.Font.FontSize = 18;

                    oSheet.Cell("A3").Value = d_barco.ToUpper() + " - " + f_llegada;
                    oSheet.Cell("A3").Style.Font.Bold = true;
                    oSheet.Row(3).Height = 23;



                    oSheet.Cell(6, 1).Value = "No.";
                    oSheet.Cell(6, 2).Value = "CONTENEDOR";
                    oSheet.Cell(6, 3).Value = "NAVIERA";
                    oSheet.Cell(6, 4).Value = "ESTADO";
                    oSheet.Cell(6, 5).Value = "TAMAÑO";
                    oSheet.Cell(6, 6).Value = "DAN";
                    oSheet.Cell(6, 7).Value = "DGA";
                    oSheet.Cell(6, 8).Value = "E. ESCANER";
                    oSheet.Cell(6, 9).Value = "S. ESCANER";

                    #region "Config"
                    oSheet.Column(1).Width = 5;
                    oSheet.Column(2).Width = 30;
                    oSheet.Column(3).Width = 20;
                    oSheet.Column(4).Width = 11;
                    oSheet.Column(5).Width = 12;
                    oSheet.Column(6).Width = 6;
                    oSheet.Column(7).Width = 6;
                    oSheet.Column(8).Width = 25;
                    oSheet.Column(9).Width = 25;


                    oSheet.Range("A6:I6").Style.Font.Bold = true;
                    oSheet.Range("A6:I6").Style.Font.FontSize = 16;
                    oSheet.Row(6).Height = 21;
                    //oSheet.Range("A5:H5").Style.Font.FontColor = cxExcel.XLColor.White;
                    oSheet.Range("A6:I6").Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                    oSheet.Range("A6:I6").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A6:I6").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                    oSheet.Range("A6", string.Concat("I", ROWS_START + d_lineas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                    oSheet.Range("A6", string.Concat("I", ROWS_START + d_lineas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                    oSheet.Range("A6", string.Concat("I", ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                    oSheet.Range("A6", string.Concat("I", ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);

                    //oSheet.Range("A6", string.Concat("H", ROWS_START + d_lineas)).Style.Font.FontColor = cxExcel.XLColor.FromArgb(0, 0, 255);
                    oSheet.Range("A7", string.Concat("I", ROWS_START + d_lineas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A7", string.Concat("I", ROWS_START + d_lineas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;


                    oSheet.Range("I7", string.Concat("I", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                    //oSheet.Range("F6", string.Concat("F", ROWS_START + d_lineas)).Style.Alignment.SetWrapText(true);
                    oSheet.Range("A7", string.Concat("I", ROWS_START + d_lineas)).Style.Font.FontSize = 14;

                    #endregion
                    foreach (var item in query)
                    {
                        int iCurrent = ROWS_START + iRow;
                        oSheet.Row(iCurrent).Height = 48;
                        oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                        oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                        oSheet.Cell(iCurrent, 3).Value = SanitizeXmlString(item.c_navi);
                        oSheet.Cell(iCurrent, 4).Value = item.b_estado;
                        oSheet.Cell(iCurrent, 5).Value = SanitizeXmlString(item.c_tamaño);
                        oSheet.Cell(iCurrent, 6).Value = SanitizeXmlString(item.b_retenido);
                        oSheet.Cell(iCurrent, 7).Value = SanitizeXmlString(item.b_aduanas);


                        if (item.b_aduanas == "X")
                        {
                            oSheet.Range(string.Concat("A", iCurrent), string.Concat("I", iCurrent)).Style.Fill.BackgroundColor = cxExcel.XLColor.FromArgb(255, 255, 204);
                        }

                        iRow = iRow + 1;
                    }

                    oSheet.PageSetup.PageOrientation = cxExcel.XLPageOrientation.Portrait;
                    oSheet.PageSetup.AdjustTo(60);
                    oSheet.PageSetup.PaperSize = cxExcel.XLPaperSize.LetterPaper;
                    oSheet.PageSetup.VerticalDpi = 600;
                    oSheet.PageSetup.HorizontalDpi = 600;
                    oSheet.PageSetup.Margins.Top = 0.3;
                    oSheet.PageSetup.Margins.Header = 0.40;
                    oSheet.PageSetup.Margins.Footer = 0.20;
                    oSheet.PageSetup.SetRowsToRepeatAtTop(1, 6);

                    oSheet.Range("B7", string.Concat("B", ROWS_START + d_lineas)).Style.Font.FontSize = 24;
                    oSheet.Range("B7", string.Concat("B", ROWS_START + d_lineas)).Style.Font.Bold = true;

                    oSheet.Range("A7", string.Concat("A", ROWS_START + d_lineas)).Style.Font.FontSize = 18;
                    oSheet.Range("A7", string.Concat("A", ROWS_START + d_lineas)).Style.Font.Bold = true;


                    oSheet.PageSetup.Footer.Left.AddText(string.Format("&B Buque : {0} Fecha : {1} &B", d_barco, f_llegada));
                    oSheet.PageSetup.Footer.Center.AddText("Página ", cxExcel.XLHFOccurrence.AllPages);
                    oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.PageNumber, cxExcel.XLHFOccurrence.AllPages);
                    oSheet.PageSetup.Footer.Center.AddText(" de ", cxExcel.XLHFOccurrence.AllPages);
                    oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.NumberOfPages, cxExcel.XLHFOccurrence.AllPages);


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
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('No se poseen registros para el listado seleccionado');", true);


                }
            }
            else if (pListado == 2)
            {
                var queryNavi = new List<DetaNaviera>();

                if (c_llegada.Length > 0)
                {
                    queryNavi = AlertaDANDAL.getNavierasOpe(DBComun.Estado.verdadero, c_llegada);

                    if (queryNavi.Count > 0)
                    {
                        string d_buque = null;

                        Document pdfDoc = new Document(PageSize.LETTER);
                        PdfWriter pdfWrite = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();


                        const int ROWS_START = 8;
                        int Filas = 0;

                        foreach (var item in queryNavi)
                        {
                            List<EnvioAuto> listAuto = new List<EnvioAuto>();
                            listAuto = DetaNavieraDAL.ObtenerEncAu(item.IdReg, DBComun.Estado.verdadero);

                            string c_navi_corto = null, c_viaje = null;
                            int n_manifiesto = 0;

                            foreach (var detaills in listAuto)
                            {
                                c_navi_corto = detaills.c_naviera_corto;
                                c_viaje = detaills.c_voyaje;
                                n_manifiesto = detaills.n_manifiesto;
                                break;
                            }

                            var lstHearder = (from a in EncaBuqueDAL.getHeaderLleg(DBComun.Estado.verdadero, item.c_navi, item.c_llegada)
                                              join b in AlertaDANDAL.getPrefiNavi(DBComun.Estado.verdadero) on a.c_cliente equals b.c_naviera
                                              select new EncaBuque
                                              {
                                                  c_llegada = a.c_llegada,
                                                  c_buque = a.c_buque,
                                                  d_buque = a.d_buque,
                                                  c_cliente = a.c_cliente,
                                                  d_cliente = a.d_cliente,
                                                  f_llegada = a.f_llegada,
                                                  c_imo = a.c_imo,
                                                  c_prefijo = b.d_naviera_p
                                              }).ToList();


                            if (lstHearder == null)
                            {
                                lstHearder = new List<EncaBuque>();
                            }


                            if (lstHearder.Count > 0)
                            {
                                string d_cliente = null, fs_llegada = null, c_prefi = null;
                                int Cuenta = 1, Aumenta = 3;

                                foreach (var itHeader in lstHearder)
                                {
                                    d_buque = itHeader.d_buque;
                                    d_cliente = itHeader.d_cliente;
                                    fs_llegada = itHeader.f_llegada.Day + "/" + itHeader.f_llegada.Month + "/" + itHeader.f_llegada.Year;
                                    c_prefi = itHeader.c_prefijo;
                                    break;

                                }
                                string Mensaje = null;
                                int PageNo = 1;
                                pdfWrite.PageEvent = null;
                                Mensaje = c_prefi + " - " + d_buque + " - " + fs_llegada;
                                PageEventHelper page = new PageEventHelper(Mensaje, PageNo);
                                pdfWrite.PageEvent = page;






                                //string vv = string.Concat("Buque: ", d_buque, "Fecha: ", fs_llegada);
                                //pdfWrite.PageEvent = new PageEventHelper(vv);

                                List<ArchivoAduana> lstDetails = new List<ArchivoAduana>();
                                List<ArchivoAduana> lstPeligro = new List<ArchivoAduana>();

                                lstDetails = DetaNavieraDAL.getOpeList(item.IdReg, DBComun.Estado.verdadero).OrderBy(z => z.c_correlativo).ToList();

                                for (int j = 1; j < 7; j++)
                                {

                                    if (j <= 6)
                                    {
                                        var ordenar = DetaNavieraLINQ.orderByListado(lstDetails, DBComun.Estado.verdadero, j).OrderBy(a => a.c_correlativo).ToList();

                                        if (ordenar.Count > 0)
                                        {


                                            if (Cuenta <= 3)
                                            {
                                                genePDF(c_llegada, d_buque, pdfDoc, ROWS_START, Filas, d_cliente, fs_llegada, ordenar, j, j, c_prefi);
                                                Cuenta = Cuenta + 1;
                                            }
                                            else
                                            {
                                                genePDF(c_llegada, d_buque, pdfDoc, ROWS_START, Filas, d_cliente, fs_llegada, ordenar, j, j, c_prefi);
                                                Aumenta = Aumenta + 1;
                                            }

                                            foreach (var item_C in ordenar)
                                            {
                                                lstDetails.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                                            }
                                        }

                                    }
                                }

                            }

                            pdfDoc.NewPage();

                        }

                        pdfDoc.Close();

                        Response.ContentType = "application/pdf";

                        Response.AddHeader("content-disposition", "attachment;" +

                                                       string.Format("filename={0}.pdf", d_buque + "_" + c_llegada));

                        Response.Cache.SetCacheability(HttpCacheability.NoCache);

                        Response.Write(pdfDoc);

                        Response.End();
                    }
                }

            }
            else if (pListado == 3)
            {
                //EXPORTACION
                var queryNavi = new List<DetaNaviera>();
                int contador = 1;


                if (c_llegada.Length > 0)
                {
                    queryNavi = AlertaDANDAL.getNavierasOpeExp(DBComun.Estado.verdadero, c_llegada);

                    if (queryNavi.Count > 0)
                    {
                        string d_buque = null;

                        Document pdfDoc = new Document(PageSize.LETTER);
                        PdfWriter pdfWrite = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();

                        const int ROWS_FIRST = 1;
                        const int ROWS_START = 8;
                        int Filas = 0;

                        foreach (var item in queryNavi)
                        {
                            List<EnvioAuto> listAuto = new List<EnvioAuto>();
                            listAuto = DetaNavieraDAL.ObtenerEncAuExp(item.IdReg, DBComun.Estado.verdadero);

                            string c_navi_corto = null, c_viaje = null;


                            foreach (var detaills in listAuto)
                            {
                                c_navi_corto = detaills.c_naviera_corto;
                                c_viaje = detaills.c_voyaje;
                                break;
                            }

                            var lstHearder = (from a in EncaBuqueDAL.getHeaderLleg(DBComun.Estado.verdadero, item.c_navi, item.c_llegada)
                                              join b in AlertaDANDAL.getPrefiNavi(DBComun.Estado.verdadero).Where(a => a.d_naviera_p == c_navi_corto) on a.c_cliente equals b.c_naviera
                                              select new EncaBuque
                                              {
                                                  c_llegada = a.c_llegada,
                                                  c_buque = a.c_buque,
                                                  d_buque = a.d_buque,
                                                  c_cliente = a.c_cliente,
                                                  d_cliente = a.d_cliente,
                                                  f_llegada = a.f_llegada,
                                                  c_imo = a.c_imo,
                                                  c_prefijo = b.d_naviera_p
                                              }).ToList();


                            if (lstHearder == null)
                            {
                                lstHearder = new List<EncaBuque>();
                            }
                            List<ArchivoExport> _listaAOrdenar1 = new List<ArchivoExport>();

                            if (lstHearder.Count > 0)
                            {
                                string d_cliente = null, fs_llegada = null, c_prefi = null;
                                int Cuenta = 1, Aumenta = 3;

                                foreach (var itHeader in lstHearder)
                                {
                                    d_buque = itHeader.d_buque;
                                    d_cliente = itHeader.d_cliente;
                                    fs_llegada = itHeader.f_llegada.Day + "/" + itHeader.f_llegada.Month + "/" + itHeader.f_llegada.Year;
                                    c_prefi = itHeader.c_prefijo;
                                    break;

                                }
                                string Mensaje = null;
                                int PageNo = 1;
                                pdfWrite.PageEvent = null;
                                Mensaje = c_prefi + " - " + d_buque + " - " + fs_llegada;
                                PageEventHelper page = new PageEventHelper(Mensaje, PageNo);
                                pdfWrite.PageEvent = page;


                                //string vv = string.Concat("Buque: ", d_buque, "Fecha: ", fs_llegada);
                                //pdfWrite.PageEvent = new PageEventHelper(vv);

                                List<ArchivoExport> lstDetails = new List<ArchivoExport>();
                                List<ArchivoExport> lstOrder = new List<ArchivoExport>();

                                lstDetails = DetaNavieraDAL.getOpeListExp(item.IdReg, DBComun.Estado.verdadero).OrderBy(z => z.c_correlativo).ToList();

                                List<EstadiaConte> lstUbi = new List<EstadiaConte>();
                                lstUbi = GetContenedor();

                                if (lstDetails.Count > 0 && lstUbi != null)
                                {
                                    lstOrder = (from a in lstDetails
                                                join b in lstUbi on a.n_contenedor equals b.Contenedor into cUbica
                                                from c in cUbica.DefaultIfEmpty()
                                                select new ArchivoExport
                                                {
                                                    c_correlativo = a.c_correlativo,
                                                    IdReg = a.IdReg,
                                                    IdDeta = a.IdDeta,
                                                    n_booking = a.n_booking,
                                                    n_contenedor = a.n_contenedor,
                                                    c_tamaño = a.c_tamaño,
                                                    IdDoc = a.IdDoc,
                                                    v_peso = a.v_peso,
                                                    c_pais_trasbordo = a.c_pais_trasbordo,
                                                    c_puerto_trasbordo = a.c_puerto_trasbordo,
                                                    c_tipo_doc = a.c_tipo_doc,
                                                    n_documento = a.n_documento,
                                                    v_tara = a.v_tara,
                                                    c_imo_imd = a.c_imo_imd,
                                                    s_trafico = a.s_trafico,
                                                    s_consignatario = a.s_consignatario,
                                                    s_almacenaje = a.s_almacenaje,
                                                    s_manejo = a.s_manejo,
                                                    s_posicion = c == null ? "" : c.Sitio,
                                                    s_pe = Convert.ToDouble(a.v_peso) > 30000.00 ? "X" : "",
                                                    c_tamaño_c = a.c_tamaño_c,
                                                    n_sello = c == null ? "" : c.Marchamo,
                                                    b_estado = a.b_estado
                                                }).OrderBy(c => c.c_correlativo).ToList();
                                }
                                else
                                {
                                    lstOrder = new List<ArchivoExport>();
                                    throw new Exception("Reportar una falla en carga de información");

                                }


                                var _destinos = (from a in lstOrder
                                                 group a by a.c_puerto_trasbordo into g
                                                 select new
                                                 {
                                                     c_puerto_trasbordo = g.Key
                                                 }).OrderBy(a => a.c_puerto_trasbordo).ToList();

                                lstOrder.OrderBy(x => x.c_correlativo).ToList();
                                List<ArchivoExport> _list = new List<ArchivoExport>();

                                if (lstOrder.Count() > 0)
                                {
                                    foreach (var iteDe in _destinos)
                                    {
                                        for (int d = 1; d <= 2; d++)
                                        {
                                            if (d == 1)
                                                _listaAOrdenar1 = lstOrder.Where(f => f.s_trafico == "PATIO CEPA").ToList();
                                            else if (d == 2)
                                                _listaAOrdenar1 = lstOrder.Where(f => f.s_trafico == "TRANSBORDO").ToList();

                                            if (_listaAOrdenar1.Count > 0)
                                            {
                                                for (int j = 1; j <= 2; j++)
                                                {


                                                    var consulta = DetaNavieraLINQ.AlmacenarArchivoEx(_listaAOrdenar1, DBComun.Estado.verdadero, j);
                                                    if (consulta.Count() > 0)
                                                    {
                                                        var _trafico = consulta.Where(c => c.c_puerto_trasbordo.Equals(iteDe.c_puerto_trasbordo)).OrderBy(a => a.c_correlativo).ToList();
                                                        if (_trafico.Count > 0)
                                                        {
                                                            _list.AddRange(consulta);
                                                            Filas = _trafico.Count + ROWS_START;
                                                            // GenerarExcel2CXExp(_listaAOrdenar1, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, i, iRow, d_naviera, c_voyage, contador);
                                                            genePDFExp(c_llegada, d_buque, pdfDoc, ROWS_START, Filas, d_cliente, fs_llegada, _trafico, j, j, c_prefi, c_navi_corto, c_viaje);
                                                            Cuenta = Cuenta + 1;
                                                            contador += 1;



                                                            foreach (var item_C in _listaAOrdenar1)
                                                            {
                                                                lstOrder.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            pdfDoc.NewPage();

                        }

                        pdfDoc.Close();

                        Response.ContentType = "application/pdf";

                        Response.AddHeader("content-disposition", "attachment;" +

                        string.Format("filename={0}.pdf", "EXP_" + d_buque + "_" + c_llegada));

                        Response.Cache.SetCacheability(HttpCacheability.NoCache);

                        Response.Write(pdfDoc);

                        Response.End();
                    }
                }
            }
            else if (pListado == 4)
            {
                //EXPORTACION 
                var queryNavi = new List<DetaNaviera>();
                int contador = 1;


                if (c_llegada.Length > 0)
                {
                    queryNavi = AlertaDANDAL.getNavierasOpeExpED(DBComun.Estado.verdadero, c_llegada);

                    if (queryNavi.Count > 0)
                    {
                        string d_buque = null;

                        Document pdfDoc = new Document(PageSize.LETTER);
                        PdfWriter pdfWrite = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();

                        const int ROWS_FIRST = 1;
                        const int ROWS_START = 8;
                        int Filas = 0;

                        foreach (var item in queryNavi)
                        {
                            List<EnvioAuto> listAuto = new List<EnvioAuto>();
                            listAuto = DetaNavieraDAL.ObtenerEncAuExp(item.IdReg, DBComun.Estado.verdadero);

                            string c_navi_corto = null, c_viaje = null;


                            foreach (var detaills in listAuto)
                            {
                                c_navi_corto = detaills.c_naviera_corto;
                                c_viaje = detaills.c_voyaje;
                                break;
                            }

                            var lstHearder = (from a in EncaBuqueDAL.getHeaderLleg(DBComun.Estado.verdadero, item.c_navi, item.c_llegada)
                                              join b in AlertaDANDAL.getPrefiNavi(DBComun.Estado.verdadero).Where(a => a.d_naviera_p == c_navi_corto) on a.c_cliente equals b.c_naviera
                                              select new EncaBuque
                                              {
                                                  c_llegada = a.c_llegada,
                                                  c_buque = a.c_buque,
                                                  d_buque = a.d_buque,
                                                  c_cliente = a.c_cliente,
                                                  d_cliente = a.d_cliente,
                                                  f_llegada = a.f_llegada,
                                                  c_imo = a.c_imo,
                                                  c_prefijo = b.d_naviera_p
                                              }).ToList();


                            if (lstHearder == null)
                            {
                                lstHearder = new List<EncaBuque>();
                            }
                            List<ArchivoExport> _listaAOrdenar1 = new List<ArchivoExport>();

                            if (lstHearder.Count > 0)
                            {
                                string d_cliente = null, fs_llegada = null, c_prefi = null;
                                int Cuenta = 1, Aumenta = 3;

                                foreach (var itHeader in lstHearder)
                                {
                                    d_buque = itHeader.d_buque;
                                    d_cliente = itHeader.d_cliente;
                                    fs_llegada = itHeader.f_llegada.Day + "/" + itHeader.f_llegada.Month + "/" + itHeader.f_llegada.Year;
                                    c_prefi = itHeader.c_prefijo;
                                    break;

                                }
                                string Mensaje = null;
                                int PageNo = 1;
                                pdfWrite.PageEvent = null;
                                Mensaje = c_prefi + " - " + d_buque + " - " + fs_llegada;
                                PageEventHelper page = new PageEventHelper(Mensaje, PageNo);
                                pdfWrite.PageEvent = page;


                                //string vv = string.Concat("Buque: ", d_buque, "Fecha: ", fs_llegada);
                                //pdfWrite.PageEvent = new PageEventHelper(vv);

                                List<ArchivoExport> lstDetails = new List<ArchivoExport>();
                                List<ArchivoExport> lstOrder = new List<ArchivoExport>();

                                lstDetails = DetaNavieraDAL.getOpeListExpED(item.IdReg, DBComun.Estado.verdadero).OrderBy(z => z.c_correlativo).ToList();

                               

                                if (lstDetails.Count > 0)
                                {
                                    lstOrder = (from a in lstDetails                                                
                                                select new ArchivoExport
                                                {
                                                    c_correlativo = a.c_correlativo,
                                                    IdReg = a.IdReg,
                                                    IdDeta = a.IdDeta,
                                                    n_booking = a.n_booking,
                                                    n_contenedor = a.n_contenedor,
                                                    c_tamaño = a.c_tamaño,
                                                    IdDoc = a.IdDoc,
                                                    v_peso = a.v_peso,
                                                    c_pais_trasbordo = a.c_pais_trasbordo,
                                                    c_puerto_trasbordo = a.c_puerto_trasbordo,
                                                    c_tipo_doc = a.c_tipo_doc,
                                                    n_documento = a.n_documento,
                                                    v_tara = a.v_tara,
                                                    c_imo_imd = a.c_imo_imd,
                                                    s_trafico = a.s_trafico,
                                                    s_consignatario = a.s_consignatario,
                                                    s_almacenaje = a.s_almacenaje,
                                                    s_manejo = a.s_manejo,                                                    
                                                    s_pe = Convert.ToDouble(a.v_peso) > 30000.00 ? "X" : "",
                                                    c_tamaño_c = a.c_tamaño_c,                                                   
                                                    b_estado = a.b_estado,
                                                    f_venc_arivu = a.f_venc_arivu,
                                                    s_nom_predio = a.s_nom_predio,
                                                    s_fec_venc = a.s_fec_venc
                                                }).OrderBy(c => c.c_correlativo).ToList();
                                }
                                else
                                {
                                    lstOrder = new List<ArchivoExport>();                                    

                                }

                                if (lstOrder.Count() > 0)
                                {
                                    var _destinos = (from a in lstOrder
                                                     group a by a.c_puerto_trasbordo into g
                                                     select new
                                                     {
                                                         c_puerto_trasbordo = g.Key
                                                     }).OrderBy(a => a.c_puerto_trasbordo).ToList();

                                    var _predios = (from a in lstOrder
                                                    group a by a.s_nom_predio into g
                                                    select new
                                                    {
                                                        s_nom_predio = g.Key
                                                    }).OrderBy(a => a.s_nom_predio).ToList();

                                    lstOrder.OrderBy(x => x.c_correlativo).ToList();
                                    List<ArchivoExport> _list = new List<ArchivoExport>();

                                    if (_destinos.Count() > 0 && _predios.Count() > 0)
                                    {
                                        foreach (var itemPre in _predios)
                                        {
                                            foreach (var iteDe in _destinos)
                                            {
                                                for (int d = 1; d <= 2; d++)
                                                {
                                                    var consulta = DetaNavieraLINQ.AlmacenarArchivoEx(lstOrder, DBComun.Estado.verdadero, d);
                                                    if (consulta.Count() > 0)
                                                    {
                                                        var _trafico = consulta.Where(c => c.c_puerto_trasbordo.Equals(iteDe.c_puerto_trasbordo) && c.s_nom_predio.Equals(itemPre.s_nom_predio)).ToList();
                                                        if (_trafico.Count > 0)
                                                        {
                                                            _list.AddRange(consulta);
                                                            Filas = _trafico.Count + ROWS_START;
                                                            // GenerarExcel2CXExp(_listaAOrdenar1, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, i, iRow, d_naviera, c_voyage, contador);
                                                            genePDFExpED(c_llegada, d_buque, pdfDoc, ROWS_START, Filas, d_cliente, fs_llegada, _trafico, d, d, c_prefi, c_navi_corto, c_viaje, itemPre.s_nom_predio);
                                                            Cuenta = Cuenta + 1;
                                                            contador += 1;



                                                            foreach (var item_C in _trafico)
                                                            {
                                                                lstOrder.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        pdfDoc.NewPage();

                        pdfDoc.Close();

                        Response.ContentType = "application/pdf";

                        Response.AddHeader("content-disposition", "attachment;" +

                        string.Format("filename={0}.pdf", "EXP_ED_" + d_buque + "_" + c_llegada));

                        Response.Cache.SetCacheability(HttpCacheability.NoCache);

                        Response.Write(pdfDoc);

                        Response.End();
                    }
                    else
                    {
                        throw new Exception("No posee contenedores por embarque directo");
                    }
                }
            }
        }



        public void genePDF(string c_llegada, string d_buque, Document pdfDoc, int ROWS_START, int Filas, string d_cliente, string fs_llegada, List<ArchivoAduana> ordenar, int Cuenta, int j, string c_prefix)
        {

            string valorCabecera = null;
            switch (Cuenta)
            {
                case 1:
                    valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN LLENOS";
                    break;
                case 2:
                    valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";
                    break;
                case 3:
                    valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN REFRIGERADOS A CONECTAR";
                    break;
                case 4:
                    valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN TRANSBORDO";
                    break;
                case 5:
                    valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO LLENOS";
                    break;
                case 6:
                    valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO VACÍOS";
                    break;
                case 7:
                    valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN PELIGROSIDAD";
                    break;
                default:
                    break;
            }





            Paragraph tituloDoc = new Paragraph(d_cliente + "\n", FontFactory.GetFont("Calibri", 14, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(tituloDoc);

            Paragraph detalleDoc1 = new Paragraph(d_buque + "\n", FontFactory.GetFont("Calibri", 10, Font.BOLD));
            Paragraph detalleDoc2 = new Paragraph(c_llegada + "\n", FontFactory.GetFont("Calibri", 10, Font.BOLD));
            Paragraph detalleDoc3 = new Paragraph(fs_llegada + "\n", FontFactory.GetFont("Calibri", 10, Font.BOLD));

            pdfDoc.Add(detalleDoc1);
            pdfDoc.Add(detalleDoc2);
            pdfDoc.Add(detalleDoc3);

            Paragraph subtituloDoc = new Paragraph(valorCabecera + "\n", FontFactory.GetFont("Calibri", 12, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(subtituloDoc);

            PdfPTable table = new PdfPTable(8)
            {
                WidthPercentage = 100
            };

            float[] widths = new float[] { 1.5f, 9.5f, 3.7f, 4.5f, 2.7f, 2.3f, 5f, 6.5f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 1;
            table.SpacingBefore = 10f;
            table.SpacingAfter = 20f;
            table.TotalWidth = pdfDoc.PageSize.Width - 80; //Le damos el tamaño de la tabla
            table.LockedWidth = true;


            PdfPCell _h1 = new PdfPCell(new Phrase("No.", FontFactory.GetFont("Calibri", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = 20f
            };
            table.AddCell(_h1);

            PdfPCell _h2 = new PdfPCell(new Phrase("CONTENEDOR", FontFactory.GetFont("Calibri", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h2);

            PdfPCell _h3 = new PdfPCell(new Phrase("TIPO", FontFactory.GetFont("Calibri", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h3);


            PdfPCell _h4 = new PdfPCell(new Phrase("PESO EN KG", FontFactory.GetFont("Calibri", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h4);

            PdfPCell _h5 = new PdfPCell(new Phrase("TARA", FontFactory.GetFont("Calibri", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h5);

            PdfPCell _h6 = new PdfPCell(new Phrase("IMDG", FontFactory.GetFont("Calibri", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h6);


            PdfPCell _h7 = new PdfPCell(new Phrase("UBICACIÓN", FontFactory.GetFont("Calibri", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h7);

            PdfPCell _h8 = new PdfPCell(new Phrase("OBSERVACIONES", FontFactory.GetFont("Calibri", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h8);



            foreach (var detaLst in ordenar)
            {
                PdfPCell _col1 = new PdfPCell(new Phrase(detaLst.c_correlativo.ToString(), FontFactory.GetFont("Calibri", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col1);


                PdfPCell _col2 = new PdfPCell(new Phrase(detaLst.n_contenedor, FontFactory.GetFont("Calibri", 17)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    FixedHeight = 32f
                };
                table.AddCell(_col2);

                PdfPCell _col3 = new PdfPCell(new Phrase(detaLst.c_tamaño_c, FontFactory.GetFont("Calibri", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col3);

                PdfPCell _col4 = new PdfPCell(new Phrase(detaLst.v_peso.ToString("#,##0.00"), FontFactory.GetFont("Calibri", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col4);

                PdfPCell _col5 = new PdfPCell(new Phrase(detaLst.v_tara.ToString(), FontFactory.GetFont("Calibri", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col5);


                PdfPCell _col6 = new PdfPCell(new Phrase(detaLst.c_imo_imd, FontFactory.GetFont("Calibri", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col6);
                table.AddCell("");
                if (j != 4 && detaLst.b_condicion.Substring(0, 3) == "LCL")
                {
                    table.AddCell("LCL");
                }
                else if (j == 4 && detaLst.b_reef.Contains("SI"))
                {
                    table.AddCell("A CONECTAR");
                }
                else if (detaLst.b_shipper.Contains("SI"))
                {
                    table.AddCell("SHIPPER OWNED");
                }
                else
                {
                    table.AddCell("");
                }
            }

            pdfDoc.Add(table);
            pdfDoc.Add(new Paragraph("\n"));
            pdfDoc.Add(new Paragraph("\n"));


            pdfDoc.NewPage();



        }

        public void genePDFExpED(string c_llegada, string d_buque, Document pdfDoc, int ROWS_START, int Filas, string d_cliente, string fs_llegada, List<ArchivoExport> ordenar, int Cuenta, int j, string c_prefix, string d_naviera, string c_viaje, string predioPrin)
        {

            string puerto_destino = null;
            string ubicacion = null;

            string valorCabecera = null;


            foreach (var details in ordenar)
            {
                puerto_destino = details.c_puerto_trasbordo;
                ubicacion = details.s_trafico;
                break;
            }


            switch (Cuenta)
            {
                case 1:
                    valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN LLENOS EMBARQUE DIRECTO";
                    break;
                case 2:
                    valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN VACÍOS EMBARQUE DIRECTO";
                    //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                    break;
                case 3:
                    valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN TRANSBORDOS EMBARQUE DIRECTO";
                    //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                    break;

                default:
                    break;
            }



            //Paragraph tituloDoc = new Paragraph(d_cliente + "\n", FontFactory.GetFont("Trebuchet MS", 14, Font.BOLD))
            //{
            //    Alignment = Element.ALIGN_CENTER
            //};
            //pdfDoc.Add(tituloDoc);



            PdfPTable tableEnca = new PdfPTable(2)
            {
                WidthPercentage = 50
            };

            tableEnca.DefaultCell.Border = Rectangle.NO_BORDER;
            tableEnca.DefaultCell.BorderColor = BaseColor.WHITE;
            tableEnca.DefaultCell.BorderColorBottom = BaseColor.BLACK;
            float[] widthsEnca = new float[] { 5f, 10f, };
            tableEnca.SetWidths(widthsEnca);
            tableEnca.HorizontalAlignment = Element.ALIGN_CENTER;
            tableEnca.SpacingBefore = 5f;
            tableEnca.SpacingAfter = 5f;
            tableEnca.TotalWidth = pdfDoc.PageSize.Width - 80; //Le damos el tamaño de la tabla
            tableEnca.LockedWidth = true;


            PdfPCell _e1 = new PdfPCell(new Phrase("AGENCIA", FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e2 = new PdfPCell(new Phrase(d_naviera, FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };
            PdfPCell _e3 = new PdfPCell(new Phrase("VAPOR - VIAJE", FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e4 = new PdfPCell(new Phrase(d_buque + " - " + c_viaje, FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };
            PdfPCell _e5 = new PdfPCell(new Phrase("DESTINO", FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e6 = new PdfPCell(new Phrase(puerto_destino, FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };
            PdfPCell _e7 = new PdfPCell(new Phrase("UBICACION", FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e8 = new PdfPCell(new Phrase("EMBARQUE DIRECTO" + " - (" + predioPrin.ToUpper() + ")", FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };
            PdfPCell _e9 = new PdfPCell(new Phrase("FECHA DE ARRIBO", FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e10 = new PdfPCell(new Phrase(fs_llegada, FontFactory.GetFont("Trebuchet MS", 11, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };


            tableEnca.AddCell(_e1);
            tableEnca.AddCell(_e2);
            tableEnca.AddCell(_e3);
            tableEnca.AddCell(_e4);
            tableEnca.AddCell(_e5);
            tableEnca.AddCell(_e6);
            tableEnca.AddCell(_e7);
            tableEnca.AddCell(_e8);
            tableEnca.AddCell(_e9);
            tableEnca.AddCell(_e10);


            pdfDoc.Add(tableEnca);
            //pdfDoc.Add(new Paragraph("\n"));


            Paragraph subtituloDoc = new Paragraph(valorCabecera + "\n", FontFactory.GetFont("Trebuchet MS", 12, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER
            };
            subtituloDoc.SpacingBefore = 6;
            pdfDoc.Add(subtituloDoc);

            PdfPTable table = new PdfPTable(8)
            {
                WidthPercentage = 100
            };

            float[] widths = new float[] { 1.9f, 8.9f, 3.9f, 3.5f, 4.1f, 6.3f, 4.1f, 7.1f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 1;
            table.SpacingBefore = 5f;
            table.SpacingAfter = 5f;
            table.TotalWidth = pdfDoc.PageSize.Width - 80; //Le damos el tamaño de la tabla
            table.LockedWidth = true;


            PdfPCell _h1 = new PdfPCell(new Phrase("No.", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = 17f
            };
            table.AddCell(_h1);

            PdfPCell _h2 = new PdfPCell(new Phrase("CONTENEDOR", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h2);

            PdfPCell _h3 = new PdfPCell(new Phrase("TAMAÑO", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h3);


            PdfPCell _h4 = new PdfPCell(new Phrase("TARA", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h4);

            PdfPCell _h5 = new PdfPCell(new Phrase("ARIVU", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h5);

            PdfPCell _h6 = new PdfPCell(new Phrase("FECHA VENCIMIENTO", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h6);

            PdfPCell _h7 = new PdfPCell(new Phrase("POSICIÓN", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h7);


            PdfPCell _h9 = new PdfPCell(new Phrase("OBSERVACIONES", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h9);


            foreach (var detaLst in ordenar)
            {
                PdfPCell _col1 = new PdfPCell(new Phrase(detaLst.c_correlativo.ToString(), FontFactory.GetFont("Trebuchet MS", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col1);


                PdfPCell _col2 = new PdfPCell(new Phrase(detaLst.n_contenedor, FontFactory.GetFont("Trebuchet MS", 15)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    FixedHeight = 42f
                };
                table.AddCell(_col2);

                PdfPCell _col3 = new PdfPCell(new Phrase(detaLst.c_tamaño_c, FontFactory.GetFont("Trebuchet MS", 11)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col3);

                PdfPCell _col4 = new PdfPCell(new Phrase(detaLst.v_tara.ToString("#,##0"), FontFactory.GetFont("Trebuchet MS", 11)))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col4);

                PdfPCell _col5 = new PdfPCell(new Phrase(detaLst.n_documento.ToString(), FontFactory.GetFont("Trebuchet MS", 11)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col5);


                PdfPCell _col6 = new PdfPCell(new Phrase(detaLst.s_fec_venc, FontFactory.GetFont("Trebuchet MS", 11)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col6);

                PdfPCell _col7 = new PdfPCell(new Phrase("", FontFactory.GetFont("Trebuchet MS", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col7);
                                
                if (detaLst.c_imo_imd != "")
                    table.AddCell("ADV. IMDG CLASE: " + detaLst.c_imo_imd);
                else if (detaLst.b_shipper == "SI")
                    table.AddCell("SHIPPER OWNED");
                else
                    table.AddCell("");

            }

            pdfDoc.Add(table);
            pdfDoc.Add(new Paragraph("\n"));
            pdfDoc.Add(new Paragraph("\n"));


            pdfDoc.NewPage();



        }

        public void genePDFExp(string c_llegada, string d_buque, Document pdfDoc, int ROWS_START, int Filas, string d_cliente, string fs_llegada, List<ArchivoExport> ordenar, int Cuenta, int j, string c_prefix, string d_naviera, string c_viaje)
        {

            string puerto_destino = null;
            string ubicacion = null;

            string valorCabecera = null;


            foreach (var details in ordenar)
            {
                puerto_destino = details.c_puerto_trasbordo;
                ubicacion = details.s_trafico;
                break;
            }



            switch (Cuenta)
            {
                case 1:
                    valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN LLENOS " + ubicacion;
                    break;
                case 2:
                    valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN VACÍOS " + ubicacion;
                    //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                    break;
                case 3:
                    valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN TRANSBORDOS";
                    //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                    break;

                default:
                    break;
            }



            Paragraph tituloDoc = new Paragraph(d_cliente + "\n", FontFactory.GetFont("Trebuchet MS", 14, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER
            };
            pdfDoc.Add(tituloDoc);



            PdfPTable tableEnca = new PdfPTable(2)
            {
                WidthPercentage = 50
            };

            tableEnca.DefaultCell.Border = Rectangle.NO_BORDER;
            tableEnca.DefaultCell.BorderColor = BaseColor.WHITE;
            tableEnca.DefaultCell.BorderColorBottom = BaseColor.BLACK;
            float[] widthsEnca = new float[] { 5f, 10f, };
            tableEnca.SetWidths(widthsEnca);
            tableEnca.HorizontalAlignment = Element.ALIGN_CENTER;
            tableEnca.SpacingBefore = 5f;
            tableEnca.SpacingAfter = 5f;
            tableEnca.TotalWidth = pdfDoc.PageSize.Width - 80; //Le damos el tamaño de la tabla
            tableEnca.LockedWidth = true;


            PdfPCell _e1 = new PdfPCell(new Phrase("AGENCIA", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e2 = new PdfPCell(new Phrase(d_naviera, FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };
            PdfPCell _e3 = new PdfPCell(new Phrase("VAPOR - VIAJE", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e4 = new PdfPCell(new Phrase(d_buque + " - " + c_viaje, FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };
            PdfPCell _e5 = new PdfPCell(new Phrase("DESTINO", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e6 = new PdfPCell(new Phrase(puerto_destino, FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };
            PdfPCell _e7 = new PdfPCell(new Phrase("UBICACION", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e8 = new PdfPCell(new Phrase(ubicacion, FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };
            PdfPCell _e9 = new PdfPCell(new Phrase("FECHA DE ARRIBO", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0
            };
            PdfPCell _e10 = new PdfPCell(new Phrase(fs_llegada, FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = 0,
                BorderColorBottom = BaseColor.BLACK,
                BorderWidthBottom = 1
            };


            tableEnca.AddCell(_e1);
            tableEnca.AddCell(_e2);
            tableEnca.AddCell(_e3);
            tableEnca.AddCell(_e4);
            tableEnca.AddCell(_e5);
            tableEnca.AddCell(_e6);
            tableEnca.AddCell(_e7);
            tableEnca.AddCell(_e8);
            tableEnca.AddCell(_e9);
            tableEnca.AddCell(_e10);


            pdfDoc.Add(tableEnca);
            //pdfDoc.Add(new Paragraph("\n"));


            Paragraph subtituloDoc = new Paragraph(valorCabecera + "\n", FontFactory.GetFont("Trebuchet MS", 12, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER
            };
            subtituloDoc.SpacingBefore = 5;
            pdfDoc.Add(subtituloDoc);

            PdfPTable table = new PdfPTable(9)
            {
                WidthPercentage = 100
            };

            float[] widths = new float[] { 1.9f, 9.5f, 3.9f, 4.5f, 2.7f, 5.3f, 5.1f, 2.5f, 6.5f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 1;
            table.SpacingBefore = 5f;
            table.SpacingAfter = 5f;
            table.TotalWidth = pdfDoc.PageSize.Width - 80; //Le damos el tamaño de la tabla
            table.LockedWidth = true;


            PdfPCell _h1 = new PdfPCell(new Phrase("No.", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = 17f
            };
            table.AddCell(_h1);

            PdfPCell _h2 = new PdfPCell(new Phrase("CONTENEDOR", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h2);

            PdfPCell _h3 = new PdfPCell(new Phrase("TAMAÑO", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h3);


            PdfPCell _h4 = new PdfPCell(new Phrase("PESO", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h4);

            PdfPCell _h5 = new PdfPCell(new Phrase("TARA", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h5);

            PdfPCell _h6 = new PdfPCell(new Phrase("POSICION", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h6);

            PdfPCell _h7 = new PdfPCell(new Phrase("MARCHAMO", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h7);


            PdfPCell _h8 = new PdfPCell(new Phrase("PE", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h8);

            PdfPCell _h9 = new PdfPCell(new Phrase("OBSERVACIONES", FontFactory.GetFont("Trebuchet MS", 10, Font.BOLD)))
            {
                BackgroundColor = ExtendedColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(_h9);


            foreach (var detaLst in ordenar)
            {
                PdfPCell _col1 = new PdfPCell(new Phrase(detaLst.c_correlativo.ToString(), FontFactory.GetFont("Trebuchet MS", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col1);


                PdfPCell _col2 = new PdfPCell(new Phrase(detaLst.n_contenedor, FontFactory.GetFont("Trebuchet MS", 15)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    FixedHeight = 42f
                };
                table.AddCell(_col2);

                PdfPCell _col3 = new PdfPCell(new Phrase(detaLst.c_tamaño_c, FontFactory.GetFont("Trebuchet MS", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col3);

                PdfPCell _col4 = new PdfPCell(new Phrase(detaLst.v_peso.ToString("#,##0.00"), FontFactory.GetFont("Trebuchet MS", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col4);

                PdfPCell _col5 = new PdfPCell(new Phrase(detaLst.v_tara.ToString(), FontFactory.GetFont("Trebuchet MS", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col5);


                PdfPCell _col6 = new PdfPCell(new Phrase(detaLst.s_posicion, FontFactory.GetFont("Trebuchet MS", 9)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_TOP
                };
                table.AddCell(_col6);

                PdfPCell _col7 = new PdfPCell(new Phrase(detaLst.n_sello, FontFactory.GetFont("Trebuchet MS", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col7);

                //AA
                PdfPCell _col8 = new PdfPCell(new Phrase(detaLst.s_pe, FontFactory.GetFont("Trebuchet MS", 10)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                table.AddCell(_col8);

                if (detaLst.c_imo_imd != "")
                    table.AddCell("ADV. IMDG CLASE: " + detaLst.c_imo_imd);
                else if (detaLst.b_shipper == "SI")
                    table.AddCell("SHIPPER OWNED");
                else
                    table.AddCell("");

            }

            pdfDoc.Add(table);
            pdfDoc.Add(new Paragraph("\n"));
            pdfDoc.Add(new Paragraph("\n"));


            pdfDoc.NewPage();



        }

        public void RegisterPostBackControl()
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                Button lnkFull = row.FindControl("lnkOpe") as Button;
                ScriptManager current = ScriptManager.GetCurrent(Page);
                if (current != null)
                {
                    current.RegisterPostBackControl(lnkFull);
                }
            }
        }

        public static List<EstadiaConte> GetContenedor()
        {
            List<EstadiaConte> _contenedores = new List<EstadiaConte>();
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
            Procedure proceso = new Procedure();
            proceso.NBase = "CONTENEDORES";
            proceso.Procedimiento = "Sql_ubi_conte"; // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
            proceso.Parametros = new List<Parametros>();
            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static List<EstadiaConte> Conectar(List<EstadiaConte> _contenedores, string apiUrl)
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
                        _contenedores = tabla.ToList<EstadiaConte>();
                    }
                }
            }
            return _contenedores;
        }
    }
}


public class PageEventHelper : PdfPageEventHelper
{
    PdfContentByte cb;
    PdfTemplate template;
    BaseFont bf = null;

    public string Mensaje { get; set; }
    public int PageNum { get; set; }

    public PageEventHelper(string csMensaje, int PageN)
    {
        Mensaje = csMensaje;
        PageNum = PageN;
    }


    //public override void OnOpenDocument(PdfWriter writer, Document document)
    //{
    //    cb = writer.DirectContent;
    //    template = cb.CreateTemplate(50, 50);
    //    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    //}

    public override void OnStartPage(PdfWriter writer, Document document)
    {
        base.OnStartPage(writer, document);
        PageNum = PageNum + 1;
    }

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        cb = writer.DirectContent;
        template = cb.CreateTemplate(50, 30);
        base.OnEndPage(writer, document);
        int pageN = PageNum;
        string text = Mensaje + " - Página " + pageN.ToString();
        float len = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED).GetWidthPoint(text, 6);

        iTextSharp.text.Rectangle pageSize = document.PageSize;



        cb.SetRGBColorFill(0, 0, 0);

        cb.BeginText();
        cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
        cb.SetTextMatrix(document.LeftMargin, pageSize.GetBottom(document.BottomMargin));
        cb.ShowText(text);

        cb.EndText();

        cb.AddTemplate(template, document.LeftMargin + len, pageSize.GetBottom(document.BottomMargin));
    }

    //public override void OnCloseDocument(PdfWriter writer, Document document)
    //{
    //    base.OnCloseDocument(writer, document);

    //    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    //    template.BeginText();
    //    template.SetFontAndSize(bf, 6);
    //    template.SetTextMatrix(0, 0);
    //    template.ShowText((PageNum - 1).ToString());
    //    template.EndText();


    //}
}








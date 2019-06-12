using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
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
                             join b in EncaBuqueDAL.ObtenerBuquesJoinC(DBComun.Estado.verdadero) on new { c_llegada = a.c_llegada, c_cliente = a.c_cliente } equals new { c_llegada = b.c_llegada, c_cliente = b.c_cliente }
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
            else
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

            float[] widths = new float[] { 1.5f, 8.5f, 3.7f, 4.5f, 3f, 2f, 5f, 6.5f };
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

}






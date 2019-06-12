using System;
using System.Collections.Generic;

using CEPA.CCO.Entidades;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
//using Microsoft.Office.Interop.Excel;

using cxExcel = ClosedXML.Excel;

namespace CEPA.CCO.Linq
{
    public class ListOIRSA
    {
        public int IdDoc { get; set; }

        string fullOIRSA1 = System.Configuration.ConfigurationManager.AppSettings["fullOIRSA1"];
        string fullOIRSA2 = System.Configuration.ConfigurationManager.AppSettings["fullOIRSA2"];
        

        int Hoja = 3;
        List<string> pRutas = new List<string>();

        public void GenerarAplicacionCX(List<ArchivoAduana> pLista)
        {


            //const int ROWS_FIRST = 1;
            //const int ROWS_START = 8;
            //int Filas = 0;
            //int iRow = 1;
            //pRutas = null;


            //System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");
            //System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";



            //var oWB = new cxExcel.XLWorkbook();
            //var oWB1 = new cxExcel.XLWorkbook();

            //List<ArchivoAduana> _list = new List<ArchivoAduana>();
            //string _f2save = string.Format("{0}{1}", fullPath, "ACO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
            //string _f4save = string.Format("{0}{1}", fullPath, "ACO1_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
            //string _f3save = string.Format("{0}{1}", fullPDF, "ACO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".pdf");
            //string _f5save = string.Format("{0}{1}", fullPath, "ACO2_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
            //string _f6save = string.Format("{0}{1}", fullPath, "ACO3_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");


            //string ruta = null;

            //try
            //{


            //    List<EnvioAuto> listAuto = new List<EnvioAuto>();
            //    if (IdDoc > 0)
            //        listAuto = DetaNavieraDAL.ObtenerDocAu(IdDoc, DBComun.Estado.falso);

            //    string c_navi_corto = null, c_viaje = null;
            //    int n_manifiesto = 0;

            //    foreach (var item in listAuto)
            //    {
            //        c_navi_corto = item.c_naviera_corto;
            //        c_viaje = item.c_voyaje;
            //        n_manifiesto = item.n_manifiesto;
            //        break;
            //    }


            //    string fechaC = f_llegada.Day + "/" + f_llegada.Month + "/" + f_llegada.Year;

            //    int Aumenta = 3;
            //    int Cuenta = 1;
            //    for (int j = 1; j < 8; j++)
            //    {
            //        if (j == 1)
            //        {
            //            var ordenar1 = DetaNavieraLINQ.EnvioArchivo(pLista, DBComun.Estado.falso, 7);
            //            if (ordenar1.Count > 0)
            //                _list.AddRange(ordenar1);
            //        }

            //        if (j <= 6)
            //        {
            //            var ordenar = DetaNavieraLINQ.EnvioArchivo(pLista, DBComun.Estado.falso, j);

            //            if (ordenar.Count > 0)
            //            {

            //                if (Cuenta <= 3)
            //                {

            //                    Filas = ordenar.Count + ROWS_START;
            //                    GenerarExcelCX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, j, iRow, d_naviera, false, n_manifiesto, c_viaje);
            //                    GenerarExcel2CX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB1, fechaC, j, iRow, d_naviera);
            //                    Cuenta = Cuenta + 1;
            //                }
            //                else
            //                {
            //                    Filas = ordenar.Count + ROWS_START;
            //                    GenerarExcelCX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, j, iRow, d_naviera, false, n_manifiesto, c_viaje);
            //                    GenerarExcel2CX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB1, fechaC, j, iRow, d_naviera);
            //                    Aumenta = Aumenta + 1;
            //                }

            //                foreach (var item_C in ordenar)
            //                {
            //                    pLista.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
            //                }
            //            }
            //        }
            //    }


            //    oWB.SaveAs(_f2save);
            //    oWB1.SaveAs(_f4save);



            //    //Load Workbook
            //    string _ruta = "@" + _f2save;



            //    ConvertExcelToPdf(_f4save, _f3save);


            //    if (pRutas == null)
            //        pRutas = new List<string>();

            //    pRutas.Add(_f2save);
            //    pRutas.Add(_f3save);

            //    if (_list.Count > 0)
            //        ruta = GenerarPeligrosidadCX(_list, c_cliente, d_naviera, c_llegada, IdReg, d_buque, f_llegada, pValor, pCantidad, n_manifiesto, c_viaje);

            //    if (ruta != null && ruta != "")
            //        pRutas.Add(ruta);

            //    List<string> pPDF = new List<string>();

            //    EnviarCorreo(pRutas, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

            //    pPDF.Add(_f3save);

            //    EnviarCorreoCO(pPDF, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

            //    System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

            //}
            //catch (Exception theException)
            //{
            //    String errorMessage;
            //    errorMessage = "Error: ";
            //    errorMessage = String.Concat(errorMessage, theException.Message);
            //    errorMessage = String.Concat(errorMessage, " Line: ");
            //    errorMessage = String.Concat(errorMessage, theException.Source);
            //    throw new Exception(errorMessage);
            //}
            //finally
            //{

            //}
        }

        private static void GenerarExcel2CX(List<ArchivoAduana> pAduana, string c_llegada, int ROWS_FIRST, int ROWS_START, int Filas, string d_buque, int cuenta, cxExcel.XLWorkbook oWB, /*Microsoft.Office.Interop.Excel._Worksheet oSheet, Microsoft.Office.Interop.Excel.Range oRng, */ string _Fecha, int pValor, int iRow, string d_naviera)
        {

            string nombreSheet = null;
            string valorCabecera = null;
            try
            {
                switch (pValor)
                {
                    case 1:
                        nombreSheet = "LLENOS";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN LLENOS";
                        break;
                    case 2:
                        nombreSheet = "VACIOS";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";
                        //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                        break;
                    case 3:
                        nombreSheet = "REEF";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN REFRIGERADOS A CONECTAR ";
                        /*oSheet.Name = "REEF";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN REFRIGERADOS A CONECTAR ";*/
                        break;
                    case 4:
                        nombreSheet = "TRASBORDO";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN TRASBORDO";
                        break;
                    case 5:
                        nombreSheet = "RET_DIR";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO LLENOS";
                        break;
                    case 6:
                        nombreSheet = "RET_DIR_VACIO";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO VACÍOS";
                        break;
                    case 7:
                        nombreSheet = "CONTE_PELIGROSIDAD";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN PELIGROSIDAD";
                        break;
                    default:
                        break;
                }

                var oSheet = oWB.Worksheets.Add(nombreSheet);

                oSheet.Cell(6, 1).Value = valorCabecera;
                string _rango = null;


                _rango = "H";

                oSheet.Range("A6", string.Format("{0}6", _rango)).Merge();
                oSheet.Range("A6", string.Format("{0}6", _rango)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A6", string.Format("{0}6", _rango)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Range("A6", string.Format("{0}6", _rango)).Style.Font.FontSize = 18;
                //Add table headers going cell by cell.
                oSheet.Cell(ROWS_FIRST, 1).Value = d_naviera;
                oSheet.Range("A1", string.Format("{0}1", _rango)).Merge();
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Font.FontSize = 28;
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.WrapText = true;
                oSheet.Row(1).Height = 63;







                //oSheet.Cell(4, 5).Value = Thread.CurrentThread.CurrentCulture.EnglishName;

                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.Cell(ROWS_START, 1).Value = "No.";
                oSheet.Cell(ROWS_START, 2).Value = "CONTENEDOR";
                oSheet.Cell(ROWS_START, 3).Value = "TIPO";
                oSheet.Cell(ROWS_START, 4).Value = "PESO EN KG";
                oSheet.Cell(ROWS_START, 5).Value = "TARA";
                oSheet.Cell(ROWS_START, 6).Value = "IMDG";
                oSheet.Cell(ROWS_START, 7).Value = "UBICACIÓN";
                oSheet.Cell(ROWS_START, 8).Value = "OBSERVACIONES";

                oSheet.Range("E8", "E8").Style.Alignment.WrapText = true;
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Font.Bold = true;
                oSheet.Range("A6", string.Format("{0}6", _rango)).Style.Font.Bold = true;
                oSheet.Range("A8", string.Format("{0}8", _rango)).Style.Font.Bold = true;
                oSheet.Range("A8", string.Format("{0}8", _rango)).Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                oSheet.Range("A8", string.Format("{0}8", _rango)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A6", string.Format("{0}8", _rango)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Range("A9", string.Concat("C", Filas)).Style.NumberFormat.SetFormat("@");
                oSheet.Range("D9", string.Concat("D", Filas)).Style.NumberFormat.SetFormat("0.00");
                oSheet.Range("E9", string.Concat("E", Filas)).Style.NumberFormat.SetFormat("0");
                oSheet.Range("F9", string.Format("{0}{1}", _rango, Filas)).Style.NumberFormat.SetFormat("@");
                oSheet.Range("A9", string.Concat("B", Filas)).Style.Font.FontSize = 14;

                //oRng = oSheet.get_Range("A1", string.Concat("D", Filas));
                //oRng.EntireColumn.AutoFit();
                oSheet.Range("H9", string.Concat("H", Filas)).Style.Font.Bold = true;
                oSheet.Range("H9", string.Concat("H", Filas)).Style.Font.FontSize = 12;
                oSheet.Range("H9", string.Format("H{0}", Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Justify);
                oSheet.Range("H9", string.Format("H{0}", Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Left);



                var oRng = oSheet.Range("A8", string.Concat(string.Format("{0}", _rango), Filas));

                oRng.Style.Border.DiagonalDown = false;
                oRng.Style.Border.DiagonalUp = false;
                /* oRng.Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                 oRng.Style.Border.LeftBorder = cxExcel.XLBorderStyleValues.Medium;
                 oRng.Style.Border.TopBorder = cxExcel.XLBorderStyleValues.Medium;
                 oRng.Style.Border.RightBorder = cxExcel.XLBorderStyleValues.Medium;*/
                oRng.Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                oRng.Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;


                oRng.Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oRng.Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Bottom;
                //oRng.Style.Border.SetOutsideBorder(cxExcel.XLBorderStyleValues.Medium);
                oRng.Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                oRng.Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);

                oRng = oSheet.Range("D9", string.Format("E{0}", Filas));
                oRng.Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oRng.Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Bottom;

                oSheet.Range("H9", string.Concat("H", Filas)).Style.Font.Bold = true;
                oSheet.Range("H9", string.Concat("H", Filas)).Style.Font.FontSize = 12;
                oSheet.Range("H9", string.Format("H{0}", Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("H9", string.Format("H{0}", Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Left);



                /*oRng = oSheet.Range("A8", string.Format("G{0}", Filas));
                oRng.Style.Font.FontSize = 12;
                //oSheet.RowHeight = 38;*/


                oSheet.Column(1).Width = 6;
                oSheet.Column(2).Width = 29;
                oSheet.Column(3).Width = 12;
                oSheet.Column(4).Width = 15;
                oSheet.Column(5).Width = 8;
                oSheet.Column(6).Width = 6;
                oSheet.Column(7).Width = 25;
                oSheet.Column(8).Width = 44;

                oSheet.PageSetup.Footer.Left.AddText(string.Format("&B Buque : {0} Fecha : {1} &B", d_buque, _Fecha));
                oSheet.PageSetup.Footer.Center.AddText("Página ", cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.PageNumber, cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(" de ", cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.NumberOfPages, cxExcel.XLHFOccurrence.AllPages);



                foreach (var item in pAduana)
                {
                    int iCurrent = ROWS_START + iRow;
                    oSheet.Row(iCurrent).Height = 40;
                    oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                    oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                    oSheet.Cell(iCurrent, 3).Value = item.c_tamaño_c;
                    oSheet.Cell(iCurrent, 4).Value = item.v_peso;
                    oSheet.Cell(iCurrent, 5).Value = item.v_tara;
                    oSheet.Cell(iCurrent, 6).Value = item.c_imo_imd;
                    oSheet.Cell(iCurrent, 7).Value = "";
                    if (pValor != 4 && item.b_condicion.Substring(0, 3) == "LCL")
                        oSheet.Cell(iCurrent, 8).Value = "LCL";
                    else if (pValor == 4 && item.b_reef.Contains("SI"))
                        oSheet.Cell(iCurrent, 8).Value = "A CONECTAR";
                    else
                        oSheet.Cell(iCurrent, 8).Value = "";





                    iRow = iRow + 1;
                }


                oSheet.PageSetup.PageOrientation = cxExcel.XLPageOrientation.Portrait;
                oSheet.PageSetup.AdjustTo(59);
                oSheet.PageSetup.PaperSize = cxExcel.XLPaperSize.LetterPaper;
                oSheet.PageSetup.VerticalDpi = 600;
                oSheet.PageSetup.HorizontalDpi = 600;
                oSheet.PageSetup.Margins.Top = 0.3;
                oSheet.PageSetup.Margins.Header = 0.40;
                oSheet.PageSetup.Margins.Footer = 0.20;


                oSheet.Row(ROWS_START).Height = 42;
                var oRng2 = oSheet.Range("A8", string.Concat(string.Format("{0}", _rango), Filas));
                oRng2.Style.Font.FontSize = 14;

                var oRng1 = oSheet.Range("A9", string.Concat("B", Filas));
                oRng1.Style.Font.FontSize = 18;

                var oRng3 = oSheet.Range("B9", string.Concat("B", Filas));
                oRng3.Style.Font.FontSize = 24;

                oSheet.Cell(3, 1).Value = d_buque;
                oSheet.Range("A3", "B3").Merge();
                oSheet.Range("A3", "B3").Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("A3", "B3").Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                oSheet.Cell(4, 1).Value = c_llegada;
                oSheet.Range("A4", "B4").Merge();
                oSheet.Range("A4", "B4").Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("A4", "B4").Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                oSheet.Cell(5, 1).Value = _Fecha;
                oSheet.Range("A5", "B5").Merge();
                oSheet.Range("A5", "B5").Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("A5", "B5").Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                oSheet.Range("A4", "A4").Style.NumberFormat.SetFormat("0.0000");
                oSheet.Range("A5", "A5").Style.NumberFormat.SetFormat("dd/mm/yyyy");
                oSheet.Range("A3", "A5").Style.Font.Bold = true;
                oSheet.Range("A3", "A5").Style.Font.FontSize = 13;






            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    }
}

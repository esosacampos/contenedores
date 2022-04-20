using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Web;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
//using Microsoft.Office.Interop.Excel;

using cxExcel = ClosedXML.Excel;

using msExcel = Microsoft.Office.Interop.Excel;

using Office = Microsoft.Office.Core;
using VBIDE = Microsoft.Vbe.Interop;
using Microsoft.Vbe.Interop;
using System.Threading;
//using ClosedXML.Excel;

namespace CEPA.CCO.Linq
{
    public class NotiAutorizadosSer
    {
        public int IdDoc { get; set; }
        public string a_manifiesto { get; set; }

        string fullPath = System.Configuration.ConfigurationManager.AppSettings["fullPath"];
        string fullPDF = System.Configuration.ConfigurationManager.AppSettings["fullPDF"];
        string fullIMDG = System.Configuration.ConfigurationManager.AppSettings["fullIMDG"];
        string fullPathC = System.Configuration.ConfigurationManager.AppSettings["fullPathC"];

        string fullPathCO = System.Configuration.ConfigurationManager.AppSettings["fullPathCO"];

        string fullEXP_PREV = System.Configuration.ConfigurationManager.AppSettings["fullExpPre"];
        string fullEXP_AUTO = System.Configuration.ConfigurationManager.AppSettings["fullExpAut"];

        string fullEXP_ARIVU = System.Configuration.ConfigurationManager.AppSettings["fullExpArivu"];



        int Hoja = 3;
        List<string> pRutas = new List<string>();

        public void GenerarAplicacionCX(List<ArchivoAduana> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque,
              DateTime f_llegada, int pValor, int pCantidad, List<DetaNaviera> pCancelados, int b_sidunea)
        {


            const int ROWS_FIRST = 1;
            const int ROWS_START = 8;
            int Filas = 0;
            int iRow = 1;
            pRutas = null;


            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";



            var oWB = new cxExcel.XLWorkbook();
            var oWB1 = new cxExcel.XLWorkbook();

            var oWB2 = new cxExcel.XLWorkbook();

            List<ArchivoAduana> _list = new List<ArchivoAduana>();
            string _f2save = string.Format("{0}{1}", fullPath, "ACO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
            string _f4save = string.Format("{0}{1}", fullPath, "ACO1_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
            string _f3save = string.Format("{0}{1}", fullPDF, "ACO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".pdf");
            string _f5save = string.Format("{0}{1}", fullPath, "ACO2_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
            string _f6save = string.Format("{0}{1}", fullPath, "ACO3_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");




            string ruta = null;

            try
            {


                List<EnvioAuto> listAuto = new List<EnvioAuto>();
                if (IdDoc > 0)
                    listAuto = DetaNavieraDAL.ObtenerDocAu(IdDoc, DBComun.Estado.falso);

                string c_navi_corto = null, c_viaje = null;
                int n_manifiesto = 0;

                foreach (var item in listAuto)
                {
                    c_navi_corto = item.c_naviera_corto;
                    c_viaje = item.c_voyaje;
                    n_manifiesto = item.n_manifiesto;
                    a_manifiesto = item.an_manifiesto;
                    break;
                }


                string fechaC = f_llegada.Day + "/" + f_llegada.Month + "/" + f_llegada.Year;

                int Aumenta = 3;
                int Cuenta = 1;

                List<string> pSize = new List<string>();

                if (pLista.Count > 0)
                {
                    foreach (var sizeV in pLista)
                    {
                        string validacionSize = DetaNavieraDAL.validSize(sizeV.n_contenedor, DBComun.Estado.falso);

                        if (validacionSize.Contains("ISOType"))
                            pSize.Add(validacionSize);
                    }
                }


                for (int j = 1; j < 8; j++)
                {
                    if (j == 1)
                    {
                        var ordenar1 = DetaNavieraLINQ.EnvioArchivo(pLista, DBComun.Estado.falso, 7);
                        if (ordenar1.Count > 0)
                            _list.AddRange(ordenar1);
                    }

                    if (j <= 6)
                    {
                        var ordenar = DetaNavieraLINQ.EnvioArchivo(pLista, DBComun.Estado.falso, j);

                        if (ordenar.Count > 0)
                        {

                            if (Cuenta <= 3)
                            {
                                Filas = ordenar.Count + ROWS_START;
                                GenerarExcelCX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, j, iRow, d_naviera, false, n_manifiesto, c_viaje);
                                GenerarExcel2CX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB1, fechaC, j, iRow, d_naviera, b_sidunea);
                                Cuenta = Cuenta + 1;
                            }
                            else
                            {
                                Filas = ordenar.Count + ROWS_START;
                                GenerarExcelCX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, j, iRow, d_naviera, false, n_manifiesto, c_viaje);
                                GenerarExcel2CX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB1, fechaC, j, iRow, d_naviera, b_sidunea);
                                Aumenta = Aumenta + 1;
                            }

                            foreach (var item_C in ordenar)
                            {
                                pLista.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                            }
                        }
                    }
                }


                oWB.SaveAs(_f2save);
                oWB1.SaveAs(_f4save);



                //Load Workbook
                string _ruta = "@" + _f2save;



                ConvertExcelToPdf(_f4save, _f3save);


                if (pRutas == null)
                    pRutas = new List<string>();

                pRutas.Add(_f2save);
                pRutas.Add(_f3save);

                if (_list.Count > 0)
                {
                    ruta = GenerarPeligrosidadCX(_list, c_cliente, d_naviera, c_llegada, IdReg, d_buque, f_llegada, pValor, pCantidad, n_manifiesto, c_viaje);
                }



                //COTECNA 
                List<ArchivoAduana> pCotecna = new List<ArchivoAduana>();
                string rutCotec = null;

                pCotecna = DetaNavieraDAL.getCotecnaLst(IdReg, IdDoc, DBComun.Estado.falso);

                if (pCotecna.Count > 0)
                    rutCotec = generarCOTECNA(pCotecna, c_cliente, d_naviera, c_llegada, IdReg, d_buque, f_llegada, pValor, pCantidad, n_manifiesto, c_viaje, c_navi_corto);

                List<string> pCotecLst = new List<string>();

                pCotecLst.Add(rutCotec);

                //COTECNA

                if (ruta != null && ruta != "")
                    pRutas.Add(ruta);

                //List<string> pPDF = new List<string>();                

                EnviarCorreo(pRutas, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, string.Concat(a_manifiesto, "-", n_manifiesto), pSize);

                //pPDF.Add(_f3save);

                //EnviarCorreoCO(pPDF, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

                EnviarCorreoCOTEC(pCotecLst, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto, a_manifiesto);

                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);
                throw new Exception(errorMessage);
            }
            finally
            {

            }
        }


        public Microsoft.Office.Interop.Excel._Worksheet EliminarHojas(Microsoft.Office.Interop.Excel._Application oXL, Microsoft.Office.Interop.Excel._Workbook oWB, Microsoft.Office.Interop.Excel._Worksheet oSheet, bool pValor)
        {
            int Hojas = 0;
            if (pValor == true)
                Hojas = oWB.Worksheets.Count;
            else
                Hojas = Hoja;

            for (int i = 1; i <= oWB.Worksheets.Count; i++)
            {
                if (i > oWB.Worksheets.Count)
                    oSheet = (Microsoft.Office.Interop.Excel._Worksheet)(oWB.Worksheets.Item[i - 1]);
                else
                    oSheet = (Microsoft.Office.Interop.Excel._Worksheet)(oWB.Worksheets.Item[i]);

                string Nombre = oSheet.Name;
                if (Nombre == "Hoja1" || Nombre == "Hoja2" || Nombre == "Hoja3")
                {
                    ((Microsoft.Office.Interop.Excel._Worksheet)oXL.ActiveWorkbook.Sheets[i]).Delete();
                }
            }
            return oSheet;
        }

        public void EnviarCorreo(List<string> pArchivo, string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente, string c_llegada, string c_navi_corto, string c_viaje, string n_manifiesto, List<string> pValidacion)
        {
            string Html;
            int i = 1;
            EnvioCorreo _correo = new EnvioCorreo();
            try
            {

                Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                Html += "MÓDULO : LISTADO DE CONTENEDORES AUTORIZADOS  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES AUTORIZADOS <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes a {0} para el barco {1}, con # de Viaje {2}, Manifiesto de Aduana # {3}  han sido autorizados {4} de {5} contenedores correspondientes a este barco.-", c_navi_corto, d_buque, c_viaje, n_manifiesto, pValor, pCantidad);
                Html += "<br /><br/>";

                if (pLista == null)
                    pLista = new List<DetaNaviera>();

                if (pValor < pCantidad)
                {
                    if (pLista.Count > 0)
                    {
                        Html += "Los siguientes contenedores fueron denegados por ADUANA para su revisión se detallan a continuación : ";
                        Html += "<OL>";
                        foreach (DetaNaviera item in pLista)
                        {
                            Html += "<LI>" + item.n_contenedor;
                        }
                        Html += "</OL>";
                    }
                }

                if (pArchivo.Count < 3 && c_cliente != "11" && c_cliente != "216")
                {
                    Html += "<font color=red> Este manifiesto no posee contenedores clasificados con peligrosidad </font><br/> ";
                }

                if (pValidacion.Count > 0)
                {
                    Html += "<br /><br/>";
                    Html += "<p style='background-color: #ffffcc'><font color:'#000000;'>";
                    Html += "Los siguientes contenedores presentaron inconsistencia en su tamaño según historial, y para su revisión se detallan a continuación : ";
                    Html += "<UL>";
                    foreach (string itemS in pValidacion)
                    {
                        Html += "<LI>" + itemS;
                    }
                    Html += "</UL>";
                    Html += "<br/><br/>";
                    Html += "</font></p>";
                }

                Html += "<br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> ACCIONES A REALIZAR: </b></font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> TODOS: </b></font><br />";
                Html += "<font color=blue>Impresión de los listados para ser utilizado en la operación del buque</font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> NAVIERA: </b></font><br />";
                Html += "<font color=blue><b>Remitir a CEPA:</b> a) Una copia impresa del Manifiesto de Carga; b) Por correo electrónico, en formato digital, el Manifiesto de Carga. </font><br /><br />";
                Html += "<font color=blue><b>ADUANA:</b> No requiere copia impresa del Manifiesto de Carga </font><br /><br />";


                _correo.Subject = string.Format("PASO 4 de 4: Autorización de Listado de IMPORTACIÓN de {0} para el Buque: {1}, # de Viaje {2}, Cod. de Llegada # {3}, Manifiesto de Aduana # {4}", c_navi_corto, d_buque, c_viaje, c_llegada, n_manifiesto);
                //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
                _correo.ListArch = pArchivo;

                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_autoriza", DBComun.Estado.falso, c_cliente);


                List<Notificaciones> _listaCC = new List<Notificaciones>();

                if (c_cliente != "11" && c_cliente != "216")
                    _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_autoriza", DBComun.Estado.falso, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_autoriza", DBComun.Estado.falso, "219"));
                _correo.ListaCC = _listaCC;

                //LIMITE

                //Notificaciones noti = new Notificaciones
                //{
                //    sMail = "elsa.sosa@cepa.gob.sv",
                //    dMail = "Elsa Sosa"
                //};

                //List<Notificaciones> pLisN = new List<Notificaciones>();

                //pLisN.Add(noti);

                //_correo.ListaNoti = pLisN;

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        private static object missing = System.Reflection.Missing.Value;

        public static void ConvertExcelToPdf(string excelFileIn, string pdfFileOut)
        {
            int Hwnd = 0;

            Excel.Application excel = new Excel.Application();
            Excel._Workbook wbk = null;

            try
            {
                Hwnd = excel.Application.Hwnd;


                excel.Visible = false;
                excel.ScreenUpdating = false;
                excel.DisplayAlerts = false;

                FileInfo excelFile = new FileInfo(excelFileIn);


                string filename = excelFile.FullName;

                wbk = excel.Workbooks.Open(filename, missing,
                missing, missing, missing, missing, missing,
                missing, missing, missing, missing, missing,
                missing, missing, missing);
                wbk.Activate();




                var oWBC = new cxExcel.XLWorkbook();



                object outputFileName = pdfFileOut;
                Excel.XlFixedFormatType fileFormat = Excel.XlFixedFormatType.xlTypePDF;



                // Save document into PDF Format
                wbk.ExportAsFixedFormat(fileFormat, outputFileName,
                missing, missing, missing,
                missing, missing, missing,
                missing);



                object saveChanges = Excel.XlSaveAction.xlDoNotSaveChanges;
                ((msExcel._Workbook)wbk).Close(saveChanges, missing, missing);
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);
                throw new Exception(errorMessage);
            }

            finally
            {
                ((Excel._Application)excel).Quit();

                if (excel != null)
                {
                    excel.Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excel);
                }

                if (wbk != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wbk);



                ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(Hwnd);

                wbk = null;
                excel = null;
            }
        }

        private static void GenerarExcelCX(List<ArchivoAduana> pAduana, string c_llegada, int ROWS_FIRST, int ROWS_START, int Filas, string d_buque, int cuenta, cxExcel.XLWorkbook oWB, /* Microsoft.Office.Interop.Excel._Worksheet oSheet, Microsoft.Office.Interop.Excel.Range oRng,*/ string _Fecha, int pValor, int iRow, string d_naviera, bool Imdg, int n_manif, string c_viaje)
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
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN TRANSBORDO";
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

                if (Imdg == true)
                {
                    _rango = "O";
                }
                else
                    _rango = "M";


                oSheet.Cell(3, 12).Value = "Manif. ADUANA : " + n_manif;
                oSheet.Range("L3", "M3").Merge();
                oSheet.Range("L3", "M3").Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("L3", "M3").Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Left);

                oSheet.Cell(4, 12).Value = "# Viaje : " + c_viaje;
                oSheet.Range("L4", "M4").Merge();
                oSheet.Range("L4", "M4").Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("L4", "M4").Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Left);

                oSheet.Range("L3", "M4").Style.Font.Bold = true;
                oSheet.Range("L3", "M4").Style.Font.FontSize = 13;


                oSheet.Range("A6", string.Format("{0}6", _rango)).Merge();
                oSheet.Range("A6", string.Format("{0}6", _rango)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("A6", string.Format("{0}6", _rango)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);
                oSheet.Range("A6", string.Format("{0}6", _rango)).Style.Font.FontSize = 18;

                //Add table headers going cell by cell.
                oSheet.Cell(ROWS_FIRST, 1).Value = d_naviera;

                oSheet.Range("A1", string.Format("{0}1", _rango)).Merge();
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Font.FontSize = 28;



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

                oSheet.Range("A4", "A4").Style.NumberFormat.SetFormat("@");
                oSheet.Range("A5", "A5").Style.NumberFormat.SetFormat("dd/mm/yyyy");
                oSheet.Range("A3", "A5").Style.Font.Bold = true;
                oSheet.Range("A3", "A5").Style.Font.FontSize = 13;


                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.Cell(ROWS_START, 1).Value = "No.";
                oSheet.Cell(ROWS_START, 2).Value = "CONTENEDOR";
                oSheet.Cell(ROWS_START, 3).Value = "TIPO";
                oSheet.Cell(ROWS_START, 4).Value = "PESO EN KG";
                oSheet.Cell(ROWS_START, 5).Value = "TARA";
                oSheet.Cell(ROWS_START, 6).Value = "CONDICION";
                oSheet.Cell(ROWS_START, 7).Value = "CONSIGNATARIO";
                oSheet.Cell(ROWS_START, 8).Value = "DESCRIPCIÓN DE MERCADERÍA";
                oSheet.Cell(ROWS_START, 9).Value = "PAÍS DE PROCEDENCIA";
                oSheet.Cell(ROWS_START, 10).Value = "PAÍS DE DESTINO";
                oSheet.Cell(ROWS_START, 11).Value = "TRANSFERENCIA";
                oSheet.Cell(ROWS_START, 12).Value = "MANEJO";


                if (Imdg == true)
                {
                    oSheet.Cell(ROWS_START, 13).Value = "DESPACHO";
                    oSheet.Cell(ROWS_START, 14).Value = "CLASE";
                    oSheet.Cell(ROWS_START, 15).Value = "No. ONU";
                }
                else
                    oSheet.Cell(ROWS_START, 13).Value = "DESPACHO";

                oSheet.Range("E8", "E8").Style.Alignment.WrapText = true;
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Font.Bold = true;
                oSheet.Range("A6", string.Format("{0}6", _rango)).Style.Font.Bold = true;
                oSheet.Range("A8", string.Format("{0}8", _rango)).Style.Font.Bold = true;
                oSheet.Range("A8", string.Format("{0}8", _rango)).Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;

                oSheet.Range("A8", string.Format("{0}8", _rango)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("A6", string.Format("{0}8", _rango)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);
                oSheet.Range("A9", string.Concat("C", Filas)).Style.NumberFormat.SetFormat("@");
                oSheet.Range("D9", string.Concat("D", Filas)).Style.NumberFormat.SetFormat("0.00");
                oSheet.Range("E9", string.Concat("E", Filas)).Style.NumberFormat.SetFormat("0");
                oSheet.Range("F9", string.Format("{0}{1}", _rango, Filas)).Style.NumberFormat.SetFormat("@");
                oSheet.Range("A9", string.Concat("B", Filas)).Style.Font.FontSize = 12;

                //oRng = oSheet.get_Range("A1", string.Concat("D", Filas));
                //oRng.EntireColumn.AutoFit();

                oSheet.Range("M9", string.Concat("M", Filas)).Style.NumberFormat.SetFormat("0");


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

                oRng = oSheet.Range("H9", string.Format("N{0}", Filas));
                oRng.Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oRng.Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Bottom;

                oRng = oSheet.Range("A8", string.Format("N{0}", Filas));
                oRng.Style.Font.FontSize = 8;

                //oSheet.Range(string.Format("{0}9", _rango), string.Format("{0}{1}", _rango, Filas)).Style.Font.FontSize = 12;
                //oSheet.Range(string.Format("{0}9", _rango), string.Format("{0}{1}", _rango, Filas)).Style.Font.Bold = true;         

                if (Imdg == true)
                {
                    oSheet.Range("K9", string.Format("O{0}", Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                    oSheet.Range("K9", string.Format("O{0}", Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);
                }
                else
                {
                    oSheet.Range("K9", string.Format("M{0}", Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                    oSheet.Range("K9", string.Format("M{0}", Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);
                }

                oSheet.PageSetup.Footer.Left.AddText(string.Format("&B Buque : {0} Fecha : {1} &B", d_buque, _Fecha));
                oSheet.PageSetup.Footer.Center.AddText("Página ", cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.PageNumber, cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(" de ", cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.NumberOfPages, cxExcel.XLHFOccurrence.AllPages);

                //oSheet.Range("M9", string.Concat("M", Filas)).Style.Font.Bold = false;

                oSheet.Column(1).Width = 3;
                oSheet.Column(2).Width = 20;
                oSheet.Column(3).Width = 10;
                oSheet.Column(4).Width = 8;
                oSheet.Column(5).Width = 5;
                oSheet.Column(6).Width = 8;
                oSheet.Column(7).Width = 29;
                oSheet.Column(8).Width = 29;
                oSheet.Column(9).Width = 20;
                oSheet.Column(10).Width = 12;
                oSheet.Column(11).Width = 11;
                oSheet.Column(12).Width = 8;
                if (Imdg == true)
                {
                    oSheet.Column(13).Width = 8;
                    oSheet.Column(14).Width = 8;
                    oSheet.Column(15).Width = 8;

                }
                else
                    oSheet.Column(13).Width = 8;

                string a, b, c;
                foreach (var item in pAduana)
                {
                    int iCurrent = ROWS_START + iRow;

                    if (item.b_transferencia.TrimEnd().TrimStart().Trim() == "Y")
                        a = "SI";
                    else
                        a = "NO";

                    if (item.b_manejo.TrimEnd().TrimStart().Trim() == "Y")
                        b = "SI";
                    else
                        b = "NO";

                    if (item.b_despacho.TrimEnd().TrimStart().Trim() == "Y")
                        c = "SI";
                    else
                        c = "NO";

                    oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                    oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                    oSheet.Cell(iCurrent, 3).Value = item.c_tamaño_c;
                    oSheet.Cell(iCurrent, 4).Value = item.v_peso;
                    oSheet.Cell(iCurrent, 5).Value = item.v_tara;
                    oSheet.Cell(iCurrent, 6).Value = item.b_condicion;
                    oSheet.Cell(iCurrent, 7).Value = item.s_consignatario;
                    oSheet.Cell(iCurrent, 8).Value = item.s_comodity;
                    oSheet.Cell(iCurrent, 9).Value = item.n_pais_origen;
                    oSheet.Cell(iCurrent, 10).Value = item.n_pais_destino;
                    oSheet.Cell(iCurrent, 11).Value = a;
                    oSheet.Cell(iCurrent, 12).Value = b;

                    if (Imdg == true)
                    {
                        oSheet.Cell(iCurrent, 13).Value = c;
                        oSheet.Cell(iCurrent, 14).Value = item.c_imo_imd;
                        oSheet.Cell(iCurrent, 15).Value = item.c_un_number;
                        //if (item.b_condicion.Substring(0, 3) == "LCL")
                        //    oSheet.Cell(iCurrent, 15).Value = "LCL";
                        //else
                        //    oSheet.Cell(iCurrent, 15).Value = "";

                    }
                    else
                    {
                        //if (item.b_condicion.Substring(0, 3) == "LCL")
                        //    oSheet.Cell(iCurrent, 13).Value = "LCL";
                        //else
                        //    oSheet.Cell(iCurrent, 13).Value = "";

                        oSheet.Cell(iCurrent, 13).Value = c;
                    }

                    iRow = iRow + 1;
                }



            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        private static void GenerarExcel2CX(List<ArchivoAduana> pAduana, string c_llegada, int ROWS_FIRST, int ROWS_START, int Filas, string d_buque, int cuenta, cxExcel.XLWorkbook oWB, /*Microsoft.Office.Interop.Excel._Worksheet oSheet, Microsoft.Office.Interop.Excel.Range oRng, */ string _Fecha, int pValor, int iRow, string d_naviera, int b_sidunea)
        {

            string nombreSheet = null;
            string valorCabecera = null;
            string v_Sidunea = null;
            try
            {
                if (b_sidunea == 0)
                    v_Sidunea = "";
                else
                    v_Sidunea = " - SW";

                switch (pValor)
                {
                    case 1:
                        nombreSheet = "LLENOS";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN LLENOS" + v_Sidunea;
                        break;
                    case 2:
                        nombreSheet = "VACIOS";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS" + v_Sidunea;
                        //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                        break;
                    case 3:
                        nombreSheet = "REEF";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN REFRIGERADOS A CONECTAR" + v_Sidunea;
                        /*oSheet.Name = "REEF";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN REFRIGERADOS A CONECTAR ";*/
                        break;
                    case 4:
                        nombreSheet = "TRASBORDO";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN TRANSBORDO" + v_Sidunea;
                        break;
                    case 5:
                        nombreSheet = "RET_DIR";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO LLENOS" + v_Sidunea;
                        break;
                    case 6:
                        nombreSheet = "RET_DIR_VACIO";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO VACÍOS" + v_Sidunea;
                        break;
                    case 7:
                        nombreSheet = "CONTE_PELIGROSIDAD";
                        valorCabecera = "LISTADO DE CONTENEDORES DE IMPORTACIÓN PELIGROSIDAD" + v_Sidunea;
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
                oSheet.Range("D9", string.Concat("D", Filas)).Style.NumberFormat.SetFormat("#,##0.00");
                oSheet.Range("E9", string.Concat("E", Filas)).Style.NumberFormat.SetFormat("#,##0");
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
                    else if (item.v_peso + item.v_tara > 30000.00)
                        oSheet.Cell(iCurrent, 8).Value = "ADVERTENCIA DE PESO (PESO + TARA): " + String.Format("{0:0,0.00}", item.v_peso + item.v_tara);
                    else if (item.b_shipper == "SI")
                        oSheet.Cell(iCurrent, 8).Value = "SHIPPER OWNED";
                    else
                        oSheet.Cell(iCurrent, 8).Value = "";


                    iRow = iRow + 1;
                }


                oSheet.Range("H9", string.Format("H{0}", Filas)).Style.Alignment.SetWrapText(true);

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

        public string GenerarPeligrosidadCX(List<ArchivoAduana> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque, DateTime f_llegada, int pValor, int pCantidad, int n_manif, string c_viaje)
        {

            const int ROWS_FIRST = 1;
            const int ROWS_START = 8;
            int Filas = 0;
            int iRow = 1;

            var oWB = new cxExcel.XLWorkbook();



            List<ArchivoAduana> _list = new List<ArchivoAduana>();

            string _f4save = string.Format("{0}{1}", fullIMDG, "IMDG_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");

            try
            {


                string fechaC = f_llegada.ToString("dd/MM/yy HH:mm", CultureInfo.CreateSpecificCulture("es-SV"));




                if (pLista.Count > 0)
                {


                    Filas = pLista.Count + ROWS_START;


                    GenerarExcelCX(pLista, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, 7, oWB, fechaC, 7, iRow, d_naviera, true, n_manif, c_viaje);

                    oWB.SaveAs(_f4save);


                }
                else
                {
                    _f4save = null;
                }

                return _f4save;

            }
            catch (NullReferenceException et)
            {
                return null;
            }
            catch (System.ArgumentException ex)
            {
                System.ArgumentException _arg = new System.ArgumentException(ex.Message);
                throw _arg;

            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);
                throw new Exception(errorMessage);
            }
            finally
            {



            }

        }

        private static void GenerarExcelCOTECNA(List<ArchivoAduana> pAduana, string c_llegada, int ROWS_FIRST, int ROWS_START, int Filas, string d_buque, int cuenta, cxExcel.XLWorkbook oWB, /* Microsoft.Office.Interop.Excel._Worksheet oSheet, Microsoft.Office.Interop.Excel.Range oRng,*/ string _Fecha, int pValor, int iRow, string d_naviera, bool Imdg, int n_manif, string c_viaje)
        {

            string nombreSheet = null;

            try
            {
                nombreSheet = "COTECNA";


                var oSheet = oWB.Worksheets.Add(nombreSheet);

                string _rango = null;

                _rango = "O";

                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Font.Bold = true;
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;

                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                oSheet.Range("E2", string.Concat("E", Filas)).Style.NumberFormat.SetFormat("@");

                oSheet.Range("C2", string.Concat("C", Filas)).Style.NumberFormat.SetFormat("@");


                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.Cell(ROWS_START, 1).Value = "No.";
                oSheet.Cell(ROWS_START, 2).Value = "TIPO DECLARACION";
                oSheet.Cell(ROWS_START, 3).Value = "MANIFIESTO";
                oSheet.Cell(ROWS_START, 4).Value = "CONSIGNATARIO";
                oSheet.Cell(ROWS_START, 5).Value = "NUMERO BL";
                oSheet.Cell(ROWS_START, 6).Value = "DESCRIPCIÓN DE MERCADERÍA";
                oSheet.Cell(ROWS_START, 7).Value = "BULTOS";
                oSheet.Cell(ROWS_START, 8).Value = "EMBALAJE";
                oSheet.Cell(ROWS_START, 9).Value = "PESO EN KG";
                oSheet.Cell(ROWS_START, 10).Value = "CONTENEDOR";
                oSheet.Cell(ROWS_START, 11).Value = "EMPRESA";
                oSheet.Cell(ROWS_START, 12).Value = "PAIS DE PROCEDENCIA";
                oSheet.Cell(ROWS_START, 13).Value = "PAIS DE DESTINO";
                oSheet.Cell(ROWS_START, 14).Value = "CONDICION";
                oSheet.Cell(ROWS_START, 15).Value = "TIPO BL";

                var oRng = oSheet.Range("A1", string.Concat(string.Format("{0}", _rango), Filas));

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

                oSheet.Column(1).Width = 6;
                oSheet.Column(2).Width = 17;
                oSheet.Column(3).Width = 9;
                oSheet.Column(4).Width = 60;
                oSheet.Column(5).Width = 15;
                oSheet.Column(6).Width = 70;
                oSheet.Column(7).Width = 6;
                oSheet.Column(8).Width = 8;
                oSheet.Column(9).Width = 9;
                oSheet.Column(10).Width = 14;
                oSheet.Column(11).Width = 50;
                oSheet.Column(12).Width = 20;
                oSheet.Column(13).Width = 15;
                oSheet.Column(14).Width = 9;
                oSheet.Column(15).Width = 6;

                foreach (var item in pAduana)
                {
                    int iCurrent = ROWS_START + iRow;

                    oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                    oSheet.Cell(iCurrent, 2).Value = item.tipoDecla;
                    oSheet.Cell(iCurrent, 3).Value = item.a_manifiesto;
                    oSheet.Cell(iCurrent, 4).Value = item.s_consignatario;
                    oSheet.Cell(iCurrent, 5).Value = item.n_BL;
                    oSheet.Cell(iCurrent, 6).Value = item.s_comodity;
                    oSheet.Cell(iCurrent, 7).Value = item.c_paquete;
                    oSheet.Cell(iCurrent, 8).Value = item.c_embalaje;
                    oSheet.Cell(iCurrent, 9).Value = item.v_peso;
                    oSheet.Cell(iCurrent, 10).Value = item.n_contenedor;
                    oSheet.Cell(iCurrent, 11).Value = item.d_cliente;
                    oSheet.Cell(iCurrent, 12).Value = item.c_pais_origen;
                    oSheet.Cell(iCurrent, 13).Value = item.c_pais_destino;
                    oSheet.Cell(iCurrent, 14).Value = item.c_condicion;
                    oSheet.Cell(iCurrent, 15).Value = item.c_tipo_bl;

                    iRow = iRow + 1;
                }

                oSheet.Range("A2", string.Format("{0}", "E" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("A2", string.Format("{0}", "E" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                oSheet.Range("G2", string.Format("{0}", "H" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("G2", string.Format("{0}", "H" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                oSheet.Range("I2", string.Format("{0}", "I" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("I2", string.Format("{0}", "I" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Right);


                oSheet.Range("J2", string.Format("{0}", "K" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("J2", string.Format("{0}", "K" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                oSheet.Range("L2", string.Format("{0}", "M" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("L2", string.Format("{0}", "M" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Left);

                oSheet.Range("N2", string.Format("{0}", "O" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                oSheet.Range("N2", string.Format("{0}", "O" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                oSheet.Range("A1", string.Format("{0}", "O" + Filas)).Style.Font.FontSize = 8;

                oSheet.Range("A1", string.Format("{0}", "O" + Filas)).Style.Font.FontName = "Calibri";



            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public string generarCOTECNA(List<ArchivoAduana> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque, DateTime f_llegada, int pValor, int pCantidad, int n_manif, string c_viaje, string c_navi_corto)
        {

            const int ROWS_FIRST = 1;
            const int ROWS_START = 1;
            int Filas = 0;
            int iRow = 1;

            var oWB = new cxExcel.XLWorkbook();



            List<ArchivoAduana> _list = new List<ArchivoAduana>();

            //COTECNA
            string _f7save = string.Format("{0}{1}", fullPathCO, "COTEC_" + d_buque + "_" + c_navi_corto + "_" + f_llegada.ToString("ddMMyy", CultureInfo.CreateSpecificCulture("es-SV")) + ".xlsx");

            try
            {


                string fechaC = f_llegada.ToString("dd/MM/yy HH:mm", CultureInfo.CreateSpecificCulture("es-SV"));




                if (pLista.Count > 0)
                {


                    Filas = pLista.Count + ROWS_START;


                    GenerarExcelCOTECNA(pLista, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, 7, oWB, fechaC, 7, iRow, d_naviera, true, n_manif, c_viaje);

                    oWB.SaveAs(_f7save);


                }
                else
                {
                    _f7save = null;
                }

                return _f7save;

            }
            catch (NullReferenceException et)
            {
                return null;
            }
            catch (System.ArgumentException ex)
            {
                System.ArgumentException _arg = new System.ArgumentException(ex.Message);
                throw _arg;

            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);
                throw new Exception(errorMessage);
            }
            finally
            {



            }

        }

        public void EnviarCorreoCO(List<string> pArchivo, string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente, string c_llegada, string c_navi_corto, string c_viaje, int n_manifiesto)
        {
            string Html;
            int i = 1;
            EnvioCorreo _correo = new EnvioCorreo();
            try
            {

                Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                Html += "MÓDULO : LISTADO DE CONTENEDORES AUTORIZADOS  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES AUTORIZADOS <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes a {0} para el barco {1}, con # de Viaje {2}, Manifiesto de Aduana # {3}  han sido autorizados {4} de {5} contenedores correspondientes a este barco.-", c_navi_corto, d_buque, c_viaje, n_manifiesto, pValor, pCantidad);
                Html += "<br /><br/>";

                if (pLista == null)
                    pLista = new List<DetaNaviera>();

                if (pValor < pCantidad)
                {
                    if (pLista.Count > 0)
                    {
                        Html += "Los siguientes contenedores fueron denegados por ADUANA para su revisión se detallan a continuación : ";
                        Html += "<OL>";
                        foreach (DetaNaviera item in pLista)
                        {
                            Html += "<LI>" + item.n_contenedor;
                        }
                        Html += "</OL>";
                    }
                }


                Html += "<br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> ACCIONES A REALIZAR: </b></font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> TODOS: </b></font><br />";
                Html += "<font color=blue>Impresión de los listados para ser utilizado en la operación del buque</font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> NAVIERA: </b></font><br />";
                Html += "<font color=blue><b>Remitir a CEPA:</b> a) Dos copias impresas del Manifiesto de Carga; b) Por correo electrónico, en formato digital, el Manifiesto de Carga. </font><br /><br />";
                Html += "<font color=blue><b>ADUANA:</b> No requiere copia impresa del Manifiesto de Carga </font><br /><br />";

                _correo.Subject = string.Format("PASO 4 de 4: Autorización de Listado de IMPORTACIÓN de {0} para el Buque: {1}, # de Viaje {2}, Cod. de Llegada # {3}, Manifiesto de Aduana # {4}", c_navi_corto, d_buque, c_viaje, c_llegada, n_manifiesto);
                //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
                _correo.ListArch = pArchivo;
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_contratista", DBComun.Estado.falso, "17");


                List<Notificaciones> _listaCC = new List<Notificaciones>();
                _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_contratista", DBComun.Estado.falso, "17");


                _correo.ListaCC = _listaCC;

                //Notificaciones noti = new Notificaciones
                //{
                //    sMail = "elsa.sosa@cepa.gob.sv",
                //    dMail = "Elsa Sosa"
                //};

                //List<Notificaciones> pLisN = new List<Notificaciones>();

                //pLisN.Add(noti);

                //_correo.ListaNoti = pLisN;

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }


        public void EnviarCorreoCOTEC(List<string> pArchivo, string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente, string c_llegada, string c_navi_corto, string c_viaje, int n_manifiesto, string a_manifiesto)
        {
            string Html;
            int i = 1;
            EnvioCorreo _correo = new EnvioCorreo();
            try
            {

                Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                Html += "MÓDULO : LISTADO DE CONTENEDORES AUTORIZADOS  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES AUTORIZADOS <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes a {0} para el barco {1}, con # de Viaje {2}, Manifiesto de Aduana # {3}  han sido autorizados {4} de {5} contenedores correspondientes a este barco.-", c_navi_corto, d_buque, c_viaje, n_manifiesto, pValor, pCantidad);
                Html += "<br /><br/>";

                if (pLista == null)
                    pLista = new List<DetaNaviera>();

                if (pValor < pCantidad)
                {
                    if (pLista.Count > 0)
                    {
                        Html += "Los siguientes contenedores fueron denegados por ADUANA para su revisión se detallan a continuación : ";
                        Html += "<OL>";
                        foreach (DetaNaviera item in pLista)
                        {
                            Html += "<LI>" + item.n_contenedor;
                        }
                        Html += "</OL>";
                    }
                }


                Html += "<br /><br />";

                _correo.Subject = string.Format("COTECNA: Autorización de Listado de IMPORTACIÓN de {0} para el Buque: {1}, Manifiesto de Aduana # {2}", c_navi_corto, d_buque, n_manifiesto);
                //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
                _correo.ListArch = pArchivo;
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_cotec_lst", DBComun.Estado.falso, "219");


                List<Notificaciones> _listaCC = new List<Notificaciones>();
                _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_cotec_lst", DBComun.Estado.falso, "219");


                _correo.ListaCC = _listaCC;

                //Notificaciones noti = new Notificaciones
                //{
                //    sMail = "elsa.sosa@cepa.gob.sv",
                //    dMail = "Elsa Sosa"
                //};

                //List<Notificaciones> pLisN = new List<Notificaciones>();

                //pLisN.Add(noti);

                //_correo.ListaNoti = pLisN;

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public void GenerarAplicacionEX(List<ArchivoExport> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque,
              DateTime f_llegada, int pValor, int pCantidad, List<DetaNaviera> pCancelados, string c_voyage, string trafico)
        {


            const int ROWS_FIRST = 1;
            const int ROWS_START = 10;
            int Filas = 0;
            int iRow = 1;
            pRutas = null;


            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            //string s_archivo = System.Windows.Forms.Application.StartupPath + "\\EXP_PLANTILLA.xlsx";

            var oWB = new cxExcel.XLWorkbook();

            List<ArchivoExport> _list = new List<ArchivoExport>();




            string ruta = null;
            string _Exf1save = null;
            string _Exf2save = null;
            string _Exf3save = null;
            string _Exf4save = null;
            int contador = 1;
            string _ruta = null;
            try
            {


                List<EnvioAuto> listAuto = new List<EnvioAuto>();
                if (IdDoc > 0)
                    listAuto = DetaNavieraDAL.ObtenerDocAuExp(IdDoc, DBComun.Estado.falso);

                string c_navi_corto = null, c_viaje = null;

                foreach (var item in listAuto)
                {
                    c_navi_corto = item.c_naviera_corto;
                    c_viaje = item.c_voyaje;
                    break;
                }

                _Exf1save = string.Format("{0}{1}", fullEXP_PREV, "EXP_PREV_" + c_navi_corto + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
                _Exf2save = string.Format("{0}{1}", fullEXP_PREV, "EXP_PREV_" + c_navi_corto + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".pdf");

                _Exf3save = string.Format("{0}{1}", fullEXP_AUTO, "EXP_AUTO_" + c_navi_corto + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
                _Exf4save = string.Format("{0}{1}", fullEXP_AUTO, "EXP_AUTO_" + c_navi_corto + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".pdf");


                string fechaC = f_llegada.Day + "/" + f_llegada.Month + "/" + f_llegada.Year;

                int Cuenta = 1;

                if (trafico == "PC")
                {
                    contador = 1;
                    var _destinos = (from a in pLista.OrderBy(a => a.c_corr_previo)
                                     group a by a.c_puerto_trasbordo into g
                                     select new
                                     {
                                         c_puerto_trasbordo = g.Key
                                     }).ToList();

                    List<ArchivoExport> _listOr = new List<ArchivoExport>();
                    List<ArchivoExport> _listaAOrdenar1 = new List<ArchivoExport>();

                    pLista = pLista.OrderBy(a => a.c_corr_previo).ToList();


                    for (int d = 1; d <= 2; d++)
                    {
                        if (d == 1)
                            _listaAOrdenar1 = pLista.Where(f => f.s_trafico == "PATIO CEPA").ToList();
                        else if (d == 2)
                            _listaAOrdenar1 = pLista.Where(f => f.s_trafico == "TRANSBORDO").ToList();

                        if (_listaAOrdenar1.Count > 0)
                        {
                            for (int i = 1; i <= 2; i++)
                            {
                                var consulta = DetaNavieraLINQ.AlmacenarArchivoEx(_listaAOrdenar1, DBComun.Estado.falso, i);

                                if (_destinos.Count() > 0 && consulta.Count() > 0)
                                {
                                    foreach (var item in _destinos)
                                    {

                                        var _trafico = consulta.Where(c => c.c_puerto_trasbordo.Equals(item.c_puerto_trasbordo)).ToList();

                                        Filas = _trafico.Count + ROWS_START;
                                        if (_trafico.Count() > 0)
                                        {
                                            GenerarExcel2CXExp(_trafico, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, i, iRow, d_naviera, c_voyage, contador, "");
                                            Cuenta = Cuenta + 1;
                                            contador += 1;


                                            foreach (var item_C in _listaAOrdenar1)
                                            {
                                                pLista.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                                            }
                                        }

                                    }
                                }
                            }

                        }
                    }


                    oWB.SaveAs(_Exf1save);
                    //  oWB1.SaveAs(_f4save);



                    //Load Workbook
                    _ruta = "@" + _Exf1save;




                    ConvertExcelToPdf(_Exf1save, _Exf2save);
                    //ExportWorkbookToPdf(_Exf1save, _Exf2save);

                    pRutas = new List<string>();

                    //pRutas.Add(_Exf1save);
                    pRutas.Add(_Exf2save);



                    //List<string> pPDF = new List<string>();                

                    //CORREO ENVIAR
                    EnviarCorreoEx(pRutas, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje);
                }
                else
                {
                    contador = 1;
                    var _destinos = (from a in pLista.OrderBy(a => a.c_correlativo)
                                     group a by a.c_puerto_trasbordo into g
                                     select new
                                     {
                                         c_puerto_trasbordo = g.Key
                                     }).ToList();

                    var _predios = (from a in pLista.OrderBy(a => a.c_correlativo)
                                    group a by a.s_nom_predio into g
                                    select new
                                    {
                                        s_nom_predio = g.Key
                                    }).ToList();

                    List<ArchivoExport> _listOr = new List<ArchivoExport>();
                    List<ArchivoExport> _listaAOrdenar1 = new List<ArchivoExport>();

                    pLista = pLista.OrderBy(a => a.c_correlativo).ToList();

                    if (_predios.Count() > 0 && _destinos.Count() > 0)
                    {
                        foreach (var itemPre in _predios)
                        {
                            foreach (var item in _destinos)
                            {
                                for (int i = 1; i <= 2; i++)
                                {
                                    var consulta = DetaNavieraLINQ.AlmacenarArchivoEx(pLista, DBComun.Estado.falso, i);

                                    if (consulta.Count() > 0)
                                    {

                                        var _trafico = consulta.Where(f => f.s_nom_predio == itemPre.s_nom_predio && f.c_puerto_trasbordo == item.c_puerto_trasbordo).ToList();
                                        
                                        Filas = _trafico.Count + ROWS_START;
                                        if (_trafico.Count() > 0)
                                        {
                                            GenerarExcel2CXExp(_trafico, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, i, iRow, d_naviera, c_voyage, contador, itemPre.s_nom_predio);
                                            Cuenta = Cuenta + 1;
                                            contador += 1;


                                            foreach (var item_C in _trafico)
                                            {
                                                consulta.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    oWB.SaveAs(_Exf3save);
                    //  oWB1.SaveAs(_f4save);



                    //Load Workbook
                    _ruta = "@" + _Exf3save;




                    ConvertExcelToPdf(_Exf3save, _Exf4save);
                    //ExportWorkbookToPdf(_Exf1save, _Exf2save);


                    pRutas = new List<string>();

                    //pRutas.Add(_Exf1save);
                    pRutas.Add(_Exf3save);
                    pRutas.Add(_Exf4save);



                    //List<string> pPDF = new List<string>();                

                    //CORREO ENVIAR
                    EnviarCorreoExPR(pRutas, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje);
                }




                //pPDF.Add(_f3save);

                //EnviarCorreoCO(pPDF, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

                //EnviarCorreoCOTEC(pCotecLst, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto, a_manifiesto);

                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);
                throw new Exception(errorMessage);
            }
            finally
            {

            }
        }

        private static void GenerarExcel2CXExp(List<ArchivoExport> pAduana, string c_llegada, int ROWS_FIRST, int ROWS_START, int Filas, string d_buque, int cuenta, cxExcel.XLWorkbook oWB, string _Fecha, int pValor, int iRow, string d_naviera, string c_voyage, int contador, string predioPrin)
        {

            string nombreSheet = null;
            string valorCabecera = null;

            string puerto_destino = null;
            string ubicacion = null;
            try
            {
                foreach (var details in pAduana)
                {
                    puerto_destino = details.c_puerto_trasbordo;
                    ubicacion = details.s_trafico;
                    break;
                }

                string ubi = ubicacion == "TRANSBORDO" ? "TR" : ubicacion == "PATIO CEPA" ? "PC" : "ED";

                switch (pValor)
                {
                    case 1:
                        nombreSheet = "LLENOS";
                        valorCabecera = "LISTADO " + ((ubi != "ED") ? "PRELIMINAR " : "") + "DE CONTENEDORES DE EXPORTACIÓN LLENOS" + " " + ubicacion;
                        break;
                    case 2:
                        nombreSheet = "VACIOS";
                        valorCabecera = "LISTADO " + ((ubi != "ED") ? "PRELIMINAR " : "") + "DE CONTENEDORES DE EXPORTACIÓN VACÍOS " + ubicacion;
                        //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                        break;
                    case 3:
                        nombreSheet = "TRANSBORDOS";
                        valorCabecera = "LISTADO " + ((ubi != "ED") ? "PRELIMINAR " : "") + "DE CONTENEDORES DE EXPORTACIÓN TRANSBORDOS";
                        //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                        break;
                    default:
                        break;
                }

                //string ubi = ubicacion == "TRANSBORDO" ? "TR" : ubicacion == "PATIO CEPA" ? "PC" : "ED";
                string valSheet = puerto_destino.Substring(0, 3) + "_" + nombreSheet.Substring(0, 3) + "_" + ubi + contador;
                var oSheet = oWB.Worksheets.Add(valSheet);
                oSheet.PageSetup.Header.Clear();

                oSheet.Range("A1", "H2").Merge();
                oSheet.Cell("A1").Value = valorCabecera;
                oSheet.Cell("A1").Style.Font.Bold = true;
                oSheet.Cell("A1").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Cell("A1").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Cell("A1").Style.Font.FontSize = 16;

                oSheet.Range("A4", "B4").Merge();
                oSheet.Range("A5", "B5").Merge();
                oSheet.Range("A6", "B6").Merge();
                oSheet.Range("A7", "B7").Merge();
                oSheet.Range("A8", "B8").Merge();

                oSheet.Cell("A4").Value = "AGENCIA";
                oSheet.Cell("A5").Value = "VAPOR - VIAJE";
                oSheet.Cell("A6").Value = "DESTINO";
                oSheet.Cell("A7").Value = "UBICACION";
                oSheet.Cell("A8").Value = "FECHA DE ARRIBO";
                oSheet.Range("A4", "H8").Style.Font.FontSize = 14;

                oSheet.Range("C4", "H4").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C5", "H5").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C6", "H6").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C7", "H7").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C8", "H8").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;

                string ubiFinal = null;
                if (ubi != "ED")
                    ubiFinal = ubicacion;
                else if (ubi == "ED")
                    ubiFinal = ubicacion + " - (" + predioPrin.ToUpper() + ")";



                oSheet.Cell("C4").Value = d_naviera;
                oSheet.Cell("C4").Style.Font.Bold = true;
                oSheet.Cell("C5").Value = d_buque + " - " + c_voyage;
                oSheet.Cell("C5").Style.Font.Bold = true;
                oSheet.Cell("C6").Value = puerto_destino;
                oSheet.Cell("C6").Style.Font.Bold = true;
                oSheet.Cell("C7").Value = ubiFinal;
                oSheet.Cell("C7").Style.Font.Bold = true;
                oSheet.Cell("C8").Value = _Fecha;
                oSheet.Cell("C8").Style.Font.Bold = true;


                if (ubi != "ED")
                {
                    oSheet.Cell("A10").Value = "No.";
                    oSheet.Cell("B10").Value = "CONTENEDOR";
                    oSheet.Cell("C10").Value = "TAMAÑO";
                    oSheet.Cell("D10").Value = "PESO";
                    oSheet.Cell("E10").Value = "TARA";
                    oSheet.Cell("F10").Value = "POSICIÓN";
                    oSheet.Cell("G10").Value = "PE";
                    oSheet.Cell("H10").Value = "OBSERVACIÓN";
                }
                else if (ubi == "ED")
                {
                    oSheet.Cell("A10").Value = "No.";
                    oSheet.Cell("B10").Value = "CONTENEDOR";
                    oSheet.Cell("C10").Value = "TAMAÑO";
                    oSheet.Cell("D10").Value = "TARA";
                    oSheet.Cell("E10").Value = "ARIVU";
                    oSheet.Cell("F10").Value = "FECHA VENCIMIENTO";
                    oSheet.Cell("G10").Value = "POSICIÓN";
                    oSheet.Cell("H10").Value = "OBSERVACIÓN";

                    oSheet.Cell("F10").Style.Alignment.SetWrapText(true);
                }

                oSheet.Range("A10", "H10").Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                oSheet.Range("A10", "H10").Style.Font.Bold = true;
                oSheet.Range("A10", "H10").Style.Font.FontSize = 14;
                oSheet.Range("A10", "H10").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A10", "H10").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                //oSheet.Range("A8", "H8").Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                //oSheet.Range("A8", "H8").Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                if (ubi != "ED")
                {
                    oSheet.Column(1).Width = 7;
                    oSheet.Column(2).Width = 21;
                    oSheet.Column(3).Width = 12;
                    oSheet.Column(4).Width = 15;
                    oSheet.Column(5).Width = 8;
                    oSheet.Column(6).Width = 23;
                    oSheet.Column(7).Width = 3;
                    oSheet.Column(8).Width = 20;

                    oSheet.Range("D11", string.Concat("D", Filas)).Style.NumberFormat.SetFormat("#,##0.00");
                    oSheet.Range("E11", string.Concat("E", Filas)).Style.NumberFormat.SetFormat("#,##0");
                }
                else if (ubi == "ED")
                {
                    oSheet.Column(1).Width = 7;
                    oSheet.Column(2).Width = 21;
                    oSheet.Column(3).Width = 12;
                    oSheet.Column(4).Width = 8;
                    oSheet.Column(5).Width = 13;
                    oSheet.Column(6).Width = 17;
                    oSheet.Column(7).Width = 20;
                    oSheet.Column(8).Width = 20;

                    oSheet.Range("D11", string.Concat("D", Filas)).Style.NumberFormat.SetFormat("#,##0");
                    oSheet.Range("E11", string.Concat("E", Filas)).Style.NumberFormat.SetFormat("@");
                }


                oSheet.Range("A11", string.Concat("H", Filas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Range("A11", string.Concat("H", Filas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;

                oSheet.Range("A10", string.Concat("H", Filas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                oSheet.Range("A10", string.Concat("H", Filas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;


                int iCurrent = 0;
                if (ubi != "ED")
                {
                    foreach (var item in pAduana)
                    {
                        iCurrent = ROWS_START + iRow;
                        oSheet.Row(iCurrent).Height = 40;
                        oSheet.Cell(iCurrent, 1).Value = ubi != "ED" ? item.c_corr_previo : item.c_correlativo;
                        oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                        oSheet.Cell(iCurrent, 3).Value = item.c_tamaño_c;
                        oSheet.Cell(iCurrent, 4).Value = item.v_peso;
                        oSheet.Cell(iCurrent, 5).Value = item.v_tara;
                        oSheet.Cell(iCurrent, 6).Value = "";
                        oSheet.Cell(iCurrent, 7).Value = "";
                        if (Convert.ToDouble(item.v_peso) > 30000.00)
                            oSheet.Cell(iCurrent, 8).Value = "ADVERTENCIA DE PESO (PESO + TARA): " + String.Format("{0:0,0.00}", Convert.ToDouble(item.v_peso).ToString("0,0.00"));
                        else if (item.b_shipper == "SI")
                            oSheet.Cell(iCurrent, 8).Value = "SHIPPER OWNED";
                        else
                            oSheet.Cell(iCurrent, 8).Value = "";

                        oSheet.Cell(iCurrent, 8).Style.Alignment.SetWrapText(true);

                        iRow = iRow + 1;
                    }
                }
                else if (ubi == "ED")
                {
                    foreach (var item in pAduana)
                    {
                        iCurrent = ROWS_START + iRow;
                        oSheet.Row(iCurrent).Height = 40;
                        oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                        oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                        oSheet.Cell(iCurrent, 3).Value = item.c_tamaño_c;
                        oSheet.Cell(iCurrent, 4).Value = item.v_tara;
                        oSheet.Cell(iCurrent, 5).Value = item.n_documento;
                        oSheet.Cell(iCurrent, 6).Value = item.s_fec_venc;
                        oSheet.Cell(iCurrent, 7).Value = "";
                        oSheet.Cell(iCurrent, 8).Value = "";

                        oSheet.Cell(iCurrent, 6).Style.Alignment.SetWrapText(true);

                        iRow = iRow + 1;
                    }
                }


                //oSheet.Cell(iCurrent + 2, 1).Value = "Referencias: PE Pedido especial";
                oSheet.Cell(iCurrent + 2, 1).Style.Font.Bold = true;
                oSheet.Range("A11", string.Concat("H", Filas)).Style.Font.FontSize = 14;
                oSheet.Range("B11", string.Concat("B", Filas)).Style.Font.FontSize = 18;





                //oSheet.Range("H9", string.Format("H{0}", Filas)).Style.Alignment.SetWrapText(true);
                oSheet.Range("A1", string.Concat("H", Filas + 2)).Style.Font.FontName = "Trebuchet MS";

                oSheet.Cell("C3").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C4").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C6").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C7").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                oSheet.Cell("A3").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A4").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A6").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A7").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;

                oSheet.PageSetup.Footer.Left.AddText(string.Format("Buque : {0} - Cod. Llegada : {1}", d_buque, c_llegada));
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.PageNumber, cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(" / ", cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.NumberOfPages, cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Right.AddText(string.Format("Fecha : {0}", _Fecha));


                oSheet.PageSetup.PageOrientation = cxExcel.XLPageOrientation.Portrait;
                oSheet.PageSetup.AdjustTo(72);
                oSheet.PageSetup.PaperSize = cxExcel.XLPaperSize.LetterPaper;
                oSheet.PageSetup.VerticalDpi = 600;
                oSheet.PageSetup.HorizontalDpi = 600;
                oSheet.PageSetup.Margins.Top = 0.3;
                oSheet.PageSetup.Margins.Header = 0.40;
                oSheet.PageSetup.Margins.Footer = 0.20;

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public void EnviarCorreoEx(List<string> pArchivo, string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente, string c_llegada, string c_navi_corto, string c_viaje)
        {
            string Html;
            int i = 1;
            EnvioCorreo _correo = new EnvioCorreo();
            try
            {

                Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                Html += "MÓDULO : LISTADO PREVIO DE CONTENEDORES DE EXPORTACIÓN  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE LISTADO PREVIO DE CONTENEDORES DE EXPORTACIÓN <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes a {0} para el barco {1}, con # de Viaje {2}, han sido validados {3} de {4} contenedores correspondientes a este barco.-", c_navi_corto, d_buque, c_viaje, pValor, pCantidad);
                Html += "<br /><br/>";

                Html += "<br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> ACCIONES A REALIZAR: </b></font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> NAVIERA: </b></font><br />";
                Html += "<font color=blue><b>Remitir:</b> Por correo electrónico, en formato digital enviar los documentos que validen el embarque en el orden proporcionado en el listado; </font><br /><br />";


                _correo.Subject = string.Format("PASO 3 de 4: Generación Automática de Listado Previo de EXPORTACIÓN de {0} para el Buque: {1}, # de Viaje {2}, Cod. de Llegada # {3}", c_navi_corto, d_buque, c_viaje, c_llegada);
                //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
                _correo.ListArch = pArchivo;

                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_auto_ex_3", DBComun.Estado.falso, c_cliente);


                List<Notificaciones> _listaCC = new List<Notificaciones>();

                if (c_cliente != "11" && c_cliente != "216")
                    _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_auto_ex_3", DBComun.Estado.falso, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_auto_ex_3", DBComun.Estado.falso, "219"));

                //_listaCC.Remove(_listaCC.Where(x => x.sMail.Equals("supervisor.muelles@cepa.gob.sv")).FirstOrDefault());

                _correo.ListaCC = _listaCC;

                //LIMITE

                //Notificaciones noti = new Notificaciones
                //{
                //    sMail = "elsa.sosa@cepa.gob.sv",
                //    dMail = "Elsa Sosa"
                //};

                //List<Notificaciones> pLisN = new List<Notificaciones>();

                //pLisN.Add(noti);

                //_correo.ListaNoti = pLisN;

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public void EnviarCorreoExPR(List<string> pArchivo, string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente, string c_llegada, string c_navi_corto, string c_viaje)
        {
            string Html;
            int i = 1;
            EnvioCorreo _correo = new EnvioCorreo();
            try
            {

                Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                Html += "MÓDULO : LISTADO PREVIO DE CONTENEDORES DE EXPORTACIÓN  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE LISTADO AUTORIZADO DE CONTENEDORES DE EXPORTACIÓN <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes a {0} para el barco {1}, con # de Viaje {2}, han sido validados {3} de {4} contenedores correspondientes a este barco.-", c_navi_corto, d_buque, c_viaje, pValor, pCantidad);
                Html += "<br /><br/>";

                Html += "<br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> ACCIONES A REALIZAR: </b></font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> NAVIERA: </b></font><br />";
                Html += "<font color=blue><b>Remitir:</b> Por correo electrónico, en formato digital enviar los documentos que validen el embarque en el orden proporcionado en el listado; </font><br /><br />";


                _correo.Subject = string.Format("PASO 4 de 4: Generación Automática de Listado Autorizado de EXPORTACIÓN EMBARQUE DIRECTO de {0} para el Buque: {1}, # de Viaje {2}, Cod. de Llegada # {3}", c_navi_corto, d_buque, c_viaje, c_llegada);
                //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
                _correo.ListArch = pArchivo;

                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_auto_ex", DBComun.Estado.falso, c_cliente);


                List<Notificaciones> _listaCC = new List<Notificaciones>();

                if (c_cliente != "11" && c_cliente != "216")
                    _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_auto_ex", DBComun.Estado.falso, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_auto_ex", DBComun.Estado.falso, "219"));
                _correo.ListaCC = _listaCC;

                //LIMITE

                //Notificaciones noti = new Notificaciones
                //{
                //    sMail = "elsa.sosa@cepa.gob.sv",
                //    dMail = "Elsa Sosa"
                //};

                //List<Notificaciones> pLisN = new List<Notificaciones>();

                //pLisN.Add(noti);

                //_correo.ListaNoti = pLisN;

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        public bool ExportWorkbookToPdf(string workbookPath, string outputPath)
        {
            // If either required string is null or empty, stop and bail out
            if (string.IsNullOrEmpty(workbookPath) || string.IsNullOrEmpty(outputPath))
            {
                return false;
            }

            // Create COM Objects
            Microsoft.Office.Interop.Excel.Application excelApplication;
            Microsoft.Office.Interop.Excel.Workbook excelWorkbook;

            // Create new instance of Excel
            excelApplication = new Microsoft.Office.Interop.Excel.Application();

            // Make the process invisible to the user
            excelApplication.ScreenUpdating = false;

            // Make the process silent
            excelApplication.DisplayAlerts = false;

            // Open the workbook that you wish to export to PDF
            excelWorkbook = excelApplication.Workbooks.Open(workbookPath);

            // If the workbook failed to open, stop, clean up, and bail out
            if (excelWorkbook == null)
            {
                excelApplication.Quit();

                excelApplication = null;
                excelWorkbook = null;

                return false;
            }

            var exportSuccessful = true;
            try
            {
                // Call Excel's native export function (valid in Office 2007 and Office 2010, AFAIK)
                excelWorkbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, outputPath);
            }
            catch (System.Exception ex)
            {
                // Mark the export as failed for the return value...
                exportSuccessful = false;

                // Do something with any exceptions here, if you wish...
                // MessageBox.Show...        
            }
            finally
            {
                // Close the workbook, quit the Excel, and clean up regardless of the results...
                excelWorkbook.Close();
                excelApplication.Quit();

                excelApplication = null;
                excelWorkbook = null;
            }

            // You can use the following method to automatically open the PDF after export if you wish
            // Make sure that the file actually exists first...
            if (System.IO.File.Exists(outputPath))
            {
                System.Diagnostics.Process.Start(outputPath);
            }

            return exportSuccessful;
        }

        private static void generarExcelARIVU(List<ArchivoExport> pAduana, int ROWS_FIRST, int ROWS_START, int Filas, int cuenta, cxExcel.XLWorkbook oWB, int iRow, EncaLiquid pEncabezado)
        {

            string nombreSheet = null;
            string valorCabecera = null;
            string predio = null;
            string naviera = null;



            try
            {

                foreach (var datos in pAduana)
                {
                    predio = datos.s_nom_predio;
                    naviera = datos.c_prefijo;
                    break;
                }

                valorCabecera = "LISTADO DE ARIVUS";

                string valSheet = naviera + "_" + predio;
                var oSheet = oWB.Worksheets.Add(valSheet);
                oSheet.PageSetup.Header.Clear();

                oSheet.Range("A1", "F2").Merge();
                oSheet.Cell("A1").Value = valorCabecera;
                oSheet.Cell("A1").Style.Font.Bold = true;
                oSheet.Cell("A1").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Cell("A1").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Cell("A1").Style.Font.FontSize = 18;

                oSheet.Range("A4", "B4").Merge();
                oSheet.Range("A5", "B5").Merge();
                oSheet.Range("A6", "B6").Merge();
                oSheet.Range("A7", "B7").Merge();
                oSheet.Range("A8", "B8").Merge();

                oSheet.Cell("A4").Value = "AGENCIA";
                oSheet.Cell("A5").Value = "VAPOR";
                oSheet.Cell("A6").Value = "PREDIO";
                oSheet.Cell("A7").Value = "FECHA DE ATRAQUE";
                oSheet.Cell("A8").Value = "FECHA DE DESATRAQUE";
                oSheet.Range("A4", "F8").Style.Font.FontSize = 14;

                oSheet.Range("C4", "F4").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C5", "F5").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C6", "F6").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C7", "F7").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C8", "F8").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;


                oSheet.Cell("C4").Value = naviera;
                oSheet.Cell("C4").Style.Font.Bold = true;
                oSheet.Cell("C5").Value = pEncabezado.n_buque;
                oSheet.Cell("C5").Style.Font.Bold = true;
                oSheet.Cell("C6").Value = predio;
                oSheet.Cell("C6").Style.Font.Bold = true;
                oSheet.Cell("C7").Value = pEncabezado.s_atraque;
                oSheet.Cell("C7").Style.Font.Bold = true;
                oSheet.Cell("C8").Value = pEncabezado.s_desatraque;
                oSheet.Cell("C8").Style.Font.Bold = true;



                oSheet.Cell("A10").Value = "CORR.";
                oSheet.Cell("B10").Value = "CONTENEDOR";
                oSheet.Cell("C10").Value = "TAMAÑO";
                oSheet.Cell("D10").Value = "ARIVU";
                oSheet.Cell("E10").Value = "F. VENCIMIENTO";
                oSheet.Cell("F10").Value = "F. FIN OPERACION";


                oSheet.Cell("F10").Style.Alignment.SetWrapText(true);
                oSheet.Cell("E10").Style.Alignment.SetWrapText(true);


                oSheet.Range("A10", "F10").Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                oSheet.Range("A10", "F10").Style.Font.Bold = true;
                oSheet.Range("A10", "F10").Style.Font.FontSize = 14;
                oSheet.Range("A10", "F10").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A10", "F10").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                //oSheet.Range("A8", "H8").Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                //oSheet.Range("A8", "H8").Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;


                oSheet.Column(1).Width = 7;
                oSheet.Column(2).Width = 21;
                oSheet.Column(3).Width = 15;
                oSheet.Column(4).Width = 15;
                oSheet.Column(5).Width = 25;
                oSheet.Column(6).Width = 25;

                oSheet.Range("A11", string.Concat("F", Filas)).Style.NumberFormat.SetFormat("@");


                oSheet.Range("A11", string.Concat("F", Filas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Range("A11", string.Concat("F", Filas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;

                oSheet.Range("A10", string.Concat("F", Filas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                oSheet.Range("A10", string.Concat("F", Filas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;


                int iCurrent = 0;

                foreach (var item in pAduana)
                {
                    iCurrent = ROWS_START + iRow;
                    oSheet.Row(iCurrent).Height = 20;
                    oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                    oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                    oSheet.Cell(iCurrent, 3).Value = item.c_tamaño;
                    oSheet.Cell(iCurrent, 4).Value = item.n_documento;
                    oSheet.Cell(iCurrent, 5).Value = item.s_fec_venc;
                    oSheet.Cell(iCurrent, 6).Value = pEncabezado.s_desatraque;


                    oSheet.Cell(iCurrent, 6).Style.Alignment.SetWrapText(true);

                    iRow = iRow + 1;
                }


                //oSheet.Cell(iCurrent + 2, 1).Value = "Referencias: PE Pedido especial";
                oSheet.Cell(iCurrent + 2, 1).Style.Font.Bold = true;
                oSheet.Range("A11", string.Concat("F", Filas)).Style.Font.FontSize = 14;

                //oSheet.Range("H9", string.Format("H{0}", Filas)).Style.Alignment.SetWrapText(true);
                oSheet.Range("A1", string.Concat("F", Filas + 2)).Style.Font.FontName = "Trebuchet MS";


                oSheet.Cell("C4").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C6").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C7").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Cell("C8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;


                oSheet.Cell("A4").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A6").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A7").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;
                oSheet.Cell("A8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;


                oSheet.PageSetup.PageOrientation = cxExcel.XLPageOrientation.Portrait;
                oSheet.PageSetup.FitToPages(1, 2);
                oSheet.PageSetup.PaperSize = cxExcel.XLPaperSize.LetterPaper;
                oSheet.PageSetup.VerticalDpi = 600;
                oSheet.PageSetup.HorizontalDpi = 600;
                oSheet.PageSetup.Margins.Top = 0.3;
                oSheet.PageSetup.Margins.Header = 0.40;
                oSheet.PageSetup.Margins.Footer = 0.20;

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        public string generarARIVUS(List<ArchivoExport> pLista, EncaLiquid pEncabezado)
        {


            const int ROWS_FIRST = 1;
            const int ROWS_START = 10;
            int Filas = 0;
            int iRow = 1;
            pRutas = null;


            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            //string s_archivo = System.Windows.Forms.Application.StartupPath + "\\EXP_PLANTILLA.xlsx";

            var oWB = new cxExcel.XLWorkbook();

            List<ArchivoExport> _list = new List<ArchivoExport>();

                        
            string _Exf5save = null;
            int contador = 1;
           
            try
            {



                _Exf5save = string.Format("{0}{1}", fullEXP_ARIVU, "EXP_ARIVU_" + pEncabezado.n_buque + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) +  ".xlsx");


                //string fechaC = f_llegada.Day + "/" + f_llegada.Month + "/" + f_llegada.Year;

                int Cuenta = 1;


                contador = 1;
                var _predios = (from a in pLista.OrderBy(a => a.c_corr_previo)
                                group a by a.s_nom_predio into g
                                select new
                                {
                                    s_nom_predio = g.Key
                                }).ToList();

                var _clientes = (from a in pLista.OrderBy(a => a.c_prefijo)
                                 group a by a.c_prefijo into g
                                 select new
                                 {
                                     c_prefijo = g.Key
                                 }).ToList();

                List<ArchivoExport> _listOr = new List<ArchivoExport>();
                List<ArchivoExport> _listaAOrdenar1 = new List<ArchivoExport>();

                pLista = pLista.OrderBy(a => a.c_correlativo).ToList();



                if (_predios.Count() > 0 && _clientes.Count() > 0)
                {
                    foreach (var item in _clientes)
                    {
                        foreach (var itemPredios in _predios)
                        {
                            var _lista = pLista.Where(c => c.c_prefijo.Equals(item.c_prefijo) && c.s_nom_predio.Equals(itemPredios.s_nom_predio)).ToList();

                            if (_lista.Count() > 0)
                            {
                                Filas = _lista.Count + ROWS_START;

                                generarExcelARIVU(_lista, ROWS_FIRST, ROWS_START, Filas, Cuenta, oWB, iRow, pEncabezado);
                                Cuenta = Cuenta + 1;
                                contador += 1;


                                foreach (var item_C in _lista)
                                {
                                    pLista.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                                }
                            }
                        }
                    }
                }



                oWB.SaveAs(_Exf5save);
                //  oWB1.SaveAs(_f4save);

                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return _Exf5save;


                //pPDF.Add(_f3save);

                //EnviarCorreoCO(pPDF, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

                //EnviarCorreoCOTEC(pCotecLst, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto, a_manifiesto);

               

            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);
                throw new Exception(errorMessage);
            }
            finally
            {

            }
        }

    }
}


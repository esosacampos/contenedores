using System;
using System.Collections.Generic;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Globalization;
using System.Web;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
//using Microsoft.Office.Interop.Excel;

using cxExcel = ClosedXML.Excel;

using msExcel = Microsoft.Office.Interop.Excel;
using VBIDE = Microsoft.Vbe.Interop;




namespace CEPA.CCO.Linq
{
    public class NotiAutorizados
    {
        public int IdDoc { get; set; }

        string fullPath = System.Configuration.ConfigurationManager.AppSettings["fullPath"];
        string fullPDF = System.Configuration.ConfigurationManager.AppSettings["fullPDF"];
        string fullIMDG = System.Configuration.ConfigurationManager.AppSettings["fullIMDG"];
        string fullPathC = System.Configuration.ConfigurationManager.AppSettings["fullPathC"];

        int Hoja = 3;
        List<string> pRutas = new List<string>();

        public void GenerarAplicacionCX(List<ArchivoAduana> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque,
              DateTime f_llegada, int pValor, int pCantidad, List<DetaNaviera> pCancelados)
        {
            // ArchivoExcelDAL.EliminarProceso();

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

            List<ArchivoAduana> _list = new List<ArchivoAduana>();
            string _f2save = string.Format("{0}{1}", fullPath, "ACO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
            string _f4save = string.Format("{0}{1}", fullPath, "ACO1_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
            string _f3save = string.Format("{0}{1}", fullPDF, "ACO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".pdf");
            string _f5save = string.Format("{0}{1}", fullPath, "ACO2_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
            string _f6save = string.Format("{0}{1}", fullPath, "ACO3_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");

            int Hwnd = 0;
            int Hwnd1 = 0;
            string ruta = null;

            try
            {


                List<EnvioAuto> listAuto = new List<EnvioAuto>();
                if (Convert.ToInt32(HttpContext.Current.Session["IdDoc"]) > 0)
                    listAuto = DetaNavieraDAL.ObtenerDocAu(Convert.ToInt32(HttpContext.Current.Session["IdDoc"]), DBComun.Estado.verdadero);

                string c_navi_corto = null, c_viaje = null;
                int n_manifiesto = 0;

                foreach (var item in listAuto)
                {
                    c_navi_corto = item.c_naviera_corto;
                    c_viaje = item.c_voyaje;
                    n_manifiesto = item.n_manifiesto;
                    break;
                }


                string fechaC = f_llegada.Day + "/" + f_llegada.Month + "/" + f_llegada.Year;

                int Aumenta = 3;
                int Cuenta = 1;
                for (int j = 1; j < 8; j++)
                {
                    if (j == 1)
                    {
                        var ordenar1 = DetaNavieraLINQ.EnvioArchivo(pLista, DBComun.Estado.verdadero, 7);
                        if (ordenar1.Count > 0)
                            _list.AddRange(ordenar1);
                    }

                    if (j <= 6)
                    {
                        var ordenar = DetaNavieraLINQ.EnvioArchivo(pLista, DBComun.Estado.verdadero, j);

                        if (ordenar.Count > 0)
                        {

                            //oSheet = (Worksheet)oWB.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                            if (Cuenta <= 3)
                            {

                                Filas = ordenar.Count + ROWS_START;
                                GenerarExcelCX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, j, iRow, d_naviera, false, n_manifiesto, c_viaje);
                                GenerarExcel2CX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB1, fechaC, j, iRow, d_naviera);
                                Cuenta = Cuenta + 1;
                            }
                            else
                            {
                                Filas = ordenar.Count + ROWS_START;
                                GenerarExcelCX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, j, iRow, d_naviera, false, n_manifiesto, c_viaje);
                                GenerarExcel2CX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB1, fechaC, j, iRow, d_naviera);
                                Aumenta = Aumenta + 1;
                            }





                            //  GenerarPDF(ordenar, c_llegada, d_buque, fechaC, j, d_naviera, document, writerPDF);                                    
                            //document.NewPage();

                            foreach (var item_C in ordenar)
                            {
                                pLista.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                            }
                        }
                    }
                }
                //  document.Close();
                //oSheet = EliminarHojas(oXL, oWB, oSheet, true);
                // oSheet1 = EliminarHojas(oXL1, oWB1, oSheet1, true);





                oWB.SaveAs(_f2save);
                oWB1.SaveAs(_f4save);



                //Load Workbook
                string _ruta = "@" + _f2save;

                //ClaveXLS(_f2save);



                //System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                //VBIDE.VBComponent oModule;

                #region "Excel Macro"

                /*oModule = oWB.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
               

                string sCode = @" Sub Macro1()
                                    Dim WS_Count As Integer
                                    Dim I As Integer
                                    WS_Count = ActiveWorkbook.Worksheets.Count
                                    Dim A As Integer
    
                                    A = 0
                                    
                                    For I = 1 To WS_Count    
       
                                        Sheets(I).Select     
       
                                        ActiveWindow.View = xlPageBreakPreview
                                        If ActiveSheet.VPageBreaks.Count > 0 Then
                                            ActiveSheet.VPageBreaks(1).DragOff Direction:=xlToRight, RegionIndex:=1
                                        End If
                                        ActiveWindow.View = xlNormalView
       
                                        A = A + 1

                                        Dim UltLinea As Long
                                        UltLinea = Range(""A"" & Rows.Count).End(xlUp).Row
                                        
                                        ActiveSheet.Range(Cells(9, 1), Cells(UltLinea, 2)).Select
                                       With Selection.Font
                                           .Size = 12
                                       End With
                                    Next I

                                End Sub";

                Object oMissing = System.Reflection.Missing.Value;


               // oModule.CodeModule.AddFromString(sCode);*/




                // oXL.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, oXL, new object[] { "Macro1" });

                #endregion

                /*CrearArchivo(_f4save, _f5save);

                CrearArchivo2(_f5save, _f6save);*/

                NotiAutorizados.ConvertExcelToPdf(_f4save, _f3save);


                if (pRutas == null)
                    pRutas = new List<string>();

                pRutas.Add(_f2save);
                pRutas.Add(_f3save);

                if (_list.Count > 0)
                    ruta = GenerarPeligrosidadCX(_list, c_cliente, d_naviera, c_llegada, IdReg, d_buque, f_llegada, pValor, pCantidad, n_manifiesto, c_viaje);

                if (ruta != null && ruta != "")
                    pRutas.Add(ruta);

                List<string> pPDF = new List<string>();                

                //EnviarCorreo(pRutas, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

                pPDF.Add(_f3save);

                //EnviarCorreoCO(pPDF, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

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


        //private static void ClaveXLS(string _f2save)
        //{
        //    Workbook book = new Workbook();
            
        //    book.LoadFromFile(_f2save);
            
        //    //Protect Workbook

        //    book.Protect("abc-123");

        //    //Save and Launch

        //    book.Save();
        //}

        private static void CrearArchivo(string _f4save, string _f5save)
        {
            GC.Collect();

            Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();

            excel1.DisplayAlerts = false;
            excel1.Visible = false;
            excel1.ScreenUpdating = false;

            excel1.AutomationSecurity = Microsoft.Office.Core.MsoAutomationSecurity.msoAutomationSecurityForceDisable;

            
            Microsoft.Office.Interop.Excel._Workbook xlWorkBook = excel1.Workbooks.Open(_f4save, missing,
               missing, missing, missing, missing, missing,
               missing, missing, missing, missing, missing,
               missing, missing, missing);
            xlWorkBook.Activate();

                    
            xlWorkBook.SaveAs(_f5save, Excel.XlFileFormat.xlExcel8,
            System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false,
            Excel.XlSaveAsAccessMode.xlShared, false, false, System.Reflection.Missing.Value,
            System.Reflection.Missing.Value, System.Reflection.Missing.Value);
           
           
            excel1.Quit();


            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel1);
            excel1 = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void CrearArchivo2(string _f5save, string _f6save)
        {
            GC.Collect();

            Excel.Application excel1 = new Microsoft.Office.Interop.Excel.Application();

            excel1.DisplayAlerts = false;
            excel1.Visible = false;
            excel1.ScreenUpdating = false;

            excel1.AutomationSecurity = Microsoft.Office.Core.MsoAutomationSecurity.msoAutomationSecurityForceDisable;


            Microsoft.Office.Interop.Excel._Workbook xlWorkBook = excel1.Workbooks.Open(_f5save, missing,
               missing, missing, missing, missing, missing,
               missing, missing, missing, missing, missing,
               missing, missing, missing);

            xlWorkBook.ActiveSheet();



            VBIDE.VBComponent oModule = xlWorkBook.VBProject.VBComponents.Add(Microsoft.Vbe.Interop.vbext_ComponentType.vbext_ct_StdModule);

            
            string sCode = @" Sub Macro2()
                                    Dim WS_Count As Integer
                                    Dim I As Integer
                                    WS_Count = ActiveWorkbook.Worksheets.Count
                                    Dim A As Integer
    
                                    A = 0
                                    
                                    For I = 1 To WS_Count    
       
                                        Sheets(I).Select     
       
                                        ActiveWindow.View = xlPageBreakPreview
                                        If ActiveSheet.VPageBreaks.Count > 0 Then
                                            ActiveSheet.VPageBreaks(1).DragOff Direction:=xlToRight, RegionIndex:=1
                                        End If
                                        ActiveWindow.View = xlNormalView
       
                                        A = A + 1

                                        Dim UltLinea As Long
                                        UltLinea = Range(""A"" & Rows.Count).End(xlUp).Row

                                        ActiveSheet.Range(Cells(9, 1), Cells(UltLinea, 1)).Select
                                        
                                        With Selection.Font
                                           .Size = 14
                                       End With
                                        
                                        ActiveSheet.Range(Cells(9, 2), Cells(UltLinea, 2)).Select
                                        
                                        With Selection.Font
                                           .Size = 18
                                       End With
                                        
                                        ActiveSheet.Range(Cells(9, 3), Cells(UltLinea, 3)).Select
                                        
                                        With Selection.Font
                                           .Size = 14
                                       End With
                                       
                                        ActiveSheet.Range(Cells(9, 4), Cells(UltLinea, 4)).Select
                                        
                                        With Selection.Font
                                           .Size = 14
                                       End With
    
                                       ActiveSheet.Range(Cells(9, 5), Cells(UltLinea, 5)).Select
                                        
                                        With Selection.Font
                                           .Size = 14
                                       End With
                                       
                                    Next I

                                End Sub";

            oModule.CodeModule.AddFromString(sCode);


            excel1.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, excel1, new object[] { "Macro2" });

            GC.Collect();

            xlWorkBook.SaveAs(_f5save, Excel.XlFileFormat.xlExcel8,
            System.Reflection.Missing.Value, System.Reflection.Missing.Value, false, false,
            Excel.XlSaveAsAccessMode.xlShared, false, false, System.Reflection.Missing.Value,
            System.Reflection.Missing.Value, System.Reflection.Missing.Value); 

            excel1.Quit();


            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel1);
            excel1 = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        
        public void GenerarAplicacion(List<ArchivoAduana> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque,
            DateTime f_llegada, int pValor, int pCantidad, List<DetaNaviera> pCancelados)
        {
           // ArchivoExcelDAL.EliminarProceso();

            const int ROWS_FIRST = 1;
            const int ROWS_START = 8;
            int Filas = 0;
            int iRow = 1;
            pRutas = null;


            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");


            //Document document = new Document(PageSize.LETTER, 10, 10, 10, 10);

            Excel.Application oXL = new Excel.Application();
            //Microsoft.Office.Interop.Excel._Application oXL = null;

            Excel.Workbook oWB = oXL.Workbooks.Add(1);
            msExcel.Sheets sheetP = null;
            //Excel.Workbook oWB = oXL.Workbooks.Add(System.Reflection.Missing.Value);
            //Excel._Workbook oWB = null;
            Excel.Worksheet oSheet = null;
            //Excel._Worksheet oSheet = null;
            Excel.Range oRng;
           // Microsoft.Office.Interop.Excel._Workbook oWB = null;

            Microsoft.Office.Interop.Excel._Application oXL1 = null;

            Microsoft.Office.Interop.Excel._Workbook oWB1 = null;         
           
           // Excel._Application
            //Excel._Worksheet oSheet = null;
           
            List<ArchivoAduana> _list = new List<ArchivoAduana>();
            string _f2save = string.Format("{0}{1}", fullPath, "ACO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
            string _f4save = string.Format("{0}{1}", fullPath, "ACO1_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
            string _f3save = string.Format("{0}{1}", fullPDF, "ACO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".pdf");
            int Hwnd = 0;
            int Hwnd1 = 0;
            string ruta = null;

            try
            {
               
                //Start Excel and get Application object.
                
                //oXL = new Microsoft.Office.Interop.Excel.Application();
                Hwnd = oXL.Application.Hwnd;

                oXL1 = new Microsoft.Office.Interop.Excel.Application();
                Hwnd1 = oXL1.Application.Hwnd;



                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                //oXL.Visible = true;
                //Get a new workbook.
               // oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                //oWB = oXL.Workbooks.Add(System.Reflection.Missing.Value);
                oWB1 = (Microsoft.Office.Interop.Excel._Workbook)(oXL1.Workbooks.Add(Missing.Value));

                //string fechaC = f_llegada.ToString("dd/MM/yyyy HH:mm");
                string fechaC = f_llegada.Day + "/" + f_llegada.Month + "/" + f_llegada.Year; 
                //PdfWriter writerPDF = PdfWriter.GetInstance(document, new FileStream(_f3save, FileMode.Create));
              //  document.Open();

                
                //Microsoft.Office.Interop.Excel._Worksheet oSheet = null;
                //Microsoft.Office.Interop.Excel.Range oRng = null;
                oRng = null;
                oSheet = null;

                Microsoft.Office.Interop.Excel._Worksheet oSheet1 = null;
                Microsoft.Office.Interop.Excel.Range oRng1 = null;

                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

                int Aumenta = 3;
                int Cuenta = 1;
                for (int j = 1; j < 8; j++)
                {
                    if (j == 1)
                    {                        
                        var ordenar1 = DetaNavieraLINQ.EnvioArchivo(pLista, DBComun.Estado.verdadero, 7);
                        if(ordenar1.Count > 0 )
                            _list.AddRange(ordenar1);
                    }

                    if (j <= 6)
                    { 
                    var ordenar = DetaNavieraLINQ.EnvioArchivo(pLista, DBComun.Estado.verdadero, j);

                    if (ordenar.Count > 0)
                    {

                        //oSheet = (Worksheet)oWB.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                        if (Cuenta <= 3)
                        {
                            sheetP = oWB.Sheets as msExcel.Sheets;
                            oSheet = (msExcel.Worksheet)sheetP.Add(sheetP[Cuenta], Type.Missing, Type.Missing, Type.Missing);
                            oSheet1 = (Microsoft.Office.Interop.Excel._Worksheet)oWB1.Worksheets[Cuenta];
                            Cuenta = Cuenta + 1;
                        }
                        else
                        {
                            sheetP = oWB.Sheets as msExcel.Sheets;
                            oSheet = (msExcel.Worksheet)sheetP.Add(Type.Missing, After: sheetP[Aumenta]);
                            //oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets.Add(Missing.Value, After: oWB.Worksheets[Aumenta]);
                            oSheet1 = (Microsoft.Office.Interop.Excel._Worksheet)oWB1.Sheets.Add(Missing.Value, After: oWB1.Worksheets[Aumenta]);
                            Aumenta = Aumenta + 1;
                        }




                        Filas = ordenar.Count + ROWS_START;
                        GenerarExcel(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, oSheet, oRng, fechaC, j, iRow, d_naviera, false);
                        //GenerarExcel2(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, oSheet1, oRng1, fechaC, j, iRow, d_naviera);
                        //  GenerarPDF(ordenar, c_llegada, d_buque, fechaC, j, d_naviera, document, writerPDF);                                    
                        //document.NewPage();

                        foreach (var item_C in ordenar)
                        {
                            pLista.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                        }
                    }
                    }
                }
              //  document.Close();
                //oSheet = EliminarHojas(oXL, oWB, oSheet, true);
                oSheet1 = EliminarHojas(oXL1, oWB1, oSheet1, true);

                VBIDE.VBComponent oModule;               

                #region "Excel Macro"

                oModule = oWB.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);
                
                string sCode = @" Sub Macro1()
                                    Dim WS_Count As Integer
                                    Dim I As Integer
                                    WS_Count = ActiveWorkbook.Worksheets.Count
                                    Dim A As Integer
    
                                    A = 0
                                    
                                    For I = 1 To WS_Count    
       
                                        Sheets(I).Select     
       
                                        ActiveWindow.View = xlPageBreakPreview
                                        If ActiveSheet.VPageBreaks.Count > 0 Then
                                            ActiveSheet.VPageBreaks(1).DragOff Direction:=xlToRight, RegionIndex:=1
                                        End If
                                        ActiveWindow.View = xlNormalView
       
                                        A = A + 1

                                        Dim UltLinea As Long
                                        UltLinea = Range(""A"" & Rows.Count).End(xlUp).Row
                                        
                                        ActiveSheet.Range(Cells(9, 1), Cells(UltLinea, 2)).Select
                                       With Selection.Font
                                           .Size = 12
                                       End With
                                    Next I

                                End Sub";

                Object oMissing = System.Reflection.Missing.Value;

               
                oModule.CodeModule.AddFromString(sCode);
              

                #endregion

                oXL.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, oXL, new object[] { "Macro1" });

                             
                oWB.SaveAs(_f2save, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                oWB.Close(true);
                oXL.Quit();

                oModule = oWB1.VBProject.VBComponents.Add(VBIDE.vbext_ComponentType.vbext_ct_StdModule);

                

                sCode = @" Sub Macro2()
                                    Dim WS_Count As Integer
                                    Dim I As Integer
                                    WS_Count = ActiveWorkbook.Worksheets.Count
                                    Dim A As Integer
    
                                    A = 0
                                    
                                    For I = 1 To WS_Count    
       
                                        Sheets(I).Select     
       
                                        ActiveWindow.View = xlPageBreakPreview
                                        If ActiveSheet.VPageBreaks.Count > 0 Then
                                            ActiveSheet.VPageBreaks(1).DragOff Direction:=xlToRight, RegionIndex:=1
                                        End If
                                        ActiveWindow.View = xlNormalView
       
                                        A = A + 1

                                        Dim UltLinea As Long
                                        UltLinea = Range(""A"" & Rows.Count).End(xlUp).Row

                                        ActiveSheet.Range(Cells(9, 1), Cells(UltLinea, 1)).Select
                                        
                                        With Selection.Font
                                           .Size = 14
                                       End With
                                        
                                        ActiveSheet.Range(Cells(9, 2), Cells(UltLinea, 2)).Select
                                        
                                        With Selection.Font
                                           .Size = 18
                                       End With
                                        
                                        ActiveSheet.Range(Cells(9, 3), Cells(UltLinea, 3)).Select
                                        
                                        With Selection.Font
                                           .Size = 14
                                       End With
                                       
                                        ActiveSheet.Range(Cells(9, 4), Cells(UltLinea, 4)).Select
                                        
                                        With Selection.Font
                                           .Size = 14
                                       End With
    
                                       ActiveSheet.Range(Cells(9, 5), Cells(UltLinea, 5)).Select
                                        
                                        With Selection.Font
                                           .Size = 14
                                       End With
                                       
                                    Next I

                                End Sub";

                oModule.CodeModule.AddFromString(sCode);


                oXL1.GetType().InvokeMember("Run", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, oXL1, new object[] { "Macro2" });

                oWB1.SaveAs(_f4save, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                oWB1.Close(true);
                oXL1.Quit();

                NotiAutorizados.ConvertExcelToPdf(_f4save, _f3save);


                if (pRutas == null)
                    pRutas = new List<string>();

                pRutas.Add(_f2save);
                pRutas.Add(_f3save);

                if (_list.Count > 0)
                    ruta = GenerarPeligrosidad(_list, c_cliente, d_naviera, c_llegada, IdReg, d_buque, f_llegada, pValor, pCantidad);

                if (ruta != null && ruta != "")
                    pRutas.Add(ruta);

                List<EnvioAuto> listAuto = new List<EnvioAuto>();
                if(Convert.ToInt32(HttpContext.Current.Session["IdDoc"]) > 0)
                    listAuto = DetaNavieraDAL.ObtenerDocAu(Convert.ToInt32(HttpContext.Current.Session["IdDoc"]), DBComun.Estado.verdadero);

                string c_navi_corto = null, c_viaje = null;
                int n_manifiesto = 0;

                foreach (var item in listAuto)
                {
                    c_navi_corto = item.c_naviera_corto;
                    c_viaje = item.c_voyaje;
                    n_manifiesto = item.n_manifiesto;
                    break;
                }


                EnviarCorreo(pRutas, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

                oModule = null;
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

                if (oXL != null)
                {
                    oXL.Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oXL);
                }

                if (oXL1 != null)
                {
                    oXL1.Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oXL1);
                }
                
               // ArchivoExcelDAL.EliminarProceso();
               if(oWB != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWB);

               if(oWB1 != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWB1);

             

                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

                ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(Hwnd);
                ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(Hwnd1);
                oXL = null;
                oXL1 = null;
                pRutas = null;
           }
        }

        public string GenerarPeligrosidad(List<ArchivoAduana> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque,
            DateTime f_llegada, int pValor, int pCantidad)
        {
            
            const int ROWS_FIRST = 1;
            const int ROWS_START = 8;
            int Filas = 0;
            int iRow = 1;

            Microsoft.Office.Interop.Excel.Application oXL = null;

            Microsoft.Office.Interop.Excel._Workbook oWB = null;

            int Hwnd = 0;

            //Excel._Worksheet oSheet = null;
            Microsoft.Office.Interop.Excel._Worksheet oSheet = null;
            Microsoft.Office.Interop.Excel.Range oRng = null;
            List<ArchivoAduana> _list = new List<ArchivoAduana>();

            string _f4save = string.Format("{0}{1}", fullIMDG, "IMDG_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");

            try
            {
                
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                Hwnd = oXL.Application.Hwnd;
                //oXL.Visible = true;
                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));

                string fechaC = f_llegada.ToString("dd/MM/yy HH:mm", CultureInfo.CreateSpecificCulture("es-SV"));


                

                if (pLista.Count > 0)
                {

                    oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                    Filas = pLista.Count + ROWS_START;
                    GenerarExcel(pLista, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, oSheet, oRng, fechaC, 7, iRow, d_naviera, true);

                    oSheet = EliminarHojas(oXL, oWB, oSheet, false);


                    oWB.SaveAs(_f4save, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                    oWB.Close(true);

                    oXL.Quit();

                   // if(oWB != null)
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWB);

                    //if (oXL != null)
                    //{
                    //    oXL.Quit();
                        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oXL);
                   // }

                    ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(Hwnd);
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

                // ArchivoExcelDAL.EliminarProceso();
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWB);

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oXL);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWB);
              
                ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(Hwnd);
                oWB = null;
                oXL = null;            
               
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

        private static void GenerarExcel(List<ArchivoAduana> pAduana, string c_llegada, int ROWS_FIRST, int ROWS_START, int Filas, string d_buque, Microsoft.Office.Interop.Excel._Worksheet oSheet, Microsoft.Office.Interop.Excel.Range oRng, string _Fecha, int pValor, int iRow, string d_naviera, bool Imdg)
        {
            try
            {
                switch (pValor)
                {
                    case 1:
                        oSheet.Name = "LLENOS";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN LLENOS";
                        break;
                    case 2:
                        oSheet.Name = "VACIOS";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS ";
                        break;                    
                    case 3:
                        oSheet.Name = "REEF";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN REFRIGERADOS A CONECTAR ";
                        break;
                    case 4:
                        oSheet.Name = "TRASBORDO";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN TRASBORDO";
                        break;
                    case 5:
                        oSheet.Name = "RET_DIR";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO ";
                        break;
                    case 6:
                        oSheet.Name = "RET_DIR_VACIO";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO VACÍOS";
                        break;         
                    case 7:
                        oSheet.Name = "CONTE_PELIGROSIDAD";
                        oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN PELIGROSIDAD";
                        break;                    
                    default:
                        break;
                }

                int n_manif = 0;
                string _rango = null;

                if (Imdg == true)
                {
                    _rango = "N";
                }
                else
                    _rango = "L";

                oSheet.get_Range("A6", string.Format("{0}6", _rango )).Merge();
                oSheet.get_Range("A6", string.Format("{0}6", _rango)).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A6", string.Format("{0}6", _rango)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oSheet.get_Range("A6", string.Format("{0}6", _rango)).Font.Size = 18;
                //Add table headers going cell by cell.
                oSheet.Cells[ROWS_FIRST, 1] = d_naviera;
                oSheet.get_Range("A1", string.Format("{0}1", _rango)).Merge();
                oSheet.get_Range("A1", string.Format("{0}1", _rango)).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A1", string.Format("{0}1", _rango)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oSheet.get_Range("A1", string.Format("{0}1", _rango)).Font.Size = 28;

                

                oSheet.Cells[3, 1] = d_buque;
                oSheet.Cells[4, 1] = c_llegada;
                oSheet.Cells[5, 1] = _Fecha;
                oSheet.get_Range("A4", "A4").NumberFormat = "0.0000";
                oSheet.get_Range("A5", "A5").NumberFormat = "dd/mm/yyyy";
                oSheet.get_Range("A3", "A5").Font.Bold = true;

               

                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.Cells[ROWS_START, 1] = "No.";
                oSheet.Cells[ROWS_START, 2] = "CONTENEDOR";
                oSheet.Cells[ROWS_START, 3] = "TIPO";
                oSheet.Cells[ROWS_START, 4] = "PESO EN KG";
                oSheet.Cells[ROWS_START, 5] = "TARA";
                oSheet.Cells[ROWS_START, 6] = "CONDICION";
                oSheet.Cells[ROWS_START, 7] = "No. MANIF";
                oSheet.Cells[ROWS_START, 8] = "CONSIGNATARIO";
                oSheet.Cells[ROWS_START, 9] = "PUERTO DE PRECEDENCIA";
                oSheet.Cells[ROWS_START, 10] = "PAÍS DE DESTINO";
                oSheet.Cells[ROWS_START, 11] = "UBICACIÓN";

                if (Imdg == true)
                {
                    oSheet.Cells[ROWS_START, 12] = "CLASE";
                    oSheet.Cells[ROWS_START, 13] = "No. ONU";
                    oSheet.Cells[ROWS_START, 14] = "OBSERVACIONES";
                }
                else
                    oSheet.Cells[ROWS_START, 12] = "OBSERVACIONES";

                oSheet.get_Range("E8", "E8").WrapText = true;
                oSheet.get_Range("A1", string.Format("{0}1", _rango)).Font.Bold = true;
                oSheet.get_Range("A6", string.Format("{0}6", _rango)).Font.Bold = true;
                oSheet.get_Range("A8", string.Format("{0}8", _rango)).Font.Bold = true;
                oSheet.get_Range("A8", string.Format("{0}8", _rango)).Interior.ColorIndex = 15;
                oSheet.get_Range("A8", string.Format("{0}8", _rango)).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oSheet.get_Range("A6", string.Format("{0}8", _rango)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oSheet.get_Range("A9", string.Concat("C", Filas)).NumberFormat = "@";
                oSheet.get_Range("D9", string.Concat("D", Filas)).NumberFormat = "0.00";
                oSheet.get_Range("E9", string.Concat("E", Filas)).NumberFormat = "0";
                oSheet.get_Range("F9", string.Format("{0}{1}", _rango, Filas)).NumberFormat = "@";
                oSheet.get_Range("A9", string.Concat("B", Filas)).Font.Size = 12;

                //oRng = oSheet.get_Range("A1", string.Concat("D", Filas));
                //oRng.EntireColumn.AutoFit();

                oRng = oSheet.get_Range("A8", string.Concat(string.Format("{0}", _rango), Filas));
                oRng.Borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                oRng.Borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;

                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
                oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);

                oRng = oSheet.get_Range("D9", string.Format("E{0}", Filas));
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;

                oRng = oSheet.get_Range("H9", string.Format("N{0}",Filas));
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;

                oRng = oSheet.get_Range("A8", string.Format("N{0}", Filas));
                oRng.Font.Size = 8;


                oSheet.PageSetup.LeftFooter = string.Format("&B Buque : {0} Fecha : {1} &B", d_buque, _Fecha);
                oSheet.PageSetup.CenterFooter = "Página &P of &N";
                

                oSheet.Columns[1].ColumnWidth = 10;
                oSheet.Columns[2].ColumnWidth = 14;
                oSheet.Columns[3].ColumnWidth = 10;
                oSheet.Columns[4].ColumnWidth = 8;
                oSheet.Columns[5].ColumnWidth = 4;
                oSheet.Columns[6].ColumnWidth = 8;
                oSheet.Columns[7].ColumnWidth = 8;
                oSheet.Columns[8].ColumnWidth = 26;           
                oSheet.Columns[9].ColumnWidth = 20;
                oSheet.Columns[10].ColumnWidth = 12;
                oSheet.Columns[11].ColumnWidth = 8;
                if (Imdg == true)
                {
                    oSheet.Columns[12].ColumnWidth = 8;
                    oSheet.Columns[13].ColumnWidth = 8;
                    oSheet.Columns[14].ColumnWidth = 12;

                }
                else
                    oSheet.Columns[12].ColumnWidth = 12;


                foreach (var item in pAduana)
                {
                    int iCurrent = ROWS_START + iRow;
                    oSheet.Cells[iCurrent, 1] = item.c_correlativo;
                    oSheet.Cells[iCurrent, 2] = item.n_contenedor;
                    oSheet.Cells[iCurrent, 3] = item.c_tamaño_c;
                    oSheet.Cells[iCurrent, 4] = item.v_peso;
                    oSheet.Cells[iCurrent, 5] = item.v_tara;
                    oSheet.Cells[iCurrent, 6] = item.b_condicion;
                    oSheet.Cells[iCurrent, 7] = "";
                    oSheet.Cells[iCurrent, 8] = item.s_consignatario;
                    oSheet.Cells[iCurrent, 9] = item.n_pais_origen;
                    oSheet.Cells[iCurrent, 10] = item.n_pais_destino;
                    oSheet.Cells[iCurrent, 11] = "";         
                                                 
                    if (Imdg == true)
                    {
                        oSheet.Cells[iCurrent, 12] = item.c_imo_imd;
                        oSheet.Cells[iCurrent, 13] = item.c_un_number;
                        oSheet.Cells[iCurrent, 14] = "";
                    }
                    else
                        oSheet.Cells[iCurrent, 12] = "";

                    iRow = iRow + 1;
                    n_manif = item.num_manif;
                }

             oSheet.Cells[3, 11] = "Manif. ADUANA";
             oSheet.Cells[3, 12] = n_manif;
               
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        //private static void GenerarPDF(List<ArchivoAduana> pAduana, string c_llegada, string d_buque, string _Fecha, int pValor, string d_naviera, Document document, PdfWriter pWriter)
        //{
        //    string Titulo = null;

        //    try
        //    {
        //        switch (pValor)
        //        {
        //            case 1:
        //                Titulo = "LISTADO DE CONTENEDORES DE IMPORTACIÓN LLENOS";
        //                break;
        //            case 2:
        //                Titulo = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS ";
        //                break;
        //            case 3:
        //                Titulo = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO ";
        //                break;
        //            case 4:
        //                Titulo = "LISTADO DE CONTENEDORES DE IMPORTACIÓN REFRIGERADOS ";
        //                break;
        //            case 5:
        //                Titulo = "LISTADO DE CONTENEDORES DE IMPORTACIÓN TRASBORDO";
        //                break;                
        //            default:
        //                break;
        //        }

        //        //Texto de cabecera
        //        Paragraph p = new Paragraph(d_naviera, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14f, iTextSharp.text.Font.BOLD));
        //        p.Alignment = Element.ALIGN_CENTER;
        //        p.SpacingAfter = 10f;
        //        document.Add(p);

        //        //Salto de linea
        //        document.Add(new Paragraph("\n"));

        //        // Detalle buque fecha 
        //        Paragraph buque = new Paragraph(d_buque, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f));
        //        buque.Alignment = Element.ALIGN_JUSTIFIED;
        //        document.Add(buque);

        //        // Detalle codigo de llegada
        //        Paragraph llegada = new Paragraph(c_llegada, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f));
        //        llegada.Alignment = Element.ALIGN_JUSTIFIED;
        //        document.Add(llegada);

        //        // Detalle fecha 
        //        Paragraph fecha = new Paragraph(_Fecha, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f));
        //        fecha.Alignment = Element.ALIGN_JUSTIFIED;
        //        document.Add(fecha);

        //        //Texto de cabecera
        //        Paragraph pTitulo = new Paragraph(Titulo, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD));
        //        pTitulo.Alignment = Element.ALIGN_CENTER;
        //        pTitulo.SpacingAfter = 10f;
        //        document.Add(pTitulo);

        //        p.Clear();
        //        buque.Clear();
        //        llegada.Clear();
        //        fecha.Clear();
        //        pTitulo.Clear();
        //        //Salto de linea
        //        document.Add(new Paragraph("\n"));

        //        // Creamos tabla
        //        PdfPTable unaTabla = new PdfPTable(9);
        //        unaTabla.WidthPercentage = 100;

        //        unaTabla.SetWidthPercentage(new float[] {30, 75, 45, 45, 45, 35, 160, 85, 70 }, PageSize.LETTER);
        //        unaTabla.HorizontalAlignment = Element.ALIGN_LEFT;


        //        //unaTabla.SpacingBefore = 1.0f;
        //        //unaTabla.SpacingAfter = 1.0f;

                

        //        //Cabeceras
        //        unaTabla.AddCell(new Paragraph("No.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));
        //        unaTabla.AddCell(new Paragraph("CONTENEDOR", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));
        //        unaTabla.AddCell(new Paragraph("TIPO", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));
        //        unaTabla.AddCell(new Paragraph("PESO EN KG.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));
        //        unaTabla.AddCell(new Paragraph("TARA", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));
        //        unaTabla.AddCell(new Paragraph("CONDICIÓN", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));
        //        unaTabla.AddCell(new Paragraph("CONSIGNATARIO", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));
        //        unaTabla.AddCell(new Paragraph("PAIS ORIGEN", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));
        //        unaTabla.AddCell(new Paragraph("PAIS DESTINO", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8f, iTextSharp.text.Font.BOLD)));                


        //        //// Formato 
        //        foreach (PdfPCell celda in unaTabla.Rows[0].GetCells())
        //        {
        //            celda.BackgroundColor = BaseColor.GRAY;
        //            celda.HorizontalAlignment = Element.ALIGN_CENTER;
        //            celda.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            celda.Padding = 3;
        //            celda.UseAscender = true;

        //        }

        //        unaTabla.HeaderRows = 1;

        //        iTextSharp.text.Font font8 = FontFactory.GetFont("ARIAL", 8f);
        //        iTextSharp.text.Font font9 = FontFactory.GetFont("ARIAL", 10f);


        //        foreach (ArchivoAduana item in pAduana)
        //        {                   
        //            unaTabla.AddCell(new Paragraph(item.c_correlativo.ToString(), font8));
        //            unaTabla.AddCell(new Paragraph(item.n_contenedor.ToString(), font9));
        //            unaTabla.AddCell(new Paragraph(item.c_tamaño_c.ToString(), font8));
        //            unaTabla.AddCell(new Paragraph(item.v_peso.ToString("N"), font8));
        //            unaTabla.AddCell(new Paragraph(item.v_tara.ToString("#.##"), font8));
        //            unaTabla.AddCell(new Paragraph(item.b_estado, font8));
        //            unaTabla.AddCell(new Paragraph(item.s_consignatario.ToString(), font8));
        //            unaTabla.AddCell(new Paragraph(item.n_pais_origen.ToString(), font8));
        //            unaTabla.AddCell(new Paragraph(item.n_pais_destino.ToString(), font8));                   
        //        }

        //        for (int i = 1; i < pAduana.Count + 1; i++)
        //        {
        //            foreach (PdfPCell celda in unaTabla.Rows[i].GetCells())
        //            {
        //                celda.HorizontalAlignment = Element.ALIGN_LEFT;
        //                celda.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                celda.UseAscender = true;
        //                celda.Padding = 2;
        //            }                    
        //        }


        //        //unaTabla.WriteSelectedRows(0, -1, 50, 700, pWriter.DirectContent);
        //        document.Add(unaTabla);


        //    }
        //    catch (Exception Ex)
        //    {
        //        throw new Exception(Ex.Message);
        //    }
        //}

        public void EnviarCorreo(List<string> pArchivo, string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente, string c_llegada, string c_navi_corto, string c_viaje, int n_manifiesto)
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
                    Html += "<font color=red> Este barco no posee contenedores clasificados con peligrosidad </font><br/> ";
                }

                Html += "<br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> ACCIONES A REALIZAR: </b></font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> TODOS: </b></font><br />";
                Html += "<font color=blue>Impresión de los listados para ser utilizado en la operación del buque</font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> NAVIERA: </b></font><br />";
                Html += "<font color=blue><b>Remitir a CEPA:</b> a) Dos copias impresas del Manifiesto de Carga; b) Por correo electrónico, en formato digital, el Manifiesto de Carga. </font><br /><br />";
                Html += "<font color=blue><b>ADUANA:</b> No requiere copia impresa del Manifiesto de Carga </font><br /><br />";

                _correo.Subject = string.Format("PASO 4 de 4: Autorización de Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Cod. de Llegada # {3}, Manifiesto de Aduana # {4}", c_navi_corto, d_buque, c_viaje, c_llegada, n_manifiesto);
                //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
                _correo.ListArch = pArchivo;
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_autoriza", DBComun.Estado.verdadero, c_cliente);


                List<Notificaciones> _listaCC = new List<Notificaciones>();

                if (c_cliente != "11" && c_cliente != "216")
                    _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_autoriza", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_autoriza", DBComun.Estado.verdadero, "219"));
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
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);                
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public void GenerarExcelDAN(List<ArchivoAduana> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque,
            DateTime f_llegada, int pValor, int pCantidad, List<DetaNaviera> pCancelados)
        {

            const int ROWS_FIRST = 1;
            int Filas = pLista.Count + ROWS_FIRST;
            int iRow = 1;           
            string _f2save = null;
            int Hwnd = 0;

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            Microsoft.Office.Interop.Excel.Application oXL = null;
            Microsoft.Office.Interop.Excel._Workbook oWB = null;
            Microsoft.Office.Interop.Excel._Worksheet oSheet = null;
            Microsoft.Office.Interop.Excel.Range oRng = null;
            try
            {
              
                string fullWeb = System.Configuration.ConfigurationManager.AppSettings["DAN"];
                _f2save = string.Format("{0}{1}", fullWeb, "DAN_" + c_cliente + "_" + DateTime.Now.ToString("ddMMyyyy", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
                


                if (System.IO.File.Exists(_f2save))
                {
                    File.Delete(_f2save);
                }

                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                Hwnd = oXL.Application.Hwnd;

                //oXL.Visible = true;
                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                //Add table headers going cell by cell.
                oSheet.Cells[ROWS_FIRST, 1] = "NUMERAL";
                oSheet.Cells[ROWS_FIRST, 2] = "IMO";
                oSheet.Cells[ROWS_FIRST, 3] = "VOYAGE";
                oSheet.Cells[ROWS_FIRST, 4] = "No. BL";
                oSheet.Cells[ROWS_FIRST, 5] = "CONTENEDOR";
                oSheet.Cells[ROWS_FIRST, 6] = "TAMAÑO/TIPO";
                oSheet.Cells[ROWS_FIRST, 7] = "ISO";
                oSheet.Cells[ROWS_FIRST, 8] = "PESO";
                oSheet.Cells[ROWS_FIRST, 9] = "ESTADO";
                oSheet.Cells[ROWS_FIRST, 10] = "CONSIGNATARIO";
                oSheet.Cells[ROWS_FIRST, 11] = "SELLO";
                oSheet.Cells[ROWS_FIRST, 12] = "PAIS DESTINO";
                oSheet.Cells[ROWS_FIRST, 13] = "PAIS ORIGEN";
                oSheet.Cells[ROWS_FIRST, 14] = "DESCRIPCIÓN";
                oSheet.Cells[ROWS_FIRST, 15] = "PROMANIFIESTO";
                oSheet.Cells[ROWS_FIRST, 16] = "TARA";
                oSheet.Cells[ROWS_FIRST, 17] = "REFRIGERADOS";
                oSheet.Cells[ROWS_FIRST, 18] = "DESPACHO DIRECTO";
                oSheet.Cells[ROWS_FIRST, 19] = "CLASE";
                oSheet.Cells[ROWS_FIRST, 20] = "No. ONU";
                oSheet.Cells[ROWS_FIRST, 21] = "TRASBORDO";
                oSheet.Cells[ROWS_FIRST, 22] = "CONDICION";

                //Format A1:D1 as bold, vertical alignment = center.


                oSheet.get_Range("A1", "V1").Font.Bold = true;
                oSheet.get_Range("A1", "V1").Interior.ColorIndex = 15;
                oSheet.get_Range("A1", "V1").VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
                oSheet.get_Range("A1", "V1").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oSheet.get_Range("A1", string.Concat("C", Filas)).NumberFormat = "0";
                oSheet.get_Range("D1", string.Concat("G", Filas)).NumberFormat = "@";
                oSheet.get_Range("H1", string.Concat("H", Filas)).NumberFormat = "0.00";
                oSheet.get_Range("I1", string.Concat("O", Filas)).NumberFormat = "@";
                oSheet.get_Range("P1", string.Concat("P", Filas)).NumberFormat = "0";
                oSheet.get_Range("Q1", string.Concat("V", Filas)).NumberFormat = "@";

                //oRng = oSheet.get_Range("A1", string.Concat("D", Filas));
                //oRng.EntireColumn.AutoFit();

                oRng = oSheet.get_Range("A1", string.Concat("U", Filas));
                oRng.Borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                oRng.Borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;

                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
                oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                oRng.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);



                oSheet.Columns[1].ColumnWidth = 10;
                oSheet.Columns[2].ColumnWidth = 8;
                oSheet.Columns[3].ColumnWidth = 8;
                oSheet.Columns[4].ColumnWidth = 11;
                oSheet.Columns[5].ColumnWidth = 14;
                oSheet.Columns[6].ColumnWidth = 14;
                oSheet.Columns[7].ColumnWidth = 14;
                oSheet.Columns[8].ColumnWidth = 9;
                oSheet.Columns[9].ColumnWidth = 11;
                oSheet.Columns[10].ColumnWidth = 45;
                oSheet.Columns[11].ColumnWidth = 22;
                oSheet.Columns[12].ColumnWidth = 13;
                oSheet.Columns[13].ColumnWidth = 28;
                oSheet.Columns[14].ColumnWidth = 135;
                oSheet.Columns[15].ColumnWidth = 88;
                oSheet.Columns[16].ColumnWidth = 7;
                oSheet.Columns[17].ColumnWidth = 14;
                oSheet.Columns[18].ColumnWidth = 18;
                oSheet.Columns[19].ColumnWidth = 6;
                oSheet.Columns[20].ColumnWidth = 15;
                oSheet.Columns[21].ColumnWidth = 14;
                oSheet.Columns[22].ColumnWidth = 11;
              

                foreach (var item in pLista)
                {
                    int iCurrent = ROWS_FIRST + iRow;
                    oSheet.Cells[iCurrent, 1] = item.c_correlativo;
                    oSheet.Cells[iCurrent, 2] = item.c_imo;
                    oSheet.Cells[iCurrent, 3] = item.c_voyage;
                    oSheet.Cells[iCurrent, 4] = item.n_BL;
                    oSheet.Cells[iCurrent, 5] = item.n_contenedor;
                    oSheet.Cells[iCurrent, 6] = item.c_tamaño_c;
                    oSheet.Cells[iCurrent, 7] = item.c_tamaño;
                    oSheet.Cells[iCurrent, 8] = item.v_peso;
                    oSheet.Cells[iCurrent, 9] = item.b_estado_c;
                    oSheet.Cells[iCurrent, 10] = item.s_consignatario;
                    oSheet.Cells[iCurrent, 11] = item.n_sello;
                    oSheet.Cells[iCurrent, 12] = item.n_pais_destino;
                    oSheet.Cells[iCurrent, 13] = item.n_pais_origen;
                    oSheet.Cells[iCurrent, 14] = item.s_comodity;
                    oSheet.Cells[iCurrent, 15] = item.s_promanifiesto;
                    oSheet.Cells[iCurrent, 16] = item.v_tara;
                    oSheet.Cells[iCurrent, 17] = item.b_reef;
                    oSheet.Cells[iCurrent, 18] = item.b_ret_dir;
                    oSheet.Cells[iCurrent, 19] = item.c_imo_imd;
                    oSheet.Cells[iCurrent, 20] = item.c_un_number;
                    oSheet.Cells[iCurrent, 21] = item.b_tranship;
                    oSheet.Cells[iCurrent, 22] = item.b_condicion;                    
                    iRow = iRow + 1;
                    
                }

               

                oWB.SaveAs(_f2save, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                oWB.Close(true);
                oXL.Quit();

                pRutas.Add(_f2save);

                //EnviarCorreo(pRutas, d_buque, pValor, pCantidad, pCancelados, "11", c_llegada);

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
                // ArchivoExcelDAL.EliminarProceso();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oXL);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWB);

                
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

                ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(Hwnd);
                oXL = null;
                pRutas = null;
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


        //private static void GenerarExcel2(List<ArchivoAduana> pAduana, string c_llegada, int ROWS_FIRST, int ROWS_START, int Filas, string d_buque, Microsoft.Office.Interop.Excel._Worksheet oSheet, Microsoft.Office.Interop.Excel.Range oRng, string _Fecha, int pValor, int iRow, string d_naviera)
        //{
        //    try
        //    {
        //        switch (pValor)
        //        {
        //            case 1:
        //                oSheet.Name = "LLENOS";
        //                oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN LLENOS";
        //                break;
        //            case 2:
        //                oSheet.Name = "VACIOS";
        //                oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS ";
        //                break;
        //            case 3:
        //                oSheet.Name = "REEF";
        //                oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN REFRIGERADOS A CONECTAR ";
        //                break;
        //            case 4:
        //                oSheet.Name = "TRASBORDO";
        //                oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN TRASBORDO";
        //                break;
        //            case 5:
        //                oSheet.Name = "RET_DIR";
        //                oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO ";
        //                break;
        //            case 6:
        //                oSheet.Name = "RET_DIR_VACIO";
        //                oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN DESPACHO DIRECTO VACÍOS";
        //                break;
        //            case 7:
        //                oSheet.Name = "CONTE_PELIGROSIDAD";
        //                oSheet.Cells[6, 1] = "LISTADO DE CONTENEDORES DE IMPORTACIÓN PELIGROSIDAD";
        //                break;
        //            default:
        //                break;
        //        }

        //        string _rango = null;

               
        //        _rango = "G";
               
        //        oSheet.get_Range("A6", string.Format("{0}6", _rango)).Merge();
        //        oSheet.get_Range("A6", string.Format("{0}6", _rango)).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        //        oSheet.get_Range("A6", string.Format("{0}6", _rango)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //        oSheet.get_Range("A6", string.Format("{0}6", _rango)).Font.Size = 18;
        //        //Add table headers going cell by cell.
        //        oSheet.Cells[ROWS_FIRST, 1] = d_naviera;
        //        oSheet.get_Range("A1", string.Format("{0}1", _rango)).Merge();
        //        oSheet.get_Range("A1", string.Format("{0}1", _rango)).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        //        oSheet.get_Range("A1", string.Format("{0}1", _rango)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //        oSheet.get_Range("A1", string.Format("{0}1", _rango)).Font.Size = 28;



        //        oSheet.Cells[3, 1] = d_buque;
        //        oSheet.Cells[4, 1] = c_llegada;
        //        oSheet.Cells[5, 1] = _Fecha;
        //        oSheet.get_Range("A4", "A4").NumberFormat = "0.0000";
        //        oSheet.get_Range("A5", "A5").NumberFormat = "mm/dd/yyyy";
        //        oSheet.get_Range("A3", "A5").Font.Bold = true;
        //        oSheet.get_Range("A3", "A5").Font.Size = 14;


        //        //Format A1:D1 as bold, vertical alignment = center.
        //        oSheet.Cells[ROWS_START, 1] = "No.";
        //        oSheet.Cells[ROWS_START, 2] = "CONTENEDOR";
        //        oSheet.Cells[ROWS_START, 3] = "TIPO";
        //        oSheet.Cells[ROWS_START, 4] = "PESO EN KG";
        //        oSheet.Cells[ROWS_START, 5] = "TARA";              
        //        oSheet.Cells[ROWS_START, 6] = "UBICACIÓN";                
        //        oSheet.Cells[ROWS_START, 7] = "OBSERVACIONES";

        //        oSheet.get_Range("E8", "E8").WrapText = true;
        //        oSheet.get_Range("A1", string.Format("{0}1", _rango)).Font.Bold = true;
        //        oSheet.get_Range("A6", string.Format("{0}6", _rango)).Font.Bold = true;
        //        oSheet.get_Range("A8", string.Format("{0}8", _rango)).Font.Bold = true;
        //        oSheet.get_Range("A8", string.Format("{0}8", _rango)).Interior.ColorIndex = 15;
        //        oSheet.get_Range("A8", string.Format("{0}8", _rango)).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        //        oSheet.get_Range("A6", string.Format("{0}8", _rango)).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //        oSheet.get_Range("A9", string.Concat("C", Filas)).NumberFormat = "@";
        //        oSheet.get_Range("D9", string.Concat("D", Filas)).NumberFormat = "0.00";
        //        oSheet.get_Range("E9", string.Concat("E", Filas)).NumberFormat = "0";
        //        oSheet.get_Range("F9", string.Format("{0}{1}", _rango, Filas)).NumberFormat = "@";
        //        oSheet.get_Range("A9", string.Concat("B", Filas)).Font.Size = 14;

        //        //oRng = oSheet.get_Range("A1", string.Concat("D", Filas));
        //        //oRng.EntireColumn.AutoFit();

        //        oRng = oSheet.get_Range("A8", string.Concat(string.Format("{0}", _rango), Filas));
        //        oRng.Borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
        //        oRng.Borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
        //        oRng.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
        //        oRng.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
        //        oRng.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
        //        oRng.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
        //        oRng.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
        //        oRng.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;

        //        oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //        oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
        //        oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
        //        oRng.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);

        //        oRng = oSheet.get_Range("D9", string.Format("E{0}", Filas));
        //        oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //        oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
               

        //        oRng = oSheet.get_Range("A8", string.Format("G{0}", Filas));
        //        oRng.Font.Size = 12;
        //        oRng.RowHeight = 38;



        //        oSheet.Columns[1].ColumnWidth = 13;
        //        oSheet.Columns[2].ColumnWidth = 21;
        //        oSheet.Columns[3].ColumnWidth = 13;
        //        oSheet.Columns[4].ColumnWidth = 15;
        //        oSheet.Columns[5].ColumnWidth = 8;
        //        oSheet.Columns[6].ColumnWidth = 25;
        //        oSheet.Columns[7].ColumnWidth = 50;

        //        oSheet.PageSetup.LeftFooter = string.Format("&B Buque : {0} Fecha : {1} &B", d_buque, _Fecha);
        //        oSheet.PageSetup.CenterFooter = "Página &P of &N";


        //        foreach (var item in pAduana)
        //        {
        //            int iCurrent = ROWS_START + iRow;
        //            oSheet.Cells[iCurrent, 1] = item.c_correlativo;
        //            oSheet.Cells[iCurrent, 2] = item.n_contenedor;
        //            oSheet.Cells[iCurrent, 3] = item.c_tamaño_c;
        //            oSheet.Cells[iCurrent, 4] = item.v_peso;
        //            oSheet.Cells[iCurrent, 5] = item.v_tara;
        //            oSheet.Cells[iCurrent, 6] = "";
        //            oSheet.Cells[iCurrent, 7] = "";                   

        //            iRow = iRow + 1;
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw new Exception(Ex.Message);
        //    }
        //}

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

                oSheet.Range("A4", "A4").Style.NumberFormat.SetFormat("0.0000");
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
                oSheet.Cell(ROWS_START, 7).Value = "No. MANIF";
                oSheet.Cell(ROWS_START, 8).Value = "CONSIGNATARIO";
                oSheet.Cell(ROWS_START, 9).Value = "DESCRIPCIÓN DE MERCADERÍA";
                oSheet.Cell(ROWS_START, 10).Value = "PAÍS DE PROCEDENCIA";
                oSheet.Cell(ROWS_START, 11).Value = "PAÍS DE DESTINO";
                oSheet.Cell(ROWS_START, 12).Value = "UBICACIÓN";

                if (Imdg == true)
                {
                    oSheet.Cell(ROWS_START, 13).Value = "CLASE";
                    oSheet.Cell(ROWS_START, 14).Value = "No. ONU";
                    oSheet.Cell(ROWS_START, 15).Value = "OBSERVACIONES";
                }
                else
                    oSheet.Cell(ROWS_START, 13).Value = "OBSERVACIONES";

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

                oSheet.Range(string.Format("{0}9", _rango), string.Format("{0}{1}", _rango, Filas)).Style.Font.FontSize = 12;
                oSheet.Range(string.Format("{0}9", _rango), string.Format("{0}{1}", _rango, Filas)).Style.Font.Bold = true;               

                oSheet.PageSetup.Footer.Left.AddText(string.Format("&B Buque : {0} Fecha : {1} &B", d_buque, _Fecha));
                oSheet.PageSetup.Footer.Center.AddText("Página ", cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.PageNumber, cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(" de ", cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.NumberOfPages, cxExcel.XLHFOccurrence.AllPages);


                
                oSheet.Column(1).Width = 3;
                oSheet.Column(2).Width = 20;
                oSheet.Column(3).Width = 10;
                oSheet.Column(4).Width = 8;
                oSheet.Column(5).Width = 5;
                oSheet.Column(6).Width = 8;
                oSheet.Column(7).Width = 8;
                oSheet.Column(8).Width = 29;
                oSheet.Column(9).Width = 29;
                oSheet.Column(10).Width = 20;
                oSheet.Column(11).Width = 12;
                oSheet.Column(12).Width = 11;
                if (Imdg == true)
                {
                    oSheet.Column(13).Width = 8;
                    oSheet.Column(14).Width = 8;
                    oSheet.Column(15).Width = 12;

                }
                else
                    oSheet.Column(13).Width = 12;


                foreach (var item in pAduana)
                {
                    int iCurrent = ROWS_START + iRow;
                    
                    oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                    oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                    oSheet.Cell(iCurrent, 3).Value = item.c_tamaño_c;
                    oSheet.Cell(iCurrent, 4).Value = item.v_peso;
                    oSheet.Cell(iCurrent, 5).Value = item.v_tara;
                    oSheet.Cell(iCurrent, 6).Value = item.b_condicion;
                    oSheet.Cell(iCurrent, 7).Value = "";
                    oSheet.Cell(iCurrent, 8).Value = item.s_consignatario;
                    oSheet.Cell(iCurrent, 9).Value = item.s_comodity;
                    oSheet.Cell(iCurrent, 10).Value = item.n_pais_origen;
                    oSheet.Cell(iCurrent, 11).Value = item.n_pais_destino;
                    oSheet.Cell(iCurrent, 12).Value = "";

                    if (Imdg == true)
                    {
                        oSheet.Cell(iCurrent, 13).Value = item.c_imo_imd;
                        oSheet.Cell(iCurrent, 14).Value = item.c_un_number;
                        if (item.b_condicion.Substring(0, 3) == "LCL")
                            oSheet.Cell(iCurrent, 15).Value = "LCL";
                        else
                            oSheet.Cell(iCurrent, 15).Value = "";
                    }
                    else
                    {
                        if (item.b_condicion.Substring(0, 3) == "LCL")
                            oSheet.Cell(iCurrent, 13).Value = "LCL";
                        else
                            oSheet.Cell(iCurrent, 13).Value = "";
                    }

                    iRow = iRow + 1;                    
                }

               

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
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

                _correo.Subject = string.Format("PASO 4 de 4: Autorización de Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Cod. de Llegada # {3}, Manifiesto de Aduana # {4}", c_navi_corto, d_buque, c_viaje, c_llegada, n_manifiesto);
                //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
                _correo.ListArch = pArchivo;
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_contratista", DBComun.Estado.verdadero, "17");


                List<Notificaciones> _listaCC = new List<Notificaciones>();
                _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_contratista", DBComun.Estado.verdadero, "17");


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
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    }
}


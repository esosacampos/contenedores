using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.OleDb;
using CEPA.CCO.Entidades;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Web;
using System.IO;
using System.ComponentModel;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;



namespace CEPA.CCO.DAL
{
    public static class ArchivoExcelDAL
    {
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static bool TryKillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0) return false;
            try
            {
                Process.GetProcessById((int)processID).Kill();
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Win32Exception)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }

        public static void KillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0)
                throw new ArgumentException("Process has not been found by the given main window handle.", "hWnd");
            Process.GetProcessById((int)processID).Kill();
        }

        public static bool GetFormato(string rutaLibro, string nombreHoja)
        {

            

            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            int Hwnd = app.Application.Hwnd;
                        
            int numFilas = GetRowExcel(rutaLibro, DBComun.Estado.verdadero, nombreHoja);
            object misValue = System.Reflection.Missing.Value;
            try
            {
                // Abrimos el libro de trabajo
                //
                //EliminarProceso();
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-SV");
                //Microsoft.Office.Interop.Excel._Workbook wrkBooks;
                // wrkBooks = app.Workbooks.Open(rutaLibro);
                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Open(rutaLibro);
                
                // Referenciamos la hoja cuyo índice sea 1.
                //
                Microsoft.Office.Interop.Excel.Worksheet ws = ((Microsoft.Office.Interop.Excel.Worksheet)(wb.Worksheets.Item[1]));
                Microsoft.Office.Interop.Excel.Range oRng;
                oRng = ws.get_Range("C1", "C" + numFilas);                
                oRng.NumberFormat = "@";

                string _f2save = string.Format("{0}\\{1}", HttpContext.Current.Session["rut"].ToString(), "BookingPlantilla.xls");

                if (System.IO.File.Exists(_f2save))
                {
                    File.Delete(_f2save);
                }
                                             
               
               wb.SaveAs(_f2save, misValue, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlShared,misValue, misValue, misValue, misValue, misValue);
               HttpContext.Current.Session["nuevoArch"] = _f2save;
             
                ws = null;
                wb = null;

                
                return true;

            }
            catch (Exception ex)
            {              
                return false;
            }
            finally
            {
                // Cerramos Excel.
                //               
                if (((app != null)))
                    app.Quit();
               
                // Disminuimos el contador de referencias y liberamos el objeto.
                //
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

                TryKillProcessByMainWindowHwnd(Hwnd);

                app = null;
               
            }

        }

        public static void EliminarProceso()
        {
            Process[] process;
           
            foreach (Process proceso in Process.GetProcesses())
            {
                if (proceso.ProcessName == "EXCEL" || proceso.ProcessName == "AcroRd32")
                {
                    proceso.Kill();
                }
            }
        }   

        public static string GetNombre(string rutaLibro)
        {
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");
           // EliminarProceso();
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet ws = null;
            int hWnd=0;
            try
            {

                hWnd = app.Application.Hwnd;
                // Abrimos el libro de trabajo
                //
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-SV");
                //Microsoft.Office.Interop.Excel._Workbook wrkBooks;
               // wrkBooks = app.Workbooks.Open(rutaLibro);
                wb = app.Workbooks.Open(rutaLibro);

                // Referenciamos la hoja cuyo índice sea 1.
                //
                ws = ((Microsoft.Office.Interop.Excel.Worksheet)(wb.Worksheets.Item[1]));
                
                // Obtenemos el nombre de la hoja.
                //
                string name = ws.Name;               

                // Cerramos el libro
                //
                wb.Close(true);
                app.Quit();
                

                // Devolvemos el nombre de la hoja
                //               
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

                return name;

            }
            catch (Exception ex)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, ex.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, ex.Source);
                throw new Exception(errorMessage);

            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wb);

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(app);
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

                ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(hWnd);
                ws = null;
                wb = null;
                app = null;               

            }

        }

        public static int GetRowExcel(string sRuta, DBComun.Estado pTipo, string nombreHoja)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
            {
                _conn.Open();

                string _consulta = @"SELECT COUNT(*) 
                                    FROM [{0}$]";

                OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja), _conn as OleDbConnection);
                _command.CommandType = CommandType.Text;

                int _reader = Convert.ToInt32(_command.ExecuteScalar());
            
                _conn.Close();
                return _reader;
            }                       
        }

        public static string ValidarViajeC(DBComun.Estado pTipo, string c_imo, string c_viaje, string c_naviera)
        {
            try
            {
                using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
                {
                    _conn.Open();

                    string _consulta = @"IF EXISTS(SELECT c_imo FROM CCO_ENCA_NAVIERAS WHERE c_imo = '{0}' AND c_voyage = '{1}' AND c_naviera = '{2}')
                                    BEGIN
	                                    SELECT 'ESTE IMO YA PRESENTO ESTE NÚMERO DE VIAJE'
                                    END
                                    ELSE
	                                    SELECT 'NULL'";

                    SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_viaje, c_naviera), _conn as SqlConnection);
                    _command.CommandType = CommandType.Text;

                    string _reader = _command.ExecuteScalar().ToString();

                    _conn.Close();
                    return _reader;
                }
            }
            catch
            {
                return "Error";
            }
        }

        public static string ValidarViajeCEx(DBComun.Estado pTipo, string c_imo, string c_viaje, string c_naviera)
        {
            try
            {
                using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
                {
                    _conn.Open();

                    string _consulta = @"IF EXISTS(SELECT c_imo FROM CCO_ENCA_EXPO_NAVI WHERE c_imo = '{0}' AND c_voyage = '{1}' AND c_naviera = '{2}')
                                    BEGIN
	                                    SELECT 'ESTE IMO YA PRESENTO ESTE NÚMERO DE VIAJE'
                                    END
                                    ELSE
	                                    SELECT 'NULL'";

                    SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_viaje, c_naviera), _conn as SqlConnection);
                    _command.CommandType = CommandType.Text;

                    string _reader = _command.ExecuteScalar().ToString();

                    _conn.Close();
                    return _reader;
                }
            }
            catch
            {
                return "Error";
            }
        }

        public static List<ArchivoExcel> GetRango(string sRuta, DBComun.Estado pTipo)
        {
            List<ArchivoExcel> listaArch = new List<ArchivoExcel>();

            string nombreHoja = GetNombre(sRuta);

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
            {
                _conn.Open();

                int num_fila = 2;
                string celdaClase = "";
                string celda = "";
                string celdaSello = "";
                string celdaPel = "";
                object Valor = null;
                object Valor2 = null;
                object Valor3 = null;
                object Valor4 = null;
                object Valor5 = null;
                object Valor9 = null;
                
                int fila = GetRowExcel(sRuta, DBComun.Estado.verdadero, nombreHoja) + 1;

               // string _consulta = @"SELECT * FROM [{0}$A1:V{1}]";

                //string _consulta = @"SELECT * FROM [{0}$A1:V{1}]";
                //string _consulta = @"SELECT * FROM [{0}$A1:W{1}]";
                string _consulta = @"SELECT * FROM [{0}$A1:Z{1}]";

                OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja, fila), _conn as OleDbConnection);
                _command.CommandType = CommandType.Text;

                OleDbDataReader _reader = _command.ExecuteReader();
                try
                {
                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            if (!_reader.IsDBNull(0))
                            {

                                if (!_reader.IsDBNull(18))
                                {
                                    if (ArchivoBookingDAL.isNumeric(_reader[18]))
                                        Valor = Convert.ToString(_reader[18]);
                                    else
                                        Valor = _reader[18];
                                }

                                if (!_reader.IsDBNull(19))
                                {
                                    if (ArchivoBookingDAL.isNumeric(_reader[19]))
                                        Valor2 = Convert.ToString(_reader[19]);
                                    else
                                        Valor2 = _reader[19];
                                }

                                if (!_reader.IsDBNull(2))
                                {
                                    if (ArchivoBookingDAL.isNumeric(_reader[2]))
                                        Valor3 = Convert.ToString(_reader[2]);
                                    else
                                        Valor3 = _reader[2];

                                }

                                if (!_reader.IsDBNull(3))
                                {
                                    if (ArchivoBookingDAL.isNumeric(_reader[3]))
                                        Valor4 = Convert.ToString(_reader[3]);
                                    else
                                        Valor4 = _reader[3];

                                }

                                if (!_reader.IsDBNull(1))
                                {
                                    if (ArchivoBookingDAL.isNumeric(_reader[1].ToString().TrimEnd().TrimStart()))
                                        Valor5 = Convert.ToString(_reader[1]);
                                    else
                                        Valor5 = _reader[1];
                                }

                                if (!_reader.IsDBNull(9))
                                {
                                    if (ArchivoBookingDAL.isNumeric(_reader[9]))
                                        Valor9 = Convert.ToString(_reader[9]);
                                    else
                                        Valor9 = _reader[9];
                                }

                                ArchivoExcel _tmpArc = new ArchivoExcel
                                {
                                    num_fila = num_fila,
                                    //num_manif = (int)_reader.GetDouble(0),                                    
                                    c_imo = Convert.ToDouble(Valor5.ToString().TrimStart().TrimEnd()),
                                    c_voyage = _reader.IsDBNull(2) ? "" : Valor3.ToString(),
                                    celda = "D",
                                    n_BL = _reader.IsDBNull(3) ? "" : Valor4.ToString(),
                                    n_contenedor = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                                    c_tamaño = _reader.GetString(5),
                                    v_peso = _reader.IsDBNull(6) ? 0.00 : (double)_reader.GetDouble(6),
                                    b_estado = _reader.GetString(7),
                                    s_consignatario = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                                    celdaSello = "J",
                                    n_sello = _reader.IsDBNull(9) ? "" : Valor9.ToString(),
                                    c_pais_destino = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                                    c_pais_origen = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                                    c_detalle_pais = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                                    s_comodity = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                                    s_prodmanifestado = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                                    v_tara = (double)_reader.GetDouble(15),
                                    b_reef = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                                    b_ret_dir = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                                    celdaPel = "S",
                                    c_imo_imd = _reader.IsDBNull(18) ? "" : Valor.ToString().Replace(",", "."),
                                    celdaClase = "T",
                                    c_un_number = _reader.IsDBNull(19) ? "" : Valor2.ToString(),
                                    b_transhipment = _reader.IsDBNull(20) ? "" : _reader.GetString(20),
                                    c_condicion = _reader.IsDBNull(21) ? "" : _reader.GetString(21),
                                    b_shipper = _reader.IsDBNull(22) ? "" : _reader.GetString(22),
                                    celdaTrans = "X",
                                    b_transferencia = _reader.IsDBNull(23) ? "" : _reader.GetString(23).Trim().TrimEnd().TrimStart(),
                                    celdaMan = "Y",
                                    b_manejo = _reader.IsDBNull(24) ? "" : _reader.GetString(24).Trim().TrimEnd().TrimStart(),
                                    celdaDes = "Z",
                                    b_despacho = _reader.IsDBNull(25) ? "" : _reader.GetString(25).Trim().TrimEnd().TrimStart()
                                };
                                listaArch.Add(_tmpArc);
                                num_fila = num_fila + 1;

                                celda = _tmpArc.celda;
                                celda = _tmpArc.celdaSello;
                                celda = _tmpArc.celdaPel;
                                celda = _tmpArc.celdaClase;
                                celda = _tmpArc.celdaTrans;
                                celda = _tmpArc.celdaMan;
                                celda = _tmpArc.celdaDes;
                                    

                            }
                            else
                                break;
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message + "Celda: " + celda + num_fila.ToString());
                }
                finally
                {
                    _reader.Close();
                    _conn.Close();
                }
              
                return listaArch;
            }
        }

        public static List<ArchivoExcel> GetVoyage(string sRuta, DBComun.Estado pTipo)
        {
            List<ArchivoExcel> listaArch = new List<ArchivoExcel>();
            string nombreHoja = GetNombre(sRuta);
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
            {
                _conn.Open();
                object Valor3 = null;

                int fila = GetRowExcel(sRuta, DBComun.Estado.verdadero, nombreHoja) + 1;

                string _consulta = @"SELECT * FROM [{0}$A1:W{1}]";

                OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja, fila), _conn as OleDbConnection);
                _command.CommandType = CommandType.Text;

                OleDbDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    if (!_reader.IsDBNull(2))
                    {
                        if (ArchivoBookingDAL.isNumeric(_reader[2]))
                            Valor3 = Convert.ToString(_reader[2]);
                        else
                            Valor3 = _reader[2];

                    }

                    ArchivoExcel _tmpArc = new ArchivoExcel
                    {
                        c_voyage = Valor3.ToString()
                    };
                    listaArch.Add(_tmpArc);

                    if (listaArch.Count == 1)
                        break;
                }

                _reader.Close();
                _conn.Close();
                return listaArch;
            }
        }
        

        public static string GetVoyageEx(string sRuta, DBComun.Estado pTipo)
        {
            string filePath = @HttpContext.Current.Server.MapPath("~/Archivos/" + Path.GetFileName(sRuta));

            string c_viaje = null;
          

            c_viaje = GetCellValue(filePath, "1", "B2");

            //System.Data.DataTable dt = ExtractExcelSheetValuesToDataTable(filePath, "1");

            //int q = dt.Rows.Count;

            //List<ArchivoExport> pListaC = ConvertToList(dt);
          
         
            return c_viaje;

        }


        public static List<ArchivoExport> GetListaEx(string sRuta, DBComun.Estado pTipo)
        {
            string filePath = @HttpContext.Current.Server.MapPath("~/Archivos/" + Path.GetFileName(sRuta));

            List<ArchivoExport> pListaC = new List<ArchivoExport>();          

            System.Data.DataTable dt = ExtractExcelSheetValuesToDataTable(filePath, "1");

            if (dt.Rows.Count > 0)
            {
                pListaC = ConvertToList(dt);
            }

            return pListaC;

        }

        public static string GetImoEx(string sRuta, DBComun.Estado pTipo)
        {
            string filePath = @HttpContext.Current.Server.MapPath("~/Archivos/" + Path.GetFileName(sRuta));

            string c_imo = null;


            c_imo = GetCellValue(filePath, "1", "A2");

           
            return c_imo;

        }

        public static int GetHojasEx(string sRuta, DBComun.Estado pTipo)
        {
            string filePath = @HttpContext.Current.Server.MapPath("~/Archivos/" + Path.GetFileName(sRuta));

            int c_hojas = 0;


            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
            {
                // Retrieve a reference to the workbook part.
                DocumentFormat.OpenXml.Spreadsheet.Sheets wbPart = document.WorkbookPart.Workbook.Sheets;

                c_hojas = wbPart.Count();                

                
            }

            //System.Data.DataTable dt = ExtractExcelSheetValuesToDataTable(filePath, "1");

            //int q = dt.Rows.Count;

            //List<ArchivoExport> pListaC = ConvertToList(dt);


            return c_hojas;

        }

        public static List<ArchivoExport> ConvertToList(System.Data.DataTable dt)
        {
            List<ArchivoExport> pLista = new List<ArchivoExport>();

            int valor = 0;
            double valorD, valorD1, valorD2 = 0.00;
            int n_fila = 2;
            
            foreach (DataRow row in dt.Rows)
            {

                double.TryParse(row[0].ToString(), out valorD);
                double.TryParse(row[6].ToString(), out valorD1);
                double.TryParse(row[16].ToString(), out valorD2);
                int.TryParse(row[3].ToString(), out valor);

                pLista.Add(new ArchivoExport
                {
                    
                    c_imo = valorD,
                    c_voyage = row.IsNull(1) ? "" : row[1].ToString(),                 
                    n_BL = row.IsNull(2) ? "" : row[2].ToString(),
                    n_booking = valor,
                    n_contenedor = row.IsNull(4) ? "" : row[4].ToString(),
                    c_tamaño = row.IsNull(5) ? "" : row[5].ToString(),
                    v_peso = valorD1,
                    b_estado = row.IsNull(7) ? "" : row[7].ToString(),
                    s_exportador = row.IsNull(8) ? "" : row[8].ToString(),
                    s_consignatario = row.IsNull(9) ? "" : row[9].ToString(),
                    s_notificador = row.IsNull(10) ? "" : row[10].ToString(),
                    n_sello = row.IsNull(11) ? "" : row[11].ToString(),
                    c_pais_destino = row.IsNull(12) ? "" : row[12].ToString(),
                    c_detalle_puerto = row.IsNull(13) ? "" : row[13].ToString(),
                    s_comodity = row.IsNull(14) ? "" : row[14].ToString(),
                    s_prodmanifestado = row.IsNull(15) ? "" : row[15].ToString(),
                    v_tara = valorD2,
                    b_emb_dir = row.IsNull(17) ? "" : row[17].ToString(),
                    b_reef = row.IsNull(18) ? "" : row[18].ToString(),
                    c_tipo_doc = row.IsNull(19) ? "" : row[19].ToString(),
                    c_arivu = row.IsNull(20) ? "" : row[20].ToString(),
                    c_fauca = row.IsNull(21) ? "" : row[21].ToString(),
                    c_dm = row.IsNull(22) ? "" : row[22].ToString(),
                    c_dut = row.IsNull(23) ? "" : row[23].ToString(),
                    c_dmti = row.IsNull(24) ? "" : row[24].ToString(),
                    c_manifiesto = row.IsNull(25) ? "" : row[25].ToString(),
                    c_pais_origen = row.IsNull(26) ? "" : row[26].ToString(),
                    b_transferencia = row.IsNull(27) ? "" : row[27].ToString(),
                    b_manejo = row.IsNull(28) ? "" : row[28].ToString(),
                    b_recepcion = row.IsNull(29) ? "" : row[29].ToString(),     
                    num_fila = n_fila
                });

                n_fila = n_fila + 1;
            }  

            return pLista;
        }


        public static int GetNumberOfColumns(string filename)
        {
            var numberOfColumns = 0;
            using (var spreadsheetDocument = SpreadsheetDocument.Open(filename, false))
            {
                // Retrieve a reference to the workbook part.
                WorkbookPart wbPart = spreadsheetDocument.WorkbookPart;

                // Find the sheet with the supplied name, and then use that 
                // Sheet object to retrieve a reference to the first worksheet.
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                  Where(s => s.SheetId == "1").FirstOrDefault();

                var worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(theSheet.Id.Value);


                //Throw an exception if there is no sheet.
                if (theSheet == null)
                {
                    throw new ArgumentException("sheetName");
                }

                DocumentFormat.OpenXml.Spreadsheet.Worksheet ws = worksheetPart.Worksheet;


                Row row = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().FirstOrDefault();
                
               
                if (row != null)
                {
                    var spans = row.Spans != null ? row.Spans.InnerText : "";
                    if (spans != String.Empty)
                    {
                        char[] delimiter = new char[1];
                        delimiter[0] = ':';
                        string[] columns = spans.Split(delimiter);
                        numberOfColumns = int.Parse(columns[1]);
                    }
                }
            }

            return numberOfColumns;
        }
     

        public static string GetCellValue(string fileName, string sheetName, string addressName)
        {
            string value = null;

            // Open the spreadsheet document for read-only access.
            using (SpreadsheetDocument document =
                SpreadsheetDocument.Open(fileName, false))
            {
                // Retrieve a reference to the workbook part.
                WorkbookPart wbPart = document.WorkbookPart;

                // Find the sheet with the supplied name, and then use that 
                // Sheet object to retrieve a reference to the first worksheet.
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().
                  Where(s => s.SheetId == sheetName).FirstOrDefault();

                 //Throw an exception if there is no sheet.
                if (theSheet == null)
                {
                    throw new ArgumentException("sheetName");
                }

           


                // Retrieve a reference to the worksheet part.
                WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(theSheet.Id));

                // Use its Worksheet property to get a reference to the cell 
                // whose address matches the address you supplied.
                Cell theCell = wsPart.Worksheet.Descendants<Cell>().
                  Where(c => c.CellReference == addressName).FirstOrDefault();

                // If the cell does not exist, return an empty string.
                if (theCell != null)
                {
                    value = theCell.InnerText;

                    // If the cell represents an integer number, you are done. 
                    // For dates, this code returns the serialized value that 
                    // represents the date. The code handles strings and 
                    // Booleans individually. For shared strings, the code 
                    // looks up the corresponding value in the shared string 
                    // table. For Booleans, the code converts the value into 
                    // the words TRUE or FALSE.
                    if (theCell.DataType != null)
                    {
                        switch (theCell.DataType.Value)
                        {
                            case CellValues.SharedString:

                                // For shared strings, look up the value in the
                                // shared strings table.
                                var stringTable =
                                    wbPart.GetPartsOfType<SharedStringTablePart>()
                                    .FirstOrDefault();

                                // If the shared string table is missing, something 
                                // is wrong. Return the index that is in
                                // the cell. Otherwise, look up the correct text in 
                                // the table.
                                if (stringTable != null)
                                {
                                    value =
                                        stringTable.SharedStringTable
                                        .ElementAt(int.Parse(value)).InnerText;
                                }
                                break;

                            case CellValues.Boolean:
                                switch (value)
                                {
                                    case "0":
                                        value = "FALSE";
                                        break;
                                    default:
                                        value = "TRUE";
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            return value;
        }

        public static System.Data.DataTable ExtractExcelSheetValuesToDataTable(string xlsxFilePath, string sheetName)    {

        System.Data.DataTable dt = new System.Data.DataTable();

        using (SpreadsheetDocument myWorkbook = SpreadsheetDocument.Open(xlsxFilePath, true))        {

            //Access the main Workbook part, which contains data

            WorkbookPart workbookPart = myWorkbook.WorkbookPart;

            WorksheetPart worksheetPart = null;

            if (!string.IsNullOrEmpty(sheetName))            {

                Sheet ss = workbookPart.Workbook.Descendants<Sheet>().Where(s => s.SheetId == sheetName).SingleOrDefault<Sheet>();

                worksheetPart = (WorksheetPart)workbookPart.GetPartById(ss.Id);

            }            else            {

                worksheetPart = workbookPart.WorksheetParts.FirstOrDefault();

            } 

            SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;

            if (worksheetPart != null)            {

                Row lastRow = worksheetPart.Worksheet.Descendants<Row>().LastOrDefault();

                Row firstRow = worksheetPart.Worksheet.Descendants<Row>().FirstOrDefault();

                if (firstRow != null)                {

                    foreach (Cell c in firstRow.ChildElements)                    {

                        string value = GetValue(c, stringTablePart);

                        dt.Columns.Add(value);

                    }

                }

                if (lastRow != null)                {

                    for (int i = 2; i <= lastRow.RowIndex; i++)                    {

                        DataRow dr = dt.NewRow();

                        bool empty = true;

                      Row row = worksheetPart.Worksheet.Descendants<Row>() .Where(r => i == r.RowIndex).FirstOrDefault();

                        int j = 0;

                         if (row != null)                        {

                            foreach (Cell c in row.ChildElements)                            {

                                //Get cell value

                                string value = GetValue(c, stringTablePart);

                                 if (!string.IsNullOrEmpty(value) && value != " ")

                                 empty = false;

                                 dr[j] = value;

                                j++;

                                if (j == dt.Columns.Count)

                                 break;

                            }

                            if (empty)

                                break;

                            dt.Rows.Add(dr);

                        }

                     }

                }

            }

        }

        return dt;

    }

         public static string GetValue(Cell cell, SharedStringTablePart stringTablePart)    
         {

            if (cell.ChildElements.Count == 0)            return null;

             //get cell value

            string value = cell.ElementAt(0).InnerText;//CellValue.InnerText;

             //Look up real value from shared string table

            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))

                value = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;

             return value;

        }

        public static string GetImo(string sRuta, DBComun.Estado pTipo)
        {
            
                DBComun.sRuta = sRuta;
                string imo = null;
                string nombreHoja = GetNombre(sRuta);
                List<ArchivoExcel> listaArch = new List<ArchivoExcel>();
                object Valor3 = null;
                try
                {
                    using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
                    {
                        _conn.Open();

                        int fila = GetRowExcel(sRuta, DBComun.Estado.verdadero, nombreHoja) + 1;

                        string _consulta = @"SELECT * FROM [{0}$A1:W{1}]";

                        OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja, fila), _conn as OleDbConnection);
                        _command.CommandType = CommandType.Text;

                        OleDbDataReader _reader = _command.ExecuteReader();

                        //ArchivoExcel _tmpArc = new ArchivoExcel
                        //{
                        //    c_imoc = "9215892" //Valor3.ToString().TrimEnd().TrimStart()
                        //};
                        //listaArch.Add(_tmpArc);

                        while (_reader.Read())
                        {
                            if (!_reader.IsDBNull(2))
                            {
                                if (ArchivoBookingDAL.isNumeric(_reader[1].ToString().TrimEnd().TrimStart()))
                                    Valor3 = Convert.ToString(_reader[1]);
                                else
                                    Valor3 = _reader[1];

                            }
                            ArchivoExcel _tmpArc = new ArchivoExcel
                            {
                                c_imoc = Valor3.ToString().TrimEnd().TrimStart()
                            };
                            listaArch.Add(_tmpArc);
                            if (listaArch.Count > 0)
                            {
                                break;
                            }
                        }

                       
                            foreach (ArchivoExcel item in listaArch)
                            {
                                imo = item.c_imoc.ToString();
                                
                            }
                         

                        _reader.Close();
                        _conn.Close();
                        return imo;
                    }
                }
                catch (Exception ex)
                {
                    String errorMessage;
                    errorMessage = "Error IMO: ";
                    errorMessage = String.Concat(errorMessage, ex.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, ex.Source);
                    return null;
                }
                
        }

        public static string GetImoRe(string sRuta, DBComun.Estado pTipo)
        {

            DBComun.sRuta = sRuta;
            string imo = null;
            string nombreHoja = GetNombre(sRuta);
            List<ArchivoExcel> listaArch = new List<ArchivoExcel>();
            object Valor3 = null;
            try
            {
                using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
                {
                    _conn.Open();

                    int fila = GetRowExcel(sRuta, DBComun.Estado.verdadero, nombreHoja) + 1;

                    string _consulta = @"SELECT * FROM [{0}$A1:L{1}]";

                    OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja, fila), _conn as OleDbConnection);
                    _command.CommandType = CommandType.Text;

                    OleDbDataReader _reader = _command.ExecuteReader();

                    //ArchivoExcel _tmpArc = new ArchivoExcel
                    //{
                    //    c_imoc = "9215892" //Valor3.ToString().TrimEnd().TrimStart()
                    //};
                    //listaArch.Add(_tmpArc);

                    while (_reader.Read())
                    {
                        if (!_reader.IsDBNull(0))
                        {
                            if (ArchivoBookingDAL.isNumeric(_reader[0].ToString().TrimEnd().TrimStart()))
                                Valor3 = Convert.ToString(_reader[0]);
                            else
                                Valor3 = _reader[0];

                        }
                        ArchivoExcel _tmpArc = new ArchivoExcel
                        {
                            c_imoc = Valor3.ToString().TrimEnd().TrimStart()
                        };
                        listaArch.Add(_tmpArc);
                        if (listaArch.Count > 0)
                        {
                            break;
                        }
                    }


                    foreach (ArchivoExcel item in listaArch)
                    {
                        imo = item.c_imoc.ToString();

                    }


                    _reader.Close();
                    _conn.Close();
                    return imo;
                }
            }
            catch (Exception ex)
            {
                String errorMessage;
                errorMessage = "Error IMO: ";
                errorMessage = String.Concat(errorMessage, ex.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, ex.Source);
                return null;
            }

        }

        public static List<ArchivoExcel> GetManifiesto(string sRuta, DBComun.Estado pTipo)
        {
            List<ArchivoExcel> listaArch = new List<ArchivoExcel>();
            string nombreHoja = GetNombre(sRuta);
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
            {
                _conn.Open();

                int fila = GetRowExcel(sRuta, DBComun.Estado.verdadero, nombreHoja) + 1;

                string _consulta = @"SELECT * FROM [{0}$A1:W{1}]";

                OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja, fila), _conn as OleDbConnection);
                _command.CommandType = CommandType.Text;

                OleDbDataReader _reader = _command.ExecuteReader();

                //ArchivoExcel _tmpArc = new ArchivoExcel
                //{
                //    num_manif = 641 //Valor3.ToString().TrimEnd().TrimStart()
                //};
                //listaArch.Add(_tmpArc);

                while (_reader.Read())
                {
                    ArchivoExcel _tmpArc = new ArchivoExcel
                    {
                        //num_manif = (int)_reader.GetDouble(0)
                        c_num_manif = _reader.IsDBNull(0) ? "" : _reader.GetString(0)
                        //anum_manif = _reader.IsDBNull(0) ? 0 : Convert.ToInt32(_reader.GetString(0))
                    };
                    listaArch.Add(_tmpArc);

                    if (listaArch.Count > 0)
                        break;
                }

                _reader.Close();
                _conn.Close();
                return listaArch;
            }
        }

        public static List<ArchivoAduanaValid> GetValidAduana(string sRuta, DBComun.Estado pTipo)
        {
            List<ArchivoAduanaValid> listaArch = new List<ArchivoAduanaValid>();

            string nombreHoja = GetNombre(sRuta);
            object Valor = null;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
            {
                _conn.Open();

                //int fila = GetRowExcel1(sRuta, DBComun.Estado.verdadero, nombreHoja);

               // string _consulta = @"SELECT * FROM [{0}$A3:H{1}]";
                object Valor3 = null;
                object Valor4 = null;
                object Valor5 = null;

                int mani = 0;
                string _consulta = @"SELECT *  
                                    FROM [{0}$]";

                OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja), _conn as OleDbConnection);
                _command.CommandType = CommandType.Text;

                OleDbDataReader _reader = _command.ExecuteReader();
                int i = 0;
                while (_reader.Read())
                {
                    i = i + 1;

                    if (i >= 3)
                    {
                        if (!_reader.IsDBNull(7))
                        {
                            if (ArchivoBookingDAL.isNumeric(_reader[7]))
                                Valor3 = Convert.ToString(_reader[7]);
                            else
                                Valor3 = _reader[7];

                        }


                        if (!_reader.IsDBNull(6))
                        {
                            if (ArchivoBookingDAL.isNumeric(_reader[6]))
                                Valor4 = Convert.ToString(_reader[6]);
                            else
                                Valor4 = _reader[6];

                        }

                        if (!_reader.IsDBNull(5))
                        {
                            if (ArchivoBookingDAL.isNumeric(_reader[5]))
                                Valor5 = Convert.ToString(_reader[5]);
                            else
                                Valor5 = _reader[5];

                        }


                        ArchivoAduanaValid _tmpArc = new ArchivoAduanaValid
                        {

                            n_contenedor = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                            n_BL = _reader.IsDBNull(7) ? "" : Valor3.ToString(),
                            a_mani = _reader.IsDBNull(5) ? "" : Valor5.ToString()
                        };



                        if (Valor4.ToString().Length > 0)
                        {
                            if (mani > 0)
                            { }
                            else
                            {
                                mani = Convert.ToInt32(Valor4);
                            }

                        }

                        _tmpArc.n_manifiesto = mani;

                        listaArch.Add(_tmpArc);
                    }
                }

                _reader.Close();
                _conn.Close();
                return listaArch;
            }
        }

        public static List<ArchivoAduanaValid> GetValidAduana1(string sRuta, DBComun.Estado pTipo)
        {
            List<ArchivoAduanaValid> listaArch = new List<ArchivoAduanaValid>();

            string nombreHoja = GetNombre(sRuta);
            object Valor = null;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
            {
                _conn.Open();

                //int fila = GetRowExcel1(sRuta, DBComun.Estado.verdadero, nombreHoja);
                int fila = GetRowExcel(sRuta, DBComun.Estado.verdadero, nombreHoja) + 1;

                // string _consulta = @"SELECT * FROM [{0}$A3:H{1}]";
                
                string _consulta = @"SELECT *  
                                    FROM [{0}$A4:H20]";

                OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja), _conn as OleDbConnection);
                _command.CommandType = CommandType.Text;

                OleDbDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    ArchivoAduanaValid _tmpArc = new ArchivoAduanaValid
                    {

                        n_manifiesto = _reader.IsDBNull(5) ? 0 : Convert.ToInt32(_reader.GetString(5))
                    };
                    listaArch.Add(_tmpArc);

                    if (listaArch.Count > 0)
                        break;
                }


                _reader.Close();
                _conn.Close();
                return listaArch;
            }
        }

        public static int GetCountHojas(string rutaLibro)
        {
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");
            // EliminarProceso();
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook wb = null;
            Microsoft.Office.Interop.Excel._Worksheet ws = null;
            int hWnd = 0;
            try
            {

                hWnd = app.Application.Hwnd;
                // Abrimos el libro de trabajo
                //
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-SV");
                //Microsoft.Office.Interop.Excel._Workbook wrkBooks;
                // wrkBooks = app.Workbooks.Open(rutaLibro);
                wb = app.Workbooks.Open(rutaLibro);

                // Referenciamos la hoja cuyo índice sea 1.
                //
                //ws = ((Microsoft.Office.Interop.Excel.Worksheet)(wb.Worksheets.Item[1]));

                int countHojas = wb.Worksheets.Count;

                // Obtenemos el nombre de la hoja.
                //
                //string name = ws.Name;

                // Cerramos el libro
                //
                wb.Close(true);
                app.Quit();


                // Devolvemos el nombre de la hoja
                //               
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

                return countHojas;

            }
            catch (Exception ex)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, ex.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, ex.Source);
                throw new Exception(errorMessage);

            }
            finally
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wb);

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(app);
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;

                ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(hWnd);
                ws = null;
                wb = null;
                app = null;

            }

        }

        public static int GetRowExcel1(string sRuta, DBComun.Estado pTipo, string nombreHoja)
        {
            int cont = 0;
            int cont1 = 0;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
            {
                _conn.Open();

                string _consulta = @"SELECT *  
                                    FROM [{0}$]";

                OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja), _conn as OleDbConnection);
                _command.CommandType = CommandType.Text;

                OleDbDataReader _reader = _command.ExecuteReader();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        cont1 = cont1 + 1;
                        
                        if (!_reader.IsDBNull(0) && !_reader.IsDBNull(5))
                        {
                            if (_reader.GetString(0).TrimEnd().TrimStart().Trim().Length > 10  && _reader.GetString(5).TrimEnd().TrimStart().Trim().Length > 2)
                            {
                                cont = cont + 1;
                            }
                        }                    

                    }
                }
                _reader.Close();
                _conn.Close();
                return cont;
            }
        }




    }
}

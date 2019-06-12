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

using cxExcel = ClosedXML.Excel;

using msExcel = Microsoft.Office.Interop.Excel;

using Office = Microsoft.Office.Core;
using VBIDE = Microsoft.Vbe.Interop;
using Microsoft.Vbe.Interop;
using System.Threading;

namespace CEPA.CCO.Linq
{
    public class NotiExport
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
            string _f2save = string.Format("{0}{1}", fullPath, "ECO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
            string _f4save = string.Format("{0}{1}", fullPath, "ECO1_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
            string _f3save = string.Format("{0}{1}", fullPDF, "ECO_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".pdf");
            string _f5save = string.Format("{0}{1}", fullPath, "ECO2_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
            string _f6save = string.Format("{0}{1}", fullPath, "ECO3_" + c_cliente + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");


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
                    break;
                }


                string fechaC = f_llegada.Day + "/" + f_llegada.Month + "/" + f_llegada.Year;

                int Aumenta = 3;
                int Cuenta = 1;
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
                                //GenerarExcelCX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, j, iRow, d_naviera, false, n_manifiesto, c_viaje);
                                //GenerarExcel2CX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB1, fechaC, j, iRow, d_naviera);
                                Cuenta = Cuenta + 1;
                            }
                            else
                            {
                                Filas = ordenar.Count + ROWS_START;
                                //GenerarExcelCX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, j, iRow, d_naviera, false, n_manifiesto, c_viaje);
                                //GenerarExcel2CX(ordenar, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB1, fechaC, j, iRow, d_naviera);
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



                //ConvertExcelToPdf(_f4save, _f3save);


                if (pRutas == null)
                    pRutas = new List<string>();

                pRutas.Add(_f2save);
                pRutas.Add(_f3save);

                //if (_list.Count > 0)
                //    ruta = GenerarPeligrosidadCX(_list, c_cliente, d_naviera, c_llegada, IdReg, d_buque, f_llegada, pValor, pCantidad, n_manifiesto, c_viaje);

                if (ruta != null && ruta != "")
                    pRutas.Add(ruta);

                List<string> pPDF = new List<string>();

               // EnviarCorreo(pRutas, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, n_manifiesto);

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


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Data;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Web;


namespace CEPA.CCO.Linq
{
    public class NotiNavieras
    {
        string fullPath = Application.StartupPath + "\\Enviados";       

        public int ProcesarCorreo(DBComun.Estado pEstado)
        {
            //List<Naviera> _listaNaviera = EncaBuqueDAL.ObtenerNavieras(pEstado);
            List<EncaNaviera> _listaNoEnviados = EncaNavieraDAL.ObtenerNoEnviados(pEstado);
            int Id = 0;
            if (_listaNoEnviados.Count > 0)
            {
                foreach (EncaNaviera item in _listaNoEnviados)
                {
                    var queryList = (from a in ResulNavieraDAL.ObtenerArchivo(pEstado, item.c_naviera, item.num_manif)
                                     join b in EncaBuqueDAL.ObtenerBuquesJoin(pEstado, item.c_naviera, item.c_llegada) on new { c_naviera = a.c_naviera, c_llegada = a.c_llegada } equals new { c_naviera = b.c_cliente, c_llegada = b.c_llegada }
                                     where a.c_llegada == item.c_llegada
                                     select new ResulNaviera
                                     {
                                         IdDeta = a.IdDeta,
                                         n_contenedor = a.n_contenedor,
                                         c_tamaño = a.c_tamaño,
                                         d_buque = b.d_buque,
                                         f_llegada = a.f_llegada,
                                         num_manif = a.num_manif
                                     }).OrderBy(v => v.n_contenedor).ToList();

                    if (queryList.Count > 0)
                    {
                        GenerarExcel(queryList, item.c_naviera, item.c_llegada, item.IdReg, pEstado);
                        Id = item.IdReg;
                    }
                    return Id;
                }
                return Id;             
            }
            return 0;
        }

        public void ProcesarCorreoId(DBComun.Estado pEstado, string c_llegada, int IdReg, int pMin, int pMax, string c_naviera)
        {

            var queryList = (from a in ResulNavieraDAL.ObtenerArchivoPost(pEstado, IdReg, pMin, pMax)
                             join b in EncaBuqueDAL.ObtenerBuquesJoin(pEstado, c_naviera, c_llegada) on new { c_naviera = a.c_naviera, c_llegada = a.c_llegada } equals new { c_naviera = b.c_cliente, c_llegada = b.c_llegada }
                             where a.c_llegada == c_llegada
                             select new ResulNaviera
                             {
                                 IdDeta = a.IdDeta,
                                 n_contenedor = a.n_contenedor,
                                 c_tamaño = a.c_tamaño,
                                 d_buque = b.d_buque,
                                 f_llegada = a.f_llegada,
                                 c_naviera = a.c_naviera,
                                 c_llegada = a.c_llegada                                 
                             }).OrderBy(v => v.n_contenedor).ToList();

            
            if (queryList.Count > 0)
            {                
               GenerarExcel(queryList, c_naviera, c_llegada, IdReg, pEstado);                              
            }
        }

        public void GenerarExcel(List<ResulNaviera> pLista, string c_cliente, string c_llegada, int IdReg, DBComun.Estado pEstado)
        {
            
            const int ROWS_FIRST = 1;
            int Filas = pLista.Count + ROWS_FIRST;
            int iRow = 1;
            string d_buque = null;
            string _f2save = null;
            int Hwnd = 0;
            int manifiesto = 0;

            Microsoft.Office.Interop.Excel.Application oXL = null;
            Microsoft.Office.Interop.Excel._Workbook oWB = null;
            Microsoft.Office.Interop.Excel._Worksheet oSheet = null;
            Microsoft.Office.Interop.Excel.Range oRng = null;
            try
            {
                if (pEstado == DBComun.Estado.falso)
                    _f2save = string.Format("{0}\\{1}", fullPath, "CCO_" + c_cliente + "_" + DateTime.Now.ToString("ddMMyyyy", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
                else
                {
                    string fullWeb = System.Configuration.ConfigurationManager.AppSettings["Enviados"];
                    _f2save = string.Format("{0}{1}", fullWeb, "CCO_" + c_cliente + "_" + DateTime.Now.ToString("ddMMyyyy", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xls");
                }


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
                oSheet.Cells[ROWS_FIRST, 1] = "# MANIFIESTO";
                oSheet.Cells[ROWS_FIRST, 2] = "CONTENEDOR";
                oSheet.Cells[ROWS_FIRST, 3] = "TAMAÑO";
                oSheet.Cells[ROWS_FIRST, 4] = "BUQUE";
                oSheet.Cells[ROWS_FIRST, 5] = "FECHA LLEGADA";
                //Format A1:D1 as bold, vertical alignment = center.


                oSheet.get_Range("A1", "E1").Font.Bold = true;
                oSheet.get_Range("A1", "E1").Interior.ColorIndex = 15;
                oSheet.get_Range("A1", "E1").VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
                oSheet.get_Range("A1", "E1").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oSheet.get_Range("A1", string.Concat("C", Filas)).NumberFormat = "@";
                oSheet.get_Range("E1", string.Concat("E", Filas)).NumberFormat = "dd/mm/yyyy;@";

                //oRng = oSheet.get_Range("A1", string.Concat("D", Filas));
                //oRng.EntireColumn.AutoFit();

                oRng = oSheet.get_Range("A1", string.Concat("E", Filas));
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


                oSheet.Columns[1].ColumnWidth = 15;
                oSheet.Columns[2].ColumnWidth = 15;
                oSheet.Columns[3].ColumnWidth = 10;
                oSheet.Columns[4].ColumnWidth = 45;
                oSheet.Columns[5].ColumnWidth = 15;

                foreach (var item in pLista)
                {
                    int iCurrent = ROWS_FIRST + iRow;
                    oSheet.Cells[iCurrent, 1] = item.num_manif;
                    oSheet.Cells[iCurrent, 2] = item.n_contenedor;
                    oSheet.Cells[iCurrent, 3] = item.c_tamaño;
                    oSheet.Cells[iCurrent, 4] = item.d_buque;
                    oSheet.Cells[iCurrent, 5] = item.f_llegada;
                    iRow = iRow + 1;
                    d_buque = item.d_buque;
                    manifiesto = item.num_manif;
                }

                oWB.SaveAs(_f2save, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                oWB.Close(true);
                oXL.Quit();

                EnviarCorreo(_f2save, d_buque, manifiesto, pEstado);
                int _resultado = Convert.ToInt32(EncaNavieraDAL.ActualizarNoti(pEstado, IdReg, 1));                

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
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWB);

                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oXL);

                    ArchivoExcelDAL.TryKillProcessByMainWindowHwnd(Hwnd);
                }
            }
        }

        public void EnviarCorreo(string pArchivo, string d_buque, int manifiesto, DBComun.Estado pEstado)
        {
            string Html;
            EnvioCorreo _correo = new EnvioCorreo();

            Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
            Html += "MÓDULO : LISTADO DE CONTENEDORES PENDIENTES DE AUTORIZACIÓN  <br />";
            Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES <br /><br />";
            Html += string.Format("El listado de contenedores para el barco {0} necesita su autorización.-", d_buque);

            _correo.Subject = string.Format("Listado de Contenedores a Autorizar {0} # Manifiesto {1}", d_buque, manifiesto);
            _correo.ListArch.Add(pArchivo);           
            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_aduana", pEstado, "219");
            _correo.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_aduana", pEstado, "219");
            _correo.Asunto = Html;
            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, pEstado);
            _correo = null;
        }

        public List<int> ProcesarRecepcion(DBComun.Estado pEstado)
        {
            //List<Naviera> _listaNaviera = EncaBuqueDAL.ObtenerNavieras(pEstado);
            List<DocBuque> _listaRecepcion = DocBuqueLINQ.ObtenerEncaOpera(pEstado);
            List<int> Id = new List<int>();
            string Html;
            DateTime _fecha;
            string c_operadora = null;

            if (_listaRecepcion.Count > 0)
            {
                foreach (DocBuque item in _listaRecepcion)
                {
                    var query = (from a in DetaNavieraDAL.ObtenerRecepcion(DBComun.Estado.falso, item.IdReg)
                                 select new DetaNaviera
                                 {
                                     IdDeta = a.IdDeta,
                                     c_correlativo = a.c_correlativo,
                                     n_contenedor = a.n_contenedor,
                                     f_recepcion = a.f_recepcion,
                                     grua = a.grua,
                                     grupo = a.grupo,
                                     s_nombre = a.s_nombre,
                                     s_operadora = a.s_operadora
                                 }).OrderBy(a => a.f_recepcion).ToList();


                    if (query.Count > 0)
                    {
                        foreach (var itemC in query)
                        {
                            c_operadora = itemC.s_operadora;
                            break;
                        }


                        _fecha = DetaNavieraLINQ.FechaBD(DBComun.Estado.falso);

                        Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                        Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em; width: 50%;\">";
                        Html += "<tr>";                        
                        Html += "<td width=\"40%\" height=\"25\" style=\"text-align: right;\"><font size=2>Operadora :</font></td>";
                        Html += "<td style=\"text-align: left;\"><font size = 2>" + c_operadora + "</font></td>";
                        Html += "</tr>";
                        Html += "<tr>";
                        Html += "<right>";
                        Html += "<td width=\"40%\" height=\"25\" style=\"text-align: right;\"><font size=2>Buque :</font></td></right>";
                        Html += "<td style=\"text-align: left;\"><font size = 2>" + item.d_buque + "</font></td>";
                        Html += "</tr>";
                        Html += "<tr>";
                        Html += "<right>";
                        Html += "<td width=\"40%\" height=\"25\" style=\"text-align: right;\"><font size=2>Fecha / Hora de Corte :</font></td></right>";
                        Html += "<td style=\"text-align: left;\"><font size = 2>" + _fecha.ToString() + "</font></td>";
                        Html += "</tr>";
                        Html += "</table>";
                        Html += "<br />";

                        Html += "<b><u>REPORTE DE CONTENEDORES RECIBIDOS EN PUERTO DE ACAJUTLA </b></u><br />";
                        Html += "<br />";

                        Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 50%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; align='center'; \">";
                        Html += "<tr>";
                        Html += "<center>";
                        Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>No.</font></th>";
                        Html += "<td width=\"20%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>CONTENEDOR</font></th>";
                        Html += "<td width=\"22%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>FECHA / HORA RECEPCION</font></th>";
                        Html += "<td width=\"10%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>GRUA</font></th>";
                        Html += "<td width=\"10%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>GRUPO</font></th>";
                        Html += "<td width=\"33%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>NOMBRE DE CHEQUE</font></th>";
                        Html += "</center>";
                        Html += "</tr>";

                        foreach (var itemD in query)
                        {
                            Html += "<tr>";
                            Html += "<center>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemD.c_correlativo + "</font></td>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemD.n_contenedor + "</font></td>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemD.f_recepcion.ToString("dd/MM/yyyy HH:mm:ss") + "</font></td>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemD.grua + "</font></td>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemD.grupo + "</font></td>";
                            Html += "</center>";
                            Html += "<td height=\"25\"><font size=1 color=blue>" + itemD.s_nombre + "</font></td>";
                            Html += "</tr>";
                            Id.Add(itemD.IdDeta);
                        }

                        Html += "</font>";
                        Html += "</table>";

                        EnvioCorreo _correo1 = new EnvioCorreo();
                        _correo1.Subject = string.Format("Recepción de Contenedores en Puerto de Acajutla: Buque {0}, número de Viaje {1} ", item.d_buque, item.c_voyage);
                        _correo1.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_recepcion", DBComun.Estado.falso, item.c_cliente);
                        List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_recepcion", DBComun.Estado.verdadero, item.c_cliente);

                        if (_listaCC == null)
                            _listaCC = new List<Notificaciones>();

                        _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_recepcion", DBComun.Estado.verdadero, "219"));
                        _correo1.ListaCC = _listaCC;

                        

                        _correo1.Asunto = Html;
                        _correo1.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);

                        foreach (int itemI in Id)
                        {
                            int _res = Convert.ToInt32(DetaNavieraDAL.ActualizarIdRece(DBComun.Estado.falso, itemI));
                        }

                        return Id;
                    }                    
                }
            }
            return Id;
        }
    }

    public class Linq
    {
    }
}

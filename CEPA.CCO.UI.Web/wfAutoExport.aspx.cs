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
using System.Globalization;

using Excel = Microsoft.Office.Interop.Excel;
using System.IO;


using cxExcel = ClosedXML.Excel;

using msExcel = Microsoft.Office.Interop.Excel;
using System.Web.Configuration;
using System.Web.Services;
using System.Net;
using System.Data;
using Newtonsoft.Json;

namespace CEPA.CCO.UI.Web
{
    public partial class wfAutoExport : System.Web.UI.Page
    {
        public string c_navi { get; set; }
        public string c_navi_corto { get; set; }
        List<EstadiaConte> lstUbi = new List<EstadiaConte>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["IdReg"] == null)
                    {
                        throw new Exception("Falta código de cabecera");
                    }
                    else
                    {
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerAduanaIdExAuto(Convert.ToInt32(Request.QueryString["IdReg"]));
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                c_naviera.Text = item.d_cliente;
                                viaje.Text = item.c_voyage;
                                c_navi = item.c_cliente;
                            }

                            Cargar();


                        }
                        else
                        {
                            //btnCargar.Enabled = false;
                        }

                    }



                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }
            }

            if (GridView1.Rows.Count > 0)
            {
                GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";


                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

                GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
                GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        private void Cargar()
        {
            
            List<ArchivoExport> pListExport = new List<ArchivoExport>();
            lstUbi = GetContenedor();
            pListExport = DetaNavieraDAL.ObtenerDetalleEx(Convert.ToInt32(Request.QueryString["IdReg"]));

            var query = (from a in pListExport
                         join b in lstUbi on a.n_contenedor equals b.Contenedor into ContUbica
                         from c in ContUbica.DefaultIfEmpty()
                         where a.IdDoc == Convert.ToInt32(Request.QueryString["IdDoc"])
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
                             c_pais_destino = a.c_pais_destino,                             
                             c_tipo_doc = a.c_tipo_doc,
                             c_puerto_trasbordo = a.c_puerto_trasbordo,
                             n_documento = a.n_documento,
                             v_tara = a.v_tara,
                             c_imo_imd = a.c_imo_imd,
                             s_trafico = a.s_trafico,
                             s_consignatario = a.s_consignatario,
                             s_almacenaje = a.s_almacenaje,
                             s_manejo = a.s_manejo,
                             s_posicion = c == null  ? "NO SE ENCUENTRA UBICADO" : c.Sitio,
                             s_pe = Convert.ToDouble(a.v_peso + a.v_tara) > 30000.00 ? ">30,000 Kg." : a.c_imo_imd.Length > 0 ? "X" : "",
                             n_sello = c== null ? "" : c.Marchamo
                         }).ToList();

            GridView1.DataSource = query;
            GridView1.DataBind();
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

        protected void onPaging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Cargar();
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            int pIdDoc = 0;
            try
            {
                List<string> pListaNo = new List<string>();
                List<int> pIdDeta = new List<int>();


                int valores = 0;
                //int cantidad = cant;
                int Correlativo = 0;
                int cont = 0;


                EnvioCorreo _correo = new EnvioCorreo();

                SeleccionManager.NoAutoExport((GridView)GridView1);
                List<DetaNaviera> _litNo = new List<DetaNaviera>();

                _litNo = HttpContext.Current.Session["NoExportados"] as List<DetaNaviera>;
                int no_resul = 0;
                string t_r = "";
              
                if (_litNo.Count() > 0)
                {
                    foreach (var itemNo in _litNo)
                    {
                        if (itemNo.TipoRevision == "")
                            throw new Exception("Por favor debe indicar porque es DENEGADO del listado el contenedor # " + itemNo.n_contenedor);
                        else {
                            if (itemNo.TipoRevision == "1")
                                t_r = "ADUANA";
                            else
                                t_r = "CEPA";
                            no_resul = Convert.ToInt32(DetaNavieraDAL.ActualizarDetaExpAutoNo(DBComun.Estado.verdadero, itemNo.IdDeta, t_r, User.Identity.Name));
                        }
                    }
                }


                SeleccionManager.AutoExport((GridView)GridView1);
                List<DetaNaviera> _lit = new List<DetaNaviera>();

                _lit = HttpContext.Current.Session["Exportados"] as List<DetaNaviera>;

                if (_lit.Count > 0)
                {
                   
                    List<ArchivoExport> _listaAOrdenar1 = new List<ArchivoExport>();
                    foreach (var item in _lit)
                    {

                        pIdDoc = item.IdDoc;

                        int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarDetaExpAuto(DBComun.Estado.verdadero, item.IdDeta, User.Identity.Name));

                        if (_resultado > 0)
                        {
                            valores = _resultado;
                            cont = cont + 1;
                        }
                    }

                    if (valores > 0)
                    {
                        foreach (var item in _lit)
                        {


                            List<ArchivoExport> _listAutorizados = DetaNavieraDAL.ObtenerAutorizadosExSta(item.IdReg, item.IdDoc, DBComun.Estado.verdadero).OrderBy(a => a.c_puerto_trasbordo).ToList();

                            Correlativo = Convert.ToInt32(DetaNavieraDAL.ObtenerCorrelativoExSta(item.IdReg, item.IdDoc, DBComun.Estado.verdadero)) + 1;

                            //var _destinos = _listAutorizados.GroupBy(i => i.c_detalle_puerto).Select(group => group.First());

                            var _destinos = (from a in _listAutorizados
                                             group a by a.c_puerto_trasbordo into g
                                             select new
                                             {
                                                 c_puerto_trasbordo = g.Key
                                             }).OrderBy(a => a.c_puerto_trasbordo).ToList();


                            List<ArchivoExport> _list = new List<ArchivoExport>();

                            if (_destinos.Count() > 0)
                            {
                                foreach (var iteDe in _destinos)
                                {
                                    

                                    for (int d = 1; d <= 2; d++)
                                    {
                                        if (d == 1)
                                            _listaAOrdenar1 = _listAutorizados.Where(f => f.s_trafico == "PATIO CEPA").ToList();
                                        else if (d == 2)
                                            _listaAOrdenar1 = _listAutorizados.Where(f => f.s_trafico == "TRANSBORDO").ToList();

                                        if (_listaAOrdenar1.Count > 0)
                                        {
                                            for (int i = 1; i <= 2; i++)
                                            {

                                                var consulta = DetaNavieraLINQ.AlmacenarArchivoEx(_listaAOrdenar1, DBComun.Estado.verdadero, i);
                                                if (consulta.Count() > 0)
                                                {
                                                    var _trafico = consulta.Where(c => c.c_puerto_trasbordo.Equals(iteDe.c_puerto_trasbordo)).ToList();
                                                    if (_trafico.Count > 0)
                                                        _list.AddRange(consulta);

                                                    foreach (var item_C in _trafico)
                                                    {
                                                        _listaAOrdenar1.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }

                            if (_list.Count > 0)
                            {
                                foreach (ArchivoExport deta in _list)
                                {
                                    int _actCorre = Convert.ToInt32(DetaNavieraDAL.ActualizarCorrelativoEx2(DBComun.Estado.verdadero, Correlativo, deta.IdDeta));
                                    Correlativo = Correlativo + 1;
                                }
                            }

                            int _rango = ((Correlativo - 1) - cont) + 1;

                            
                            List<ArchivoExport> _listaAOrder = DetaNavieraDAL.ObtenerResultadoExAll(item.IdReg, _rango, Correlativo - 1, DBComun.Estado.verdadero);
                            var _listaAOrdenar = new List<ArchivoExport>();
                            lstUbi = GetContenedor();

                            if (_listaAOrder.Count > 0 && lstUbi != null)
                            {
                                _listaAOrdenar = (from a in _listaAOrder
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
                                                          s_pe = Convert.ToDouble(a.v_peso + a.v_tara) > 30000.00 ? "X" : "",
                                                          c_tamaño_c = a.c_tamaño_c,
                                                          n_sello = c == null ? "" : c.Marchamo,
                                                          b_estado = a.b_estado
                                                      }).OrderBy(c => c.c_correlativo).ToList();
                            }
                            else
                            {
                                _listaAOrdenar = new List<ArchivoExport>();
                                throw new Exception("Reportar una falla en carga de información");

                            }


                            List<DocBuque> pLstPendientes = DocBuqueLINQ.ObtenerAduanaExSta(DBComun.Estado.verdadero, item.IdDoc);
                            NotiAutorizadosSer _notiAutorizados = new NotiAutorizadosSer();
                            foreach (var iPendiente in pLstPendientes)
                            {
                                List<ArchivoExport> pNoAuto = new List<ArchivoExport>();                                
                                pNoAuto = DetaNavieraDAL.obtNoAutoEx(item.IdDoc, DBComun.Estado.verdadero);
                                GenerarAplicacionEX(_listaAOrdenar, iPendiente.c_cliente, iPendiente.d_cliente, iPendiente.c_llegada, (int)iPendiente.IdReg,
                                 iPendiente.d_buque, iPendiente.f_llegada, cont, GridView1.Rows.Count, null, iPendiente.c_voyage, DBComun.Estado.verdadero, iPendiente.IdDoc, pNoAuto);
                                int resuly = Convert.ToInt32(DetaDocDAL.ActualizarValidacionEx(1, (int)iPendiente.IdReg, DBComun.Estado.verdadero, iPendiente.IdDoc));
                                break;
                            }
                            break;
                        }

                    }

                    int _resultadoGen = Convert.ToInt32(DetaNavieraDAL.actGenListExp(DBComun.Estado.verdadero, pIdDoc));
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Cantidad de contenedores autorizados " + cont + " de "+ GridView1.Rows.Count + "');", true);
                    
                }
                else
                {
                    throw new Exception("No se poseen contenedores seleccionados a validar.");
                }

            }
            catch (Exception ex)
            {
                int resuly = Convert.ToInt32(DetaDocDAL.RevertirAutoEx(DBComun.Estado.verdadero, pIdDoc));
                string mensaje = ex.Message.Replace("\r\n", "");
                ScriptManager.RegisterStartupScript(this, typeof(string), "alertMess", "alertMess('"+ mensaje + "');", true);                
            }
            finally
            {
                Cargar();
                HttpContext.Current.Session["Exportados"] = null;
                HttpContext.Current.Session["NoExportados"] = null;
                lstUbi = new List<EstadiaConte>();
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("~/wfPrinciExport.aspx");
            
        }

        string fullEXP_AUTO = WebConfigurationManager.AppSettings["fullExpAut"].ToString();



        int Hoja = 3;
        List<string> pRutas = new List<string>();
        public void GenerarAplicacionEX(List<ArchivoExport> pLista, string c_cliente, string d_naviera, string c_llegada, int IdReg, string d_buque,
              DateTime f_llegada, int pValor, int pCantidad, List<DetaNaviera> pCancelados, string c_voyage, DBComun.Estado pEstado, int IdDoc, List<ArchivoExport> pListaNo)
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
            int contador = 1;

            try
            {


                List<EnvioAuto> listAuto = new List<EnvioAuto>();
                if (IdDoc > 0)
                    listAuto = DetaNavieraDAL.ObtenerDocAuExp(IdDoc, pEstado);

                string c_navi_corto = null, c_viaje = null;

                foreach (var item in listAuto)
                {
                    c_navi_corto = item.c_naviera_corto;
                    c_viaje = item.c_voyaje;
                    break;
                }

                //Path.Combine(Server.MapPath("~/files"), fileName);
                //var rutaC = string.Format("{0}{1}", fullEXP_AUTO, "EXP_AUTO_" + c_navi_corto + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
                _Exf1save = string.Format("{0}{1}", fullEXP_AUTO, "EXP_AUTO_" + c_navi_corto + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".xlsx");
                _Exf2save = string.Format("{0}{1}", fullEXP_AUTO, "EXP_AUTO_" + c_navi_corto + "_" + DateTime.Now.ToString("MMyyhhmmss", CultureInfo.CreateSpecificCulture("es-SV")) + "_" + c_llegada + ".pdf");


                string fechaC = f_llegada.Day + "/" + f_llegada.Month + "/" + f_llegada.Year;

                int Cuenta = 1;

                var _destinos = (from a in pLista.OrderBy(a => a.c_correlativo)
                                 group a by a.c_puerto_trasbordo into g
                                 select new
                                 {
                                     c_puerto_trasbordo = g.Key
                                 }).ToList();

                List<ArchivoExport> _listOr = new List<ArchivoExport>();
                List<ArchivoExport> _listaAOrdenar1 = new List<ArchivoExport>();

                pLista = pLista.OrderBy(a => a.c_correlativo).ToList();

                if (_destinos.Count() > 0)
                {
                    foreach (var item in _destinos)
                    {

                        

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

                                    var consulta = DetaNavieraLINQ.AlmacenarArchivoEx(_listaAOrdenar1, DBComun.Estado.verdadero, i);
                                    if (consulta.Count() > 0)
                                    {
                                        var _trafico = consulta.Where(c => c.c_puerto_trasbordo.Equals(item.c_puerto_trasbordo)).ToList();

                                        Filas = _trafico.Count + ROWS_START;
                                        if (_trafico.Count() > 0)
                                        {
                                            GenerarExcel2CXExp(_trafico, c_llegada, ROWS_FIRST, ROWS_START, Filas, d_buque, Cuenta, oWB, fechaC, i, iRow, d_naviera, c_voyage, contador);
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
                }

               
                oWB.SaveAs(_Exf1save);

                string _ruta = "@" + _Exf1save;



                ConvertExcelToPdf(_Exf1save, _Exf2save);

                if (pRutas == null)
                    pRutas = new List<string>();

                pRutas.Add(_Exf2save);
                pRutas.Add(_Exf1save);




                EnviarCorreoEx(pRutas, d_buque, pValor, pCantidad, pCancelados, c_cliente, c_llegada, c_navi_corto, c_viaje, pListaNo);


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

        private static void GenerarExcel2CXExp(List<ArchivoExport> pAduana, string c_llegada, int ROWS_FIRST, int ROWS_START, int Filas, string d_buque, int cuenta, cxExcel.XLWorkbook oWB, string _Fecha, int pValor, int iRow, string d_naviera, string c_voyage, int contador)
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

                switch (pValor)
                {
                    case 1:
                        nombreSheet = "LLENOS";
                        valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN LLENOS " + ubicacion;
                        break;
                    case 2:
                        nombreSheet = "VACIOS";
                        valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN VACÍOS " + ubicacion;
                        //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                        break;
                    case 3:
                        nombreSheet = "TRANSBORDOS";
                        valorCabecera = "LISTADO DE CONTENEDORES DE EXPORTACIÓN TRANSBORDOS";
                        //oSheet.Cell(6, 1).Value = "LISTADO DE CONTENEDORES DE IMPORTACIÓN VACÍOS";              
                        break;

                    default:
                        break;
                }

                string ubi = ubicacion == "TRASBORDO" ? "TR" : ubicacion == "PATIO CEPA" ? "PC" : "ED";
                string valSheet = puerto_destino.Substring(0, 3) + "_" + nombreSheet.Substring(0, 3) + "_" + ubi + contador;
                var oSheet = oWB.Worksheets.Add(valSheet);
                oSheet.PageSetup.Header.Clear();

                oSheet.Range("A1", "I2").Merge();
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
                oSheet.Cell("A5").Value = "VAPOR - VIAJE";
                oSheet.Cell("A6").Value = "DESTINO";
                oSheet.Cell("A7").Value = "UBICACION";
                oSheet.Cell("A8").Value = "FECHA DE ARRIBO";
                oSheet.Range("A4", "I8").Style.Font.FontSize = 14;

                oSheet.Range("C4", "I4").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C5", "I5").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C6", "I6").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C7", "I7").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;
                oSheet.Range("C8", "I8").Merge().Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;

                oSheet.Cell("C4").Value = d_naviera;
                oSheet.Cell("C4").Style.Font.Bold = true;
                oSheet.Cell("C5").Value = d_buque + " - " + c_voyage;
                oSheet.Cell("C5").Style.Font.Bold = true;
                oSheet.Cell("C6").Value = puerto_destino;
                oSheet.Cell("C6").Style.Font.Bold = true;
                oSheet.Cell("C7").Value = ubicacion;
                oSheet.Cell("C7").Style.Font.Bold = true;
                oSheet.Cell("C8").Value = _Fecha;
                oSheet.Cell("C8").Style.Font.Bold = true;




                oSheet.Cell("A10").Value = "No.";
                oSheet.Cell("B10").Value = "CONTENEDOR";
                oSheet.Cell("C10").Value = "TAMAÑO";
                oSheet.Cell("D10").Value = "PESO";
                oSheet.Cell("E10").Value = "TARA";
                oSheet.Cell("F10").Value = "POSICIÓN";
                oSheet.Cell("G10").Value = "MARCHAMO";
                oSheet.Cell("H10").Value = "PE";
                oSheet.Cell("I10").Value = "OBSERVACIÓN";

                oSheet.Range("A10", "I10").Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                oSheet.Range("A10", "I10").Style.Font.Bold = true;
                oSheet.Range("A10", "I10").Style.Font.FontSize = 14;
                oSheet.Range("A10", "I10").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                oSheet.Range("A10", "I10").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                //oSheet.Range("A8", "H8").Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                //oSheet.Range("A8", "H8").Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                oSheet.Column(1).Width = 7;
                oSheet.Column(2).Width = 29;
                oSheet.Column(3).Width = 12;
                oSheet.Column(4).Width = 15;
                oSheet.Column(5).Width = 8;
                oSheet.Column(6).Width = 19;
                oSheet.Column(7).Width = 14;
                oSheet.Column(8).Width = 3;
                oSheet.Column(9).Width = 25;

                oSheet.Range("D11", string.Concat("D", Filas)).Style.NumberFormat.SetFormat("#,##0.00");
                oSheet.Range("E11", string.Concat("E", Filas)).Style.NumberFormat.SetFormat("#,##0");
                oSheet.Range("A11", string.Concat("I", Filas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                oSheet.Range("A11", string.Concat("I", Filas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;

                oSheet.Range("A10", string.Concat("I", Filas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                oSheet.Range("A10", string.Concat("I", Filas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                oSheet.Range("F11", string.Concat("F", Filas)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                oSheet.Range("F11", string.Concat("F", Filas)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Top;

                int iCurrent = 0;
                foreach (var item in pAduana)
                {
                    iCurrent = ROWS_START + iRow;
                    oSheet.Row(iCurrent).Height = 60;
                    oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                    oSheet.Cell(iCurrent, 2).Value = item.n_contenedor;
                    oSheet.Cell(iCurrent, 3).Value = item.c_tamaño_c;
                    oSheet.Cell(iCurrent, 4).Value = item.v_peso;
                    oSheet.Cell(iCurrent, 5).Value = item.v_tara;
                    oSheet.Cell(iCurrent, 6).Value = item.s_posicion;
                    oSheet.Cell(iCurrent, 7).Value = item.n_sello;
                    oSheet.Cell(iCurrent, 8).Value = item.s_pe;
                    if (item.c_imo_imd!= "")
                        oSheet.Cell(iCurrent, 9).Value = "ADVERTENCIA DE PELIGROSIDAD CLASE: " + item.c_imo_imd;
                    else if (item.b_shipper == "SI")
                        oSheet.Cell(iCurrent, 9).Value = "SHIPPER OWNED";
                    else
                        oSheet.Cell(iCurrent, 9).Value = "";

                    oSheet.Cell(iCurrent, 6).Style.Alignment.SetWrapText(true);
                    oSheet.Cell(iCurrent, 7).Style.Alignment.SetWrapText(true);
                    oSheet.Cell(iCurrent, 8).Style.Alignment.SetWrapText(true);
                    oSheet.Cell(iCurrent, 9).Style.Alignment.SetWrapText(true);
                    iRow = iRow + 1;
                }


                oSheet.Cell(iCurrent + 2, 1).Value = "Referencias: PE Pedido especial";
                oSheet.Cell(iCurrent + 2, 1).Style.Font.Bold = true;
                oSheet.Range("A11", string.Concat("I", Filas)).Style.Font.FontSize = 14;
                oSheet.Range("B11", string.Concat("B", Filas)).Style.Font.FontSize = 22;

                oSheet.Range("I11", string.Concat("I", Filas)).Style.Alignment.SetWrapText(true);



                //oSheet.Range("H9", string.Format("H{0}", Filas)).Style.Alignment.SetWrapText(true);
                oSheet.Range("A1", string.Concat("I", Filas + 2)).Style.Font.FontName = "Trebuchet MS";

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

                oSheet.PageSetup.Footer.Left.AddText(string.Format("Buque : {0}", d_buque));
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.PageNumber, cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(" / ", cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Center.AddText(cxExcel.XLHFPredefinedText.NumberOfPages, cxExcel.XLHFOccurrence.AllPages);
                oSheet.PageSetup.Footer.Right.AddText(string.Format("Fecha : {0}", _Fecha));


                oSheet.PageSetup.PageOrientation = cxExcel.XLPageOrientation.Portrait;
                oSheet.PageSetup.AdjustTo(63);
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

        public void EnviarCorreoEx(List<string> pArchivo, string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente, string c_llegada, string c_navi_corto, string c_viaje, List<ArchivoExport> pListaNo)
        {
            string Html;
            int i = 1;
            EnvioCorreo _correo = new EnvioCorreo();
            try
            {

                Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                Html += "MÓDULO : LISTADO AUTORIZADO DE CONTENEDORES DE EXPORTACIÓN  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE LISTADO AUTORIZADO DE CONTENEDORES DE EXPORTACIÓN <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes a {0} para el barco {1}, con # de Viaje {2}, han sido validados {3} de {4} contenedores correspondientes a este barco.-", c_navi_corto, d_buque, c_viaje, pValor, pCantidad);
                Html += "<br /><br/>";

                if (pListaNo.Count > 0)
                {
                    Html += "Los siguientes contenedores fueron denegados por PATIO DE CONTENEDORES para su revisión se detallan a continuación : ";
                    Html += "< br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";                    
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>DENEGADO POR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";
                    
                    foreach (ArchivoExport item in pListaNo)
                    {                        
                        Html += "<tr>";
                        Html += "<center>";                        
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_tamaño + "</font></td>";                        
                        Html += "<td height=\"25\"><font size=2 color=blue>" + "Por incumplimiento con " + item.t_estatus + "</font></td>";
                        Html += "<center>";
                        Html += "</tr>";
                        Html += "</font>";
                    }
                    Html += "</table><br /><br/>";                   

                }

                Html += "<br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> ACCIONES A REALIZAR: </b></font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> TODOS: </b></font><br />";
                Html += "<font color=blue>Impresión de los listados para ser utilizado en la operación del buque</font><br /><br />";
                Html += "<font style=\"color:#1F497D;\"><b> NAVIERA: </b></font><br />";
                Html += "<font color=blue>Responder de RECIBIDO, si hubieron contenedores sin autorizar pueden volver a cargar un nuevo listado</font><br /><br />";


                _correo.Subject = string.Format("PASO 4 de 4: Autorización de Listado de EXPORTACIÓN de {0} para el Buque: {1}, # de Viaje {2}, Cod. de Llegada # {3}", c_navi_corto, d_buque, c_viaje, c_llegada);
                //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
                _correo.ListArch = pArchivo;

                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_auto_ex", DBComun.Estado.verdadero, c_cliente);


                List<Notificaciones> _listaCC = new List<Notificaciones>();

                if (c_cliente != "11" && c_cliente != "216")
                    _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_auto_ex", DBComun.Estado.verdadero, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_auto_ex", DBComun.Estado.verdadero, "219"));
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
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
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

        [WebMethod]
        public static List<TipoRevisiones> LlenarTipo()
        {
            TipoRevisiones _tipo = new TipoRevisiones
            {
                IdRevision = 1,
                t_revision = "ADUANA"
            };

            TipoRevisiones _tipo1 = new TipoRevisiones
            {
                IdRevision = 2,
                t_revision = "CEPA"
            };

            List<TipoRevisiones> pLista = new List<TipoRevisiones>();

            pLista.Add(_tipo);
            pLista.Add(_tipo1);
            

            return pLista;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlTipoAuto = (e.Row.FindControl("ddlTipoAuto") as DropDownList);
                CheckBox chk = (e.Row.FindControl("CheckBox1") as CheckBox);
                if (chk != null)
                {
                    
                    TipoRevisiones _tipo = new TipoRevisiones
                    {
                        IdRevision = 1,
                        t_revision = "ADUANA"
                    };

                    TipoRevisiones _tipo1 = new TipoRevisiones
                    {
                        IdRevision = 2,
                        t_revision = "CEPA"
                    };

                    List<TipoRevisiones> pLista = new List<TipoRevisiones>();

                    pLista.Add(_tipo);
                    pLista.Add(_tipo1);



                    ddlTipoAuto.DataSource = pLista;
                    ddlTipoAuto.DataTextField = "t_revision";
                    ddlTipoAuto.DataValueField = "IdRevision";
                    ddlTipoAuto.DataBind();

                    ddlTipoAuto.Items.Insert(0, new ListItem("-- Seleccionar -- "));
                    if (ddlTipoAuto != null)
                    {
                        //assign the JavaScript function to execute when the checkbox is clicked               
                        //pass in the checkbox element and the client id of the select
                        
                        //chk.Attributes.Add("onchange", string.Format("toggleSelectList(this, '{0}');", ddlTipoAuto.ClientID));


                    }
                }       
               

                ScriptManager current = ScriptManager.GetCurrent(Page);
                if (current != null)
                {
                    current.RegisterAsyncPostBackControl(ddlTipoAuto);
                
                }
   

            }
        }
    }
}
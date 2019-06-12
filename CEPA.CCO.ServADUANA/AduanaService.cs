using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using System.Timers;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Globalization;
using CEPA.CCO.Linq;
using System.Xml;
using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;

namespace CEPA.CCO.ServADUANA
{
    public partial class AduanaService : ServiceBase
    {
        System.Timers.Timer tmpService = null;
        bool EnProceso = false;
        string Archivo = Application.StartupPath + "\\CEPA_CCO_ADUANA.TXT";
        Linq.NotiNavieras _notiNavieras = new Linq.NotiNavieras();
        bool Error = false;


        public AduanaService()
        {
            InitializeComponent();
            tmpService = new System.Timers.Timer(300000);
            tmpService.Elapsed += new ElapsedEventHandler(tmpService_Elapsed);
        }

        protected override void OnStart(string[] args)
        {
            TextWriter tw = new StreamWriter(Archivo, true);
            try
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ===>> INICIO DE SERVICIO <<===");
                tw.Flush();
                tw.Dispose();
                tw.Close();
                tmpService.Interval = 60000;

                EnvioServicio("SERVICIO ADUANA SE INICIO", "SERVICIO DE ADUANA SE INICIO");

                tmpService.Enabled = true;
            }
            catch (Exception ex)
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDO INICIAR EL SERVICIO (" + ex.Message + ")");
                tw.Flush();
                tw.Dispose();
                tw.Close();
                tmpService.Enabled = false;

                EnvioServicio("SERVICIO ADUANA SE DETUVO", ex.Message);

                this.Stop();
            }
        }

        protected override void OnStop()
        {
            TextWriter tw = new StreamWriter(Archivo, true);
            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ====> FIN DE PROCESO <====" + "\n\n");
            tw.Flush();
            tw.Dispose();
            tw.Close();

            EnvioServicio("SERVICIO ADUANA SE DETUVO", "SERVICIO DE ADUANA SE DETUVO");


            tmpService.Enabled = false;
        }

        private static void EnvioServicio(string mensaje, string asunto)
        {
            EnvioCorreo _correo = new EnvioCorreo();
            string Html = null;

            _correo.Subject = asunto;

            Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
            Html += string.Format("<b><u> {0} </b></u><br />", mensaje);


            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_cancela", DBComun.Estado.falso, "0"); ;

            _correo.Asunto = Html;
            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
        }

        void tmpService_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (EnProceso == true)
                return;


            EnProceso = true;

           TransmisionRecep();

           ValiMani();

            RetencionDAN();

            LiberacionDAN();

            CorteCOTECNA();

            AlertaDAN();

            EnProceso = false;

        }

        private void ValiMani()
        {
            TextWriter tw = new StreamWriter(Archivo, true);

            try
            {
                List<string> _resultado = ValidacionADUANA();

                if (_resultado == null)
                    _resultado = new List<string>();

                if (_resultado.Count > 0)
                {
                    foreach (string item in _resultado)
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": VALIDACION Y AUTORIZACION: " + item);
                    }
                }

                tw.Flush();
                tw.Dispose();
                tw.Close();
            }
            catch (Exception ex)
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: " + ex.Message + " " + ex.Source + " " + ex.StackTrace + " " + ex.TargetSite);
                tw.Flush();
                tw.Dispose();
                tw.Close();
                this.Stop();
            }
        }

        private void TransmisionRecep()
        {
            TextWriter tw = new StreamWriter(Archivo, true);

            try
            {
                List<string> _resultado = ObtenerResultados();
                if (Error == false)
                {
                    if (_resultado == null)
                        _resultado = new List<string>();

                    if (_resultado.Count > 0)
                    {
                        foreach (string item in _resultado)
                        {

                            if (item.Substring(0, 2) != "1|")
                            {
                                string act = TrasnferXMLDAL.ActTrasmision(Convert.ToInt32(item));
                                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION IdDeta #: " + item);
                            }
                            else
                                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": RESULTADO CAPTURADO: " + item);
                        }
                    }
                    else
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ENVIADO: SERVICIO EJECUTADO ");

                    }
                }
                else
                {
                    if (_resultado.Count > 0)
                    {
                        foreach (string item in _resultado)
                        {
                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": SERVICIO PRESENTO PROBLEMA: " + item);
                        }
                    }

                    Error = false;
                }


                tw.Flush();
                tw.Dispose();
                tw.Close();
            }
            catch (Exception ex)
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: " + ex.Message + "SOURCE : " + ex.Source + "TARGET : " + ex.TargetSite + "STACKTRACE : " + ex.StackTrace);
                tw.Flush();
                tw.Dispose();
                tw.Close();
                this.Stop();
            }
        }

        private List<string> ValidacionADUANA()
        {
            //Pendientes de validación
            List<DocBuque> pLstPendientes = DocBuqueLINQ.ObtenerAduana(DBComun.Estado.falso);
            int _resulDeta = 0;
            List<string> pListaNo = new List<string>();
            List<string> pRespuesta = new List<string>();

            if (pLstPendientes == null)
                pLstPendientes = new List<DocBuque>();

            DateTime _fecha;
            string Html;
            bool mEstado = false;
            bool mAuto = false;

            EnvioCorreo _correo = new EnvioCorreo();

            #region "cod"

            if (pLstPendientes.Count > 0)
            {
                foreach (var iPendiente in pLstPendientes)
                {
                    pListaNo = new List<string>();
                    pRespuesta = new List<string>();
                    string resul = ResulNavieraDAL.EliminarManifiesto(DBComun.Estado.falso, iPendiente.num_manif, iPendiente.a_manifiesto);

                    MemoryStream memoryStream = new MemoryStream();
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Encoding = new UTF8Encoding(false);
                    xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
                    xmlWriterSettings.Indent = true;

                    XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                    CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();


                    string _Aduana = null;

                    xml.WriteStartDocument();

                    xml.WriteStartElement("MDS4");

                    xml.WriteElementString("CAR_REG_YEAR", iPendiente.a_manifiesto.ToString());
                    xml.WriteElementString("KEY_CUO", "02");
                    xml.WriteElementString("CAR_REG_NBER", iPendiente.num_manif.ToString());

                    xml.WriteEndDocument();
                    xml.Flush();
                    xml.Close();

                    //Generar XML para enviar parametros al servicio.
                    _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(_proxy.getContenedorData(_Aduana));

                    XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4/MDS5");

                    XmlNode unContenedor;

                    List<ArchivoAduanaValid> pGuarda = new List<ArchivoAduanaValid>();

                    //Contenedore devueltos por ADUANA
                    if (listaCntres.Count > 0)
                    {

                        for (int i = 0; i < listaCntres.Count; i++)
                        {
                            unContenedor = listaCntres.Item(i);

                            string _contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText;


                            ArchivoAduanaValid validAduana = new ArchivoAduanaValid
                            {
                                IdValid = -1,
                                n_contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText.Replace("-", ""),
                                n_manifiesto = iPendiente.num_manif,
                                n_BL = unContenedor.SelectSingleNode("KEY_BOL_REF").InnerText,
                                a_mani = iPendiente.a_manifiesto
                            };

                            //Almacenar manifiesto devuelto por aduana
                            _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValid(validAduana, DBComun.Estado.falso));

                            if (_resulDeta == 2)
                                pListaNo.Add(validAduana.n_contenedor);
                        }

                        //Verificar si ya fueron validados.
                        int validR = Convert.ToInt32(DetaDocDAL.VerificarValid((int)iPendiente.num_manif, (int)iPendiente.IdReg, DBComun.Estado.falso, iPendiente.IdDoc));


                        List<EncaNaviera> pEnca = new List<EncaNaviera>();
                        //Cuando no posee validacion 
                        if (validR == 2)
                        {
                            pEnca = EncaNavieraDAL.ObtenerNavierasValid(DBComun.Estado.falso, iPendiente.num_manif, (int)iPendiente.IdReg, iPendiente.a_manifiesto, iPendiente.IdDoc);

                            EnvioValidacion(pListaNo, ref mEstado, ref mAuto, ref pRespuesta, ref _correo, iPendiente, pEnca);

                        }
                        //Cuando ya fue validado pero rechazado
                        else if (validR == 0)
                        {

                            //Verificar inconsistencias con las actuales.
                            bool _incoC = false;
                            bool _autoAduana = false;
                            pEnca = EncaNavieraDAL.ObtenerNavierasValid(DBComun.Estado.falso, iPendiente.num_manif, (int)iPendiente.IdReg, iPendiente.a_manifiesto, iPendiente.IdDoc);


                            foreach (EncaNaviera pIncoC in pEnca)
                            {

                                List<ContenedoresAduana> pListaNoAduana = ResulNavieraDAL.ObtenerAutorizadosNOADUANA(DBComun.Estado.falso, iPendiente.num_manif, pIncoC.c_naviera);

                                if (pListaNoAduana.Count > 0)
                                {

                                    List<ContenedoresAduana> pLista1 = ResulNavieraDAL.ObtenerNoInco(DBComun.Estado.falso, iPendiente.num_manif, pIncoC.c_naviera, iPendiente.a_manifiesto);

                                    if (pLista1.Count > 0)
                                        _incoC = true;
                                }
                                else if (pListaNoAduana.Count == 0)
                                {
                                    _autoAduana = true;
                                }
                            }


                            if (_incoC == true || _autoAduana == true)
                            {

                                EnvioValidacion(pListaNo, ref mEstado, ref mAuto, ref pRespuesta, ref _correo, iPendiente, pEnca);

                            }

                            _incoC = false;
                            _autoAduana = false;
                        }

                        //Paso 4 de 4
                        if (mAuto == true)
                        {
                            GeneAuto(iPendiente, ref pRespuesta);
                            mAuto = false;
                        }

                    }
                    else
                    {
                        pRespuesta.Add(string.Format("PASO 3 de 4: Validación ADUANA: DENEGADA(NO ENCONTRO MANIFIESTO) Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", iPendiente.d_cliente, iPendiente.d_buque, iPendiente.c_voyage, iPendiente.num_manif));
                    }
                }


            }
            else
            {
                pRespuesta.Add("NO EXISTEN MANIFIESTOS PENDIENTES");
            }

            #endregion 
           

            return pRespuesta;

        }

        private static void GeneAuto(DocBuque iPendiente, ref List<string> pRespuesta)
        {
            int valores = 0;
            //int cantidad = cant;
            int Correlativo = 0;
            int cont = 0;
            NotiAutorizadosSer _notiAutorizados = new NotiAutorizadosSer();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();

            int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarDeta(DBComun.Estado.falso, (int)iPendiente.IdReg, iPendiente.IdDoc));

            if (_resultado > 0)
            {
                valores = _resultado;
                cont = cont + 1;
            }

            if (valores > 0)
            {

                List<DetaNaviera> _listAutorizados = DetaNavieraDAL.ObtenerAutorizados((int)iPendiente.IdReg, iPendiente.IdDoc, DBComun.Estado.falso);
                Correlativo = Convert.ToInt32(DetaNavieraDAL.ObtenerCorrelativo(iPendiente.c_imo, iPendiente.c_llegada, iPendiente.c_cliente, DBComun.Estado.falso)) + 1;

                List<DetaNaviera> _list = new List<DetaNaviera>();

                for (int i = 1; i < 7; i++)
                {

                    var consulta = DetaNavieraLINQ.AlmacenarArchivo(_listAutorizados, DBComun.Estado.falso, i);

                    if (consulta.Count > 0)
                        _list.AddRange(consulta);

                    foreach (var item_C in consulta)
                    {
                        _listAutorizados.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                    }
                }

                if (_list.Count > 0)
                {
                    foreach (DetaNaviera deta in _list)
                    {
                        int _actCorre = Convert.ToInt32(DetaNavieraDAL.ActualizarCorrelativo(DBComun.Estado.falso, Correlativo, deta.IdDeta));
                        Correlativo = Correlativo + 1;
                    }
                }

                int _rango = ((Correlativo - 1) - valores) + 1;
                List<ArchivoAduana> _listaAOrdenar = DetaNavieraDAL.ObtenerResultado((int)iPendiente.IdReg, _rango, Correlativo - 1, DBComun.Estado.falso);

                //foreach (var item in _listaAOrdenar)
                //{
                //    DetaNaviera pDeta = new DetaNaviera
                //    {
                //        n_contenedor = item.n_contenedor,
                //        b_manejo = item.b_manejo.Trim().TrimEnd().TrimStart(),
                //        b_transferencia = item.b_transferencia.Trim().TrimEnd().TrimStart(),
                //        b_despacho = item.b_despacho.Trim().TrimEnd().TrimStart(),
                //        c_cliente = iPendiente.c_cliente,
                //        c_llegada = iPendiente.c_llegada,
                //        c_tamaño = item.c_tamaño,
                //        b_estado = item.b_estado,
                //        c_condicion = item.c_condicion,
                //        v_peso = item.v_peso,
                //        s_consignatario = item.s_consignatario
                //    };

                //    string resultado = DetaNavieraDAL.AlmacenarSADFI(pDeta, DBComun.Estado.falso);

                //}


                foreach (var item in _listaAOrdenar)
                {
                    DetaNaviera pDeta = new DetaNaviera
                    {
                        n_contenedor = item.n_contenedor,
                        b_manejo = item.b_manejo.Trim().TrimEnd().TrimStart(),
                        b_transferencia = item.b_transferencia.Trim().TrimEnd().TrimStart(),
                        b_despacho = item.b_despacho.Trim().TrimEnd().TrimStart(),
                        c_cliente = iPendiente.c_cliente,
                        c_llegada = iPendiente.c_llegada,
                        c_tamaño = item.c_tamaño,
                        b_estado = item.b_estado,
                        c_condicion = item.c_condicion,
                        v_peso = item.v_peso,
                        s_consignatario = item.s_consignatario,
                        c_correlativo = item.c_correlativo,
                        v_tara = item.v_tara,
                        s_comodity = item.s_comodity,
                        c_pais_origen = item.c_pais_origen,
                        c_pais_destino = item.c_pais_destino
                    };

                    string resultado = DetaNavieraDAL.AlmacenarSADFI_AMP(pDeta, DBComun.Estado.falso);

                }

                _notiAutorizados.IdDoc = iPendiente.IdDoc;
                _notiAutorizados.GenerarAplicacionCX(_listaAOrdenar, iPendiente.c_cliente, iPendiente.d_cliente, iPendiente.c_llegada, (int)iPendiente.IdReg,
                 iPendiente.d_buque, iPendiente.f_llegada, valores, valores, null);



                pRespuesta.Add(string.Format("PASO 4 DE 4: GENERADO LISTADO BUQUE {0} NAVIERA {1} ", iPendiente.d_buque, iPendiente.d_cliente));
            }
        }

        private static void EnvioValidacion(List<string> pListaNo, ref bool mEstado, ref bool mAuto, ref List<string> pRespuesta, ref EnvioCorreo _correo, DocBuque iPendiente, List<EncaNaviera> pEnca)
        {
            string Html;
            DateTime _fecha;

            List<ContenedoresAduana> pLista = new List<ContenedoresAduana>();
            List<ContenedoresAduana> pLista1 = new List<ContenedoresAduana>();
            List<ContenedoresAduana> pLista2 = new List<ContenedoresAduana>();
            List<ContenedoresAduana> pLista3 = new List<ContenedoresAduana>();


            if (pEnca.Count > 0)
            {
                foreach (EncaNaviera itemEnca in pEnca)
                {
                    pLista = new List<ContenedoresAduana>();
                    pLista1 = new List<ContenedoresAduana>();
                    pLista2 = new List<ContenedoresAduana>();
                    pLista3 = new List<ContenedoresAduana>();


                    string _rinco = ResulNavieraDAL.EliminarInco(DBComun.Estado.falso, (int)iPendiente.IdReg, iPendiente.num_manif, itemEnca.c_naviera);

                    mEstado = false;
                    mAuto = false;
                    pRespuesta = new List<string>();

                    string c_navi_corto = EncaNavieraDAL.ObtenerNavi(DBComun.Estado.falso, itemEnca.c_naviera);

                    Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                    Html += "<b><u> CARGA DE ARCHIVO  </b></u><br />";
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                    Html += "<tr>";
                    _fecha = DetaNavieraLINQ.FechaBDS();
                    Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                    Html += "</tr>";
                    Html += "<tr>";
                    Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + "servicio.contenedores" + "</font></td>";
                    Html += "</tr>";
                    Html += "</table>";
                    Html += "<br />";

                    pLista = ResulNavieraDAL.ObtenerAutorizadosADUANA(DBComun.Estado.falso, iPendiente.num_manif, itemEnca.c_naviera);


                    Html += "<b><u>DETALLE DE VALIDACIÓN</b></u><br />";
                    Html += "<br />";
                    Html += string.Format("<b><u>AUTORIZADOS ({0})</b></u><br />", pLista.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    if (pLista.Count > 0)
                    {
                        foreach (ContenedoresAduana item in pLista)
                        {
                            Html += "<tr>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                            Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                            Html += "</tr>";
                            Html += "</font>";

                        }
                    }
                    else
                    {
                        foreach (ContenedoresAduana item in pLista)
                        {
                            Html += "<tr>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                            Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                            Html += "</tr>";
                            Html += "</font>";
                            mEstado = true;
                        }

                        mEstado = true;
                    }

                    Html += "</table>";

                    pLista1 = ResulNavieraDAL.ObtenerAutorizadosNOADUANA(DBComun.Estado.falso, iPendiente.num_manif, itemEnca.c_naviera);

                    Html += "<br />";
                    Html += string.Format("<b><u>NO REPORTADOS A ADUANA ({0})</b></u><br />", pLista1.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (ContenedoresAduana item in pLista1)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                        mEstado = true;


                        IncoAduana _incoAduana = new IncoAduana
                        {
                            IdReg = (int)iPendiente.IdReg,
                            c_naviera = itemEnca.c_naviera,
                            n_contenedor = item.n_contenedor,
                            n_manifiesto = iPendiente.num_manif,
                            a_mani = iPendiente.a_manifiesto
                        };

                        string _alInco = DetaNavieraDAL.AlmacenarInco(_incoAduana, DBComun.Estado.falso);


                    }

                    Html += "</table>";

                    pLista2 = ResulNavieraDAL.ObtenerAutorizadosNONAVIERA(DBComun.Estado.falso, iPendiente.num_manif, itemEnca.c_naviera);

                    Html += "<br />";
                    Html += string.Format("<b><u>NO REPORTADOS A CEPA ({0})</b></u><br />", pLista2.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (ContenedoresAduana item in pLista2)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                    }

                    Html += "</table>";



                    Html += "<br />";
                    Html += string.Format("<b><u>CONTENEDORES QUE NO PASAN VALIDACIÓN SEGUN BIC ({0})</b></u><br />", pListaNo.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    int cont = 0;

                    foreach (string item in pListaNo)
                    {
                        cont = cont + 1;
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + cont.ToString() + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                        //mEstado = true;
                    }

                    Html += "</table>";


                    pLista3 = ResulNavieraDAL.ObtenerRepetidos(DBComun.Estado.falso, iPendiente.num_manif);

                    Html += "<br />";
                    Html += string.Format("<b><u>REPETIDOS EN MANIFIESTO SEGÚN ADUANA ({0})</b></u><br />", pLista3.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (ContenedoresAduana item in pLista3)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                    }

                    Html += "</table>";

                    if (mEstado == true)
                    {
                        Html += "<br /><br />";
                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                        Html += "<font color=red>En espera de correcciones por la Naviera </font>";
                        _correo.Subject = string.Format("PASO 3 de 4: Validación ADUANA: DENEGADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_navi_corto, iPendiente.d_buque, itemEnca.c_voyage, iPendiente.num_manif);
                        int resuly = Convert.ToInt32(DetaDocDAL.ActualizarValidacion(0, iPendiente.num_manif, (int)iPendiente.IdReg, DBComun.Estado.falso, iPendiente.IdDoc));
                        pRespuesta.Add(string.Format("PASO 3 de 4: Validación ADUANA: DENEGADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_navi_corto, iPendiente.d_buque, itemEnca.c_voyage, iPendiente.num_manif));
                    }
                    else
                    {
                        Html += "<br /><br />";
                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                        Html += "<font color=blue>En espera autorización de ADUANA </font>";
                        _correo.Subject = string.Format("PASO 3 de 4: Validación ADUANA: ACEPTADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_navi_corto, iPendiente.d_buque, itemEnca.c_voyage, iPendiente.num_manif);
                        int resuly = Convert.ToInt32(DetaDocDAL.ActualizarValidacion(1, iPendiente.num_manif, (int)iPendiente.IdReg, DBComun.Estado.falso, iPendiente.IdDoc));
                        pRespuesta.Add(string.Format("PASO 3 de 4: Validación ADUANA: ACEPTADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_navi_corto, iPendiente.d_buque, itemEnca.c_voyage, iPendiente.num_manif));
                        mAuto = true;
                    }


                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.falso, "219");

                    List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.falso, itemEnca.c_naviera);

                    if (_listaCC == null)
                        _listaCC = new List<Notificaciones>();

                    _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_carga", DBComun.Estado.falso, itemEnca.c_naviera));
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


            }
        }

        public void WriteFullElementString(XmlWriter xml,
                                         string localName,
                                         string value)
        {
            xml.WriteStartElement(localName);
            xml.WriteString(value);
            xml.WriteFullEndElement();
        }

        public List<string> ObtenerResultados()
        {
            //string ruta = Application.StartupPath +  "\\ArchivoXML.TXT";
            XmlDocument x = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            TransferXML _clase = new TransferXML();

            List<string> _resultado = new List<string>();

            CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();


            string _Aduana = null;

            List<TransferXML> _pListM = CEPA.CCO.DAL.TrasnferXMLDAL.TransferenciaCabeceraM(DBComun.Estado.falso);

            //using (XmlWriter xml = XmlWriter.Create(sb))

            foreach (var itemM in _pListM)
            {

                MemoryStream memoryStream = new MemoryStream();
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Encoding = new UTF8Encoding(false);
                xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
                xmlWriterSettings.Indent = true;

                XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                List<TransferXML> _pList = CEPA.CCO.DAL.TrasnferXMLDAL.TransferenciaCabecera(DBComun.Estado.falso, itemM.nmanifiesto, itemM.year);
                List<TransferXML> _lista = CEPA.CCO.DAL.TrasnferXMLDAL.Transferencia(DBComun.Estado.falso, itemM.nmanifiesto, itemM.year);



                xml.WriteStartDocument();




                xml.WriteStartElement("MDS4S");
                xml.WriteStartElement("MDS4");

                foreach (var enca in _lista)
                {
                    _clase.year = enca.year;
                    _clase.aduana = enca.aduana;
                    _clase.nmanifiesto = enca.nmanifiesto;

                    break;
                }


                xml.WriteElementString("CAR_REG_YEAR", _clase.year);
                xml.WriteElementString("KEY_CUO", _clase.aduana);
                xml.WriteElementString("CAR_REG_NBER", _clase.nmanifiesto);

                xml.WriteStartElement("MDS6S");
                foreach (var item in _pList)
                {

                    xml.WriteStartElement("MDS6");

                    xml.WriteElementString("KEY_BOL_REF", item.nbl);

                    xml.WriteStartElement("MDS7S");
                    foreach (var itemC in _lista.Where(a => a.nbl == item.nbl).Distinct())
                    {

                        xml.WriteStartElement("MDS7");
                        xml.WriteElementString("CAR_DIS_DATE", itemC.f_rpatio);
                        xml.WriteElementString("CAR_CTN_IDENT", itemC.contenedor);
                        xml.WriteElementString("CARBOL_SHP_MARK78", itemC.sitio);
                        WriteFullElementString(xml, "CARBOL_SHP_MARK90", itemC.comentarios.Replace(" ", ""));
                        xml.WriteEndElement();

                        _resultado.Add(itemC.IdDeta.ToString());
                    }

                    xml.WriteEndElement();
                    xml.WriteEndElement();

                }
                xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndDocument();
                // _Aduna = str.ToString();
                xml.Flush();
                xml.Close();

                _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());


                string resultado = _proxy.updateCepaData(_Aduana);

                string sadfi_res = DetaNavieraDAL.ActSADFI_AMP(_lista, DBComun.Estado.falso);

                string act_res = DetaNavieraDAL.ActSADFI_BD(DBComun.Estado.falso, _lista);

                //string resultado = "1| Mensaje";

                if (resultado.Substring(0, 1) == "1")
                {
                    _resultado.Add(resultado);
                }
                else
                {
                    Error = true;
                    _resultado.Add(resultado);
                }
            }

            //TextWriter tw = new StreamWriter(ruta, true);
            //tw.WriteLine(_Aduana);
            //tw.Flush();
            //tw.Dispose();
            //tw.Close();
            return _resultado;
        }

        private void RetencionDAN()
        {
            List<TransferXML> retenidos = new List<TransferXML>();
            CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();
            TextWriter tw = new StreamWriter(Archivo, true);

            retenidos = TransmiDANDAL.TransDANR(DBComun.Estado.falso, "b_transdanr = 0 AND f_reg_dan IS NOT NULL");

            if (retenidos.Count > 0)
            {
                foreach (var item in retenidos)
                {                                     

                    MemoryStream memoryStream = new MemoryStream();
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Encoding = new UTF8Encoding(false);
                    xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
                    xmlWriterSettings.Indent = true;

                    XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);                   


                    string _Aduana = null;

                    xml.WriteStartDocument();

                    xml.WriteStartElement("MDS4");

                    xml.WriteElementString("CAR_REG_YEAR", item.year);
                    xml.WriteElementString("KEY_CUO", item.aduana);
                    xml.WriteElementString("CAR_REG_NBER", item.nmanifiesto);
                    xml.WriteElementString("CAR_CTN_IDENT", item.contenedor);
                    xml.WriteElementString("ACCION", "0");

                    xml.WriteEndDocument();
                    xml.Flush();
                    xml.Close();

                    _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                    string resultado = _proxy.insertContenedorDAN(_Aduana);


                    if (resultado.Substring(0, 1) == "1")
                    {
                        string act = TransmiDANDAL.ActTrasmision(item.IdDeta, "b_transdanr = 1, f_transdanr = GETDATE()");
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION RETENIDO IdDeta #: " + item.IdDeta);
                    }
                    else
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION RETENIDO PRESENTO PROBLEMAS: " + resultado.Replace("0|", ""));
                    }
                }
            }
            else
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO HAY CONTENEDORES RETENIDOS PENDIENTES ");
            }



            tw.Flush();
            tw.Dispose();
            tw.Close();
        }


        private void LiberacionDAN()
        {
            List<TransferXML> liberados = new List<TransferXML>();
            CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();
            TextWriter tw = new StreamWriter(Archivo, true);

            liberados = TransmiDANDAL.TransDANR(DBComun.Estado.falso, "b_transdanl = 0 AND f_reg_liberacion IS NOT NULL");

            


            if (liberados.Count > 0)
            {
                
                foreach (var item in liberados)
                {

                    MemoryStream memoryStream = new MemoryStream();
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Encoding = new UTF8Encoding(false);
                    xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
                    xmlWriterSettings.Indent = true;

                    XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);


                    string _Aduana = null;

                    xml.WriteStartDocument();

                    xml.WriteStartElement("MDS4");

                    xml.WriteElementString("CAR_REG_YEAR", item.year);
                    xml.WriteElementString("KEY_CUO", item.aduana);
                    xml.WriteElementString("CAR_REG_NBER", item.nmanifiesto);
                    xml.WriteElementString("CAR_CTN_IDENT", item.contenedor);
                    xml.WriteElementString("ACCION", "1");

                    xml.WriteEndDocument();
                    xml.Flush();
                    xml.Close();

                    _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                    string resultado = _proxy.insertContenedorDAN(_Aduana);


                    if (resultado.Substring(0, 1) == "1")
                    {
                        string act = TransmiDANDAL.ActTrasmision(item.IdDeta, "b_transdanl = 1, f_transdanl = GETDATE()");
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION LIBERADO IdDeta #: " + item.IdDeta);
                    }
                    else
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION LIBERADO PRESENTO PROBLEMAS: " + resultado.Replace("0|", ""));
                    }
                }
            }
            else
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO HAY CONTENEDORES LIBERADOS PENDIENTES ");
            }



            tw.Flush();
            tw.Dispose();
            tw.Close();
        }

        private void CorteCOTECNA()
        {
            List<CorteCOTECNA> pCortePendiente = new List<CorteCOTECNA>();

            TextWriter tw = new StreamWriter(Archivo, true);

            pCortePendiente = CorteCOTECNADAL.CodLlegadasGroup(DBComun.Estado.falso);

            if (pCortePendiente.Count > 0)
            {
                //List<CorteCOTECNA> pGrupo = pCortePendiente.GroupBy(p => p.c_llegada).Select(g => g.First()).ToList();

                foreach (CorteCOTECNA item in pCortePendiente)
                {
                    string Html = "";
                    List<CorteCOTECNA> pBuquesCorte = new List<CorteCOTECNA>();
                    List<CorteCOTECNA> pCorteConte = new List<CorteCOTECNA>();

                    pCortePendiente = CorteCOTECNADAL.CorteLlegadas(DBComun.Estado.falso, item.c_llegada);

                    
                    pBuquesCorte = CorteCOTECNADAL.BuquesCorte(DBComun.Estado.falso, item.c_llegada);


                    if(pBuquesCorte.Count > 0)
                    {
                        string barco = null;
                        string f_atraque = null;


                        List<CorteCOTECNA> query = (from a in pCortePendiente
                                     join b in pBuquesCorte on new { c_llegada = a.c_llegada, c_cliente = a.c_cliente } equals new { c_llegada = b.c_llegada, c_cliente = b.c_cliente }
                                     where a.c_llegada == item.c_llegada
                                     select new CorteCOTECNA
                                     {
                                         c_llegada = a.c_llegada,
                                         c_cliente = a.c_cliente,
                                         d_buque = b.d_buque,
                                         d_cliente = b.d_cliente + " - " + a.c_prefijo,
                                         n_manifiesto = a.n_manifiesto,
                                         t_contenedores = a.t_contenedores
                                     }).ToList();

                        if (query == null)
                            query = new List<CorteCOTECNA>();

                        

                        if(query.Count>0)
                        {
                            int c_total = 0;
                            EnvioCorreo _correo = new EnvioCorreo();

                            foreach (var bitem in pBuquesCorte)
	                        {
		                        barco = bitem.d_buque;
                                f_atraque = bitem.f_atraque.ToString("dd/MM/yyyy HH:mm");
                                break;
	                        }

                            Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";

                            Html += "MÓDULO : LISTADO DE CONTENEDORES DE IMPORTACION AUTORIZADOS POR BUQUE <br />";
                            Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES DE IMPORTACION AUTORIZADOS POR BUQUE <br /><br />";

                            Html += string.Format("Estimados, se presenta a continuacion el informe de contenedores de importación autorizados por manifiesto y naviera, procedentes del barco <b>{0}</b>:", barco);
                            Html += "<br/><br/>";

                            Html += "<b><u>RESUMEN</b></u><br />";
                            Html += "<br />";

                            Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                            Html += "<tr>";
                            Html += "<center>";
                            Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>MANIFIESTO</font></th>";
                            Html += "<td width=\"50%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>NAVIERA</font></th>";                          
                            Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TOTAL CONTENEDORES</font></th>";
                            Html += "</center>";
                            Html += "</tr>";

                            foreach (var ritem in query)
                            {
                                Html += "<center>";
                                Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.n_manifiesto + "</font></td>";
                                Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.d_cliente + "</font></td>";
                                Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.t_contenedores.ToString() + "</font></td>";
                                Html += "</center>";
                                Html += "</tr>";
                                c_total = c_total + ritem.t_contenedores;
                            }

                            Html += "<tr>";
                            Html += "<center>";                           
                            Html += "<td colspan=\"2\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font size=2 color=white>TOTAL</font></td>";
                            Html += "<td height=\"25\" bgcolor=#1584CE><font size=2 color=white>" + c_total.ToString() + "</font></td>";
                            Html += "</center>";
                            Html += "</tr>";
                            Html += "</font></table>";


                            _correo.Subject = string.Format("COTECNA: RESUMEN DE CONTENEDORES DE IMPORTACION AUTORIZADOS PARA EL BUQUE {0} CON FECHA DE ATRAQUE {1}", barco, f_atraque);

                            //Notificaciones _notiCC = new Notificaciones
                            //{
                            //    IdNotificacion = -1,
                            //    sMail = "elsa.sosa@cepa.gob.sv",
                            //    dMail = "Elsa Sosa"
                            //};

                            //List<Notificaciones> _ccList = new List<Notificaciones>();
                            //_ccList.Add(_notiCC);

                            //_correo.ListaNoti = _ccList;

                            _correo.ListaNoti = NotificacionesDAL.ObtenerNotiCOTECNA(DBComun.Estado.falso);
                            List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_cotecna", DBComun.Estado.falso, "0");
                            _correo.ListaCC = _listaCC;

                            _correo.Asunto = Html;
                            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);

                            string act = CorteCOTECNADAL.ActCOTECNA(item.c_llegada);

                            if (act != null)
                                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION CORTE COTECNA: " + item.c_llegada);
                        }

                       


                    }


                   

                }
            }
            else
            {
                //Mensaje de no hay validaciones
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO EXISTE CORTE COTECNA PENDIENTE");
            }

            tw.Flush();
            tw.Dispose();
            tw.Close();
        }

        private void AlertaDAN()
        {
            TextWriter tw = new StreamWriter(Archivo, true);
            string Html = null;

            List<ParametroAlerta> pParametroAlerta = new List<ParametroAlerta>();

            pParametroAlerta = ParametroAlertaDAL.ObtenerPara(DBComun.Estado.falso);

            if (pParametroAlerta.Count > 0)
            {
                List<AlertaDANTarja> pValidas = new List<AlertaDANTarja>();

                foreach (var iParam in pParametroAlerta)
                {
                                     

                    List<AlertaDANTarja> pAlertaTarja = new List<AlertaDANTarja>();

                    List<TarjaNotificada> pTarjaNoti = new List<TarjaNotificada>();

                    pAlertaTarja = AlertaDanTarjaDAL.ObtenerTarjas(DBComun.Estado.falso, iParam.c_parametro);

                    pTarjaNoti = TarjasNotificadasDAL.ObtenerTarjasNoti(DBComun.Estado.falso);

                    if (pTarjaNoti.Count > 0)
                    {
                        if (pAlertaTarja.Count > 0)
                        {
                            foreach (var iTarja in pTarjaNoti)
                            {
                                foreach (var iAlerta in pAlertaTarja)
                                {
                                    if (iTarja.c_tarja != iAlerta.c_tarja )
                                    {
                                        AlertaDANTarja pTarjaDAN = new AlertaDANTarja
                                        {
                                            c_tarja = iAlerta.c_tarja,
                                            c_parametro = iAlerta.c_parametro
                                        };

                                        pValidas.Add(pTarjaDAN);
                                    }
                                }
                            }
                        }
                        else
                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO EXISTE ALERTA PENDIENTE DAN ");
                    }
                    else
                    {
                        if (pAlertaTarja.Count > 0)
                            pValidas = pAlertaTarja;
                        else
                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO EXISTE ALERTA PENDIENTE DAN ");
                    }
                }

                if(pValidas.Count > 0)
                {
                    EnvioCorreo _correo = new EnvioCorreo();
                    Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";

                    Html += "MÓDULO : ALERTA DAN POR DESCRIPCION TARJA <br />";
                    Html += "TIPO DE MENSAJE : ALERTA DAN POR DESCRIPCION TARJA <br /><br />";

                    foreach (var item in pValidas)
                    {

                        Html += string.Format("Se encontró la palabra <b>{0}</b> en el # de Tarja : <b>{1}.</b><BR/><BR/>", item.c_parametro, item.c_tarja);

                        TarjaNotificada pTarja = new TarjaNotificada
                        {
                            c_tarja = item.c_tarja,
                            c_parametro = item.c_parametro
                        };

                        string resultado = TarjasNotificadasDAL.AlmacenarTarjas(pTarja, DBComun.Estado.falso);


                    }

                    _correo.Subject = "DAN: ALERTA DAN POR DESCRIPCION EN TARJA";

                    //Notificaciones _notiCC = new Notificaciones
                    //{
                    //    IdNotificacion = -1,
                    //    sMail = "elsa.sosa@cepa.gob.sv",
                    //    dMail = "Elsa Sosa"
                    //};

                    //List<Notificaciones> _ccList = new List<Notificaciones>();
                    //_ccList.Add(_notiCC);

                    //_correo.ListaNoti = _ccList;

                    _correo.ListaNoti = NotificacionesDAL.ObtenerAlertaParametro(DBComun.Estado.falso);                   
                    

                    _correo.Asunto = Html;
                    _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);



                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION DAN ALERTA ENVIADA ");


                }
                else
                {
                    //Mensaje de no hay validaciones
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO EXISTE ALERTA PENDIENTE DAN");
                }
                
            }

            tw.Flush();
            tw.Dispose();
            tw.Close();
        }
    }
}

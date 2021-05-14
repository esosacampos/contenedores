using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Xml;

namespace CEPA.CCO.AduanaService
{
    public partial class AduanaTransfer : ServiceBase
    {
        System.Timers.Timer tmpService = null;
        bool EnProceso = false;
        string Archivo = Application.StartupPath + "\\CEPA_CCO_ADUANA_" + DateTime.Now.ToString("MMyy", CultureInfo.CreateSpecificCulture("es-SV")) + ".TXT";
        Linq.NotiNavieras _notiNavieras = new Linq.NotiNavieras();
        bool Error = false;

        public AduanaTransfer()
        {
            InitializeComponent();
            tmpService = new System.Timers.Timer(300000);
            tmpService.Elapsed += new ElapsedEventHandler(tmpService_Elapsed);
        }

        public class Resultados
        {
            public int IdDeta { get; set; }
            public string n_contenedor { get; set; }
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            try
            {

                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ===>> INICIO DE SERVICIO <<===");
                }


                tmpService.Interval = 180000;

                EnvioServicio("SERVICIO ADUANA SE INICIO", "SERVICIO DE ADUANA SE INICIO");

                tmpService.Enabled = true;

            }
            catch (Exception ex)
            {
                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDO INICIAR EL SERVICIO (" + ex.Message + ")");
                }

                tmpService.Enabled = false;

                //EnvioServicio(ex.Message, "SERVICIO ADUANA SE DETUVO");

                Stop();
            }
        }

        protected override void OnStop()
        {



            using (StreamWriter tw = new StreamWriter(Archivo, true))
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ====> FIN DE PROCESO <====" + "\n\n");
            }

            EnvioServicio("SERVICIO ADUANA SE DETUVO", "SERVICIO DE ADUANA SE DETUVO");


            tmpService.Enabled = false;

        }

        private static void EnvioServicio(string mensaje, string asunto)
        {
            EnvioCorreo _correo = new EnvioCorreo();
            string Html = null;

            _correo.Subject = asunto;

            Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";

            if (mensaje.Contains("MODULO"))
            {
                Html += string.Format("{0}<br />", mensaje);
            }
            else
            {
                Html += string.Format("<b><u> {0} </b></u><br />", mensaje);
            }

            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_service", DBComun.Estado.falso, "0"); ;

            _correo.Asunto = Html;
            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso, "servicio");


        }

        void tmpService_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (EnProceso == true)
            {
                return;
            }

            EnProceso = true;

            try
            {
                CorteCOTECNA();
            }
            catch (Exception ex)
            {
                string detalleExcep;
                string Mensaje;
                string tipoExcepcion;

                getExcepcion(ex, out detalleExcep, out Mensaje, out tipoExcepcion);

                string _cadena = "<b>MENSAJE: </b>" + Mensaje + "<br/>" + "<b>TIPO EXCEPCION: </b>" + tipoExcepcion + "<br/>" + "<b>DETALLE ERROR: </b>" + detalleExcep;

                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDE EJECUTAR ALERTA COTECNA (" + ex.Message + ")");
                }
                EnvioServicio("MODULO ALERTA COTECNA:<br/><br/>Detalle Error : <br/>" + _cadena, "SERVICIO DE ADUANA ERROR");

                //this.Stop();
                //return;
                liquidBuque();
            }

            try
            {
                liquidBuque();
            }
            catch (Exception ex)
            {
                string detalleExcep;

                string Mensaje;

                string tipoExcepcion;
                getExcepcion(ex, out detalleExcep, out Mensaje, out tipoExcepcion);

                string _cadena = "<b>MENSAJE: </b>" + Mensaje + "<br/>" + "<b>TIPO EXCEPCION: </b>" + tipoExcepcion + "<br/>" + "<b>DETALLE ERROR: </b>" + detalleExcep;


                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDE EJECUTAR ALERTA ADUANA (" + ex.Message + ")");
                }
                EnvioServicio("MODULO ALERTA ADUANA:<br/><br/>Detalle Error : <br/>" + _cadena, "SERVICIO DE ADUANA ERROR");

                //this.Stop();

                //return;
                TransmisionRecep();
            }


            try
            {
                TransmisionRecep();
            }
            catch (Exception ex)
            {

                string detalleExcep;

                string Mensaje;

                string tipoExcepcion;
                getExcepcion(ex, out detalleExcep, out Mensaje, out tipoExcepcion);

                string _cadena = "<b>MENSAJE: </b>" + Mensaje + "<br/>" + "<b>TIPO EXCEPCION: </b>" + tipoExcepcion + "<br/>" + "<b>DETALLE ERROR: </b>" + detalleExcep;

                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDE EJECUTAR TRANSMISION DE RECEPCION (" + ex.Message + ")");
                }

                EnvioServicio("MODULO RECEPCION DE CONTENEDORES:<br/><br/>Detalle Error : <br/>" + _cadena, "SERVICIO DE ADUANA ERROR");

                //this.Stop();
                //return;
                TransmisionAuto();

            }

            try
            {
               TransmisionAuto();
            }
            catch (Exception ex)
            {
                string detalleExcep;
                string Mensaje;
                string tipoExcepcion;

                getExcepcion(ex, out detalleExcep, out Mensaje, out tipoExcepcion);

                string _cadena = "<b>MENSAJE: </b>" + Mensaje + "<br/>" + "<b>TIPO EXCEPCION: </b>" + tipoExcepcion + "<br/>" + "<b>DETALLE ERROR: </b>" + detalleExcep;

                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDE EJECUTAR ALERTA TRANSMISION AUTOMATICA (" + ex.Message + ")");
                }
                EnvioServicio("MODULO TRANSMISION AUTOMATICA:<br/><br/>Detalle Error : <br/>" + _cadena, "SERVICIO DE ADUANA ERROR");

                //this.Stop();
                //return;
                ValiMani();
            }

            try
            {
                ValiMani();
            }
            catch (Exception ex)
            {
                string detalleExcep;

                string Mensaje;

                string tipoExcepcion;
                getExcepcion(ex, out detalleExcep, out Mensaje, out tipoExcepcion);

                string _cadena = "<b>MENSAJE: </b>" + Mensaje + "<br/>" + "<b>TIPO EXCEPCION: </b>" + tipoExcepcion + "<br/>" + "<b>DETALLE ERROR: </b>" + detalleExcep;

                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDE EJECUTAR VALIDACION PASO 3 DE 4 (" + ex.Message + ")");
                }
                EnvioServicio("MODULO VALIDAR MANIFIESTO:<br/><br/>Detalle Error : <br/>" + _cadena, "SERVICIO DE ADUANA ERROR");

                //this.Stop();
                //return;
                AlertaOIRSA();
            }


            try
            {
                AlertaOIRSA();
            }
            catch (Exception ex)
            {
                string detalleExcep;

                string Mensaje;

                string tipoExcepcion;
                getExcepcion(ex, out detalleExcep, out Mensaje, out tipoExcepcion);

                string _cadena = "<b>MENSAJE: </b>" + Mensaje + "<br/>" + "<b>TIPO EXCEPCION: </b>" + tipoExcepcion + "<br/>" + "<b>DETALLE ERROR: </b>" + detalleExcep;

                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDE EJECUTAR ALERTA CONTENEDORES PENDIENTES DE RECEPCION (" + ex.Message + ")");
                }
                EnvioServicio("MODULO ALERTA PENDIENTES RECEPCION:<br/><br/>Detalle Error : <br/>" + _cadena, "SERVICIO DE ADUANA ERROR");

                //this.Stop();
                //return;
                RetencionDAN();

            }

            try
            {
                RetencionDAN();
            }
            catch (Exception ex)
            {
                string detalleExcep;

                string Mensaje;

                string tipoExcepcion;
                getExcepcion(ex, out detalleExcep, out Mensaje, out tipoExcepcion);

                string _cadena = "<b>MENSAJE: </b>" + Mensaje + "<br/>" + "<b>TIPO EXCEPCION: </b>" + tipoExcepcion + "<br/>" + "<b>DETALLE ERROR: </b>" + detalleExcep;

                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDE EJECUTAR RETENCION DAN (" + ex.Message + ")");
                }
                EnvioServicio("MODULO RETENCION DAN:<br/><br/>Detalle Error : <br/>" + _cadena, "SERVICIO DE ADUANA ERROR");

                //this.Stop();
                //return;
                LiberacionDAN();

            }

            try
            {
                LiberacionDAN();
            }
            catch (Exception ex)
            {

                string detalleExcep;

                string Mensaje;

                string tipoExcepcion;
                getExcepcion(ex, out detalleExcep, out Mensaje, out tipoExcepcion);

                string _cadena = "<b>MENSAJE: </b>" + Mensaje + "<br/>" + "<b>TIPO EXCEPCION: </b>" + tipoExcepcion + "<br/>" + "<b>DETALLE ERROR: </b>" + detalleExcep;

                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDE EJECUTAR LIBERACION DAN (" + ex.Message + ")");
                }
                EnvioServicio("MODULO LIBERACION DE CONTENEDORES:<br/><br/>Detalle Error : <br/>" + _cadena, "SERVICIO DE ADUANA ERROR");

                Stop();
                return;
            }

            EnProceso = false;


        }

        private static void getExcepcion(Exception ex, out string detalleExcep, out string Mensaje, out string tipoExcepcion)
        {

            Mensaje = ex.Message;
            tipoExcepcion = ex.GetType().Name;
            detalleExcep = ex.StackTrace;


        }

        public static string EscapeXMLValue(string xmlString)
        {

            if (xmlString == null)
            {
                throw new ArgumentNullException("xmlString");
            }

            return xmlString.Replace("'", "&apos;").Replace("\"", "&quot;").Replace("(", "").Replace(")", "").Replace("&", "&amp;");
        }

        private void ValiMani()
        {


            try
            {
                List<string> _resultado = ValidacionADUANA();

                if (_resultado == null)
                {
                    _resultado = new List<string>();
                }

                if (_resultado.Count > 0)
                {
                    foreach (string item in _resultado)
                    {
                        using (StreamWriter tw = new StreamWriter(Archivo, true))
                        {
                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": VALIDACION Y AUTORIZACION: " + item);
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                EnProceso = false;
                throw new Exception(ex.Message);

            }

        }

        private void TransmisionRecep()
        {
            int b_sidu = 0;

            try
            {
                List<string> _resultado = ObtenerResultados(out b_sidu);

            }
            catch (Exception ex)
            {
                EnProceso = false;
                throw new Exception(string.Concat(b_sidu == 0 ? "Sidunea++: " : "SiduneaWorld: ", ex.Message));

            }

        }

        private List<string> ValidacionADUANA()
        {
            //Pendientes de validación
            List<DocBuque> pLstPendientes = DocBuqueLINQ.ObtenerAduana(DBComun.Estado.falso);
            int _resulDeta = 0;
            int _resulDel = 0;
            int _validar = 0;
            string a_manifiesto = "";

            List<string> pListaNo = new List<string>();
            List<string> pRespuesta = new List<string>();
            string _mensaje = null;

            if (pLstPendientes == null)
            {
                pLstPendientes = new List<DocBuque>();
            }

            bool mEstado = false;
            bool mAuto = false;
            int? IdReg = 0;
            int IdDoc = 0;
            int numMa = 0;
            EnvioCorreo _correo = new EnvioCorreo();
            try
            {

                #region "cod"


                if (pLstPendientes.Count > 0)
                {
                    foreach (var iPendiente in pLstPendientes)
                    {
                        pListaNo = new List<string>();
                        pRespuesta = new List<string>();
                        //string resul = ResulNavieraDAL.EliminarManifiesto(DBComun.Estado.falso, iPendiente.num_manif, iPendiente.a_manifiesto, iPendiente.b_sidunea);

                        MemoryStream memoryStream = new MemoryStream();
                        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                        {
                            Encoding = new UTF8Encoding(false),
                            ConformanceLevel = ConformanceLevel.Document,
                            Indent = true
                        };

                        XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                        string _Aduana = null;

                        xml.WriteStartDocument();

                        xml.WriteStartElement("MDS4");

                        xml.WriteElementString("CAR_REG_YEAR", iPendiente.a_manifiesto.ToString());
                        xml.WriteElementString("KEY_CUO", "02");
                        xml.WriteElementString("CAR_REG_NBER", iPendiente.num_manif.ToString());

                        xml.WriteEndDocument();
                        xml.Flush();
                        xml.Close();

                        IdReg = iPendiente.IdReg;
                        IdDoc = iPendiente.IdDoc;
                        numMa = iPendiente.num_manif;

                        //Generar XML para enviar parametros al servicio.
                        _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                        XmlDocument doc = new XmlDocument();
                        if (iPendiente.b_sidunea == 0)
                        {
                            CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();
                            string s = EscapeXMLValue(_proxy.getContenedorData(_Aduana));

                            doc.LoadXml(s);




                        }
                        else
                        {
                            CepaSiduneaWorld.WSManifiestoCEPAClient _proxy = new CepaSiduneaWorld.WSManifiestoCEPAClient();
                            string _user = ConfigurationManager.AppSettings["userSidunea"];
                            string _psw = ConfigurationManager.AppSettings["pswSidunea"];

                            _proxy.ClientCredentials.UserName.UserName = _user;
                            _proxy.ClientCredentials.UserName.Password = _psw;

                            _mensaje = _proxy.getContenedorData(_Aduana);
                            if (_mensaje.Substring(0, 1) == "0")
                            {
                                pRespuesta.Add("PASO 3 de 4: Validación ADUANA: EL MANIFIESTO # " + string.Concat(iPendiente.a_manifiesto.ToString(), "-", iPendiente.num_manif.ToString()) + " NO PRODUJO RESULTADOS");
                            }
                            else
                            {
                                doc.LoadXml(_mensaje);
                            }
                        }

                        if (doc.ChildNodes.Count > 0)
                        {
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
                                        n_contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText.Replace("-", "").Replace(" ", ""),
                                        n_manifiesto = iPendiente.num_manif,
                                        n_BL = unContenedor.SelectSingleNode("KEY_BOL_REF").InnerText.Trim(),
                                        a_mani = iPendiente.a_manifiesto,
                                        c_tipo_bl = unContenedor.SelectSingleNode("CARBOL_TYP_COD").InnerText.Trim(),
                                        b_sidunea = iPendiente.b_sidunea,
                                        c_tamaño = unContenedor.SelectSingleNode("CAR_CTN_TYP").InnerText.Trim(),
                                        s_agencia = unContenedor.SelectSingleNode("CAR_CAR_NAM").InnerText.Trim(),
                                        v_peso = Convert.ToDouble(unContenedor.SelectSingleNode("CAR_CTN_GWG").InnerText.Trim()),
                                        c_paquete = Convert.ToInt32(unContenedor.SelectSingleNode("CAR_CTN_NBR").InnerText.Trim()),
                                        c_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_COD").InnerText.Trim(),
                                        d_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_NAM").InnerText.Trim(),
                                        c_pais_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(0, 2).Trim(),
                                        d_puerto_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(2, 3).Trim(),
                                        c_pais_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(0, 2).Trim(),
                                        d_puerto_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(2, 3).Trim(),
                                        s_nit = unContenedor.SelectSingleNode("CARBOL_CON_COD") != null ? unContenedor.SelectSingleNode("CARBOL_CON_COD").InnerText.Trim() : "",
                                        s_consignatario = unContenedor.SelectSingleNode("CARBOL_CON_NAM") != null ? unContenedor.SelectSingleNode("CARBOL_CON_NAM").InnerText.Trim() : ""
                                    };

                                    //Almacenar manifiesto devuelto por aduana
                                    _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValid(validAduana, DBComun.Estado.falso));

                                    string valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.falso, validAduana.n_contenedor, DBComun.TipoBD.SqlServer);

                                    if (valida != "VALIDO")
                                    {
                                        pListaNo.Add(validAduana.n_contenedor);
                                    }

                                    if (_resulDeta == 0)
                                    {
                                        _validar = 1;
                                        _resulDel = Convert.ToInt32(DetaNavieraDAL.delMani(validAduana, DBComun.Estado.falso));
                                        if (_resulDel == 0)
                                            _resulDel = Convert.ToInt32(DetaNavieraDAL.delMani(validAduana, DBComun.Estado.falso));

                                        pRespuesta.Add("PASO 3 de 4: Validación ADUANA: EL MANIFIESTO # " + string.Concat(iPendiente.a_manifiesto.ToString(), "-", iPendiente.num_manif.ToString()) + " PRODUJO UN ERROR EL SERVICIO VOLVERA A INTENTAR LA VALIDACIÓN");
                                    }
                                }

                                if (_validar == 0)
                                {
                                    //Verificar si ya fueron validados.
                                    int validR = Convert.ToInt32(DetaDocDAL.VerificarValid(iPendiente.num_manif, (int)iPendiente.IdReg, DBComun.Estado.falso, iPendiente.IdDoc));


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

                                        int pAduana = 0;
                                        foreach (EncaNaviera pIncoC in pEnca)
                                        {
                                            
                                            List<ContenedoresAduana> pListaNoAduana = ResulNavieraDAL.ObtenerAutorizadosNOADUANA(DBComun.Estado.falso, iPendiente.num_manif, pIncoC.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

                                            if (pListaNoAduana.Count > 0)
                                            {

                                                List<ContenedoresAduana> pLista1 = ResulNavieraDAL.ObtenerNoInco(DBComun.Estado.falso, iPendiente.num_manif, pIncoC.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

                                                if (pLista1.Count > 0)
                                                {
                                                    _incoC = true;
                                                }
                                            }
                                            else if (pListaNoAduana.Count == 0)
                                            {
                                                pAduana += 1;
                                            }

                                           
                                            // Inconsistencias Peso
                                            List<ContenedoresAduana> pListaNoWight = ResulNavieraDAL.ObtenerAutorizadosNOWEIGTH(DBComun.Estado.falso, iPendiente.num_manif, pIncoC.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

                                            if (pListaNoWight.Count > 0)
                                            {

                                                List<ContenedoresAduana> pLista1 = ResulNavieraDAL.ObtenerNoInco(DBComun.Estado.falso, iPendiente.num_manif, pIncoC.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

                                                if (pLista1.Count > 0)
                                                {
                                                    _incoC = true;
                                                }
                                            }
                                            else if (pListaNoWight.Count == 0)
                                            {
                                                pAduana += 1;
                                            }

                                         
                                            // Inconsistencias Shipper
                                            List<ContenedoresAduana> pListaShipper = ResulNavieraDAL.ObtenerAutorizadosSHIPPER(DBComun.Estado.falso, iPendiente.num_manif, pIncoC.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

                                            if (pListaShipper.Count > 0)
                                            {

                                                List<ContenedoresAduana> pLista1 = ResulNavieraDAL.ObtenerNoInco(DBComun.Estado.falso, iPendiente.num_manif, pIncoC.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

                                                if (pLista1.Count > 0)
                                                {
                                                    _incoC = true;
                                                }
                                            }
                                            else if (pListaShipper.Count == 0)
                                            {
                                                pAduana += 1;
                                            }

                                        }

                                        if (pAduana == 3)
                                            _autoAduana = true;

                                        if (_incoC == true || _autoAduana == true)
                                        {

                                            EnvioValidacion(pListaNo, ref mEstado, ref mAuto, ref pRespuesta, ref _correo, iPendiente, pEnca);

                                        }
                                        else
                                        {
                                            pRespuesta.Add("NO EXISTEN INCONSISTENCIAS DIFERENTES QUE VALIDAR EN: # MANIFIESTO: " + iPendiente.a_manifiesto + "-"+ iPendiente.num_manif );
                                        }


                                        _incoC = false;
                                        _autoAduana = false;
                                        pAduana = 0;
                                    }


                                    //Paso 4 de 4
                                    if (mAuto == true)
                                    {
                                        GeneAuto(iPendiente, ref pRespuesta);
                                        mAuto = false;
                                    }
                                }

                            }
                            else
                            {
                                pRespuesta.Add(string.Format("PASO 3 de 4: Validación ADUANA: DENEGADA(NO ENCONTRO MANIFIESTO) Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", iPendiente.d_cliente, iPendiente.d_buque, iPendiente.c_voyage, iPendiente.num_manif));
                            }
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
            catch (Exception ex)
            {
                int resuly = Convert.ToInt32(DetaDocDAL.RevertirValidacion(numMa, (int)IdReg, DBComun.Estado.falso, IdDoc));
                int _resultado = Convert.ToInt32(DetaNavieraDAL.RevertirDeta(DBComun.Estado.falso, (int)IdReg, IdDoc));
                string _rinco = ResulNavieraDAL.delADUANA_Auto(DBComun.Estado.falso, numMa, a_manifiesto);
                throw new Exception(ex.Message);
            }

        }

        private static void GeneAuto(DocBuque iPendiente, ref List<string> pRespuesta)
        {
            int valores = 0;
            //int cantidad = cant;
            int Correlativo = 0;
            int cont = 0;
            NotiAutorizadosSer _notiAutorizados = new NotiAutorizadosSer();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();

            try
            {
                int _resulManiAuto = Convert.ToInt32(DetaNavieraDAL.ManifiestoAuto(DBComun.Estado.falso, iPendiente.a_manifiesto, iPendiente.num_manif));
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
                        {
                            _list.AddRange(consulta);
                        }

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

                    List<ArchivoAduana> _listaSADFI = DetaNavieraDAL.getBL_SADFI((int)iPendiente.IdReg, _rango, Correlativo - 1, DBComun.Estado.falso);

                    string an_manifiesto = "";

                    if (_listaSADFI.Count > 0)
                    {
                        foreach (var item in _listaSADFI)
                        {
                            an_manifiesto = item.a_manifiesto;
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
                                c_pais_destino = item.c_pais_destino,
                                b_requiere = item.b_req_tarja,
                                n_BL = item.n_BL,
                                n_manifiesto = item.num_manif.ToString(),
                                a_manifiesto = item.a_manifiesto
                            };

                            string resultado = DetaNavieraDAL.SADFI_BL(pDeta, DBComun.Estado.falso);
                        }
                    }

                    _notiAutorizados.IdDoc = iPendiente.IdDoc;
                    _notiAutorizados.a_manifiesto = an_manifiesto;
                    _notiAutorizados.GenerarAplicacionCX(_listaAOrdenar, iPendiente.c_cliente, iPendiente.d_cliente, iPendiente.c_llegada, (int)iPendiente.IdReg,
                     iPendiente.d_buque, iPendiente.f_llegada, valores, valores, null, iPendiente.b_sidunea);

                    pRespuesta.Add(string.Format("PASO 4 DE 4: GENERADO LISTADO BUQUE {0} NAVIERA {1} ", iPendiente.d_buque, iPendiente.d_cliente));

                    pRespuesta.Add(string.Format("COTECNA: GENERADO LISTADO BUQUE {0} NAVIERA {1} ", iPendiente.d_buque, iPendiente.d_cliente));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            List<ContenedoresAduana> pLista4 = new List<ContenedoresAduana>();

            try
            {
                if (pEnca.Count > 0)
                {
                    foreach (EncaNaviera itemEnca in pEnca)
                    {
                        pLista = new List<ContenedoresAduana>();
                        pLista1 = new List<ContenedoresAduana>();
                        pLista2 = new List<ContenedoresAduana>();
                        pLista3 = new List<ContenedoresAduana>();
                        pLista4 = new List<ContenedoresAduana>();


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

                        pLista = ResulNavieraDAL.ObtenerAutorizadosADUANA(DBComun.Estado.falso, iPendiente.num_manif, itemEnca.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));


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

                        pLista1 = ResulNavieraDAL.ObtenerAutorizadosNOADUANA(DBComun.Estado.falso, iPendiente.num_manif, itemEnca.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

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

                        pLista2 = ResulNavieraDAL.ObtenerAutorizadosNONAVIERA(DBComun.Estado.falso, iPendiente.num_manif, itemEnca.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

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

                        pLista4 = ResulNavieraDAL.ObtenerAutorizadosNOWEIGTH(DBComun.Estado.falso, iPendiente.num_manif, itemEnca.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

                        if (pLista4.Count > 0)
                        {
                            Html += "<br />";
                            Html += string.Format("<b><u>CONTENEDORES QUE NO COINCIDEN EN PESO ({0})</b></u><br />", pLista4.Count);
                            Html += "<br />";

                            Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                            Html += "<tr>";
                            Html += "<center>";
                            Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                            Html += "<td width=\"40px%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                            Html += "<td width=\"40px%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>PESO SEGÚN LISTADO ELECTRÓNICO CEPA</font></th>";
                            Html += "<td width=\"40px%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>PESO SEGÚN MANIFIESTO ELECTRÓNICO DGA</font></th>";
                            Html += "</center>";
                            Html += "</tr>";

                            foreach (ContenedoresAduana item in pLista4)
                            {
                                Html += "<tr>";
                                Html += "<center>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + String.Format("{0:0,0.00}", item.v_cepa) + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + String.Format("{0:0,0.00}", item.v_aduana) + "</font></td>";
                                Html += "<center>";
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
                        }

                        // VALIDACION SHIPPER
                        pLista4 = new List<ContenedoresAduana>();
                        pLista4 = ResulNavieraDAL.ObtenerAutorizadosSHIPPER(DBComun.Estado.falso, iPendiente.num_manif, itemEnca.c_naviera, Convert.ToInt32(iPendiente.a_manifiesto));

                        if (pLista4.Count > 0)
                        {
                            Html += "<br />";
                            Html += string.Format("<b><u>CONTENEDORES QUE NO COINCIDEN COMO SHIPPER OWNED ({0})</b></u><br />", pLista4.Count);
                            Html += "<br />";

                            Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                            Html += "<tr>";
                            Html += "<center>";
                            Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                            Html += "<td width=\"40px%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                            Html += "<td width=\"40px%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>SOC SEGÚN LISTADO ELECTRÓNICO CEPA</font></th>";
                            Html += "<td width=\"40px%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>SOC SEGÚN MANIFIESTO ELECTRÓNICO DGA</font></th>";
                            Html += "</center>";
                            Html += "</tr>";

                            foreach (ContenedoresAduana item in pLista4)
                            {
                                Html += "<tr>";
                                Html += "<center>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.b_ship_cepa + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.b_ship_aduana + "</font></td>";
                                Html += "<center>";
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
                        }

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
                        {
                            _listaCC = new List<Notificaciones>();
                        }

                        _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_carga", DBComun.Estado.falso, itemEnca.c_naviera));
                        _correo.ListaCC = _listaCC;

                        // LIMITE

                        //Notificaciones noti = new Notificaciones
                        //{
                        //    sMail = "elsa.sosa@cepa.gob.sv",
                        //    dMail = "Elsa Sosa"
                        //};

                        //List<Notificaciones> pLisN = new List<Notificaciones>();

                        //pLisN.Add(noti);

                        //_correo.ListaNoti = pLisN;

                        _correo.Asunto = Html;
                        string servicio = "Servicio";
                        _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso, servicio);
                        EnvioErrorCorreo(_correo.Subject, "MODULO VALIDACION :");
                    }



                }
            }
            catch (Exception ex)
            {
                int resuly = Convert.ToInt32(DetaDocDAL.RevertirValidacion(iPendiente.num_manif, (int)iPendiente.IdReg, DBComun.Estado.falso, iPendiente.IdDoc));
                throw new Exception(ex.Message);
            }
        }

        private static void EnvioErrorCorreo(string c_asunto, string c_detalle)
        {
            List<CorreoError> _pListaCorreo = new List<CorreoError>();

            _pListaCorreo = CorreoErroresDAL.Consultar(c_asunto);

            if (_pListaCorreo.Count > 0)
            {
                foreach (CorreoError item in _pListaCorreo)
                {
                    AduanaTransfer _aduanaTransfer = new AduanaTransfer();
                    using (StreamWriter tw = new StreamWriter(_aduanaTransfer.Archivo, true))
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + c_detalle + " " + item.c_mail);
                    }

                    string _actNoti = CorreoErroresDAL.Actualizar(item.c_asunto, item.c_mail);


                }


                EnvioServicio(c_detalle + "<br/><br/>Detalle Error : <br/>" + "CORREOS CON PROBLEMAS DE ENVIO REVISAR TABLA O BITACORA", "SERVICIO DE ADUANA ERROR");



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

        public List<string> ObtenerResultados(out int b_sid)
        {
            //string ruta = Application.StartupPath +  "\\ArchivoXML.TXT";


            try
            {


                List<TransferXML> _pListM = CEPA.CCO.DAL.TrasnferXMLDAL.Transferencia(DBComun.Estado.falso);
                List<string> _resultado = new List<string>();
                b_sid = 0;
                //using (XmlWriter xml = XmlWriter.Create(sb))

                /* Verificar que BL completos listos de enviar */
                if (_pListM.Count > 0)
                {
                    //foreach (var enca in _pListM)
                    //{

                    //    break;
                    //}



                    foreach (var item in _pListM)
                    {
                        XmlDocument x = new XmlDocument();
                        StringBuilder sb = new StringBuilder();
                        List<Resultados> _resultadoCon = new List<Resultados>();
                        Error = false;
                        string _Aduana = null;
                        _resultado = new List<string>();
                        _resultadoCon = new List<Resultados>();

                        MemoryStream memoryStream = new MemoryStream();
                        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                        {
                            Encoding = new UTF8Encoding(false),
                            ConformanceLevel = ConformanceLevel.Document,
                            Indent = true
                        };

                        XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                        xml.WriteStartDocument();

                        xml.WriteStartElement("MDS4S");
                        xml.WriteStartElement("MDS4");

                        xml.WriteElementString("CAR_REG_YEAR", item.year);
                        xml.WriteElementString("KEY_CUO", item.aduana);
                        xml.WriteElementString("CAR_REG_NBER", item.nmanifiesto);

                        xml.WriteStartElement("MDS6S");

                        xml.WriteStartElement("MDS6");
                        xml.WriteElementString("KEY_BOL_REF", item.nbl.Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&"));

                        xml.WriteStartElement("MDS7S");

                        xml.WriteStartElement("MDS7");
                        xml.WriteElementString("CAR_DIS_DATE", item.f_rpatio);
                        xml.WriteElementString("CAR_CTN_IDENT", item.contenedor);
                        xml.WriteElementString("CARBOL_SHP_MARK78", item.sitio);
                        WriteFullElementString(xml, "CARBOL_SHP_MARK90", item.comentarios.Replace(" ", ""));
                        xml.WriteEndElement();

                        _resultado.Add(item.IdDeta.ToString());
                        Resultados _resultadosCon = new Resultados
                        {
                            IdDeta = item.IdDeta,
                            n_contenedor = item.contenedor
                        };

                        _resultadoCon.Add(_resultadosCon);


                        xml.WriteEndElement();
                        xml.WriteEndElement();

                        xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.WriteEndDocument();

                        xml.Flush();
                        xml.Close();
                        string resultado = null;


                        if (_resultado.Count >= 1)
                        {
                            _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());
                            b_sid = item.b_sidunea;

                            int sadfi_res = Convert.ToInt32(DetaNavieraDAL.ActSADFI_AMP(_pListM, DBComun.Estado.falso, item.contenedor, item.c_llegada, item.IdDeta, item.f_rpatio));

                            if (sadfi_res == 0)
                            {
                                using (StreamWriter tw = new StreamWriter(Archivo, true))
                                {
                                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION SADFI IdDeta #: " + item.IdDeta + " # Contenedor: " + item.contenedor);
                                }
                            }
                            else
                            {
                                using (StreamWriter tw = new StreamWriter(Archivo, true))
                                {
                                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION INCORRECTA SADFI IdDeta #: " + item.IdDeta + " # Contenedor: " + item.contenedor);
                                }
                            }

                            string url = ConfigurationManager.AppSettings["urlCOARRI"] + item.IdDeta.ToString();
                            string respuesta = GetHttp(url);

                            int pValor = 0;

                            if (respuesta.Contains("EDI COARRI"))
                                pValor = 1;
                            else
                                pValor = 0;


                            if (pValor == 1)
                            {
                                string actCOARRI = TrasnferXMLDAL.ActCOARRI(pValor, Convert.ToInt32(item.IdDeta));

                                if (Convert.ToInt32(actCOARRI) == 1)
                                {
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION COARRI IdDeta #: " + item.IdDeta + " # Contenedor: " + item.contenedor);
                                    }
                                }
                                else
                                {
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION COARRI INCORRECTA IdDeta #: " + item.IdDeta + " # Contenedor: " + item.contenedor + "BD NO ACTUALIZADA");
                                    }
                                }
                            }
                            else
                            {
                                using (StreamWriter tw = new StreamWriter(Archivo, true))
                                {
                                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION COARRI INCORRECTA IdDeta #: " + item.IdDeta + " # Contenedor: " + item.contenedor + "LOG ERROR: " + respuesta);
                                }
                            }
                           
                           
                         

                            if (item.b_sidunea == 0)
                            {
                                CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();
                                resultado = _proxy.updateCepaData(_Aduana);
                            }
                            else
                            {
                                CepaSiduneaWorld.WSManifiestoCEPAClient _proxySidunea = new CepaSiduneaWorld.WSManifiestoCEPAClient();

                                string _usuario = ConfigurationManager.AppSettings["userSidunea"];
                                string _pass = ConfigurationManager.AppSettings["pswSidunea"];

                                _proxySidunea.ClientCredentials.UserName.UserName = _usuario;
                                _proxySidunea.ClientCredentials.UserName.Password = _pass;

                                resultado = _proxySidunea.updateCepaData(_Aduana);
                            }

                            //string resultado = "1| Mensaje";


                            if (resultado.Substring(0, 1) == "1")
                            {
                                _resultado.Add(resultado);

                            }
                            else
                            {
                                Error = true;
                                _resultado.Add(resultado + ' ' + _Aduana);
                            }
                        }


                        if (Error == false)
                        {
                            if (_resultado == null)
                            {
                                _resultado = new List<string>();
                            }

                            if (_resultadoCon == null)
                            {
                                _resultadoCon = new List<Resultados>();
                            }

                            if (_resultado.Count > 0)
                            {
                                foreach (string itemC in _resultado)
                                {
                                    if (resultado.Substring(0, 1) == "1" && ArchivoBookingDAL.isNumeric(itemC) == true)
                                    {

                                        string act = TrasnferXMLDAL.ActTrasmision(Convert.ToInt32(itemC));


                                        var n_Container1 = from a in _resultadoCon
                                                           where a.IdDeta == Convert.ToInt32(itemC)
                                                           select a.n_contenedor;


                                        using (StreamWriter tw = new StreamWriter(Archivo, true))
                                        {
                                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION IdDeta #: " + itemC + " # Contenedor: " + n_Container1.FirstOrDefault());
                                        }
                                    }
                                    else if (resultado.Substring(0, 1) == "0" && ArchivoBookingDAL.isNumeric(itemC) == true)
                                    {
                                        var n_Container1 = from a in _resultadoCon
                                                           where a.IdDeta == Convert.ToInt32(itemC)
                                                           select a.n_contenedor;


                                        using (StreamWriter tw = new StreamWriter(Archivo, true))
                                        {
                                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION NO EFECTUADA IdDeta #: " + itemC + " # Contenedor: " + n_Container1.FirstOrDefault());

                                        }

                                        EnvioServicio("MODULO TRANSMISION RECEPCION:<br/><br/>Detalle Error : <br/>Bloqueada transmisión Contenedor #" + itemC, "SERVICIO DE ADUANA BLOQUEO TRANSMISION");
                                    }
                                    else
                                    {
                                        if (itemC.Substring(0, 2) == "1|" || itemC.Substring(0, 2) == "0|")
                                        {

                                            using (StreamWriter tw = new StreamWriter(Archivo, true))
                                            {
                                                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": RESPUESTA DE LA DGA A LA TRANSMISION DE CEPA: " + itemC);
                                            }


                                        }

                                    }
                                }
                            }
                            else
                            {
                                using (StreamWriter tw = new StreamWriter(Archivo, true))
                                {
                                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ENVIADO (RECEPCION DE CONTENEDORES): SERVICIO EJECUTADO ");
                                }


                            }
                        }
                        else
                        {
                            if (_resultado.Count > 0)
                            {
                                foreach (string itemC in _resultado)
                                {
                                    if (resultado.Substring(0, 1) == "1" && ArchivoBookingDAL.isNumeric(itemC) == true)
                                    {

                                        string act = TrasnferXMLDAL.ActTrasmision(Convert.ToInt32(itemC));

                                        var n_Container1 = from a in _resultadoCon
                                                           where a.IdDeta == Convert.ToInt32(itemC)
                                                           select a.n_contenedor;


                                        using (StreamWriter tw = new StreamWriter(Archivo, true))
                                        {
                                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION IdDeta #: " + itemC + " # Contenedor: " + n_Container1.FirstOrDefault());
                                        }
                                    }
                                    else if (resultado.Substring(0, 1) == "0" && ArchivoBookingDAL.isNumeric(itemC) == true)
                                    {
                                        var n_Container1 = from a in _resultadoCon
                                                           where a.IdDeta == Convert.ToInt32(itemC)
                                                           select a.n_contenedor;


                                        using (StreamWriter tw = new StreamWriter(Archivo, true))
                                        {
                                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ACTUALIZACION NO EFECTUADA IdDeta #: " + itemC + " # Contenedor: " + n_Container1.FirstOrDefault());
                                        }
                                        //EnvioServicio("MODULO TRANSMISION RECEPCION:<br/><br/>Detalle Error : <br/>Bloqueada transmisión Contenedor #" + itemC, "SERVICIO DE ADUANA BLOQUEO TRANSMISION");
                                    }
                                    else
                                    {
                                        if (itemC.Substring(0, 2) == "1|" || itemC.Substring(0, 2) == "0|")
                                        {

                                            using (StreamWriter tw = new StreamWriter(Archivo, true))
                                            {
                                                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": RESPUESTA DE LA DGA A LA TRANSMISION DE CEPA: " + itemC);
                                            }
                                        }

                                    }
                                }
                            }
                            else
                            {
                                using (StreamWriter tw = new StreamWriter(Archivo, true))
                                {
                                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ENVIADO (RECEPCION DE CONTENEDORES): SERVICIO EJECUTADO ");
                                }
                            }

                            Error = false;
                            b_sid = 0;
                            _resultado = new List<string>();
                        }


                    }
                    return _resultado;
                }

                return _resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string GetHttp(string url)
        {
            try
            {
                WebRequest oRequest = WebRequest.Create(url);
                WebResponse oResponse = oRequest.GetResponse();

                StreamReader sr = new StreamReader(oResponse.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        private void RetencionDAN()
        {
            List<TransferXML> retenidos = new List<TransferXML>();

            List<string> pTipos = new List<string>();

            pTipos.Add("DAN");
            pTipos.Add("UCC");

            if (pTipos.Count > 0)
            {
                foreach (var itemTipo in pTipos)
                {

                    string resultado = null;

                    try
                    {
                        if (itemTipo == "DAN")
                        {
                            retenidos = TransmiDANDAL.TransDANR(DBComun.Estado.falso, "b_transdanr = 0 AND f_reg_dan IS NOT NULL", "f_reg_dan");
                        }
                        else
                        {
                            retenidos = TransmiDANDAL.TransDANR(DBComun.Estado.falso, "b_transdanr = 0 AND f_retencion_ucc IS NOT NULL", "f_retencion_ucc");
                        }

                        if (retenidos.Count > 0)
                        {
                            foreach (var item in retenidos)
                            {

                                MemoryStream memoryStream = new MemoryStream();
                                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                                {
                                    Encoding = new UTF8Encoding(false),
                                    ConformanceLevel = ConformanceLevel.Document,
                                    Indent = true
                                };

                                XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);


                                string _Aduana = null;

                                xml.WriteStartDocument();

                                xml.WriteStartElement("MDS4");

                                xml.WriteElementString("CAR_REG_YEAR", item.year);
                                xml.WriteElementString("KEY_CUO", item.aduana);
                                xml.WriteElementString("CAR_REG_NBER", item.nmanifiesto);
                                xml.WriteElementString("CAR_CTN_IDENT", item.contenedor);
                                xml.WriteElementString("ACC_DATE", item.f_retencion);
                                xml.WriteElementString("AREA", itemTipo);
                                xml.WriteElementString("ACCION", "0");

                                xml.WriteEndDocument();
                                xml.Flush();
                                xml.Close();

                                _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                                if (item.b_sidunea == 0)
                                {
                                    //CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();
                                    //resultado = _proxy.insertContenedorDAN(_Aduana);
                                    resultado = "0| Esperando respuesa de DGA por problemas en método de recepción DAN";
                                }
                                else
                                {
                                    CepaSiduneaWorld.WSManifiestoCEPAClient _proxy = new CepaSiduneaWorld.WSManifiestoCEPAClient();

                                    string _usuario = ConfigurationManager.AppSettings["userSidunea"];
                                    string _pass = ConfigurationManager.AppSettings["pswSidunea"];

                                    _proxy.ClientCredentials.UserName.UserName = _usuario;
                                    _proxy.ClientCredentials.UserName.Password = _pass;

                                    resultado = _proxy.insertContenedorDAN(_Aduana);
                                }


                                if (resultado.Substring(0, 1) == "1")
                                {
                                    string act = TransmiDANDAL.ActTrasmision(item.IdDeta, "b_transdanr = 1, f_transdanr = GETDATE()");
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION RETENIDO " + itemTipo + " Contenedor #: " + item.contenedor + " - " + item.IdDeta);
                                    }

                                }
                                else
                                {
                                    string act = null;
                                    if (item.b_sidunea == 0)
                                    {
                                        act = TransmiDANDAL.ActTrasmision(item.IdDeta, "b_transdanr = 1, f_transdanr = NULL");
                                    }

                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION RETENIDO " + itemTipo + " PRESENTO PROBLEMAS: " + item.contenedor + " : ERROR: " + resultado.Replace("0|", ""));
                                    }

                                }
                            }
                        }
                        else
                        {
                            using (StreamWriter tw = new StreamWriter(Archivo, true))
                            {
                                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO HAY CONTENEDORES RETENIDOS " + itemTipo + " PENDIENTES ");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

        }


        private void LiberacionDAN()
        {
            List<TransferXML> liberados = new List<TransferXML>();

            List<string> pTipos = new List<string>();

            pTipos.Add("DAN");
            pTipos.Add("UCC");

            if (pTipos.Count > 0)
            {
                foreach (var itemTipo in pTipos)
                {
                    string resultado = null;

                    try
                    {
                        if (itemTipo == "DAN")
                        {
                            liberados = TransmiDANDAL.TransDANR(DBComun.Estado.falso, "b_transdanl = 0 AND f_reg_liberacion IS NOT NULL", "f_reg_liberacion");
                        }
                        else
                        {
                            liberados = TransmiDANDAL.TransDANR(DBComun.Estado.falso, "b_transdanl = 0 AND f_lib_ucc IS NOT NULL", "f_lib_ucc");
                        }

                        if (liberados.Count > 0)
                        {

                            foreach (var item in liberados)
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                                {
                                    Encoding = new UTF8Encoding(false),
                                    ConformanceLevel = ConformanceLevel.Document,
                                    Indent = true
                                };

                                XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                                string _Aduana = null;

                                xml.WriteStartDocument();

                                xml.WriteStartElement("MDS4");

                                xml.WriteElementString("CAR_REG_YEAR", item.year);
                                xml.WriteElementString("KEY_CUO", item.aduana);
                                xml.WriteElementString("CAR_REG_NBER", item.nmanifiesto);
                                xml.WriteElementString("CAR_CTN_IDENT", item.contenedor);
                                xml.WriteElementString("ACC_DATE", item.f_retencion);
                                xml.WriteElementString("AREA", itemTipo);
                                xml.WriteElementString("ACCION", "1");

                                xml.WriteEndDocument();
                                xml.Flush();
                                xml.Close();

                                _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                                if (item.b_sidunea == 0)
                                {
                                    //CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();
                                    //resultado = _proxy.insertContenedorDAN(_Aduana);

                                    resultado = "0| Esperando respuesa de DGA por problemas en método de liberación DAN ";
                                }
                                else
                                {
                                    CepaSiduneaWorld.WSManifiestoCEPAClient _proxy = new CepaSiduneaWorld.WSManifiestoCEPAClient();

                                    string _usuario = ConfigurationManager.AppSettings["userSidunea"];
                                    string _pass = ConfigurationManager.AppSettings["pswSidunea"];

                                    _proxy.ClientCredentials.UserName.UserName = _usuario;
                                    _proxy.ClientCredentials.UserName.Password = _pass;

                                    resultado = _proxy.insertContenedorDAN(_Aduana);
                                }

                                if (resultado.Substring(0, 1) == "1")
                                {
                                    string act = TransmiDANDAL.ActTrasmision(item.IdDeta, "b_transdanl = 1, f_transdanl = GETDATE()");
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION LIBERADO " + itemTipo + " Contenedor #: " + item.contenedor + " - " + item.IdDeta);
                                    }

                                }
                                else
                                {
                                    string act = TransmiDANDAL.ActTrasmision(item.IdDeta, "b_transdanl = 1, f_transdanl = NULL");
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION LIBERADO " + itemTipo + " PRESENTO PROBLEMAS: " + item.contenedor + " : ERROR: " + resultado.Replace("0|", ""));
                                    }

                                }
                            }
                        }
                        else
                        {
                            using (StreamWriter tw = new StreamWriter(Archivo, true))
                            {
                                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO HAY CONTENEDORES LIBERADOS " + itemTipo + " PENDIENTES ");
                            }

                        }




                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

        }

        private void CorteCOTECNA()
        {
            List<CorteCOTECNA> pCortePendiente = new List<CorteCOTECNA>();


            try
            {
                pCortePendiente = CorteCOTECNADAL.CodLlegadasGroup(DBComun.Estado.falso);

                if (pCortePendiente.Count > 0)
                {
                    //List<CorteCOTECNA> pGrupo = pCortePendiente.GroupBy(p => p.c_llegada).Select(g => g.First()).ToList();

                    foreach (CorteCOTECNA item in pCortePendiente)
                    {
                        string barco = null;
                        string Html = "";
                        List<CorteCOTECNA> pBuquesCorte = new List<CorteCOTECNA>();
                        List<CorteCOTECNA> pCorteConte = new List<CorteCOTECNA>();

                        pCortePendiente = CorteCOTECNADAL.CorteLlegadas(DBComun.Estado.falso, item.c_llegada);


                        pBuquesCorte = CorteCOTECNADAL.BuquesCorte(DBComun.Estado.falso, item.c_llegada);


                        if (pBuquesCorte.Count > 0)
                        {

                            string f_atraque = null;


                            List<CorteCOTECNA> query = (from a in pCortePendiente
                                                        join b in pBuquesCorte on new { c_llegada = a.c_llegada, c_cliente = a.c_cliente } equals new { c_llegada = b.c_llegada, c_cliente = b.c_cliente }
                                                        where a.c_llegada == item.c_llegada && b.f_atraque != null
                                                        select new CorteCOTECNA
                                                        {
                                                            c_nul = b.c_nul,
                                                            c_llegada = a.c_llegada,
                                                            c_cliente = a.c_cliente,
                                                            d_buque = b.d_buque,
                                                            d_cliente = b.d_cliente + " - " + a.c_prefijo,
                                                            n_manifiesto = a.n_manifiesto,
                                                            t_contenedores = a.t_contenedores,
                                                            t_dan = a.t_dan,
                                                            t_dga = a.t_dga
                                                        }).ToList();

                            if (query == null)
                            {
                                query = new List<CorteCOTECNA>();
                            }

                            if (query.Count > 0)
                            {
                                int c_total = 0, c_dan = 0, c_dga = 0;
                                string cb_llegada = null, cb_nul = null;

                                EnvioCorreo _correo = new EnvioCorreo();

                                foreach (var bitem in pBuquesCorte)
                                {
                                    barco = bitem.d_buque;
                                    f_atraque = bitem.f_atraque.ToString("dd/MM/yyyy HH:mm");
                                    cb_llegada = bitem.c_llegada;
                                    cb_nul = bitem.c_nul;
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
                                Html += "<td width=\"10%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>MANIFIESTO</font></th>";
                                Html += "<td width=\"40%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>NAVIERA</font></th>";
                                Html += "<td width=\"15%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TOTAL CONTENEDORES</font></th>";
                                Html += "<td width=\"15%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TOTAL R. DAN</font></th>";
                                Html += "<td width=\"15%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TOTAL S. DGA</font></th>";
                                Html += "</center>";
                                Html += "</tr>";

                                foreach (var ritem in query)
                                {
                                    Html += "<center>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.n_manifiesto + "</font></td>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.d_cliente + "</font></td>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.t_contenedores.ToString() + "</font></td>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.t_dan.ToString() + "</font></td>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.t_dga.ToString() + "</font></td>";
                                    Html += "</center>";
                                    Html += "</tr>";
                                    c_total = c_total + ritem.t_contenedores;
                                    c_dan = c_dan + ritem.t_dan;
                                    c_dga = c_dga + ritem.t_dga;
                                }

                                Html += "<tr>";
                                Html += "<center>";
                                Html += "<td colspan=\"2\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font size=2 color=white>TOTAL</font></td>";
                                Html += "<td height=\"25\" bgcolor=#1584CE><font size=2 color=white>" + c_total.ToString() + "</font></td>";
                                Html += "<td height=\"25\" bgcolor=#1584CE><font size=2 color=white>" + c_dan.ToString() + "</font></td>";
                                Html += "<td height=\"25\" bgcolor=#1584CE><font size=2 color=white>" + c_dga.ToString() + "</font></td>";
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

                                string act = CorteCOTECNADAL.ActCOTECNA(cb_llegada, cb_nul);

                                if (act != null)
                                {
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION CORTE COTECNA: " + item.c_llegada + " - " + barco.ToString());
                                    }
                                }


                                _correo.ListaNoti = NotificacionesDAL.ObtenerNotiCOTECNA(DBComun.Estado.falso);
                                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_cotecna", DBComun.Estado.falso, "0");
                                _correo.ListaCC = _listaCC;

                                _correo.Asunto = Html;
                                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso, "servicio");

                                EnvioErrorCorreo(_correo.Subject, "MODULO COTECNA :");

                                string actNoti = CorteCOTECNADAL.ActNotiCOTECNA(item.c_llegada);

                                if (actNoti != null)
                                {
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION CORTE COTECNA: SE NOTIFICO SATISFACTORIAMENTE EL " + item.c_llegada + " - " + barco.ToString());
                                    }
                                }

                            }




                        }




                    }
                }
                else
                {


                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + " COTECNA : NO HAY BUQUES PENDIENTES DE ALERTAR");
                    }

                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        private void TransmisionAuto()
        {
            List<DetaNaviera> pCortePendiente = new List<DetaNaviera>();


            try
            {
                pCortePendiente = CorteCOTECNADAL.CodLlegadasGroupAuto(DBComun.Estado.falso);

                if (pCortePendiente.Count > 0)
                {
                    //List<CorteCOTECNA> pGrupo = pCortePendiente.GroupBy(p => p.c_llegada).Select(g => g.First()).ToList();

                    foreach (DetaNaviera item in pCortePendiente)
                    {
                        string barco = null;
                        string Html = "";
                        List<CorteCOTECNA> pBuquesCorte = new List<CorteCOTECNA>();
                        List<CorteCOTECNA> pCorteConte = new List<CorteCOTECNA>();

                        //pCortePendiente = CorteCOTECNADAL.CorteLlegadas(DBComun.Estado.falso, item.c_llegada);


                        pBuquesCorte = CorteCOTECNADAL.BuquesCorte(DBComun.Estado.falso, item.c_llegada);


                        if (pBuquesCorte.Count > 0)
                        {

                            string f_atraque = null;


                            string cb_llegada = null, cb_nul = null;
                            DateTime _fecha;



                            EnvioCorreo _correo = new EnvioCorreo();

                            foreach (var bitem in pBuquesCorte)
                            {
                                barco = bitem.d_buque;
                                f_atraque = bitem.f_atraque.ToString("dd/MM/yyyy HH:mm");
                                cb_llegada = bitem.c_llegada;
                                cb_nul = bitem.c_nul;
                                break;
                            }

                            List<DetaNaviera> pLstAuto = new List<DetaNaviera>();

                            pLstAuto = DocBuqueLINQ.ObtenerTransmiConsulAutoSrv(cb_llegada);

                            Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                            Html += "<b><u> TRANSMISIÓN AUTOMÁTICA  </b></u><br />";
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



                            Html += "MÓDULO : RECEPCION DE CONTENEDORES EN PUERTO Y TRANSMISION AUTOMATICA A LA DGA  <br />";
                            Html += "TIPO DE MENSAJE : RECEPCION DE CONTENEDORES EN PUERTO Y TRANSMISION AUTOMATICA A LA DGA <br /><br />";
                            Html += string.Format("El presente listado de contenedores correspondientes al buque {0},  han sido recepcionado en puerto y transmitidos automáticamente a la DGA.-", barco);
                            Html += "<br /><br/>";


                            Html += "Los siguientes contenedores que se detallan a continuación fueron recepcionado en puerto y transmitidos automáticamente a la DGA: ";
                            Html += "<br /><br/>";

                            if (pLstAuto == null)
                            {
                                pLstAuto = new List<DetaNaviera>();
                            }

                            if (pLstAuto.Count > 0)
                            {
                                Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                                Html += "<tr>";
                                Html += "<center>";
                                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>AGENCIA</font></th>";
                                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>MANIFIESTO</font></th>";
                                Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>PESO</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>ESTADO</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CLIENTE</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>DESPACHO</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>MANEJO</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TRANSFERENCIA</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. RECEPCION</font></th>";
                                Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. TRANSMISION AUTO</font></th>";
                                Html += "</center>";
                                Html += "</tr>";

                                foreach (DetaNaviera itemt in pLstAuto)
                                {
                                    Html += "<tr>";
                                    Html += "<center>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.c_cliente + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.c_manifiesto + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.n_contenedor + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.c_tamaño + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + string.Format("{0:0.00}", itemt.v_peso) + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.b_estado + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.s_consignatario + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.b_despacho + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.b_manejo + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.b_transferencia + "</font></td>";
                                    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemt.f_recep + "</font></td>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + itemt.f_tramite_s + "</font></td>";
                                    Html += "<center>";
                                    Html += "</tr>";
                                    Html += "</font>";
                                }
                                Html += "</table><br /><br/>";


                                _correo.Subject = string.Format("CEPA : Recepción de contenedores en Puerto y transmisión automática a la DGA del buque {0}", barco);
                                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_trans_auto", DBComun.Estado.falso, "0");
                                //List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_detenido", DBComun.Estado.verdadero, c_naviera);

                                //if (_listaCC == null)
                                //{
                                //    _listaCC = new List<Notificaciones>();
                                //}

                                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_detenido", DBComun.Estado.verdadero, c_cliente));
                                //_correo.ListaCC = _listaCC;

                                //Notificaciones noti = new Notificaciones
                                //{
                                //    sMail = "elsa.sosa@cepa.gob.sv",
                                //    dMail = "Elsa Sosa"
                                //};

                                //List<Notificaciones> pLisN = new List<Notificaciones>();

                                //pLisN.Add(noti);

                                //_correo.ListaNoti = pLisN;

                                _correo.Asunto = Html;


                                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso, "servicio");

                                EnvioErrorCorreo(_correo.Subject, "MODULO RECEPCION AUTO:");

                                string actNoti = CorteCOTECNADAL.ActNotiAUTO(item.c_llegada, DBComun.Estado.falso);

                                if (actNoti != null)
                                {
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION AUTOMATICA A DGA: SE NOTIFICO SATISFACTORIAMENTE EL " + item.c_llegada + " - " + barco.ToString());
                                    }
                                }
                            }
                            else
                            {
                                using (StreamWriter tw = new StreamWriter(Archivo, true))
                                {
                                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + " TRANSMISION AUTOMATICA A DGA : NO HAY CONTENEDORES PENDIENTES DE ALERTAR");
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + " TRANSMISION AUTOMATICA A DGA : NO HAY BUQUES PENDIENTES DE ALERTAR");
                    }

                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void liquidBuque()
        {
            List<EncaLiquid> pLiquidPendiente = new List<EncaLiquid>();


            try
            {
                pLiquidPendiente = LiquidAduanaDAL.CodLlegadasLiquid(DBComun.Estado.falso);

                if (pLiquidPendiente.Count > 0)
                {
                    //List<CorteCOTECNA> pGrupo = pCortePendiente.GroupBy(p => p.c_llegada).Select(g => g.First()).ToList();

                    foreach (EncaLiquid item in pLiquidPendiente)
                    {
                        string Html = "";
                        List<EncaLiquid> pEncaLiquid = new List<EncaLiquid>();
                        List<LiquidADUANA> pResumenLiquid = new List<LiquidADUANA>();

                        List<DetaillLiquid> pDetalleLiquid = new List<DetaillLiquid>();
                        List<EncaLiquid> pEncaResumen = new List<EncaLiquid>();

                        pEncaLiquid = LiquidAduanaDAL.EncaLiquidADUANA(DBComun.Estado.falso, item.c_llegada);


                        if (pEncaLiquid.Count > 0)
                        {
                            pResumenLiquid = LiquidAduanaDAL.LiquidResumen(DBComun.Estado.falso, item.c_llegada);

                            pEncaResumen = LiquidAduanaDAL.EncaLiquidSADFI(DBComun.Estado.falso, item.c_llegada);


                            List<LiquidADUANA> query = (from a in pResumenLiquid
                                                        join b in pEncaResumen on new { c_llegada = a.c_llegada, c_cliente = a.c_cliente } equals new { c_llegada = b.c_llegada, c_cliente = b.c_cliente }
                                                        where a.c_llegada == item.c_llegada && b.f_desatraque != null
                                                        select new LiquidADUANA
                                                        {
                                                            c_llegada = a.c_llegada,
                                                            c_cliente = a.c_cliente,
                                                            d_buque = b.n_buque,
                                                            d_cliente = b.d_cliente + " - " + a.c_prefijo,
                                                            t_manifestados = a.t_manifestados,
                                                            t_recibidos = a.t_recibidos,
                                                            t_cancelados = a.t_cancelados
                                                        }).ToList();

                            if (query == null)
                            {
                                query = new List<LiquidADUANA>();
                            }

                            if (query.Count > 0)
                            {
                                int c_manifestados = 0, c_recibidos = 0, c_cancelados = 0;

                                EnvioCorreo _correo = new EnvioCorreo();
                                EncaLiquid _encaItem = new EncaLiquid();

                                foreach (var bitem in pEncaLiquid)
                                {
                                    _encaItem.n_buque = bitem.n_buque;
                                    _encaItem.s_atraque = bitem.f_atraque.ToString("dd/MM/yyyy HH:mm");
                                    _encaItem.s_desatraque = bitem.f_desatraque.ToString("dd/MM/yyyy HH:mm");
                                    _encaItem.c_llegada = bitem.c_llegada;
                                    _encaItem.c_null = bitem.c_null;
                                    _encaItem.c_imo = bitem.c_imo;
                                    break;
                                }

                                Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";

                                Html += "MÓDULO : LISTADO DE CONTENEDORES DE IMPORTACION DESCARGADOS POR BUQUE <br />";
                                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES DE IMPORTACION DESCARGADOS POR BUQUE <br /><br />";

                                Html += string.Format("Estimados, se presenta a continuacion el informe de contenedores de importación descargados por manifiesto y naviera, procedentes del barco <b>{0}</b>:", _encaItem.n_buque);
                                Html += "<br/><br/>";

                                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                                Html += "<tr>";

                                Html += "<td style=\"text-align: left;\"><font size=2>Nombre del Buque</font></td>";
                                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _encaItem.n_buque + "</font></td>";
                                Html += "</tr>";
                                Html += "<tr>";
                                Html += "<td style=\"text-align: left;\" ><font size = 2>IMO del Buque</font></td>";
                                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + _encaItem.c_imo + "</font></td>";
                                Html += "</tr>";
                                Html += "<tr>";
                                Html += "<td style=\"text-align: left;\" ><font size = 2>NUL</font></td>";
                                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + _encaItem.c_null + "</font></td>";
                                Html += "</tr>";
                                Html += "<tr>";
                                Html += "<td style=\"text-align: left;\" ><font size = 2>Fecha de Atraque</font></td>";
                                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + _encaItem.s_atraque + "</font></td>";
                                Html += "</tr>";
                                Html += "<tr>";
                                Html += "<td style=\"text-align: left;\" ><font size = 2>Fecha de Desatraque</font></td>";
                                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + _encaItem.s_desatraque + "</font></td>";
                                Html += "</tr>";
                                Html += "</table>";
                                Html += "<br />";


                                Html += "<b><u>RESUMEN</b></u><br />";
                                Html += "<br />";

                                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                                Html += "<tr>";
                                Html += "<center>";
                                Html += "<td width=\"45%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>NAVIERA</font></th>";
                                Html += "<td width=\"20%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>MANIFESTADOS</font></th>";
                                Html += "<td width=\"15%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>RECIBIDOS</font></th>";
                                Html += "<td width=\"15%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>NO RECIBIDOS</font></th>";
                                Html += "</center>";
                                Html += "</tr>";

                                foreach (var ritem in query)
                                {
                                    Html += "<center>";
                                    //Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.n_manifiesto + "</font></td>";
                                    Html += "<td align=\"left\" height=\"25\"><font size=2 color=blue>" + ritem.d_cliente + "</font></td>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.t_manifestados.ToString() + "</font></td>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.t_recibidos.ToString() + "</font></td>";
                                    Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.t_cancelados.ToString() + "</font></td>";
                                    Html += "</center>";
                                    Html += "</tr>";
                                    c_manifestados = c_manifestados + ritem.t_manifestados;
                                    c_recibidos = c_recibidos + ritem.t_recibidos;
                                    c_cancelados = c_cancelados + ritem.t_cancelados;
                                }

                                Html += "<tr>";
                                Html += "<center>";
                                Html += "<td height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font size=2 color=white>TOTAL</font></td>";
                                Html += "<td height=\"25\" bgcolor=#1584CE><font size=2 color=white>" + c_manifestados.ToString() + "</font></td>";
                                Html += "<td height=\"25\" bgcolor=#1584CE><font size=2 color=white>" + c_recibidos.ToString() + "</font></td>";
                                Html += "<td height=\"25\" bgcolor=#1584CE><font size=2 color=white>" + c_cancelados.ToString() + "</font></td>";
                                Html += "</center>";
                                Html += "</tr>";
                                Html += "</font></table>";
                                Html += "<br/><br/>";

                                pDetalleLiquid = LiquidAduanaDAL.LiquidDetalle(DBComun.Estado.falso, item.c_llegada);

                                if (pDetalleLiquid.Count > 0)
                                {
                                    List<DetaillLiquid> queryDetaills = (from a in pDetalleLiquid
                                                                         join b in pEncaResumen on new { c_llegada = a.c_llegada, c_cliente = a.c_naviera } equals new { c_llegada = b.c_llegada, c_cliente = b.c_cliente }
                                                                         where a.c_llegada == item.c_llegada && b.f_desatraque != null
                                                                         select new DetaillLiquid
                                                                         {
                                                                             c_correlativo = a.c_correlativo,
                                                                             a_manifiesto = a.a_manifiesto,
                                                                             n_manifiesto = a.n_manifiesto,
                                                                             d_cliente = b.d_cliente + " - " + a.d_cliente,
                                                                             n_contenedor = a.n_contenedor,
                                                                             c_naviera = a.c_naviera,
                                                                             c_llegada = a.c_llegada
                                                                         }).ToList();

                                    if (queryDetaills == null)
                                    {
                                        queryDetaills = new List<DetaillLiquid>();
                                    }

                                    if (queryDetaills.Count > 0)
                                    {
                                        Html += "Detalle de contenedores no recibidos por Naviera:";
                                        Html += "<br/><br/>";

                                        Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                                        Html += "<tr>";
                                        Html += "<center>";
                                        Html += "<td width=\"7%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>#</font></th>";
                                        Html += "<td width=\"10%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>AÑO MANIFIESTO</font></th>";
                                        Html += "<td width=\"10%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2># DE MANIFIESTO</font></th>";
                                        Html += "<td width=\"53%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>NAVIERA</font></th>";
                                        Html += "<td width=\"20%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2># DE CONTENEDOR</font></th>";
                                        Html += "</center>";
                                        Html += "</tr>";

                                        foreach (var rDetaill in queryDetaills)
                                        {
                                            Html += "<center>";
                                            Html += "<td height=\"25\"><font size=2 color=blue>" + rDetaill.c_correlativo + "</font></td>";
                                            Html += "<td height=\"25\"><font size=2 color=blue>" + rDetaill.a_manifiesto + "</font></td>";
                                            Html += "<td height=\"25\"><font size=2 color=blue>" + rDetaill.n_manifiesto + "</font></td>";
                                            Html += "<td align=\"left\" height=\"25\"><font size=2 color=blue>" + rDetaill.d_cliente + "</font></td>";
                                            Html += "<td height=\"25\"><font size=2 color=blue>" + rDetaill.n_contenedor + "</font></td>";
                                            Html += "</center>";
                                            Html += "</tr>";

                                        }
                                        Html += "</font></table>";
                                        Html += "<br/><br/>";
                                    }
                                }



                                _correo.Subject = string.Format("ADUANA: RESUMEN DE CONTENEDORES DE IMPORTACION DESCARGADOS PARA EL BUQUE {0} - NUL # {1} CON FECHA DE DESATRAQUE {2}", _encaItem.n_buque, _encaItem.c_null, _encaItem.s_desatraque);

                                //Notificaciones _notiCC = new Notificaciones
                                //{
                                //    IdNotificacion = -1,
                                //    sMail = "elsa.sosa@cepa.gob.sv",
                                //    dMail = "Elsa Sosa"
                                //};

                                //List<Notificaciones> _ccList = new List<Notificaciones>();
                                //_ccList.Add(_notiCC);

                                //_correo.ListaNoti = _ccList;

                                _correo.ListaNoti = NotificacionesDAL.ObtenerNotiLIQUID(DBComun.Estado.falso);
                                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_liquid", DBComun.Estado.falso, "0");
                                _correo.ListaCC = _listaCC;

                                _correo.Asunto = Html;
                                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso, "servicio");

                                EnvioErrorCorreo(_correo.Subject, "MODULO ADUANA :");

                                string act = CorteCOTECNADAL.ActLIQUID(item.c_llegada);

                                if (act != null)
                                {
                                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                                    {
                                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION RESUMEN DESCARGA: " + item.c_llegada + " - " + _encaItem.n_buque);
                                    }
                                }
                            }




                        }




                    }
                }
                else
                {


                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + "COTECNA : NO HAY BUQUES PENDIENTES DE ALERTAR");
                    }

                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        private void AlertaDAN()
        {

            string Html = null;

            List<ParametroAlerta> pParametroAlerta = new List<ParametroAlerta>();

            try
            {
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
                                        if (iTarja.c_tarja != iAlerta.c_tarja)
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
                            {
                                using (StreamWriter tw = new StreamWriter(Archivo, true))
                                {
                                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO EXISTE ALERTA PENDIENTE DAN PARAMETRO " + iParam.c_parametro);
                                }

                            }
                        }
                        else
                        {
                            if (pAlertaTarja.Count > 0)
                            {
                                pValidas = pAlertaTarja;
                            }
                            else
                            {
                                using (StreamWriter tw = new StreamWriter(Archivo, true))
                                {
                                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO EXISTE ALERTA PENDIENTE DAN ");
                                }

                            }
                        }
                    }

                    if (pValidas.Count > 0)
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
                        _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso, "servicio");
                        EnvioErrorCorreo(_correo.Subject, "MODULO DAN :");

                        using (StreamWriter tw = new StreamWriter(Archivo, true))
                        {
                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": TRANSMISION DAN ALERTA ENVIADA ");
                        }



                    }
                    else
                    {
                        using (StreamWriter tw = new StreamWriter(Archivo, true))
                        {
                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": NO EXISTE ALERTA PENDIENTE DAN");
                        }
                        //Mensaje de no hay validaciones

                    }

                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void AlertaOIRSA()
        {
            List<InfOperaciones> pCortePendiente = new List<InfOperaciones>();


            try
            {
                pCortePendiente = ValidaTarjaDAL.getAlertaRecepcion(DBComun.Estado.falso);


                string Html = "";

                List<int> pValores = new List<int>();



                if (pCortePendiente.Count > 0)
                {

                    EnvioCorreo _correo = new EnvioCorreo();


                    Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";

                    Html += "MÓDULO : LISTADO DE CONTENEDORES DE IMPORTACION RECEPCIONADOS <br />";
                    Html += "TIPO DE MENSAJE : LISTADO DE CONTENEDORES DE IMPORTACION RECEPCIONADOS <br /><br />";

                    Html += string.Format("Estimados, se presenta a continuacion el informe de contenedores de importación recepcionados en patio, que no poseen fecha de recepción por Arco de OIRSA: ");
                    Html += "<br/><br/>";

                    Html += "<b><u>RESUMEN</b></u><br />";
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"15%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>NAVIERA</font></th>";
                    Html += "<td width=\"10%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                    Html += "<td width=\"20%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TRAFICO</font></th>";
                    Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>FECHA RECEPCION PATIO</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    string _envio = null;
                    foreach (var ritem in pCortePendiente)
                    {
                        Html += "<center>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.c_cliente + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.n_contenedor + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.c_tamaño + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.c_trafico + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + ritem.f_recepcion + "</font></td>";
                        Html += "</center>";
                        Html += "</tr>";
                        pValores.Add(ritem.IdDeta);

                        _envio = CorteCOTECNADAL.ActRecepPatio(ritem.s_comentarios, ritem.IdDeta);

                    }

                    Html += "</font></table>";


                    _correo.Subject = "RECEPCION PATIO: RESUMEN DE CONTENEDORES DE IMPORTACION RECEPCIONADOS";

                    //Notificaciones _notiCC = new Notificaciones
                    //{
                    //    IdNotificacion = -1,
                    //    sMail = "elsa.sosa@cepa.gob.sv",
                    //    dMail = "Elsa Sosa"
                    //};

                    //List<Notificaciones> _ccList = new List<Notificaciones>();
                    //_ccList.Add(_notiCC);

                    //_correo.ListaNoti = _ccList;


                    List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCCN("b_not_alertrecep", DBComun.Estado.falso, "0");
                    _correo.ListaNoti = _listaCC;

                    _correo.Asunto = Html;
                    _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso, "servicio");

                    EnvioErrorCorreo(_correo.Subject, "MODULO RECEPCION :");

                    string act = null;
                    foreach (int pV in pValores)
                    {
                        act = CorteCOTECNADAL.ActRecep(pV);
                    }


                    if (act != null)
                    {
                        using (StreamWriter tw = new StreamWriter(Archivo, true))
                        {
                            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": RECEPCION CONTENEDORES PATIO PRESENTO CONTENEDORES PENDIENTES ARCO OIRSA ");
                        }
                    }

                }
                else
                {
                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": RECEPCION CONTENEDORES PATIO NO TIENE CONTENEDORES PENDIENTES");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using System.Xml;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.Data;

namespace CEPA.CCO.Srv.Test
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://190.86.214.193:6046/", Description = "Servicio de Transferencia de Informacion ADUANA - CEPA")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class wsCEPA : System.Web.Services.WebService
    {


        [WebMethod(Description = "Metodo para registrar los estados de las declaraciones transmitido por ADUANA")]
        public string InsertRegAduana(string xmlDoc)
        {
            string _respuesta = null;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlDoc);

            XmlNodeList listaCntres = doc.SelectNodes("CntsEstados/DECLA");

            XmlNode unContenedor;

            if (listaCntres.Count > 0)
            {
                for (int i = 0; i < listaCntres.Count; i++)
                {
                    unContenedor = listaCntres.Item(i);

                    string[] cadenaADUANA = unContenedor.SelectSingleNode("NUM_DOC").InnerText.Split('-');

                    string a_doc = null;
                    string c_doc = null;
                    string n_doc = null;
                    string b_sidunea = null;
                    int b_sidu = 0;
                    int b_transito = 0;




                    if (cadenaADUANA.Count() == 3)
                    {
                        a_doc = cadenaADUANA[0].ToString();
                        c_doc = cadenaADUANA[1].ToString();
                        n_doc = cadenaADUANA[2].ToString();
                    }
                    else if (cadenaADUANA.Count() == 4)
                    {
                        a_doc = cadenaADUANA[0].ToString();
                        c_doc = cadenaADUANA[1].ToString();
                        n_doc = cadenaADUANA[2].ToString();
                        b_sidunea = cadenaADUANA[3].ToString();
                    }

                    if (n_doc == "T")
                    {
                        b_transito = 1;
                        b_sidu = 1;
                    }

                    if (b_sidunea == "W")
                        b_sidu = 1;



                    string _fecha = unContenedor.SelectSingleNode("FEC_REG_ESTADO").InnerText.ToString();
                    string _resultP = null;

                    if (b_sidu == 0)
                    {
                        EstadosDecla estadoDe = new EstadosDecla
                        {
                            IdRegAduana = -1,
                            c_aduana = unContenedor.SelectSingleNode("KEY_ADUANA").InnerText,
                            n_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("NUM_MANI").InnerText),
                            a_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("YEAR_MANI").InnerText),
                            n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper(),
                            a_decla = Convert.ToInt32(a_doc),
                            s_decla = Convert.ToInt32(c_doc),
                            c_decla = Convert.ToInt32(n_doc),
                            IdEstado = Convert.ToInt32(unContenedor.SelectSingleNode("ESTADO_ADUANA").InnerText),
                            f_reg_aduana = Convert.ToDateTime(unContenedor.SelectSingleNode("FEC_REG_ESTADO").InnerText),
                            IdSelectividad = Convert.ToInt32(unContenedor.SelectSingleNode("SELECTIVIDAD").InnerText),
                            //n_nit = unContenedor.SelectSingleNode("N_NIT").InnerText.Replace("-", "").ToUpper(),
                            b_siduneawd = b_sidu,
                            n_nit = "NULL",
                            s_consignatario = "NULL",
                            s_descripcion = "NULL",
                            n_BL = "NULL"
                        };

                        _resultP = EstadosDeclaDAL.InsertarTest(estadoDe);
                    }
                    else
                    {
                        if (b_transito == 0)
                        {
                            EstadosDecla estadoDe = new EstadosDecla
                            {
                                IdRegAduana = -1,
                                c_aduana = unContenedor.SelectSingleNode("KEY_ADUANA").InnerText,
                                n_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("NUM_MANI").InnerText),
                                a_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("YEAR_MANI").InnerText),
                                n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper(),
                                a_decla = Convert.ToInt32(a_doc),
                                s_decla = Convert.ToInt32(c_doc),
                                c_decla = Convert.ToInt32(n_doc),
                                IdEstado = Convert.ToInt32(unContenedor.SelectSingleNode("ESTADO_ADUANA").InnerText),
                                f_reg_aduana = Convert.ToDateTime(unContenedor.SelectSingleNode("FEC_REG_ESTADO").InnerText),
                                IdSelectividad = Convert.ToInt32(unContenedor.SelectSingleNode("SELECTIVIDAD").InnerText),
                                n_nit = unContenedor.SelectSingleNode("N_NIT").InnerText.Replace("-", "").ToUpper(),
                                b_siduneawd = b_sidu,
                                s_consignatario = unContenedor.SelectSingleNode("CONSIGNATARIO").InnerText.ToUpper(),
                                n_BL = unContenedor.SelectSingleNode("BL").InnerText.ToUpper(),
                                s_descripcion = unContenedor.SelectSingleNode("DESCRIPCION").InnerText.ToUpper()
                            };

                            _resultP = EstadosDeclaDAL.InsertarTest(estadoDe);
                        }
                        else
                        {
                            EstadosDecla estadoDe = new EstadosDecla
                            {
                                IdRegAduana = -1,
                                c_aduana = unContenedor.SelectSingleNode("KEY_ADUANA").InnerText,
                                n_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("NUM_MANI").InnerText),
                                a_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("YEAR_MANI").InnerText),
                                n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper(),
                                a_transito = Convert.ToInt32(a_doc),
                                r_transito = Convert.ToString(c_doc),
                                IdEstado = Convert.ToInt32(unContenedor.SelectSingleNode("ESTADO_ADUANA").InnerText),
                                f_reg_aduana = Convert.ToDateTime(unContenedor.SelectSingleNode("FEC_REG_ESTADO").InnerText),
                                IdSelectividad = Convert.ToInt32(unContenedor.SelectSingleNode("SELECTIVIDAD").InnerText),
                                n_nit = unContenedor.SelectSingleNode("N_NIT").InnerText.Replace("-", "").ToUpper(),
                                b_siduneawd = b_sidu,
                                s_consignatario = unContenedor.SelectSingleNode("CONSIGNATARIO").InnerText.ToUpper(),
                                n_BL = unContenedor.SelectSingleNode("BL").InnerText.ToUpper(),
                                s_descripcion = unContenedor.SelectSingleNode("DESCRIPCION").InnerText.ToUpper()
                            };

                            _resultP = EstadosDeclaDAL.InsertarTest(estadoDe);
                        }

                    }



                    int _result = Convert.ToInt32(_resultP);

                    if (_result == 0)
                        _respuesta = "<MSG>1| Código de ADUANA incorrecto</MSG>";
                    else if (_result == 1)
                        _respuesta = "<MSG>1| Problemas en número de contenedor no cumple con los estandares internacionales</MSG>";
                    else if (_result == 2)
                        _respuesta = "<MSG>1| Año de la declaracion incorrecto</MSG>";
                    else if (_result == 3)
                        _respuesta = "<MSG>0| Registrado satisfactoriamente</MSG>";
                    else if (_result == 4)
                        _respuesta = "<MSG>1| Verificar Estado declaración y selectividad</MSG>";
                    else if (_result == 5)
                        _respuesta = "<MSG>1| El número de NIT no es válido</MSG>";
                }

            }




            return _respuesta;
        }


        [WebMethod(Description = "Metodo para evaluar si un contenedor esta solicitado por la DAN")]
        public string validDAN(string xmlDoc)
        {
            string _respuesta = null;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlDoc);

            XmlNodeList listaCntres = doc.SelectNodes("Cnts/DAN");

            XmlNode unContenedor;

            if (listaCntres.Count > 0)
            {
                for (int i = 0; i < listaCntres.Count; i++)
                {
                    string c_llegada = null, n_contenedor = null;
                    unContenedor = listaCntres.Item(i);

                    n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper();
                    c_llegada = unContenedor.SelectSingleNode("C_LLEGADA").InnerText.Replace("-", "").ToUpper();


                    string _resultP = EstadosDeclaDAL.ConsulDAN(c_llegada, n_contenedor);

                    int _result = Convert.ToInt32(_resultP);

                    if (_result == 0)
                        _respuesta = "<MSG>0| Contenedor no posee retencion</MSG>";
                    else if (_result == 1)
                        _respuesta = "<MSG>1| Contenedor retenido por la DAN</MSG>";
                }

            }




            return _respuesta;
        }

        [WebMethod(Description = "Metodo para registrar las deconsolidaciones realizadas a un contenedor")]
        public string InsertarBLs(string xmlDoc)
        {
            string _respuesta = null;
            int _resulDeta = 0;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(xmlDoc);

            XmlNodeList encaMani = doc.SelectNodes("MdsParts/MDS4");

            XmlNode unMani;

            List<DetaDoc> lstDoc = new List<DetaDoc>();
            DetaDoc pDoc = new DetaDoc();

            if (encaMani.Count > 0)
            {
                for (int i = 0; i < encaMani.Count; i++)
                {

                    unMani = encaMani.Item(i);

                    pDoc.a_manif = unMani.SelectSingleNode("CAR_REG_YEAR").InnerText.Trim();
                    pDoc.num_manif = Convert.ToInt32(unMani.SelectSingleNode("CAR_REG_NBER").InnerText.Trim());

                    break;

                }
            }



            XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4/MDS5");

            XmlNode unContenedor;

            List<ArchivoAduanaValid> pGuarda = new List<ArchivoAduanaValid>();

            if (listaCntres.Count > 0)
            {
                for (int i = 0; i < listaCntres.Count; i++)
                {
                    unContenedor = listaCntres.Item(i);

                    string _contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText;

                    ArchivoAduanaValid validAduana = new ArchivoAduanaValid
                    {
                        IdValid = -1,
                        n_contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText.Replace(" -", "").Replace(" ", ""),
                        n_manifiesto = pDoc.num_manif,
                        n_BL = unContenedor.SelectSingleNode("KEY_BOL_REF").InnerText.Trim(),
                        n_BL_master = unContenedor.SelectSingleNode("KEY_BOL_REF_MST").InnerText.Trim(),
                        a_mani = pDoc.a_manif,
                        c_tipo_bl = unContenedor.SelectSingleNode("CARBOL_TYP_COD").InnerText.Trim(),
                        b_sidunea = 1,
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
                    _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValidMst(validAduana, DBComun.Estado.verdadero));

                    if (_resulDeta == 0)
                        _respuesta = "<MSG>1| Desconsolidación NO registrada</MSG>";
                    else if (_resulDeta == 1)
                        _respuesta = "<MSG>0| Registrado satisfactoriamente</MSG>";
                    else if (_resulDeta == 2)
                        _respuesta = "<MSG>1| Contenedor no pasa estándar de validación</MSG>";


                }



            }

            return _respuesta;
        }

        [WebMethod(Description = "Método para registrar bloqueos/desbloqueos DGA")]
        public string RegBloqueos(string xmlDoc)
        {
            string respuesta = null;
            string valida = null;


            string c_llegada = null, c_naviera = null;

            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlDoc);

                XmlNodeList listaCntres = doc.SelectNodes("CntsRet/RETENCION");

                XmlNode unContenedor;

                string b_observa = null, n_contenedor = null;
                int retencion = 0, n_manifiesto = 0, a_mani = 0;

                if (listaCntres.Count > 0)
                {
                    for (int i = 0; i < listaCntres.Count; i++)
                    {
                        unContenedor = listaCntres.Item(i);

                        a_mani = Convert.ToInt32(unContenedor.SelectSingleNode("YEAR_MANI").InnerText);
                        n_manifiesto = Convert.ToInt32(unContenedor.SelectSingleNode("NUM_MANI").InnerText);
                        n_contenedor = unContenedor.SelectSingleNode("NUM_CONTEN").InnerText.Replace("-", "").ToUpper();
                        b_observa = unContenedor.SelectSingleNode("JUST").InnerText.Replace("-", "").ToUpper();
                        retencion = Convert.ToInt32(unContenedor.SelectSingleNode("RETENIDO").InnerText);
                        break;
                    }
                }

                if (n_contenedor.TrimStart().TrimEnd() == "" || (b_observa.TrimStart().TrimEnd() == ""))
                {
                    respuesta = "1|Ingrese el número de contenedor a validar y sus observaciones";
                }
                else
                {
                    if (b_observa.Length <= 254)
                    {
                        //valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, n_contenedor.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlServer);

                        //int valor = ValidaTarjaDAL.ValidaCantidad(n_contenedor.TrimStart().TrimEnd(), n_manifiesto, a_mani);

                       string valRet = DetaNavieraDAL.ValidRetDGA(DBComun.Estado.verdadero, n_contenedor.Trim().TrimEnd().TrimStart(), a_mani.ToString(), n_manifiesto, DBComun.TipoBD.SqlServer);

                        if (valRet == "SIN COINCIDENCIAS")
                        {
                            respuesta = "1| El contenedor especifico que no produce resultados verificar si número de manifiesto es correcto";
                        }
                        else if (valRet == "NO VALIDO")
                        {
                            respuesta = "1| El contenedor ya no se encuentra en el recinto portuario";
                        }
                        else if (valRet == "CANCELADO")
                        {
                            respuesta = "1| El contenedor fue cancelado a solicitud de la naviera";
                        }
                        else if (valRet == "VALIDO")
                        {
                            List<EncaNaviera> pEnca = new List<EncaNaviera>();

                            pEnca = DetaNavieraDAL.getEncaContenedor(n_contenedor, a_mani.ToString(), n_manifiesto, DBComun.Estado.verdadero);

                            string ubica = null;
                            if (pEnca.Count > 0)
                            {
                                foreach (var item in pEnca)
                                {
                                    ubica = getUbica(n_contenedor, item.c_llegada, item.c_naviera);
                                    c_llegada = item.c_llegada;
                                    c_naviera = item.c_naviera;
                                    break;
                                }
                            }

                            if (ubica.Contains("Exportado"))
                            {
                                respuesta = "1| " + ubica;
                            }
                            else
                            {
                                int _resul = 0;
                                if (retencion == 1)
                                    _resul = Convert.ToInt32(DetaNavieraDAL.upRetDGA(DBComun.Estado.verdadero, n_contenedor, a_mani.ToString(), n_manifiesto, "aduana.service", b_observa, retencion));
                                else
                                    _resul = Convert.ToInt32(DetaNavieraDAL.upRetDGA(DBComun.Estado.verdadero, n_contenedor, a_mani.ToString(), n_manifiesto, "aduana.service", "NULL", retencion));

                                if (_resul == 1)
                                {
                                    //string buque = null;
                                    //string cliente = null;
                                    //List<DocBuque> pDoc = new List<DocBuque>();
                                    //pDoc = DocBuqueLINQ.ObtEncDGA(c_llegada, c_naviera);

                                    //if (pDoc.Count > 0)
                                    //{
                                    //    foreach (var item in pDoc)
                                    //    {
                                    //        buque = item.d_buque;
                                    //        cliente = item.d_cliente;
                                    //        break;
                                    //    }
                                    //}
                                    //  EnviarCorreo(n_contenedor, buque, cliente, string.Concat(a_mani, "-", n_manifiesto), c_naviera, b_observa);

                                    respuesta = "0|Registrado Correctamente";

                                }
                                else
                                {
                                    respuesta = "1| Verificar la información introducida, y volverlo a intentar.";
                                }


                            }

                        }

                    }
                    else
                    {
                        respuesta = "1| La justificación es superior al rango permitido.";
                    }                   
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = "1| Información no logra ser validada verifique e intente de nuevo";
            }


        }

        public static string getUbica(string c_contenedor, string c_llegada, string c_naviera)
        {
            string _contenedores = "";
            string apiUrl = @"http://190.86.251.57:83/api/Ejecutar/?Consulta=";
            Procedure proceso = new Procedure
            {
                NBase = "CONTENEDORES",
                Procedimiento = "Sqlzonas", // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
                Parametros = new List<Parametros>()
            };
            proceso.Parametros.Add(new Parametros { nombre = "llegada", valor = c_llegada });
            proceso.Parametros.Add(new Parametros { nombre = "_contenedor", valor = c_contenedor });
            proceso.Parametros.Add(new Parametros { nombre = "navi", valor = c_naviera });

            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = ConectarUb(_contenedores, apiUrl);
            return _contenedores;
        }

        private static string ConectarUb(string _contenedores, string apiUrl)
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
                        _contenedores = tabla.Rows[0][0].ToString();
                    }
                }
            }
            return _contenedores;
        }
        public static void EnviarCorreo(string n_contenedor, string d_buque, string c_cliente, string nmani, string c_naviera, string comentarios)
        {
            string Html;

            EnvioCorreo _correo = new EnvioCorreo();
            string c_prefijo = string.Empty;
            DateTime _fecha;
            try
            {
                List<UsuarioNaviera> pUser = UsuarioDAL.ObtenerUsuNavi(c_naviera);

                if (pUser == null)
                    pUser = new List<UsuarioNaviera>();


                foreach (var itemU in pUser)
                {
                    c_prefijo = itemU.c_navi_corto;
                    break;
                }

                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<b><u> RETENCIÓN  DE CONTENEDORES ADUANA </b></u><br />";
                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                Html += "<tr>";
                _fecha = DetaNavieraLINQ.FechaBD();
                Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                Html += "</tr>";
                Html += "<tr>";
                Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + HttpContext.Current.User.Identity.Name + "</font></td>";
                Html += "</tr>";
                Html += "</table>";
                Html += "<br />";

                Html += "MÓDULO : CONTENEDOR RETENIDO POR ADUANA <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDOR RETENIDO POR ADUANA <br /><br />";
                Html += string.Format("El contenedor {0} correspondientes al buque {1}, del manifiesto de ADUANA # {2} ha sido solicitado por ADUANA justificando que la retención es debido a: {3}", n_contenedor, d_buque, nmani, comentarios);
                Html += "<br /><br/>";




                _correo.Subject = string.Format("ADUANA : Contenedor Retenido para {0} del buque {1}, Manifiesto de Aduana # {2}", c_prefijo, d_buque, nmani);
                //_correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_sol_dga", DBComun.Estado.verdadero, c_naviera);
                //List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_sol_dga", DBComun.Estado.verdadero, c_cliente);

                //if (_listaCC == null)
                //    _listaCC = new List<Notificaciones>();

                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_sol_dga", DBComun.Estado.verdadero, c_naviera));
                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_sol_dga", DBComun.Estado.verdadero, "219"));
                //_correo.ListaCC = _listaCC;

                Notificaciones noti = new Notificaciones
                {
                    sMail = "elsa.sosa@cepa.gob.sv",
                    dMail = "Elsa Sosa"
                };

                List<Notificaciones> pLisN = new List<Notificaciones>();

                pLisN.Add(noti);

                _correo.ListaNoti = pLisN;


                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                _correo = null;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    }
}

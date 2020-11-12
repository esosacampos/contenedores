using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.OleDb;
using CEPA.CCO.Linq;
using System.Threading;
using System.Text;
using System.Web.Services;
using Newtonsoft.Json;
using System.Net;
using System.Data;
using System.IO;
using System.Web.Configuration;

namespace CEPA.CCO.UI.Web
{
    public partial class wfRetDGA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Datepicker.Text = DateTime.Now.Year.ToString();

            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
           // CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
           // RegValid(_cargar);
        }


        public static string inTarjaValid(ValiadaTarja pTarja)
        {
            string _contenedores = "";
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
            Procedure proceso = new Procedure
            {
                NBase = "CONTENEDORES",
                Procedimiento = "InsValTarje", // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
                Parametros = new List<Parametros>()
            };
            proceso.Parametros.Add(new Parametros { nombre = "amanifiesto", valor = pTarja.Amanifiesto.ToString() });
            proceso.Parametros.Add(new Parametros { nombre = "nmanifiesto", valor = pTarja.Nmanifiesto.ToString() });
            proceso.Parametros.Add(new Parametros { nombre = "ncontenedor", valor = pTarja.Ncontenedor });
            proceso.Parametros.Add(new Parametros { nombre = "observa", valor = pTarja.Observa });
            proceso.Parametros.Add(new Parametros { nombre = "usuario", valor = pTarja.Usuario });           

            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static string Conectar(string _contenedores, string apiUrl)
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
        public static string RegValid(int n_manifiesto, int a_mani, string b_observa, string n_contenedor, int radio3)
        {
            string respuesta = null;
            string valida = null;

            string c_llegada = null, c_naviera = null;

            try
            {                
                if (radio3 == 0)
                {
                    if (n_contenedor.TrimStart().TrimEnd() == "" || (b_observa.TrimStart().TrimEnd() == ""))
                    {
                        respuesta = "1|Ingrese el número de contenedor a validar y sus observaciones";
                    }
                    else if (n_contenedor.Trim().TrimEnd().TrimStart().Length == 11)
                    {
                        if (b_observa.Length <= 254)
                        {
                            valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, n_contenedor.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlServer);

                            if (valida == "VALIDO")
                            {
                                //int valor = ValidaTarjaDAL.ValidaCantidad(n_contenedor.TrimStart().TrimEnd(), n_manifiesto, a_mani);

                                string valRet = DetaNavieraDAL.ValidRetDGA(DBComun.Estado.verdadero, n_contenedor.Trim().TrimEnd().TrimStart(), a_mani.ToString(), n_manifiesto, DBComun.TipoBD.SqlServer);

                                if (valRet == "SIN COINCIDENCIAS")
                                {
                                    respuesta = "2|El contenedor especifico que no produce resultados verificar si número de manifiesto es correcto";
                                }
                                else if (valRet == "NO VALIDO")
                                {
                                    respuesta = "3| El contenedor ya no se encuentra en el recinto portuario";
                                }
                                else if(valRet == "CANCELADO")
                                {
                                    respuesta = "6| El contenedor fue cancelado a solicitud de la naviera";
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
                                        respuesta = "4| " + ubica;
                                    }
                                    else
                                    {
                                        int _resul = Convert.ToInt32(DetaNavieraDAL.upRetDGA(DBComun.Estado.verdadero, n_contenedor, a_mani.ToString(), n_manifiesto, HttpContext.Current.User.Identity.Name, b_observa));

                                        if (_resul == 1)
                                        {
                                            string buque = null;
                                            string cliente = null;
                                            List<DocBuque> pDoc = new List<DocBuque>();
                                            pDoc = DocBuqueLINQ.ObtEncDGA(c_llegada, c_naviera);

                                            if (pDoc.Count > 0)
                                            {
                                                foreach (var item in pDoc)
                                                {
                                                    buque = item.d_buque;
                                                    cliente = item.d_cliente;
                                                    break;
                                                }
                                            }
                                            EnviarCorreo(n_contenedor, buque, cliente, string.Concat(a_mani, "-", n_manifiesto), c_naviera, b_observa);

                                            respuesta = "0|Registrado Correctamente";

                                        }
                                        else
                                        {
                                            respuesta = "5|Verificar la información introducida, y volverlo a intentar.";
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        respuesta = "3|El número de contenedor " + n_contenedor + " no es válido debe poseer 11 caracteres Ej. XXXU9999999";
                    }
                }
                else
                {
                    
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = "5|Verificar la información introducida, y volverlo a intentar.";
            }

           
        }

        public static string getUbica(string c_contenedor, string c_llegada, string c_naviera)
        {
            string _contenedores = "";
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
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

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetConte(string prefix, int n_manifiesto, int a_mani)
        {
            List<string> customers = new List<string>();

            customers = ValidaTarjaDAL.GetContenedor(prefix, n_manifiesto, a_mani, "");


            return customers.ToArray();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string SaveValid(int n_manifiesto, int a_mani, string b_observa, string n_contenedor, int radio3)
        {

            string resultado = RegValid(n_manifiesto, a_mani, b_observa, n_contenedor, radio3);

            return resultado;

            
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
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_sol_dga", DBComun.Estado.verdadero, c_naviera);
                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_sol_dga", DBComun.Estado.verdadero, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_sol_dga", DBComun.Estado.verdadero, c_naviera));
                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_sol_dga", DBComun.Estado.verdadero, "219"));
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
                _correo = null;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        protected void txtObserva_Load(object sender, EventArgs e)
        {
            txtObserva.Attributes.Add("onkeypress", " ValidarCaracteres(this, 254);");
            txtObserva.Attributes.Add("onkeyup", " ValidarCaracteres(this, 254);");
        }
    }
}
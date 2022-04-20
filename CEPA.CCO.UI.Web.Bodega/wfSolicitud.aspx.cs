using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEPA.CCO.UI.Web.Bodega
{
    public partial class wfSolicitud : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);
        private static readonly DateTime SECOND_GOOD_DATE = new DateTime(2016, 01, 01);

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlTipoVac.DataSource = getTipoVac();
                ddlTipoVac.DataTextField = "Descripcion";
                ddlTipoVac.DataValueField = "IdTipoVac";
                ddlTipoVac.DataBind();

                ddlTipoVac.Items.Insert(0, new ListItem("-- Seleccionar Tipo De Vaciado -- ", "0"));

            }
        }       
        
        [WebMethod]
        public static string GetVaciados()
        {
            try
            {
                var dt = getTipoVac();
                return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string saveVaciados(int pIdTipo, string n_manifiesto, string n_contenedor, string n_contacto, string t_contacto,
            string e_contacto, string s_archivo)
        {

            try
            {                
                var vaciado = new Vaciado();                            

                string[] man = n_manifiesto.Trim().Replace("_", "").Split('-');

                vaciado.IdTipoVa = pIdTipo;
                vaciado.a_manifiesto = Convert.ToInt32(man[0]);
                vaciado.n_manifiesto = Convert.ToInt32(man[1]);
                vaciado.n_contenedor = n_contenedor.ToUpper().Trim();
                vaciado.n_contacto = n_contacto.ToUpper();
                vaciado.t_contacto = t_contacto.Replace("-", "");
                vaciado.e_contacto = e_contacto.ToLower();
                vaciado.s_archivo_aut = s_archivo.ToUpper();

                string valor = ValidaTarjaDAL.ValidFact(DBComun.Estado.verdadero, n_contenedor, n_manifiesto.Trim().Replace("_", ""), DBComun.TipoBD.SqlServer);
                int _resultado = 0;
                if (valor == "EXISTE")
                {
                    _resultado = VaciadoDAL.InsertSolVac(vaciado);
                    if (_resultado > 0)
                        EnviarCorreo(vaciado, _resultado);
                    else
                        throw new Exception("No se registro su solicitud, contactar con encargado de bodega #3 para pedir soporte técnico o al contacto de Elsa Sosa 2405-3255 / 7070-8256");

                }
                else
                {
                    throw new Exception("Número de contenedor no esta asociado con el manifiesto indicado");
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(string.Format("La solicitud Nº {0} ha sido recibida satisfactoriamente, muy pronto nos pondremos en contacto con usted.", _resultado));
            }
            catch (Exception ex)

            {                
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message.Replace("\"", ""));
            }

        }

        public static List<TipoVaciado> getTipoVac()
        {
            List<TipoVaciado> pTipoVaciados = new List<TipoVaciado>();
            try
            {        
                pTipoVaciados = VaciadoDAL.ObtenerResultado(DBComun.Estado.verdadero);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pTipoVaciados;
        }

        public static void EnviarCorreo(Vaciado pSoli, int pNum)
        {
            string Html;

            EnvioCorreo _correo = new EnvioCorreo();
            string c_prefijo = string.Empty;
            DateTime _fecha;
            try
            {
                string c_llegada = DetaNavieraDAL.obt_Llegada(DBComun.Estado.verdadero, string.Concat(pSoli.a_manifiesto, "-", pSoli.n_manifiesto));
                string d_buque = EncaBuqueDAL.getBuque(DBComun.Estado.verdadero, c_llegada);

                string tipoVac = getTipoVac().Where(a => a.IdTipoVac == pSoli.IdTipoVa).Select(t => t.Descripcion).FirstOrDefault();

                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<b><u> SOLICITUD  DE VACIADO DE CONTENEDOR </b></u><br />";
                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                Html += "<tr>";
                _fecha = DetaNavieraLINQ.FechaBD();
                Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                Html += "</tr>";
                Html += "<tr>";
                Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + pSoli.n_contacto.ToUpper() + "</font></td>";
                Html += "</tr>";
                Html += "</table>";
                Html += "<br />";

                Html += "MÓDULO : SOLICITUD DE VACIADO DE CONTENEDOR <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE SOLICITUD DE VACIADO DE CONTENEDOR <br /><br />";
                Html += "<p style='text-align: justify;'>";
                Html += string.Format("La solicitud de vacíado <b>#{0}</b> para el contenedor <b>{1}</b> correspondientes al buque <b>{2}</b>, del manifiesto de ADUANA <b># {3}</b> ha sido solicitado para realizar la operación de: <b>{4}</b> a petición del solicitante: <b>{5}</b> - teléfono de solicitante: <b>{6}</b> / correo electrónico de solicitante: <b>{7}</b>", pNum.ToString(), pSoli.n_contenedor, d_buque, string.Concat(pSoli.a_manifiesto, "-", pSoli.n_manifiesto), tipoVac, pSoli.n_contacto.ToUpper(), pSoli.t_contacto, pSoli.e_contacto);
                Html += "</p><br /><br/>";


                _correo.Subject = string.Format("VACIADO : Solicitud de Vacíado #{0} para # Contenedor {1} del buque {2}, Manifiesto de Aduana # {3}", pNum.ToString(), pSoli.n_contenedor, d_buque, string.Concat(pSoli.a_manifiesto, "-", pSoli.n_manifiesto));
                
                
                //_correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_sol_dga", DBComun.Estado.verdadero, c_naviera);
                //List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_sol_dga", DBComun.Estado.verdadero, c_cliente);

                //if (_listaCC == null)
                //    _listaCC = new List<Notificaciones>();

                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_sol_dga", DBComun.Estado.verdadero, c_naviera));
                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_sol_dga", DBComun.Estado.verdadero, "219"));
                //_correo.ListaCC = _listaCC;


                Notificaciones noti = new Notificaciones
                {
                    sMail = pSoli.e_contacto,
                    dMail = pSoli.n_contacto
                };

                List<Notificaciones> pLisN = new List<Notificaciones>();

                pLisN.Add(noti);

                _correo.ListaNoti = pLisN;

                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificaciones("b_noti_vaciado", DBComun.Estado.verdadero, "0");

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();
                
                _correo.ListaCC = _listaCC;


                _correo.ListArch.Add(HttpContext.Current.Server.MapPath("~/Solicitudes/" + pSoli.s_archivo_aut));

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                _correo = null;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string getCliente(string n_nit)
        {
            var query = (from a in ClienteDAL.getTipoCliente(DBComun.Estado.verdadero)
                         where a.s_nit == n_nit
                         select new
                         {
                             s_nombre = a.s_nombre_comercial
                         }).FirstOrDefault();



            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }
    }
}
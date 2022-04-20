using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEPA.CCO.UI.Web.PatioRecep
{
    public partial class Ubicacion : System.Web.UI.Page
    {
        public class Ubication {
            public int IdZona { get; set; }
            public int IdUbication { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetConte(string prefix)
        {
            List<string> customers = new List<string>();

            //customers = ValidaTarjaDAL.GetContenedor(prefix);


            var query = (from a in ValidaTarjaDAL.GetContePatio(prefix)
                          join b in EncaBuqueDAL.showShipping(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                         select new InfOperaciones
                         {
                             n_contenedor = a.n_contenedor,
                             IdDeta = a.IdDeta
                         }).ToList();



            foreach (var item in query)
            {
                customers.Add(string.Format("{0}-{1}", item.n_contenedor, item.IdDeta));
            }


            return customers.ToArray();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string GetConteInfo(string n_contenedor)
        {
            var query = (from a in ValidaTarjaDAL.GetContenedorInforPatio(n_contenedor)
                         join b in EncaBuqueDAL.getBuqueLleg(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada into conInfo
                         from c in conInfo.DefaultIfEmpty()
                         select new InfOperaciones
                         {
                             c_tamaño = a.c_tamaño,
                             v_tara = a.v_tara,
                             c_trafico = a.c_trafico,
                             c_llegada = a.c_llegada,
                             c_estado = a.c_estado,
                             c_buque = c == null ? "" : c.d_buque,
                             c_cliente = a.c_cliente,
                             IdDeta = a.IdDeta,
                             b_detenido = a.b_detenido,
                             b_style = a.b_style,
                             b_aduana = a.b_aduana,
                             b_staduana = a.b_staduana,
                             c_correlativo = a.c_correlativo,
                             v_peso = a.v_peso
                         }).ToList();



            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string getUbicaciones()
        {
            var query = (from a in ValidaTarjaDAL.getUbicacion()
                         select new Ubicaciones
                         {
                             IdZona = a.IdZona,
                             Zona = a.Zona
                         }).ToList();


            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string getCarriles(int pIdZona)
        {
            int carril = Convert.ToInt32(ValidaTarjaDAL.getCarril(pIdZona));


            List<Ubication> query = new List<Ubication>();

            for (int i = 1; i <= carril; i++)
            {
                Ubication pCarril = new Ubication
                {
                    IdZona = pIdZona,
                    IdUbication = i
                };
                query.Add(pCarril);
            }


            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string getPosicion(int pIdZona)
        {
            int carril = Convert.ToInt32(ValidaTarjaDAL.getPosicion(pIdZona));


            List<Ubication> query = new List<Ubication>();

            for (int i = 1; i <= carril; i++)
            {
                Ubication pPosicion = new Ubication
                {
                    IdZona = pIdZona,
                    IdUbication = i
                };
                query.Add(pPosicion);
            }


            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string getNivel(int pIdZona)
        {
            int carril = Convert.ToInt32(ValidaTarjaDAL.getNivel(pIdZona));


            List<Ubication> query = new List<Ubication>();

            for (int i = 1; i <= carril; i++)
            {
                Ubication pNivel = new Ubication
                {
                    IdZona = pIdZona,
                    IdUbication = i
                };
                query.Add(pNivel);
            }


            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string getGruas()
        {
            var query = (from a in ValidaTarjaDAL.getGruas()
                         select new Gruas
                         {
                             IdGrua = a.IdGrua,
                             Nombre = a.Nombre
                         }).ToList();


            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string saveConfirmacion(int zonaId, int gruaId, int carril, int posicion, int nivel, string s_condicion, int c_marcacion, int v_tara, int b_sobre, int IdDeta)
        {

            
            string _valor = null;
            Confirmacion clsConfirmacion = new Confirmacion
            {
                IdDeta = IdDeta,
                IdGrua = gruaId,
                IdZona = zonaId,
                Carril = carril,
                Posicion = posicion,
                Nivel = nivel,
                s_condicion = s_condicion == "" ? "RECIBIDO SIN DAÑO": s_condicion,
                c_marcacion = c_marcacion,
                v_tara = v_tara,
                b_sobredimensionado = b_sobre
            };




            string _resultado = ValidaTarjaDAL.saveConfirPatio(clsConfirmacion);
            
         

            if (Convert.ToInt32(_resultado) > 0)
                _valor = "Registro de confirmación exitoso";
            else
                _valor = "Verificar información o reportar problemas en confirmación";


            return Newtonsoft.Json.JsonConvert.SerializeObject(_valor);
        }

        [System.Web.Services.WebMethod]
        public static string getResumen(string c_llegada)
        {
            try
            {


                List<RecepEsta> consulta = new List<RecepEsta>();
                consulta = RecepEstaDAL.getResumenDAL(DBComun.Estado.verdadero, c_llegada);
                var query = (from a in consulta
                             select new RecepEsta
                             {
                                 d_buque = c_llegada + " - " + EncaBuqueDAL.getBuque(DBComun.Estado.verdadero, c_llegada),
                                 c_tamaño = a.c_tamaño,
                                 manifestados = a.manifestados,
                                 cancelados = a.cancelados,
                                 total = a.total,
                                 recibidos = a.recibidos,
                                 pendientes = a.pendientes
                             }).ToList();


                var jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(query);

                return jConsult;
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }
        }

    }
}
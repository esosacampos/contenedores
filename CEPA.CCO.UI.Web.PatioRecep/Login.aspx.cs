using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using Newtonsoft.Json;
using System.Web.Security;

namespace CEPA.CCO.UI.Web.PatioRecep
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string setAccess(string codigo)
        {
            try
            {
                string c_marcacion = codigo;
                if (c_marcacion == string.Empty || c_marcacion.Length == 0)
                    throw new Exception("Debe de introducir código de marcación");

                List<UsuarioContra> _lista = UsuarioDAL.UserContratista(DBComun.Estado.verdadero, Convert.ToInt32(c_marcacion));

                if (_lista == null)
                    _lista = new List<UsuarioContra>();

                if (_lista.Count == 0)
                    throw new Exception("El código de marcación ingresado no es válido");



                foreach (var item in _lista)
                {
                    HttpContext.Current.Session["c_marcacion"] = item.c_marcacion;
                    HttpContext.Current.Session["NombreC"] = item.NombreC;
                    HttpContext.Current.Session["c_operadora"] = item.c_operadora;
                    HttpContext.Current.Session["d_operadora"] = item.d_operadora;
                    HttpContext.Current.Session["c_cargo"] = item.c_cargo;
                }


                string nombreC = HttpContext.Current.Session["NombreC"].ToString() + " - " + HttpContext.Current.Session["d_operadora"].ToString();


                System.Web.Security.FormsAuthenticationTicket AuthTicket = new System.Web.Security.FormsAuthenticationTicket(1, nombreC, DateTime.Now, DateTime.Now.AddMinutes(60), false, c_marcacion);

                HttpContext.Current.Request.Cookies.Add(new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, System.Web.Security.FormsAuthentication.Encrypt(AuthTicket)));

                return Newtonsoft.Json.JsonConvert.SerializeObject("Aceptado");
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");      
            }
        }

        [System.Web.Services.WebMethod]
        public static string getNombre(string codigo)
        {
            try
            {

                List<string> customers = new List<string>();

                //customers = ValidaTarjaDAL.GetContenedor(prefix);



                var query = (from a in UsuarioDAL.UserContratista(DBComun.Estado.verdadero, Convert.ToInt32(codigo))
                                 //join b in EncaBuqueDAL.showShipping(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                             select new UsuarioContra
                             {
                                 NombreC = a.NombreC,
                                 c_marcacion = a.c_marcacion
                             }).ToList();


                string nombre = null;

                if(query.Count() ==0)
                    throw new Exception("Error: Código de marcación no existe");

                foreach (var item in query)
                {
                    nombre = item.NombreC;
                    break;
                }


                return Newtonsoft.Json.JsonConvert.SerializeObject(nombre);
               

            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }


        }
    }
}
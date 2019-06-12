using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;


namespace CEPA.CCO.ServiBusqueda
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://10.1.4.184:2500/Service1.asmx")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {      

        /*[WebMethod(EnableSession=true)]
        public string[] ObtenerContenedores(string prefixText, int count)        
        {
            int pId = 0;
            if (Context.Session["IdRegistro"] != null)
            {
                pId = Convert.ToInt32(Context.Session["IdRegistro"]);
            }
            else
            {
                throw new Exception("Registro Inválido");
            }

            var query = from a in DetaNavieraDAL.ObtenerBusqueda(pId)
                        where a.n_contenedor.StartsWith(prefixText)
                        orderby a.n_contenedor
                        select new
                        {
                            IdDeta = a.IdDeta,
                            n_contenedor = a.n_contenedor
                        };

            List<string> contenedores = new List<string>();
            foreach (var item in query)
            {
                contenedores.Add(item.n_contenedor);
            }

            return contenedores.ToArray();
                       
        }*/


    }
}
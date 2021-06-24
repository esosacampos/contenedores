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
    public partial class wfValidaTarja : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
            string apiUrl = WebConfigurationManager.AppSettings["apiInt"].ToString();
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

            try
            {
                if (ArchivoBookingDAL.isNumeric(a_mani) == true && ArchivoBookingDAL.isNumeric(n_manifiesto) == true)
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
                                valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, n_contenedor.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlTracking);

                                if (valida == "VALIDO")
                                {

                                    string valor = ValidaTarjaDAL.ValidFact(DBComun.Estado.verdadero, n_contenedor.TrimStart().TrimEnd(), string.Concat(a_mani, '-', n_manifiesto), DBComun.TipoBD.SqlServer);

                                    if (valor == "EXISTE")
                                    {
                                        ValiadaTarja _valida = new ValiadaTarja()
                                        {
                                            Observa = b_observa.ToUpper(),
                                            Usuario = System.Web.HttpContext.Current.Session["d_usuario"].ToString().ToUpper(),
                                            Amanifiesto = Convert.ToInt32(a_mani),
                                            Nmanifiesto = Convert.ToInt32(n_manifiesto),
                                            Ncontenedor = n_contenedor
                                        };

                                        string _resultado = inTarjaValid(_valida);
                                        if (_resultado.ToUpper().Contains("EXISTE"))
                                        {
                                            respuesta = "2|El número de contenedor " + n_contenedor + " ya se encuentra validado";
                                        }
                                        else if (_resultado.ToUpper().Contains("OK"))
                                        {

                                            respuesta = "0|Registrado Correctamente";
                                        }
                                        else
                                        {
                                            respuesta = "5|Verificar la información introducida, y volverlo a intentar.";
                                        }
                                    }
                                    else
                                    {
                                        respuesta = "4|Verificar ya que contenedor NO EXISTE en manifiesto seleccionado";
                                    }
                                }
                                else
                                {

                                    respuesta = "4|El número de contenedor " + n_contenedor + " no cumple con el estandar de validación Ej. XXXU9999999";
                                }
                            }
                            else
                            {
                                respuesta = "4|El máximo de caracteres permitidos en las observaciones es 254 ";
                            }
                        }
                        else
                        {
                            respuesta = "3|El número de contenedor " + n_contenedor + " no es válido debe poseer 11 caracteres Ej. XXXU9999999";
                        }
                    }
                    else
                    {
                        if (n_contenedor.TrimStart().TrimEnd() == "" || (b_observa.TrimStart().TrimEnd() == ""))
                        {
                            respuesta = "1|Ingrese el número de contenedor a validar y sus observaciones";
                        }
                        else if (b_observa.Length <= 254)
                        {
                            valida = DetaNavieraDAL.ValidaContenedorShipper(DBComun.Estado.verdadero, n_contenedor.Trim().TrimEnd().TrimStart(), string.Concat(a_mani, '-', n_manifiesto), DBComun.TipoBD.SqlServer);


                            if (valida == "VALIDO")
                            {
                                //int valor = ValidaTarjaDAL.ValidaCantidad(n_contenedor.TrimStart().TrimEnd(), n_manifiesto, a_mani);

                                ValiadaTarja _valida = new ValiadaTarja()
                                {
                                    Observa = b_observa.ToUpper(),
                                    Usuario = System.Web.HttpContext.Current.Session["d_usuario"].ToString().ToUpper(),
                                    Amanifiesto = Convert.ToInt32(a_mani),
                                    Nmanifiesto = Convert.ToInt32(n_manifiesto),
                                    Ncontenedor = n_contenedor
                                };

                                string _resultado = inTarjaValid(_valida);
                                if (_resultado.ToUpper().Contains("EXISTE"))
                                {
                                    respuesta = "2|El número de contenedor " + n_contenedor + " ya se encuentra validado";
                                }
                                else if (_resultado.ToUpper().Contains("OK"))
                                {

                                    respuesta = "0|Registrado Correctamente";
                                }
                                else
                                {
                                    respuesta = "5|Verificar la información introducida, y volverlo a intentar.";
                                }
                            }
                            else
                            {
                                respuesta = "4|El número de contenedor " + n_contenedor + " no cumple con el estandar de validación Ej. XXXU9999999";
                            }
                        }
                        else
                        {

                            respuesta = "4|El máximo de caracteres permitidos en las observaciones es 254 ";
                        }
                    }
                }
                else
                {
                    respuesta = "5|Verificar la información introducida, volverlo a internar (Verificar que año y número de manifiesto sean númericos.";
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                //return respuesta = "5|Verificar la información introducida, y volverlo a intentar.";
                return ex.Message;
            }


        }



        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetConte(string prefix, int n_manifiesto, int a_mani)
        {
            List<string> customers = new List<string>();

            if(prefix.Length > 4)
                prefix = prefix.Substring((prefix.Length - 4), 4);
            
            customers = ValidaTarjaDAL.GetContenedor(prefix, n_manifiesto, a_mani);


            return customers.ToArray();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string SaveValid(int n_manifiesto, int a_mani, string b_observa, string n_contenedor, int radio3)
        {

            string resultado = RegValid(n_manifiesto, a_mani, b_observa, n_contenedor, radio3);

            return resultado;


        }

        protected void txtObserva_Load(object sender, EventArgs e)
        {
            txtObserva.Attributes.Add("onkeypress", " ValidarCaracteres(this, 254);");
            txtObserva.Attributes.Add("onkeyup", " ValidarCaracteres(this, 254);");
        }
    }
}
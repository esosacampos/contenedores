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

namespace CEPA.CCO.UI.Web
{
    public partial class wfValidaTarja : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

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

        public static string RegValid(int n_manifiesto, int a_mani, string b_observa, string n_contenedor)
        {
            string respuesta = null;
            string valida = null;

            try
            {
                

                if (n_contenedor.TrimStart().TrimEnd() == "" || b_observa.TrimStart().TrimEnd() == "")
                {
                    respuesta = "1|Ingrese el número de contenedor a validar y sus observaciones";
                }
                else if (n_contenedor.Trim().TrimEnd().TrimStart().Length == 11)
                {
                    valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, n_contenedor.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlTracking);

                    if (valida == "VALIDO")
                    {
                        int valor = ValidaTarjaDAL.ValidaCantidad(n_contenedor.TrimStart().TrimEnd(), n_manifiesto, a_mani);
                        if (valor > 0)
                        {

                            respuesta = "2|El número de contenedor " + n_contenedor + " ya se encuentra validado";


                        }
                        else
                        {
                            ValiadaTarja _valida = new ValiadaTarja()
                            {
                                b_observa = b_observa,
                                c_usuario = System.Web.HttpContext.Current.Session["d_usuario"].ToString().ToUpper(),
                                a_manifiesto = Convert.ToInt32(a_mani),
                                n_manifiesto = Convert.ToInt32(n_manifiesto),
                                n_contenedor = n_contenedor
                            };
                            ValidaTarjaDAL.InsertarValida1(_valida);

                            respuesta = "0|Registrado Correctamente";
                        }
                    }
                    else
                    {
                        respuesta = "4|El número de contenedor " + n_contenedor + " no cumple con el estandar de validación Ej. XXXU9999999";
                    }
                }
                else
                {
                    respuesta = "3|El número de contenedor " + n_contenedor + " no es válido debe poseer 11 caracteres Ej. XXXU9999999";
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta = "5|Verificar la información introducida, y volverlo a intentar.";
            }

           
        }

      

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string[] GetConte(string prefix, int n_manifiesto, int a_mani)
        {
            List<string> customers = new List<string>();

            customers = ValidaTarjaDAL.GetContenedor(prefix, n_manifiesto, a_mani);


            return customers.ToArray();
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string SaveValid(int n_manifiesto, int a_mani, string b_observa, string n_contenedor)
        {

            string resultado = RegValid(n_manifiesto, a_mani, b_observa, n_contenedor);

            return resultado;

            
        }
    }
}
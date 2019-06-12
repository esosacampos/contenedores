using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region "Namespace Usados"
//using System.Web.Configuration;
//using System.DirectoryServices;
//using System.DirectoryServices.ActiveDirectory;
//using System.Security;
//using System.Security.Permissions;
//using System.Configuration;
//using System.Web.UI.HtmlControls;
//using System.Text;
//using System.Web.Security;
using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Web.Security;
#endregion

namespace CEPA.CCO.UI.Web
{

    public partial class Inicio : System.Web.UI.Page
    {
            

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(inputTxtandPassw.Text == string.Empty || inputTxtandPassw.Text.Length == 0)
                    throw new Exception("Debe de introducir código de marcación");

                List<UsuarioContra> _lista = UsuarioDAL.UserContratista(DBComun.Estado.verdadero, Convert.ToInt32(inputTxtandPassw.Text));

                if (_lista == null)
                    _lista = new List<UsuarioContra>();

                if (_lista.Count == 0)
                    throw new Exception("El código de marcación ingresado no es válido");

               
                
                foreach (var item in _lista)
	            {
		            Session["c_marcacion"] = item.c_marcacion;
                    Session["NombreC"] = item.NombreC;
                    Session["c_operadora"] = item.c_operadora;
                    Session["d_operadora"] = item.d_operadora;
                    Session["c_cargo"] = item.c_cargo;
	            }

                string nombreC = Session["NombreC"].ToString() + " - " + Session["d_operadora"].ToString();


                System.Web.Security.FormsAuthenticationTicket AuthTicket = new System.Web.Security.FormsAuthenticationTicket(1, nombreC, DateTime.Now, DateTime.Now.AddMinutes(60), false, inputTxtandPassw.Text);

                Response.Cookies.Add(new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, System.Web.Security.FormsAuthentication.Encrypt(AuthTicket)));

                Response.Redirect(FormsAuthentication.GetRedirectUrl(nombreC, false), false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");      
            }
        }


    }
}
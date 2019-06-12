using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace CEPA.CCO.UI.Web
{
    public partial class wfConfiguracion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string setPassword(string oldPass, string newPass, string userName)
        {
            string resultado = null;

            string adminUser = @"CENTRAL\acajutla";
            string adminPassword = "@A1003aa";
            string ADPath = "LDAP://cepa.gob.sv/OU=acajutla,DC=cepa,DC=gob,DC=sv";
            string ADUser = userName;
            string strOldPassword = oldPass;
            string strNewPassword = newPass;
            try
            {
                PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, "cepa.gob.sv", "OU=acajutla,DC=cepa,DC=gob,DC=sv", adminUser, adminPassword);

                UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, ADUser);
                if (user == null) return "Usuario no pertenece al dominio";
                user.SetPassword(strNewPassword);
            
                        resultado = "1|Cambio Efectuado!!!";
                        return resultado; 
            }
            catch (Exception ex)
            {
                resultado = "0|Error: " + ex.Message;
                return resultado;
            }
       }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static void logOut()
        {
            HttpContext.Current.Session["c_naviera"] = null;
            HttpContext.Current.Session["c_usuario"] = null;
            HttpContext.Current.Session["d_usuario"] = null;
            //Session["s_email_reg"] = item.c_mail;
            HttpContext.Current.Session["c_iso_navi"] = null;
            HttpContext.Current.Session["c_navi_corto"] = null;

            System.Web.Security.FormsAuthentication.SignOut();
            
        }
   }
}

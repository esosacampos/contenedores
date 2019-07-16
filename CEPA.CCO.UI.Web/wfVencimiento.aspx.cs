using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;

namespace CEPA.CCO.UI.Web
{
    public partial class wfVencimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Inicio.aspx", false);
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod()]
        public static string setPassword(string oldPass, string newPass, string userName)
        {
            string resultado = null;

            string adminUser = @"CENTRAL\acajutla";
            string adminPassword = "@c@:P)20$9";
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

                //ResetPassword(HttpContext.Current.Session["c_usuario"].ToString(), strNewPassword);

                string _resulta = UsuarioDAL.ActChagePass(HttpContext.Current.Session["c_usuario"].ToString(), HttpContext.Current.Session["c_naviera"].ToString());

                if (_resulta == "1")
                    resultado = "1|Cambio Efectuado!!!";
                else
                    resultado = "0|Error: Vuelva a intentar ya que no genero cambios";
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

            foreach (var cookie in HttpContext.Current.Request.Cookies.Keys)
            {
                HttpContext.Current.Request.Cookies.Remove(cookie.ToString());
            }

            System.Web.Security.FormsAuthentication.SignOut();

        }

        public static void ResetPassword(string userDn, string password)
        {
            DirectoryEntry uEntry = new DirectoryEntry(userDn);
            uEntry.Invoke("SetPassword", new object[] { password });
            uEntry.Properties["LockOutTime"].Value = 0; //unlock account

            uEntry.Close();
        }
    }
}
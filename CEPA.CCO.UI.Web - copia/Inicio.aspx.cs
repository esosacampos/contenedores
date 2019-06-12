using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region "Namespace Usados"
using System.Web.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Security;
using System.Security.Permissions;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Security;
using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
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
               
                UsuarioBL _usuBL = new UsuarioBL();
                string _usuario = inputTxtandPassw.Value + "@cepa.gob.sv";

                if (Membership.Providers["LDAP"].ValidateUser(_usuario, inputTxtandPassw2.Value))
                {
                    Ldap.LDAPRoleProvider _ldap = new Ldap.LDAPRoleProvider();
                    string[] groups = _ldap.GetGruposLDAP(inputTxtandPassw.Value);

                    System.Web.Security.FormsAuthenticationTicket AuthTicket = new System.Web.Security.FormsAuthenticationTicket(1, inputTxtandPassw.Value, DateTime.Now, DateTime.Now.AddHours(12), false, groups.ToString());



                    Response.Cookies.Add(new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, System.Web.Security.FormsAuthentication.Encrypt(AuthTicket)));
                    //Session["nombre"] = FullName(inputTxtandPassw.Text);
                    
                    List<Usuario> _usuList = new List<Usuario>();
                    _usuList = _usuBL.ObtenerUsuariAC(DBComun.Estado.verdadero, inputTxtandPassw.Value);

                    if (_usuList.Count > 0)
                    {
                        foreach (Usuario item in _usuList)
                        {
                            if (item.c_usuario.ToUpper() == inputTxtandPassw.Value.ToUpper())
                            {
                                Session["c_naviera"] = item.c_naviera;
                                Session["c_usuario"] = item.c_usuario;
                                Session["d_usuario"] = string.Concat(" ", item.d_usuario, " - ", item.c_navi_corto, " ");
                                //Session["s_email_reg"] = item.c_mail;
                                Session["c_iso_navi"] = item.c_iso_navi;
                                Session["c_navi_corto"] = item.c_navi_corto;
                            }
                        }

                        //string urlR = FormsAuthentication.GetRedirectUrl(inputTxtandPassw.Text, false);

                        Response.Redirect(FormsAuthentication.GetRedirectUrl(inputTxtandPassw.Value, false), false);                   
                        
                    }
                }
                else
                {
                    throw new Exception("Error de autenticación, revisar usuario y contraseña");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            finally
            {
                //Session["nombre_user"] = _filterAttribute.ToString();
            }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Web.Security;

namespace CEPA.Estilo
{
    public partial class wfInicioE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (inputTxtandPassw.Value == string.Empty || inputTxtandPassw.Value.Length == 0)
                    throw new Exception("Debe de introducir nombre de usuario");

                if (inputTxtandPassw.Value == string.Empty || inputTxtandPassw.Value.Length == 0)
                    throw new Exception("Debe de introducir nombre de usuario");

                        UsuarioBL _usuBL = new UsuarioBL();
                string _usuario = inputTxtandPassw.Value + "@cepa.gob.sv";

                if (Membership.Providers["LDAP"].ValidateUser(_usuario, inputTxtandPassw.Value))
                {
                    Ldap.LDAPRoleProvider _ldap = new Ldap.LDAPRoleProvider();
                    string[] groups = _ldap.GetGruposLDAP(inputTxtandPassw.Value);

                    System.Web.Security.FormsAuthenticationTicket AuthTicket = new System.Web.Security.FormsAuthenticationTicket(1, inputTxtandPassw.Value, DateTime.Now, DateTime.Now.AddMinutes(60), false, groups.ToString());

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
                    //lblError.Text = "Error de autenticación, revisar usuario y contraseña";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
             //   lblError.Text = ex.Message;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");      
            }
        }
    }
}
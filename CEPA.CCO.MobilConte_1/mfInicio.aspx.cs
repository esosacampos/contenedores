﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.BL;
using CEPA.CCO.Linq;

namespace CEPA.CCO.MobilConte
{
    public partial class mfInicio : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
       {

       }


        protected void btnAcceso_Click(object sender, EventArgs e)
        {
            try
            {
                string c_marcacion = Request.Form["txtCodigo"];
                if (c_marcacion == string.Empty || c_marcacion.Length == 0)
                    throw new Exception("Debe de introducir código de marcación");

                List<UsuarioContra> _lista = UsuarioDAL.UserContratista(DBComun.Estado.verdadero, Convert.ToInt32(c_marcacion));

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


                System.Web.Security.FormsAuthenticationTicket AuthTicket = new System.Web.Security.FormsAuthenticationTicket(1, nombreC, DateTime.Now, DateTime.Now.AddMinutes(60), false, c_marcacion);

                Response.Cookies.Add(new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, System.Web.Security.FormsAuthentication.Encrypt(AuthTicket)));

                Response.Redirect(FormsAuthentication.GetRedirectUrl(nombreC, false), false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "alert('" + "Problemas al momento de ingresar intentelo de nuevo, o solicitar soporte" + "');", true);
                //Response.Write("<script>alert('" + ex.Message + "');</script>");      
            }
        }
    }
}
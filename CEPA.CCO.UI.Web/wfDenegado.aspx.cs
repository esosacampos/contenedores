using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Text;
using System.Xml;
//using CEPA.CCO.UI.Web.WSManifiestoCEPAService;
using System.IO;
using System.Web.Services;

namespace CEPA.CCO.UI.Web
{
    public partial class wfDenegado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClientScript.GetPostBackEventReference(this, string.Empty);

                string targetCtrl = Page.Request.Params.Get("__EVENTTARGET");
                if (targetCtrl != null && targetCtrl != string.Empty)
                {
                    //lblMsg.Text = targetCtrl + " button make Postback";
                }

                if (Request.Form["__EVENTTARGET"] == "btnLogOut")
                {
                    btnLogOut_Click(this, new EventArgs());
                }

                if (!IsPostBack)
                {
                    
                    //sessionId.InnerText = Session["c_usuario"].ToString();
                    sessionInput.InnerText = Session["d_usuario"].ToString().ToUpper();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["c_naviera"] = null;
            Session["c_usuario"] = null;
            Session["d_usuario"] = null;
            //Session["s_email_reg"] = item.c_mail;
            Session["c_iso_navi"] = null;
            Session["c_navi_corto"] = null;

            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Inicio.aspx", false);
        }

        public void WriteFullElementString(XmlTextWriter xml,
                                          string localName,
                                          string value)
        {
            xml.WriteStartElement(localName);
            xml.WriteString(value);
            xml.WriteFullEndElement();
        }

        public string AsString(XmlDocument xmlDoc)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xmlDoc.WriteTo(xmlTextWriter);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
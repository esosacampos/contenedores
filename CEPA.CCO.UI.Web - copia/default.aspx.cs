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

namespace CEPA.CCO.UI.Web
{
    public partial class _default : System.Web.UI.Page
    {
        private MenuItem mnuMenuItem;
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
                    ObtenerMenu();
                    //sessionId.InnerText = Session["c_usuario"].ToString();
                    sessionInput.InnerText = Session["d_usuario"].ToString();
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

        public void ObtenerMenu()
        {
            List<MenuItem> menuItem = new List<MenuItem>();

            List<CEPA.CCO.Entidades.Menu> menuLista = MenuDAL.ObtenerMenu(HttpContext.Current.User.Identity.Name);


            foreach (CEPA.CCO.Entidades.Menu item in menuLista)
            {
                if (item.PadreId == 0)
                {
                    mnuMenuItem = new MenuItem();
                    mnuMenuItem.Value = item.MenuId.ToString();
                    mnuMenuItem.Text = item.NombreMenu;
                    mnuMenuItem.ImageUrl = item.Icono;
                    mnuMenuItem.NavigateUrl = item.Url;
                   

                    //agregar al menu principal de la pagina html
                    MenuPrincipal.Items.Add(mnuMenuItem);

                    //hacemos un llamado al metodo recursivo encargado de generar el arbol del menu.
                    AddMenuItem(mnuMenuItem, menuLista);
                }
            }

        }

        private void AddMenuItem(MenuItem mnuMenuItem, List<CEPA.CCO.Entidades.Menu> lista)
        {
            MenuItem mnuNewMenuItem;

            foreach (CEPA.CCO.Entidades.Menu menuSubItem in lista)
            {
                if (menuSubItem.PadreId.ToString().Equals(mnuMenuItem.Value)
                    && !menuSubItem.MenuId.ToString().Equals(menuSubItem.PadreId.ToString()))
                {
                    mnuNewMenuItem = new MenuItem();

                    mnuNewMenuItem.Value = menuSubItem.MenuId.ToString();
                    mnuNewMenuItem.Text = menuSubItem.NombreMenu;
                    mnuNewMenuItem.ImageUrl = menuSubItem.Icono;
                    mnuNewMenuItem.NavigateUrl = menuSubItem.Url;

                    mnuMenuItem.ChildItems.Add(mnuNewMenuItem);

                    //llamada recursiva para ver si el nuevo menu item aun tiene elementos hijos.
                    AddMenuItem(mnuNewMenuItem, lista);
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //ObtenerResultados();

            //List<Facturacion> pLista = DocBuqueLINQ.ObtenerFacturaTarja();
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

        #region "Codigo"
        
        //public XmlDocument ObtenerResultados()
        //{
        //    string ruta = @"C:\inetpub\ArchivoXML.xml";
        //    XmlDocument x = new XmlDocument();
        //    StringBuilder sb = new StringBuilder();
        //    TransferXML _clase = new TransferXML();

        //    WSManifiestoCEPAClient _proxu = new WSManifiestoCEPAClient();

        //    string _Aduna = null;          

        //    List<TransferXML> _pList = CEPA.CCO.DAL.TrasnferXMLDAL.TransferenciaCabecera(DBComun.Estado.verdadero);
        //    List<TransferXML> _lista = CEPA.CCO.DAL.TrasnferXMLDAL.Transferencia(DBComun.Estado.verdadero);
        //    //using (XmlWriter xml = XmlWriter.Create(sb))
        //    using (StringWriter str = new StringWriter())
        //    using (XmlTextWriter xml = new XmlTextWriter(str))
        //    {
        //        xml.WriteStartDocument();

        //        xml.Formatting = Formatting.Indented;
        //        xml.Indentation = 5;
                

        //        xml.WriteStartElement("MDS4S");
        //        xml.WriteStartElement("MDS4");

        //        foreach (var enca in _lista)
        //        {                   
        //           _clase.year = enca.year;
        //           _clase.aduana = enca.aduana;
        //           _clase.nmanifiesto = enca.nmanifiesto;                                 

        //            break;
        //        }

                
        //        xml.WriteElementString("CAR_REG_YEAR", _clase.year);
        //        xml.WriteElementString("KEY_CUO", _clase.aduana);
        //        xml.WriteElementString("CAR_REG_NBER", _clase.nmanifiesto);

        //        xml.WriteStartElement("MDS6S");
        //        foreach (var item in _pList)
        //        {
                   
        //            xml.WriteStartElement("MDS6");

        //            xml.WriteElementString("KEY_BOL_REF", item.nbl);

        //            xml.WriteStartElement("MDS7S");
        //            foreach (var itemC in _lista.Where(a => a.nbl == item.nbl).Distinct())
        //            {
                        
        //                xml.WriteStartElement("MDS7");
        //                xml.WriteElementString("CAR_DIS_DATE", itemC.f_rpatio);
        //                xml.WriteElementString("CAR_CTN_IDENT", itemC.contenedor);
        //                xml.WriteElementString("CARBOL_SHP_MARK78", itemC.sitio);
        //                WriteFullElementString(xml, "CARBOL_SHP_MARK90", itemC.comentarios);                        
        //                xml.WriteEndElement();
                       
                       
        //            }

        //            xml.WriteEndElement();
        //            xml.WriteEndElement();
                   
        //        }
        //    xml.WriteEndElement();
        //    xml.WriteEndElement();
        //    xml.WriteEndElement();
        //    xml.WriteEndDocument();
        //    _Aduna = str.ToString();
        //    xml.Flush();      

        //    }
            

        // //   x.LoadXml(sb.ToString());

        //    string resultado = _proxu.updateCepaData(_Aduna);

        //    string mensaje = null;
        //    if (resultado.Substring(0, 1) == "1")
        //        mensaje = "Satisfactoria";
        //    else
        //        mensaje = "Error";

        //    return x;
        //}
        #endregion

    }
}
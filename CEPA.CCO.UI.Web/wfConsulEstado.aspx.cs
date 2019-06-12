using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Configuration;

namespace CEPA.CCO.UI.Web
{
    public partial class wfConsulEstado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Datepicker.Text = DateTime.Now.Year.ToString();
               
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

        protected void btnReg_Click(object sender, EventArgs e)
        {
            try 
            { 
            b_autorizado.Text = "";
            n_declaraciones.Text = "";
            string _resultado = null;

            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = new UTF8Encoding(false);
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
            xmlWriterSettings.Indent = true;

            XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

            
            string _Aduana = null;
            string _cadena = null;

            string _autorizacion = null;
            int b_auto;

            xml.WriteStartDocument();

            xml.WriteStartElement("MDS4");

            xml.WriteElementString("CAR_REG_YEAR", Datepicker.Text);
            xml.WriteElementString("KEY_CUO", "02");
            xml.WriteElementString("CAR_REG_NBER", txtMani.Text);            
            xml.WriteElementString("CAR_CTN_IDENT", txtContenedor.Text);

            xml.WriteEndDocument();
            xml.Flush();
            xml.Close();

            //Generar XML para enviar parametros al servicio.
            _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

            XmlDocument doc = new XmlDocument();

            
            CepaSW.WSManifiestoCEPAClient _proxySW = new CepaSW.WSManifiestoCEPAClient();

            string _usuario = ConfigurationManager.AppSettings["userSidunea"];
            string _pass = ConfigurationManager.AppSettings["pswSidunea"];

            _proxySW.ClientCredentials.UserName.UserName = _usuario;
            _proxySW.ClientCredentials.UserName.Password = _pass;

            _resultado = _proxySW.getDocumentoInfoDocumento(_Aduana);

            if (_resultado.Substring(0, 1) == "1")
            {
                _cadena = _resultado.Remove(0, 2);
                doc.LoadXml(_cadena);
                XmlNodeList listaH = doc.SelectNodes("MdsParts/MDS4S/MDS4");

                XmlNode _cabecera;

                _cabecera = listaH.Item(0);
                b_auto = Convert.ToInt32(_cabecera.SelectSingleNode("DESP_CEPA").InnerText);

                if (b_auto == 1)
                    b_autorizado.Text = "AUTORIZADO |" + string.Concat("# Manifiesto: ", Datepicker.Text, '-', txtMani.Text, " # Contenedor: ", txtContenedor.Text);
                else
                    b_autorizado.Text = "DENEGADO |" + string.Concat("# Manifiesto: ", Datepicker.Text, '-', txtMani.Text, " # Contenedor: ", txtContenedor.Text);

                XmlNodeList listaD = doc.SelectNodes("MdsParts/MDS4S/MDS4/MDS5S/MDS5");

                XmlNode _detalle;

                for (int i = 0; i < listaD.Count; i++)
                {
                    _detalle = listaD.Item(i);

                    _autorizacion = _detalle.SelectSingleNode("NUM_DOC").InnerText.Remove(4, 3);
                    n_declaraciones.Text = n_declaraciones.Text + string.Concat(_autorizacion, "/");

                }


               

               
            }
            else
            {
                b_autorizado.Text = "DENEGADO |" + string.Concat("# Manifiesto: ", Datepicker.Text, '-', txtMani.Text, " # Contenedor: ", txtContenedor.Text);
            }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
             
        }
        
    }
}
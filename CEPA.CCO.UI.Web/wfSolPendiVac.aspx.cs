using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using CEPA.CCO.Linq;
using System.Threading;
using System.Globalization;
using System.Drawing;

using Newtonsoft.Json;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Text;
using System.Net;

namespace CEPA.CCO.UI.Web
{
    public partial class wfSolPendiVac : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string getSoli()
        {
            try
            {
             
                List<Vaciado> pSolis = new List<Vaciado>();


                pSolis = (from a in VaciadoDAL.getPendiSolic(DBComun.Estado.verdadero)
                           select new Vaciado
                           {
                              IdTipoVa = a.IdTipoVa,
                              t_solicitud = a.t_solicitud,
                              num_mani = a.num_mani,
                              n_contenedor = a.n_contenedor,
                              bl_master = a.bl_master,
                              n_contacto = a.n_contacto,
                              t_contacto = a.t_contacto,
                              e_contacto = a.e_contacto,
                              f_registro = a.f_registro,
                              t_retencion = a.t_retencion
                           }).ToList();

                var query = Newtonsoft.Json.JsonConvert.SerializeObject(pSolis);

                return query;


            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }
        }

        [System.Web.Services.WebMethod]
        public static string getBLes(string n_contenedor, string n_manifiesto)
        {
            try
            {

                string[] mani = n_manifiesto.Split('-');

                insertMani(mani[1].ToString(), mani[0].ToString());


                List<Vaciado> pDescon = new List<Vaciado>();


                pDescon = VaciadoDAL.getBLVaciado(DBComun.Estado.verdadero, n_contenedor, n_manifiesto);

                var query = Newtonsoft.Json.JsonConvert.SerializeObject(pDescon);

                return query;


            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }
        }
        public static string EscapeXMLValue(string xmlString)
        {

            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

            return xmlString.Replace("'", "&apos;").Replace("\"", "&quot;").Replace("(", "").Replace(")", "").Replace("&", "&amp;");
        }


        [System.Web.Services.WebMethod]
        public static void insertMani(string n_manifiesto, string a_manifiesto)
        {
            
            List<string> pListaNo = new List<string>();
            List<string> pRespuesta = new List<string>();
            int _resulDeta = 0;
            int b_sidunea = 1;
            string _mensaje = null;

            string resul = ResulNavieraDAL.EliminarManifiesto(DBComun.Estado.verdadero, Convert.ToInt32(n_manifiesto), a_manifiesto, b_sidunea);
            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = new UTF8Encoding(false);
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
            xmlWriterSettings.Indent = true;

            XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

            string _Aduana = null;

            xml.WriteStartDocument();

            xml.WriteStartElement("MDS4");

            xml.WriteElementString("CAR_REG_YEAR", a_manifiesto);
            xml.WriteElementString("KEY_CUO", "02");
            xml.WriteElementString("CAR_REG_NBER", n_manifiesto.ToString());

            xml.WriteEndDocument();
            xml.Flush();
            xml.Close();

            //Generar XML para enviar parametros al servicio.
            _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

            XmlDocument doc = new XmlDocument();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;


            if (b_sidunea == 0)
            {
                CEPAService.WSManifiestoCEPAClient _proxy = new CEPAService.WSManifiestoCEPAClient();
                string s = EscapeXMLValue(_proxy.getContenedorData(_Aduana));
                //s = UnescapeXMLValue(s);
                doc.LoadXml(s);
            }
            else
            {
                CepaSW.WSManifiestoCEPAClient _proxy = new CepaSW.WSManifiestoCEPAClient();
                string _user = ConfigurationManager.AppSettings["userSidunea"];
                string _psw = ConfigurationManager.AppSettings["pswSidunea"];



                _proxy.ClientCredentials.UserName.UserName = _user;
                _proxy.ClientCredentials.UserName.Password = _psw;

                _mensaje = _proxy.getContenedorData(_Aduana);
                if (_mensaje.Substring(0, 1) == "0")
                    pRespuesta.Add("PASO 3 de 4: Validación ADUANA: EL MANIFIESTO # " + string.Concat(a_manifiesto, "-", n_manifiesto) + " NO PRODUJO RESULTADOS");
                else
                    doc.LoadXml(_mensaje);
            }

            if (doc.ChildNodes.Count > 0)
            {
                XmlNodeList listEnca = doc.SelectNodes("MdsParts/MDS4");

                XmlNode xEnca;

                xEnca = listEnca.Item(0);

                string _amani = xEnca.SelectSingleNode("CAR_REG_YEAR").InnerText;
                string _nmani = xEnca.SelectSingleNode("CAR_REG_NBER").InnerText;

                XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4/MDS5");

                XmlNode unContenedor;

                List<ArchivoAduanaValid> pGuarda = new List<ArchivoAduanaValid>();

                //Contenedore devueltos por ADUANA
                if (listaCntres.Count > 0)
                {

                    for (int i = 0; i < listaCntres.Count; i++)
                    {
                        unContenedor = listaCntres.Item(i);

                        string _contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText;


                        ArchivoAduanaValid validAduana = new ArchivoAduanaValid
                        {
                            IdValid = -1,
                            n_contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText.Replace("-", ""),
                            n_manifiesto = Convert.ToInt32(_nmani),
                            n_BL = unContenedor.SelectSingleNode("KEY_BOL_REF").InnerText,
                            a_mani = _amani,
                            c_tipo_bl = unContenedor.SelectSingleNode("CARBOL_TYP_COD").InnerText,
                            b_sidunea = b_sidunea,
                            c_tamaño = unContenedor.SelectSingleNode("CAR_CTN_TYP").InnerText,
                            s_agencia = unContenedor.SelectSingleNode("CAR_CAR_NAM").InnerText,
                            v_peso = Convert.ToDouble(unContenedor.SelectSingleNode("CAR_CTN_GWG").InnerText),
                            c_paquete = Convert.ToInt32(unContenedor.SelectSingleNode("CAR_CTN_NBR").InnerText),
                            c_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_COD").InnerText,
                            d_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_NAM").InnerText,
                            c_pais_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(0, 2),
                            d_puerto_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(2, 3),
                            c_pais_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(0, 2),
                            d_puerto_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(2, 3),
                            s_nit = unContenedor.SelectSingleNode("CARBOL_CON_COD") != null ? unContenedor.SelectSingleNode("CARBOL_CON_COD").InnerText : "",
                            s_consignatario = unContenedor.SelectSingleNode("CARBOL_CON_NAM") != null ? unContenedor.SelectSingleNode("CARBOL_CON_NAM").InnerText : ""
                        };

                        //Almacenar manifiesto devuelto por aduana
                        _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValid(validAduana, DBComun.Estado.verdadero));

                        if (_resulDeta == 2)
                            pListaNo.Add(validAduana.n_contenedor);
                    }


                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.BL;
using CEPA.CCO.Linq;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;

namespace CEPA.CCO.App.TransferMani
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static string EscapeXMLValue(string xmlString) 
        {

            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

            return xmlString.Replace("'","&apos;").Replace("\"", "&quot;").Replace("(","").Replace(")","").Replace("&","&amp;"); 
        } 

        public static string UnescapeXMLValue(string xmlString) 
        {
            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

        return xmlString.Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&"); 
        } 


        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> pListaNo = new List<string>();
                List<string> pRespuesta = new List<string>();
                int _resulDeta = 0;
                int b_sidunea = cbSidunea.IsChecked == true ? 1 : 0;
                string _mensaje = null;

                List<ContenedoresAduana> pListManis = new List<ContenedoresAduana>();

                pListManis = ResulNavieraDAL.ObtenerManiYear(DBComun.Estado.falso, Convert.ToInt32(txtYear.Text));

                if(cbActAll.IsChecked == true){
                    foreach (var iMani in pListManis)
                    {
                        insertMani(pListaNo, pRespuesta, ref _resulDeta, iMani.b_sidunea, ref _mensaje, iMani.n_manifiesto, iMani.a_manifiesto);
                    }                    
                }
                else
                {
                    insertMani(pListaNo, pRespuesta, ref _resulDeta, b_sidunea, ref _mensaje, Convert.ToInt32(txtMani.Text), txtYear.Text);
                }

                MessageBox.Show("Satisfactorio!");
                txtMani.Text = "";
                txtYear.Text = "";
                cbSidunea.IsChecked = false;
                cbActAll.IsChecked = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void insertMani(List<string> pListaNo, List<string> pRespuesta, ref int _resulDeta, int b_sidunea, ref string _mensaje, int n_manifiesto, string a_manifiesto)
        {
            //string resul = ResulNavieraDAL.EliminarManifiesto(DBComun.Estado.falso, n_manifiesto, a_manifiesto, b_sidunea);

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

            if (b_sidunea == 0)
            {
                CepaWebService.WSManifiestoCEPAClient _proxy = new CepaWebService.WSManifiestoCEPAClient();
                string s = EscapeXMLValue(_proxy.getContenedorData(_Aduana));
                //s = UnescapeXMLValue(s);
                doc.LoadXml(s);
            }
            else
            {
                CepaSiduneaWorld.WSManifiestoCEPAClient _proxy = new CepaSiduneaWorld.WSManifiestoCEPAClient();
                string _user = "integracion.mancepa02";
                string _psw = "2mOHV2uje3YqJ*";

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
                        _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValid(validAduana, DBComun.Estado.falso));

                        if (_resulDeta == 2)
                            pListaNo.Add(validAduana.n_contenedor);
                    }


                }
            }
        }

        private void FrmTransfer_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}

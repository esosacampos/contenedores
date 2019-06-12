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
using System.Windows.Shapes;
using System.Xml;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;

namespace CEPA.CCO.App.TransferMani
{
    /// <summary>
    /// Lógica de interacción para Cargar.xaml
    /// </summary>
    public partial class Cargar : Window
    {
        public Cargar()
        {
            InitializeComponent();
        }

        private void btnCargar_Click(object sender, RoutedEventArgs e)
        {
            int _resulDeta = 0;
            List<string> pListaNo = new List<string>();
            XmlDocument xDoc = new XmlDocument();

            //La ruta del documento XML permite rutas relativas 
            //respecto del ejecutable!

            xDoc.LoadXml(txtBloque.Text);

            XmlNodeList listaCntresE = xDoc.SelectNodes("MdsParts/MDS4");
            XmlNode xEnca;

            xEnca  = listaCntresE.Item(0);
            string _amani = xEnca.SelectSingleNode("CAR_REG_YEAR").InnerText;
            string _nmani = xEnca.SelectSingleNode("CAR_REG_NBER").InnerText;

            string resul = ResulNavieraDAL.EliminarManifiesto(DBComun.Estado.falso, Convert.ToInt32(_nmani), _amani, 1);

            XmlNodeList listaCntres = xDoc.SelectNodes("MdsParts/MDS4/MDS5");

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
                                        b_sidunea = 1,
                                        c_tamaño = unContenedor.SelectSingleNode("CAR_CTN_TYP").InnerText,
                                        s_agencia = unContenedor.SelectSingleNode("CAR_CAR_NAM").InnerText,
                                        v_peso = Convert.ToDouble(unContenedor.SelectSingleNode("CAR_CTN_GWG").InnerText),
                                        c_paquete = Convert.ToInt32(unContenedor.SelectSingleNode("CAR_CTN_NBR").InnerText),
                                        c_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_COD").InnerText,
                                        d_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_NAM").InnerText,
                                        c_pais_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(0, 2),
                                        d_puerto_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(2, 3),
                                        c_pais_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(0, 2),
                                        d_puerto_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(2, 3)
                                    };

                                    //Almacenar manifiesto devuelto por aduana
                                    _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValid(validAduana, DBComun.Estado.falso));

                                    if (_resulDeta == 2)
                                        pListaNo.Add(validAduana.n_contenedor);
                                }
                            }

            MessageBox.Show("Registrado Satisfactoriamente Manifiesto #" + _amani + "-"+ _nmani);
            txtBloque.Text = "";
        }

        private void TxtBloque_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

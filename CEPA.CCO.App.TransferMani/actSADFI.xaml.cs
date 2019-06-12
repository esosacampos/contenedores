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

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.BL;
using CEPA.CCO.Linq;

namespace CEPA.CCO.App.TransferMani
{
    /// <summary>
    /// Lógica de interacción para actSADFI.xaml
    /// </summary>
    public partial class actSADFI : Window
    {
        public actSADFI()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<TransferXML> pLista = new List<TransferXML>();

                string[] manifiesto = txtMani.Text.Split('-');

                int a_mani = Convert.ToInt32(manifiesto[0]);
                int n_mani = Convert.ToInt32(manifiesto[1]);

                pLista = TrasnferXMLDAL.listaSADFI_Mani(DBComun.Estado.falso, a_mani, n_mani);

                //int sadfi_res = Convert.ToInt32(DetaNavieraDAL.ActSADFI_AMP(pLista, DBComun.Estado.falso));

                MessageBox.Show("Actualización Exitosa");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

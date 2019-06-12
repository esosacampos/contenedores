using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using System.Timers;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Globalization;
using CEPA.CCO.Linq;
using System.Xml;
using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;

namespace CEPA.CCO.ServMONITOR
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer tmpService = null;
        bool EnProceso = false;
        string Archivo = Application.StartupPath + "\\CEPA_CCO_MONITOR.TXT";
        string bSize = Application.StartupPath + "\\CEPA_CCO_SIZE.TXT";
        string bRead = @"C:\Archivos de programa\Default Company Name\CEPA.CCO.InstallADUANA\CEPA_CCO_ADUANA.TXT";
        Linq.NotiNavieras _notiNavieras = new Linq.NotiNavieras();
        bool Error = false;

        public Service1()
        {
            InitializeComponent();
            tmpService = new System.Timers.Timer(300000);
            tmpService.Elapsed += new ElapsedEventHandler(tmpService_Elapsed);
        }

        private static void EnvioServicio(string mensaje, string asunto)
        {
            EnvioCorreo _correo = new EnvioCorreo();
            string Html = null;

            _correo.Subject = asunto;

            Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
            Html += string.Format("<b><u> {0} </b></u><br />", mensaje);


            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_cancela", DBComun.Estado.falso, "0"); ;

            _correo.Asunto = Html;
            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.falso);
        }

        protected override void OnStart(string[] args)
        {
            TextWriter tw = new StreamWriter(Archivo, true);
            try
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ===>> INICIO DE SERVICIO <<===");
                tw.Flush();
                tw.Dispose();
                tw.Close();
                tmpService.Interval = 300000;

                EnvioServicio("MONITOR DE SERVICIO DE ADUANA SE INICIO", "SERVICIO DE ADUANA SE INICIO");

                tmpService.Enabled = true;
            }
            catch (Exception ex)
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDO INICIAR EL SERVICIO (" + ex.Message + ")");
                tw.Flush();
                tw.Dispose();
                tw.Close();
                tmpService.Enabled = false;

                EnvioServicio("MONITOR DE SERVICIO DE ADUANA SE DETUVO", ex.Message);

                this.Stop();
            }
        }

        protected override void OnStop()
        {
            TextWriter tw = new StreamWriter(Archivo, true);
            tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ====> FIN DE PROCESO <====" + "\n\n");
            tw.Flush();
            tw.Dispose();
            tw.Close();

            EnvioServicio("MONITOR DE SERVICIO DE ADUANA SE DETUVO", "SERVICIO DE ADUANA SE DETUVO");


            tmpService.Enabled = false;
        }

        void tmpService_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (EnProceso == true)
                return;


            EnProceso = true;



            EnProceso = false;

        }

        private void valiArchivo()
        {
            TextWriter tw = new StreamWriter(Archivo, true);
            

            try
            {
                StreamReader _sReader = new StreamReader(bSize);
                string line = _sReader.ReadLine();
                _sReader.Close();

                FileInfo _fInfo = new FileInfo(bRead);
                long s1 = _fInfo.Length;

                if(s1 > Convert.ToInt64(line))
                {
                    File.WriteAllText(bSize, s1.ToString(), Encoding.UTF8);

                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": SERVICIO DE ADUANA ARRIBA TAMAÑO: " + s1.ToString());
                }
                else
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": SERVICIO DE ADUANA CON PROBLEMAS TAMAÑO ACTUAL: " + s1.ToString() + " TAMAÑO ANTERIOR: " + line );
                }

                tw.Flush();
                tw.Dispose();
                tw.Close();

            }
            catch (Exception ex)
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: " + ex.Message + " " + ex.Source + " " + ex.StackTrace + " " + ex.TargetSite);
                tw.Flush();
                tw.Dispose();
                tw.Close();
                this.Stop();
            }
        }
    }
}

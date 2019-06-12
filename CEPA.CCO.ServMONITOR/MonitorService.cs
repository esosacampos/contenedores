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
using System.Threading;

namespace CEPA.CCO.ServMONITOR
{
    public partial class MonitorService : ServiceBase
    {
        System.Timers.Timer tmpService = null;
        bool EnProceso = false;
        string Archivo = Application.StartupPath + "\\CEPA_CCO_MONITOR.TXT" ;
        string bSize = Application.StartupPath + "\\CEPA_CCO_SIZE.TXT";
        //string bRead = @"C:\Program Files (x86)\CEPA Acajutla\CEPA.CCO.IAduaServi\CEPA_CCO_ADUANA.TXT";
        string bRead = @"C:\Program Files (x86)\CEPA Acajutla\CEPA.CCO.IAduaServi\CEPA_CCO_ADUANA_" + DateTime.Now.ToString("MMyy", CultureInfo.CreateSpecificCulture("es-SV")) +".TXT";
        
        Linq.NotiNavieras _notiNavieras = new Linq.NotiNavieras();
        bool Error = false;
        const int RestartTimeout = 10000;

        public MonitorService()
        {
            InitializeComponent();
            //tmpService.AutoReset = true;         
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
            //System.Diagnostics.Debugger.Launch();
           
            try
            {
                
                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ===>> INICIO DE SERVICIO <<===");
                }
                tmpService.Interval = 300000;
                

                EnvioServicio("MONITOR DE SERVICIO DE ADUANA SE INICIO", "MONITOR DE SERVICIO DE ADUANA SE INICIO");


                tmpService.Enabled = true;
               

            }
            catch (Exception ex)
            {
                using (StreamWriter tw = new StreamWriter(Archivo, true))
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDO INICIAR EL SERVICIO (" + ex.Message + ")");
                }

                tmpService.Enabled = false;

                //EnvioServicio("MONITOR DE SERVICIO DE ADUANA SE DETUVO", ex.Message);

                this.Stop();
            }
        }

        protected override void OnStop()
        {
            
            using (StreamWriter tw = new StreamWriter(Archivo, true))
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ====> FIN DE PROCESO <====" + "\n\n");
            }

            EnvioServicio("MONITOR SERVICIO DE ADUANA SE DETUVO", "MONITOR SERVICIO DE ADUANA SE DETUVO");


            tmpService.Enabled = false;
            tmpService.Stop();
        }

        private void tmpService_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            
           
                if (EnProceso == true)
                    return;

                EnProceso = true;



                try
                {
                    valiArchivo();
                }
                catch (Exception ex)
                {

                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: " + ex.Message + " " + ex.Source + " " + ex.StackTrace + " " + ex.TargetSite);
                    }
                    this.Stop();
                    return;
                }

                EnProceso = false;
            
            
           

            //EnProceso = false;
            

        }

        private void valiArchivo()
        {

            string line = null;
            try
            {
                using (StreamReader sr = new StreamReader(bSize))
                {

                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while (sr.Peek() > -1)
                    {

                        line = sr.ReadLine();
                        if (line != null)
                            break;
                    }
                }

                long s1;
                using (var file = new FileStream(bRead, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    s1 = file.Length;
                }



                if (s1 > Convert.ToInt64(line))
                {
                    File.WriteAllText(bSize, s1.ToString(), Encoding.UTF8);

                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": SERVICIO DE ADUANA ARRIBA TAMAÑO: " + s1.ToString());
                    }
                }
                else
                {

                    /* foreach (var process in Process.GetProcessesByName("CEPA.CCO.AduanaService"))
                     {
                         process.Kill();
                     }*/



                    ServiceController sc = new ServiceController("AduanaTransfer");
                    sc.Refresh();
                    TimeSpan timeout = TimeSpan.FromMinutes(1);

                    if (sc != null && sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();

                    }

                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    sc.Close();


                    using (StreamWriter tw = new StreamWriter(Archivo, true))
                    {
                        tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": SERVICIO DE ADUANA SE RESTAURO TAMAÑO ACTUAL: " + s1.ToString() + " TAMAÑO ANTERIOR: " + line);
                    }

                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using System.Timers;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Globalization;

namespace CEPA.CCO.Service
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer tmpService = null;
        bool EnProceso = false;
        string Archivo = Application.StartupPath + "\\CEPA_CONTENEDORES_SERVICIO.TXT";
        Linq.NotiNavieras _notiNavieras = new Linq.NotiNavieras();

        public Service1()
        {
            InitializeComponent();
            tmpService = new System.Timers.Timer(60000);
            tmpService.Elapsed += new ElapsedEventHandler(tmpService_Elapsed);
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
                tmpService.Interval = 60000;
                tmpService.Enabled = true;
            }
            catch (Exception ex)
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: NO SE PUEDO INICIAR EL SERVICIO (" + ex.Message + ")" );
                tw.Flush();
                tw.Dispose();
                tw.Close();
                tmpService.Enabled = false;
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
            tmpService.Enabled = false;
        }

        void tmpService_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (EnProceso == true)
                return;


            EnProceso = true;

            TextWriter tw = new StreamWriter(Archivo, true);
            CEPA.CCO.Linq.NotiNavieras _procesos = new Linq.NotiNavieras();


            try
            {
                int _resultado = _notiNavieras.ProcesarCorreo(CEPA.CCO.DAL.DBComun.Estado.falso);
                if (_resultado > 0)
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ENVIADO: CORREO CON CABECERA No. " + _resultado.ToString() );
                    tw.Flush();
                    tw.Dispose();
                    tw.Close();
                }
                else
                {
                    tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ENVIADO: SERVICIO EJECUTADO ");
                    tw.Flush();
                    tw.Dispose();
                    tw.Close();
                }
            }         
            catch (Exception ex)
            {
                tw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV")) + ": ERROR: " + ex.Message + " " + ex.Source + " " + ex.StackTrace + " " + ex.TargetSite );
                tw.Flush();
                tw.Dispose();
                tw.Close();
                this.Stop();
            }
            


            

            EnProceso = false;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace CEPA.CCO.AduanaService
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
//#if DEBUG
//                System.Diagnostics.Debugger.Launch();
//#endif
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new AduanaTransfer() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

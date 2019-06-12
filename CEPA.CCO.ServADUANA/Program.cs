using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace CEPA.CCO.ServADUANA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
//#if DEBUG
//            System.Diagnostics.Debugger.Launch();
//#endif
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new AduanaService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}

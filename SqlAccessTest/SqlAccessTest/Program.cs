using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SqlAccessTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            if (Environment.UserInteractive)
            {
                var service = new Service1(true);
                service.CmdlineStart();
            }
            else
            {
                ServicesToRun = new ServiceBase[]
                {
                    new Service1(false)
                };

                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

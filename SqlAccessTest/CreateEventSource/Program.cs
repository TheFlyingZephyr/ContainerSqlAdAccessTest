using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateEventSource
{
    class Program
    {
        const string _eLogName = "DbAccessTest";

        static void Main(string[] args)
        {
            try
            {
                var el = new EventLog("Application") {Source = _eLogName};
                el.WriteEntry("Successfully created '" + _eLogName + "' event source.", EventLogEntryType.Information, 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

        }
    }
}

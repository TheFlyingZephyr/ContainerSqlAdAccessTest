using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SqlAccessTest
{
    public partial class Service1 : ServiceBase
    {
        const string _filePath = @"C:\\Data\DbAccessTest.txt";
        const string _eLogName = "DbAccessTest";
        Thread workerThread = null;
        static EventLog _EventLog = null;

        public Service1()
        {
            _EventLog = new EventLog("Application") {Source = _eLogName};
            TestDbAccess();

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _EventLog.WriteEntry("DbAccessTest Service Starting", EventLogEntryType.Information, 1, 1);

            workerThread = new Thread(Woker);
            workerThread.Start();
        }

        protected override void OnStop()
        {
            workerThread?.Abort();
        }

        private void Woker()
        {
            TestDbAccess();
        }

        void TestDbAccess()
        {
            string server = "";

            try
            {
                using (var dbC = new MyDbContext())
                {
                    server = dbC.Database.Connection.DataSource ?? "---";
                    var res1 = from rows in dbC.Table1 select rows;
                    var res2 = res1.ToList();
                    //File.WriteAllText(_filePath, "SQL Access Test Passed");
                    _EventLog.WriteEntry("SQL Access (" + server + ") Test Passed", EventLogEntryType.Information, 1, 1);
                }

            }
            catch (Exception ex)
            {
                //File.WriteAllText(_filePath, "SQL Access Test Failed : " + UnwindExceptionMessages(ex));
                _EventLog.WriteEntry("SQL Access (" + server + ") Test Failed : " + UnwindExceptionMessages(ex), EventLogEntryType.Error, 2, 1);
                return;
            }

        }

        public static string UnwindExceptionMessages(Exception ex)
        {
            var e = ex;
            var s = new StringBuilder();
            while (e != null)
            {
                s.AppendLine(e.Message + " : ");
                e = e.InnerException;
            }
            return s.ToString();
        }

    }
}

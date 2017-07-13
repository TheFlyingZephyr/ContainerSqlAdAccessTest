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
        private bool _IsCmd = false;

        public Service1(bool isCmdline)
        {
            _IsCmd = isCmdline;

            if (_IsCmd)
                Console.WriteLine("INFO: Try to access eventlog");

            try
            {
                _EventLog = new EventLog("Application") {Source = _eLogName};

                _EventLog.WriteEntry("Able to access eventlog", EventLogEntryType.Information, 1, 1);

                if (_IsCmd)
                    Console.WriteLine("INFO: Able to access eventlog");

                //TestDbAccess();
            }
            catch (Exception e)
            {
                if (_IsCmd)
                    Console.WriteLine("EXCEPTION: Unable to access eventlog : " + UnwindExceptionMessages(e));

                throw;
            }

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (_IsCmd)
            {
                Console.WriteLine("INFO: DbAccessTest Service Starting");
                TestDbAccess();
                return;
            }

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

                    if (_IsCmd)
                        Console.WriteLine("INFO: SQL Access(" + server + ") Test Passed");

                    _EventLog.WriteEntry("SQL Access (" + server + ") Test Passed", EventLogEntryType.Information, 1, 1);
                }

            }
            catch (Exception ex)
            {
                //File.WriteAllText(_filePath, "SQL Access Test Failed : " + UnwindExceptionMessages(ex));
                if (_IsCmd)
                    Console.WriteLine("EXCEPTION: SQL Access(" + server + ") Test Failed : " + UnwindExceptionMessages(ex));

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

        public void CmdlineStart()
        {
            OnStart(new string[0]);
        }
    }
}

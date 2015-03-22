using System;
using System.IO;
using UserManagementApplication.Common.Diagnostics.Interfaces;
using UserManagementApplication.Common.Exceptions;

namespace UserManagementApplication.Common.Diagnostics
{
    public class DefaultLogProvider : ILogProvider
    {
        private const string LOGFILE = "UserManagementApplication.Engine.log";

        public void LogMessage(string message)
        {
            writeToLog(message);
        }

        public void LogMessage(UserManagementApplicationException ex)
        {
            writeToLog(String.Format("[{0}] {1}", ex.ErrorSeverity, ex.Message));
        }

        public void LogMessage(Exception ex)
        {
            writeToLog(String.Format("{1}\n[StackTrace]: {2}", ex.Message, ex.StackTrace));
        }

        private void writeToLog(string message)
        {
            using (var streamWriter = new StreamWriter(LOGFILE, true))
            {
                streamWriter.WriteLine(String.Format("[{0}]: {1}", DateTime.Now, message));
            }
        }
    }
}

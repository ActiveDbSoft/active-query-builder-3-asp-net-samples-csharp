using System;
using System.Web;
using ActiveQueryBuilder.Web.Server.Handlers;
using log4net;

namespace WebForms_Samples
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            BaseHandler.Log = new Log();
        }

        void Application_Error(object sender, EventArgs e)
        {
            var s = Server.GetLastError();
        }
    }

    public class Log : ActiveQueryBuilder.Core.ILog
    {
        private static readonly ILog _Log = LogManager.GetLogger("Logger");

        public void Trace(string message)
        {
            _Log.Info(message);
        }

        public void Warning(string message)
        {
            _Log.Warn(message);
        }

        public void Error(string message)
        {
            _Log.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            _Log.Error(message, ex);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentAlertEmail
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));
       
        static void Main(string[] args)
        {
            log.Info("====== Start Process WOLF_SentAlertEmail ====== : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            log.Info(string.Format("Run batch as :{0}", System.Security.Principal.WindowsIdentity.GetCurrent().Name));

            Services.sentAlertEmail();

        }
    }
}

using SendAlertEmail.Extensions;
using SendAlertEmail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendAlertEmail
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));
       
        static void Main(string[] args)
        {
            WriteLogFile.writeLogFile("====== Start Process WOLF_SendAlertEmail ====== : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            WriteLogFile.writeLogFile(string.Format("Run batch as :{0}", System.Security.Principal.WindowsIdentity.GetCurrent().Name));
            Console.WriteLine("Start Process WOLF_SendAlertEmail : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            //string a = "welovelove93@gmail.com";
            //string b = "TestingForEmail";
            //string c = "BodyTest";
            //SendEmail.sendEmail(c, a, b);
            Services.sendAlertEmail();

        }
    }
}

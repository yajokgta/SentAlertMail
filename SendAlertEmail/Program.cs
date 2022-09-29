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
        static void Main(string[] args)
        {
            //---For TestSendEmail Functions----//
            //string a = "test@gmail.com";
            //string b = "TestingForEmail";
            //string c = "BodyTest";
            //SendEmail.SendEmailTemplate(c, b, a,"");
            //---For TestSendEmail Functions----//

            WriteLogFile.writeLogFile("====== Start Process WOLF_SendAlertEmail ======");
            WriteLogFile.writeLogFile(string.Format("Run batch as :{0}", System.Security.Principal.WindowsIdentity.GetCurrent().Name));

            Console.WriteLine("Start Process WOLF_SendAlertEmail : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Console.WriteLine("Wait For PROCESS... : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            Services.sendAlertEmail();
            WriteLogFile.writeLogFile("======= Stop Process WOLF_SendAlertEmail =======");
            Console.WriteLine("SendEmail Success : "+DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Console.WriteLine("Exit.. : "+ DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Environment.Exit(0);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendAlertEmail.Extensions
{
    class WriteLogFile
    {
        public static string _LogFile = System.Web.Configuration.WebConfigurationManager.AppSettings["LogFile"];
        public static void writeLogFile(String iText)
        {

            String LogFilePath = String.Format("{0}{1}_Log.txt", _LogFile, DateTime.Now.ToString("yyyyMMdd"));

            try
            {
                using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(LogFilePath, true))
                {
                    System.Text.StringBuilder sbLog = new System.Text.StringBuilder();

                    //sbLog.AppendLine(String.Empty);
                    //sbLog.AppendLine(String.Format("--------------- Start Time ({0}) ---------------", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));

                    String[] ListText = iText.Split('|').ToArray();

                    foreach (String s in ListText)
                    {
                        sbLog.AppendLine(s);
                    }

                    //sbLog.AppendLine(String.Format("--------------- End Time ({0})   ---------------", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                    //sbLog.AppendLine(String.Empty);

                    outfile.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("dd:MM/yyyy HH:mm:ss"), sbLog.ToString()));
                }
            }
            catch (Exception ex) { }
        }
    }
}

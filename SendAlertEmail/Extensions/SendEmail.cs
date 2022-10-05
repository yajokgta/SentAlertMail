using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WolfApprove.Model.CustomClass;
using WolfApprove.API2.Controllers.Services;
using System.Text.RegularExpressions;

namespace SendAlertEmail.Extensions
{
    public class SendEmail
    {
        
        public static string _SMTPServer = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPServer"];
        public static string _SMTPPort = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPPort"];
        public static string _SMTPEnableSSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPEnableSSL"];
        public static string _SMTPUser = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPUser"];
        public static string _SMTPPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPPassword"];
        public static string _DisplayName = System.Web.Configuration.WebConfigurationManager.AppSettings["DisplayName"];

        //public static string sendEmail(string Body, string MailTo, string subject)
        //{
        //    var result = "";
        //    try
        //    {
        //        SmtpClient smtpClient = new SmtpClient();
        //        NetworkCredential basicCredential = new NetworkCredential(_SMTPUser, _SMTPPassword);
        //        MailMessage message = new MailMessage();
        //        MailAddress fromAddress = new MailAddress(_SMTPUser, _DisplayName);

        //        smtpClient.Host = _SMTPServer;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = basicCredential;
        //        smtpClient.EnableSsl = Convert.ToBoolean(_SMTPEnableSSL);
        //        smtpClient.Port = Convert.ToInt32(_SMTPPort);

        //        message.From = fromAddress;
        //        //message.CC = sCc;
        //        message.Subject = subject;

        //        //Set IsBodyHtml to true means you can send HTML email.
        //        message.IsBodyHtml = true;

        //        message.Priority = MailPriority.High;
        //        message.Body = Body;
        //        string[] mail = MailTo.Split(',');
        //        foreach (string s in mail)
        //        {
        //            if (s != "")
        //            {
        //                message.Bcc.Add(new MailAddress(s));
        //            }
        //        }

        //        message.Sender = fromAddress;
        //        smtpClient.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //        result = ex.ToString();
        //    }
        //    return result;
        //}
        public static string ReplaceInActiveEmail(String Email, List<CustomMasterData> list_CustomMasterData)
        {
            if (list_CustomMasterData.Count > 0)
            {
                List<String> listTo = Email.Split(';').ToList();
                Email = String.Empty;
                foreach (String s in listTo)
                {
                    var temp = list_CustomMasterData.Where(a => a.Value1.ToUpper() == s.ToUpper()).ToList();
                    if (temp.Count == 0)
                    {
                        if (!String.IsNullOrEmpty(Email))
                        {
                            Email += ";";
                        }
                        Email += s.Trim();
                    }
                }
            }
            return Email;
        }
        public static bool IsValidEmail(String email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static void SendEmailTemplate(string Body, string subject, String To, String CC,List<string> MAdvance)
        {
            DataClasses1DataContext db = new DataClasses1DataContext(Services.dbConnectionString);
            String Subject = subject;
            
            
            To = "";
            string enddate = MAdvance[0];
            string srno1 = MAdvance[1];
            string srno2 = MAdvance[2];
            string srno3 = MAdvance[3];
            string srno4 = MAdvance[4];
            string srno5 = MAdvance[5];
            string sono = MAdvance[6];
            string memoid = MAdvance[7];
            String html = Body;

            MailAddress fromAddress = new MailAddress(_SMTPUser, _DisplayName);
            try
            {
                String tempSMTPServer = _SMTPServer;
                int tempSMTPPort = Convert.ToInt32(_SMTPPort);
                Boolean tempSMTPEnableSsl = Convert.ToBoolean(_SMTPEnableSSL) ;
                String tempSMTPUser = _SMTPUser;
                String tempSMTPPassword = _SMTPPassword;
                String tempSMTPTestMode = "";
                String tempSMTPTo = To;

                List<CustomMasterData> list_CustomMasterData =
                    new MasterDataService().GetMasterDataListInActiveEmail(new CustomMasterData { connectionString = Services.dbConnectionString });
                To = ReplaceInActiveEmail(To, list_CustomMasterData);
                CC = ReplaceInActiveEmail(CC, list_CustomMasterData);
                if (String.IsNullOrEmpty(To))
                {
                    To = CC;
                    CC = String.Empty;
                }

                SmtpClient _smtp = new SmtpClient();
                _smtp.Host = tempSMTPServer;
                _smtp.Port = tempSMTPPort;
                _smtp.EnableSsl = tempSMTPEnableSsl;
                _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                _smtp.UseDefaultCredentials = false;
                _smtp.Timeout = 300000; //5 Minutes Timeout
                if (!String.IsNullOrEmpty(tempSMTPUser) && !String.IsNullOrEmpty(tempSMTPPassword))
                {
                    _smtp.Credentials = new System.Net.NetworkCredential(tempSMTPUser, tempSMTPPassword);
                }
                else
                {
                    _smtp.UseDefaultCredentials = true;
                }

                var mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = fromAddress;
                new System.Net.Mail.MailAddress(tempSMTPUser);

                Boolean blTestMode = false;
                if (!String.IsNullOrEmpty(tempSMTPTestMode))
                {
                    blTestMode = tempSMTPTestMode == "T";
                }
                if (blTestMode)
                {
                    html = $"{html}<br/><br/>To : {To}<br/>CC : {CC}";
                    To = tempSMTPTo;
                    CC = tempSMTPTo;
                }

                if (!String.IsNullOrEmpty(To))
                {

                    if (To.IndexOf(',') > -1)
                    {
                        String[] obj = To.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim().ToLower()).Distinct().ToArray();
                        for (int i = 0; i < obj.Length; i++)
                        {
                            if (obj[i].ToString().Contains("@"))
                            {
                                if (!String.IsNullOrEmpty(obj[i].Trim()))
                                {
                                    IsValidEmail(obj[i].Trim());
                                    mailMessage.To.Add(obj[i].Trim());
                                }
                            }
                            else
                            {
                                string ccname = obj[i].ToString();
                                if (Regex.IsMatch(ccname, "^[a-zA-Z0-9]*$"))
                                {
                                    List<MSTEmployee> objemp = new List<MSTEmployee>();
                                    objemp = db.MSTEmployees.Where(x => x.NameEn == ccname).ToList();
                                    string[] mailvalue = new string[1];
                                    foreach (var empname in objemp)
                                    {
                                        mailvalue[0] = empname.Email.ToString();
                                    }
                                    if (!string.IsNullOrEmpty(mailvalue[0]))
                                    {
                                        IsValidEmail(mailvalue[0].Trim());
                                        mailMessage.To.Add(mailvalue[0].Trim());
                                    }
                                }
                                else
                                {
                                    List<MSTEmployee> objemp = new List<MSTEmployee>();
                                    objemp = db.MSTEmployees.Where(x => x.NameTh == ccname).ToList();
                                    string[] mailvalue = new string[1];
                                    foreach (var empname in objemp)
                                    {
                                        mailvalue[0] = empname.Email.ToString();
                                    }
                                    if (!string.IsNullOrEmpty(mailvalue[0]))
                                    {
                                        IsValidEmail(mailvalue[0].Trim());
                                        mailMessage.To.Add(mailvalue[0].Trim());
                                    }
                                }
                            }
                        }
                    }
                    else if (IsValidEmail(To.Trim()))
                        mailMessage.To.Add(To.Trim());

                    if (!string.IsNullOrEmpty(CC))
                        if (CC.IndexOf(',') > -1)
                        {
                            String[] objcc = CC.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim().ToLower()).Distinct().ToArray();
                            for (int i = 0; i < objcc.Length; i++)
                            {
                                if (objcc[i].ToString().Contains("@"))
                                {
                                    if (!String.IsNullOrEmpty(objcc[i].Trim()))
                                    {
                                        IsValidEmail(objcc[i].Trim());
                                        mailMessage.CC.Add(objcc[i].Trim());
                                    }
                                }
                                else
                                {
                                    string ccname = objcc[i].ToString();
                                    if (Regex.IsMatch(ccname, "^[a-zA-Z0-9]*$"))
                                    {
                                        List<MSTEmployee> objemp = new List<MSTEmployee>();
                                        objemp = db.MSTEmployees.Where(x => x.NameEn == ccname).ToList();
                                        string[] mailvalue = new string[1];
                                        foreach (var empname in objemp)
                                        {
                                            mailvalue[0] = empname.Email.ToString();
                                        }
                                        if (!string.IsNullOrEmpty(mailvalue[0]))
                                        {
                                            IsValidEmail(mailvalue[0].Trim());
                                            mailMessage.CC.Add(mailvalue[0].Trim());
                                        }
                                    }
                                    else
                                    {
                                        List<MSTEmployee> objemp = new List<MSTEmployee>();
                                        objemp = db.MSTEmployees.Where(x => x.NameTh == ccname).ToList();
                                        string[] mailvalue = new string[1];
                                        foreach (var empname in objemp)
                                        {
                                            mailvalue[0] = empname.Email.ToString();
                                        }
                                        if (!string.IsNullOrEmpty(mailvalue[0]))
                                        {
                                            IsValidEmail(mailvalue[0].Trim());
                                            mailMessage.CC.Add(mailvalue[0].Trim());
                                        }
                                    }                                   
                                }
                            }
                        }
                        else if (IsValidEmail(CC.Trim()))
                            mailMessage.CC.Add(CC.Trim());
                    string sSubject = Subject;
                    mailMessage.Subject = sSubject;
                    html = html.Replace("[End Date]", enddate)
                              .Replace("[SO No.]", sono)
                              .Replace("[Serial No.#1]", srno1)
                              .Replace("[Serial No.#2]", srno2)
                              .Replace("[Serial No.#3]", srno3)
                              .Replace("[Serial No.#4]", srno4)
                              .Replace("[Serial No.#5]", srno5)
                              .Replace("[URLToRequest]", string.Format("<a href='{0}{1}'>Click</a>", System.Web.Configuration.WebConfigurationManager.AppSettings["MemoURL"], memoid));

                    System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(html, null,
                    System.Net.Mime.MediaTypeNames.Text.Html);
                    mailMessage.AlternateViews.Add(htmlView);
                    mailMessage.IsBodyHtml = true;

                    if (mailMessage.To.Count > 0)
                    {
                        WriteLogFile.writeLogFile($"{DateTime.Now} - before _smtp.Send(mailMessage) | SendAsync = false");
                        _smtp.Send(mailMessage);
                        WriteLogFile.writeLogFile($"{DateTime.Now} - after _smtp.Send(mailMessage) | SendAsync = false");
                    }
                    WriteLogFile.writeLogFile("Time after Sendmail :" + DateTime.Now.ToString("hh:mm:ss tt"));
                }

            }
            catch (Exception ex)
            {
                WriteLogFile.writeLogFile($"{ex.ToString()}");
                WriteLogFile.writeLogFile("Send Email Fail...!!");
                Environment.Exit(0);
            }

        }


    }
}

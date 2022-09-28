using SendAlertEmail.Extensions;
using SendAlertEmail.Model;
using SendAlertEmail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfApprove.Model.CustomClass;
using Microsoft.SharePoint.Client.Publishing;

namespace SendAlertEmail
{
    public class Services
    {
        public static string dbConnectionString
        {
            get
            {
                var dbConnectionString = ConfigurationManager.AppSettings["dbConnectionString"];
                if (!string.IsNullOrEmpty(dbConnectionString))
                {
                    return dbConnectionString;
                }
                return "";
            }
        }
        private static string masterDataType
        {
            get
            {
                string MasterDataType = ConfigurationManager.AppSettings["MasterDataType"].ToString();
                if (!string.IsNullOrEmpty(MasterDataType))
                {
                    return MasterDataType;
                }
                return "";
            }
        }
        private static string checkSendEmail = ConfigurationManager.AppSettings["SendEmailEveryDay"].ToString();

        public static void sendAlertEmail ()
        {
            DataClasses1DataContext db = new DataClasses1DataContext(dbConnectionString);
            bool chk = Convert.ToBoolean(checkSendEmail);
            if (db.Connection.State == ConnectionState.Open)
            {
                db.Connection.Close();
                db.Connection.Open();
            }
            else
            {
                WriteLogFile.writeLogFile("Connect To Database Server...");
                try 
                {
                    db.Connection.Open();
                }
                catch
                {
                    WriteLogFile.writeLogFile("Connect Fail..!!");
                    Environment.Exit(0);
                }
                WriteLogFile.writeLogFile("Connect Success");
                WriteLogFile.writeLogFile("Search MasterDataType : "+masterDataType);
                List<MSTMasterData> objmasterdata = new List<MSTMasterData>();
                objmasterdata = db.MSTMasterDatas.Where(x => x.MasterType == masterDataType).ToList();
                WriteLogFile.writeLogFile("Found : " + objmasterdata.Count);
                if (objmasterdata.Count != 0)
                {
                    foreach (var objmstdata in objmasterdata)
                    { 
                    List<MSTTemplate> objtemp = new List<MSTTemplate>();
                        int mstdataDay = Convert.ToInt32(objmstdata.Value3);
                        objtemp = db.MSTTemplates.Where(x => x.DocumentCode == objmstdata.Value1).ToList();
                        foreach (var temp in objtemp)
                        {
                            List<TRNMemo> objtrnmemo = new List<TRNMemo>();
                            WriteLogFile.writeLogFile("Search TRNMemo Status : Wait for Approve...");
                            objtrnmemo = db.TRNMemos.Where(x => x.StatusName == "Wait for Approve" && x.TemplateId == temp.TemplateId).ToList();
                            WriteLogFile.writeLogFile("Found : "+objtrnmemo.Count);
                            if (objtrnmemo.Count != 0)
                            {
                                foreach (var trnmemo in objtrnmemo)
                                {
                                    string[] value = new string[1];
                                    List<CustomJsonAdvanceForm.BoxLayout_RefDoc> tempMAdvanceFormItem = ConvertAdvanceFromToList.convertAdvanceFormToList(trnmemo.MAdvancveForm);
                                    foreach (var tempItem in tempMAdvanceFormItem)
                                    {
                                        if (tempItem.Box_Control_Label == objmstdata.Value2.ToString())
                                        {
                                            value[0] = tempItem.Box_Control_Value;
                                        }
                                        DateTime enddate = Convert.ToDateTime(value[0]);
                                        DateTime dtnow = DateTime.Now;
                                        DateTime sumDatetime = enddate.AddDays(-mstdataDay);
                                        //Search EmailTemplate Form MasterData : Value4//
                                        List<MSTEmailTemplate> mstemailtemp = new List<MSTEmailTemplate>();
                                        mstemailtemp = db.MSTEmailTemplates.Where(x => x.FormState == objmstdata.Value4).ToList();
  
                                        foreach (var emailtemp in mstemailtemp)
                                        {
                                            string emailBody = emailtemp.EmailBody;
                                            string emailSubject = emailtemp.EmailSubject;
                                            string sendTo = trnmemo.ToPerson;
                                            if (chk)
                                            {
                                                if (sumDatetime >= dtnow)
                                                {
                                                    SendEmail.sendEmail(emailBody, sendTo, emailSubject);
                                                }
                                            }
                                            if (!chk)
                                            {
                                                if (sumDatetime == dtnow)
                                                {
                                                    SendEmail.sendEmail(emailBody, sendTo, emailSubject);
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            WriteLogFile.writeLogFile("Not Found TRNMemo Status : Wait for Approve");
                        }
                    }
                }
                WriteLogFile.writeLogFile("Not Found MasterDataType : " + masterDataType);
            }
        }
    }
}

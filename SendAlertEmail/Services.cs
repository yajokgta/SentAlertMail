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
        private static string _BaseAPI
        {
            get
            {
                var BaseAPI = ConfigurationManager.AppSettings["BaseAPI"];
                if (!string.IsNullOrEmpty(BaseAPI))
                {
                    return BaseAPI;
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
        private static bool checkSendEmail
        {
            get
            {
                string CheckSendEmail = ConfigurationManager.AppSettings["SendEmailEveryDay"].ToString();
                bool chk = Convert.ToBoolean(CheckSendEmail);
                if (!chk)
                {
                    return false;
                }
                return true;
              
            }
        }
        public static void sendAlertEmail ()
        {
            DataClasses1DataContext db = new DataClasses1DataContext(dbConnectionString);

            if (db.Connection.State == ConnectionState.Open)
            {
                db.Connection.Close();
                db.Connection.Open();
            }
            else
            {
                WriteLogFile.writeLogFile("Connect To Database Server...");
                db.Connection.Open();
                WriteLogFile.writeLogFile("Connect Success");
                List<MSTMasterData> objmasterdata = new List<MSTMasterData>();
                objmasterdata = db.MSTMasterDatas.Where(x => x.MasterType == masterDataType).ToList();
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
                            objtrnmemo = db.TRNMemos.Where(x => x.StatusName == "Wait for Approve" && x.TemplateId == temp.TemplateId).ToList();
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

                                        List<MSTEmailTemplate> mstemailtemp = new List<MSTEmailTemplate>();
                                        mstemailtemp = db.MSTEmailTemplates.Where(x => x.FormState == objmstdata.Value4).ToList();
                                        foreach (var emailtemp in mstemailtemp)
                                        {
                                            string emailBody = emailtemp.EmailBody;
                                            string emailSubject = emailtemp.EmailSubject;
                                            string sendTo = trnmemo.ToPerson;
                                            if (checkSendEmail)
                                            {
                                                if (sumDatetime >= dtnow)
                                                {
                                                    SendEmail.sendEmail(emailBody, sendTo, emailSubject);
                                                }
                                            }
                                            if (!checkSendEmail)
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

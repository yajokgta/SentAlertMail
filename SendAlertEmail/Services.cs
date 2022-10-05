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
using WolfApprove.Model.Extension;
using System.Globalization;

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
                        bool isActive = Convert.ToBoolean(objmstdata.IsActive);
                        if (isActive)
                        {
                            List<MSTTemplate> objtemp = new List<MSTTemplate>();
                            int mstdataDay = Convert.ToInt32(objmstdata.Value3);
                            objtemp = db.MSTTemplates.Where(x => x.DocumentCode == objmstdata.Value1).ToList();
                            foreach (var temp in objtemp)
                            {
                                List<TRNMemo> objtrnmemo = new List<TRNMemo>();
                                WriteLogFile.writeLogFile("Search TRNMemo Status : Wait for Approve...");
                                objtrnmemo = db.TRNMemos.Where(x => x.StatusName == "Wait for Approve" && x.TemplateId == temp.TemplateId).ToList();
                                WriteLogFile.writeLogFile("Found : " + objtrnmemo.Count);
                                if (objtrnmemo.Count != 0)
                                {
                                    foreach (var trnmemo in objtrnmemo)
                                    {
                                        string[] value = new string[8];
                                        
                                        List<CustomJsonAdvanceForm.BoxLayout_RefDoc> tempMAdvanceFormItem = ConvertAdvanceFromToList.convertAdvanceFormToList(trnmemo.MAdvancveForm);
                                        foreach (var tempItem in tempMAdvanceFormItem)
                                        {
                                            if (tempItem.Box_Control_Label == objmstdata.Value2.ToString())
                                            {
                                                value[0] = tempItem.Box_Control_Value;
                                            }
                                            if (tempItem.Box_Control_Label == "Serial No.#1")
                                            {
                                                value[1] = tempItem.Box_Control_Value;
                                            }
                                            if (tempItem.Box_Control_Label == "Serial No.#2")
                                            {
                                                value[2] = tempItem.Box_Control_Value;
                                            }
                                            if (tempItem.Box_Control_Label == "Serial No.#3")
                                            {
                                                value[3] = tempItem.Box_Control_Value;
                                            }
                                            if (tempItem.Box_Control_Label == "Serial No.#4")
                                            {
                                                value[4] = tempItem.Box_Control_Value;
                                            }
                                            if (tempItem.Box_Control_Label == "Serial No.#5")
                                            {
                                                value[5] = tempItem.Box_Control_Value;
                                            }
                                            if (tempItem.Box_Control_Label == "SO No.")
                                            {
                                                value[6] = tempItem.Box_Control_Value;
                                            }
                                        }
                                        value[7] = trnmemo.MemoId.ToString();
                                        List<String> listvalue = new List<String>();
                                        for (int i = 0; i < value.Length; i++)
                                        {
                                            listvalue.Add(value[i].ToString());
                                        }

                                        if (!string.IsNullOrEmpty(value[0]))
                                        {
                                            
                                            DateTime enddatevalue = Convert.ToDateTime(value[0].ToString());
                                            //WriteLogFile.writeLogFile(enddatevalue.ToString());
                                            //string testdate = enddatevalue.ToString("dd/MM/yyyy HH:mm:ss");
                                            //DateTime dt = DateTime.ParseExact(testdate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                            //string enddateformatt = enddatevalue.ToString("dd/MM/yyyy");
                                            //WriteLogFile.writeLogFile(enddateformatt);
                                            DateTime enddate = enddatevalue.Date;
                                            //WriteLogFile.writeLogFile(enddate.ToString());

                                            //string dtformatt = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("en-US"));
                                            //DateTime dtnow = Convert.ToDateTime(dtformatt);
                                            DateTime dtnow = DateTime.Now.Date;
                                            DateTime sumDatetime = enddate.AddDays(-mstdataDay);
                                            //Search EmailTemplate Form MasterData : Value5//
                                            List<MSTEmailTemplate> mstemailtemp = new List<MSTEmailTemplate>();
                                            mstemailtemp = db.MSTEmailTemplates.Where(x => x.FormState == objmstdata.Value5).ToList();
                                            if (mstemailtemp.Count != 0)
                                            {
                                                foreach (var emailtemp in mstemailtemp)
                                                {
                                                    string emailBody = emailtemp.EmailBody;
                                                    string emailSubject = emailtemp.EmailSubject;
                                                    string sendTo = "";
                                                    string sendToCC = trnmemo.CcPerson;
                                                    if (chk)
                                                    {
                                                        WriteLogFile.writeLogFile("เข้าเงื่อนไข : ส่งทุกวัน");
                                                        if (sumDatetime.Date <= dtnow.Date)
                                                        {
                                                            WriteLogFile.writeLogFile("ทำการส่ง Mail TemplateID : " + trnmemo.MemoId.ToString());
                                                            WriteLogFile.writeLogFile("End Date = " + enddate.Date.ToString() + " DateTimeNow = " + dtnow.Date.ToString());
                                                            SendEmail.SendEmailTemplate(emailBody, emailSubject, sendTo, sendToCC, listvalue);
                                                        }
                                                        else
                                                        {
                                                            WriteLogFile.writeLogFile("ยังไม่ถึงวันที่ต้องส่ง หรือ เลยวันที่ต้องส่งแล้ว TRNMemo ID = " + trnmemo.MemoId.ToString());
                                                            WriteLogFile.writeLogFile("End Date = " + enddate.Date.ToString() + " DateTimeNow = " + dtnow.Date.ToString());
                                                        }
                                                    }
                                                    if (!chk)
                                                    {
                                                        WriteLogFile.writeLogFile("เข้าเงื่อนไข : ส่งครั้งเดียว");
                                                        if (sumDatetime.Date == dtnow.Date)
                                                        {
                                                            WriteLogFile.writeLogFile("ทำการส่ง Mail TemplateID : " + trnmemo.TemplateId);
                                                            WriteLogFile.writeLogFile("End Date = " + enddate.Date.ToString() + " DateTimeNow = " + dtnow.Date.ToString());
                                                            SendEmail.SendEmailTemplate(emailBody, emailSubject, sendTo, sendToCC, listvalue);
                                                        }
                                                        else
                                                        {
                                                            WriteLogFile.writeLogFile("ยังไม่ถึงวันที่ต้องส่ง หรือ เลยวันที่ต้องส่งแล้ว TRNMemo ID = " + trnmemo.MemoId.ToString());
                                                            WriteLogFile.writeLogFile("End Date = " + enddate.Date.ToString() + " DateTimeNow = " + dtnow.Date.ToString());
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                        }
                        
                    }
                }
                
            }
        }
    }
}

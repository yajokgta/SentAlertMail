using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfApprove.Model.CustomClass;

namespace SentAlertEmail
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
        private static bool checkSentEmail
        {
            get
            {
                string CheckSentEmail = ConfigurationManager.AppSettings["SentEmailEveryDay"].ToString();
                bool chk = Convert.ToBoolean(CheckSentEmail);
                if (!chk)
                {
                    return false;
                }
                return true;
              
            }
        }
        public static void sentAlertEmail ()
        {
            DataClasses1DataContext db = new DataClasses1DataContext(dbConnectionString);

            if (db.Connection.State == ConnectionState.Open)
            {
                db.Connection.Close();
                db.Connection.Open();
            }
            else
            {
                db.Connection.Open();
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
                                        if (checkSentEmail)
                                        {
                                            if (sumDatetime >= dtnow)
                                            {
                                                
                                            }
                                        }
                                        if (!checkSentEmail)
                                        {
                                            if(sumDatetime == dtnow)
                                            {

                                            }
                                        }

                                    }
                                }
                            }
                            Console.WriteLine("Not Found TRNMemo Status : Wait for Approve");
                        }
                    }
                }
                Console.WriteLine("Not Found MasterDataType");
            }
        }
    }
}

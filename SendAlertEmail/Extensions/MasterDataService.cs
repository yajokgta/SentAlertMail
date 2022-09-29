using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendAlertEmail.Extensions;
using WolfApprove.Model;
using WolfApprove.Model.CustomClass;
using WolfApprove.Model.Extension;

namespace WolfApprove.API2.Controllers.Services
{
    public class MasterDataService
    {
        public int? seq { get; set; }
        public int? masterId { get; set; }

        public CustomMasterData GetMasterData(CustomMasterData iCustom)
        {
            try
            {
                List<MSTMasterData> lstTempData = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(iCustom.connectionString))
                {
                    lstTempData = db.MSTMasterDatas.Where(x => x.MasterId == iCustom.MasterId).ToList();
                }
                foreach (MSTMasterData item in lstTempData)
                {
                    CustomMasterData CustomData = new CustomMasterData();
                    CustomData.RetrieveFromDtoWithDateTimeFormat(item);
                    return CustomData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public List<CustomMasterData> GetMasterDataList(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x => x.IsActive == true).ToList();
                    mstMasterDataList = db.MSTMasterDatas.Where(x => x.IsActive == true).Select(x => new CustomMasterData() { MasterId = x.MasterId, Value1 = x.Value1, Value2 = x.Value2, Value3 = x.Value3, Value4 = x.Value4, Value5 = x.Value5, MasterType = x.MasterType, IsActive = x.IsActive }).ToList();
                }
                //foreach(var DataModel in mstMasterDataListModel)
                //  {
                //      CustomMasterData TranferData = new CustomMasterData();
                //      TranferData.RetrieveFromDto(DataModel);
                //      mstMasterDataList.Add(TranferData);
                //  }

            }
            catch (Exception)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }

        public List<CustomMasterData> GetMasterDataList(CustomMasterData customMasterData, string MasterType)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x => x.IsActive == true && x.MasterType.ToUpper() == MasterType.ToUpper()).ToList();
                }
                mstMasterDataList = mstMasterDataListModel.ToJson().ToArrayObject<CustomMasterData>();
                //foreach(var DataModel in mstMasterDataListModel)
                //  {
                //      CustomMasterData TranferData = new CustomMasterData();
                //      TranferData.RetrieveFromDto(DataModel);
                //      mstMasterDataList.Add(TranferData);
                //  }

            }
            catch (Exception)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }

        public List<CustomMasterData> GetMasterDataListAll(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }

            }
            catch (Exception)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }
        public List<CustomMasterData> GetSignatureList(CustomMasterData customMasterData)
        {
            List<CustomMasterData> customMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x => x.IsActive == true && x.MasterType.ToLower().Equals("signature")).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    customMasterDataList.Add(TranferData);
                }

            }
            catch (Exception)
            {
                customMasterDataList = new List<CustomMasterData>();
            }
            return customMasterDataList;
        }
        public List<CustomMasterData> GetMasterDataByType(CustomMasterData customMasterData)
        {
            List<CustomMasterData> customMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x => x.IsActive == true && x.MasterType.ToLower().Equals(customMasterData.MasterType)).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    customMasterDataList.Add(TranferData);
                }

            }
            catch (Exception)
            {
                customMasterDataList = new List<CustomMasterData>();
            }
            return customMasterDataList;
        }
       
        public bool UpdateSeq(CustomMasterData MSTMasterData)
        {
            bool result = true;
            try
            {
                using (WolfApproveModel db = DBContext.OpenConnection(MSTMasterData.connectionString))
                {
                    if (seq != MSTMasterData.Seq)
                    {
                        string sql = "UPDATE MSTMasterData SET Seq = ";
                        if (seq > MSTMasterData.Seq)
                        {
                            sql = sql + "(Seq - 1) WHERE Seq BETWEEN " + (seq + 1) + " AND " + MSTMasterData.Seq + " ";
                        }
                        else
                        {
                            sql = sql + "(Seq + 1) WHERE Seq BETWEEN " + MSTMasterData.Seq + " AND " + (seq - 1) + " ";
                        }
                        sql = sql + "AND ApproveMatrixId <> " + MSTMasterData.MasterId;
                        db.Database.ExecuteSqlCommand(sql);
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }


        #region | LeaveRequest |

        public List<CustomMasterData> GetMasterDataListLeaveRequest(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == "LR" && x.Value1 == customMasterData.Value1 && x.Seq == customMasterData.Seq &&
                        x.IsActive == true).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }

            }
            catch (Exception)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }
        public List<CustomMasterData> GetMasterDataListLeaveRequestTemplateCode(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == "LRTempCode" &&
                        x.IsActive == true).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }

            }
            catch (Exception ex)
            {
                WriteLogFile.writeLogFile("GetMasterDataListLeaveRequestTemplateCode : " + ex.Message.ToString());
                mstMasterDataList = new List<CustomMasterData>();
            }
            WriteLogFile.writeLogFile("mstMasterDataList :" + mstMasterDataList.Count);
            return mstMasterDataList;
        }
        public CustomMasterData GetTemplateIsLeaveRequest(MemoDetail memo)
        {

            CustomMasterData iMasterData = new CustomMasterData { connectionString = memo.connectionString, userPrincipalName = memo.actor.Email };
            List<CustomMasterData> list_MasterData = GetMasterDataListLeaveRequestTemplateCode(iMasterData);
            list_MasterData = list_MasterData.Where(a => a.Value1 == memo.template_code).ToList();
            if (list_MasterData.Count > 0)
            {
                return list_MasterData.First();
            }

            return null;

        }

        public List<CustomMasterData> GetTemplateIsUpdateDataEndFlow(MemoDetail memo)
        {

            CustomMasterData iMasterData = new CustomMasterData { connectionString = memo.connectionString, userPrincipalName = memo.actor.Email };
            List<CustomMasterData> list_MasterData = GetMasterDataListUpdateDataEndFlowTemplateCode(iMasterData);
            WriteLogFile.writeLogFile("memo.template_code : " + memo.template_code);
            list_MasterData = list_MasterData.Where(a => a.Value1 == memo.template_code && a.Value2 == memo.status).ToList();
            if (list_MasterData.Count > 0)
            {
                return list_MasterData;
            }

            return null;

        }

        public List<CustomMasterData> GetMasterDataListUpdateDataEndFlowTemplateCode(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == "CondUpdate" &&
                        x.IsActive == true).ToList();
                }
                foreach (MSTMasterData DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }
            }
            catch (Exception)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }

        #endregion

        #region | EXRequest |
        public List<CustomMasterData> GetMasterDataListEXRequest(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == "EX" && x.Value1 == customMasterData.Value1 && x.Seq == customMasterData.Seq &&
                        x.IsActive == true).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }

            }
            catch (Exception)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }
        public List<CustomMasterData> GetMasterDataListEXRequestTemplateCode(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == "EXTempCode" &&
                        x.IsActive == true).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }
                WriteLogFile.writeLogFile("mstMasterDataList " + mstMasterDataList.Count);
            }
            catch (Exception ex)
            {
                WriteLogFile.writeLogFile("ex GetMasterDataListEXRequestTemplateCode "+ ex.Message.ToString());
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }
        public CustomMasterData GetTemplateIsEXRequest(MemoDetail memo)
        {

            CustomMasterData iMasterData = new CustomMasterData { connectionString = memo.connectionString, userPrincipalName = memo.actor.Email };
            List<CustomMasterData> list_MasterData = GetMasterDataListEXRequestTemplateCode(iMasterData);
            list_MasterData = list_MasterData.Where(a => a.Value1 == memo.template_code).ToList();
            if (list_MasterData.Count > 0)
            {
                return list_MasterData.First();
            }

            return null;

        }

        #endregion

        #region | Template is Approve Version |
        public CustomMasterData GetMasterDataListTemplateIsApproveVersion(MemoDetail memo)
        {

            CustomMasterData iMasterData = new CustomMasterData { connectionString = memo.connectionString, userPrincipalName = memo.actor.Email };
            List<CustomMasterData> list_MasterData = GetMasterDataListTemplateIsApproveVersionTemplateCode(iMasterData);
            list_MasterData = list_MasterData.Where(a => a.Value1 == memo.template_code).ToList();
            if (list_MasterData.Count > 0)
            {
                return list_MasterData.First();
            }

            return null;

        }
        public List<CustomMasterData> GetMasterDataListTemplateIsApproveVersionTemplateCode(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == MasterTypeEnum.TempVer.ToString() &&
                        x.IsActive == true).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }

            }
            catch (Exception)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }
        public CustomMasterData GetTemplateIsApproveVersion(MemoDetail memo)
        {

            CustomMasterData iMasterData = new CustomMasterData { connectionString = memo.connectionString, userPrincipalName = memo.actor.Email };
            List<CustomMasterData> list_MasterData = GetMasterDataListTemplateIsApproveVersionTemplateCode(iMasterData);
            if (list_MasterData.Count > 0)
            {
                return list_MasterData.First();
            }

            return null;

        }

        #endregion

        #region | Validate Attachment |

        public List<CustomMasterData> GetMasterDataListLeaveValidateAttachment(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == "ValidAtt" &&
                        x.IsActive == true).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }

            }
            catch (Exception)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }

        #endregion

        public List<CustomMasterData> GetMasterDataListInActiveEmail(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == "InActEmail" &&
                        x.IsActive == true).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }

            }
            catch (Exception ex)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }

        public List<CustomMasterData> GetMasterDataListCustomEmail(CustomMasterData customMasterData)
        {
            List<CustomMasterData> mstMasterDataList = new List<CustomMasterData>();
            try
            {
                List<MSTMasterData> mstMasterDataListModel = new List<MSTMasterData>();
                using (WolfApproveModel db = DBContext.OpenConnection(customMasterData.connectionString))
                {
                    mstMasterDataListModel = db.MSTMasterDatas.Where(x =>
                        x.MasterType == "CTEmail" &&
                        x.IsActive == true).ToList();
                }
                foreach (var DataModel in mstMasterDataListModel)
                {
                    CustomMasterData TranferData = new CustomMasterData();
                    TranferData.RetrieveFromDto(DataModel);
                    mstMasterDataList.Add(TranferData);
                }

            }
            catch (Exception ex)
            {
                mstMasterDataList = new List<CustomMasterData>();
            }
            return mstMasterDataList;
        }

    }
}
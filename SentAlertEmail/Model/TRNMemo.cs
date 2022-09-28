using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentAlertEmail.Model
{
    class TRNMemo
    {
        [Key]
        public int MemoId { get; set; }

        [StringLength(50)]
        public string DocumentNo { get; set; }

        public int? TemplateId { get; set; }

        [StringLength(500)]
        public string TemplateName { get; set; }

        [StringLength(100)]
        public string TemplateType { get; set; }

        public int? DepartmentId { get; set; }

        [StringLength(50)]
        public string DocumentCode { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(400)]
        public string CompanyName { get; set; }

        public int? ProjectID { get; set; }

        [StringLength(400)]
        public string ProjectName { get; set; }

        public bool? IsPublic { get; set; }

        [StringLength(2)]
        public string ReportLang { get; set; }

        [StringLength(500)]
        public string TemplateDetail { get; set; }

        public string ToPerson { get; set; }

        public string CcPerson { get; set; }

        [StringLength(500)]
        public string TemplateSubject { get; set; }

        public string TTextForm { get; set; }

        public string TAdvanceForm { get; set; }

        public bool? AutoApprove { get; set; }

        [StringLength(2)]
        public string AutoApproveWhen { get; set; }

        public bool? ApproverCanEdit { get; set; }

        public int? CreatorId { get; set; }

        [StringLength(500)]
        public string CNameTh { get; set; }

        [StringLength(500)]
        public string CNameEn { get; set; }

        public int? CPositionId { get; set; }

        [StringLength(500)]
        public string CPositionTh { get; set; }

        [StringLength(500)]
        public string CPositionEn { get; set; }

        public int? CDepartmentId { get; set; }

        [StringLength(500)]
        public string CDepartmentTh { get; set; }

        [StringLength(500)]
        public string CDepartmentEn { get; set; }

        public int? CDivisionId { get; set; }

        [StringLength(500)]
        public string CDivisionTh { get; set; }

        [StringLength(500)]
        public string CDivisionEn { get; set; }

        public int? RequesterId { get; set; }

        [StringLength(500)]
        public string RNameTh { get; set; }

        [StringLength(500)]
        public string RNameEn { get; set; }

        public int? RPositionId { get; set; }

        [StringLength(500)]
        public string RPositionTh { get; set; }

        [StringLength(500)]
        public string RPositionEn { get; set; }

        public int? RDepartmentId { get; set; }

        [StringLength(500)]
        public string RDepartmentTh { get; set; }

        [StringLength(500)]
        public string RDepartmentEn { get; set; }

        public int? RDivisionId { get; set; }

        [StringLength(500)]
        public string RDivisionTh { get; set; }

        [StringLength(500)]
        public string RDivisionEn { get; set; }

        [StringLength(500)]
        public string MemoSubject { get; set; }

        public string MTextForm { get; set; }

        public string MAdvancveForm { get; set; }

        public int? StatusId { get; set; }

        [StringLength(500)]
        public string StatusName { get; set; }

        public int? CurrentApprovalLevel { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? RequestDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(500)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(500)]
        public string ModifiedBy { get; set; }

        [StringLength(500)]
        public string LastActionBy { get; set; }

        public int? LastStatusId { get; set; }

        [StringLength(500)]
        public string LastStatusName { get; set; }

        public int? PersonWaitingId { get; set; }

        [StringLength(500)]
        public string PersonWaiting { get; set; }

        public int? AccountId { get; set; }

        public bool IsReaded { get; set; }

        [StringLength(500)]
        public string GroupTemplateName { get; set; }
        [StringLength(10)]
        public string TemplateApproveId { get; set; }

        public int? CheckEmpID { get; set; }

        public DateTime? SLAStartDate { get; set; }
        public DateTime? SLACompletedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendAlertEmail.Model
{
    class MSTTemplate
    {
        [Key]
        public int? TemplateId { get; set; }

        [StringLength(500)]
        public string GroupTemplateName { get; set; }

        [StringLength(500)]
        public string TemplateName { get; set; }

        public int? DepartmentId { get; set; }

        [StringLength(50)]
        public string DocumentCode { get; set; }

        public bool? isPublic { get; set; }

        [StringLength(2)]
        public string ReportLang { get; set; }

        [StringLength(500)]
        public string TemplateDetail { get; set; }

        [StringLength(500)]
        public string ToId { get; set; }

        [StringLength(500)]
        public string CcId { get; set; }

        [StringLength(500)]
        public string TemplateSubject { get; set; }

        public bool? AutoApprove { get; set; }

        public string TextForm { get; set; }

        public string AdvanceForm { get; set; }

        public bool? IsTextForm { get; set; }

        [StringLength(2)]
        public string AutoApproveWhen { get; set; }

        public bool? ApproverCanEdit { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(500)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(500)]
        public string ModifiedBy { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsEdit { get; set; }

        public bool? IsDelete { get; set; }

        public string RoleID { get; set; }

        public int? AccountId { get; set; }
        [NotMapped]
        public int? MemoId { get; set; }
        public bool? isPDFShowInfo { get; set; }
        public bool? isRequesterEditApproval { get; set; }
        public bool? isFormControl { get; set; }

        public string SpecificEmployeeId { get; set; }
        public string SpecificRoleId { get; set; }

        public string MultiDeptId { get; set; }
        public bool? ReqAttach { get; set; }
        public string RefTemplate { get; set; }
        public string RefDocColumn { get; set; }
        public String RefDocDisplay { get; set; }
        public bool? IsCheckAccess { get; set; }
        public bool? IsDefaultLineApprove { get; set; }
    
    }
}

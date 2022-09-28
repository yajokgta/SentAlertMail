using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentAlertEmail.Model
{
    class MSTEmailTemplate
    {
        [Key]
        public int EmailTemplateId { get; set; }

        public int? TemplateId { get; set; }

        [NotMapped] //Temp Column
        public string TemplateName { get; set; }

        [NotMapped] //Temp Column
        public string TemplateDocumentCode { get; set; }

        [StringLength(100)]
        public string FormState { get; set; }

        [StringLength(250)]
        public string EmailTo { get; set; }

        [StringLength(250)]
        public string EmailCC { get; set; }

        [StringLength(250)]
        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(500)]
        public string CreatedBy { get; set; }

        [NotMapped] //Temp Column
        public string CreatedByName { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(500)]
        public string ModifiedBy { get; set; }

        [NotMapped] //Temp Column
        public string ModifiedByName { get; set; }

        public bool? IsActive { get; set; }
    }
}

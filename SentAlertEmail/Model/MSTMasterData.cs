using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentAlertEmail.Model
{
    class MSTMasterData
    {
        [Key]
        public int? MasterId { get; set; }

        [StringLength(10)]
        public string MasterType { get; set; }

        [StringLength(500)]
        public string Value1 { get; set; }

        [StringLength(500)]
        public string Value2 { get; set; }

        [StringLength(500)]
        public string Value3 { get; set; }

        [StringLength(500)]
        public string Value4 { get; set; }

        [StringLength(500)]
        public string Value5 { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(500)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(500)]
        public string ModifiedBy { get; set; }

        public int? Seq { get; set; }
    }
}

using JobEntryy.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobEntryy.Domain.Entities
{
    public class JobDetail : BaseEntity
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string RequiredSkills { get; set; } //Tələb olunan bacarıq //+ -
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Responsibilities { get; set; } // Vəzifə öhdəlikləri //+ -
        public string? JobGraphic { get; set; } // İş qrafiki -
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Salary { get; set; }  // -
        public string? Email { get; set; } // -
        public string? Link { get; set; }

        public Job Job { get; set; }
        [ForeignKey("Job")]
        public int JobId { get; set; }
    }
}

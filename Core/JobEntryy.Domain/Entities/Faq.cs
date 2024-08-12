using JobEntryy.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JobEntryy.Domain.Entities
{
    public class Faq : EntityList
    {
        [Required(ErrorMessage ="Bu xana boş ola bilməz")]
        public string Quetsion { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Answer { get; set; }
    }
}

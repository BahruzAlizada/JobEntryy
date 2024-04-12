using JobEntryy.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JobEntryy.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessage ="Bu xana boş ola bilməz")]
        public string Name { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}

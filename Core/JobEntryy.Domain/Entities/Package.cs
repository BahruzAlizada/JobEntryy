using JobEntryy.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JobEntryy.Domain.Entities
{
    public class Package : EntityList
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public int Price { get; set; }
        public bool ForCompany { get; set; }
    }
}

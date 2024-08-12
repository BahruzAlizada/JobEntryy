using JobEntryy.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JobEntryy.Domain.Entities
{
    public class SocialMedia : EntityList
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Icon { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Link { get; set; }
    }
}

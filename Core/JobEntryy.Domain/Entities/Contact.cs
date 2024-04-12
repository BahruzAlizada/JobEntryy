using JobEntryy.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JobEntryy.Domain.Entities
{
    public class Contact : BaseEntity
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Email adresini düzgün qeyd edin")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Message { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}



using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobEntryy.Application.ViewModels
{
    public  class CompanyVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage ="Bu xana boş ola bilməz")]
        public string Email { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Name { get; set; }
        public string? CompanyDescription { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WebUrl { get; set; }
        public string? Address { get; set; }
        public bool IsPremium { get; set; }
        public bool Status { get; set; }
        public DateTime Created { get; set; }


        public int JobsCount { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}

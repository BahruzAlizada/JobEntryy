using JobEntryy.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobEntryy.Domain.Identity
{
    public class Company : AppUser
    {
        public string? Image { get; set; }
        public string? CompanyDescription { get; set; }
        public string? WebUrl { get; set; }
        public string? Address { get; set; }
        public bool IsPremium { get; set; }

        public List<Job> Jobs { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Bu xana boş qala bilməz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Bu xana boş qala bilməz")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifrəni düzgün daxil edin")]
        public string CheckPassword { get; set; }
        [NotMapped]
        public bool IsRemember { get; set; }
    }
}

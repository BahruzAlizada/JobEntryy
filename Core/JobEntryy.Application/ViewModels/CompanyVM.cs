

using Microsoft.AspNetCore.Http;

namespace JobEntryy.Application.ViewModels
{
    public  class CompanyVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string? CompanyDescription { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WebUrl { get; set; }
        public string? Address { get; set; }
        public bool IsPremium { get; set; }
        public bool Status { get; set; }

        public int JobsCount { get; set; }
        public IFormFile Photo { get; set; }
    }
}

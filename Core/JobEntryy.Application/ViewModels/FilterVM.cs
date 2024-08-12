

namespace JobEntryy.Application.ViewModels
{
    public class FilterVM
    {
        public string search { get; set; }
        public Guid? typeId { get; set; }
        public Guid? catId { get; set; }
        public Guid? cityId { get; set; }
        public Guid? expId { get; set; }
    }
}

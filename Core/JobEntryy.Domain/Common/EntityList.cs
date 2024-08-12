
namespace JobEntryy.Domain.Common
{
    public class EntityList : BaseEntity
    {
        public DateTime Created { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime? Updated { get; set; }
        public string? ByChanged { get; set; }
    }
}

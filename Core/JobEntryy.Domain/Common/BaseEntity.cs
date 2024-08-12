namespace JobEntryy.Domain.Common
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool Status { get; set; } = true;
    }
}

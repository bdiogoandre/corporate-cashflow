namespace Corporate.Cashflow.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        protected BaseEntity()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}

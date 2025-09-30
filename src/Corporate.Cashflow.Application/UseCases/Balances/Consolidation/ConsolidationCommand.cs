using MediatR;

namespace Corporate.Cashflow.Application.UseCases.Balances.Consolidation
{
    public class ConsolidationCommand : IRequest
    {
        public Guid AccountId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Data { get; set; }
        public long Sequence { get; set; }
    }
}

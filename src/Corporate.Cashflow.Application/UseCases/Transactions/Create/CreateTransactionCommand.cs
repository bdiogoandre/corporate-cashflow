using Corporate.Cashflow.Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corporate.Cashflow.Application.UseCases.Transactions.Create
{
    public class CreateTransactionCommand : IRequest<CreateTransactionCommandResponse>
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public ETransactionType TransactionType { get; set; }
    }

    public record CreateTransactionCommandResponse(Guid Id);
}

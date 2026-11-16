using Microsoft.EntityFrameworkCore;
using transactions.portal.services.Models;

namespace transactions.portal.services.Data.Sql;

public static class SqlSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Transactions.AnyAsync())
        {
            return;
        }

        var definitions = SeedData.GetSeedTransactions();

        foreach (var def in definitions)
        {
            var transactionId = Guid.NewGuid().ToString();

            var transaction = new Transaction
            {
                Id = transactionId,
                Reference = def.Reference,
                Date = def.Date,
                Amount = def.Amount,
                Merchant = def.Merchant,
                Status = def.Status,
                PaymentBrand = def.PaymentBrand,
                CardLast4 = def.CardLast4,
                CustomerId = def.CustomerId
            };

            context.Transactions.Add(transaction);

            var merchantEntry = new MerchantEntry
            {
                Id = Guid.NewGuid().ToString(),
                TransactionId = transactionId,
                Reference = def.MerchantEntry.Reference,
                Type = def.MerchantEntry.Type,
                PostingDate = def.MerchantEntry.PostingDate,
                Amount = def.MerchantEntry.Amount
            };
            context.MerchantEntries.Add(merchantEntry);

            foreach (var evt in def.Events)
            {
                var eventLog = new TransactionEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    TransactionId = transactionId,
                    Timestamp = evt.Timestamp,
                    Message = evt.Message
                };
                context.TransactionEvents.Add(eventLog);
            }

            if (def.Dispute != null)
            {
                var disputeId = Guid.NewGuid().ToString();

                var dispute = new Dispute
                {
                    Id = disputeId,
                    TransactionId = transactionId,
                    Reason = def.Dispute.Reason,
                    Description = def.Dispute.Description,
                    Status = def.Dispute.Status,
                    CreatedAt = def.Dispute.CreatedAt,
                    ResolvedAt = def.Dispute.ResolvedAt,
                    CustomerId = def.CustomerId
                };
                context.Disputes.Add(dispute);

                foreach (var hist in def.Dispute.StatusHistory)
                {
                    var historyEntry = new DisputeStatusHistory
                    {
                        Id = Guid.NewGuid().ToString(),
                        DisputeId = disputeId,
                        Status = hist.Status,
                        ChangedAt = hist.ChangedAt,
                        Note = hist.Note
                    };
                    context.DisputeStatusHistories.Add(historyEntry);
                }
            }

            if (def.Refund != null)
            {
                var refund = new Refund
                {
                    Id = Guid.NewGuid().ToString(),
                    Reference = def.Refund.Reference,
                    TransactionId = transactionId,
                    Amount = def.Refund.Amount,
                    Percentage = def.Refund.Percentage,
                    ProcessedAt = def.Refund.ProcessedAt
                };
                context.Refunds.Add(refund);
            }
        }

        await context.SaveChangesAsync();
    }
}


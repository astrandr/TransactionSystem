
namespace TransactionSystem.Core
{
    public struct AccountServiceResult
    {
        public AccountServiceResultsStatus Status { get; set; }

        public decimal Amount { get; set; }
    }
}

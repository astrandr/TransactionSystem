namespace TransactionSystem.Core
{
    public enum AccountServiceResultsStatus
    {
        Success,
        ExistingAccount,
        NonExistingAccount,
        InsufficientFunds,
        InvalidAmountValue,
        NonExistingSourceAccount,
        NonExistingDestAccount,
    }
}

using TransactionSystem.Core;

namespace TransactionSystem.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        private AccountService service;

        [SetUp]
        public void Setup()
        {
            var repository = new AccountRepository();
            service = new AccountService(repository);
        }

        #region CreateAccount

        [Test]
        public void CreateAccount_ValidAccount_ReturnsSuccess()
        {
            var result = service.CreateAccount("John", "ACC1", 100m);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.Success));
            Assert.That(result.Amount, Is.EqualTo(100m));
        }

        [Test]
        public void CreateAccount_ExistingAccount_ReturnsExistingAccount()
        {
            service.CreateAccount("John", "ACC1", 100m);

            var result = service.CreateAccount("John", "ACC1", 200m);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.ExistingAccount));
        }

        [TestCase(0)]
        [TestCase(-100)]
        public void CreateAccount_InvalidDepositAmount_ReturnsInvalidAmountValue(
            decimal amount)
        {
            var result = service.CreateAccount("John", "ACC1", amount);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.InvalidAmountValue));
        }

        #endregion

        #region Deposit

        [Test]
        public void Deposit_ValidAmount_IncreasesBalance()
        {
            service.CreateAccount("John", "ACC1", 100m);

            var result = service.Deposit("ACC1", 50m);
            var balance = service.GetAccountBalance("ACC1");

            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.Success));
                Assert.That(result.Amount, Is.EqualTo(50m));
                Assert.That(balance.Amount, Is.EqualTo(150m));
            });
        }

        [TestCase(0)]
        [TestCase(-50)]
        public void Deposit_InvalidAmount_ReturnsInvalidAmountValue(decimal amount)
        {
            service.CreateAccount("John", "ACC1", 100m);

            var result = service.Deposit("ACC1", amount);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.InvalidAmountValue));
        }

        [Test]
        public void Deposit_NonExistingAccount_ReturnsNonExistingAccount()
        {
            var result = service.Deposit("UNKNOWN", 50m);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.NonExistingAccount));
        }

        #endregion

        #region Withdraw

        [Test]
        public void Withdraw_ValidAmount_DecreasesBalance()
        {
            service.CreateAccount("John", "ACC1", 100m);

            var result = service.Withdraw("ACC1", 40m);
            var balance = service.GetAccountBalance("ACC1");

            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.Success));
                Assert.That(balance.Amount, Is.EqualTo(60m));
            });
        }

        [Test]
        public void Withdraw_AmountGreaterThanBalance_ReturnsInsufficientFunds()
        {
            service.CreateAccount("John", "ACC1", 100m);

            var result = service.Withdraw("ACC1", 101m);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.InsufficientFunds));
        }

        [Test]
        public void Withdraw_EntireBalance_Succeeds()
        {
            service.CreateAccount("John", "ACC1", 100m);

            var result = service.Withdraw("ACC1", 100m);
            var balance = service.GetAccountBalance("ACC1");

            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.Success));
                Assert.That(balance.Amount, Is.EqualTo(0m));
            });
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void Withdraw_InvalidAmount_ReturnsInvalidAmountValue(decimal amount)
        {
            service.CreateAccount("John", "ACC1", 100m);

            var result = service.Withdraw("ACC1", amount);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.InvalidAmountValue));
        }

        [Test]
        public void Withdraw_NonExistingAccount_ReturnsNonExistingAccount()
        {
            var result = service.Withdraw("UNKNOWN", 10m);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.NonExistingAccount));
        }

        #endregion

        #region GetAccountBalance

        [Test]
        public void GetAccountBalance_ExistingAccount_ReturnsBalance()
        {
            service.CreateAccount("John", "ACC1", 123.45m);

            var result = service.GetAccountBalance("ACC1");

            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.Success));
                Assert.That(result.Amount, Is.EqualTo(123.45m));
            });
        }

        [Test]
        public void GetAccountBalance_NonExistingAccount_ReturnsNonExistingAccount()
        {
            var result = service.GetAccountBalance("UNKNOWN");

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.NonExistingAccount));
        }

        #endregion

        #region Transfer

        [Test]
        public void Transfer_ValidTransfer_MovesMoneyBetweenAccounts()
        {
            service.CreateAccount("John", "ACC1", 100m);
            service.CreateAccount("Mary", "ACC2", 50m);

            var result = service.Transfer("ACC1", "ACC2", 30m);

            var source = service.GetAccountBalance("ACC1");
            var destination = service.GetAccountBalance("ACC2");

            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.Success));
                Assert.That(source.Amount, Is.EqualTo(70m));
                Assert.That(destination.Amount, Is.EqualTo(80m));
            });
        }

        [Test]
        public void Transfer_InsufficientFunds_DoesNotChangeBalances()
        {
            service.CreateAccount("John", "ACC1", 100m);
            service.CreateAccount("Mary", "ACC2", 50m);

            var result = service.Transfer("ACC1", "ACC2", 101m);

            var source = service.GetAccountBalance("ACC1");
            var destination = service.GetAccountBalance("ACC2");

            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.InsufficientFunds));
                Assert.That(source.Amount, Is.EqualTo(100m));
                Assert.That(destination.Amount, Is.EqualTo(50m));
            });
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void Transfer_InvalidAmount_ReturnsInvalidAmountValue(decimal amount)
        {
            service.CreateAccount("John", "ACC1", 100m);
            service.CreateAccount("Mary", "ACC2", 100m);

            var result = service.Transfer("ACC1", "ACC2", amount);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.InvalidAmountValue));
        }

        [Test]
        public void Transfer_NonExistingSource_ReturnsNonExistingSourceAccount()
        {
            service.CreateAccount("Mary", "ACC2", 100m);

            var result = service.Transfer("UNKNOWN", "ACC2", 10m);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.NonExistingSourceAccount));
        }

        [Test]
        public void Transfer_NonExistingDestination_ReturnsNonExistingDestAccount()
        {
            service.CreateAccount("John", "ACC1", 100m);

            var result = service.Transfer("ACC1", "UNKNOWN", 10m);

            Assert.That(result.Status, Is.EqualTo(AccountServiceResultsStatus.NonExistingDestAccount));
        }

        #endregion

        #region Concurrency

        [Test]
        public async Task Deposit_ConcurrentDeposits_ProducesCorrectBalance()
        {
            service.CreateAccount("John", "ACC1", 100m);

            const int operationCount = 1000;

            var tasks = Enumerable.Range(0, operationCount).Select(_ => Task.Run(() => service.Deposit("ACC1", 1m)));

            await Task.WhenAll(tasks);

            var balance = service.GetAccountBalance("ACC1");

            Assert.That(balance.Amount, Is.EqualTo(1100m));
        }

        [Test]
        public async Task Transfer_ConcurrentOppositeTransfers_PreservesTotalBalance()
        {
            service.CreateAccount("John", "ACC1", 1000m);
            service.CreateAccount("Mary", "ACC2", 1000m);

            const int operationCount = 500;

            var tasks = Enumerable.Range(0, operationCount).SelectMany(_ => new[]
                {
                    Task.Run(() => service.Transfer("ACC1", "ACC2", 1m)),
                    Task.Run(() => service.Transfer("ACC2", "ACC1", 1m))
                });

            await Task.WhenAll(tasks);

            var account1 = service.GetAccountBalance("ACC1");
            var account2 = service.GetAccountBalance("ACC2");

            Assert.Multiple(() =>
            {
                Assert.That(account1.Amount + account2.Amount, Is.EqualTo(2000m));
                Assert.That(account1.Amount, Is.EqualTo(1000m));
                Assert.That(account2.Amount, Is.EqualTo(1000m));
            });
        }

        #endregion
    }
}
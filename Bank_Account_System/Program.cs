using System.ComponentModel;
using System.Security.Principal;

namespace Bank_Account_System
{
    enum AccountMenu
    {
        [Description("1. Create account")] Create,
        [Description("2. Deposit money")] Deposite,
        [Description("3. Withdraw money")] Withdraw,
        [Description("4. Show account details")] Show,
        [Description("5. Exit")] Exit
    }
    enum AccountType
    {
        [Description("1. Saving")] Saving,
        [Description("2. Current")] Current
    }
    public class BankAccount
    {
        
        public string AccountNumber { get; }
        protected string AccountType { get; set; }
        protected string OwnerName { get; }
        protected DateTime CreatedDate { get; }
        protected double Balance { get; set; }
        public BankAccount(string accountType, string accountNumber, string ownerName, DateTime createdDate, double balance)
        {
            if (string.IsNullOrEmpty(accountType))
            {
                throw new ArgumentNullException(nameof(accountType));
            }
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber));
            }
            if (string.IsNullOrEmpty(ownerName))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            if (balance < 0)
            {
                throw new ArgumentException("Balance cannot be negative");
            }
            AccountType = accountType;
            AccountNumber = accountNumber;
            OwnerName = ownerName;
            CreatedDate = createdDate;
            Balance = balance;
            Console.WriteLine($"Account Created {accountType}\t{accountNumber}\t{ownerName}\t{createdDate}\t{balance}\n===================");
        }
        public virtual void Deposit(double amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                  Console.WriteLine($"Deposited {amount:C} to {OwnerName}'s account. New balance: {Balance}\n===================");
            }
            else
            {
                Console.WriteLine("Deposit amount must be positive.");
            }
        }
        public virtual void Withdraw(double amount)
        {
            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount;
                 Console.WriteLine($"Withdrew {amount} from {OwnerName}'s account. New balance: {Balance}\n===================");
            }
            else
            {
                Console.WriteLine("Withdrawal amount must be positive and less than or equal to the current balance.\n===================");
            }
        }
        public void DisplayAccountInfo()
        {
            Console.WriteLine($"Account Type: {AccountType}");
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Owner Name: {OwnerName}");
            Console.WriteLine($"Created Date: {CreatedDate}");
            Console.WriteLine($"Balance: {Balance}");
        }

    }
    public class SavingAccount : BankAccount
    {
        public SavingAccount(string accountType, string accountNumber, string ownerName, DateTime createdDate, double balance) : base(accountType,accountNumber, ownerName, createdDate, balance)
        {

        }
        public override void Deposit(double amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"Deposited {amount:C} to {OwnerName}'s account. New balance: {Balance}\n===================");
            }
            else
            {
                Console.WriteLine("Deposit amount must be positive.");
            }

            var duration = DateTime.UtcNow.Month - CreatedDate.Month;
            if (duration > 0)
            {
                Balance += Balance * .50 * duration;
                Console.WriteLine($"Deposited {amount:C} to {OwnerName}'s account. New balance: {Balance}\n===================");
            }
        }
    }
    public class CurrentAccount : BankAccount
    {
        public CurrentAccount(string accountType, string accountNumber, string ownerName, DateTime createdDate, double balance) : base(accountType, accountNumber, ownerName, createdDate, balance)
        {

        }
        public override void Withdraw(double amount)
        {
            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount +10;
                Console.WriteLine($"Withdrew {amount} from {OwnerName}'s account. New balance: {Balance}");
            }
            else
            {
                Console.WriteLine("Withdrawal amount must be positive and less than or equal to the current balance.\n===================");
            }
        }
    }
    internal class Program
    {
        public static List<BankAccount> accounts = new List<BankAccount>();
        static void Main(string[] args)
        {
            while (true)
            {
                string _accountNumber = "";
                string _ownerName = "";
                string accno = "";
                Console.WriteLine("=== Account Menu ===");
                Console.WriteLine("1. Create account");
                Console.WriteLine("2. Deposit money");
                Console.WriteLine("3. Withdraw money");
                Console.WriteLine("4. Show account details");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                var selection = int.TryParse(Console.ReadLine(), out int entry);
                AccountMenu accountMenu = (AccountMenu)entry - 1;
                switch (accountMenu)
                {
                    case AccountMenu.Create:
                        Console.WriteLine("=== Account Type ===");
                        Console.WriteLine("1. Saving Account");
                        Console.WriteLine("2. Current Account");
                        selection = int.TryParse(Console.ReadLine(), out int entry2);
                        AccountType accountType = (AccountType)entry2 - 1;
                        Console.WriteLine("Enter Account Number");
                        _accountNumber = Console.ReadLine();
                        Console.WriteLine("Enter Owner Name");
                        _ownerName = Console.ReadLine();
                        Console.WriteLine("Enter Initial Balance");
                        var balanceChk = double.TryParse(Console.ReadLine(), out double balance);
                        switch (accountType)
                        {
                            case AccountType.Saving:
                                SavingAccount savingAccount = new SavingAccount("Saving",_accountNumber ?? "", _ownerName ?? "", DateTime.UtcNow, balance);
                                accounts.Add(savingAccount);
                                break;
                            case AccountType.Current:
                                CurrentAccount currentAccount = new CurrentAccount("Current",_accountNumber ?? "", _ownerName ?? "", DateTime.UtcNow, balance);
                                accounts.Add(currentAccount);
                                break;
                            default:
                                return;
                        }
                        break;
                    case AccountMenu.Deposite:
                        Console.WriteLine("Please Enter Account Number");
                        accno = Console.ReadLine();
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            if (accounts[i].AccountNumber == accno)
                            {
                                Console.WriteLine("Please Enter Amount");
                                accounts[i].Deposit(int.Parse(Console.ReadLine()));
                                break;
                            }
                        }
                        break;

                    case AccountMenu.Withdraw:
                        Console.WriteLine("Please Enter Account Number");
                        accno = Console.ReadLine();
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            if (accounts[i].AccountNumber == accno)
                            {
                                Console.WriteLine("Please Enter Amount");
                                accounts[i].Withdraw(int.Parse(Console.ReadLine()));
                                break;
                            }
                        }
                        break;

                    case AccountMenu.Show:
                        Console.WriteLine("Please Enter Account Number");
                        accno = Console.ReadLine();
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            if (accounts[i].AccountNumber == accno)
                            {
                                accounts[i].DisplayAccountInfo();
                                break;
                            }
                        }
                        break;
                    default:
                        return;
                }
            }
            
        }
    }
}

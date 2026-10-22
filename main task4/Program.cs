namespace main_task4
{
    using System;
    using System.Collections.Generic;

    public class Account
    {
        public string Name { get; set; }
        public double Balance { get; protected set; }

        public Account(string name = "Unnamed Account", double balance = 0.0)
        {
            Name = name;
            Balance = balance;
        }

        public virtual bool Deposit(double amount)
        {
            if (amount < 0)
                return false;

            Balance += amount;
            return true;
        }

        public virtual bool Withdraw(double amount)
        {
            if (amount < 0 || Balance - amount < 0)
                return false;

            Balance -= amount;
            return true;
        }

        public override string ToString() => $"[Account: {Name}, Balance: {Balance:F2}]";
    }

    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(string name = "Unnamed Savings Account", double balance = 0.0, double interestRate = 0.0)
            : base(name, balance)
        {
            InterestRate = interestRate;
        }

        public override bool Deposit(double amount)
        {
            if (amount < 0)
                return false;

          
            Balance += amount + (amount * InterestRate / 100.0);
            return true;
        }

        public override string ToString() => $"[Savings Account: {Name}, Balance: {Balance:F2}, Interest: {InterestRate:F2}%]";
    }

    public class CheckingAccount : Account
    {
        private const double WithdrawalFee = 1.50;

        public CheckingAccount(string name = "Unnamed Checking Account", double balance = 0.0)
            : base(name, balance)
        {
        }

        public override bool Withdraw(double amount)
        {
            if (amount < 0 || Balance - amount - WithdrawalFee < 0)
                return false;

            Balance -= amount + WithdrawalFee;
            return true;
        }

        public override string ToString() => $"[Checking Account: {Name}, Balance: {Balance:F2}]";
    }

    public class TrustAccount : SavingsAccount
    {
        private int withdrawalsThisYear;

        public TrustAccount(string name = "Unnamed Trust Account", double balance = 0.0, double interestRate = 0.0)
            : base(name, balance, interestRate)
        {
            withdrawalsThisYear = 0;
        }

        public override bool Deposit(double amount)
        {
            if (amount < 0)
                return false;

            
            base.Deposit(amount);
            if (amount >= 5000.00)
                Balance += 50.00;

            return true;
        }

        public override bool Withdraw(double amount)
        {
            if (amount < 0 || withdrawalsThisYear >= 3 || amount >= Balance * 0.20)
                return false;

            if (!base.Withdraw(amount))
                return false;

            withdrawalsThisYear++;
            return true;
        }

        public override string ToString() => $"[Trust Account: {Name}, Balance: {Balance:F2}, Interest: {InterestRate:F2}%, Withdrawals: {withdrawalsThisYear}/3]";
    }

    public static class AccountUtil
    {
        
        public static void Deposit(IEnumerable<Account> accounts, double amount)
        {
            Console.WriteLine($"\n=== Depositing {amount:F2} ===");
            foreach (Account acc in accounts)
            {
                if (acc.Deposit(amount))
                    Console.WriteLine($"Deposited {amount:F2} to {acc}");
                else
                    Console.WriteLine($"Failed deposit of {amount:F2} to {acc}");
            }
        }

        public static void Withdraw(IEnumerable<Account> accounts, double amount)
        {
            Console.WriteLine($"\n=== Withdrawing {amount:F2} ===");
            foreach (Account acc in accounts)
            {
                if (acc.Withdraw(amount))
                    Console.WriteLine($"Withdrew {amount:F2} from {acc}");
                else
                    Console.WriteLine($"Failed withdrawal of {amount:F2} from {acc}");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            
            var accounts = new List<Account>
        {
            new Account(),
            new Account("Larry"),
            new Account("Moe", 2000),
            new Account("Curly", 5000)
        };
            AccountUtil.Deposit(accounts, 1000);
            AccountUtil.Withdraw(accounts, 2000);

            
            var savAccounts = new List<SavingsAccount>
        {
            new SavingsAccount(),
            new SavingsAccount("Superman"),
            new SavingsAccount("Batman", 2000),
            new SavingsAccount("Wonderwoman", 5000, 5.0)
        };
            AccountUtil.Deposit(savAccounts, 1000);
            AccountUtil.Withdraw(savAccounts, 2000);

           
            var checAccounts = new List<CheckingAccount>
        {
            new CheckingAccount(),
            new CheckingAccount("Larry2"),
            new CheckingAccount("Moe2", 2000),
            new CheckingAccount("Curly2", 5000)
        };
            AccountUtil.Deposit(checAccounts, 1000);
            AccountUtil.Withdraw(checAccounts, 2000);
            AccountUtil.Withdraw(checAccounts, 2000);

            var trustAccounts = new List<TrustAccount>
        {
            new TrustAccount(),
            new TrustAccount("Superman2"),
            new TrustAccount("Batman2", 2000),
            new TrustAccount("Wonderwoman2", 5000, 5.0)
        };
            AccountUtil.Deposit(trustAccounts, 1000);
            AccountUtil.Deposit(trustAccounts, 6000);
            AccountUtil.Withdraw(trustAccounts, 2000);
            AccountUtil.Withdraw(trustAccounts, 3000);
            AccountUtil.Withdraw(trustAccounts, 500);

            Console.WriteLine("\nDone. Press any key to exit.");
            Console.ReadKey();
        }
    }

}

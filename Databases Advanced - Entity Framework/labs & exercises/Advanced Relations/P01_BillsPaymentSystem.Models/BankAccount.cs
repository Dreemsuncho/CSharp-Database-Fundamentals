namespace P01_BillsPaymentSystem.Models
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }
        public decimal Balance { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }


        public bool Withdraw(decimal money)
        {
            bool success = false;

            if (money <= Balance)
            { Balance -= money; success = true; }
            return success;
        }

        public void Deposit(decimal money)
        {
            if (money > 0)
                Balance += money;
        }
    }
}

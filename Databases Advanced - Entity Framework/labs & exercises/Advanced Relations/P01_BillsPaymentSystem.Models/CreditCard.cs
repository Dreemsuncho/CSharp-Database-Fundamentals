using System;

namespace P01_BillsPaymentSystem.Models
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }
        public decimal Limit { get; set; }
        public decimal MoneyOwed { get; set; }
        public DateTime ExpirationDate { get; set; }

        // calculated property, not included in the database
        public decimal LimitLeft { get { return Limit - MoneyOwed; } }

        public bool Withdraw(decimal money)
        {
            bool success = false;

            if (money <= LimitLeft)
            {
                Limit -= money;
                success = true;
            }
            return success;
        }

        public void Deposit(decimal money)
        {
            if (money > 0)
                Limit += money;
        }
    }
}

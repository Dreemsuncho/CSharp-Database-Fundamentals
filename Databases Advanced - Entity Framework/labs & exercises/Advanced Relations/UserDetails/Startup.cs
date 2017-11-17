using System;
using P01_BillsPaymentSystem.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UserDetails
{
    class Startup
    {
        static void Main()
        {
            Console.WriteLine("Enter a user id:");
            int userId = int.Parse(Console.ReadLine());

            using (var context = new BillsPaymentSystemContext())
            {

                var user = context.Users.Find(userId);
                if (user == null)
                {
                    Console.WriteLine($"User with id {userId} not found!");
                }
                else
                {
                    var paymentMethods = context.PaymentMethods
                        .Where(pm => pm.UserId == userId)
                        .Include(pm => pm.BankAccount)
                        .Include(pm => pm.CreditCard);

                    if (paymentMethods.Count() > 0)
                    {
                        var bankAccounts = paymentMethods.Select(pm => pm.BankAccount).ToList();
                        Console.WriteLine("Bank Accounts:");
                        foreach (var ba in bankAccounts)
                        {
                            if (ba != null)
                                Console.WriteLine($@"-- ID: {ba.BankAccountId}
--- Balance: {ba.Balance:f2}
--- Bank: {ba.BankName}
--- SWIFT: {ba.SwiftCode}");
                        }

                        var creditCards = paymentMethods.Select(pm => pm.CreditCard).ToList();
                        Console.WriteLine("Credit Cards:");
                        foreach (var cc in creditCards)
                        {
                            if (cc != null)
                                Console.WriteLine($@"-- ID: {cc.CreditCardId}
--- Limit: {cc.Limit:f2}
--- Moeny Owed: {cc.MoneyOwed:f2}
--- Limit Left: {cc.LimitLeft:f2}
--- Expiration Date: {cc.ExpirationDate:yyyy/MM}");
                        }
                    }
                }
            }
        }
    }
}

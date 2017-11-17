using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using P01_BillsPaymentSystem.Models;

namespace P01_BillsPaymentSystem.Data
{
    public class DbInitializer
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            if (!context.Users.Any())
            {
                string dataAsString = File.ReadAllText(Path.Combine("../P01_BillsPaymentSystem.Data/data.json"));
                var data = JObject.Parse(dataAsString);


                var users = new List<User>();
                data["Users"].ToList().ForEach(user =>
                {
                    var userProperties = user.ToArray();
                    users.Add(new User
                    {
                        FirstName = userProperties[0].Values().First().ToString(),
                        LastName = userProperties[1].Values().First().ToString(),
                        Email = userProperties[2].Values().First().ToString(),
                        Password = userProperties[3].Values().First().ToString()
                    });
                });

                var creditCards = new List<CreditCard>();
                data["CreditCards"].ToList().ForEach(creditCard =>
                {
                    var creditCardProperties = creditCard.ToArray();
                    creditCards.Add(new CreditCard
                    {
                        Limit = decimal.Parse(creditCardProperties[0].Values().First().ToString().Replace("$", "")),
                        MoneyOwed = decimal.Parse(creditCardProperties[1].Values().First().ToString().Replace("$", "")),
                        ExpirationDate = DateTime.Parse(creditCardProperties[2].Values().First().ToString())
                    });
                });


                var bankAccounts = new List<BankAccount>();
                data["BankAccounts"].ToList().ForEach(bankAccount =>
                {
                    var bankAccountProperties = bankAccount.ToArray();
                    bankAccounts.Add(new BankAccount
                    {
                        Balance = decimal.Parse(bankAccountProperties[0].Values().First().ToString().Replace("$", "")),
                        BankName = bankAccountProperties[1].Values().First().ToString(),
                        SwiftCode = bankAccountProperties[2].Values().First().ToString()
                    });
                });

                context.Users.AddRange(users);
                context.CreditCards.AddRange(creditCards);
                context.BankAccounts.AddRange(bankAccounts);
                context.SaveChanges();

                var payments = new List<PaymentMethod>();
                for (int i = 0; i < 1000; i++)
                {
                    var remaind = i % 90;
                    PaymentMethod payment;

                    if (i % 2 == 0)
                    {
                        payment = new PaymentMethod
                        {
                            UserId = users[remaind].UserId,
                            User = users[remaind],

                            CreditCardId = creditCards[i].CreditCardId,
                            //CreditCard = creditCards[i],
                            Type = PaymentType.CreditCard
                        };
                    }
                    else
                    {
                        payment = new PaymentMethod
                        {
                            UserId = users[remaind].UserId,
                            User = users[remaind],

                            BankAccountId = bankAccounts[i].BankAccountId,
                            //BankAccount = bankAccounts[i],
                            Type = PaymentType.BankAccount
                        };
                    }

                    payments.Add(payment);
                }

                context.PaymentMethods.AddRange(payments);
                context.SaveChanges();
            }
        }
    }
}

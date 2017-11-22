namespace BookShop
{
    using System;
    using System.Linq;
    using BookShop.Data;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Text;

    public class StartUp
    {
        static void Main()
        {
            //var input = Console.ReadLine();
            using (var db = new BookShopContext())
            {
                var result = RemoveBooks(db);
                Console.WriteLine(result);
            }
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksForDelete = context.Books.Where(b => b.Copies < 4200);
            var result = booksForDelete.Count();
            context.Books.RemoveRange(booksForDelete);
            /*int result = */context.SaveChanges();

            return result;
        }

        public static void IncreasePrices(BookShopContext context)
        {
            context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList()
                .ForEach(b => b.Price += 5);

            context.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var mostRecentBooks = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    CategoryBooks = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Select(cb => cb.Book)
                        .Take(3)
                }).ToList();

            var sb = new StringBuilder();

            mostRecentBooks.ForEach(mrb =>
            {
                sb.AppendLine($"--{mrb.CategoryName}");
                mrb.CategoryBooks
                    .ToList()
                    .ForEach(cb => sb.AppendLine($"{cb.Title} ({cb.ReleaseDate.Value.Year})"));
            });

            return sb.ToString();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoryProfits = context.Categories
                .Select(c => new { CategoryName = c.Name, TotalProfit = c.CategoryBooks.Sum(cb => cb.Book.Price * cb.Book.Copies) })
                .OrderByDescending(x => x.TotalProfit)
                .ThenBy(x => x.CategoryName)
                .Select(x => $"{x.CategoryName} ${x.TotalProfit.ToString()}");

            return string.Join(Environment.NewLine, categoryProfits);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorsWithNumberOfCopies = context.Authors
                .Select(a => new { AuthorName = $"{a.FirstName} {a.LastName}", BookCopies = a.Books.Sum(b => b.Copies) })
                .OrderByDescending(a => a.BookCopies)
                .Select(a => $"{a.AuthorName} - {a.BookCopies}");

            return string.Join(Environment.NewLine, authorsWithNumberOfCopies);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int booksCount = context.Books.Count(b => b.Title.Length > lengthCheck);
            return booksCount;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            input = input.ToLower();

            var bookTitlesWithAuthors = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})");

            return string.Join(Environment.NewLine, bookTitlesWithAuthors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            input = input.ToLower();

            var bookTitles = context.Books
                .Where(b => b.Title.ToLower().Contains(input))
                .Select(b => b.Title)
                .OrderBy(t => t);

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorNames = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(n => n);

            return string.Join(Environment.NewLine, authorNames);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateParsed = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < dateParsed)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - {b.Price:C}");

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .ToLower()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList();

            var bookTitles = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(t => t);

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var bookTitles = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var bookTitlesWithPrice = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - {b.Price:C}");

            return string.Join(Environment.NewLine, bookTitlesWithPrice);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var bookTitles = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            int ageRestrictionNumber = -1;

            switch (command.ToLower())
            {
                case "minor":
                    ageRestrictionNumber = (int)AgeRestriction.Minor;
                    break;
                case "teen":
                    ageRestrictionNumber = (int)AgeRestriction.Teen;
                    break;
                case "adult":
                    ageRestrictionNumber = (int)AgeRestriction.Adult;
                    break;
            }

            var bookTitles = context.Books
                .Where(b => (int)b.AgeRestriction == ageRestrictionNumber)
                .Select(b => b.Title)
                .OrderBy(t => t);

            return string.Join(Environment.NewLine, bookTitles);
        }
    }
}

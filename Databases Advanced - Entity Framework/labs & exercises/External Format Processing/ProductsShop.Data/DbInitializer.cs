using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductsShop.Models;
using System.Xml.XPath;
using Newtonsoft.Json.Linq;

namespace ProductsShop.Data
{
    public class DbInitializer
    {
        public static void Seed(ProductsShopContext context)
        {
            if (!context.Users.Any())
            {
                IList<User> users;
                IList<Product> products;
                IList<Category> categories;

                // import from json
                //ImportDataAsJson(out users, out products, out categories);

                // import from xml
                ImportDataAsXml(out users, out products, out categories);

                context.Users.AddRange(users);
                context.SaveChanges();

                int userId;
                User user;

                bool hasBuyer = false;

                Random random = new Random();
                foreach (var prod in products)
                {
                    userId = random.Next(users.Count);
                    user = users[userId];
                    prod.Seller = user;
                    prod.SellerId = user.Id;

                    if (hasBuyer)
                    {
                        userId = random.Next(users.Count);
                        user = users[userId];

                        user.ProductsBought.Add(prod);
                        prod.Buyer = user;
                        prod.BuyerId = user.Id;

                        prod.Seller.ProductsSold.Add(prod);
                        context.Entry(user).State = EntityState.Modified;
                    }
                    hasBuyer = !hasBuyer;
                }
                context.Products.AddRange(products);
                context.SaveChanges();

                int categoryId;
                Category category;
                foreach (var prod in products)
                {
                    categoryId = random.Next(categories.Count);
                    category = categories[categoryId];

                    var categoryProduct = new CategoryProduct
                    {
                        Product = prod,
                        ProductId = prod.Id,
                        Category = category,
                        CategoryId = categoryId
                    };
                    prod.CategoryProducts.Add(categoryProduct);
                    category.CategoryProducts.Add(categoryProduct);

                    context.Entry(prod).State = EntityState.Modified;
                }
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }

        private static void ImportDataAsJson(out IList<User> users, out IList<Product> products, out IList<Category> categories)
        {
            string jsonImportPath = @"../ProductsShop.Data/ImportExportData/JsonImport";

            string usersJsonString = File.ReadAllText(jsonImportPath + @"/users.json");
            string productsJsonString = File.ReadAllText(jsonImportPath + @"/products.json");
            string categoriesJsonString = File.ReadAllText(jsonImportPath + @"/categories.json");

            users = JsonConvert.DeserializeObject<IList<User>>(usersJsonString);
            products = JsonConvert.DeserializeObject<IList<Product>>(productsJsonString);
            categories = JsonConvert.DeserializeObject<IList<Category>>(categoriesJsonString);
        }

        private static void ImportDataAsXml(out IList<User> users, out IList<Product> products, out IList<Category> categories)
        {
            string xmlImportPath = @"../ProductsShop.Data/ImportExportData/XmlImport";

            // load, assembly and convert users
            var xmlUsers = XElement.Load(xmlImportPath + @"/users.xml");
            var usersJArray = new JArray();

            foreach (var item in xmlUsers.Elements())
                usersJArray.Add(new JObject(
                    new JProperty("firstName", item.Attribute("firstName")?.Value),
                    new JProperty("lastName", item.Attribute("lastName")?.Value),
                    new JProperty("age", item.Attribute("age")?.Value)));

            users = _ConvertToEntities<User>(usersJArray);


            // load, assembly and convert products
            var xmlProducts = XElement.Load(xmlImportPath + @"/products.xml");
            var productsJArray = new JArray();

            foreach (var item in xmlProducts.Elements())
                productsJArray.Add(new JObject(
                    new JProperty("name", item.Element("name")?.Value),
                    new JProperty("price", item.Element("price")?.Value)));

            products = _ConvertToEntities<Product>(productsJArray);


            // load, assembly and convert categories
            var xmlCategories = XElement.Load(xmlImportPath + @"/categories.xml");
            var categoriesJArray = new JArray();

            foreach (var item in xmlCategories.Elements())
                categoriesJArray.Add(new JObject(
                    new JProperty("name", item.Element("name")?.Value)));

            categories = _ConvertToEntities<Category>(categoriesJArray);
        }

        private static IList<T> _ConvertToEntities<T>(JArray jArray) => JsonConvert.DeserializeObject<IList<T>>(
                JsonConvert.SerializeObject(jArray, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
    }
}

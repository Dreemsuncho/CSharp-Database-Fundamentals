using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using AutoMapper.QueryableExtensions;
using ProductsShop.Data;
using ProductsShop.DTOs;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductsShop.Services
{
    class ProductShopService
    {
        private readonly ProductsShopContext _context;

        public ProductShopService(ProductsShopContext context)
        {
            _context = context;
        }

        public void ExportProducts_PriceRange500to1000Inclusive_ToJson(string path)
        {
            var products = _context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Include(p => p.Seller)
                .ProjectTo<ProductInRangeWithSellerDTO>()
                .ToList();

            _ExportToJson(path, products);
        }

        internal void ExportUsers_WithSoldProducts_ToJson(string path)
        {
            var users = _context.Users
                .Where(u => u.ProductsSold.All(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Include(u => u.ProductsSold)
                    .ThenInclude(p => p.Buyer)
                .ProjectTo<UserSuccessfullySoldProductDTO>()
                .ToList();

            _ExportToJson(path, users);
        }

        internal void ExportCategories_ByProductsCount_ToJson(string path)
        {
            var categories = _context.Categories
                .OrderBy(c => c.Name)
                .ProjectTo<CategoryDTO>()
                .ToList();

            _ExportToJson(path, categories);
        }


        internal void ExportUsersWithProducts_LeastOneSoldProduct_ToJson(string path)
        {
            var users = _context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .ThenBy(u => u.LastName)
                .ProjectTo<UserProductDTO>()
                .ToList();

            var usersToExport = new
            {
                UsersCount = users.Count,
                Users = users.Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new
                    {
                        Count = u.SoldProducts.Count,
                        Products = u.SoldProducts.Select(sp => new
                        {
                            Name = sp.Name,
                            Price = sp.Price
                        })
                    }
                })
            };
            _ExportToJson(path, usersToExport);
        }


        private static void _ExportToJson(string path, object users)
        {
            var jsonResult = JsonConvert.SerializeObject(users, Formatting.Indented);

            using (var streamWriter = File.CreateText(path))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                jsonWriter.Formatting = Formatting.Indented;

                var serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, users);
            }

            /* Simple Solution 1
               File.WriteAllText(path, jsonResult);
 
             * Simple Solution 2
               using (var sw = new StreamWriter(path))
               {
                   sw.Write(jsonResult);
               } 
            */
        }

        internal void ExportProducts_PriceRange1000to2000Inclusive_ToXml(string path)
        {
            var products = _context.Products
                .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.BuyerId != null)
                .OrderBy(p => p.Price)
                .ProjectTo<ProductInRangeWithBuyerDTO>()
                .ToList();

            _ExportProductsToXml(path, products);
        }

        internal void ExportUsers_WithSoldProducts_ToXml(string path)
        {
            var users = _context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .ProjectTo<UserProductDTO>()
                .ToList();

            _ExportUsersAtLeastOneSoldProductToXml(path, users);

        }

        internal void ExportUsers_WithSoldProducts_WithAttributesToXml(string path)
        {
            var users = _context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .ThenBy(u => u.LastName)
                .ProjectTo<UserProductDTO>()
                .ToList();

            _ExportUsersToXml(path, users);
        }

        private void _ExportUsersToXml(string path, List<UserProductDTO> users)
        {
            var xUsers = new XElement("users");
            users.ForEach(u =>
            {
                var xUser = new XElement("user");
                xUser.SetAttributeValue("first-name", u.FirstName);
                xUser.SetAttributeValue("last-name", u.LastName);
                xUser.SetAttributeValue("age", u.Age);

                var xSoldProducts = new XElement("sold-products");
                xSoldProducts.SetAttributeValue("count", u.SoldProducts.Count);

                u.SoldProducts.ToList().ForEach(p =>
                {
                    var xProduct = new XElement("product");
                    xProduct.SetAttributeValue("name", p.Name);
                    xProduct.SetAttributeValue("price", p.Price);
                    xSoldProducts.Add(xProduct);
                });
                xUser.Add(xSoldProducts);
                xUsers.Add(xUser);
            });
            xUsers.Save(path);
        }

        internal void ExportCategories_ByProductsCount_ToXml(string path)
        {
            var categories = _context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .ProjectTo<CategoryDTO>()
                .ToList();

            _ExportCategoriesToXml(path, categories);
        }

        private void _ExportCategoriesToXml(string path, List<CategoryDTO> categories)
        {
            var xCategories = new XElement("categories");
            categories.ForEach(c =>
            {
                var xCategory = new XElement("category");
                xCategory.SetAttributeValue("name", c.Category);
                xCategory.SetElementValue("product-count", c.ProductsCount);
                xCategory.SetElementValue("average-price", c.AveragePrice);
                xCategory.SetElementValue("total-revenue", c.TotalRevenue);

                xCategories.Add(xCategory);
            });

            xCategories.Save(path);
        }

        private void _ExportUsersAtLeastOneSoldProductToXml(string path, List<UserProductDTO> users)
        {
            var xUsers = new XElement("users");
            users.ForEach(u =>
            {
                var xUser = new XElement("User");
                xUser.SetAttributeValue("first-name", u.FirstName);
                xUser.SetAttributeValue("last-name", u.LastName);

                var xSoldProducts = new XElement("sold-products");
                u.SoldProducts.ToList().ForEach(p =>
                    xSoldProducts.Add(new XElement("product", new[] { new XElement("name", p.Name), new XElement("price", p.Price) })));

                xUser.Add(xSoldProducts);

                xUsers.Add(xUser);
            });

            xUsers.Save(path);
        }

        private void _ExportProductsToXml(string path, List<ProductInRangeWithBuyerDTO> products)
        {
            var jsonProducts = JsonConvert.SerializeObject(products)
                    .Replace("Name", "@Name")
                    .Replace("Price", "@Price")
                    .Replace("Buyer", "@Buyer");

            var xml = JsonConvert.DeserializeXNode("{'product':" + jsonProducts + "}", "products");
            xml.Save(path);
        }
    }
}


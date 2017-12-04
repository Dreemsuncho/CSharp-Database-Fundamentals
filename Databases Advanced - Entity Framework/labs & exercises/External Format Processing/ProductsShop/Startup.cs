using System.Linq;
using AutoMapper;
using ProductsShop.Data;
using ProductsShop.DTOs;
using ProductsShop.Models;
using ProductsShop.Services;

namespace ProductsShop
{
    class Program
    {
        static void Main()
        {
            using (var context = new ProductsShopContext())
            {
                _ConfigEntityMappings();
                _InitializeDatabase(context);

                var service = new ProductShopService(context);
                string exportJsonPath = "../ProductsShop.Data/ImportExportData/JsonExport";

                string path = Path(folderName: exportJsonPath, fileName: "products-in-range.json");
                service.ExportProducts_PriceRange500to1000Inclusive_ToJson(path);

                path = Path(folderName: exportJsonPath, fileName: "users-sold-products.json");
                service.ExportUsers_WithSoldProducts_ToJson(path);

                path = Path(folderName: exportJsonPath, fileName: "categories-by-products.json");
                service.ExportCategories_ByProductsCount_ToJson(path);

                path = Path(folderName: exportJsonPath, fileName: "users-and-products.json");
                service.ExportUsersWithProducts_LeastOneSoldProduct_ToJson(path);


                string exportXmlPath = "../ProductsShop.Data/ImportExportData/XmlExport";

                path = Path(folderName: exportXmlPath, fileName: "products-in-range.xml");
                service.ExportProducts_PriceRange1000to2000Inclusive_ToXml(path);

                path = Path(folderName: exportXmlPath, fileName: "users-sold-products.xml");
                service.ExportUsers_WithSoldProducts_ToXml(path);

                path = Path(folderName: exportXmlPath, fileName: "categories-by-products.xml");
                service.ExportCategories_ByProductsCount_ToXml(path);

                path = Path(folderName: exportXmlPath, fileName: "users-and-products.xml");
                service.ExportUsers_WithSoldProducts_WithAttributesToXml(path);
            }
        }

        private static string Path(string folderName, string fileName)
        {
            return System.IO.Path.Combine(folderName, fileName);
        }

        private static void _InitializeDatabase(ProductsShopContext context)
        {
            //context.Database.EnsureDeleted();
            //context.Database.Migrate();
            DbInitializer.Seed(context);
        }

        private static void _ConfigEntityMappings()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Product, ProductInRangeWithSellerDTO>()
                    .ForMember(d => d.Seller, options => options.MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}"));
                //--
                config.CreateMap<Product, ProductInRangeWithBuyerDTO>()
                    .ForMember(d => d.Buyer, options => options.MapFrom(src => $"{src.Buyer.FirstName} {src.Buyer.LastName}"));
                //--
                config.CreateMap<User, UserSuccessfullySoldProductDTO>()
                    .ForMember(d => d.FirstName, options => options.MapFrom(src => src.FirstName))
                    .ForMember(d => d.LastName, options => options.MapFrom(src => src.LastName))
                    .ForMember(d => d.SoldProducts, options => options.MapFrom(src => src.ProductsSold));
                config.CreateMap<Product, SoldProductDTO>()
                    .ForMember(d => d.Name, options => options.MapFrom(src => src.Name))
                    .ForMember(d => d.Price, options => options.MapFrom(src => src.Price))
                    .ForMember(d => d.BuyerFirstName, options => options.MapFrom(src => src.Buyer.FirstName))
                    .ForMember(d => d.BuyerLastName, options => options.MapFrom(src => src.Buyer.LastName));
                //--
                config.CreateMap<Category, CategoryDTO>()
                    .ForMember(d => d.Category, options => options.MapFrom(src => src.Name))
                    .ForMember(d => d.ProductsCount, options => options.MapFrom(src => src.CategoryProducts.Count))
                    .ForMember(d => d.AveragePrice, options => options.MapFrom(src => src.CategoryProducts.Average(p => p.Product.Price)))
                    .ForMember(d => d.TotalRevenue, options => options.MapFrom(src => src.CategoryProducts.Sum(p => p.Product.Price)));
                //--
                config.CreateMap<User, UserProductDTO>()
                    .ForMember(d => d.SoldProducts, options => options.MapFrom(src => src.ProductsSold));
                config.CreateMap<Product, UserSoldProductDTO>();
            });
        }
    }
}

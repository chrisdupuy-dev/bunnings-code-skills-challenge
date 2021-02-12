namespace BunningsCodeSkillsChallenge
{
    using System;
    using System.Linq;
    using Domain;
    using Domain.Interfaces;
    using Domain.Interfaces.Services;
    using Domain.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        private const string CompanyAName = "A";
        private const string SuppliersALocation = "./Input/SuppliersA.csv";
        private const string CatalogALocation = "./Input/CatalogA.csv";
        private const string BarcodesALocation = "./Input/BarcodesA.csv";

        private const string CompanyBName = "B";
        private const string SuppliersBLocation = "./Input/SuppliersB.csv";
        private const string CatalogBLocation = "./Input/CatalogB.csv";
        private const string BarcodesBLocation = "./Input/BarcodesB.csv";

        private static string[] DummyInput =
        {
            CompanyAName, SuppliersALocation, CatalogALocation, BarcodesALocation, 
            CompanyBName, SuppliersBLocation, CatalogBLocation, BarcodesBLocation
        };

        static void Main(string[] args)
        {
            if (args.Length % 4 != 0)
                throw new Exception("Invalid number of input values");

            var inputData = args.Length == 0 ? DummyInput : args;

            using (var serviceProvider = ConfigureServices(new ServiceCollection()))
            {
                var app = serviceProvider.GetService<IApplication>();

                for (var i = 0; i < inputData.Length; i += 4)
                {
                    app.ImportCompany(inputData[i], inputData[i+1], inputData[i+2], inputData[i+3]);
                }

                var running = true;
                while (running)
                {
                    switch (DisplayMenu().KeyChar)
                    {
                        case '1':
                            DisplayCommonCatalog(app);
                            break;
                        case '2':
                            ExportCommonCatalog(app);
                            break;
                        case '3':
                            AddProduct(app);
                            break;
                        case '4':
                            RemoveProduct(app);
                            break;
                        case '5':
                            AddBarcodesForProduct(app);
                            break;
                        case '6':
                            running = false;
                            break;
                    }
                }
            }
        }

        private static ServiceProvider ConfigureServices(ServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddLogging(c => c.AddConsole())
                .AddScoped<IImportExportService, CsvImportExportService>()
                .AddScoped<IMegaMergerService, MegaMergerService>()
                .AddScoped<ICompanyService, CompanyService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<ISupplierService, SupplierService>()
                .AddTransient<IApplication, BunningsCodeSkillsChallengeApplication>()
                .BuildServiceProvider();
        }

        private static ConsoleKeyInfo DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to BAU Mode!\n");
            Console.WriteLine("1. Display catalog");
            Console.WriteLine("2. Export catalog");
            Console.WriteLine("3. Add new product");
            Console.WriteLine("4. Remove existing product");
            Console.WriteLine("5. Add barcodes for product");
            Console.WriteLine("6. Exit");

            return Console.ReadKey();
        }

        private static void DisplayCommonCatalog(IApplication app)
        {
            Console.Clear();

            Console.WriteLine("SKU - Description - Source");

            var commonCatalog = app.GetCommonCatalog();
            foreach (var commonCatalogItem in commonCatalog.CommonCatalogItems.OrderBy(_ => _.Description))
            {
                Console.WriteLine($"{ commonCatalogItem.SKU } - {commonCatalogItem.Description} - {commonCatalogItem.Source}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void ExportCommonCatalog(IApplication app)
        {
            Console.Clear();

            Console.WriteLine("Exporting to results.csv...");
            
            app.ExportCommonCatalog("results.csv");
            
            Console.WriteLine("Success!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void AddProduct(IApplication app)
        {
            Console.Clear();

            Console.Write("Enter company name (case sensitive): ");
            var companyName = Console.ReadLine();

            Console.Write("Enter SKU: ");
            var sku = Console.ReadLine();

            Console.Write("Enter description: ");
            var description = Console.ReadLine();

            app.AddNewProduct(companyName, sku, description);

            Console.WriteLine("Success!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void RemoveProduct(IApplication app)
        {
            Console.Clear();

            Console.Write("Enter company name (case sensitive): ");
            var companyName = Console.ReadLine();

            Console.Write("Enter SKU: ");
            var sku = Console.ReadLine();

            app.RemoveProduct(companyName, sku);

            Console.WriteLine("Success!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void AddBarcodesForProduct(IApplication app)
        {
            Console.Clear();

            Console.Write("Enter company name (case sensitive): ");
            var companyName = Console.ReadLine();

            Console.WriteLine("Is this a new supplier (Y/N)?: ");
            var yesNo = Console.ReadKey();

            int supplierId;
            if (yesNo.Key == ConsoleKey.Y)
            {
                Console.WriteLine("Enter supplier name: ");
                var supplierName = Console.ReadLine();

                supplierId = app.AddSupplier(companyName, supplierName).ID;
            }
            else
            {
                Console.WriteLine("\n");

                foreach (var supplier in app.GetSuppliers(companyName))
                {
                    Console.WriteLine($"{supplier.ID}. {supplier.Name}");
                }

                Console.WriteLine("Enter supplier ID: ");
                supplierId = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("Enter SKU: ");
            var sku = Console.ReadLine();

            Console.WriteLine("Enter barcodes (can be comma separated): ");
            var barcodes = Console.ReadLine().Split(',').Select(_ => _.Trim());

            app.AddProductBarcodes(companyName, sku, supplierId, barcodes);

            Console.WriteLine("Success!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}

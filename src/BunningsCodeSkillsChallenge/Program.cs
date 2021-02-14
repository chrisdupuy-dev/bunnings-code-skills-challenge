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
        private const string SuppliersALocation = @".\Input\SuppliersA.csv";
        private const string CatalogALocation = @".\Input\CatalogA.csv";
        private const string BarcodesALocation = @".\Input\BarcodesA.csv";

        private const string CompanyBName = "B";
        private const string SuppliersBLocation = @".\Input\SuppliersB.csv";
        private const string CatalogBLocation = @".\Input\CatalogB.csv";
        private const string BarcodesBLocation = @".\Input\BarcodesB.csv";

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
                    try
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
                                AddCatalog(app);
                                break;
                            case '4':
                                RemoveCatalog(app);
                                break;
                            case '5':
                                AddBarcodesForCatalog(app);
                                break;
                            case '6':
                                running = false;
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"\nOops something went wrong: {e.Message}");
                        Console.WriteLine("\nPress any key to return to menu...");

                        Console.ReadKey();
                    }
                }
            }
        }

        private static ServiceProvider ConfigureServices(ServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddLogging(c => c.AddConsole())
                .AddScoped<IImportExportService, CsvImportExportService>()
                .AddScoped<ICommonCatalogService, CommonCatalogService>()
                .AddTransient<ICompanyManager, CompanyManager>()
                .AddScoped<ICatalogService, CatalogService>()
                .AddScoped<ISupplierService, SupplierService>()
                .AddScoped<ISupplierProductBarcodeService, SupplierProductBarcodeService>()
                .AddTransient<IApplication, BunningsCodeSkillsChallengeApplication>()
                .BuildServiceProvider();
        }

        private static ConsoleKeyInfo DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to BAU Mode!\n");
            Console.WriteLine("1. Display common catalog");
            Console.WriteLine("2. Export common catalog");
            Console.WriteLine("3. Add new catalog to company");
            Console.WriteLine("4. Remove existing catalog from company");
            Console.WriteLine("5. Add barcodes for catalog");
            Console.WriteLine("6. Exit");

            return Console.ReadKey();
        }

        private static void DisplayCommonCatalog(IApplication app)
        {
            Console.Clear();

            Console.WriteLine("SKU - Description - Source");

            var commonCatalogs = app.GetCommonCatalogs();
            foreach (var commonCatalogItem in commonCatalogs.OrderBy(_ => _.Description))
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
            
            app.ExportCommonCatalog($"{System.IO.Directory.GetCurrentDirectory()}\\results.csv");
            
            Console.WriteLine("Success!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void AddCatalog(IApplication app)
        {
            Console.Clear();

            Console.Write("Enter company name (case sensitive): ");
            var companyName = Console.ReadLine();

            Console.Write("Enter SKU: ");
            var sku = Console.ReadLine();

            Console.Write("Enter description: ");
            var description = Console.ReadLine();

            app.InsertCatalog(companyName, sku, description);

            Console.WriteLine("Success!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void RemoveCatalog(IApplication app)
        {
            Console.Clear();

            Console.Write("Enter company name (case sensitive): ");
            var companyName = Console.ReadLine();

            Console.Write("Enter SKU: ");
            var sku = Console.ReadLine();

            app.DeleteCatalog(companyName, sku);

            Console.WriteLine("Success!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void AddBarcodesForCatalog(IApplication app)
        {
            Console.Clear();

            Console.Write("Enter company name (case sensitive): ");
            var companyName = Console.ReadLine();

            Console.Write("Is this a new supplier (Y/N)?: ");
            var yesNo = Console.ReadKey();

            Console.WriteLine("\n");

            int supplierId;
            if (yesNo.Key == ConsoleKey.Y)
            {
                Console.Write("Enter supplier name: ");
                var supplierName = Console.ReadLine();

                supplierId = app.InsertSupplier(companyName, supplierName).ID;
            }
            else
            {
                foreach (var supplier in app.GetSuppliers(companyName))
                {
                    Console.WriteLine($"{supplier.ID}. {supplier.Name}");
                }

                Console.Write("\nEnter supplier ID: ");
                supplierId = int.Parse(Console.ReadLine());
            }

            Console.Write("Enter SKU: ");
            var sku = Console.ReadLine();

            Console.Write("Enter barcodes (can be comma separated): ");
            var barcodes = Console.ReadLine().Split(',').Select(_ => _.Trim());

            app.InsertSupplierProductBarcodes(companyName, sku, supplierId, barcodes);

            Console.WriteLine("Success!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}

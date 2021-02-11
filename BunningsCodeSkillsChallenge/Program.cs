namespace BunningsCodeSkillsChallenge
{
    using System;
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
                throw new Exception("Invalid input values");

            var inputData = args.Length == 0 ? DummyInput : args;

            using (var serviceProvider = ConfigureServices(new ServiceCollection()))
            {
                var app = serviceProvider.GetService<IApplication>();

                for (var i = 0; i < inputData.Length; i += 4)
                {
                    app.ImportCompany(inputData[i], inputData[i+1], inputData[i+2], inputData[i+3]);
                }

                app.ExportCommonCatalog("results.csv");
            }
        }

        private static ServiceProvider ConfigureServices(ServiceCollection serviceCollection)
        {
            return serviceCollection.AddLogging(c => c.AddConsole())
                .AddScoped<IImportExportService, CsvImportExportService>()
                .AddScoped<IMegaMergerService, MegaMergerService>()
                .AddTransient<IApplication, BunningsCodeSkillsChallengeApplication>()
                .BuildServiceProvider();
        }
    }
}

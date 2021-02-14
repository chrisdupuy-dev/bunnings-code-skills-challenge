# Bunnings Code Skills Challenge
## Dependencies
* **.NET Core 5.0** - [Windows](https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-5.0.103-windows-x64-installer), [Mac](https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-5.0.103-macos-x64-installer) or [Linux](https://docs.microsoft.com/en-us/dotnet/core/install/linux)

## Building the app from Visual Studio 2019 (Recommended)
Right-click the '**BunningsCodeSkillsChallenge**' solution and click build

## Building the app from command-line
From the .\src\ directory run the following command to build the solution:
    
    dotnet build -c {debug|release} BunningsCodeSkillsChallenge.sln

## Running the tests from Visual Studio 2019 (Recommended)
Right-click the '**BunningsCodeSkillsChallenge.UnitTests**' or '**BunningsCodeSkillsChallenge.IntegrationTests**' projects and click '**Run Unit Tests**'

## Running the tests from command-line
From the build_output directory (.\src\build_output\\{debug|release}\net5.0\\) run the following command to run the unit tests for the solution after building:
    
    dotnet test .\BunningsCodeSkillsChallenge.UnitTests.dll
    dotnet test .\BunningsCodeSkillsChallenge.IntegrationTests.dll

## Running the app from Visual Studio 2019 (Recommended)
Set **'BunningsCodeSkillsChallenge'** project as the start-up project and click run, this will use the provided test data

## Running the app from command-line
From the build_output directory (.\src\build_output\\{debug|release}\net5.0\\) if you wish to run the program with the provided test data simply run the following command:

    dotnet .\BunningsCodeSkillsChallenge.dll

Or if you you wish to input your own data you may do the following command:

    dotnet .\BunningsCodeSkillsChallenge.dll <CompanyName> <PathToSuppliersCsv> <PathToCatalogCsv> <PathToBarcodesCsv>

*Note: Repeat the pattern of '\<CompanyName> \<PathToSuppliersCsv> \<PathToCatalogCsv> \<PathToBarcodesCsv>' to add additional companies*

**Example**

    dotnet BunningsCodeSkillsChallenge.dll A .\Input\suppliersA.csv .\Input\catalogA.csv .\Input\barcodesA.csv B .\Input\suppliersB.csv .\Input\catalogB.csv .\Input\barcodesB.csv 

## Design
A basic command-line interface is used to facilate the use of various functions in the  'BunningsCodeSkillsChallengeApplication', this application class has all the services it requires to import, export and manipulate catalogs injected into it on construction. The data imported into the application is housed in a 'Company' where the company specific catalog is validated on any change to maintain data integrity, these 'Company' imports are then stored and maintained by the 'CompanyManger' for later reference.

The 'SupplierService', 'CatalogService' and 'SupplierProductBarcodeService' take care of their respective entity classes when it comes to basic CRUD operations whilst the 'CsvImportExportService' makes use of the 'CsvHelper' package to import and export files.

Lastly the 'CommonCatalogService' is responsible for merging multiple companies catalogs together into a 'CommonCatalog', this is done by iterating over each company and adding any catalog with available barcodes to the 'CommonCatalog' then checking against each other companies catalogs for matching barcodes. If a matching barcode is found the SKU for that match is added to an ignore hashset to prevent duplication however if a SKU matches with no matching barcodes then it is not possible to determine if it is the same product and therfore an exception is thrown. 

*Note: Exporting 'CommonCatalog' automatically exports to 'results.csv' within the current working directory*

## Areas for improvement
* Whilst the merging of catalogs is functional it could be done more efficiently, the values used in the HashSet to keep track of what catalogs have already been added to the system are also not collision safe
* Currently when a catalog is modified in the system the entire common catalog is reloaded, this could be optimised more to not reload the entire common catalog if not needed or to more intelligently add/remove single catalogs through an event based system
* I added logging to the application but did not make use of it anywhere as I did not think for the coding challenge it would add much value and it was more to show how logging could be fed into the system through dependency injection
* Generic exceptions are currently thrown in error scenarios but I do not catch or handle these besides a 'catch all' in the console application itself just to stop it from crashing when an exception occurs, this could be improved with more time to handle specific scenarios better
* User input and experience could be improved as it does not provide any pre-emptive input validation, everything is case-senstive and only provides exiting back to the menu as a error recovery

## Assumptions
* Company names must be unique
* Only catalogs with barcodes available are to be shown in common catalog
* SKUs must be unique within a Company
* Both Catalog SKU and Supplier ID must exist for a SupplierProductBarcode to exist
* If two companies have the same SKU but no matching barcodes I assume an unrecoverable conflict has occured and throw an exception, this could easily be changed to "skip" that SKU though if needed
* The order of the common catalog does not matter provided the data is the same, I sorted by description
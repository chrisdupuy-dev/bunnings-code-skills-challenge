namespace BunningsCodeSkillsChallenge.Domain.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Models.Entities;
    using Models;

    public interface ISupplierProductBarcodeService
    {
        IEnumerable<SupplierProductBarcode> GetSupplierProductBarcodesForProduct(ICompany company, string sku);
        IEnumerable<SupplierProductBarcode> InsertSupplierProductBarcodes(ICompany company, int supplierId, 
            string sku, IEnumerable<string> barcodes);
    }
}

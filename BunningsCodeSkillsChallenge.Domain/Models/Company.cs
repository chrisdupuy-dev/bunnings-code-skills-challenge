namespace BunningsCodeSkillsChallenge.Domain.Models
{
    using System.Collections.Generic;

    public class Company
    {
        public IEnumerable<Catalog> Catalogs { get; set; }
        public IEnumerable<SupplierProductBarcode> SupplierProductBarcodes { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}

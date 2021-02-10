namespace BunningsCodeSkillsChallenge.Domain.Models
{
    using System.Collections.Generic;

    public class Company
    {
        public string Name { get; set; }

        public List<Catalog> Catalogs { get; set; }
        public List<SupplierProductBarcode> SupplierProductBarcodes { get; set; }
        public List<Supplier> Suppliers { get; set; }

        public Company()
        {
            Catalogs = new List<Catalog>();
            SupplierProductBarcodes = new List<SupplierProductBarcode>();
            Suppliers = new List<Supplier>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public string? Barcode { get; set; }
        public int CategoryId { get; set; }

        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal VatPercent { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        public Category Category { get; set; } = default!;
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}

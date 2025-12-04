using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string SKU { get; set; } = default!;
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal VatPercent { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
    }
}

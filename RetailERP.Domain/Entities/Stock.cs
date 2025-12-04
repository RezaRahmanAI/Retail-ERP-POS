using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Domain.Entities
{
    public class Stock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int BranchId { get; set; }   

        public decimal Quantity { get; set; }

        // Navigation
        public Product Product { get; set; } = default!;
        public Branch Branch { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Domain.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Code { get; set; }
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}

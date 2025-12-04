using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}

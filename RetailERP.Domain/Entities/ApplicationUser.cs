using Microsoft.AspNetCore.Identity;

namespace RetailERP.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = default!;
}

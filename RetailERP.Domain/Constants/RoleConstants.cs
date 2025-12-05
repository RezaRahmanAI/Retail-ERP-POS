using RetailERP.Domain.Enums;

namespace RetailERP.Domain.Constants;

public static class RoleConstants
{
    public const string Admin = nameof(UserRole.Admin);
    public const string Manager = nameof(UserRole.Manager);
    public const string Cashier = nameof(UserRole.Cashier);

    public const string AdminAndManager = Admin + "," + Manager;
    public const string AllStaff = Admin + "," + Manager + "," + Cashier;
}

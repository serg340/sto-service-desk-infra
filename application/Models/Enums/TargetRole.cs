namespace STO_Desk_backend.Models.Enums
{
    public enum TargetRole
    {
        Mechanic = 0,

        StoOwner = 1,

        NewStoOwner = 2,

        Operator = 3,

        Admin = 4,
    }

    public static class TargetRoleExtensions
    {
        public static string GetTargetRoleName(this TargetRole role)
        {
            return role switch
            {
                TargetRole.Mechanic => "Mechanic",
                TargetRole.StoOwner => "StoOwner",
                TargetRole.NewStoOwner => "StoOwner",
                TargetRole.Operator => "Operator",
                TargetRole.Admin => "Admin",
                _ => string.Empty
            };
        }
    }
}

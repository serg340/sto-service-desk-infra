namespace STO_Desk_backend.Models.Enums
{
    /// <summary>
    /// Used to represent an entity type for polymorhic key connetions. <br></br>
    /// (currently has 3 types)
    /// </summary>
    public enum EntityType
    {
        Sto = 0,

        Ticket = 1,

        RoleTicket = 2,
    }
}

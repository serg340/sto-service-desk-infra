using System.ComponentModel.DataAnnotations;

namespace STO_Desk_backend.Models.Enums
{
    /// <summary>
    /// Represents the status of a Ticket in the monitoring process.
    /// </summary>
    public enum TicketStatus
    {
        [Display(Name = "Очікування")]
        Pending = 1,

        [Display(Name = "Розглядається")]
        UnderReview = 2,

        [Display(Name = "Призначений")]
        Assigned = 3,

        [Display(Name = "Виконаний")]
        Completed = 4,

        [Display(Name = "Відмінений")]
        Canceled = 5,

        [Display(Name = "Відхилений")]
        Rejected = 6
    }
}

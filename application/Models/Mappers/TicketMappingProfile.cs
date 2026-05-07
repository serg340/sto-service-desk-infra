using AutoMapper;
using STO_Desk_backend.Models.DTOs.Tickets;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Mappers
{
    public class TicketMappingProfile : Profile
    {
        public TicketMappingProfile()
        {
            CreateMap<Ticket, TicketDto>();
        }
    }
}

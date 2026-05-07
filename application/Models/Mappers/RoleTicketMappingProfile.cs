using AutoMapper;
using STO_Desk_backend.Models.DTOs.RoleTickets;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Mappers
{
    public class RoleTicketMappingProfile : Profile
    {
        public RoleTicketMappingProfile()
        {
            CreateMap<RoleTicket, RoleTicketDto>();
        }
    }
}

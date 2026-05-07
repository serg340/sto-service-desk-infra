using AutoMapper;
using STO_Desk_backend.Models.DTOs.TicketCategories;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Mappers
{
    public class TicketCategoryMappingProfile : Profile
    {
        public TicketCategoryMappingProfile()
        {
            CreateMap<TicketCategory, TicketCategoryDto>();
        }
    }
}

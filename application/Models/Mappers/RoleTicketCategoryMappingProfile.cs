using AutoMapper;
using STO_Desk_backend.Models.DTOs.RoleTicketCategories;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Mappers
{
    public class RoleTicketCategoryMappingProfile : Profile
    {
        public RoleTicketCategoryMappingProfile()
        {
            CreateMap<RoleTicketCategory, RoleTicketCategoryDto>();
        }
    }
}

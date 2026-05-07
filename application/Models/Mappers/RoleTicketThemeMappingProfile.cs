using AutoMapper;
using STO_Desk_backend.Models.DTOs.RoleTicketThemes;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Mappers
{
    public class RoleTicketThemeMappingProfile : Profile
    {
        public RoleTicketThemeMappingProfile()
        {
            CreateMap<RoleTicketTheme, RoleTicketThemeDto>();
        }
    }
}

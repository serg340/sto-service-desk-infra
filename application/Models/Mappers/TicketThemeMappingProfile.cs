using AutoMapper;
using STO_Desk_backend.Models.DTOs.TicketThemes;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Mappers
{
    public class TicketThemeMappingProfile : Profile
    {
        public TicketThemeMappingProfile()
        {
            CreateMap<TicketTheme, TicketThemeDto>();
        }
    }
}

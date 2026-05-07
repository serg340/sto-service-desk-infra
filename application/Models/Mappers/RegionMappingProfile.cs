using AutoMapper;
using STO_Desk_backend.Models.DTOs.Regions;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Mappers
{
    public class RegionMappingProfile : Profile
    {
        public RegionMappingProfile()
        {
            CreateMap<Region, RegionDto>();
        }
    }
}

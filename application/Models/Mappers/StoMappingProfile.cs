using AutoMapper;
using STO_Desk_backend.Models.DTOs.Stos;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Mappers
{
    public class StoMappingProfile : Profile
    {
        public StoMappingProfile()
        {
            CreateMap<Sto, StoDto>();
        }
    }
}

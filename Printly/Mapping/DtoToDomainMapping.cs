using AutoMapper;
using MongoDB.Bson;

namespace Printly.Mapping
{
    public class DtoToDomainMapping : Profile
    {
        public DtoToDomainMapping()
        {
            CreateMap<Dto.Request.Printer, Domain.Models.Printer>()
                .ForMember(dest => dest.BedSizeX, src => src.MapFrom(x => x.BedSize.X))
                .ForMember(dest => dest.BedSizeY, src => src.MapFrom(x => x.BedSize.Y))
                .ForMember(dest => dest.BedSizeZ, src => src.MapFrom(x => x.BedSize.Z));
        }
    }
}

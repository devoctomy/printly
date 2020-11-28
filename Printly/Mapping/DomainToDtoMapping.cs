using AutoMapper;

namespace Printly.Mapping
{
    public class DomainToDtoMapping : Profile
    {
        public DomainToDtoMapping()
        {
            CreateMap<Domain.Models.Printer, Dto.Response.Printer>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id.ToString()))
                .ForMember(dest => dest.BedSize, src => src.MapFrom(x => MapBedSizeToDimensionResponse(x)));

            CreateMap<Dto.Request.Printer, Domain.Models.Printer>();
        }

        private Dto.Response.Dimensions MapBedSizeToDimensionResponse(Domain.Models.Printer printer)
        {
            return new Dto.Response.Dimensions()
            {
                X = printer.BedSizeX,
                Y = printer.BedSizeY,
                Z = printer.BedSizeZ
            };
        }
    }
}

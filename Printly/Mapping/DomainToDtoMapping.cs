using AutoMapper;

namespace Printly.Mapping
{
    public class DomainToDtoMapping : Profile
    {
        public DomainToDtoMapping()
        {
            CreateMap<Domain.Models.Printer, Dto.Response.Printer>();

            CreateMap<Dto.Request.Printer, Domain.Models.Printer>();
        }
    }
}

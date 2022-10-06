using AutoMapper;
using MicroBeard.Entities.DataTransferObjects;
using MicroBeard.Entities.Models;

namespace MicroBeard
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactDto>();
        }
    }
}

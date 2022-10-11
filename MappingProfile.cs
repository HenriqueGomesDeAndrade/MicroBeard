using AutoMapper;
using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.Models;

namespace MicroBeard
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Contact
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactCreationDto, Contact>();
            CreateMap<ContactUpdateDto, Contact>();

            //Collaborator
            CreateMap<Collaborator, CollaboratorDto>();
            CreateMap<CollaboratorCreationDto, Collaborator>();
            CreateMap<CollaboratorUpdateDto, Collaborator>();
        }
    }
}

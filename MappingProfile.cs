using AutoMapper;
using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.DataTransferObjects.License;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.DataTransferObjects.Scheduling;

using MicroBeard.Entities.Models;
using System.Net;

namespace MicroBeard
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Contact
            CreateMap<Contact, ContactDto>();
            CreateMap<Contact, SimpleContactDto>();
            CreateMap<ContactCreationDto, Contact>();
            CreateMap<ContactUpdateDto, Contact>();

            //Collaborator
            CreateMap<Collaborator, CollaboratorDto>();
            CreateMap<Collaborator, SimpleCollaboratorDto>();
            CreateMap<CollaboratorCreationDto, Collaborator>();
            CreateMap<CollaboratorUpdateDto, Collaborator>();

            //License
            CreateMap<License, LicenseDto>();
            CreateMap<License, SimpleLicenseDto>();
            CreateMap<LicenseCreationDto, License>();
            CreateMap<LicenseUpdateDto, License>();

            //Service
            CreateMap<Service, ServiceDto>();
            CreateMap<Service, SimpleServiceDto>();
            CreateMap<ServiceCreationDto, Service>();
            CreateMap<ServiceUpdateDto, Service>();

            //Scheduling
            CreateMap<Scheduling, SchedulingDto>()
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.ServiceCodeNavigation))
                .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.ContactCodeNavigation));
            CreateMap<Scheduling, SimpleSchedulingDto>();
            CreateMap<SchedulingCreationDto, Scheduling>();
            CreateMap<SchedulingUpdateDto, Scheduling>();
        }
    }
}

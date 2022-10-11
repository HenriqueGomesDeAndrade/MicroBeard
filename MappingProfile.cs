using AutoMapper;
using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.DataTransferObjects.License;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.DataTransferObjects.Scheduling;

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

            //License
            CreateMap<License, LicenseDto>();
            CreateMap<LicenseCreationDto, License>();
            CreateMap<LicenseUpdateDto, License>();

            //Service
            CreateMap<Service, ServiceDto>();
            CreateMap<ServiceCreationDto, Service>();
            CreateMap<ServiceUpdateDto, Service>();

            //Scheduling
            CreateMap<Scheduling, SchedulingDto>();
            CreateMap<SchedulingCreationDto, Scheduling>();
            CreateMap<SchedulingUpdateDto, Scheduling>();
        }
    }
}

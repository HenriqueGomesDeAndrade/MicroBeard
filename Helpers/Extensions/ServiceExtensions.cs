using AutoMapper;
using AutoMapper.EntityFrameworkCore;
using AutoMapper.EquivalencyExpression;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.DataTransferObjects.License;
using MicroBeard.Entities.DataTransferObjects.Scheduling;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.Models;
using MicroBeard.Helpers.Sort;
using MicroBeard.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Runtime.CompilerServices;
using System.Text;

namespace MicroBeard.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        public static void ConfigureSqlServer(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("MicroBeardCS");
            services.AddDbContext<MicroBeardContext>(o => {
                o.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging().EnableDetailedErrors();
                ; }, contextLifetime: ServiceLifetime.Scoped);
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<ISortHelper<Contact>, SortHelper<Contact>>();
            services.AddScoped<ISortHelper<Collaborator>, SortHelper<Collaborator>>();
            services.AddScoped<ISortHelper<License>, SortHelper<License>>();
            services.AddScoped<ISortHelper<Scheduling>, SortHelper<Scheduling>>();
            services.AddScoped<ISortHelper<Service>, SortHelper<Service>>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration _config)
        {
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("TokenKey"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "MicroBeard API",
                    Version = "v1" 
                });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, "MicroBeard.xml");
                option.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper((serviceProvider, automapper) =>
            {
                automapper.AddCollectionMappers();
                automapper.UseEntityFrameworkCoreModel<MicroBeardContext>(serviceProvider);

                //Contact
                automapper.CreateMap<Contact, ContactDto>();
                automapper.CreateMap<Contact, SimpleContactDto>().ReverseMap().EqualityComparison((odto, o) => odto.Code == o.Code);
                automapper.CreateMap<ContactCreationDto, Contact>();
                automapper.CreateMap<ContactUpdateDto, Contact>();

                //Collaborator
                automapper.CreateMap<Collaborator, CollaboratorDto>();
                automapper.CreateMap<Collaborator, SimpleCollaboratorDto>().ReverseMap().EqualityComparison((odto, o) => odto.Code == o.Code);
                automapper.CreateMap<CollaboratorCreationDto, Collaborator>();
                automapper.CreateMap<CollaboratorUpdateDto, Collaborator>();

                //License
                automapper.CreateMap<License, LicenseDto>();
                automapper.CreateMap<License, SimpleLicenseDto>().ReverseMap().EqualityComparison((odto, o) => odto.Code == o.Code);
                automapper.CreateMap<LicenseCreationDto, License>();
                automapper.CreateMap<LicenseUpdateDto, License>();

                //Service
                automapper.CreateMap<Service, ServiceDto>()
                .ForMember(dest => dest.License, opt => opt.MapFrom(src => src.LicenseCodeNavigation));

                automapper.CreateMap<Service, SimpleServiceDto>().ReverseMap().EqualityComparison((odto, o) => odto.Code == o.Code);
                automapper.CreateMap<ServiceCreationDto, Service>();
                automapper.CreateMap<ServiceUpdateDto, Service>();

                //Scheduling
                automapper.CreateMap<Scheduling, SchedulingDto>()
                    .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.ServiceCodeNavigation))
                    .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.ContactCodeNavigation))
                    .ForMember(dest => dest.Collaborator, opt => opt.MapFrom(src => src.CollaboratorCodeNavigation));
                automapper.CreateMap<Scheduling, SimpleSchedulingDto>().ReverseMap();
                automapper.CreateMap<SchedulingCreationDto, Scheduling>();
                automapper.CreateMap<SchedulingUpdateDto, Scheduling>();
            }, typeof(MicroBeardContext).Assembly);

            ServiceProvider service = services.BuildServiceProvider();
        }
    }
}

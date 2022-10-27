using AutoMapper;
using MicroBeard.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace MicroBeard.Controllers 
{
    public class MicroBeardController : ControllerBase
    {
        protected int ContactId { get; set; }
        protected int CollaboratorId { get; set; }

        protected readonly ILoggerManager _logger;
        protected readonly IRepositoryWrapper _repository;
        protected readonly IMapper _mapper;
        public MicroBeardController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }


        protected virtual void Initialize(System.Web.Routing.RequestContext controllerContext)
        {
            base.Initialize(controllerContext);
        }
    }
}

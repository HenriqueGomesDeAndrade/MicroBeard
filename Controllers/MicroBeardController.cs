using AutoMapper;
using MicroBeard.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroBeard.Controllers 
{
    public class MicroBeardController : Controller
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

        
        [ApiExplorerSettings(IgnoreApi = true)]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            Authenticate(context);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void Authenticate(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers.Authorization;


        }
    }
}

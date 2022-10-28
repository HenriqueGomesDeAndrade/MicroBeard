using AutoMapper;
using AutoMapper.Internal;
using MicroBeard.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;
using System.Security.Claims;

namespace MicroBeard.Controllers 
{
    [Authorize]
    public class MicroBeardController : Controller
    {
        protected int? ContactCode { get; set; }
        protected int? CollaboratorCode { get; set; }

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
            FillRolesInfo();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void FillRolesInfo()
        {
            if (User.IsInRole("Contact"))
                ContactCode = int.Parse(User.FindFirstValue("Code"));
            else if (User.IsInRole("Collaborator"))
                CollaboratorCode = int.Parse(User.FindFirstValue("Code"));
        }
    }
}

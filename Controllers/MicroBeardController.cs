using AutoMapper;
using AutoMapper.Internal;
using MicroBeard.Contracts;
using MicroBeard.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MicroBeard.Controllers 
{
    [Authorize(Roles ="Collaborator, Contact")]
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
            IActionResult valitationResult = ValidadeTokenAndFillInfos();
            if (valitationResult != null)
                context.Result = valitationResult;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ValidadeTokenAndFillInfos()
        {
            if (User.IsInRole("Contact"))
            {
                var token = ControllerContext.HttpContext.Request.Headers.Authorization.ToString().Split(' ')[1];

                ContactCode = int.Parse(User.FindFirstValue("Code"));
                var contact = _repository.Contact.GetContactByCode((int)ContactCode);
                if (contact.Token != token)
                    return Unauthorized("Invalid Token");
                return null;
            }
            else if (User.IsInRole("Collaborator"))
            {
                var token = ControllerContext.HttpContext.Request.Headers.Authorization.ToString().Split(' ')[1];

                CollaboratorCode = int.Parse(User.FindFirstValue("Code"));
                var collaborator = _repository.Collaborator.GetCollaboratorByCode((int)CollaboratorCode);
                if (collaborator.Token != token)
                    return Unauthorized("Invalid Token");
                return null;
            }
            else
                return null;
        }
    }
}

using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.Internal.Mappers;
using MicroBeard.Contracts;
using MicroBeard.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MicroBeard.Controllers 
{
    [EnableCors("CorsPolicy")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize(Roles ="Collaborator, Contact, CollaboratorAdmin")]
    public class MicroBeardController : Controller
    {
        protected int? ContactCode { get; set; }
        protected int? CollaboratorCode { get; set; }

        protected readonly IRepositoryWrapper _repository;
        protected readonly IMapper _mapper;
        public MicroBeardController(IRepositoryWrapper repository, IMapper mapper)
        {
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
                string contactToken = _repository.Contact.GetContactByCode((int)ContactCode).Token;
                if (contactToken != token)
                    return Unauthorized("Invalid Token");
                return null;
            }
            else if (User.IsInRole("Collaborator") || User.IsInRole("CollaboratorAdmin"))
            {
                var token = ControllerContext.HttpContext.Request.Headers.Authorization.ToString().Split(' ')[1];

                CollaboratorCode = int.Parse(User.FindFirstValue("Code"));
                string collaboratorToken = _repository.Collaborator.GetCollaboratorByCode((int)CollaboratorCode).Token;
                if (collaboratorToken != token)
                    return Unauthorized("Invalid Token");
                return null;
            }
            else
                return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetFullException(Exception ex)
        {
            string innerException = ex.Message;
            while (ex.InnerException != null)
            {
                innerException += $" Inner Exception: {ex.InnerException.Message}";
                ex = ex.InnerException;
            }
            return innerException;
        }
    }
}

using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        public ContactController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllContacts()
        {
            try
            {
                var contacts = _repository.Contact.GetAllContacts();
                _logger.LogInfo($"Returned all owners from database");

                var contactsResult = _mapper.Map<IEnumerable<ContactDto>>(contacts);

                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{code}")]
        public IActionResult GetContactByCode(int code)
        {
            try
            {
                var contact = _repository.Contact.GetContactByCode(code);

                if (contact is null)
                {
                    _logger.LogError($"Contact with code: {code} hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"returned owner with code: {code}");
                var contactResult = _mapper.Map<ContactDto>(contact);

                return Ok(contactResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

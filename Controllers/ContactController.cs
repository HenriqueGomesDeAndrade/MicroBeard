using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.Models;
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
                IEnumerable<Contact> contacts = _repository.Contact.GetAllContacts();
                _logger.LogInfo($"Returned all contacts from database");

                IEnumerable<ContactDto> contactsResult = _mapper.Map<IEnumerable<ContactDto>>(contacts);

                return Ok(contactsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllContacts action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{code}", Name = "ContactByCode")]
        public IActionResult GetContactByCode(int code)
        {
            try
            {
                Contact contact = _repository.Contact.GetContactByCode(code);

                if (contact is null)
                {
                    _logger.LogError($"Contact with code: {code} hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"returned contact with code: {code}");
                ContactDto contactResult = _mapper.Map<ContactDto>(contact);

                return Ok(contactResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetContactByCode action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{code}/schedullings")]
        public IActionResult GetContactWithDetails(int code)
        {
            try
            {
                Contact contact = _repository.Contact.GetContactWithDetails(code);

                if (contact is null)
                {
                    _logger.LogError($"Contact with code: {code} hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"returned contact with code: {code}");
                ContactDto contactResult = _mapper.Map<ContactDto>(contact);

                return Ok(contactResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetContactByCode action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateContact([FromBody] ContactCreationDto contact)
        {
            try
            {
                if (contact is null)
                {
                    _logger.LogError("Contact object sent from client is null");
                    return BadRequest("Contact object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid contact object sent from client");
                    return BadRequest("Invalid model object");
                }

                Contact contactEntity = _mapper.Map<Contact>(contact);

                contactEntity.CreateDate = DateTime.Now;
                //contactEntity.CreatorCode = CollaboratorCode;

                _repository.Contact.CreateContact(contactEntity);
                _repository.Save();

                ContactDto createdContact = _mapper.Map<ContactDto>(contactEntity);

                return CreatedAtRoute("ContactByCode", new { code = createdContact.Code }, createdContact);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateContact action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{code}")]
        public IActionResult UpdateContact(int code, [FromBody] ContactUpdateDto contact)
        {
            try
            {
                if (contact is null)
                {
                    _logger.LogError("Contact object sent from client is null");
                    return BadRequest("Contact object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid contact object sent from client");
                    return BadRequest("Invalid model object");
                }

                Contact contactEntity = _repository.Contact.GetContactByCode(code);
                if (contactEntity is null)
                {
                    _logger.LogError($"Contact with code {code} hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(contact, contactEntity);

                contactEntity.UpdateDate = DateTime.Now;
                //contactEntity.UpdateCode = CollaboratorCode;

                _repository.Contact.UpdateContact(contactEntity);
                _repository.Save();

                return Ok(contactEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateContact action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{code}")]
        public IActionResult DeleteContact(int code)
        {
            try
            {
                var contact = _repository.Contact.GetContactByCode(code);
                if (contact == null)
                {
                    _logger.LogError($"Contact with code {code} hasn't been found in db.");
                    return NotFound();
                }

                contact.DeleteDate = DateTime.Now;
                //contactEntity.DeleterCode = CollaboratorCode;
                contact.Deleted = true;

                _repository.Contact.UpdateContact(contact);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteContact action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

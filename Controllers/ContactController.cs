using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.Models;
using MicroBeard.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Common.CommandTrees;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : MicroBeardController
    {
        public ContactController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
            : base(logger, repository, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAllContacts()
        {
            try
            {
                IEnumerable<Contact> contacts = _repository.Contact.GetAllContacts();
                _logger.LogInfo($"Returned all contacts from database");

                IEnumerable<SimpleContactDto> contactsResult = _mapper.Map<IEnumerable<SimpleContactDto>>(contacts);

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
                Contact contact = _repository.Contact.GetContactByCode(code, expandRelations: true);

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

                var guid = Guid.NewGuid();
                contactEntity.Password = PasswordManager.EncryptPassword(contactEntity.Password + guid.ToString());
                contactEntity.PasswordSaltGUID = guid.ToString();

                contactEntity.CreateDate = DateTime.Now;
                contactEntity.CreatorCode = CollaboratorCode;

                _repository.Contact.CreateContact(contactEntity);
                _repository.Save();

                ContactDto createdContact = _mapper.Map<ContactDto>(contactEntity);

                return CreatedAtRoute("ContactByCode", new { code = createdContact.Code }, createdContact);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Something went wrong inside CreateContact action: {ex.Message}");
                return StatusCode(500, $"Internal server error. Unable to insert values on the Database: {ex.InnerException.Message}");
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

                Contact contactEntity = _repository.Contact.GetContactByCode(code, expandRelations: true);
                if (contactEntity is null)
                {
                    _logger.LogError($"Contact with code {code} hasn't been found in db.");
                    return NotFound();
                }

                if (contactEntity.Code != ContactCode)
                    return Unauthorized();

                _mapper.Map(contact, contactEntity);

                contactEntity.UpdateDate = DateTime.Now;
                contactEntity.UpdaterCode = CollaboratorCode;

                _repository.Contact.UpdateContact(contactEntity);
                _repository.Save();

                ContactDto contactResult = _mapper.Map<ContactDto>(contactEntity);

                return Ok(contactResult);
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

                if (contact.Code != ContactCode)
                    return Unauthorized();

                contact.DeleteDate = DateTime.Now;
                contact.DeleterCode = CollaboratorCode;
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

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(ContactLoginDto loginDto)
        {
            Contact contact = _repository.Contact.GetContactByEmail(loginDto.Email);
            if (contact == null)
                return NotFound("The email was not found");

            bool passwordIsValid = PasswordManager.ValidatePassword(loginDto.Password + contact.PasswordSaltGUID, contact.Password);
            if(!passwordIsValid)
                return Unauthorized("Password invalid");

            contact.Token = TokenService.GenerateToken(contact);
            _repository.Contact.UpdateContact(contact);
            _repository.Save();

            return Ok($"Bearer {contact.Token}");
        }
    }
}

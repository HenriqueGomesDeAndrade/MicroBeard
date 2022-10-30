﻿using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        [Authorize(Roles = "Collaborator")]
        [HttpGet]
        public IActionResult GetAllContacts([FromQuery] ContactParameters contactParameters)
        {
            try
            {
                PagedList<Contact> contacts = _repository.Contact.GetAllContacts(contactParameters);
                _logger.LogInfo($"Returned {contacts.PageSize} contacts from page number {contacts.CurrentPage} database");

                var metadata = new
                {
                    contacts.TotalCount,
                    contacts.PageSize,
                    contacts.CurrentPage,
                    contacts.TotalPages,
                    contacts.HasNext,
                    contacts.HasPrevious,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                IEnumerable<SimpleContactDto> contactsResult = _mapper.Map<IEnumerable<SimpleContactDto>>(contacts);

                return Ok(contactsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllContacts action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

                if (ContactCode != null && contact.Code != ContactCode)
                    return Unauthorized();

                _logger.LogInfo($"returned contact with code: {code}");
                ContactDto contactResult = _mapper.Map<ContactDto>(contact);

                return Ok(contactResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetContactByCode action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [AllowAnonymous]
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

                if (ContactCode != null && contactEntity.Code != ContactCode)
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
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

                if (ContactCode != null && contact.Code != ContactCode)
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(ContactLoginDto loginDto)
        {
            try
            {
                Contact contact = _repository.Contact.GetContactByEmail(loginDto.Email);
                if (contact == null)
                    return NotFound("The email was not found");

                bool passwordIsValid = PasswordManager.ValidatePassword(loginDto.Password + contact.PasswordSaltGUID, contact.Password);
                if (!passwordIsValid)
                    return Unauthorized("Password invalid");

                contact.Token = TokenService.GenerateToken(contact);
                _repository.Contact.UpdateContact(contact);
                _repository.Save();

                return Ok($"Bearer {contact.Token}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteContact action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

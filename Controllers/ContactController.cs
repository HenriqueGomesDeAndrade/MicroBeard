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
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Security.Claims;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : MicroBeardController
    {
        private readonly IConfiguration _config;
        public ContactController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IConfiguration config)
            : base(logger, repository, mapper)
        {
            _config = config;
        }

        /// <summary>
        /// Consulta todos os clientes.
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas colaboradores estão autorizados. Se for um cliente utilize o endpoint Contacts/Code</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "Collaborator, CollaboratorAdmin")]
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
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside GetAllContacts action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Consulta apenas um cliente pelo código
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Clientes só podem ver o seu próprio código</response>
        /// <response code="404">Não Encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
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
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside GetContactByCode action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Cria um cliente
        /// </summary>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="500">Ocorreu algum erro interno</response>
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
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside CreateContact action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Atualiza um cliente
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas clientes (vendo seu próprio código) e colaboradores estão autorizados</response>
        /// <response code="404">Não encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
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
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside UpdateContact action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Apaga um Cliente (Soft Delete)
        /// </summary>
        /// <response code="204">Sucesso, mas sem retorno de conteúdo</response>
        /// <response code="401">Sem autorização. Clientes só podem editar seu próprio código</response>
        /// <response code="404">Não encontrado. O código passado está inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
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
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside DeleteContact action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Realiza o login
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. A senha está errada</response>
        /// <response code="404">Não encontrado. O email passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
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
                contact.Token = TokenService.GenerateToken(contact, _config.GetValue<string>("TokenKey"));
                _repository.Contact.UpdateContact(contact);
                _repository.Save();

                return Ok($"Bearer {contact.Token}");
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside Login action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Realiza o Logout e Invalida o token passado
        /// </summary>
        /// <response code="204">Sucesso, mas sem retorno de conteúdo</response>
        /// <response code="401">Sem autorização. Apenas colaboradores estão autorizados</response>
        /// <response code="404">Não encontrado. Colaborador não encontrado</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "Contact")]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            try
            {
                Contact contact = _repository.Contact.GetContactByCode((int)ContactCode);
                if (contact == null)
                    return NotFound("The contact was not found");

                contact.Token = null;
                _repository.Contact.UpdateContact(contact);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside Logout action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }
    }
}

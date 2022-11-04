using AutoMapper;
using AutoMapper.Internal.Mappers;
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
using Microsoft.IdentityModel.Tokens;


namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : MicroBeardController
    {
        private readonly IConfiguration _config;
        public ContactController(IRepositoryWrapper repository, IMapper mapper, IConfiguration config)
            : base(repository, mapper)
        {
            _config = config;
        }

        /// <summary>
        /// Consulta todos os clientes.
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas colaboradores estão autorizados. Se for um cliente utilize o endpoint Contacts/Code</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "CollaboratorAdmin, Collaborator")]
        [HttpGet]
        public IActionResult GetAllContacts([FromQuery] ContactParameters contactParameters)
        {
            try
            {
                PagedList<Contact> contacts = _repository.Contact.GetAllContacts(contactParameters);

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
        public IActionResult GetContactByCode(int code, [FromQuery] ContactParameters contactParameters)
        {
            try
            {
                Contact contact = _repository.Contact.GetContactByCode(code, expandRelations: contactParameters.ExpandRelations);

                if (contact is null)
                    return NotFound();

                if (ContactCode != null && contact.Code != ContactCode)
                    return Unauthorized();

                ContactDto contactResult = _mapper.Map<ContactDto>(contact);

                return Ok(contactResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
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
                    return BadRequest("Contact object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

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
                return StatusCode(500, $"Internal server error. Unable to insert values on the Database: {ex.InnerException.Message}");
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
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
        public IActionResult UpdateContact(int code, [FromBody] ContactUpdateDto contact, [FromQuery] ContactParameters contactParameters)
        {
            try
            {
                if (contact is null)
                    return BadRequest("Contact object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                Contact contactEntity = _repository.Contact.GetContactByCode(code, expandRelations: contactParameters.ExpandRelations);
                if (contactEntity is null)
                    return NotFound();

                if (ContactCode != null && contactEntity.Code != ContactCode)
                    return Unauthorized();

                _mapper.Map(contact, contactEntity);

                if (contact.Password.IsNullOrEmpty())
                    _repository.UnchangeProperty(contactEntity, "Password");
                else
                    contactEntity.Password = PasswordManager.EncryptPassword(contact.Password + contactEntity.PasswordSaltGUID);

                contactEntity.UpdateDate = DateTime.Now;
                contactEntity.UpdaterCode = CollaboratorCode;

                _repository.Contact.UpdateContact(contactEntity);
                _repository.Save();

                _repository.ChangeState<Contact>(contactEntity, EntityState.Detached);
                var updatedContactEntity = _repository.Contact.GetContactByCode(code, contactParameters.ExpandRelations);
                ContactDto updatedContact = _mapper.Map<ContactDto>(updatedContactEntity);

                return Ok(updatedContact);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
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
                    return NotFound();

                if (ContactCode != null && contact.Code != ContactCode)
                    return Unauthorized();

                contact.DeleteDate = DateTime.Now;
                contact.DeleterCode = ContactCode;
                contact.Deleted = true;

                _repository.Contact.UpdateContact(contact);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
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
                return StatusCode(500, $"Internal server error: {message}");
            }
        }
    }
}

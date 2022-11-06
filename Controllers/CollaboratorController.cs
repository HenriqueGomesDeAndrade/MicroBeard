using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Policy;
using MicroBeard.Helpers;
using MicroBeard.Entities.DataTransferObjects.Contact;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using MicroBeard.Entities.Parameters;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations.Schema;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.DataTransferObjects.License;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CollaboratorController : MicroBeardController
    {
        private readonly IConfiguration _config;

        public CollaboratorController(IRepositoryWrapper repository, IMapper mapper, IConfiguration config)
            : base(repository, mapper)
        {
            _config = config;
        }

        /// <summary>
        /// Consulta todos os colaboradores.
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas clientes e colaboradores estão autorizados</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpGet]
        public IActionResult GetAllCollaborators([FromQuery] CollaboratorParameters collaboratorParameters)
        {
            try
            {
                PagedList<Collaborator> collaborators = _repository.Collaborator.GetAllCollaborators(collaboratorParameters);

                var metadata = new
                {
                    collaborators.TotalCount,
                    collaborators.PageSize,
                    collaborators.CurrentPage,
                    collaborators.TotalPages,
                    collaborators.HasNext,
                    collaborators.HasPrevious,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                IEnumerable<SimpleCollaboratorDto> CollaboratorsResult = _mapper.Map<IEnumerable<SimpleCollaboratorDto>>(collaborators);

                return Ok(CollaboratorsResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Consulta apenas um colaborador pelo código
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas clientes e colaboradores estão autorizados</response>
        /// <response code="404">Não Encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpGet("{code}", Name = "CollaboratorByCode")]
        public IActionResult GetCollaboratorByCode(int code, [FromQuery] CollaboratorParameters collaboratorParameters)
        {
            try
            {
                Collaborator collaborator = _repository.Collaborator.GetCollaboratorByCode(code, expandRelations: collaboratorParameters.ExpandRelations);

                if (collaborator is null)
                    return NotFound();

                CollaboratorDto CollaboratorResult = _mapper.Map<CollaboratorDto>(collaborator);

                return Ok(CollaboratorResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }


        /// <summary>
        /// Cria um colaborador
        /// </summary>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas Colaboradores Administradores estão autorizados</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "CollaboratorAdmin")]
        [HttpPost]
        public IActionResult CreateCollaborator([FromBody] CollaboratorCreationDto collaborator)
        {
            try
            {
                if (collaborator is null)
                    return BadRequest("Collaborator object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                if (collaborator.Services != null)
                {
                    if (collaborator.Licenses == null)
                    {
                        return Unauthorized("A collaborator cannot be related to a service without the correct license");
                    }
                    else
                    {
                        foreach (SimpleServiceDto service in collaborator.Services)
                        {
                            bool isLicenseAllowedOnService = false;
                            foreach (SimpleLicenseDto license in collaborator.Licenses)
                                if (service.LicenseCode == license.Code)
                                    isLicenseAllowedOnService = true;
                            
                            if (isLicenseAllowedOnService == false)
                                return Unauthorized($"The collaborator does not have the license {service.LicenseCode} to be related to service {service.Code}");
                        }
                    }
                }

                Collaborator collaboratorEntity = _mapper.Map<Collaborator>(collaborator);

                var guid = Guid.NewGuid();
                collaboratorEntity.Password = PasswordManager.EncryptPassword(collaboratorEntity.Password + guid.ToString());
                collaboratorEntity.PasswordSaltGUID = guid.ToString();

                collaboratorEntity.CreateDate = DateTime.Now;
                collaboratorEntity.CreatorCode = CollaboratorCode;

                _repository.Collaborator.CreateCollaborator(collaboratorEntity);
                _repository.Save();

                CollaboratorDto createdCollaborator = _mapper.Map<CollaboratorDto>(collaboratorEntity);

                return CreatedAtRoute("CollaboratorByCode", new { code = createdCollaborator.Code }, createdCollaborator);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }


        /// <summary>
        /// Atualiza um colaborador
        /// </summary>
        /// <remarks>
        /// A senha pode ser passada opcionalmente para realizar a sua alteração.
        /// O Array de Licenses e Services pode ser passado opcionalmente.
        /// </remarks>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas Colaboradores Administradores estão autorizados. Colaboradores normais podem editar o seu próprio código.</response>
        /// <response code="404">Não encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "CollaboratorAdmin")]
        [HttpPut("{code}")]
        public IActionResult UpdateCollaborator(int code, [FromBody] CollaboratorUpdateDto collaborator, [FromQuery] CollaboratorParameters collaboratorParameters)
        {
            try
            {
                if (collaborator is null)
                    return BadRequest("Collaborator object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");
                
                Collaborator collaboratorEntity = _repository.Collaborator.GetCollaboratorByCode(code, true);
                if (collaboratorEntity is null)
                    return NotFound();

                if ((bool)collaboratorEntity.IsAdmin == true && collaborator.IsAdmin != true)
                {
                    bool isLastAdminCollaborator = _repository.Collaborator.CheckIfIsLastAdminCollaborator(collaboratorEntity.Code);
                    if (isLastAdminCollaborator)
                        collaborator.IsAdmin = true;
                }

                _mapper.Map(collaborator, collaboratorEntity);

                _repository.UnchangeCollection(collaboratorEntity, "Schedulings");


                if (collaboratorEntity.Services != null)
                {
                    if (collaboratorEntity.Licenses == null)
                    {
                        return Unauthorized("A collaborator cannot be related to a service without the a license");
                    }
                    else
                    {
                        foreach (var service in collaboratorEntity.Services)
                        {
                            bool isLicenseAllowedOnService = false;
                            foreach (var license in collaboratorEntity.Licenses)
                                if (service.LicenseCode == license.Code)
                                    isLicenseAllowedOnService = true;

                            if (isLicenseAllowedOnService == false)
                                return Unauthorized($"The collaborator does not have the license {service.LicenseCode} to be related to service {service.Code}");
                        }
                    }
                }

                if (collaborator.Licenses == null)
                    _repository.UnchangeCollection(collaboratorEntity, "Licenses");

                if (collaborator.Services == null)
                    _repository.UnchangeCollection(collaboratorEntity, "Services");

                if (collaborator.Password.IsNullOrEmpty())
                    _repository.UnchangeProperty(collaboratorEntity, "Password");
                else
                    collaboratorEntity.Password = PasswordManager.EncryptPassword(collaborator.Password + collaboratorEntity.PasswordSaltGUID);

                collaboratorEntity.UpdateDate = DateTime.Now;
                collaboratorEntity.UpdaterCode = CollaboratorCode;

                _repository.Collaborator.UpdateCollaborator(collaboratorEntity);
                _repository.Save();

                _repository.ChangeState<Collaborator>(collaboratorEntity, EntityState.Detached);
                var updatedCollaboratorEntity = _repository.Collaborator.GetCollaboratorByCode(code, collaboratorParameters.ExpandRelations);
                CollaboratorDto updatedCollaborator = _mapper.Map<CollaboratorDto>(updatedCollaboratorEntity);

                return Ok(updatedCollaborator);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Apaga um Colaborador (Soft Delete)
        /// </summary>
        /// <response code="204">Sucesso, mas sem retorno de conteúdo</response>
        /// <response code="401">Sem autorização. Apenas Colaboradores Administradores estão autorizados</response>
        /// <response code="404">Não encontrado. O código passado está inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "CollaboratorAdmin")]
        [HttpDelete("{code}")]
        public IActionResult DeleteCollaborator(int code)
        {
            try
            {
                var collaborator = _repository.Collaborator.GetCollaboratorByCode(code);
                if (collaborator == null)
                    return NotFound();

                if ((bool)collaborator.IsAdmin == true)
                {
                    bool isLastAdminCollaborator = _repository.Collaborator.CheckIfIsLastAdminCollaborator(code);
                    if (isLastAdminCollaborator)
                        Unauthorized("This collaborator is the last Administrator of the account, it cannot be deleted");
                }

                collaborator.DesactivationDate = DateTime.Now;
                collaborator.DesactivatorCode = CollaboratorCode;
                collaborator.Desactivated = true;

                _repository.Collaborator.UpdateCollaborator(collaborator);
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
        public IActionResult Login(CollaboratorLoginDto loginDto)
        {
            try
            {
                Collaborator collaborator = _repository.Collaborator.GetCollaboratorByEmail(loginDto.Email);
                if (collaborator == null)
                    return NotFound("The email was not found");

                bool passwordIsValid = PasswordManager.ValidatePassword(loginDto.Password + collaborator.PasswordSaltGUID, collaborator.Password);
                if (!passwordIsValid)
                    return Unauthorized("Password invalid");

                collaborator.Token = TokenService.GenerateToken(collaborator, _config.GetValue<string>("TokenKey"));
                _repository.Collaborator.UpdateCollaborator(collaborator);
                _repository.Save();

                return Ok($"Bearer {collaborator.Token}");
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
        [Authorize(Roles = "Collaborator, CollaboratorAdmin")]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            try
            {
                Collaborator collaborator = _repository.Collaborator.GetCollaboratorByCode((int)CollaboratorCode);
                if (collaborator == null)
                    return NotFound("The collaborator was not found");

                collaborator.Token = null;
                _repository.Collaborator.UpdateCollaborator(collaborator);
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

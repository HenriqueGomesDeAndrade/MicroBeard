﻿using AutoMapper;
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

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CollaboratorController : MicroBeardController
    {
        private readonly IConfiguration _config;

        public CollaboratorController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IConfiguration config)
            : base(logger, repository, mapper)
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
                _logger.LogInfo($"Returned all collaborators from database");

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
                _logger.LogError($"Something went wrong inside GetAllCollaborators action: {message}");
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
        public IActionResult GetCollaboratorByCode(int code)
        {
            try
            {
                Collaborator collaborator = _repository.Collaborator.GetCollaboratorByCode(code, expandRelations: true);

                if (collaborator is null)
                {
                    _logger.LogError($"Collaborator with code: {code} hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"returned Collaborator with code: {code}");
                CollaboratorDto CollaboratorResult = _mapper.Map<CollaboratorDto>(collaborator);

                return Ok(CollaboratorResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside GetCollaboratorByCode action: {message}");
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
                {
                    _logger.LogError("Collaborator object sent from client is null");
                    return BadRequest("Collaborator object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Collaborator object sent from client");
                    return BadRequest("Invalid model object");
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
                _logger.LogError($"Something went wrong inside CreateCollaborator action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }


        /// <summary>
        /// Atualiza um colaborador
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas Colaboradores Administradores estão autorizados</response>
        /// <response code="404">Não encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "CollaboratorAdmin")]
        [HttpPut("{code}")]
        public IActionResult UpdateCollaborator(int code, [FromBody] CollaboratorUpdateDto collaborator)
        {
            try
            {
                if (collaborator is null)
                {
                    _logger.LogError("Collaborator object sent from client is null");
                    return BadRequest("Collaborator object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Collaborator object sent from client");
                    return BadRequest("Invalid model object");
                }

                Collaborator collaboratorEntity = _repository.Collaborator.GetCollaboratorByCode(code, expandRelations: true);
                if (collaboratorEntity is null)
                {
                    _logger.LogError($"Collaborator with code {code} hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(collaborator, collaboratorEntity);

                collaboratorEntity.UpdateDate = DateTime.Now;
                collaboratorEntity.UpdaterCode = CollaboratorCode;

                _repository.Collaborator.UpdateCollaborator(collaboratorEntity);
                _repository.Save();

                return Ok(collaborator);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside UpdateCollaborator action: {message}");
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
                {
                    _logger.LogError($"Collaborator with code {code} hasn't been found in db.");
                    return NotFound();
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
                _logger.LogError($"Something went wrong inside DeleteCollaborator action: {message}");
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
                _logger.LogError($"Something went wrong inside Logout action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }
    }
}

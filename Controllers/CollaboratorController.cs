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

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CollaboratorController : MicroBeardController
    {
        public CollaboratorController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
            : base(logger, repository, mapper)
        {
        }

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
                _logger.LogError($"Something went wrong inside GetAllCollaborators action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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
                _logger.LogError($"Something went wrong inside GetCollaboratorByCode action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Collaborator")]
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
                _logger.LogError($"Something went wrong inside CreateCollaborator action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Collaborator")]
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
                _logger.LogError($"Something went wrong inside UpdateCollaborator action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Collaborator")]
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
                _logger.LogError($"Something went wrong inside DeleteCollaborator action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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

                collaborator.Token = TokenService.GenerateToken(collaborator);
                _repository.Collaborator.UpdateCollaborator(collaborator);
                _repository.Save();

                return Ok($"Bearer {collaborator.Token}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Login action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Collaborator")]
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

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Logout action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

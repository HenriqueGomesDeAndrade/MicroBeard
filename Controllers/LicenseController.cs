﻿using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.License;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LicenseController : MicroBeardController
    {
        public LicenseController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
            : base(logger, repository, mapper)
        {
        }

        /// <summary>
        /// Consulta todos as habilitações.
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas clientes e colaboradores estão autorizados</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpGet]
        public IActionResult GetAllLicenses([FromQuery] LicenseParameters licenseParameters)
        {
            try
            {
                PagedList<License> licenses = _repository.License.GetAllLicenses(licenseParameters);
                _logger.LogInfo($"Returned all licenses from database");

                var metadata = new
                {
                    licenses.TotalCount,
                    licenses.PageSize,
                    licenses.CurrentPage,
                    licenses.TotalPages,
                    licenses.HasNext,
                    licenses.HasPrevious,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                IEnumerable<SimpleLicenseDto> licensesResult = _mapper.Map<IEnumerable<SimpleLicenseDto>>(licenses);

                return Ok(licensesResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside GetAllLicenses action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Consulta apenas uma habilitação pelo código
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas clientes e colaboradores estão autorizados</response>
        /// <response code="404">Não Encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpGet("{code}", Name = "LicenseByCode")]
        public IActionResult GetLicenseByCode(int code)
        {
            try
            {
                License license = _repository.License.GetLicenseByCode(code, expandRelations: true);

                if (license is null)
                {
                    _logger.LogError($"License with code: {code} hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"returned License with code: {code}");
                LicenseDto LicenseResult = _mapper.Map<LicenseDto>(license);

                return Ok(LicenseResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside GetLicenseByCode action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Cria uma habilitação
        /// </summary>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas Colaboradores estão autorizados</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "Collaborator, CollaboratorAdmin")]
        [HttpPost]
        public IActionResult CreateLicense([FromBody] LicenseCreationDto license)
        {
            try
            {
                if (license is null)
                {
                    _logger.LogError("License object sent from client is null");
                    return BadRequest("License object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid license object sent from client");
                    return BadRequest("Invalid model object");
                }

                License licenseEntity = _mapper.Map<License>(license);

                licenseEntity.CreateDate = DateTime.Now;
                licenseEntity.CreatorCode = CollaboratorCode;

                _repository.License.CreateLicense(licenseEntity);
                _repository.Save();

                LicenseDto createdLicense = _mapper.Map<LicenseDto>(licenseEntity);

                return CreatedAtRoute("LicenseByCode", new { code = createdLicense.Code }, createdLicense);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside CreateLicense action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Atualiza uma habilitação
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas Colaboradores estão autorizados</response>
        /// <response code="404">Não encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "Collaborator, CollaboratorAdmin")]
        [HttpPut("{code}")]
        public IActionResult UpdateLicense(int code, [FromBody] LicenseUpdateDto license)
        {
            try
            {
                if (license is null)
                {
                    _logger.LogError("License object sent from client is null");
                    return BadRequest("License object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid License object sent from client");
                    return BadRequest("Invalid model object");
                }

                License licenseEntity = _repository.License.GetLicenseByCode(code, expandRelations: true);
                if (licenseEntity is null)
                {
                    _logger.LogError($"License with code {code} hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(license, licenseEntity);

                licenseEntity.UpdateDate = DateTime.Now;
                licenseEntity.UpdaterCode = CollaboratorCode;

                _repository.License.UpdateLicense(licenseEntity);
                _repository.Save();

                return Ok(licenseEntity);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside UpdateLicense action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Apaga uma habilitação (Soft Delete)
        /// </summary>
        /// <response code="204">Sucesso, mas sem retorno de conteúdo</response>
        /// <response code="401">Sem autorização. Apenas Colaboradores estão autorizados</response>
        /// <response code="404">Não encontrado. O código passado está inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [Authorize(Roles = "Collaborator, CollaboratorAdmin")]
        [HttpDelete("{code}")]
        public IActionResult DeleteLicense(int code)
        {
            try
            {
                var license = _repository.License.GetLicenseByCode(code);
                if (license == null)
                {
                    _logger.LogError($"License with code {code} hasn't been found in db.");
                    return NotFound();
                }

                license.DesactivationDate = DateTime.Now;
                license.DesactivatorCode = CollaboratorCode;
                license.Desactivated = true;

                _repository.License.UpdateLicense(license);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside DeleteLicense action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }
    }
}

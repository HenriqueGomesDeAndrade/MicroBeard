using AutoMapper;
using AutoMapper.Internal.Mappers;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Contact;
using MicroBeard.Entities.DataTransferObjects.License;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LicenseController : MicroBeardController
    {
        public LicenseController(IRepositoryWrapper repository, IMapper mapper)
            : base(repository, mapper)
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
        public IActionResult GetLicenseByCode(int code, [FromQuery] LicenseParameters licenseParameters)
        {
            try
            {
                License license = _repository.License.GetLicenseByCode(code, expandRelations: licenseParameters.ExpandRelations);

                if (license is null)
                    return NotFound();

                LicenseDto LicenseResult = _mapper.Map<LicenseDto>(license);

                return Ok(LicenseResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
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
                    return BadRequest("License object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

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
        public IActionResult UpdateLicense(int code, [FromBody] LicenseUpdateDto license, [FromQuery] LicenseParameters licenseParameters)
        {
            try
            {
                if (license is null)
                    return BadRequest("License object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                License licenseEntity = _repository.License.GetLicenseByCode(code, expandRelations: licenseParameters.ExpandRelations);
                if (licenseEntity is null)
                {
                    return NotFound();
                }

                _mapper.Map(license, licenseEntity);

                _repository.UnchangeCollection(licenseEntity, "Services");


                if (license.Collaborators == null)
                    _repository.UnchangeCollection(licenseEntity, "Collaborators");

                licenseEntity.UpdateDate = DateTime.Now;
                licenseEntity.UpdaterCode = CollaboratorCode;

                _repository.License.UpdateLicense(licenseEntity);
                _repository.Save();

                _repository.ChangeState<License>(licenseEntity, EntityState.Detached);
                var updatedLicenseEntity = _repository.License.GetLicenseByCode(code, licenseParameters.ExpandRelations);
                LicenseDto updatedLicense = _mapper.Map<LicenseDto>(updatedLicenseEntity);

                return Ok(updatedLicense);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
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
                    return NotFound();

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
                return StatusCode(500, $"Internal server error: {message}");
            }
        }
    }
}

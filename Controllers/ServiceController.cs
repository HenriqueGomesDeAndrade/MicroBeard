using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Collaborator;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : MicroBeardController
    {
        public ServiceController(IRepositoryWrapper repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        /// <summary>
        /// Consulta todos os serviços.
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas clientes e colaboradores estão autorizados</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpGet]
        public IActionResult GetAllServices([FromQuery] ServiceParameters serviceParameters)
        {
            try
            {
                PagedList<Service> services = _repository.Service.GetAllServices(serviceParameters);

                var metadata = new
                {
                    services.TotalCount,
                    services.PageSize,
                    services.CurrentPage,
                    services.TotalPages,
                    services.HasNext,
                    services.HasPrevious,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                IEnumerable<SimpleServiceDto> servicesResult = _mapper.Map<IEnumerable<SimpleServiceDto>>(services);

                return Ok(servicesResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Consulta apenas um serviço pelo código
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas clientes e colaboradores estão autorizados</response>
        /// <response code="404">Não Encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpGet("{code}", Name = "ServiceByCode")]
        public IActionResult GetServiceByCode(int code, [FromQuery] ServiceParameters serviceParameters)
        {
            try
            {
                Service service = _repository.Service.GetServiceByCode(code, expandRelations: serviceParameters.ExpandRelations);

                if (service is null)
                {
                    return NotFound();
                }

                ServiceDto servicesResult = _mapper.Map<ServiceDto>(service);

                return Ok(servicesResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Cria um serviço
        /// </summary>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas clientes e colaboradores estão autorizados</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpPost]
        public IActionResult CreateService([FromBody] ServiceCreationDto service)
        {
            try
            {
                if (service is null)
                {
                    return BadRequest("Service object is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                Service serviceEntity = _mapper.Map<Service>(service);

                if (service.LicenseCode != null)
                {
                    License licenseCheck = _repository.License.GetLicenseByCode(service.LicenseCode);
                    if (licenseCheck == null)
                        return NotFound($"Unable to find the License code {service.LicenseCode}");
                }

                serviceEntity.CreateDate = DateTime.Now;
                serviceEntity.CreatorCode = CollaboratorCode;

                _repository.Service.CreateService(serviceEntity);
                _repository.Save();

                ServiceDto createdService = _mapper.Map<ServiceDto>(serviceEntity);

                return CreatedAtRoute("ServiceByCode", new { code = createdService.Code }, createdService);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Atualiza um serviço
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas clientes e colaboradores estão autorizados</response>
        /// <response code="404">Não encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpPut("{code}")]
        public IActionResult UpdateService(int code, [FromBody] ServiceUpdateDto service, [FromQuery] ServiceParameters serviceParameters)
        {
            try
            {
                if (service is null)
                    return BadRequest("Service object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                Service serviceEntity = _repository.Service.GetServiceByCode(code, expandRelations: serviceParameters.ExpandRelations);
                if (serviceEntity is null)
                    return NotFound();

                
                
                _mapper.Map(service, serviceEntity);

                if (service.LicenseCode != null)
                {
                    License licenseCheck = _repository.License.GetLicenseByCode(service.LicenseCode);
                    if (licenseCheck == null)
                        return NotFound($"Unable to find the License code {service.LicenseCode}");
                    serviceEntity.LicenseCodeNavigation = licenseCheck;
                }

                if (service.Collaborators == null)
                    _repository.UnchangeCollection(serviceEntity, "Collaborators");

                if (service.Schedulings == null)
                    _repository.UnchangeCollection(serviceEntity, "Schedulings");

                serviceEntity.UpdateDate = DateTime.Now;
                serviceEntity.UpdaterCode = CollaboratorCode;

                _repository.Service.UpdateService(serviceEntity);
                _repository.Save();

                _repository.ChangeState<Service>(serviceEntity, EntityState.Detached);
                var updatedServiceEntity = _repository.Service.GetServiceByCode(code, serviceParameters.ExpandRelations);
                ServiceDto updatedService = _mapper.Map<ServiceDto>(updatedServiceEntity);

                return Ok(updatedService);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Apaga um Serviço (Soft Delete)
        /// </summary>
        /// <response code="204">Sucesso, mas sem retorno de conteúdo</response>
        /// <response code="401">Sem autorização. Apenas Colaboradores Administradores estão autorizados</response>
        /// <response code="404">Não encontrado. O código passado está inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpDelete("{code}")]
        public IActionResult DeleteService(int code)
        {
            try
            {
                var service = _repository.Service.GetServiceByCode(code);
                if (service == null)
                    return NotFound();

                service.DeleteDate = DateTime.Now;
                service.DeleterCode = CollaboratorCode;
                service.Deleted = true;

                _repository.Service.UpdateService(service);
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

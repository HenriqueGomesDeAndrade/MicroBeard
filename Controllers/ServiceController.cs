using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController : MicroBeardController
    {
        public ServiceController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
            : base(logger, repository, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAllServices([FromQuery] ServiceParameters serviceParameters)
        {
            try
            {
                PagedList<Service> services = _repository.Service.GetAllServices(serviceParameters);
                _logger.LogInfo($"Returned all Services from database");

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
                _logger.LogError($"Something went wrong inside GetAllServices action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{code}", Name = "ServiceByCode")]
        public IActionResult GetServiceByCode(int code)
        {
            try
            {
                Service service = _repository.Service.GetServiceByCode(code, expandRelations: true);

                if (service is null)
                {
                    _logger.LogError($"Service with code: {code} hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"returned Service with code: {code}");
                ServiceDto servicesResult = _mapper.Map<ServiceDto>(service);

                return Ok(servicesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetServiceByCode action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateService([FromBody] ServiceCreationDto service)
        {
            try
            {
                if (service is null)
                {
                    _logger.LogError("Service object sent from client is null");
                    return BadRequest("Service object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Service object sent from client");
                    return BadRequest("Invalid model object");
                }

                Service serviceEntity = _mapper.Map<Service>(service);

                serviceEntity.CreateDate = DateTime.Now;
                serviceEntity.CreatorCode = CollaboratorCode;

                _repository.Service.CreateService(serviceEntity);
                _repository.Save();

                ServiceDto createdService = _mapper.Map<ServiceDto>(serviceEntity);

                return CreatedAtRoute("ServiceByCode", new { code = createdService.Code }, createdService);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateService action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{code}")]
        public IActionResult UpdateService(int code, [FromBody] ServiceUpdateDto service)
        {
            try
            {
                if (service is null)
                {
                    _logger.LogError("Service object sent from client is null");
                    return BadRequest("Service object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Service object sent from client");
                    return BadRequest("Invalid model object");
                }

                Service serviceEntity = _repository.Service.GetServiceByCode(code, expandRelations: true);
                if (serviceEntity is null)
                {
                    _logger.LogError($"Service with code {code} hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(service, serviceEntity);

                serviceEntity.UpdateDate = DateTime.Now;
                serviceEntity.UpdaterCode = CollaboratorCode;

                _repository.Service.UpdateService(serviceEntity);
                _repository.Save();

                return Ok(serviceEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateService action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{code}")]
        public IActionResult DeleteService(int code)
        {
            try
            {
                var service = _repository.Service.GetServiceByCode(code);
                if (service == null)
                {
                    _logger.LogError($"Service with code {code} hasn't been found in db.");
                    return NotFound();
                }

                service.DeleteDate = DateTime.Now;
                service.DeleterCode = CollaboratorCode;
                service.Deleted = true;

                _repository.Service.UpdateService(service);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteService action: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

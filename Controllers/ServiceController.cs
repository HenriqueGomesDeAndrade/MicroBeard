using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Service;
using MicroBeard.Entities.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAllServices()
        {
            try
            {
                IEnumerable<Service> services = _repository.Service.GetAllServices();
                _logger.LogInfo($"Returned all Services from database");

                IEnumerable<SimpleServiceDto> servicesResult = _mapper.Map<IEnumerable<SimpleServiceDto>>(services);

                return Ok(servicesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllServices action: {ex.Message}");
                return StatusCode(500, "Internal server error");
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
                return StatusCode(500, "Internal server error");
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
                //ServiceEntity.CreatorCode = CollaboratorCode;

                _repository.Service.CreateService(serviceEntity);
                _repository.Save();

                ServiceDto createdService = _mapper.Map<ServiceDto>(serviceEntity);

                return CreatedAtRoute("ServiceByCode", new { code = createdService.Code }, createdService);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateService action: {ex.Message}");
                return StatusCode(500, "Internal server error");
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
                //ServiceEntity.UpdateCode = CollaboratorCode;

                _repository.Service.UpdateService(serviceEntity);
                _repository.Save();

                return Ok(serviceEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateService action: {ex.Message}");
                return StatusCode(500, "Internal server error");
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
                //ServiceEntity.DeleterCode = CollaboratorCode;
                service.Deleted = true;

                _repository.Service.UpdateService(service);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteService action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Scheduling;
using MicroBeard.Entities.Models;
using MicroBeard.Entities.Parameters;
using MicroBeard.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.Entity;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchedulingController : MicroBeardController
    {
        public SchedulingController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
            : base(logger, repository, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAllSchedulings([FromQuery] SchedulingParameters schedulingParameters)
        {
            try
            {
                if (ContactCode != null)
                    schedulingParameters.ContactCode = ContactCode;

                PagedList<Scheduling> schedulings = _repository.Scheduling.GetAllSchedulings(schedulingParameters);
                _logger.LogInfo($"Returned all Schedulings from database");

                var metadata = new
                {
                    schedulings.TotalCount,
                    schedulings.PageSize,
                    schedulings.CurrentPage,
                    schedulings.TotalPages,
                    schedulings.HasNext,
                    schedulings.HasPrevious,
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                IEnumerable<SimpleSchedulingDto> schedulingsResult = _mapper.Map<IEnumerable<SimpleSchedulingDto>>(schedulings);

                return Ok(schedulingsResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside GetAllSchedulings action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        [HttpGet("{code}", Name = "SchedulingByCode")]
        public IActionResult GetSchedulingByCode(int code)
        {
            try
            {
                Scheduling scheduling = _repository.Scheduling.GetSchedulingByCode(code, expandRelations: true);

                if (scheduling is null)
                {
                    _logger.LogError($"Scheduling with code: {code} hasn't been found in db.");
                    return NotFound();
                }

                if (ContactCode != null && scheduling.ContactCode != ContactCode)
                    return Unauthorized();

                _logger.LogInfo($"returned Scheduling with code: {code}");
                SchedulingDto schedulingResult = _mapper.Map<SchedulingDto>(scheduling);

                return Ok(schedulingResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside GetSchedulingByCode action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        [HttpPost]
        public IActionResult CreateScheduling([FromBody] SchedulingCreationDto scheduling)
        {
            try
            {
                if (scheduling is null)
                {
                    _logger.LogError("Scheduling object sent from client is null");
                    return BadRequest("Scheduling object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Scheduling object sent from client");
                    return BadRequest("Invalid model object");
                }

                Contact contactCheck = _repository.Contact.GetContactByCode(scheduling.ContactCode);
                if (contactCheck == null)
                    return NotFound($"Unable to find the Contact code {scheduling.ContactCode}");

                if (ContactCode != null && scheduling.ContactCode != ContactCode)
                    return Unauthorized();

                Service ServiceCheck = _repository.Service.GetServiceByCode(scheduling.ServiceCode);
                if (ServiceCheck == null)
                    return NotFound($"Unable to find the Service code {scheduling.ServiceCode}");


                Scheduling schedulingEntity = _mapper.Map<Scheduling>(scheduling);
                

                schedulingEntity.CreateDate = DateTime.Now;
                schedulingEntity.CreatorCode = CollaboratorCode;

                _repository.Scheduling.CreateScheduling(schedulingEntity);
                _repository.Save();

                SchedulingDto createdScheduling = _mapper.Map<SchedulingDto>(schedulingEntity);

                return CreatedAtRoute("SchedulingByCode", new { code = createdScheduling.Code }, createdScheduling);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside CreateScheduling action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        [HttpPut("{code}")]
        public IActionResult UpdateScheduling(int code, [FromBody] SchedulingUpdateDto scheduling)
        {
            try
            {
                if (scheduling is null)
                {
                    _logger.LogError("Scheduling object sent from client is null");
                    return BadRequest("Scheduling object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Scheduling object sent from client");
                    return BadRequest("Invalid model object");
                }

                Scheduling schedulingEntity = _repository.Scheduling.GetSchedulingByCode(code, expandRelations: true);
                if (schedulingEntity is null)
                {
                    _logger.LogError($"Scheduling with code {code} hasn't been found in db.");
                    return NotFound();
                }

                Contact contactCheck = _repository.Contact.GetContactByCode(scheduling.ContactCode);
                if (contactCheck == null)
                    return NotFound($"Unable to find the Contact code {scheduling.ContactCode}");

                if (ContactCode != null && scheduling.ContactCode != ContactCode)
                    return Unauthorized();

                Service serviceCheck = _repository.Service.GetServiceByCode(scheduling.ServiceCode);
                if (serviceCheck == null)
                    return NotFound($"Unable to find the Service code {scheduling.ServiceCode}");

                _mapper.Map(scheduling, schedulingEntity);

                schedulingEntity.UpdateDate = DateTime.Now;
                schedulingEntity.UpdaterCode = CollaboratorCode;
                schedulingEntity.ContactCodeNavigation = contactCheck;
                schedulingEntity.ServiceCodeNavigation = serviceCheck;

                _repository.Scheduling.UpdateScheduling(schedulingEntity);
                _repository.Save();

                SchedulingDto updatedScheduling = _mapper.Map<SchedulingDto>(schedulingEntity);

                return Ok(updatedScheduling);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside UpdateScheduling action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        [HttpDelete("{code}")]
        public IActionResult DeleteScheduling(int code)
        {
            try
            {
                var scheduling = _repository.Scheduling.GetSchedulingByCode(code);
                if (scheduling == null)
                {
                    _logger.LogError($"Scheduling with code {code} hasn't been found in db.");
                    return NotFound();
                }

                if (ContactCode != null && scheduling.ContactCode != ContactCode)
                    return Unauthorized();

                scheduling.DeleteDate = DateTime.Now;
                scheduling.DeleterCode = CollaboratorCode;
                scheduling.Deleted = true;

                _repository.Scheduling.UpdateScheduling(scheduling);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                _logger.LogError($"Something went wrong inside DeleteScheduling action: {message}");
                return StatusCode(500, $"Internal server error: {message}");
            }
        }
    }
}

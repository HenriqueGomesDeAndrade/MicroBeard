using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.Scheduling;
using MicroBeard.Entities.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAllSchedulings()
        {
            try
            {
                IEnumerable<Scheduling> schedulings = _repository.Scheduling.GetAllSchedulings();
                _logger.LogInfo($"Returned all Schedulings from database");

                IEnumerable<SimpleSchedulingDto> schedulingsResult = _mapper.Map<IEnumerable<SimpleSchedulingDto>>(schedulings);

                return Ok(schedulingsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllSchedulings action: {ex.Message}");
                return StatusCode(500, "Internal server error");
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

                _logger.LogInfo($"returned Scheduling with code: {code}");
                SchedulingDto schedulingResult = _mapper.Map<SchedulingDto>(scheduling);

                return Ok(schedulingResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetSchedulingByCode action: {ex.Message}");
                return StatusCode(500, "Internal server error");
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

                Service ServiceCheck = _repository.Service.GetServiceByCode(scheduling.ServiceCode);
                if (ServiceCheck == null)
                    return NotFound($"Unable to find the Service code {scheduling.ServiceCode}");


                Scheduling schedulingEntity = _mapper.Map<Scheduling>(scheduling);
                

                schedulingEntity.CreateDate = DateTime.Now;
                //SchedulingEntity.CreatorCode = CollaboratorCode;

                _repository.Scheduling.CreateScheduling(schedulingEntity);
                _repository.Save();

                SchedulingDto createdScheduling = _mapper.Map<SchedulingDto>(schedulingEntity);

                return CreatedAtRoute("SchedulingByCode", new { code = createdScheduling.Code }, createdScheduling);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateScheduling action: {ex.Message}");
                return StatusCode(500, "Internal server error");
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

                Service serviceCheck = _repository.Service.GetServiceByCode(scheduling.ServiceCode);
                if (serviceCheck == null)
                    return NotFound($"Unable to find the Service code {scheduling.ServiceCode}");

                _mapper.Map(scheduling, schedulingEntity);

                schedulingEntity.UpdateDate = DateTime.Now;
                //SchedulingEntity.UpdateCode = CollaboratorCode;
                schedulingEntity.ContactCodeNavigation = contactCheck;
                schedulingEntity.ServiceCodeNavigation = serviceCheck;

                _repository.Scheduling.UpdateScheduling(schedulingEntity);
                _repository.Save();

                SchedulingDto updatedScheduling = _mapper.Map<SchedulingDto>(schedulingEntity);

                return Ok(updatedScheduling);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateScheduling action: {ex.Message}");
                return StatusCode(500, "Internal server error");
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

                scheduling.DeleteDate = DateTime.Now;
                //SchedulingEntity.DeleterCode = CollaboratorCode;
                scheduling.Deleted = true;

                _repository.Scheduling.UpdateScheduling(scheduling);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteScheduling action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

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
        public SchedulingController(IRepositoryWrapper repository, IMapper mapper)
            : base(repository, mapper)
        {
        }

        /// <summary>
        /// Consulta todos os agendamentos.
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas clientes (que só acessam seus próprios agendamentos) e colaboradores estão autorizados</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpGet]
        public IActionResult GetAllSchedulings([FromQuery] SchedulingParameters schedulingParameters)
        {
            try
            {
                if (ContactCode != null)
                    schedulingParameters.ContactCode = ContactCode;

                PagedList<Scheduling> schedulings = _repository.Scheduling.GetAllSchedulings(schedulingParameters);

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
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Consulta apenas um agendamento pelo código
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Sem autorização. Apenas clientes (Passando seu próprio código) e colaboradores estão autorizados</response>
        /// <response code="404">Não Encontrado. O código passado é inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpGet("{code}", Name = "SchedulingByCode")]
        public IActionResult GetSchedulingByCode(int code, [FromQuery] SchedulingParameters schedulingParameters)
        {
            try
            {
                Scheduling scheduling = _repository.Scheduling.GetSchedulingByCode(code, expandRelations: schedulingParameters.ExpandRelations);

                if (scheduling is null)
                    return NotFound();

                if (ContactCode != null && scheduling.ContactCode != ContactCode)
                    return Unauthorized();

                SchedulingDto schedulingResult = _mapper.Map<SchedulingDto>(scheduling);

                return Ok(schedulingResult);
            }
            catch (Exception ex)
            {
                string message = GetFullException(ex);
                return StatusCode(500, $"Internal server error: {message}");
            }
        }


        /// <summary>
        /// Cria um agendamento
        /// </summary>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas clientes (passando seu próprio código) e colaboradores estão autorizados</response>
        /// <response code="404">Não encontrado. Código do contado ou cliente inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpPost]
        public IActionResult CreateScheduling([FromBody] SchedulingCreationDto scheduling)
        {
            try
            {
                if (scheduling is null)
                    return BadRequest("Scheduling object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                Contact contactCheck = _repository.Contact.GetContactByCode(scheduling.ContactCode ?? scheduling.Contact.Code);
                if (contactCheck == null)
                    return NotFound($"Unable to find the Contact code {scheduling.ContactCode ?? scheduling.Contact.Code}");

                if (ContactCode != null && (scheduling.ContactCode ?? scheduling.Contact.Code) != ContactCode)
                    return Unauthorized();

                Service ServiceCheck = _repository.Service.GetServiceByCode(scheduling.ServiceCode ?? scheduling.Service.Code);
                if (ServiceCheck == null)
                    return NotFound($"Unable to find the Service code {scheduling.ServiceCode ?? scheduling.Service.Code}");


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
                return StatusCode(500, $"Internal server error: {message}");
            }
        }


        /// <summary>
        /// Cria um agendamento
        /// </summary>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Algo está errado no modelo</response>
        /// <response code="401">Sem autorização. Apenas clientes (passando seu próprio código) e colaboradores estão autorizados</response>
        /// <response code="404">Não encontrado. Código do contado ou cliente inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpPut("{code}")]
        public IActionResult UpdateScheduling(int code, [FromBody] SchedulingUpdateDto scheduling, [FromQuery] SchedulingParameters schedulingParameters)
        {
            try
            {
                if (scheduling is null)
                    return BadRequest("Scheduling object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                Scheduling schedulingEntity = _repository.Scheduling.GetSchedulingByCode(code, expandRelations: schedulingParameters.ExpandRelations);
                if (schedulingEntity is null)
                    return NotFound();

                Contact contactCheck = _repository.Contact.GetContactByCode(scheduling.ContactCode ?? scheduling.Contact.Code);
                if (contactCheck == null)
                    return NotFound($"Unable to find the Contact code {scheduling.ContactCode ?? scheduling.Contact.Code}");

                if (ContactCode != null && (scheduling.ContactCode ?? scheduling.Contact.Code) != ContactCode)
                    return Unauthorized();

                Service serviceCheck = _repository.Service.GetServiceByCode(scheduling.ServiceCode ?? scheduling.Service.Code);
                if (serviceCheck == null)
                    return NotFound($"Unable to find the Service code {scheduling.ServiceCode ?? scheduling.Service.Code}");

                _mapper.Map(scheduling, schedulingEntity);

                if (scheduling.Contact == null)
                    _repository.UnchangeProperty(schedulingEntity, "Contact");

                if (scheduling.Service == null)
                    _repository.UnchangeProperty(schedulingEntity, "Service");

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
                return StatusCode(500, $"Internal server error: {message}");
            }
        }

        /// <summary>
        /// Apaga um Agendamento (Soft Delete)
        /// </summary>
        /// <response code="204">Sucesso, mas sem retorno de conteúdo</response>
        /// <response code="401">Sem autorização. Apenas clientes (passando seu próprio código) e colaboradores estão autorizados</response>
        /// <response code="404">Não encontrado. O código passado está inválido</response>
        /// <response code="500">Ocorreu algum erro interno</response>
        [HttpDelete("{code}")]
        public IActionResult DeleteScheduling(int code)
        {
            try
            {
                var scheduling = _repository.Scheduling.GetSchedulingByCode(code);
                if (scheduling == null)
                    return NotFound();

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
                return StatusCode(500, $"Internal server error: {message}");
            }
        }
    }
}

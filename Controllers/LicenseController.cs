using AutoMapper;
using MicroBeard.Contracts;
using MicroBeard.Entities.DataTransferObjects.License;
using MicroBeard.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroBeard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LicenseController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        public LicenseController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllLicenses()
        {
            try
            {
                IEnumerable<License> licenses = _repository.License.GetAllLicenses();
                _logger.LogInfo($"Returned all licenses from database");

                IEnumerable<SimpleLicenseDto> licensesResult = _mapper.Map<IEnumerable<SimpleLicenseDto>>(licenses);

                return Ok(licensesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllLicenses action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{code}", Name = "LicenseByCode")]
        public IActionResult GetLicenseByCode(int code)
        {
            try
            {
                License license = _repository.License.GetLicenseByCode(code);

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
                _logger.LogError($"Something went wrong inside GetLicenseByCode action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

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
                //LicenseEntity.CreatorCode = CollaboratorCode;

                _repository.License.CreateLicense(licenseEntity);
                _repository.Save();

                LicenseDto createdLicense = _mapper.Map<LicenseDto>(licenseEntity);

                return CreatedAtRoute("LicenseByCode", new { code = createdLicense.Code }, createdLicense);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateLicense action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

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

                License licenseEntity = _repository.License.GetLicenseByCode(code);
                if (licenseEntity is null)
                {
                    _logger.LogError($"License with code {code} hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(license, licenseEntity);

                licenseEntity.UpdateDate = DateTime.Now;
                //LicenseEntity.UpdateCode = CollaboratorCode;

                _repository.License.UpdateLicense(licenseEntity);
                _repository.Save();

                return Ok(licenseEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateLicense action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

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
                //LicenseEntity.DeleterCode = CollaboratorCode;
                license.Desactivated = true;

                _repository.License.UpdateLicense(license);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteLicense action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

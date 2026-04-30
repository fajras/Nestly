using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SymptomDiaryController : ControllerBase
    {
        private readonly ISymptomDiaryService _service;

        public SymptomDiaryController(ISymptomDiaryService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<PagedResult<SymptomDiaryResponseDto>> Get([FromQuery] SymptomDiarySearchObject search)
            => Ok(_service.Get(search));
        [HttpPost]
        public ActionResult<SymptomDiaryResponseDto> Create([FromBody] CreateSymptomDiaryDto request)
        {
            try
            {
                var dto = _service.Create(request);

                return CreatedAtRoute(
                    "GetSymptomByDate",
                    new
                    {
                        parentProfileId = dto.ParentProfileId,
                        date = dto.Date.ToString("yyyy-MM-dd")
                    },
                    dto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("parent/{parentProfileId:long}")]
        public ActionResult<PagedResult<SymptomDiaryResponseDto>> GetByParent(
     long parentProfileId,
     [FromQuery] SymptomDiarySearchObject search)
        {
            search.ParentProfileId = parentProfileId;
            return Ok(_service.GetByParent(parentProfileId, search));
        }

        [HttpGet("by-date", Name = "GetSymptomByDate")]
        public ActionResult<SymptomDiaryResponseDto> GetByDate(
            [FromQuery] long parentProfileId,
            [FromQuery] DateTime date)
        {
            var dto = _service.GetByDate(parentProfileId, date);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPatch("{id:long}")]
        public ActionResult<SymptomDiaryResponseDto> Patch(
            long id,
            [FromBody] SymptomDiaryPatchDto patch)
        {
            try
            {
                var dto = _service.Patch(id, patch);
                return dto is null ? NotFound() : Ok(dto);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("marked-days")]
        public ActionResult<PagedResult<DateTime>> GetMarkedDays(
     [FromQuery] long parentProfileId,
     [FromQuery] SymptomDiarySearchObject search)
        {
            return Ok(_service.GetMarkedDays(parentProfileId, search));
        }
    }
}

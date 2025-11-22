using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SymptomDiaryController : ControllerBase
    {
        private readonly ISymptomDiaryService _service;

        public SymptomDiaryController(ISymptomDiaryService service)
        {
            _service = service;
        }

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
        public ActionResult<IEnumerable<SymptomDiaryResponseDto>> GetByParent(long parentProfileId)
            => Ok(_service.GetByParent(parentProfileId));

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
    }
}

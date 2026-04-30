using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nestly.Model.DTOObjects;
using Nestly.Services.Interfaces;

namespace Nestly.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FoodTypeController : ControllerBase
    {
        private readonly IFoodTypeService _service;

        public FoodTypeController(IFoodTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<PagedResult<FoodTypeDto>> Get([FromQuery] FoodTypeSearchObject search)
        {
            return Ok(_service.Get(search));
        }

        [HttpGet("{id}")]
        public ActionResult<FoodTypeDto> GetById(int id)
        {
            var result = _service.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<FoodTypeDto> Create([FromBody] FoodTypeInsertDto request)
        {
            var result = _service.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id}")]
        public ActionResult<FoodTypeDto> Update(int id, [FromBody] FoodTypeUpdateDto request)
        {
            var result = _service.Update(id, request);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}

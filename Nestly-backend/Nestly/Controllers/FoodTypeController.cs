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
        public async Task<ActionResult<PagedResult<FoodTypeDto>>> Get([FromQuery] FoodTypeSearchObject search)
        {
            return Ok(await _service.Get(search));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodTypeDto>> GetById(int id)
        {
            var result = await _service.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<FoodTypeDto>> Create([FromBody] FoodTypeInsertDto request)
        {
            var result = await _service.Create(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<FoodTypeDto>> Update(int id, [FromBody] FoodTypeUpdateDto request)
        {
            var result = await _service.Update(id, request);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}

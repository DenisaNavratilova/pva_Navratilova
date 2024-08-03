using Microsoft.AspNetCore.Mvc;
using pva.Models;
using pva.Services;
using System.Threading.Tasks;

namespace pva.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _valueService;

        public ValuesController(ApplicationDbContext context, INotificationService valueService)
        {
            _context = context;
            _valueService = valueService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateValue([FromBody] Value newValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authToken = Request.Headers["Authorization"];
            if (authToken != "123456789")
            {
                return Unauthorized();
            }

            var station = await _context.Stations.FindAsync(newValue.StationId);
            if (station == null)
            {
                return BadRequest("StationId does not exist.");
            }

            _context.Values.Add(newValue);
            await _context.SaveChangesAsync();

            bool emailSent = await _valueService.HandleWarningsAndSendEmailAsync(newValue);

            if (!emailSent)
            {
                return StatusCode(500, "An error occurred while processing warnings.");
            }

            return CreatedAtAction(nameof(GetValue), new { id = newValue.ValueId.ToString() }, newValue);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _context.Values.FindAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }
    }
}

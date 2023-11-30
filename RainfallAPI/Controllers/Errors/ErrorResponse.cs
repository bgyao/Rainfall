using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RainfallAPI.Models.Errors;

namespace RainfallAPI.Controllers.Errors
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorResponse : ControllerBase
    {
        private readonly ErrorResponseDbContext _context;

        public ErrorResponse(ErrorResponseDbContext context)
        {
            _context = context;
        }

        // GET: api/ErrorResponses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Errors.ErrorResponse>>> GetErrorResponses()
        {
            return await _context.ErrorResponses.ToListAsync();
        }

        // GET: api/ErrorResponses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Errors.ErrorResponse>> GetErrorResponse(Guid id)
        {
            var errorResponse = await _context.ErrorResponses.FindAsync(id);

            if (errorResponse == null)
            {
                return NotFound();
            }

            return errorResponse;
        }

        // PUT: api/ErrorResponses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutErrorResponse(Guid id, Models.Errors.ErrorResponse errorResponse)
        {
            if (id != errorResponse.Id)
            {
                return BadRequest();
            }

            _context.Entry(errorResponse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ErrorResponseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ErrorResponses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Errors.ErrorResponse>> PostErrorResponse(Models.Errors.ErrorResponse errorResponse)
        {
            _context.ErrorResponses.Add(errorResponse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetErrorResponse", new { id = errorResponse.Id }, errorResponse);
        }

        // DELETE: api/ErrorResponses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteErrorResponse(Guid id)
        {
            var errorResponse = await _context.ErrorResponses.FindAsync(id);
            if (errorResponse == null)
            {
                return NotFound();
            }

            _context.ErrorResponses.Remove(errorResponse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ErrorResponseExists(Guid id)
        {
            return _context.ErrorResponses.Any(e => e.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Data;
using QuotesWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuotesWebAPI.Controllers
{
    [Route("api/quotes")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        public QuotesController(QuotesDbContext context)
        {
            _context = context;
        }

        // GET: api/quotes
        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetQuotes()
        {
            var quotes = await _context.Quotes
                .Include(q => q.TagAssignments)
                .ThenInclude(ta => ta.Tag)
                .Select(q => new
                {
                    q.Id,
                    q.Text,
                    q.Author,
                    q.Likes,
                    Tags = q.TagAssignments.Select(ta => new { ta.Tag.Id, ta.Tag.Name }).ToList()
                })
                .ToListAsync();

            return Ok(quotes);
        }


        // GET: api/quotes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetQuote(int id)
        {
            var quote = await _context.Quotes
                .Include(q => q.TagAssignments)
                .ThenInclude(ta => ta.Tag)
                .Where(q => q.Id == id)
                .Select(q => new
                {
                    q.Id,
                    q.Text,
                    q.Author,
                    q.Likes,
                    Tags = q.TagAssignments.Select(ta => new { ta.Tag.Id, ta.Tag.Name }).ToList()
                })
                .FirstOrDefaultAsync();

            if (quote == null)
            {
                return NotFound();
            }

            return Ok(quote);
        }

        // POST: api/quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuote), new { id = quote.Id }, quote);
        }

        // DELETE: api/quotes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }
            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/like")]
        public async Task<IActionResult> LikeQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            quote.Likes++;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Quote liked successfully", likes = quote.Likes });
        }

        [HttpPost("{id}/tags")]
        public async Task<IActionResult> AssignTagToQuote(int id, [FromBody] string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return BadRequest(new { message = "Tag name cannot be empty" });
            }

            var quote = await _context.Quotes.Include(q => q.TagAssignments).FirstOrDefaultAsync(q => q.Id == id);
            if (quote == null)
            {
                return NotFound(new { message = "Quote not found" });
            }

            // Create tag if it does not exist
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
            if (tag == null)
            {
                tag = new Tag { Name = tagName };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync(); // Save tag creation immediately
            }

            // Check if tag is already assigned to the quote
            bool alreadyAssigned = await _context.TagAssignments.AnyAsync(ta => ta.QuoteId == quote.Id && ta.TagId == tag.Id);
            if (!alreadyAssigned)
            {
                var tagAssignment = new TagAssignment { QuoteId = quote.Id, TagId = tag.Id };
                _context.TagAssignments.Add(tagAssignment);
                await _context.SaveChangesAsync(); // Save tag assignment
            }

            return Ok(new { message = $"Tag '{tagName}' assigned to quote ID {quote.Id}.", quoteId = quote.Id, tagId = tag.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuote(int id, [FromBody] Quote updatedQuote)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            // Update the fields
            if (!string.IsNullOrWhiteSpace(updatedQuote.Text))
                quote.Text = updatedQuote.Text;

            // Author is optional
            quote.Author = updatedQuote.Author;

            await _context.SaveChangesAsync();
            return Ok(quote);
        }

        [HttpGet("topliked")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopLikedQuotes([FromQuery] int count = 10)
        {
            var topQuotes = await _context.Quotes
                .OrderByDescending(q => q.Likes)
                .Take(count)
                .Include(q => q.TagAssignments)
                    .ThenInclude(ta => ta.Tag)
                .Select(q => new
                {
                    q.Id,
                    q.Text,
                    q.Author,
                    q.Likes,
                    Tags = q.TagAssignments.Select(ta => new { ta.Tag.Id, ta.Tag.Name }).ToList()
                })
                .ToListAsync();

            return Ok(topQuotes);
        }

        [HttpGet("bytag/{tag}")]
        public async Task<ActionResult<IEnumerable<object>>> GetQuotesByTag(string tag)
        {
            var quotes = await _context.Quotes
                .Include(q => q.TagAssignments)
                .ThenInclude(ta => ta.Tag)
                .Where(q => q.TagAssignments.Any(ta => ta.Tag.Name.ToLower() == tag.ToLower()))
                .Select(q => new
                {
                    q.Id,
                    q.Text,
                    q.Author,
                    q.Likes,
                    Tags = q.TagAssignments.Select(ta => new { ta.Tag.Id, ta.Tag.Name }).ToList()
                })
                .ToListAsync();

            return Ok(quotes);
        }



    }
}

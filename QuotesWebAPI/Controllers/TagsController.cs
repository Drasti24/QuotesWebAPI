//DRASTI PATEL
//MARCH 30, 2025
//PROLEM ANALYSIS 03

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Data;
using QuotesWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesWebAPI.Controllers
{
    //set the base route for this controller to "api/tags"
    [Route("api/tags")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly QuotesDbContext _context;

        //constructor that receives the database context via dependency injection
        public TagsController(QuotesDbContext context)
        {
            _context = context;
        }

        // GET: api/tags
        //returns a list of all available tags in the database.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            return await _context.Tags.ToListAsync();
        }

        // POST: api/tags
        // adds a new tag to the dtaabase
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
            _context.Tags.Add(tag);     //add new tag to the context
            await _context.SaveChangesAsync();   //save changes to the databse

            //return a 201 created response with location info
            return CreatedAtAction(nameof(GetTags), new { id = tag.Id }, tag);
        }
    }
}

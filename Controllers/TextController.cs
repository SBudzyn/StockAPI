using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Data;
using StockAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly StockContext _context;
        public TextController(StockContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult GetTexts()
        {
            if (_context.Texts == null || !_context.Texts.Any())
            {
                return NotFound();
            }
            
            var texts = _context.Texts.Include(t => t.Author).Select(t => new
            {
                Id = t.Id,
                Name = t.Name,
                Text = t.TextInfo,
                DateOfCreation = t.DateOfCreation,
                Cost = t.Cost,
                NameOfAuthor = t.Author.FirstName,
                NicknameOfAuthor = t.Author.NickName,
                NumberOfSales = t.NumberOfSales,
                Rating = Math.Round(t.SumOfRatings / (t.NumOfReviews == 0 ? 1 : t.NumOfReviews), 2)
            });

            return Ok(texts);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetText(int id)
        {
            if (_context.Texts == null)
            {
                return NotFound();
            }

            var text = await _context.Texts.Include(t => t.Author).FirstOrDefaultAsync(t => t.Id == id);
            if (text == null)
            {
                return NotFound();
            }

            var result = new
            {
                Id = text.Id,
                Name = text.Name,
                Text = text.TextInfo,
                DateOfCreation = text.DateOfCreation,
                Cost = text.Cost,
                NameOfAuthor = text.Author.FirstName,
                NicknameOfAuthor = text.Author.NickName,
                NumberOfSales = text.NumberOfSales,
                Rating = Math.Round(text.SumOfRatings / (text.NumOfReviews == 0 ? 1 : text.NumOfReviews), 2)
            };
            return Ok(result);

        }
        [HttpPost]
        public async Task<ActionResult<Text>> PostText(PostTextRequest textRequest)
        {
            if (_context.Texts == null)
            {
                return BadRequest();
            }
            if (_context.Authors.FirstOrDefault(a => a.Id == textRequest.AuthorsId) == null)
            {
                return BadRequest();
            }

            Text text = new();

            text.Name = textRequest.Name;
            text.Author = _context.Authors.Find(textRequest.AuthorsId)!;
            text.TextInfo = textRequest.Text;
            text.DateOfCreation = DateTime.Now;
            text.Cost = textRequest.Cost;
            text.NumberOfSales = textRequest.NumberOfSales;

            await _context.Texts.AddAsync(text);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostText), new { Id = text.Id }, text);
        }
        [HttpPost("Rate/{id}/{rating}")]
        public async Task<IActionResult> RateText(int id, Decimal rating)
        {
            if (id == null || rating == null)
            {
                return BadRequest();
            }
            var text = await _context.Texts.FindAsync(id);
            if (text == null)
            {
                return BadRequest();
            }
            text.SumOfRatings += rating!;
            text.NumOfReviews++;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutText(int? id, PostTextRequest textRequest)
        {
            if (id == null || !_context.Texts.Any(t => t.Id == id))
            {
                return BadRequest();
            }

            Text text = _context.Texts.FirstOrDefault(t => t.Id == id)!;

            text.Name = textRequest.Name;
            text.Author = _context.Authors.Find(textRequest.AuthorsId)!;
            text.TextInfo = textRequest.Text;
            text.Cost = textRequest.Cost;
            text.NumberOfSales = textRequest.NumberOfSales;

            _context.Entry(text).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!_context.Texts.Any(t => t.Id == id))
                {
                    return NotFound();
                }

            }
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteText(int id)
        {
            if (_context.Texts == null)
            {
                return NotFound();
            }
            var text = _context.Texts.FirstOrDefault(t => t.Id == id);
            if (text == null)
            {
                return NotFound();
            }
            _context.Texts.Remove(text);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        

    }
}

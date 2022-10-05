using StockAPI.Data;
using StockAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly StockContext _context;
        private readonly IWebHostEnvironment _environment;
        
        
        public PhotosController(StockContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
           
        }
        [HttpGet]
        public async Task<ActionResult> GetPhotos()
        {

            if (_context.Photos == null || !_context.Photos.Any())
            {
                return NotFound();
            }

            Photo firstPhoto = (await _context.Photos.Include(p => p.Author).FirstOrDefaultAsync())!;

            var photos = _context.Photos.Include(p => p.Author).Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Link = p.Link,
                OriginalSize = p.OriginalSize,
                DateOfCreation = p.DateOfCreation,
                NameOfAuthor = p.Author.FirstName,
                NicknameOfAuthor = p.Author.NickName,
                Cost = p.Cost,
                NumberOfSales = p.NumberOfSales,
                Rating = Math.Round(p.SumOfRatings / (p.NumOfReviews == 0 ? 1 : p.NumOfReviews), 2)
            });

            return Ok(photos);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Photo>> GetPhoto(int id)
        {
            if (_context.Photos == null)
            {
                return NotFound();
            }
            var photo = await _context.Photos.Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == id);
            if (photo == null)
            {
                return NotFound();
            }
            var result = new
            {
                Id = photo.Id,
                Name = photo.Name,
                Link = photo.Link,
                OriginalSize = photo.OriginalSize,
                DateOfCreation = photo.DateOfCreation,
                NameOfAuthor = photo.Author.FirstName,
                NicknameOfAuthor = photo.Author.NickName,
                Cost = photo.Cost,
                NumberOfSales = photo.NumberOfSales,
                Rating = Math.Round(photo.SumOfRatings / (photo.NumOfReviews == 0 ? 1 : photo.NumOfReviews), 2)
            };
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Photo>> PostPhoto([FromForm] PostPhotoRequest photoRequest)
        {           

            if (photoRequest.Photo == null)
            {
                return BadRequest();
            }
            if (!_context.Authors.Any(a => a.Id == photoRequest.AuthorsId))
            {
                return BadRequest();
            }

            IFormFile file = photoRequest.Photo;

            Photo photo = new();
            photo.Name = file.FileName;
            photo.Author = _context.Authors.FirstOrDefault(a => a.Id == photoRequest.AuthorsId)!;
            photo.Cost = photoRequest.Cost;
            photo.DateOfCreation = DateTime.Now;
            photo.Link = "images/" + file.FileName;
            photo.NumberOfSales = photoRequest.NumberOfSales;
            photo.OriginalSize = file.Length;           
            
            var path = Path.Combine("images", file.FileName);

            await using FileStream filestream = new(path, FileMode.Create);

            await file.CopyToAsync(filestream);

            await _context.Photos.AddAsync(photo);

            await _context.SaveChangesAsync();

            
            return CreatedAtAction(nameof(PostPhoto), new { Id = photo.Id }, photo);

        }
        [HttpPost("Rate/{id}/{rating}")]
        public async Task<IActionResult> RatePhoto(int id, Decimal rating)
        {
            if (id == null || rating == null)
            {
                return BadRequest();
            }
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return BadRequest();
            }
            photo.SumOfRatings += rating!;
            photo.NumOfReviews++;
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
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutPhoto(int? id,[FromForm] PostPhotoRequest photoRequest)
        {
            if (_context.Photos == null || id == null || !_context.Photos.Any(p => p.Id == id))
            {
                return BadRequest();
            }

            Photo photo = _context.Photos.FirstOrDefault(p => p.Id == id)!;

            var oldPath = Path.Combine(_environment.ContentRootPath, "images", photo.Name);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            photo.Name = photoRequest.Photo.FileName;
            photo.Author = _context.Authors.Find(photoRequest.AuthorsId)!;
            photo.Cost = photoRequest.Cost;
            photo.Link = "images/" + photoRequest.Photo.FileName;
            photo.NumberOfSales = photoRequest.NumberOfSales;
            photo.OriginalSize = photoRequest.Photo.Length;

            _context.Entry(photo).State = EntityState.Modified;

            var newPath = Path.Combine(_environment.ContentRootPath, "images", photo.Name);

            await using FileStream filestream = new(newPath, FileMode.Create);

            await photoRequest.Photo.CopyToAsync(filestream);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!_context.Photos.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                
            }
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            if (_context.Photos == null)
            {
                return NotFound();
            }
            var photo = _context.Photos.FirstOrDefault(t => t.Id == id);
            if (photo == null)
            {
                return NotFound();
            }
            _context.Photos.Remove(photo);

            var path = Path.Combine(_environment.ContentRootPath, "images", photo.Name);

            System.IO.File.Delete(path);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}

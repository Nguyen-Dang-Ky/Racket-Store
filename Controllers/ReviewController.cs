using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serberus_Racket_Store.Data;
using Serberus_Racket_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace racket_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     // Restrict access to Admin role
    public class ReviewController : ControllerBase
    {
        private readonly SeberusDbContext _context;

        public ReviewController(SeberusDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reviews>>> GetReviews()
        {
            var reviews = await _context.Reviews.Where(r => !r.isDelete).ToListAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reviews>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review is null || review.isDelete)
                return NotFound();
            return Ok(review);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Reviews>> AddReview(Reviews newReview)
        {
            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReview), new { id = newReview.reviewId }, newReview);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Reviews>> UpdateReview(int id, Reviews updateReview)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review is null || review.isDelete)
                return BadRequest();

            review.userId = updateReview.userId;
            review.racketId = updateReview.racketId;
            review.rating = updateReview.rating;
            review.comment = updateReview.comment;
            review.createAt = updateReview.createAt;

            await _context.SaveChangesAsync();
            return Ok(review);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review is null || review.isDelete)
                return NotFound();

            review.isDelete = true; // Soft delete
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

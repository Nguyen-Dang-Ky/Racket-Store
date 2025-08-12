using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serberus_Racket_Store.Data;
using Serberus_Racket_Store.Models;

namespace racket_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController(SeberusDbContext context) : ControllerBase
    {
        private readonly SeberusDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brands>>> GetBrands()
        {
            return Ok(await _context.Brands.Where(b => !b.isDelete).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brands>> GetBrand(int id)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.brandId == id && !b.isDelete);
            if (brand is null)
                return NotFound();
            return Ok(brand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Brands>> AddBrand(Brands newBrand)
        {
            _context.Brands.Add(newBrand);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBrand), new { id = newBrand.brandId }, newBrand);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Brands>> updateBrand(int id, Brands updatebrand)
        {
            var brand = await _context.Brands.FindAsync(id);
            if(brand is null || brand.isDelete)
                return BadRequest();

            brand.brandName = updatebrand.brandName;
            await _context.SaveChangesAsync();
            return Ok(brand);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Brands>> deleteBrand (int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand is null || brand.isDelete)
                return BadRequest();

            brand.isDelete = true;
            await _context.SaveChangesAsync();
            return NoContent();
                
        }
    }
}

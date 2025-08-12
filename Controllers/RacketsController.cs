using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serberus_Racket_Store.Data;
using Serberus_Racket_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Serberus_Racket_Store.DTOs.RacketDTOs;
using System;

namespace racket_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Restrict access to Admin role
    public class RacketsController : ControllerBase
    {
        private readonly SeberusDbContext _context;
        private readonly IMapper _mapper;
        public RacketsController(SeberusDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RacketDto>>> GetRackets()
        {
            var rackets = await _context.Rackets
                                        .Where(r => !r.isDelete)
                                        .Include(r => r.Brands)
                                        .ToListAsync();

            var racketDtos = _mapper.Map<IEnumerable<RacketDto>>(rackets).ToList();

            foreach (var racketDto in racketDtos)
            {
                if (racketDto.ImageURL != null && racketDto.ImageURL.StartsWith("PICTURE:"))
                {
                    var relativePath = racketDto.ImageURL.Replace("PICTURE:", "/images");
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                    racketDto.ImageURL = $"{baseUrl}{relativePath}";
                }
            }
            return Ok(racketDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RacketDto>> GetRacket(int id)
        {
            var racket = await _context.Rackets
                                       .Include(r => r.Brands)
                                       .FirstOrDefaultAsync(r => r.racketId == id);
            if (racket is null || racket.isDelete)
                return NotFound(new { mesage = "không tìm thấy vợt hoặc bị xóa." });

            var racketDto = _mapper.Map<RacketDto>(racket);

            if (racketDto.ImageURL != null && racketDto.ImageURL.StartsWith("PICTURE:"))
            {
                var relativePath = racketDto.ImageURL.Replace("PICTURE:", "/images");
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                racketDto.ImageURL = $"{baseUrl}{relativePath}";
            }
            return Ok(racketDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<RacketDto>> CreateRacket(CreateRacketDto createDto)
        {
            var racket = _mapper.Map<Rackets>(createDto);

            //racket.racketCode = "RCK" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            _context.Rackets.Add(racket);
            await _context.SaveChangesAsync();

            var createdRacketWithBrand = await _context.Rackets
                                                       .Include(r => r.Brands)
                                                       .FirstOrDefaultAsync(r => r.racketId == racket.racketId);
            var createdDto = _mapper.Map<RacketDto>(createdRacketWithBrand);
            if (createdDto.ImageURL != null && createdDto.ImageURL.StartsWith("PICTURE:"))
            {
                var relativePath = createdDto.ImageURL.Replace("PICTURE:", "/images");
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                createdDto.ImageURL = $"{baseUrl}{relativePath}";
            }
            return CreatedAtAction(nameof(GetRacket), new { id = createdDto.RacketId }, createdDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<RacketDto>> UpdateRacket(int id, UpdateRacketDto updateDto) 
        {
            var racketToUpdate = await _context.Rackets
                                                .Include(r => r.Brands)
                                                .FirstOrDefaultAsync(r => r.racketId == id);

            if (racketToUpdate is null || racketToUpdate.isDelete)
                return NotFound(new { message = "Không tìm thấy vợt hoặc đã bị xóa." });

            _mapper.Map(updateDto, racketToUpdate);


            await _context.SaveChangesAsync();

            var updatedDto = _mapper.Map<RacketDto>(racketToUpdate);

            if (updatedDto.ImageURL != null && updatedDto.ImageURL.StartsWith("PICTURE:"))
            {
                var relativePath = updatedDto.ImageURL.Replace("PICTURE:", "/images");
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                updatedDto.ImageURL = $"{baseUrl}{relativePath}";
            }

            return Ok(updatedDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRacket(int id)
        {
            var racket = await _context.Rackets.FindAsync(id);
            if (racket is null || racket.isDelete)
                return NotFound(new { message = "Không tìm thấy vợt hoặc đã bị xóa." });

            racket.isDelete = true; // Soft delete
            await _context.SaveChangesAsync();
            return NoContent(); 
        }
    }
}

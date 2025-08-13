using AutoMapper; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serberus_Racket_Store.Data;
using Serberus_Racket_Store.Models;
using Serberus_Racket_Store.DTOs.ShippingInfoDTOs;

namespace racket_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingInfoController : ControllerBase
    {
        private readonly SeberusDbContext _context;
        private readonly IMapper _mapper;
        public ShippingInfoController(SeberusDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingInfoDto>>> GetInfo() 
        {
            var shippingInfos = await _context.Shippinginfo
                                            .Where(s => !s.isDelete)
                                            .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ShippingInfoDto>>(shippingInfos));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingInfoDto>> GetInfo(int id)
        {
            var shipinfo = await _context.Shippinginfo.FindAsync(id);
            if (shipinfo is null || shipinfo.isDelete)
                return NotFound();
            return Ok(_mapper.Map<ShippingInfoDto>(shipinfo));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ShippingInfoDto>> AddInfo(CreateShippingInfoDto newInfoDto) 
        {
            var newShippingInfo = _mapper.Map<Shipping_Info>(newInfoDto);

            newShippingInfo.shippingCode = GenerateShippingCode(); 
            newShippingInfo.shippingFee = CalculateShippingFee(newInfoDto.ShippingMethod); 

            _context.Shippinginfo.Add(newShippingInfo);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<ShippingInfoDto>(newShippingInfo);
            return CreatedAtAction(nameof(GetInfo), new { id = createdDto.ShippingId }, createdDto);
        }
        private string GenerateShippingCode()
        {
            // Logic để tạo mã vận chuyển duy nhất, ví dụ: "SHP-" + Guid.NewGuid().ToString("N").Substring(0, 8)
            return "SHP-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
        private decimal CalculateShippingFee(string? shippingMethod)
        {
            return shippingMethod?.ToLower() switch
            {
                "standard" => 25000,
                "express" => 50000,
                _ => 0 
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ShippingInfoDto>> UpdateInfo(int id, UpdateShippingInfoDto updateInfoDto) // Nhận DTO đầu vào
        {
            var shipinfo = await _context.Shippinginfo.FindAsync(id);
            if (shipinfo is null || shipinfo.isDelete)
                return NotFound("Thông tin vận chuyển không tồn tại hoặc đã bị xóa."); 

            _mapper.Map(updateInfoDto, shipinfo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Shippinginfo.Any(e => e.shippingId == id))
                {
                    return NotFound("Thông tin vận chuyển không còn tồn tại.");
                }
                throw;
            }
            return Ok(_mapper.Map<ShippingInfoDto>(shipinfo));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInfo(int id)
        {
            var shipinfo = await _context.Shippinginfo.FindAsync(id);
            if (shipinfo is null || shipinfo.isDelete)
                return NotFound("Thông tin vận chuyển không tồn tại hoặc đã bị xóa.");

            shipinfo.isDelete = true; 
            await _context.SaveChangesAsync();
            return NoContent(); 
        }
    }
}

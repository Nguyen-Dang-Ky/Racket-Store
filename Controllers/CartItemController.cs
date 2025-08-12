using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serberus_Racket_Store.Data;
using Serberus_Racket_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper; 
using Serberus_Racket_Store.DTOs.CartItemDTOs;

namespace Serberus_Racket_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly SeberusDbContext _context ;
        private readonly IMapper _mapper;

        public CartItemController(SeberusDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<IEnumerable<CartDto>>> GetAllCartItems() 
        {
            var cartItems = await _context.CartItems
                                           .Where(c => !c.isDelete)
                                           .Include(ci => ci.Users)    
                                           .Include(ci => ci.Rackets)  
                                           .ToListAsync();
            var cartItemDtos = _mapper.Map<IEnumerable<CartDto>>(cartItems);
            return Ok(cartItemDtos);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetCartItemsByUserId(int userId)  
        {
            var cartItems = await _context.CartItems
                                           .Where(ci => ci.userId == userId && !ci.isDelete)
                                           .Include(ci => ci.Users)    
                                           .Include(ci => ci.Rackets)  
                                           
                                           .ToListAsync();

            if (!cartItems.Any())
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm nào trong giỏ hàng của người dùng này." });
            }

            var cartItemDtos = _mapper.Map<IEnumerable<CartDto>>(cartItems);
            return Ok(cartItemDtos);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartItemId, [FromBody] UpdateCartDto updateDto) // <-- Sử dụng UpdateCartItemDto
        {
            if (updateDto.Quantity <= 0) 
            {
                return BadRequest(new { message = "Số lượng sản phẩm phải lớn hơn 0." });
            }

            var cartItem = await _context.CartItems
                                         .FirstOrDefaultAsync(ci => ci.cartItemId == cartItemId && !ci.isDelete);

            if (cartItem == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng." });
            }

            _mapper.Map(updateDto, cartItem);

            await _context.SaveChangesAsync();

            var updatedCartItemWithRelations = await _context.CartItems
                                                            .Include(ci => ci.Users)
                                                            .Include(ci => ci.Rackets)
                                                            .FirstOrDefaultAsync(ci => ci.cartItemId == cartItemId);

            var updatedDto = _mapper.Map<CartDto>(updatedCartItemWithRelations);

            return Ok(updatedDto); 
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.cartItemId == cartItemId && !ci.isDelete);
            if (cartItem == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng." });
            }

            cartItem.isDelete = true; // Soft delete
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sản phẩm đã được xóa khỏi giỏ hàng (soft delete)." });
        }

        [HttpPost("add")]
        public async Task<ActionResult<CartDto>> AddOrUpdateCartItem([FromBody] CreateCartDto createDto) 
        {
            if (createDto.Quantity <= 0) 
            {
                return BadRequest(new { message = "Số lượng sản phẩm phải lớn hơn 0." });
            }

            var existingCartItem = await _context.CartItems
                                                 .FirstOrDefaultAsync(ci =>
                                                     ci.userId == createDto.UserId &&
                                                     ci.racketId == createDto.RacketId && 
                                                     !ci.isDelete);

            if (existingCartItem != null)
            {
                existingCartItem.quantity += createDto.Quantity; 
                await _context.SaveChangesAsync();
                var updatedCartItemWithRelations = await _context.CartItems
                                                                .Include(ci => ci.Users)
                                                                .Include(ci => ci.Rackets)
                                                                .FirstOrDefaultAsync(ci => ci.cartItemId == existingCartItem.cartItemId);

                var updatedDto = _mapper.Map<CartDto>(updatedCartItemWithRelations);
                return Ok(updatedDto); 
            }
            else
            {
                var newCartItem = _mapper.Map<CartItems>(createDto);


                _context.CartItems.Add(newCartItem);
                await _context.SaveChangesAsync();

                var createdCartItemWithRelations = await _context.CartItems
                                                                .Include(ci => ci.Users)
                                                                .Include(ci => ci.Rackets)
                                                                .FirstOrDefaultAsync(ci => ci.cartItemId == newCartItem.cartItemId);

                var createdDto = _mapper.Map<CartDto>(createdCartItemWithRelations);
                return CreatedAtAction(nameof(GetCartItemsByUserId), new { userId = createdDto.UserId }, createdDto); 
            }
        }
    }
}

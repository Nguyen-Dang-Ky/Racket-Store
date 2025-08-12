using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serberus_Racket_Store.Data;
using Serberus_Racket_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Serberus_Racket_Store.DTOs.OrderItemDTOs;
using Serberus_Racket_Store.DTOs.OrderDTOs;
using System.Net.WebSockets;

namespace Serberus_Racket_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")] // Restrict access to Admin role
    public class OrderItemController : ControllerBase
    {
        private readonly SeberusDbContext _context;
        private readonly IMapper _mapper;

        public OrderItemController(SeberusDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetOrderIDto()
        {
            var orderItems = await _context.OrderItems
                .Where(o => !o.isDelete)
                .Include(oi => oi.Rackets)
                .ToListAsync();

            var orderItemDtos = _mapper.Map<IEnumerable<OrderItemDto>>(orderItems);
            return Ok(orderItemDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemDto>> GetOrderItems(int id)
        {
            var orderItem = await _context.OrderItems
                                            .Include(oi => oi.Rackets)
                                            .FirstOrDefaultAsync(oi => oi.orderItemId == id);
            if (orderItem is null || orderItem.isDelete)
                return NotFound();

            var orderItemDto = _mapper.Map<OrderItemDto>(orderItem);
            return Ok(orderItemDto);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDto>> CreateOrderItem(CreateOrderItemDto createDto)
        {
            var orderItem = _mapper.Map<OrderItems>(createDto);

            var racket = await _context.Rackets.FindAsync(createDto.RacketId);
            if(racket == null)
            {
                return BadRequest("Invalid RacketId.");
            }
            orderItem.price = racket.price;

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            var createOrderItem = await _context.OrderItems
                                                .Include(oi => oi.Rackets)
                                                .FirstOrDefaultAsync(oi => oi.orderItemId == orderItem.orderItemId);

            var createdDto = _mapper.Map<OrderItemDto>(createOrderItem);
            return CreatedAtAction(nameof(GetOrderItems), new { id = createdDto.OrderItemId }, createdDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItemDto>> UpdateOrderItem(int id, UpdateOrderItemDto updateOrderItems)
        {
            var itemToUpdate = await _context.OrderItems
                .Include(oi => oi.Rackets)
                .FirstOrDefaultAsync(oi => oi.orderItemId == id);

            if (itemToUpdate is null || itemToUpdate.isDelete)
                return NotFound();

            _mapper.Map(updateOrderItems, itemToUpdate.isDelete);

            await _context.SaveChangesAsync();

            var updateDto = _mapper.Map<OrderItemDto>(itemToUpdate);
            return Ok(updateDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem is null || orderItem.isDelete)
                return NotFound();

            orderItem.isDelete = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serberus_Racket_Store.Data;
using Serberus_Racket_Store.DTOs.OrderDTOs;
using Serberus_Racket_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace racket_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     // Restrict access to Admin role
    public class OrdersController : ControllerBase
    {
        private readonly SeberusDbContext _context;
        private readonly IMapper _mapper;
        public OrdersController(SeberusDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                                       .Where(o => !o.isDelete)
                                       .Include(o => o.Shipping_Info)
                                       .Include(o => o.OrderItems)
                                            .ThenInclude(o => o.Rackets)
                                       .ToListAsync();

            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                                      .Include(o => o.Shipping_Info)
                                      .Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Rackets)
                                      .FirstOrDefaultAsync(o => o.orderId == id);

            if (order is null || order.isDelete)
                return NotFound(new { message = "didn't found order " });

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }


        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createDto) 
        {
            var order = _mapper.Map<Orders>(createDto);

            order.orderCode = "ORD" + Guid.NewGuid().ToString().Substring(0, 8);

            // Xử lý OrderItems và tính toán total
            decimal calculatedTotal = 0;
            if (order.OrderItems != null && order.OrderItems.Any())
            {
                foreach (var item in order.OrderItems)
                {
                    var racket = await _context.Rackets.FindAsync(item.racketId);
                    if (racket == null)
                    {
                        return BadRequest($"Không tìm thấy vợt với ID: {item.racketId}");
                    }
                    item.price = racket.price; 
                    calculatedTotal += item.quantity * item.price;
                }
            }
            order.total = calculatedTotal; // Gán tổng tiền đã tính

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var createdOrderWithRelations = await _context.Orders
                                                          .Include(o => o.Shipping_Info)
                                                          .Include(o => o.OrderItems)
                                                                .ThenInclude(oi => oi.Rackets)
                                                          .FirstOrDefaultAsync(o => o.orderId == order.orderId);

            var createdDto = _mapper.Map<OrderDto>(createdOrderWithRelations);
            return CreatedAtAction(nameof(GetOrder), new { id = createdDto.OrderId }, createdDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, UpdateOrderDto updateDto)
        {
            var orderToUpdate = await _context.Orders
                                              .Include(o => o.Shipping_Info) 
                                              .Include(o => o.OrderItems)
                                                    .ThenInclude(oi => oi.Rackets)
                                              .FirstOrDefaultAsync(o => o.orderId == id);

            if (orderToUpdate is null || orderToUpdate.isDelete)
                return NotFound(new { message = "Không tìm thấy đơn hàng hoặc đã bị xóa." });

            _mapper.Map(updateDto, orderToUpdate);


            await _context.SaveChangesAsync();

            var updatedDto = _mapper.Map<OrderDto>(orderToUpdate);
            return Ok(updatedDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is null || order.isDelete)
                return NotFound(new { message = "Không tìm thấy đơn hàng hoặc đã bị xóa." });

            order.isDelete = true; // Soft delete
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

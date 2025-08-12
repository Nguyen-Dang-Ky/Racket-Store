using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serberus_Racket_Store.Data;
using Serberus_Racket_Store.DTOs.UserDTOs;
using Serberus_Racket_Store.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System; 

namespace racket_store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SeberusDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(SeberusDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.Where(u => !u.isDelete).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id) 
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null || user.isDelete)
                return NotFound(new { message = "Không tìm thấy người dùng hoặc đã bị xóa." });

            return Ok(_mapper.Map<UserDto>(user));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(CreateUserDto dto) 
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.email == dto.email && !u.isDelete);
            if (existingUser != null)
                return BadRequest(new { message = "Email đã được sử dụng." });

            var user = _mapper.Map<Users>(dto);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetUser), new { id = userDto.userId }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
        {
            var currentUserIdClaim = User.FindFirst("userId")?.Value;
            var currentUserRoleClaim = User.FindFirst("role")?.Value;

            if (!int.TryParse(currentUserIdClaim, out int currentUserId))
            {
                return Unauthorized(new { message = "Không xác định được ID người dùng hiện tại." });
            }

            var userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate == null || userToUpdate.isDelete)
                return NotFound(new { message = "Không tìm thấy người dùng hoặc đã bị xóa." });

            if (id != currentUserId && currentUserRoleClaim != "Admin")
            {
                return StatusCode(403, new { message = "Bạn không có quyền cập nhật thông tin của người dùng khác." });
            }

            _mapper.Map(dto, userToUpdate);

            if (currentUserRoleClaim == "Admin" && !string.IsNullOrEmpty(dto.role))
            {
                userToUpdate.role = dto.role;
            }
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.isDelete)
                return NotFound(new { message = "Không tìm thấy người dùng hoặc đã bị xóa." });

            user.isDelete = true; // Soft delete
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email.ToLower() == request.Email.ToLower() && !u.isDelete);

            if (user == null)
                return StatusCode(401, new { message = "Email hoặc mật khẩu không chính xác." });


            if (user.passwordHash != request.Password) 
            {
                return StatusCode(401, new { message = "Email hoặc mật khẩu không chính xác." });
            }

            string jwtToken = "simple-jwt-token-placeholder-generated-by-your-auth-service";

            var response = new LoginResponseDto
            {
                UserId = user.userId,
                FullName = user.fullName,
                Email = user.email,
                Role = user.role,
                Token = jwtToken
            };
            return Ok(response);
        }


        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
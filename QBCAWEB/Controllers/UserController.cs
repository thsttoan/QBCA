using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QBCAWEB.Data;
using QBCAWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace QBCAWEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<ResponseModel<List<UserViewModel>>>> GetAll()
        {
            var usersFromDb = await _context.Users
                                        .Include(u => u.UserRole)
                                        .ToListAsync();

            var userViewModels = usersFromDb.Select(u => new UserViewModel
            {
                UserID = u.UserID,
                FullName = u.FullName,
                Email = u.Email,
                RoleName = u.UserRole?.RoleName ?? "N/A",
                RoleID = u.RoleID,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            }).ToList();

            return Ok(ResponseModel<List<UserViewModel>>.SuccessResult(userViewModels, "Successfully retrieved user list."));
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<UserViewModel>>> GetById(int id)
        {
            var user = await _context.Users
                                     .Include(u => u.UserRole)
                                     .FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
            {
                return NotFound(ResponseModel<UserViewModel>.ErrorResult("User not found.", 404));
            }

            var userViewModel = new UserViewModel
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                RoleName = user.UserRole?.RoleName ?? "N/A",
                RoleID = user.RoleID,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return Ok(ResponseModel<UserViewModel>.SuccessResult(userViewModel, "Successfully retrieved user information."));
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<ResponseModel<UserViewModel>>> Create([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage);
                string combinedErrorMessage = string.Join("; ", errorMessages);
                if (string.IsNullOrEmpty(combinedErrorMessage))
                {
                    combinedErrorMessage = "Invalid data provided.";
                }
                return BadRequest(ResponseModel<UserViewModel>.ErrorResult(combinedErrorMessage, 400));
            }

            if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email))
            {
                return BadRequest(ResponseModel<UserViewModel>.ErrorResult("Email already exists."));
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == createUserDto.RoleName);
            if (role == null)
            {
                return BadRequest(ResponseModel<UserViewModel>.ErrorResult($"Role '{createUserDto.RoleName}' not found."));
            }

            var newUser = new User
            {
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                RoleID = role.RoleID,
                CreatedAt = DateTime.UtcNow,
                IsActive = createUserDto.IsActive ?? true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var userViewModel = new UserViewModel
            {
                UserID = newUser.UserID,
                FullName = newUser.FullName,
                Email = newUser.Email,
                RoleName = role.RoleName,
                RoleID = newUser.RoleID,
                IsActive = newUser.IsActive,
                CreatedAt = newUser.CreatedAt
            };

            return CreatedAtAction(nameof(GetById),
                new { id = newUser.UserID },
                ResponseModel<UserViewModel>.SuccessResult(userViewModel, "User created successfully."));
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseModel<UserViewModel>>> Update(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (id != updateUserDto.UserID)
            {
                return BadRequest(ResponseModel<UserViewModel>.ErrorResult("User ID mismatch."));
            }

            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                return NotFound(ResponseModel<UserViewModel>.ErrorResult("User not found.", 404));
            }

            if (!string.IsNullOrEmpty(updateUserDto.Email) && updateUserDto.Email != existingUser.Email)
            {
                if (await _context.Users.AnyAsync(u => u.Email == updateUserDto.Email && u.UserID != id))
                {
                    return BadRequest(ResponseModel<UserViewModel>.ErrorResult("New email already exists for another user."));
                }
                existingUser.Email = updateUserDto.Email;
            }

            if (!string.IsNullOrEmpty(updateUserDto.FullName))
                existingUser.FullName = updateUserDto.FullName;

            if (!string.IsNullOrEmpty(updateUserDto.RoleName))
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == updateUserDto.RoleName);
                if (role == null)
                {
                    return BadRequest(ResponseModel<UserViewModel>.ErrorResult($"Role '{updateUserDto.RoleName}' not found."));
                }
                existingUser.RoleID = role.RoleID;
            }

            if (updateUserDto.IsActive.HasValue)
                existingUser.IsActive = updateUserDto.IsActive.Value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound(ResponseModel<UserViewModel>.ErrorResult("User not found during update.", 404));
                }
                else
                {
                    throw;
                }
            }

            await _context.Entry(existingUser).Reference(u => u.UserRole).LoadAsync();

            var updatedUserViewModel = new UserViewModel
            {
                UserID = existingUser.UserID,
                FullName = existingUser.FullName,
                Email = existingUser.Email,
                RoleName = existingUser.UserRole?.RoleName ?? "N/A",
                RoleID = existingUser.RoleID,
                IsActive = existingUser.IsActive,
                CreatedAt = existingUser.CreatedAt
            };

            return Ok(ResponseModel<UserViewModel>.SuccessResult(updatedUserViewModel, "User updated successfully."));
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(ResponseModel<bool>.ErrorResult("User not found.", 404));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(ResponseModel<bool>.SuccessResult(true, "User deleted successfully."));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
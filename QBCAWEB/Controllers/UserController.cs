using Microsoft.AspNetCore.Mvc;
using QBCAWEB.Models;

namespace QBCAWEB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "admin", Email = "admin@qbcaweb.com", FullName = "Administrator", Role = "Admin" },
            new User { Id = 2, Username = "toantester", Email = "toantester@qbcaweb.com", FullName = "Lê Trung Toàn", Role = "User" }
         
        };

        [HttpGet]
        public ActionResult<ResponseModel<List<User>>> GetAll()
        {
            return Ok(ResponseModel<List<User>>.SuccessResult(_users, "Lấy danh sách người dùng thành công"));
        }

        [HttpGet("{id}")]
        public ActionResult<ResponseModel<User>> GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(ResponseModel<User>.ErrorResult("Không tìm thấy người dùng", 404));
            }
            return Ok(ResponseModel<User>.SuccessResult(user, "Lấy thông tin người dùng thành công"));
        }

        [HttpPost]
        public ActionResult<ResponseModel<User>> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseModel<User>.ErrorResult("Dữ liệu không hợp lệ"));
            }

            // Check if username or email already exists
            if (_users.Any(u => u.Username == user.Username))
            {
                return BadRequest(ResponseModel<User>.ErrorResult("Tên đăng nhập đã tồn tại"));
            }

            if (_users.Any(u => u.Email == user.Email))
            {
                return BadRequest(ResponseModel<User>.ErrorResult("Email đã tồn tại"));
            }

            user.Id = _users.Max(u => u.Id) + 1;
            user.CreatedDate = DateTime.Now;
            _users.Add(user);

            return CreatedAtAction(nameof(GetById),
                new { id = user.Id },
                ResponseModel<User>.SuccessResult(user, "Tạo người dùng thành công"));
        }

        [HttpPut("{id}")]
        public ActionResult<ResponseModel<User>> Update(int id, [FromBody] User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound(ResponseModel<User>.ErrorResult("Không tìm thấy người dùng", 404));
            }

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.Role = user.Role;
            existingUser.IsActive = user.IsActive;

            return Ok(ResponseModel<User>.SuccessResult(existingUser, "Cập nhật người dùng thành công"));
        }

        [HttpDelete("{id}")]
        public ActionResult<ResponseModel<bool>> Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound(ResponseModel<bool>.ErrorResult("Không tìm thấy người dùng", 404));
            }

            _users.Remove(user);
            return Ok(ResponseModel<bool>.SuccessResult(true, "Xóa người dùng thành công"));
        }
    }
}
using ASPNETAuthAPI.Context;
using ASPNETAuthAPI.Models;
using ASPNETAuthAPI.Services.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public UserController(ApplicationDBContext applicationDBContext) {
            _applicationDBContext = applicationDBContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObject) {
            if (userObject == null) return BadRequest(new {Message="Bad Request", code="0"});
            User userReturned = await _applicationDBContext.Users.FirstOrDefaultAsync(x => x.Email == userObject.Email);
            if (userReturned == null) return NotFound(new {Message="User Not Found", code="2"});
            if (!Encrypt.VerifyEncryption(userObject.Password, userReturned.Password))
                return BadRequest(new {Message="Password Mismatch", code="22"});
            return Ok(new {Message="Authentication Success", code="1"});
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObject) {
            if (userObject == null) return BadRequest();
            userObject.Password = Encrypt.Hash(userObject.Password);
            User userReturned = await _applicationDBContext.Users.FirstOrDefaultAsync(x => x.Email == userObject.Email);
            if (userReturned == null) {
                await _applicationDBContext.Users.AddAsync(userObject);
                await _applicationDBContext.SaveChangesAsync();
                return Ok(new { Message = "User Registered", code = "11" });
            }
            return BadRequest(new { Message = "User Exists", code = "10" });
        }
    }
}

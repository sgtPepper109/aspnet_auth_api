using ASPNETAuthAPI.Context;
using ASPNETAuthAPI.Models;
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
            if (userObject == null) return BadRequest();
            User userReturned = await _applicationDBContext.Users.FirstOrDefaultAsync(x => x.Email == userObject.Email);
            if (userReturned == null) return NotFound(new {Message = "User Not Found"});
            if (userReturned.Password != userObject.Password) return BadRequest(new {Message = "Password Mismatch"});
            return Ok(new {Message = "User Found"});
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObject) {
            if (userObject == null) return BadRequest();
            await _applicationDBContext.Users.AddAsync(userObject);
            await _applicationDBContext.SaveChangesAsync();
            return Ok (new {Message = "User Registered"});
        }
    }
}

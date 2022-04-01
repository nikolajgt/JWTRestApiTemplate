using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRStreamingJson.Interfaces;
using SignalRStreamingJson.Models;
using SignalRStreamingJson.Models.DTO;
using SignalRStreamingJson.Models.JWT;

namespace SignalRStreamingJson.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MockController
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMockService _service;
        private readonly IJWTService _jwtService;

        public MockController(IHttpContextAccessor contextAccessor, IMockService service, IJWTService jwtService)
        {
            _contextAccessor = contextAccessor;
            _service = service;
            _jwtService = jwtService;
        }

        //POSTS

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Login([FromBody] AuthenticateDTO a)
        {
            var newCustomer = new User(a.Username, a.Password);

            var response = await _jwtService.Authenticate(newCustomer, IpAddress());

            if (response == null)
                return new UnauthorizedObjectResult(response);

            return new OkObjectResult(response);
        }

        [HttpPost("Refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RevokeTokenRequest model)
        {
            var response = await _jwtService.RefreshToken(model.Token, IpAddress());

            if (response == null)
                return new NotFoundObjectResult(response);

            return new OkObjectResult(response);
        }

        [HttpPost("Revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            var response = await _jwtService.RevokeToken(model.Token, IpAddress());

            if (response == null)
                return new NotFoundObjectResult(response);

            return new OkObjectResult(response);
        }


        [HttpPost("Post-User")]
        public async Task<ActionResult> PostCustomerAsync([FromBody] UserDTO u)
        {
            var userModel = new User(u.Username, u.Password, u.Firstname, u.Lastname, u.Email, u.Roles);
            var response = await _service.PostUserAsync(userModel);
            if (response == false)
                return new NotFoundObjectResult(response);

            return new OkObjectResult(response);
        }



        //GETS

        [HttpGet("Get-User")]
        public async Task<ActionResult> GetUserAsync([FromQuery] string userid)
        {
            var response = await _service.GetUserAsync(userid);
            if(response == null)
                return new NotFoundObjectResult(response);

            return new OkObjectResult(response);
        }


        //UPDATES





        //REMOVES




        private string IpAddress()
        {
            if (_contextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                return _contextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            else
                return _contextAccessor.HttpContext?.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}

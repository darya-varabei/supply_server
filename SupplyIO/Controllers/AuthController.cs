using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SupplyIO.SupplyIO.Services;
using SupplyIO.SupplyIO.Services.Models;
using SupplyIO.SupplyIO.Services.Models.Login;
using SupplyIO.SupplyIO.Services.ViewModel;

namespace SupplyIO.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly string _headerName;

        public AuthController(IAuthService authService, ITokenService tokenService, IConfiguration configuration)
            => (_authService, _tokenService, _headerName) = (authService, tokenService, configuration.GetSection("HeaderName").Value);

        [HttpPost("login")]
        public async Task<ActionResult<TokenApiModel>> Login([FromBody] User loginModel)
        {
            if (loginModel == null)
                return BadRequest("Invalid client request");

            var tokens = await _authService.Login(loginModel);

            if (tokens is null)
                return Unauthorized();

            return tokens;
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckToken()
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                return Ok();
            else
                return Unauthorized();
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<UserViewModel>>> GetUsersAsync()
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                return await _authService.GetUsersAsync();
            else
                return Unauthorized();
        }

        [HttpGet]
        public async Task<ActionResult<UserViewModel>> GetUser(string login)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                var userInfo = await _authService.GetUser(login);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<UserInfo, UserViewModel>());
                var mapper = new Mapper(config);

                var userViewModel = mapper.Map<UserViewModel>(userInfo);

                return userViewModel;
            }
            else
                return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(User user)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                var result = await _authService.AddUserAsync(user);

                return result ? Ok() : BadRequest();
            }
            else
                return Unauthorized();
        }
    }
}

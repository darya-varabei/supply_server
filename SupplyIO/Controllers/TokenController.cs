using Microsoft.AspNetCore.Mvc;
using SupplyIO.SupplyIO.Services;
using SupplyIO.SupplyIO.Services.Models;
using SupplyIO.SupplyIO.Services.Models.Context;

namespace SupplyIO.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenService _service;

        public TokenController(MetalContext userContext, ITokenService tokenService)
        {
            _service = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<TokenApiModel>> Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            var newTokens = await _service.Refresh(tokenApiModel);

            return newTokens is not null ? newTokens : NotFound();
        }
    }
}

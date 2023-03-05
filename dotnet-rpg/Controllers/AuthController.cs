using dotnet_rpg.DataModels.User;
using dotnet_rpg.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO request)
        {
            var response = await _authRepo.Register(new User { Username = request.Username}, request.Password);

            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDTO request)
        {
            var response = await _authRepo.Login(request.Username, request.Password);

            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
    }
}

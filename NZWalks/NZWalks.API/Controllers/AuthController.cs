using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("controller")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHandler _tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            _userRepository = userRepository;
            _tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //Validate with fluent validator
            //check if the user is authenticated
            //check username password
            var user = await _userRepository.AuthenticateAsync(loginRequest.username, loginRequest.password);
            if (user != null)
            {
                //Generate a jwt token
                var userToken = await _tokenHandler.CreateTokenAsync(user);
                return Ok(userToken);
            }

            return BadRequest("Usernam or password is inccorect"); 
        }
    }
}

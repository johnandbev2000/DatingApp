using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;



namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // Not required when have [apiController] attribute if(!ModelState.IsValid) return BadRequest(ModelState);
            // validate request
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            /*
                        // we do not have to check the model state if we have the whole class with the [APIController attribute]
                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }
                        */

            if (await _repo.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username already exists");
            }


            var userToCreate = new User()
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            //return CreatedAtRoute(); //TODO return the user
            return StatusCode(201);// later replace as CreatedATRoute
        }

        [HttpPost("login")]
        public async Task<IActionResult> Register(UserForLoginDto userForRegisterDto)
        {
            var userFromRepo = await _repo.Login(userForRegisterDto.Username.ToLower(), userForRegisterDto.Password);

            if (userFromRepo == null) return Unauthorized();

            // two claims user id and user name
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()), // user id
                new Claim(ClaimTypes.Name, userFromRepo.Username)  // username
            };

            //create key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            //secure signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            // return token to client
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });

        }

    }
}
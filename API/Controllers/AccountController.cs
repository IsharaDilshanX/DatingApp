using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController // r.click on "AccountController" n generate constructor
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService) //r.click on "DataContext" n generate field with parameter
        {
            _tokenService = tokenService;
            _context = context;
        }

        //[ApiController] derived from my "BaseApiController" automatically binds parameters(username, password) to the "Register" methord
        //if we didn't have [ApiController] attributes we will have to add another attribute to the parameter as ...Register([FromBody or From Query or ..] string username, ..)... BUT BC WE ARE USING [ApiController] WE DON'T HAVE TO DO SO!
        [HttpPost("register")] //we use this "register" method to register a new user , we use Post if we want to add/create a new resource through our api endpoint
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) //usings (Tasks,Mvc,Entities) //specify "async" whenever it's about dealing with DB, "ActionResult" which is the normal thing we gonna return from any of our controller methord, "AppUser" is the type of thing we are expecting tobe returned
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken!");

            using var hmac = new HMACSHA512(); //HMACSHA512() provides us with hashing algorythm to create password hash, "using"  garantee that asap it done with HMACSHA512() it will automatically trigger the disposal methord to reuse memory resources
            //HMACSHA512 - Initializes a new instance of the HMACSHA512 class with a randomly generated key.

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // "ComputeHash" methord returns a byte array and it takes a byte array as a parameter, bc our pssword is a string we use "Encoding.UTF8.GetBytes" methord to convert password string value to byte array, and "PasswordHash" is already declared as a byte array in out AppUser entity so the result hash value from the conversion can be directly assigned
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
        {
            var user = await _context.Users
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); //if the user exists we are going to return "true" if not exists return "false"!
        }
    }
}
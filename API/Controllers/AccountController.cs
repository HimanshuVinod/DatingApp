using System.Security.Cryptography;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using API.Interfaces;

namespace API.Controllers
{
    /// <summary>
    /// Handles user authentication operations including registration and login
    /// </summary>
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Constructor with dependency injection for database context and token service
        /// </summary>
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }   

        /// <summary>
        /// Registers a new user account
        /// </summary>
        /// <param name="registerDto">Contains username and password for registration</param>
        /// <returns>UserDto with username and JWT token</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTOs registerDto)
        {
            // Check if username already exists in database
            if(await UserExists(registerDto.UserName)) 
                return BadRequest("Username is taken Already");

            // Create HMAC-SHA512 instance for password hashing
            using var hmac = new HMACSHA512();
            
            // Create new user with hashed password
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key  // Store the key used for hashing
            };

            // Add user to database
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            // Return user DTO with JWT token
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        /// <summary>
        /// Authenticates an existing user
        /// </summary>
        /// <param name="loginDto">Contains username and password for login</param>
        /// <returns>UserDto with username and JWT token if successful</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Find user by username
            var user = await _context.User
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            // Return 401 if user not found
            if(user == null) 
                return Unauthorized("Invalid username");

            // Use stored salt to hash the provided password
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            // Compare hashed passwords byte by byte
            for(int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) 
                    return Unauthorized("Invalid password");
            }

            // Return user DTO with JWT token if authentication successful
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        /// <summary>
        /// Checks if a username already exists in the database
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <returns>True if username exists, false otherwise</returns>
        private async Task<bool> UserExists(string username)
        {
            return await _context.User.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
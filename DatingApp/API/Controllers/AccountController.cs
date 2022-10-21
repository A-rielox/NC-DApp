using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // POST: api/Account/register

        // la [ApiController] que se le pone al controller se asegura de que lo que se le pasa al los
        // controllers tenga las validaciones q yo especifico, aqui puse [Required] en Username del DTO
        // asi q aqui no puede llegar el registerDto sin Username

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) 
                return BadRequest("Username is taken.");

            using var hmac = new HMACSHA512();

            var user = new AppUser 
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // POST: api/Account/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName == loginDto.Username);

            if(user == null)
                return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        private async Task<bool> UserExists(string username)
        {
            // al ingresar un usuario se va a poner el nombre en .ToLower()
            return await _context.AppUsers.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}

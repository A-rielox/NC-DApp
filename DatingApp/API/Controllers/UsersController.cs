using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using System.Security.Claims;
using API.Extensions;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper,
            IPhotoService photoService
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // GET: api/Users
        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            //var users = await _userRepository.GetUsersAsync();
            //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            //return Ok(usersToReturn);

            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }

        // ActionResult
        // Representa el resultado de un método de acción.

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // GET: api/Users/username
        [HttpGet("{username}",Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            //var user = await _userRepository.GetUserByUsernameAsync(username);
            //var userToReturn = _mapper.Map<MemberDto>(user);
            //return userToReturn;

            var user = await _userRepository.GetMemberAsync(username);

            return user;


        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // PUT: api/Users
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // busco en el claim del token donde puse su username ( el token lo creo en el _tokenService)
            // me va a devolver el username que aparece en el token
            //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // le cree el extension method

            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(username);

            // p' pasar el memberUpdateDto a un user ( me hace el update del user con lo q viene en 
            // el memberUpdateDto )
            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Fail to update user.");
        }
        
        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        // POST: api/Users/add-photo
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            // GetUsername() es el extension method que me devuelve el username q esta en el token
            // recordar q necesito q en GetUserByUsernameAsync se haga el eagerLoading de las fotos
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
                // return _mapper.Map<PhotoDto>(photo);
                // el " new {} " es xq la ruta GetUser ocupa ese parametro para ir al user
                return CreatedAtRoute("GetUser", new { username = user.UserName } , _mapper.Map<PhotoDto>(photo));

            }

            return BadRequest("Problem adding photo.");
        }

    }
}

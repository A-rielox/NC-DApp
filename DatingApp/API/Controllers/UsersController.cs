using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using API.Extensions;
using API.Helpers;

namespace API.Controllers;

//[Authorize]
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
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
    {
        //var users = await _userRepository.GetUsersAsync();
        //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
        //return Ok(usersToReturn);

        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        userParams.CurrentUsername = user.UserName;

        if (string.IsNullOrEmpty(userParams.Gender))
        {
            userParams.Gender = user.Gender == "male" ? "female" : "male";
        }

        var users = await _userRepository.GetMembersAsync(userParams);

        Response.AddPaginationHeader(users.CurrentPage, users.PageSize, 
            users.TotalCount, users.TotalPages);

        return Ok(users);
    }

    // ActionResult
    // Representa el resultado de una llamada a un endpoint, me habilita las respuestas estandard
    // de Ok o NotFound etc.

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // GET: api/Users/username
    [HttpGet("{username}", Name = "GetUser")]
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

        if (await _userRepository.SaveAllAsync()) return NoContent();

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

        if (user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        user.Photos.Add(photo);

        if (await _userRepository.SaveAllAsync())
        {
            // return _mapper.Map<PhotoDto>(photo);
            // el " new {} " es xq la ruta GetUser ocupa ese parametro para ir al user
            return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));

        }

        return BadRequest("Problem adding photo.");
    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // PUT: api/Users/set-main-photo/{photoId}
    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        // saco el usernane del token
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo.IsMain) return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain = false;

        photo.IsMain = true;

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to set main photo.");


    }

    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    // DELETE: api/Users/delete-photo/{photoId}
    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        // saco el usernane del token
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("You can not delete your main photo.");

        // si esta en cloudinary => tiene una publicId
        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to delete the photo.");
    }
}

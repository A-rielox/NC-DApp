﻿using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    // NO es async ( no es Task ) xq solo hace update del status de la entity en entityFramework
    // p' activar el tracking
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);

    //Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto> GetMemberAsync(string username);
}

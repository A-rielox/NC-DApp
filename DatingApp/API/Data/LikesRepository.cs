using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;

        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            // para ver si ya existe un like
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            // p' buscar una de 2 listas 
            // la lista de a quienes a dado like - userId = SourceUserId
            // la lista de quienes le han dado like - userId = LikedUserId
            var users = _context.AppUsers.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            // para los q este user ha dado like
            if(predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == userId);
                users = likes.Select(like => like.LikedUser);
            }

            // los q le han dado like al user actual
            if(predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == userId);
                users = likes.Select(like => like.SourceUser);
            }

            return await users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id,
            }).ToListAsync();
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            // el user con la lista de a quienes ha dado like
            return await _context.AppUsers.Include(u => u.LikedUsers)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}

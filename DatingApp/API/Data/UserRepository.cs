using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.AppUsers.FindAsync(id);
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.AppUsers.SingleOrDefaultAsync(u => u.UserName == username);
            // SingleOrDefaultAsync si hay mas de uno retorna una excepcion
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            // asi da circular reference exception xq un user tiene photos y photos tiene user y ...
            // para evitarlo uso el DTO de member q ocupa photodto q no tiene appuser 
            //return await _context.AppUsers.Include(u => u.Photos).ToListAsync();
            return await _context.AppUsers.Include(u => u.Photos).ToListAsync();
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;

            // el de trevoir
            // el "Update()" pone el EntityState en modified y ya luego se puede guardar cambios
            //_context.Update(user);
            //await SaveAllAsync();
        }
    }
}

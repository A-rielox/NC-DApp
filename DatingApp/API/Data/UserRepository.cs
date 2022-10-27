using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.AppUsers
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            // con projectto me hace mas eficiente la busqueda en la db, ya q solo extrae las
            // props q necesito
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.AppUsers
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
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
            return await _context.AppUsers
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.UserName == username);
            // SingleOrDefaultAsync si hay mas de uno retorna una excepcion
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            // asi da circular reference exception xq un user tiene photos y photos tiene user y ...
            // para evitarlo uso el DTO de member q ocupa photodto q no tiene appuser ( EN LO Q DEVUELVO EN 
            // EL USERCONTROLLER )
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

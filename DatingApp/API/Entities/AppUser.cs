using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        // relacion one-to-many ( un user many photos )
        public ICollection<Photo> Photos { get; set; }
        // el Photo hago
        // p'q ocupe la id del AppUser como foreign-key
        // public AppUser AppUser { get; set; }
        // public int AppUserId { get; set; }
        // asi las fotos quedan ligadas a un AppUser, y cuando se borre un user se van a borrar las fotos
        // el cascade delete






        //public ICollection<UserLike> LikedByUsers { get; set; }
        //public ICollection<UserLike> LikedUsers { get; set; }

        //public ICollection<Message> MessagesSent { get; set; }
        //public ICollection<Message> MessagesReceived { get; set; }
        //public ICollection<AppUserRole> UserRoles { get; set; }

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}

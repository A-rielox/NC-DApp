namespace API.Entities
{
    public class UserLike
    {
        // CONFIGURO LA TABLA EN DataContext.cs
        public AppUser SourceUser { get; set; } // el q da el like
        public int SourceUserId { get; set; } // id del q da le like


        public AppUser LikedUser { get; set; } // al q se le da el like
        public int LikedUserId { get; set; }
    }
}
// un SourceUser puede tener varios LikedUsers
// un LikedUser puede tener varios LikedByUsers
using MinimalJwt.Models;

namespace MinimalJwt.Repositories
{
    public class UserRepository
    {
        public static List<User> Users = new()
        {
            new() { Username = "joshrich", Email = "joshrich32@gmail.com", Password = "1234@password", GivenName = "Josh", Surname = "Rich", Role = "standard" },
            new() { Username = "alex3453", Email = "alexcharles3@gmail.com", Password = "1234@password", GivenName = "Alex", Surname = "Charles", Role = "administrator" },
            new() { Username = "robert3213", Email = "robertbusiness34@gmail.com", Password = "1234@password", GivenName = "Robert", Surname = "Burton", Role = "artist-seller" },
        };
    }
}

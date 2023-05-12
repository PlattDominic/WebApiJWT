using MinimalJwt.Models;

namespace MinimalJwt.Repositories
{
    public class SongRepository
    {
        public static List<Song> Songs = new()
        {
            new() { Id = 1, Title = "My Closet is a Graveyard", Artist = "$B", Favorites = 12321, Genre = "Hip-Hop", Duration = "2:17" },
            new() { Id = 2, Title = "PrettyLeaf", Artist = "$B", Favorites = 3532, Genre = "Hip-Hop", Duration = "2:55" },
            new() { Id = 3, Title = "Grey Gods", Artist = "Ramirez", Favorites = 11221, Genre = "Hip-Hop", Duration = "3:12" },
            new() { Id = 4, Title = "Dark Light", Artist = "Night Lovell", Favorites = 10354, Genre = "Hip-Hop", Duration = "3:26" },
            new() { Id = 5, Title = "Paint it black", Artist = "The Rolling Stones", Favorites = 15321, Genre = "Rock", Duration = "3:20" }
        };
    }
}

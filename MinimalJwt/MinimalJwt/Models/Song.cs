namespace MinimalJwt.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Favorites { get; set; }
        public string Genre { get; set; }
        public string Duration { get; set; }
    }
}

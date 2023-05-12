using MinimalJwt.Models;

namespace MinimalJwt.Services
{
    public interface ISongService
    {
        public Song Create(Song song);
        public Song GetSong(int  id);
        public List<Song> GetAll();
        public Song Update(Song newSong);
        public bool Delete(int id);
    }
}

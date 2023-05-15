using MinimalJwt.Models;
using MinimalJwt.Repositories;

namespace MinimalJwt.Services
{
    public class SongService : ISongService
    {
        public Song Create(Song song)
        {
            song.Id = SongRepository.Songs.Count + 1;
            SongRepository.Songs.Add(song);

            return song;
        }
        
        public Song Get(int id)
        {
            var song = SongRepository.Songs.FirstOrDefault(o => o.Id == id);

            if (song == null) return null;

            return song;
        }

        public List<Song> GetAll()
        {
            var songs = SongRepository.Songs;

            return songs;
        }

        public Song Update(Song newSong)
        {
            var oldSong = SongRepository.Songs.FirstOrDefault(o => o.Id == newSong.Id);

            if (oldSong == null) return null;

            oldSong.Title = newSong.Title;
            oldSong.Artist = newSong.Artist;
            oldSong.Favorites = newSong.Favorites;
            oldSong.Genre = newSong.Genre;
            oldSong.Duration = newSong.Duration;

            return newSong;
        }
        
        public bool Delete(int id)
        {
            var oldSong = SongRepository.Songs.FirstOrDefault(o => o.Id == id);

            if (oldSong == null) return false;

            SongRepository.Songs.Remove(oldSong);

            return true;
        }
    }
}

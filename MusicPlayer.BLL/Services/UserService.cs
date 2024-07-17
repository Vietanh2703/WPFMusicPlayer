using MusicPlayer.DAL.Entities;
using MusicPlayer.DAL.Repositories;

namespace MusicPlayer.BLL.Services
{
    public class UserService
    {
        private UserRepository _repo = new();

        public UserAccount? CheckLogin(string username, string password)
        { 
            return _repo.GetAccount(username, password);

        }
    }
}

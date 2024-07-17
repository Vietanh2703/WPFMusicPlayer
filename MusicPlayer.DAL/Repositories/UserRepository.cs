using MusicPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.DAL.Repositories
{
    public class UserRepository
    {
        private MusicPlayerDbContext? _context;

        public UserAccount? GetAccount(string username, string password)
        {
            _context = new MusicPlayerDbContext();
            //return _context.UserAccounts.FirstOrDefault(delegate (UserAccount x)
            //    {
            //        if (x.Email == email && x.Password == password)
            //            return true;
            //        return false;
            //    });


            //return _context.UserAccounts.FirstOrDefault(delegate (UserAccount x)
            //{
            //    return x.Email == email && x.Password == password;
            //});

            //Lambda expression
            return _context.UserAccounts.FirstOrDefault(x => x.Username == username && x.Password == password);
        }
    }
}


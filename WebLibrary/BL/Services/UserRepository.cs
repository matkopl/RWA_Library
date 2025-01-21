using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UserRepository : IRepository<User>
    {
        private readonly WebLibraryContext _context;

        public UserRepository(WebLibraryContext context)
        {
            _context = context;
        }

        public User Create(User value)
        {
            _context.Users.Add(value);
            _context.SaveChanges();

            return value;
        }

        public User Delete(int id)
        {
            var user = Get(id);

            _context.Users.Remove(user);
            _context.SaveChanges();

            return user;
        }

        public User Edit(int id, User value)
        {
            var user = Get(id);

            user.UserName = value.UserName;
            user.Email = value.Email;
            user.Phone = value.Phone;
            user.FirstName = value.FirstName;
            user.LastName = value.LastName;

            _context.SaveChanges();

            return user;
        }

        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }
    }
}

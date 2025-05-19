using Pokemon.Data;
using Pokemon.Models;

namespace Pokemon.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetOrCreateUserAsync(string nickname, string plan = "basic")
        {
            var user = _context.Users.FirstOrDefault(u => u.Nickname == nickname);
            if (user != null) return user;

            user = new User { Nickname = nickname, Plan = plan };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public User? GetUser(string nickname)
        {
            return _context.Users.FirstOrDefault(u => u.Nickname == nickname);
        }
    }
}
